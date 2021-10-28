namespace MediaInfo
{
    using System;

    /// <summary>
    /// Custome exception for media url getter.
    /// </summary>
    public class PostMediaException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostMediaException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public PostMediaException(string message, Exception innerException)
          : base(message, innerException)
        {
        }
    }
}
