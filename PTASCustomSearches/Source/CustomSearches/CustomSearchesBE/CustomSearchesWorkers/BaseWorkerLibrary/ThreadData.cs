// <copyright file="ThreadData.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace BaseWorkerLibrary.SqlServer.Model
{
    using System;

    /// <summary>
    /// Entity for thread data.
    /// </summary>
    public class ThreadData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadData" /> class.
        /// </summary>
        /// <param name="threadId">The thread id.</param>
        /// <param name="queueName">The queue name.</param>
        public ThreadData(int threadId, string queueName)
        {
            this.ThreadId = threadId;
            this.QueueName = queueName;
            this.ThreadKeepAliveTimestamp = DateTime.UtcNow;
            this.JobKeepAliveTimestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets the thread id.
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// Gets or sets the queue name.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the job id.
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the thread keep alive timestamp.
        /// </summary>
        public DateTime ThreadKeepAliveTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the job keep alive timestamp.
        /// </summary>
        public DateTime JobKeepAliveTimestamp { get; set; }

        /// <summary>
        /// Gets or sets job expiration time.
        /// </summary>
        public DateTime JobExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the thread is unhealthy.
        /// </summary>
        public bool IsUnhealthy { get; set; }

        /// <summary>
        /// Gets or sets the unhealthy reason.
        /// </summary>
        public string UnhealthyReason { get; set; }
    }
}