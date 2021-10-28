namespace CustomSearchesServicesLibrary.Gis.Model
{
    /// <summary>
    /// Model for the data of the folder creation.
    /// </summary>
    public class RenameFolderData
    {
        /// <summary>
        /// Gets or sets the folder path.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the new name.
        /// </summary>
        public string NewName { get; set; }

        /// <summary>
        /// Gets or sets the folder item type.
        /// </summary>
        public string FolderItemType { get; set; }
    }
}
