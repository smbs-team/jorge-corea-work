namespace CustomSearchesServicesLibrary.Model
{
    /// <summary>
    /// Model for the delete entity error.
    /// </summary>
    public struct DeleteEntityErrorData
    {
        /// <summary>
        /// Gets or sets the id of the entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the folder path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }
    }
}
