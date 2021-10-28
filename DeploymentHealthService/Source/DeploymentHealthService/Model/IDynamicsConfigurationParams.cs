namespace DeploymentHealthService.Model
{
    /// <summary>
    /// Generic configuration params provider.
    /// </summary>
    public interface IDynamicsConfigurationParams
    {
        /// <summary>
        /// Gets or sets a client ID for the Dynamics connection.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        string ClientId { get; set; }

        /// <summary>
        /// Gets or sets a client secret for the Dynamics connection.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the route to the api to connect to.
        /// </summary>
        string ApiRoute { get; set; }

        /// <summary>
        /// Gets or sets the URL of the CRM to connect to.
        /// </summary>
        string CRMUri { get; set; }

        /// <summary>
        /// Gets or sets the URI to connect to to authorize requests.
        /// </summary>
        string AuthUri { get; set; }

        /// <summary>
        /// Gets or sets the token for the access to map=geocoding places api service.
        /// </summary>
        string MBToken { get; set; }

        /// <summary>
        /// Gets or sets the token for the access to map=geocoding places api service.
        /// </summary>
        string MapboxUri { get; set; }
    }
}
