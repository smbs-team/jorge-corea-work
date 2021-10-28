namespace CustomSearchesServicesLibrary.CustomSearches.Model
{
    using CustomSearchesServicesLibrary.Model;

    /// <summary>
    /// Model for the response of the DeleteDatasetFolder service.
    /// </summary>
    public class DeleteDatasetFolderResponse
    {
        /// <summary>
        /// Gets or sets the delete entity errors.
        /// </summary>
        public DeleteEntityErrorData[] DeleteEntityErrors { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}
