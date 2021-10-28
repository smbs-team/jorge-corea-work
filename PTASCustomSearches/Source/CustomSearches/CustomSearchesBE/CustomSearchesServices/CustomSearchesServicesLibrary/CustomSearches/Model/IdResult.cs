namespace CustomSearchesServicesLibrary.CustomSearches.Model
{
    /// <summary>
    /// Model for results that returns an id.
    /// </summary>
    public class IdResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdResult"/> class.
        /// </summary>
        /// <param name="newId">The new identifier.</param>
        public IdResult(object newId)
        {
            this.Id = newId;
        }

        /// <summary>
        /// Gets or sets the id of the result.
        /// </summary>
        public object Id { get; set; }
    }
}
