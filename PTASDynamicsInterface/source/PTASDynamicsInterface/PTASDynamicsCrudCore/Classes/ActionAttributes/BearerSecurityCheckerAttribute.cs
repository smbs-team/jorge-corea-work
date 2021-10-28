////// <copyright file="BearerSecurityCheckerAttribute.cs" company="King County">
//////  Copyright (c) King County. All rights reserved.
////// </copyright>

////namespace PTASIlinxService.Classes.ActionAttributes
////{
////    using System;
////    using System.Linq;
////    using System.Net.Http;
////    using System.Threading;
////    using System.Threading.Tasks;
////    using System.Web.Http.Controllers;
////    using System.Web.Http.Filters;
////    using Microsoft.Extensions.DependencyInjection;
////    using PTASServicesCommon.CloudStorage;

////    /// <summary>
////    /// Checks that the bearer token matches a string.
////    /// </summary>
////    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
////    public class BearerSecurityCheckerAttribute : AuthorizationFilterAttribute
////    {
////        private readonly string paramName;
////        private readonly bool ignoreWhenNotGuid;

////        /// <summary>
////        /// Initializes a new instance of the <see cref="BearerSecurityCheckerAttribute"/> class.
////        /// </summary>
////        /// <param name="paramName">Name of the action parameter to check.</param>
////        /// <param name="ignoreWhenNotGuid">Ignore when not a guid.</param>
////        public BearerSecurityCheckerAttribute(string paramName, bool ignoreWhenNotGuid = false)
////        {
////            this.paramName = paramName;
////            this.ignoreWhenNotGuid = ignoreWhenNotGuid;
////        }

////        /// <summary>
////        /// Happens when needs authorization.
////        /// </summary>
////        /// <param name="actionContext">Action Context.</param>
////        /// <param name="cancellationToken">Cancellation Token.</param>
////        /// <returns>Task.</returns>
////        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
////        {

////            Microsoft.Extensions.DependencyInjection            var config = AutofacConfig.Container.Resolve<IConfigParams>();
////            var cloudStorage = AutofacConfig.Container.Resolve<ICloudStorageProvider>();
////            var table = cloudStorage.GetTableRef("ptascontacts");
////            await table.CreateIfNotExistsAsync();
////            var toCheck = actionContext.Request
////                    .GetQueryNameValuePairs()
////                    .Where(x => x.Key == this.paramName)
////                    .Select(x => x.Value)
////                    .FirstOrDefault();
////            if (this.ignoreWhenNotGuid)
////            {
////                int length = Guid.NewGuid().ToString().Length;
////                var gx = toCheck.Substring(0, Math.Min(toCheck.Length, length));
////                if (!Guid.TryParse(gx, out Guid _))
////                {
////                    return;
////                }
////            }

////            await SecurityChecker.CheckSecurityAsync(toCheck, table, config.DynamicsApiURL);

////            await base.OnAuthorizationAsync(actionContext, cancellationToken);
////        }
////    }
////}