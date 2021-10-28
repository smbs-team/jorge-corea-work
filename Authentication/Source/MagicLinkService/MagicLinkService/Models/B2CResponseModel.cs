//-----------------------------------------------------------------------
// <copyright file="B2CResponseModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTAS.MagicLinkService.Models
{
    using System.Net;
    using System.Reflection;

    /// <summary>
    /// B2C Response Model.
    /// </summary>
    public class B2CResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="B2CResponseModel"/> class.
        /// </summary>
        public B2CResponseModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="B2CResponseModel"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="status">The status.</param>
        public B2CResponseModel(string message, HttpStatusCode status)
        {
            this.UserMessage = message;
            this.Status = (int)status;
            this.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the user message.
        /// </summary>
        public string UserMessage { get; set; }
    }
}
