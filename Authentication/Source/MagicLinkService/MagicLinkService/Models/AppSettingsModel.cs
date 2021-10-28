//-----------------------------------------------------------------------
// <copyright file="AppSettingsModel.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTAS.MagicLinkService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Application Settings Model.
    /// </summary>
    public class AppSettingsModel
    {
        /// <summary>
        /// Gets or sets the B2CTenant setting.
        /// </summary>
        public string B2CTenant { get; set; }

        /// <summary>
        /// Gets or sets the B2CPolicy setting.
        /// </summary>
        public string B2CPolicy { get; set; }

        /// <summary>
        /// Gets or sets the B2CClientId setting.
        /// </summary>
        public string B2CClientId { get; set; }

        /// <summary>
        /// Gets or sets the B2CRedirectUri setting.
        /// </summary>
        public string B2CRedirectUri { get; set; }

        /// <summary>
        /// Gets or sets the B2CSignUpUrl setting.
        /// </summary>
        public string B2CSignUpUrl { get; set; }

        /// <summary>
        /// Gets or sets the Scope for the app registration.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the SigningCertAlgorithm setting.
        /// </summary>
        public string SigningCertAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the SigningCertThumbprint setting.
        /// </summary>
        public string SigningCertThumbprint { get; set; }

        /// <summary>
        /// Gets or sets the SMTPFromAddress setting.
        /// </summary>
        public string SMTPFromAddress { get; set; }

        /// <summary>
        /// Gets or sets the fromDisplayName setting.
        /// </summary>
        public string FromDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the SMTPSubject setting.
        /// </summary>
        public string SMTPSubject { get; set; }

        /// <summary>
        /// Gets or sets the LinkExpiresAfterMinutesc setting.
        /// </summary>
        public int LinkExpiresAfterMinutes { get; set; }

        /// <summary>
        /// Gets or sets the ApimEndpoint setting.
        /// </summary>
        public string ApimEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the ApimSubscriptionKey setting.
        /// </summary>
        public string ApimSubscriptionKey { get; set; }

        /// <summary>
        /// Gets or sets the NotificatioServiceResourceId setting.
        /// </summary>
        public string NotificatioServiceResourceId { get; set; }

        /// <summary>
        /// Gets or sets the UndeliverableEmailRecipient setting.
        /// </summary>
        public string UndeliverableEmailRecipient { get; set; }

        /// <summary>
        /// Gets or sets the MagicLinkServiceClientId setting.
        /// </summary>
        public string MagicLinkServiceClientId { get; set; }

        /// <summary>
        /// Gets or sets the MagicLinkServiceClientSecret setting.
        /// </summary>
        public string MagicLinkServiceClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the QrTokenSigningKey setting.
        /// </summary>
        public string QrTokenSigningKey { get; set; }

        /// <summary>
        /// Gets or sets the QrTokenDurationInSeconds setting.
        /// </summary>
        public string QrTokenDurationInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the KCTenantId setting.
        /// </summary>
        public string KCTenantId { get; set; }

        /// <summary>
        /// Gets or sets the KicTenant setting.
        /// </summary>
        public string KCTenant { get; set; }

        /// <summary>
        /// Gets or sets the PTASAppRegistrationId setting.
        /// </summary>
        public string PTASAppRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the StorageConnectionString setting.
        /// </summary>
        public string StorageConnectionString { get; set; }
    }
}
