namespace CustomSearchesServicesLibrary.ServiceFramework
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The base for services.
    /// </summary>
    public class BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public BaseService(IServiceContext serviceContext)
        {
            this.ServiceContext = serviceContext;
        }

        /// <summary>
        /// Gets or sets the security principal.
        /// </summary>
        public IServiceContext ServiceContext { get; set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public ILogger Logger
        {
            get
            {
                if (this.ServiceContext != null)
                {
                    return this.ServiceContext.Logger;
                }

                return null;
            }
        }
    }
}
