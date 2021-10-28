namespace CustomSearchesServicesLibrary.Gis.Model
{
    using System;
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using Newtonsoft.Json;

    /// <summary>
    /// Model for the data of the map renderer.
    /// </summary>
    public class MapRendererData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapRendererData"/> class.
        /// </summary>
        public MapRendererData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRendererData" /> class.
        /// </summary>
        /// <param name="mapRenderer">The folder.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        /// <param name="userDetails">The user details.</param>
        public MapRendererData(MapRenderer mapRenderer, ModelInitializationType initializationType, Dictionary<Guid, UserInfoData> userDetails)
        {
            this.CreatedBy = mapRenderer.CreatedBy;
            this.CreatedTimestamp = mapRenderer.CreatedTimestamp;
            this.UserMapId = mapRenderer.UserMapId;
            this.DatasetId = mapRenderer.DatasetId;
            this.LayerSourceId = mapRenderer.LayerSourceId;
            this.LastModifiedBy = mapRenderer.LastModifiedBy;
            this.LastModifiedTimestamp = mapRenderer.LastModifiedTimestamp;
            this.MapRendererId = mapRenderer.MapRendererId;
            this.MapRendererName = mapRenderer.MapRendererName;
            this.MapRendererType = mapRenderer.MapRendererType;
            this.HasLabelRenderer = mapRenderer.HasLabelRenderer;
            this.RendererRules = JsonHelper.DeserializeObject(mapRenderer.RendererRules);

            UserDetailsHelper.GatherUserDetails(mapRenderer.CreatedByNavigation, userDetails);
            UserDetailsHelper.GatherUserDetails(mapRenderer.LastModifiedByNavigation, userDetails);

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
            }
        }

        /// <summary>
        /// Gets or sets the map renderer id.
        /// </summary>
        public int MapRendererId { get; set; }

        /// <summary>
        /// Gets or sets the map renderer name.
        /// </summary>
        public string MapRendererName { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who created this map renderer.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who last modified this map renderer.
        /// </summary>
        public Guid LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets dataset id.
        /// </summary>
        public Guid? DatasetId { get; set; }

        /// <summary>
        /// Gets or sets the map renderer type.
        /// </summary>
        public string MapRendererType { get; set; }

        /// <summary>
        /// Gets or sets the renderer rules.
        /// </summary>
        public object RendererRules { get; set; }

        /// <summary>
        /// Gets or sets the created timestamp.
        /// </summary>
        public DateTime CreatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the last modified timestamp.
        /// </summary>
        public DateTime LastModifiedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether map renderer has label renderer.
        /// </summary>
        public bool HasLabelRenderer { get; set; }

        /// <summary>
        /// Gets or sets the custom search name.
        /// </summary>
        public string CustomSearchName { get; set; }

        /// <summary>
        /// Gets or sets the user map id.
        /// </summary>
        public int UserMapId { get; set; }

        /// <summary>
        /// Gets or sets the layer source id.
        /// </summary>
        public int LayerSourceId { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public MapRenderer ToEfModel()
        {
            var toReturn = new MapRenderer()
            {
                CreatedBy = this.CreatedBy,
                CreatedTimestamp = this.CreatedTimestamp,
                UserMapId = this.UserMapId,
                DatasetId = this.DatasetId,
                LastModifiedBy = this.LastModifiedBy,
                LastModifiedTimestamp = this.LastModifiedTimestamp,
                MapRendererLogicType = "Label",
                MapRendererName = this.MapRendererName,
                MapRendererType = this.MapRendererType,
                RendererRules = JsonHelper.SerializeObject(this.RendererRules),
                HasLabelRenderer = this.HasLabelRenderer,
                LayerSourceId = this.LayerSourceId
            };

            return toReturn;
        }
    }
}
