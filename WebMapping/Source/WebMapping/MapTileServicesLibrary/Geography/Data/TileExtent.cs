namespace PTASMapTileServicesLibrary.Geography.Data
{
    /// <summary>
    /// Describes an spatial extent.
    /// </summary>
    public class TileExtent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TileExtent"/> class.
        /// </summary>
        /// <param name="minX">The minimum x in EPSG4326.</param>
        /// <param name="minY">The minimum y in EPSG4326.</param>
        /// <param name="maxX">The maximum x in EPSG4326.</param>
        /// <param name="maxY">The maximum y in EPSG4326.</param>
        /// <param name="z">The zoom level.</param>
        public TileExtent(int minX, int minY, int maxX, int maxY, int z)
        {
            this.Min = new TileLocation(minX, minY, z);
            this.Max = new TileLocation(maxX, maxY, z);
            this.Sort();
        }

        /// <summary>
        /// Gets or sets the min location of the extent.
        /// </summary>
        public TileLocation Min { get; set; }

        /// <summary>
        /// Gets or sets the max location of the extent.
        /// </summary>
        public TileLocation Max { get; set; }

        /// <summary>
        /// Sorts the min/max values.
        /// </summary>
        public void Sort()
        {
            if (this.Min.X > this.Max.X)
            {
                int tmp = this.Min.X;
                this.Min.X = this.Max.X;
                this.Max.X = tmp;
            }

            if (this.Min.Y > this.Max.Y)
            {
                int tmp = this.Min.Y;
                this.Min.Y = this.Max.Y;
                this.Max.Y = tmp;
            }
        }
    }
}
