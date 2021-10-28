// <copyright file="ConfigParams.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASLinxConnectorHelperClasses.Models
{
    /// <summary>
    /// Configuration params for this service.
    /// </summary>
    public class ConfigParams : IConfigParams
    {
        /// <inheritdoc/>
        public string SubscriptionKey { get; set; }

        /// <inheritdoc/>
        public string UserName { get; set; }

        /// <inheritdoc/>
        public string Password { get; set; }

        /// <inheritdoc/>
        public string ActivationId { get; set; }

        /// <inheritdoc/>
        public string ApplicationName { get; set; }

        /// <inheritdoc/>
        public string EdmsSoapServicesEndpoint { get; set; }

        /// <inheritdoc/>
        public string BlobStorageConnectionString { get; set; }

        /// <inheritdoc/>
        public string PremiumBlobStorageConnectionString { get; set; }

        /// <inheritdoc/>
        public string SqlServerConnectionString { get; set; }

        /// <inheritdoc/>
        public string BlobStorageContainer { get; set; }

        /// <inheritdoc/>
        public string FinalizerUrl { get; set; }

        /// <inheritdoc/>
        public string CognitiveEndPoint { get; set; }

        /// <inheritdoc/>
        public string CognitiveSubscriptionKey { get; set; }

        /// <inheritdoc/>
        public string DocuSignImpersonatedUserId { get; set; }

        /// <inheritdoc/>
        public string DocuSignAccountId { get; set; }

        /// <inheritdoc/>
        public string DocuSignApiUrl { get; set; }

        /// <inheritdoc/>
        public string DocuSignAuthServer { get; set; }

        /// <inheritdoc/>
        public string DocusignPrivateKey { get; set; }

        /// <inheritdoc/>
        public string DocuSignIntegratorId { get; set; }

        /// <inheritdoc/>
        public string DynamicsApiURL { get; set; }

        /// <inheritdoc/>
        public string SketchContainer { get; set; }

        /// <inheritdoc/>
        public bool LoadDocusignHtmlFromBlob { get; set; }

        /// <inheritdoc/>
        public string JSONContainerName { get; set; }

        /// <inheritdoc/>
        public string ProcessedJSONContainerName { get; set; }

        /// <inheritdoc/>
        public string SautinLicense { get; set; }

        /// <inheritdoc/>
        public string SharepointApiURL { get; set; }
    }
}