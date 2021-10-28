namespace PTASMapTileServicesLibrary.TileProvider.Blob
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class that provides configuration information for the blob tile provider.
    /// </summary>
    public class BlobTileConfigurationProvider : IBlobTileConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlobTileConfigurationProvider" /> class.
        /// </summary>
        /// <param name="tileContainerName">Name of the tile container.</param>
        /// <param name="tilePathMask">The tile path mask.</param>
        /// <exception cref="ArgumentNullException">When tileContainerName/tilePathMask parameter is null.</exception>
        public BlobTileConfigurationProvider(string tileContainerName, string tilePathMask)
        {
            if (string.IsNullOrEmpty(tileContainerName))
            {
                throw new ArgumentNullException(nameof(tileContainerName));
            }

            if (string.IsNullOrEmpty(tilePathMask))
            {
                throw new ArgumentNullException(nameof(tilePathMask));
            }

            this.TilePathMask = tilePathMask;
            this.TileContainerName = tileContainerName;
        }

        /// <summary>
        /// Gets the path mask for a tile in azure.
        /// </summary>
        /// <remarks>
        /// This should be a mask containing the tile parameters:  "{0}/{1}/{2}/{3}.pbf", where {3}=x, {2}=y, {1}=z,{0}=layerId".
        /// </remarks>
        public string TilePathMask { get; private set; }

        /// <summary>
        /// Gets the path mask for a tile in blob storage.
        /// </summary>
        public string TileContainerName { get; private set; }
    }
}
