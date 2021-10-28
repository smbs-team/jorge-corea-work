// <copyright file="CacheControlMiddleware.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Owin;

    using Owin;

    /// <summary>
    /// CacheControlMiddleware.
    /// </summary>
    public static class CacheControlMiddleware
    {
        /// <summary>
        /// Middleware to configure http request.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="next">Next function.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static Task Middleware(IOwinContext context, Func<Task> next)
        {
            if (context.Request.Method == "GET")
            {
                context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            }

            return next();
        }

        /// <summary>
        /// Prevents web responses from being cached.
        /// </summary>
        /// <param name="app">Current app builder.</param>
        public static void PreventResponseCaching(this IAppBuilder app)
        {
            app.Use(Middleware);
        }
    }
}