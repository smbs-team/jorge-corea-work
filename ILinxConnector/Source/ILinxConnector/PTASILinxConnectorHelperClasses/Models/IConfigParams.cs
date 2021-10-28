// <copyright file="IConfigParams.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASLinxConnectorHelperClasses.Models
{
    /// <summary>
    /// Required parameters for app to connect to soap service.
    /// </summary>
    public interface IConfigParams
    {
        /// <summary>
        /// Gets or sets activation Id for connection.
        /// </summary>
        string ActivationId { get; set; }

        /// <summary>
        /// Gets or sets application name also used for container name.
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets uRL of the connection end point.
        /// </summary>
        string EdmsSoapServicesEndpoint { get; set; }

        /// <summary>
        /// Gets or sets password for the service.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets key for the subscription.
        /// </summary>
        string SubscriptionKey { get; set; }

        /// <summary>
        /// Gets or sets username for security.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Gets or sets the blob storage connection string.
        /// </summary>
        string BlobStorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the premium blob storage connection string.
        /// </summary>
        string PremiumBlobStorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the premium blob storage connection string.
        /// </summary>
        string SqlServerConnectionString { get; set; }

        /// <summary>
        /// Gets or sets container name for the blob storage.
        /// </summary>
        string BlobStorageContainer { get; set; }

        /// <summary>
        /// Gets or sets url to the finalizer service to finish off the blob move.
        /// </summary>
        string FinalizerUrl { get; set; }

        /// <summary>
        /// Gets or sets url to the json saving service to finish off the blob move.
        /// </summary>
        string DynamicsApiURL { get; set; }

        /// <summary>
        /// Gets or sets endpoint for MS cognitive services.
        /// </summary>
        string CognitiveEndPoint { get; set; }

        /// <summary>
        /// Gets or sets subscription key for MS cognitive services.
        /// </summary>
        string CognitiveSubscriptionKey { get; set; }

        /// <summary>
        /// Gets or sets impersonated User Id.
        /// </summary>
        string DocuSignImpersonatedUserId { get; set; }

        /// <summary>
        /// Gets or sets account Id for DocuSign.
        /// </summary>
        string DocuSignAccountId { get; set; }

        /// <summary>
        ///  Gets api url to docusign.
        /// </summary>
        string DocuSignApiUrl { get; }

        /// <summary>
        ///  Gets or sets authorization Server for docusign.
        /// </summary>
        string DocuSignAuthServer { get; set; }

        /// <summary>
        ///  Gets or sets the RSA private key for docusign.
        /// </summary>
        string DocusignPrivateKey { get; set; }

        /// <summary>
        ///  Gets or sets the RSA private key for docusign.
        /// </summary>
        string DocuSignIntegratorId { get; set; }

        /// <summary>
        /// Gets or sets container for XML to JSON sketches.
        /// </summary>
        string SketchContainer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether load from blob or file system.
        /// </summary>
        bool LoadDocusignHtmlFromBlob { get; set; }

        /// <summary>
        /// Gets or sets container name for json snippets.
        /// </summary>
        string JSONContainerName { get; set; }

        /// <summary>
        /// Gets or sets container name for processed json snippets.
        /// </summary>
        string ProcessedJSONContainerName { get; set; }

        /// <summary>
        /// Gets or sets licence number for sautinsoft.
        /// </summary>
        string SautinLicense { get; set; }

        /// <summary>
        /// Gets or sets sharepoint Api URL.
        /// </summary>
        string SharepointApiURL { get; set; }
    }
}