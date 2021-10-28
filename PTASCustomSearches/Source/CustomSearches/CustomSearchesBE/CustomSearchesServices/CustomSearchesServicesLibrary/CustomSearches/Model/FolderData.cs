namespace CustomSearchesServicesLibrary.CustomSearches.Model
{
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;

    /// <summary>
    /// Model for the data of the folder.
    /// </summary>
    public class FolderData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderData"/> class.
        /// </summary>
        public FolderData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderData" /> class.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        public FolderData(Folder folder, ModelInitializationType initializationType)
        {
            this.FolderName = folder.FolderName;
            this.FolderId = folder.FolderId;
            this.ParentFolderId = folder.ParentFolderId;

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                int childrenCount = folder.InverseParentFolder.Count;
                if (childrenCount > 0)
                {
                    this.Children = new FolderData[folder.InverseParentFolder.Count];

                    int index = 0;
                    foreach (var child in folder.InverseParentFolder)
                    {
                        FolderData childData = new FolderData(child, initializationType);
                        this.Children[index] = childData;
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the if of the folder.
        /// </summary>
        public int FolderId { get; set; }

        /// <summary>
        /// Gets or sets the if of the parent folder.
        /// </summary>
        public int? ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        public virtual FolderData[] Children { get; set; }
    }
}
