namespace CustomSearchesServicesLibrary.Gis.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Model for the data of the folder type.
    /// </summary>
    public class FolderTypeData
    {
        /// <summary>
        /// Gets or sets the folder type.
        /// </summary>
        public string FolderType { get; set; }

        /// <summary>
        /// Gets or sets the folder collection.
        /// </summary>
        public virtual ICollection<FolderData> Folders { get; set; }
    }
}
