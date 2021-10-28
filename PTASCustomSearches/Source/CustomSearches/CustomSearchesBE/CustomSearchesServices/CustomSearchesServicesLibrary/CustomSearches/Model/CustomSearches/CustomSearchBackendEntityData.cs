namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;

    /// <summary>
    /// Model for the data of the custom search backend entity.
    /// </summary>
    public class CustomSearchBackendEntityData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchBackendEntityData"/> class.
        /// </summary>
        public CustomSearchBackendEntityData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSearchBackendEntityData"/> class.
        /// </summary>
        /// <param name="backendEntity">The backend entity from EF.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        public CustomSearchBackendEntityData(
            CustomSearchBackendEntity backendEntity,
            ModelInitializationType initializationType)
        {
            this.CustomSearchDefinitionId = backendEntity.CustomSearchDefinitionId;
            this.BackendEntityName = backendEntity.BackendEntityName;
            this.CustomSearchKeyFieldName = backendEntity.CustomSearchKeyFieldName;
            this.BackendEntityKeyFieldName = backendEntity.BackendEntityKeyFieldName;

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
            }

            if (initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
            }
        }

        /// <summary>
        /// Gets or sets the custom search definition id.
        /// </summary>
        public int CustomSearchDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets the backend entity name.
        /// </summary>
        public string BackendEntityName { get; set; }

        /// <summary>
        /// Gets or sets the backend entity key field name.
        /// </summary>
        public string BackendEntityKeyFieldName { get; set; }

        /// <summary>
        /// Gets or sets the custom search key field name.
        /// </summary>
        public string CustomSearchKeyFieldName { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public CustomSearchBackendEntity ToEfModel()
        {
            var toReturn = new CustomSearchBackendEntity()
            {
                CustomSearchDefinitionId = this.CustomSearchDefinitionId,
                BackendEntityName = this.BackendEntityName,
                BackendEntityKeyFieldName = this.BackendEntityKeyFieldName,
                CustomSearchKeyFieldName = this.CustomSearchKeyFieldName
            };

            return toReturn;
        }
    }
}
