// <copyright file="ShortNullableDateConverter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    /// <summary>
    /// Json converter to transform DateTime properties to Edm.Date for odata Date only values.
    /// </summary>
    public class ShortNullableDateConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime?);
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DateTime? date = !string.IsNullOrWhiteSpace((string)reader.Value) ? (DateTime?)DateTime.ParseExact((string)reader.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture) : null;
            return date;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime? d = (DateTime?)value;
            writer.WriteValue(d.HasValue ? d.Value.ToString("yyyy-MM-dd") : null);
        }
    }
}
