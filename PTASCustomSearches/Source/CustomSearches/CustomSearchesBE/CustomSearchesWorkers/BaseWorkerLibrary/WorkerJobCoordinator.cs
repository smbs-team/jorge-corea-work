// <copyright file="WorkerJobCoordinator.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace CustomSearchesWorkerLibrary.RScript
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BaseWorkerLibrary.SqlServer.Model;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Worker job coordinator class.
    /// </summary>
    public static class WorkerJobCoordinator
    {
        /// <summary>
        /// Gets or sets the time threshold. Initial value is -1 that means no threshold.
        /// </summary>
        public static int TimeThreshold { get; set; } = -1;

        /// <summary>
        /// Gets or sets a value indicating whether a value indicating whether the worker should be closed.
        /// </summary>
        public static bool ShouldClose { get; set; } = false;

        /// <summary>
        /// Gets or sets count of consecutive lost access to share folder.
        /// </summary>
        public static int LostAccessToShareFolderCount { get; set; } = 0;

        /// <summary>
        /// Limit of consecutive lost access to share folder.
        /// </summary>
        private const int LostAccessToShareFolderLimit = 3;

        /// <summary>
        /// The time interval between exit condition probes in seconds.
        /// </summary>
        private const int ExitConditionProbeInterval = 300;

        /// <summary>
        /// The critical exceptions by expiration time.
        /// </summary>
        private static readonly Dictionary<Exception, DateTime> CriticalExceptionExpirationMap = new Dictionary<Exception, DateTime>();

        /// <summary>
        /// Value indicating whether the critical exceptions are exceeded the limit.
        /// </summary>
        private static bool criticalExceptionsExceeded = false;

        /// <summary>
        /// The exit condition probe time.
        /// </summary>
        private static DateTime exitConditionProbeTime = DateTime.UtcNow;

        /// <summary>
        /// The critical exceptions time limit in seconds.
        /// </summary>
        private static readonly int CriticalExceptionsTimeLimit = 60;

        /// <summary>
        /// The critical exceptions count limit.
        /// </summary>
        private static readonly int CriticalExceptionsCountLimit = 10;

        /// <summary>
        /// The expiration time of the worker.
        /// </summary>
        private static readonly DateTime WorkerExpirationTime;

        /// <summary>
        /// The thread keep alive threshold in seconds.
        /// </summary>
        private static readonly int ThreadKeepAliveThreshold = 600;

        /// <summary>
        /// The data of the queue threads.
        /// </summary>
        private static readonly Dictionary<int, ThreadData> QueueThreadDictionary = new Dictionary<int, ThreadData>();

        /// <summary>
        /// Gets or sets a value indicating whether the coordinator is allowed to exit the application.
        /// </summary>
        public static bool AllowApplicationExit { get; set; }

        /// <summary>
        /// The job keep alive threshold in seconds.
        /// </summary>
        public readonly static int JobKeepAliveThreshold = 180;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerJobCoordinator"/> class.
        /// </summary>
        static WorkerJobCoordinator()
        {
            Random random = new Random();
            int expectedWorkerLife = (int)(24 * (0.75 + random.NextDouble() * 0.5));
            WorkerExpirationTime = DateTime.UtcNow.Add(new TimeSpan(hours: expectedWorkerLife, 0, 0));
            AllowApplicationExit = true;
        }

        /// <summary>
        /// Registers a queue thread. Every time a queue thread is created should be registered.
        /// </summary>
        /// <param name="queueThreadId">The queue thread id.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="logger">The logger.</param>
        public static void RegisterQueueThread(int queueThreadId, string queueName, ILogger logger)
        {
            ThreadData threadData = new ThreadData(queueThreadId, queueName);
            bool addedThread = false;

            lock (WorkerJobCoordinator.QueueThreadDictionary)
            {
                if (!WorkerJobCoordinator.QueueThreadDictionary.ContainsKey(queueThreadId))
                {
                    WorkerJobCoordinator.QueueThreadDictionary.Add(queueThreadId, threadData);
                    addedThread = true;
                }
            }

            if (addedThread)
            {
                logger.LogInformation($"WorkerJobCoordinator: Added thread '{queueThreadId}' ({queueName}).");
            }
        }

        /// <summary>
        /// Registers a worker job. Every time a job is started should be registered.
        /// </summary>
        /// <param name="queueThreadId">The queue thread id.</param>
        /// <param name="jobId">The worker job id.</param>
        /// <param name="jobTimeoutInSeconds">The worker job timeout in seconds.</param>
        /// <param name="logger">The logger.</param>
        public static void RegisterJob(int queueThreadId, int jobId, int jobTimeoutInSeconds, ILogger logger)
        {
            ThreadData threadData = null;
            lock (WorkerJobCoordinator.QueueThreadDictionary)
            {
                if (!WorkerJobCoordinator.QueueThreadDictionary.ContainsKey(queueThreadId))
                {
                    return;
                }

                threadData = WorkerJobCoordinator.QueueThreadDictionary[queueThreadId];
                threadData.JobExpirationTime = DateTime.UtcNow.AddSeconds(jobTimeoutInSeconds);
                threadData.JobKeepAliveTimestamp = DateTime.UtcNow;
                threadData.JobId = jobId;
            }

            logger.LogInformation($"WorkerJobCoordinator: Added job '{jobId}' to thread '{queueThreadId}' ({threadData.QueueName}).");
        }

        /// <summary>
        /// Unregisters a worker job. Every time a job is finished should be unregistered.
        /// </summary>
        /// <param name="queueThreadId">The queue thread id.</param>
        /// <param name="logger">The logger.</param>
        public static void UnregisterJob(int queueThreadId, ILogger logger)
        {
            int jobId = 0;
            ThreadData threadData = null;
            lock (WorkerJobCoordinator.QueueThreadDictionary)
            {
                if (!WorkerJobCoordinator.QueueThreadDictionary.ContainsKey(queueThreadId))
                {
                    return;
                }

                threadData = WorkerJobCoordinator.QueueThreadDictionary[queueThreadId];
                jobId = threadData.JobId;
                if (jobId > 0)
                {
                    threadData.JobId = 0;
                }
            }

            if (jobId > 0)
            {
                logger.LogInformation($"WorkerJobCoordinator: Removed job '{jobId}' from thread '{queueThreadId}' ({threadData.QueueName}).");
            }
        }

        /// <summary>
        /// Checks it the thread is healthy.
        /// </summary>
        /// <param name="queueThreadId">The queue thread id.</param>
        /// <returns>True if thread is healthy.</returns>
        public static bool IsThreadHealthy(int queueThreadId)
        {
            lock (WorkerJobCoordinator.QueueThreadDictionary)
            {
                return !WorkerJobCoordinator.QueueThreadDictionary[queueThreadId].IsUnhealthy;
            }
        }

        /// <summary>
        /// Updates the thread keep alive time.
        /// </summary>
        /// <param name="queueThreadId">The queue thread id.</param>
        public static void KeepAliveThread(int queueThreadId)
        {
            lock (WorkerJobCoordinator.QueueThreadDictionary)
            {
                if (!WorkerJobCoordinator.QueueThreadDictionary.ContainsKey(queueThreadId))
                {
                    return;
                }

                ThreadData threadData = WorkerJobCoordinator.QueueThreadDictionary[queueThreadId];
                threadData.ThreadKeepAliveTimestamp = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Updates the job keep alive time.
        /// </summary>
        /// <param name="queueThreadId">The queue thread id.</param>
        public static void KeepAliveJob(int queueThreadId)
        {
            lock (WorkerJobCoordinator.QueueThreadDictionary)
            {
                if (!WorkerJobCoordinator.QueueThreadDictionary.ContainsKey(queueThreadId))
                {
                    return;
                }

                ThreadData threadData = WorkerJobCoordinator.QueueThreadDictionary[queueThreadId];
                threadData.JobKeepAliveTimestamp = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Reports a critical exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="logger">The logger.</param>
        public static void ReportCriticalException(Exception exception, ILogger logger)
        {
            lock (WorkerJobCoordinator.CriticalExceptionExpirationMap)
            {
                // Remove expired exceptions
                Exception[] keys = WorkerJobCoordinator.CriticalExceptionExpirationMap.Keys.ToArray();
                foreach (var key in keys)
                {
                    if (DateTime.UtcNow > WorkerJobCoordinator.CriticalExceptionExpirationMap[key])
                    {
                        WorkerJobCoordinator.CriticalExceptionExpirationMap.Remove(key);
                    }
                }

                // Add new reported exception
                if (!WorkerJobCoordinator.CriticalExceptionExpirationMap.ContainsKey(exception))
                {
                    WorkerJobCoordinator.CriticalExceptionExpirationMap.Add(exception, DateTime.UtcNow.AddSeconds(WorkerJobCoordinator.CriticalExceptionsTimeLimit));
                }

                // Store unexpired exception count
                WorkerJobCoordinator.criticalExceptionsExceeded = WorkerJobCoordinator.CriticalExceptionExpirationMap.Count > WorkerJobCoordinator.CriticalExceptionsCountLimit;
            }

            logger.LogInformation($"WorkerJobCoordinator: Added critical exception '{exception.GetBaseException().Message}'.");
        }

        /// <summary>
        /// Coordinate the worker job threads.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public static void Coordinate(ILogger logger)
        {
            List<ThreadData> threadDataList;
            lock (WorkerJobCoordinator.QueueThreadDictionary)
            {
                threadDataList = WorkerJobCoordinator.QueueThreadDictionary.Values.ToList();
            }

            DateTime maxJobExpiration = DateTime.UtcNow;
            foreach (var threadData in threadDataList)
            {
                if (threadData.JobExpirationTime > maxJobExpiration)
                {
                    maxJobExpiration = threadData.JobExpirationTime;
                }

                WorkerJobCoordinator.UpdateThreadHealthStatus(threadData, logger);
            }

            if (AllowApplicationExit && (DateTime.UtcNow > WorkerJobCoordinator.WorkerExpirationTime))
            {
                int newTimeThreshold = Math.Max((int)(maxJobExpiration - DateTime.UtcNow).TotalSeconds, 0);

                if (WorkerJobCoordinator.TimeThreshold == -1)
                {
                    logger.LogInformation($"WorkerJobCoordinator: Worker has expired, the new time threshold is {newTimeThreshold}");
                }

                WorkerJobCoordinator.TimeThreshold = newTimeThreshold;
            }

            if ((WorkerJobCoordinator.ShouldClose == false) &&
                (threadDataList.Where(d => d.JobId != 0 && !d.IsUnhealthy).Count() == 0))
            {
                bool workerTimeThresholdExceeded = WorkerJobCoordinator.TimeThreshold == 0;
                bool isThreadUnhealthy = threadDataList.FirstOrDefault(d => d.IsUnhealthy) != null;

                if (AllowApplicationExit &&
                    (WorkerJobCoordinator.criticalExceptionsExceeded ||
                    workerTimeThresholdExceeded ||
                    isThreadUnhealthy ||
                    WorkerJobCoordinator.LostAccessToShareFolderCount >= WorkerJobCoordinator.LostAccessToShareFolderLimit))
                {
                    WorkerJobCoordinator.ShouldClose = true;
                    logger.LogInformation($"WorkerJobCoordinator: Worker should close, additional information:" +
                        $" Exceptions exceeded: '{WorkerJobCoordinator.criticalExceptionsExceeded}'," +
                        $" Time threshold: '{WorkerJobCoordinator.TimeThreshold}'," +
                        $" Lost access to share folder count: '{WorkerJobCoordinator.LostAccessToShareFolderCount}'," +
                        $" Is worker unhealthy: '{isThreadUnhealthy}'.");
                }
            }

            WorkerJobCoordinator.ProbeExitCondition(threadDataList, logger);
        }

        /// <summary>
        /// Updates the thread health status.
        /// </summary>
        /// <param name="threadData">The thread data.</param>
        /// <param name="logger">The logger.</param>
        private static void UpdateThreadHealthStatus(ThreadData threadData, ILogger logger)
        {
            if (!threadData.IsUnhealthy)
            {
                DateTime now = DateTime.UtcNow;
                if (now > threadData.ThreadKeepAliveTimestamp.AddSeconds(WorkerJobCoordinator.ThreadKeepAliveThreshold))
                {
                    threadData.UnhealthyReason = nameof(WorkerJobCoordinator.ThreadKeepAliveThreshold);
                }
                else if (threadData.JobId != 0)
                {
                    if (now > threadData.JobExpirationTime)
                    {
                        threadData.UnhealthyReason = nameof(threadData.JobExpirationTime);
                    }
                    else if (now > threadData.JobKeepAliveTimestamp.AddSeconds(WorkerJobCoordinator.JobKeepAliveThreshold))
                    {
                        threadData.UnhealthyReason = nameof(WorkerJobCoordinator.JobKeepAliveThreshold);
                    }
                }

                if (!string.IsNullOrWhiteSpace(threadData.UnhealthyReason))
                {
                    threadData.IsUnhealthy = true;
                    logger.LogInformation($"WorkerJobCoordinator: Thread '{threadData.ThreadId}' ({threadData.QueueName}) is unhealthy" +
                        $" due to {threadData.UnhealthyReason}. ");
                }
            }
        }

        /// <summary>
        /// Probes the exit condition.
        /// </summary>
        /// <param name="threadDataList">The thread data list.</param>
        /// <param name="logger">The logger.</param>
        private static void ProbeExitCondition(List<ThreadData> threadDataList, ILogger logger)
        {
            if (WorkerJobCoordinator.exitConditionProbeTime > DateTime.UtcNow)
            {
                return;
            }

            WorkerJobCoordinator.exitConditionProbeTime = DateTime.UtcNow.AddSeconds(WorkerJobCoordinator.ExitConditionProbeInterval);

            if (DateTime.UtcNow > WorkerJobCoordinator.WorkerExpirationTime ||
                WorkerJobCoordinator.criticalExceptionsExceeded ||
                WorkerJobCoordinator.LostAccessToShareFolderCount >= WorkerJobCoordinator.LostAccessToShareFolderLimit ||
                threadDataList.FirstOrDefault(d => d.IsUnhealthy) != null)
            {
                string threadsInfo = string.Join(".\n", threadDataList.Select(
                    t => $"IsUnhealthy: {t.IsUnhealthy}. QueueName: {t.QueueName}. ThreadId: {t.ThreadId}. UnhealthyReason: {t.UnhealthyReason}"));

                logger.LogInformation($"WorkerJobCoordinator: Probes to worker exit condition:\n" +
                    $"AllowApplicationExit: '{WorkerJobCoordinator.AllowApplicationExit}'.\n" +
                    $"Exceptions exceeded: '{WorkerJobCoordinator.criticalExceptionsExceeded}'.\n" +
                    $"Time threshold: '{WorkerJobCoordinator.TimeThreshold}'.\n" +
                    $"Lost access to share folder count: '{WorkerJobCoordinator.LostAccessToShareFolderCount}'.\n" +
                    $"All threads info:\n" +
                    $"{threadsInfo}");
            }
        }
    }
}