namespace CustomSearchesFunctions.Shared.Functions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Extensions.SignalRService;

    /// <summary>
    /// Serverless hub that manages the worker connection with the Azure SignalR service.
    /// </summary>
    public class WorkerHub : ServerlessHub
    {
        /// <summary>
        /// Gets the SignalR connection information for the worker hub.
        /// </summary>
        /// <group>Hub</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/WorkerHub/Negotiate</url>
        /// <param name="req">The req.</param>
        /// <param name="connectionInfo">The connection information.</param>
        /// <returns>
        /// The SignalR connection information.
        /// </returns>
        /// <response code="200" cref="SignalRConnectionInfo">The request succeeded.</response>
        [FunctionName("GetWorkerSignalRInfo")]
        public SignalRConnectionInfo GetWorkerSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/WorkerHub/Negotiate")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "WorkerHub")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
    }
}
