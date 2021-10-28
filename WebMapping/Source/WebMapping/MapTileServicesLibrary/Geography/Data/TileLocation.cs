namespace PTASMapTileServicesLibrary.Geography.Data
{
    /// <summary>
    /// Describes a geographic location.
    /// </summary>
    public class TileLocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TileLocation"/> class.
        /// </summary>
        /// <param name="x">The latitude.</param>
        /// <param name="y">The longitude.</param>
        /// <param name="z">The zoom.</param>
        public TileLocation(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TileLocation"/> class.
        /// </summary>
        public TileLocation()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the Z coordinate.
        /// </summary>
        public int Z { get; set; }
    }
}
