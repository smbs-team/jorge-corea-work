namespace CustomSearchesServicesLibrary.CustomSearches.Model
{
    using System;

    /// <summary>
    /// Model for the rename dataset.
    /// </summary>
    public class UpdateDatasetAttributesData
    {
        /// <summary>
        /// Gets or sets the new name.
        /// </summary>
        public string NewName { get; set; }

        /// <summary>
        /// Gets or sets the new comments.
        /// </summary>
        public string NewComments { get; set; }
    }
}
