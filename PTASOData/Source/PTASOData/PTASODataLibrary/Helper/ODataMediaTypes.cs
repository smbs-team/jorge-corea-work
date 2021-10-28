namespace PTASODataLibrary.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.AspNet.OData.Formatter;

    /// <summary>
    /// Contains media types used by the OData formatter.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class ODataMediaTypes
    {
        /// <summary>
        /// The application json.
        /// </summary>
        internal static readonly string ApplicationJson = "application/json";

        /// <summary>
        /// The application json o data full metadata.
        /// </summary>
        internal static readonly string ApplicationJsonODataFullMetadata = "application/json;odata.metadata=full";

        /// <summary>
        /// The application json o data full metadata streaming false.
        /// </summary>
        internal static readonly string ApplicationJsonODataFullMetadataStreamingFalse = "application/json;odata.metadata=full;odata.streaming=false";

        /// <summary>
        /// The application json o data full metadata streaming true.
        /// </summary>
        internal static readonly string ApplicationJsonODataFullMetadataStreamingTrue = "application/json;odata.metadata=full;odata.streaming=true";

        /// <summary>
        /// The application json o data minimal metadata.
        /// </summary>
        internal static readonly string ApplicationJsonODataMinimalMetadata = "application/json;odata.metadata=minimal";

        /// <summary>
        /// The application json o data minimal metadata streaming false.
        /// </summary>
        internal static readonly string ApplicationJsonODataMinimalMetadataStreamingFalse = "application/json;odata.metadata=minimal;odata.streaming=false";

        /// <summary>
        /// The application json o data minimal metadata streaming true.
        /// </summary>
        internal static readonly string ApplicationJsonODataMinimalMetadataStreamingTrue = "application/json;odata.metadata=minimal;odata.streaming=true";

        /// <summary>
        /// The application json o data no metadata.
        /// </summary>
        internal static readonly string ApplicationJsonODataNoMetadata = "application/json;odata.metadata=none";

        /// <summary>
        /// The application json o data no metadata streaming false.
        /// </summary>
        internal static readonly string ApplicationJsonODataNoMetadataStreamingFalse = "application/json;odata.metadata=none;odata.streaming=false";

        /// <summary>
        /// The application json o data no metadata streaming true.
        /// </summary>
        internal static readonly string ApplicationJsonODataNoMetadataStreamingTrue = "application/json;odata.metadata=none;odata.streaming=true";

        /// <summary>
        /// The application json streaming false.
        /// </summary>
        internal static readonly string ApplicationJsonStreamingFalse = "application/json;odata.streaming=false";

        /// <summary>
        /// The application json streaming true.
        /// </summary>
        internal static readonly string ApplicationJsonStreamingTrue = "application/json;odata.streaming=true";

        /// <summary>
        /// The application XML.
        /// </summary>
        internal static readonly string ApplicationXml = "application/xml";

        /// <summary>
        /// Gets the metadata level for a media type and request parameters.
        /// </summary>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The metadata level.</returns>
        internal static ODataMetadataLevel GetMetadataLevel(string mediaType, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (mediaType == null)
            {
                return ODataMetadataLevel.MinimalMetadata;
            }

            if (!string.Equals(ODataMediaTypes.ApplicationJson, mediaType, StringComparison.Ordinal))
            {
                return ODataMetadataLevel.MinimalMetadata;
            }

            Contract.Assert(parameters != null);
            KeyValuePair<string, string> odataParameter =
                parameters.FirstOrDefault(
                    (p) => string.Equals("odata.metadata", p.Key, StringComparison.OrdinalIgnoreCase));

            if (!odataParameter.Equals(default(KeyValuePair<string, string>)))
            {
                if (string.Equals("full", odataParameter.Value, StringComparison.OrdinalIgnoreCase))
                {
                    return ODataMetadataLevel.FullMetadata;
                }

                if (string.Equals("none", odataParameter.Value, StringComparison.OrdinalIgnoreCase))
                {
                    return ODataMetadataLevel.NoMetadata;
                }
            }

            // Minimal is the default metadata level
            return ODataMetadataLevel.MinimalMetadata;
        }
    }
}
