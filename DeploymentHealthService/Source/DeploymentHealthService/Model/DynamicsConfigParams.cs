namespace DeploymentHealthService.Model
{
    /// <summary>
    /// Class that pulls the app params from a configuration object.
    /// </summary>
    public class DynamicsConfigParams : IDynamicsConfigurationParams
    {
        private const string ApiRouteValue = "/api/data/v9.1/";

        /// <inheritdoc/>
        public string ClientId { get; set; }

        /// <inheritdoc/>
        public string ClientSecret { get; set; }

        /// <inheritdoc/>
        public string ApiRoute { get; set; }

        /// <inheritdoc/>
        public string CRMUri { get; set; }

        /// <inheritdoc/>
        public string AuthUri { get; set; }

        /// <inheritdoc/>
        public string MBToken { get; set; }

        /// <inheritdoc/>
        public string MapboxUri { get; set; }
    }
}
