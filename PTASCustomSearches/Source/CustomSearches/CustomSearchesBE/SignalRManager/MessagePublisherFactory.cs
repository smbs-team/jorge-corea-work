namespace SignalRManager
{
    using System.Threading.Tasks;

    /// <summary>
    /// The message publisher factory.
    /// </summary>
    public class MessagePublisherFactory : IMessagePublisherFactory
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublisherFactory" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MessagePublisherFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Creates the message publisher.
        /// </summary>
        /// <param name="hubName">The hub name.</param>
        /// <returns>The message publisher.</returns>
        public async Task<IMessagePublisher> CreateAsync(string hubName)
        {
            var messagePublisher = new MessagePublisher(this.connectionString, hubName);
            await messagePublisher.InitAsync();
            return messagePublisher;
        }
    }
}
