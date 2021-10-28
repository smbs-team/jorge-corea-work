namespace PTASDynamicsCrudCore.Classes
{
    using System;

    /// <summary>
    /// Result for searches.
    /// </summary>
    public class SketchSearchResult
    {
        /// <summary>
        /// Gets or sets relatedEntityId.
        /// </summary>
        public Guid RelatedEntityId { get; set; }

        /// <summary>
        /// Gets or sets relatedEntityType.
        /// </summary>
        public string RelatedEntityType { get; set; }

        /// <summary>
        /// Gets or sets parcelId.
        /// </summary>
        public string ParcelId { get; set; }

        /// <summary>
        /// Gets or sets parcelName.
        /// </summary>
        public string ParcelName { get; set; }

        /// <summary>
        /// Gets or sets address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets sketchId.
        /// </summary>
        public string SketchId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isOfficial.
        /// </summary>
        public bool IsOfficial { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isDraft.
        /// </summary>
        public bool IsDraft { get; set; }

        /// <summary>
        /// Gets or sets imageUrl.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets svg.
        /// </summary>
        public string Svg { get; set; }
    }
}