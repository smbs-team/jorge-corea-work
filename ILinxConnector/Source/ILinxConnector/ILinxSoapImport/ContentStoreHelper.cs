// <copyright file="ContentStoreHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace ILinxSoapImport
{
    using System;
    using System.ServiceModel;
    using ILinxSoapImport.EdmsService;
    using ILinxSoapImport.Exceptions;
    using PTASLinxConnectorHelperClasses.Models;
    using Serilog;

    /// <summary>
    /// Helper class to connect to the ILinx content store.
    /// </summary>
    public class ContentStoreHelper : IContentStoreHelper
    {
        private readonly IConfigParams config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentStoreHelper"/> class.
        /// </summary>
        /// <param name="config">Injected configuration parameters.</param>
        public ContentStoreHelper(IConfigParams config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Initialize and retrieve a content store client.
        /// </summary>
        /// <returns>Newly created content store.</returns>
        /// <remarks>Must dispose and close.</remarks>
        public ContentStoreContractClient GetContentStoreClient()
        {
            // Create endpoint address gathered from functions app settings
            var address = new EndpointAddress(this.config.EdmsSoapServicesEndpoint);

            // Create binding to match the client proxy configuration
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.MessageEncoding = WSMessageEncoding.Mtom;

            // Create an instance of our client proxy passing in binding and endpoint address
            ContentStoreContractClient client = new ContentStoreContractClient(binding, address);

            // Add client credentials from functions app settings
            client.ClientCredentials.UserName.UserName = this.config.UserName;
            client.ClientCredentials.UserName.Password = this.config.Password;

            return client;
        }

        /// <summary>
        /// Retrieve security token for further operations.
        /// </summary>
        /// <returns>The token encoded as a string.</returns>
        public string GetSecurityToken()
        {
            User response = null;
            using (ContentStoreContractClient client = this.GetContentStoreClient())
            {
                try
                {
                    response = client.Login2(this.config.UserName, this.config.Password, this.config.ActivationId, this.config.ApplicationName);
                    client.Close();
                }
                catch (ApplicationException)
                {
                    if (client != null)
                    {
                        client.Abort();
                    }

                    throw;
                }
            }

            if (response == null)
            {
                throw new IlinxLoginFailedException();
            }

            return response.SecurityToken;
        }
    }
}
