namespace CustomSearchesServicesLibrary.Gis.Model
{
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using DocumentFormat.OpenXml.Office2010.Drawing;
    using Newtonsoft.Json;

    /// <summary>
    /// Model for the data of the layer source.
    /// </summary>
    public class LayerSourceData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayerSourceData"/> class.
        /// </summary>
        public LayerSourceData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerSourceData" /> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public LayerSourceData(LayerSource entity)
        {
            this.LayerSourceId = entity.LayerSourceId;
            this.DataSourceUrl = entity.DataSourceUrl;
            this.DbTableName = entity.DbTableName;
            this.DefaultLabelMaxZoom = entity.DefaultLabelMaxZoom;
            this.DefaultLabelMinZoom = entity.DefaultLabelMinZoom;
            this.DefaultMaxZoom = entity.DefaultMaxZoom;
            this.DefaultMinZoom = entity.DefaultMinZoom;
            this.Description = entity.Description;
            this.HasOfflineSupport = entity.HasOfflineSupport;
            this.IsParcelSource = entity.IsParcelSource;
            this.IsVectorLayer = entity.IsVectorLayer;
            this.Jurisdiction = entity.Jurisdiction;
            this.LayerSourceAlias = entity.LayerSourceAlias;
            this.LayerSourceName = entity.LayerSourceName;
            this.Organization = entity.Organization;
            this.TileSize = entity.TileSize;
            this.GisLayerName = entity.GisLayerName;
            this.ServeFromFileShare = entity.ServeFromFileShare;
            this.HasMobileSupport = entity.HasMobileSupport;
            this.HasOverlapSupport = entity.HasOverlapSupport;
            this.IsBlobPassThrough = entity.IsBlobPassThrough;

            if (!string.IsNullOrWhiteSpace(entity.DefaultLabelMapboxLayer))
            {
                this.DefaultLabelMapboxLayer = JsonHelper.DeserializeObject(entity.DefaultLabelMapboxLayer);
            }

            this.DefaultMapboxLayer = JsonHelper.DeserializeObject(entity.DefaultMapboxLayer);

            this.NativeMapboxLayers = JsonHelper.DeserializeObject(entity.NativeMapboxLayers);

            this.EmbeddedDataFields = JsonHelper.DeserializeObject(entity.EmbeddedDataFields);

            this.Metadata = JsonHelper.DeserializeObject(entity.Metadata);

            if (!string.IsNullOrWhiteSpace(entity.OgrLayerData))
            {
                this.OgrLayerData = JsonHelper.DeserializeObject<OgrLayerData[]>(entity.OgrLayerData);
            }
        }

        /// <summary>
        /// Gets or sets the layer source id.
        /// </summary>
        public int LayerSourceId { get; set; }

        /// <summary>
        /// Gets or sets the layer source name.
        /// </summary>
        public string LayerSourceName { get; set; }

        /// <summary>
        /// Gets or sets the layer source alias.
        /// </summary>
        public string LayerSourceAlias { get; set; }

        /// <summary>
        /// Gets or sets the jurisdiction.
        /// </summary>
        public string Jurisdiction { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a parcel source.
        /// </summary>
        public bool IsParcelSource { get; set; }

        /// <summary>
        /// Gets or sets the default min zoom.
        /// </summary>
        public int DefaultMinZoom { get; set; }

        /// <summary>
        /// Gets or sets the default max zoom.
        /// </summary>
        public int DefaultMaxZoom { get; set; }

        /// <summary>
        /// Gets or sets the default label min zoom.
        /// </summary>
        public int DefaultLabelMinZoom { get; set; }

        /// <summary>
        /// Gets or sets the default label max zoom.
        /// </summary>
        public int DefaultLabelMaxZoom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a vector layer.
        /// </summary>
        public bool IsVectorLayer { get; set; }

        /// <summary>
        /// Gets or sets the data source url.
        /// </summary>
        public string DataSourceUrl { get; set; }

        /// <summary>
        /// Gets or sets the size of the tile (valid if this is an image layer).
        /// </summary>
        public int TileSize { get; set; }

        /// <summary>
        /// Gets or sets the default mapbox layer.
        /// </summary>
        public object DefaultMapboxLayer { get; set; }

        /// <summary>
        /// Gets or sets the native mapbox layers.
        /// </summary>
        public object NativeMapboxLayers { get; set; }

        /// <summary>
        /// Gets or sets the default label mapbox layer.
        /// </summary>
        public object DefaultLabelMapboxLayer { get; set; }

        /// <summary>
        /// Gets or sets the embedded data fields.
        /// </summary>
        public object EmbeddedDataFields { get; set; }

        /// <summary>
        /// Gets or sets the layer file path.
        /// </summary>
        public string LayerFilePath { get; set; }

        /// <summary>
        /// Gets or sets the db table name.
        /// </summary>
        public string DbTableName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the layer source has offline support.
        /// </summary>
        public bool HasOfflineSupport { get; set; }

        /// <summary>
        /// Gets or sets the gis layer name.
        /// </summary>
        public string GisLayerName { get; set; }

        /// <summary>
        /// Gets or sets the layer metadata.
        /// </summary>
        public object Metadata { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to serve from the share or from the cache.
        /// </summary>
        public bool ServeFromFileShare { get; set; }

        /// <summary>
        /// Gets or sets the layer ogr data (used to build and query mapserver mapfile data).
        /// </summary>
        public OgrLayerData[] OgrLayerData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has mobile support.
        /// </summary>
        public bool HasMobileSupport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has overlap support.
        /// </summary>
        public bool HasOverlapSupport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is BLOB pass through.
        /// </summary>
        public bool IsBlobPassThrough { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public LayerSource ToEfModel()
        {
            var toReturn = new LayerSource();
            this.UpdateEFModel(toReturn);
            toReturn.LayerSourceId = this.LayerSourceId;
            return toReturn;
        }

        /// <summary>
        /// Updates the ef model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void UpdateEFModel(LayerSource entity)
        {
            entity.DataSourceUrl = this.DataSourceUrl;
            entity.DbTableName = this.DbTableName;
            entity.DefaultLabelMapboxLayer = JsonHelper.SerializeObject(this.DefaultLabelMapboxLayer);
            entity.DefaultLabelMaxZoom = this.DefaultLabelMaxZoom;
            entity.DefaultLabelMinZoom = this.DefaultLabelMinZoom;
            entity.DefaultMapboxLayer = JsonHelper.SerializeObject(this.DefaultMapboxLayer);
            entity.NativeMapboxLayers = JsonHelper.SerializeObject(this.NativeMapboxLayers);
            entity.DefaultMaxZoom = this.DefaultMaxZoom;
            entity.DefaultMinZoom = this.DefaultMinZoom;
            entity.Description = this.Description;
            entity.EmbeddedDataFields = JsonHelper.SerializeObject(this.EmbeddedDataFields);
            entity.HasOfflineSupport = this.HasOfflineSupport;
            entity.IsParcelSource = this.IsParcelSource;
            entity.IsVectorLayer = this.IsVectorLayer;
            entity.Jurisdiction = this.Jurisdiction;
            entity.LayerSourceAlias = this.LayerSourceAlias;
            entity.LayerSourceName = this.LayerSourceName;
            entity.Organization = this.Organization;
            entity.TileSize = this.TileSize;
            entity.GisLayerName = this.GisLayerName;
            entity.Metadata = JsonHelper.SerializeObject(this.Metadata);
            entity.OgrLayerData = JsonHelper.SerializeObject(this.OgrLayerData);
            entity.ServeFromFileShare = this.ServeFromFileShare;
            entity.HasMobileSupport = this.HasMobileSupport;
            entity.HasOverlapSupport = this.HasOverlapSupport;
            entity.IsBlobPassThrough = this.IsBlobPassThrough;
        }

        /// <summary>
        /// Validates the ogr data.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        /// True if the OgrData is valid.
        /// </returns>
        public bool ValidateOgrData(out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!this.IsBlobPassThrough)
            {
                if (this.OgrLayerData == null || this.OgrLayerData.Length == 0)
                {
                    errorMessage = "OgrLayerData not present.";
                    return false;
                }

                int index = 0;
                foreach (var layer in this.OgrLayerData)
                {
                    if (string.IsNullOrWhiteSpace(layer.LayerName))
                    {
                        errorMessage = $"LayerName not present for ogr layer at index {index}.";
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(layer.LayerConnectionPath))
                    {
                        errorMessage = $"LayerConnectionPath not present for ogr layer at index {index}.";
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(layer.OgrLayerName))
                    {
                        errorMessage = $"OgrLayerName not present for ogr layer at index {index}.";
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(layer.LayerProjection))
                    {
                        errorMessage = $"LayerProjection not present for ogr layer at index {index}.";
                        return false;
                    }

                    index++;
                }
            }

            return true;
        }
    }
}
