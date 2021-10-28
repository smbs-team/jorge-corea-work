// <copyright file="ExceptionFilter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.Filters;

    /// <summary>
    /// Process uncaught events.
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Override system on exception handling.
        /// </summary>
        /// <param name="actionExecutedContext">Context of execution.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is HttpResponseException)
            {
                actionExecutedContext.Response = (actionExecutedContext.Exception as HttpResponseException).Response;
                return;
            }

            var responseContent = new ErrorResponse(actionExecutedContext.Exception);
            var status = HttpStatusCode.InternalServerError;
            if (actionExecutedContext.Exception is KeyNotFoundException)
            {
                status = HttpStatusCode.NotFound;
            }

            if (actionExecutedContext.Exception is System.UnauthorizedAccessException)
            {
                status = HttpStatusCode.Unauthorized;
            }

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(status, responseContent);
        }

        /// <summary>
        /// Error description class.
        /// </summary>
        public class ErrorDescription
        {
            /// <summary>
            /// Gets or sets inner exception.
            /// </summary>
            public string InnerException { get; set; }

            /// <summary>
            /// Gets or sets error Message.
            /// </summary>
            public string Message { get; set; }
        }

        /// <summary>
        /// Response on error.
        /// </summary>
        public class ErrorResponse
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
            /// </summary>
            /// <param name="ex">Exception that produced this error.</param>
            public ErrorResponse(Exception ex) =>
              this.Error = new ErrorDescription
              {
                  Message = ex.Message,
                  InnerException = this.GetInnerException(ex),
              };

            /// <summary>
            /// Gets or sets reported error.
            /// </summary>
            public ErrorDescription Error { get; set; }

            private string GetInnerException(Exception ex) => ex == null ? string.Empty : $"{ex.Message}\n{this.GetInnerException(ex.InnerException)}";
        }
    }
}