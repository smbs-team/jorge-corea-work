namespace DeploymentHealthService.Controllers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using DeploymentHealthService.Model;
    using ILinxSoapImport;
    using ILinxSoapImport.Exceptions;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using PTASCRMHelpers;
    using PTASLinxConnectorHelperClasses.Models;

    /// <summary>
    /// Controller that retrieves environment information.
    /// </summary>
    public class EnvInfoController : ApiController
    {
        /// <summary>
        /// The configuration parameters.
        /// </summary>
        private readonly IConfigParams configParams;

        /// <summary>
        /// The configuration parameters.
        /// </summary>
        private readonly IDynamicsConfigurationParams dynamicsConfigurationParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvInfoController" /> class.
        /// </summary>
        /// <param name="configParams">The configuration parameters.</param>
        /// <param name="dynamicsConfigurationParams">The dynamics configuration parameters.</param>
        public EnvInfoController(IConfigParams configParams, IDynamicsConfigurationParams dynamicsConfigurationParams)
        {
            this.configParams = configParams;
            this.dynamicsConfigurationParams = dynamicsConfigurationParams;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>A string with the health status.</returns>
        [HttpGet]
        public EnvInfoCollection Get()
        {
            EnvInfoCollection envInfoCollection = new EnvInfoCollection();

            List<EnvInfo> infoList = new List<EnvInfo>();
            infoList.Add(new EnvInfo()
            {
                Item = "Ilinx API Endpoint",
                Info = this.configParams.EdmsSoapServicesEndpoint
            });

            infoList.Add(new EnvInfo()
            {
                Item = "Data Service API Endpoint",
                Info = this.configParams.DynamicsApiURL
            });

            infoList.Add(new EnvInfo()
            {
                Item = "Cognitive Services Endpoint",
                Info = this.configParams.CognitiveEndPoint
            });

            infoList.Add(new EnvInfo()
            {
                Item = "Dynamics CRM Endpoint",
                Info = this.dynamicsConfigurationParams.CRMUri
            });

            infoList.Add(new EnvInfo()
            {
                Item = "Dynamics Authentication Endpoint",
                Info = this.dynamicsConfigurationParams.AuthUri
            });

            infoList.Add(new EnvInfo()
            {
                Item = "Mapbox API Endpoint",
                Info = this.dynamicsConfigurationParams.MapboxUri
            });

            envInfoCollection.InfoArray = infoList.ToArray();

            return envInfoCollection;
        }
    }
}