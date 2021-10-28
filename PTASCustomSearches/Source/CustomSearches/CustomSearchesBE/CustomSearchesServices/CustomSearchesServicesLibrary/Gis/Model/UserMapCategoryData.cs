namespace CustomSearchesServicesLibrary.Gis.Model
{
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;

    /// <summary>
    /// Model for the data of the user map category.
    /// </summary>
    public class UserMapCategoryData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapCategoryData" /> class.
        /// </summary>
        public UserMapCategoryData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapCategoryData" /> class.
        /// </summary>
        /// <param name="userMapCategory">The map renderer category.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        public UserMapCategoryData(UserMapCategory userMapCategory, ModelInitializationType initializationType)
        {
            this.UserMapCategoryId = userMapCategory.UserMapCategoryId;
            this.CategoryName = userMapCategory.CategoryName;
            this.CategoryDescription = userMapCategory.CategoryDescription;

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the user map category.
        /// </summary>
        public int UserMapCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the category description.
        /// </summary>
        public string CategoryDescription { get; set; }
    }
}
