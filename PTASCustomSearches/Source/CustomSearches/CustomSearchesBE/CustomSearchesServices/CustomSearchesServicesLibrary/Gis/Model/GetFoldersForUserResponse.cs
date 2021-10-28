namespace CustomSearchesServicesLibrary.Gis.Model
{
    /// <summary>
    /// Model for the response of the GetFoldersForUser service.
    /// </summary>
    public class GetFoldersForUserResponse
    {
        /// <summary>
        /// Gets or sets the folder data collection.
        /// </summary>
        public FolderData[] Folders { get; set; }
    }
}
