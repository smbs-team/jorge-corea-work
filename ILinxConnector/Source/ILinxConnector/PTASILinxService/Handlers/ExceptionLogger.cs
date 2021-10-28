// <copyright file="ExceptionLogger.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.ExceptionHandling;

    using Serilog;

    /// <summary>
    /// Log exception to log class.
    /// </summary>
    public class ExceptionLogger : IExceptionLogger
    {
        /// <summary>
        /// Write to log.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="cancellationToken">Do we cancel.</param>
        /// <returns>Async task.</returns>
        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            Log.Error(context.Exception, context.ExceptionContext.Request.RequestUri.ToString());
            return Task.CompletedTask;
        }
    }
}