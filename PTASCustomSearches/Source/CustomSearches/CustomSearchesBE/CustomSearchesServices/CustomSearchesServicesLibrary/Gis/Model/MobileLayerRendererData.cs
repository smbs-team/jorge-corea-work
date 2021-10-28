namespace CustomSearchesServicesLibrary.Gis.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using DocumentFormat.OpenXml.Office2010.Drawing;
    using Newtonsoft.Json;

    /// <summary>
    /// Model for register MobileLayerRenders.
    /// </summary>
    public class MobileLayerRendererData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileLayerRendererData"/> class.
        /// </summary>
        public MobileLayerRendererData()
        {
            this.MobileLayerRendererId = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileLayerRendererData" /> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public MobileLayerRendererData(MobileLayerRenderer entity)
        {
            this.MobileLayerRendererId = entity.MobileLayerRendererId;
            this.LayerSourceId = entity.LayerSourceId;
            this.RendererRules = JsonHelper.DeserializeObject(entity.RendererRules);
            this.Query = entity.Query;
            this.Name = entity.Name;
            this.Role = entity.Role;
            this.Categories = entity.Categories;
            this.CreatedTimestamp = entity.CreatedTimestamp;
            this.LastModifiedTimestamp = entity.LastModifiedTimestamp;
        }

        /// <summary>
        /// Gets or sets the mobile layer renderer id.
        /// </summary>
        public Guid MobileLayerRendererId { get; set; }

        /// <summary>
        /// Gets or sets the layer source id.
        /// </summary>
        public int LayerSourceId { get; set; }

        /// <summary>
        /// Gets or sets the renderer rules.
        /// </summary>
        public object RendererRules { get; set; }

        /// <summary>
        /// Gets or sets the mobile db query used as source for the renderer.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the display name for the renderer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the created timestamp.
        /// </summary>
        public DateTime CreatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the last modified timestamp.
        /// </summary>
        public DateTime LastModifiedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets if the renderer is the selected one by default.
        /// </summary
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the Categories.
        /// </summary
        public string Categories { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public MobileLayerRenderer ToEfModel()
        {
            var toReturn = new MobileLayerRenderer()
            {
                MobileLayerRendererId = this.MobileLayerRendererId,
                CreatedTimestamp = this.CreatedTimestamp,
                LastModifiedTimestamp = this.LastModifiedTimestamp,
                Role = this.Role,
                Categories = this.Categories,
                Name = this.Name,
                Query = this.Query,
                RendererRules = JsonHelper.SerializeObject(this.RendererRules),
                IsSelected = this.IsSelected,
                LayerSourceId = this.LayerSourceId
            };

            return toReturn;
        }

        /// <summary>
        /// Update the <see cref="MobileLayerRenderer"/> recieved as parameter with the data in this instance.
        /// </summary>
        /// <param name="mobileLayerRenderer">the <see cref="MobileLayerRenderer"/> to be updated.</param>
        public void UpdateEFModel(MobileLayerRenderer mobileLayerRenderer)
        {
            mobileLayerRenderer.LastModifiedTimestamp = DateTime.UtcNow;
            mobileLayerRenderer.Role = this.Role;
            mobileLayerRenderer.Categories = this.Categories;
            mobileLayerRenderer.Name = this.Name;
            mobileLayerRenderer.Query = this.Query;
            mobileLayerRenderer.RendererRules = JsonHelper.SerializeObject(this.RendererRules);
            mobileLayerRenderer.IsSelected = this.IsSelected;
            mobileLayerRenderer.LayerSourceId = this.LayerSourceId;
        }
    }
}
