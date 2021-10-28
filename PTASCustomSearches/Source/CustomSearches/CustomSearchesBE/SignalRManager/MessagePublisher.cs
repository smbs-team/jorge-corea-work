namespace SignalRManager
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Azure.SignalR.Management;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Publisher that sends Azure SignalR messages.
    /// </summary>
    public class MessagePublisher : IMessagePublisher
    {
        /// <summary>
        /// The hub name.
        /// </summary>
        private readonly string hubName;

        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// The service hub context.
        /// </summary>
        private IServiceHubContext serviceHubContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublisher" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="hubName">The hub name.</param>
        public MessagePublisher(string connectionString, string hubName)
        {
            this.hubName = hubName;
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Intializes the service hub context.
        /// </summary>
        /// <returns>The result of the asynchronous operation.</returns>
        public async Task InitAsync()
        {
            var serviceManager = new ServiceManagerBuilder().WithOptions(option =>
            {
                option.ConnectionString = connectionString;
            }).Build();

            serviceHubContext = await serviceManager.CreateHubContextAsync(this.hubName, new LoggerFactory());
        }

        /// <summary>
        /// Sends an Azure SignalR message.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="method">The method name.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <returns>The result of the asynchronous operation.</returns>
        public Task SendAsync(string userId, string method, object arg1, object arg2, object arg3, object arg4)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return serviceHubContext.Clients.All.SendAsync(method, arg1, arg2, arg3, arg4);
            }
            else
            {
                return serviceHubContext.Clients.User(userId).SendAsync(method, arg1, arg2, arg3, arg4);
            }
        }

        /// <summary>
        /// Disposes the instance of the message publisher.
        /// </summary>
        /// <returns>The result of the asynchronous operation.</returns>
        public ValueTask DisposeAsync()
        {
            return new ValueTask(serviceHubContext?.DisposeAsync());
        }
    }
}