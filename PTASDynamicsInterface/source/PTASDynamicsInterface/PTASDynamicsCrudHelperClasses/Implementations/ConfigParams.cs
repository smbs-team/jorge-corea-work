// <copyright file="ConfigParams.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Implementations
{
    using Microsoft.Extensions.Configuration;

    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Class that pulls the app params from a configuration object.
    /// </summary>
    public class ConfigParams : IConfigurationParams
    {
        private const string ApiRouteValue = "/api/data/v9.1/";
        private readonly IConfiguration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigParams"/> class.
        /// </summary>
        /// <param name="config">Configuration to get the values from.</param>
        public ConfigParams(IConfiguration config)
        {
            this.config = config;
        }

        /// <inheritdoc/>
        public string ApimEndpoint => this.config["ApimEndpoint"];

        /// <inheritdoc/>
        public string ApimSubscriptionKey => this.config["ApimSubscriptionKey"];

        /// <inheritdoc/>
        public string ApiRoute => ApiRouteValue;

        /// <inheritdoc/>
        public string AuthUri => this.config["AuthUri"];

        /// <inheritdoc/>
        public string ClientId => this.config["ClientId"];

        /// <inheritdoc/>
        public string ClientSecret => this.config["ClientSecret"];

        /// <inheritdoc/>
        public string CRMUri => this.config["CRMUri"];

        /// <inheritdoc/>
        public string FromEmailAddress => this.config["FromEmailAddress"];

        /// <inheritdoc/>
        public string MagiclinkserviceClientId => this.config["ptas-dev-magiclinkservice-clientid"];

        /// <inheritdoc/>
        public string MagiclinkserviceClientSecret => this.config["ptas-dev-magiclinkservice-clientsecret"];

        /// <inheritdoc/>
        public string MapboxUri => this.config["MapboxUri"];

        /// <inheritdoc/>
        public string MBToken => this.config["MBToken"];

        /// <inheritdoc/>
        public string NotificatioServiceResourceId => this.config["NotificatioServiceResourceId"];

        /// <inheritdoc/>
        public string ParcelWhatIfUri => this.config["ParcelWhatIfUri"];

        /// <inheritdoc/>
        public string ToEmailAddress => this.config["ToEmailAddress"];

        /// <inheritdoc/>
        public string UndeliverableEmailRecipient => this.config["UndeliverableEmailRecipient"];

        /// <inheritdoc/>
        public bool IsWhatifEnvironment => "true".Equals(this.config["IsWhatifEnvironment"]);

        /// <inheritdoc/>
        public string DocumentStorageUrl => this.config["document-storage-url"];

        /// <inheritdoc/>
        public string SharepointDocsUri => this.config["SharepointDocsUri"];

        /// <inheritdoc/>
        public string SaveMediaUri => this.config["SaveMediaUri"];

        /// <inheritdoc/>
        public string SharepointSite => this.config["SharepointSite"];

        /// <inheritdoc/>
        public string SharepointDrive => this.config["SharepointDrive"];

        /// <inheritdoc/>
        public string StorageConnectionString => this.config["StorageConnectionString"];

        /// <inheritdoc/>
        public int PermitSASExpireHours => int.TryParse(this.config["PermitSASExpireHours"], out int hours) ? hours : 8;
    }
}