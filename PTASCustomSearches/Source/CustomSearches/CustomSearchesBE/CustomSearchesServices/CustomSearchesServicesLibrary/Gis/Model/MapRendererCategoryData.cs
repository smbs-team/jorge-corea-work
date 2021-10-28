namespace CustomSearchesServicesLibrary.Gis.Model
{
    using System.Collections.Generic;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;

    /// <summary>
    /// Model for the data of the map renderer category.
    /// </summary>
    public class MapRendererCategoryData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapRendererCategoryData" /> class.
        /// </summary>
        public MapRendererCategoryData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRendererCategoryData" /> class.
        /// </summary>
        /// <param name="mapRendererCategory">The map renderer category.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        public MapRendererCategoryData(MapRendererCategory mapRendererCategory, ModelInitializationType initializationType)
        {
            this.MapRendererCategoryId = mapRendererCategory.MapRendererCategoryId;
            this.CategoryName = mapRendererCategory.CategoryName;
            this.CategoryDescription = mapRendererCategory.CategoryDescription;

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                if ((mapRendererCategory.MapRendererCategoryMapRenderer != null) &&
                    (mapRendererCategory.MapRendererCategoryMapRenderer.Count > 0))
                {
                    List<MapRendererData> mapRenderers = new List<MapRendererData>();
                    foreach (var mapRendererCategoryMapRenderer in mapRendererCategory.MapRendererCategoryMapRenderer)
                    {
                        if (mapRendererCategoryMapRenderer.MapRenderer != null)
                        {
                            MapRenderer mapRenderer = mapRendererCategoryMapRenderer.MapRenderer;
                            MapRendererData mapRendererData = new MapRendererData(mapRenderer, ModelInitializationType.Summary, userDetails: null);
                            mapRenderers.Add(mapRendererData);
                        }
                    }

                    if (mapRenderers.Count > 0)
                    {
                        this.MapRenderers = mapRenderers.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the map renderer category.
        /// </summary>
        public int MapRendererCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the category description.
        /// </summary>
        public string CategoryDescription { get; set; }

        /// <summary>
        /// Gets or sets the map renderers.
        /// </summary>
        public MapRendererData[] MapRenderers { get; set; }
    }
}
