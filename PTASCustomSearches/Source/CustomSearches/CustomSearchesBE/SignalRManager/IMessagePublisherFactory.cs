namespace SignalRManager
{
    using System.Threading.Tasks;

    /// <summary>
    /// The message publisher factory interface.
    /// </summary>
    public interface IMessagePublisherFactory
    {
        /// <summary>
        /// Creates the message publisher.
        /// </summary>
        /// <param name="hubName">The hub name.</param>
        /// <returns>The message publisher.</returns>
        Task<IMessagePublisher> CreateAsync(string hubName);
    }
}