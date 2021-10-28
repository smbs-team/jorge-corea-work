// <copyright file="SignalRNotificationQueue.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace BaseWorkerLibrary
{
    using System.Collections.Generic;

    /// <summary>
    /// Class that provides an in memory queue for SignalR notifications.
    /// It is used to store notifications until sleeping workers wake up and can check them up.
    /// </summary>
    public static class SignalRNotificationQueue
    {
        /// <summary>
        /// Stores the SignalR notifications in memory.
        /// </summary>
        private static readonly HashSet<string> Notifications = new HashSet<string>();

        /// <summary>
        /// If it was, returns true and clears the notification.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <returns>Value indicating whether queue name was notified.</returns>
        public static bool WasNotified(string queueName)
        {
            lock (Notifications)
            {
                if (Notifications.Contains(queueName))
                {
                    Notifications.Remove(queueName);
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Stores the SignalR notification in memory.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        public static void Notify(string queueName)
        {
            lock (Notifications)
            {
                Notifications.Add(queueName);
            }
        }
    }
}