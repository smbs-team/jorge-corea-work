namespace CustomSearchesServicesLibrary.CustomSearches
{
    using Newtonsoft.Json;

    /// <summary>
    /// Helper for common json operations.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string SerializeObject(object value)
        {
            if (value != null)
            {
                return JsonConvert.SerializeObject(value);
            }

            return null;
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T DeserializeObject<T>(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }

        /// <summary>
        /// Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object DeserializeObject(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return JsonConvert.DeserializeObject(value);
            }

            return null;
        }
    }
}
