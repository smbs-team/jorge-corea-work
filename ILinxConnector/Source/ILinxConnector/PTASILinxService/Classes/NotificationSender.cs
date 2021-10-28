// <copyright file="NotificationSender.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;

    using Newtonsoft.Json;

    using PTASLinxConnectorHelperClasses.Models;

    /// <summary>
    /// Notification sender class.
    /// </summary>
    public class NotificationSender
    {
        private readonly IConfigParams config;
        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSender"/> class.
        /// </summary>
        /// <param name="config">Sys config.</param>
        public NotificationSender(IConfigParams config)
        {
            this.config = config;
        }

        /// <summary>
        /// Send a notification.
        /// </summary>
        /// <param name="subject">Subject.</param>
        /// <param name="body">Body.</param>
        public void SendNotification(string subject, string body)
        {
            try
            {
                var url = this.config.DynamicsApiURL;
                var itemPath = Path.Combine(url, "ErrorReporting");

                var jsonStr = JsonConvert.SerializeObject(new
                {
                    MessageBody = body,
                    MessageSubject = subject,
                });
                var response = this.client.SendAsync(new HttpRequestMessage(HttpMethod.Post, itemPath)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "application/json"),
                }).Result;
                var r = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {
                // purposefully do nothing, this is an edge situation.
            }
        }
    }
}