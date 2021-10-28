namespace PTASMapTileServicesLibrary.OverlapCalutionProvider
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Provides overlap data by parcel.
    /// </summary>
    public class OverlapData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OverlapData"/> class.
        /// </summary>
        public OverlapData()
        {
            this.AdditionalFields = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the Parcel PIN for mapping. It is mayor and minor without the dash.
        /// </summary>
        public string ParcelPIN { get; set; }

        /// <summary>
        /// Gets or sets the  the ID of the map layer againts the olverlap calcuation was done.
        /// </summary>
        public long LayerID { get; set; }

        /// <summary>
        /// Gets or sets the percentage of overlap over the parcel.
        /// </summary>
        public double OverlapPercentage { get; set; }

        /// <summary>
        /// Gets or sets the area of overlap over the parcel.
        /// </summary>
        public double OverlapArea { get; set; }

        /// <summary>
        /// Gets or sets the area of overlap over the parcel.
        /// </summary>
        public double ParcelArea { get; set; }

        /// <summary>
        /// Gets the list of additional data fields in the layer for the intesected data. if there is multiple records the data is concatated and separated by comma.
        /// </summary>
        public Dictionary<string, string> AdditionalFields { get; private set; }
    }
}
