namespace CustomSearchesServicesLibrary.Gis.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Model for the data of the map renderer type.
    /// </summary>
    public class MapRendererTypeData
    {
        /// <summary>
        /// Gets or sets the map renderer type.
        /// </summary>
        public string MapRendererType { get; set; }

        /// <summary>
        /// Gets or sets the map renderers.
        /// </summary>
        public virtual ICollection<MapRendererData> MapRenderers { get; set; }
    }
}
