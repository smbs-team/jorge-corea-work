// <copyright file="IConfigurationParams.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    /// <summary>
    /// Generic configuration params provider.
    /// </summary>
    public interface IConfigurationParams
    {
        /// <summary>
        /// Gets apimEndpoint.
        /// </summary>
        string ApimEndpoint { get; }

        /// <summary>
        /// Gets apimSubscriptionKey.
        /// </summary>
        string ApimSubscriptionKey { get; }

        /// <summary>
        /// Gets the route to the api to connect to.
        /// </summary>
        string ApiRoute { get; }

        /// <summary>
        /// Gets the URI to connect to to authorize requests.
        /// </summary>
        string AuthUri { get; }

        /// <summary>
        /// Gets client ID for the Dynamics connection.
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Gets client secret for the Dynamics connection.
        /// </summary>
        string ClientSecret { get; }

        /// <summary>
        /// Gets the URL of the CRM to connect to.
        /// </summary>
        string CRMUri { get; }

        /// <summary>
        /// Gets from email for error reporting.
        /// </summary>
        string FromEmailAddress { get; }

        /// <summary>
        /// Gets a value indicating whether we are in the whatif environment.
        /// </summary>
        bool IsWhatifEnvironment { get; }

        /// <summary>
        /// Gets MagiclinkserviceClientId.
        /// </summary>
        string MagiclinkserviceClientId { get; }

        /// <summary>
        /// Gets MagiclinkserviceClientSecret.
        /// </summary>
        string MagiclinkserviceClientSecret { get; }

        /// <summary>
        /// Gets the token for the access to map=geocoding places api service.
        /// </summary>
        string MapboxUri { get; }

        /// <summary>
        /// Gets the token for the access to map=geocoding places api service.
        /// </summary>
        string MBToken { get; }

        /// <summary>
        /// Gets notificatioServiceResourceId.
        /// </summary>
        string NotificatioServiceResourceId { get; }

        /// <summary>
        /// Gets get the uri to post to.
        /// </summary>
        string ParcelWhatIfUri { get; }

        /// <summary>
        /// Gets to email for error reporting.
        /// </summary>
        string ToEmailAddress { get; }

        /// <summary>
        /// Gets undeliverableEmailRecipient.
        /// </summary>
        string UndeliverableEmailRecipient { get; }

        /// <summary>
        /// Gets document storage URL.
        /// </summary>
        string DocumentStorageUrl { get; }

        /// <summary>
        /// Gets sharepoint Docs Url.
        /// </summary>
        string SharepointDocsUri { get; }

        /// <summary>
        /// Gets sharepoint Site.
        /// </summary>
        string SharepointSite { get; }

        /// <summary>
        /// Gets sharepoint Drive.
        /// </summary>
        string SharepointDrive { get; }

        /// <summary>
        /// Gets the URI to the media function.
        /// </summary>
        string SaveMediaUri { get; }

        /// <summary>
        /// Gets storage Connection String.
        /// </summary>
        string StorageConnectionString { get; }

        /// <summary>
        /// Gets how long will it take to the sas to expire in hours.
        /// </summary>
        int PermitSASExpireHours { get; }
    }
}