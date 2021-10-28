// <copyright file="BearerSecurityCheckerAttribute.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes.ActionAttributes
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using Autofac;

    using PTASIlinxService;
    using PTASIlinxService.Classes.Entities;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    /// <summary>
    /// Checks that the bearer token matches a string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class BearerSecurityCheckerAttribute : AuthorizationFilterAttribute
    {
        private const int GuidLength = 36;
        private readonly string paramName;
        private readonly bool ignoreWhenNotGuid;

        /// <summary>
        /// Initializes a new instance of the <see cref="BearerSecurityCheckerAttribute"/> class.
        /// </summary>
        /// <param name="paramName">Name of the action parameter to check.</param>
        /// <param name="ignoreWhenNotGuid">Ignore when not a guid.</param>
        public BearerSecurityCheckerAttribute(string paramName, bool ignoreWhenNotGuid = false)
        {
            this.paramName = paramName;
            this.ignoreWhenNotGuid = ignoreWhenNotGuid;
        }

        /// <summary>
        /// Happens when needs authorization.
        /// </summary>
        /// <param name="actionContext">Action Context.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Task.</returns>
        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            string bypass = GetParamValueFromContext(actionContext, "bypassBearer");
            string toCheck = GetParamValueFromContext(actionContext, this.paramName);
            bool bypassIsTrue = IsTrue(bypass);
            if (!bypassIsTrue &&
                toCheck != null &&
                (!this.ignoreWhenNotGuid || IsGuid(toCheck)))
            {
                var config = AutofacConfig.Container.Resolve<IConfigParams>();
                var cloudStorage = AutofacConfig.Container.Resolve<ICloudStorageProvider>();
                var table = cloudStorage.GetTableRef("ptascontacts");
                await table.CreateIfNotExistsAsync();
                await SecurityChecker.CheckSecurityAsync(toCheck, table, config.DynamicsApiURL);

                await base.OnAuthorizationAsync(actionContext, cancellationToken);
            }
        }

        private static bool IsTrue(string bypass)
        {
            return "true".Equals(bypass, StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool IsGuid(string toCheck) =>
            toCheck.Length >= GuidLength
                && Guid.TryParse(toCheck.Substring(0, GuidLength), out Guid _);

        private static string GetParamValueFromContext(HttpActionContext actionContext, string paramName) => actionContext.Request
                    .GetQueryNameValuePairs()
                    .Where(x => x.Key == paramName)
                    .Select(x => x.Value)
                    .FirstOrDefault();
    }
}