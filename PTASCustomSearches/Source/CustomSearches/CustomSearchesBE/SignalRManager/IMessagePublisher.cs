namespace SignalRManager
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Publisher that sends Azure SignalR messages.
    /// </summary>
    public interface IMessagePublisher : IAsyncDisposable
    {
        /// <summary>
        /// Intializes the service hub context.
        /// </summary>
        /// <returns>The result of the asynchronous operation.</returns>
        Task InitAsync();

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
        Task SendAsync(string userId, string method, object arg1, object arg2, object arg3, object arg4);
    }
}