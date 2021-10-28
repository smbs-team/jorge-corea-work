namespace PTASMapTileServicesLibrary.TileProvider
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface that defines methods to provide map tiles from different sources.
    /// </summary>
    public interface ITileProvider
    {
        /// <summary>
        /// Gets the map tile.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="layer">The layer.</param>
        /// <returns>An array of bytes with the specified tile.  NULL if the tile was not found.</returns>
        Task<byte[]> GetTile(int x, int y, int z, string layer);
    }
}
