// <copyright file="IntegerValidationConverter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Json converter to tranform any integer value from a negative value to a minimun of 0.
    /// </summary>
    public class IntegerValidationConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int?);
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int? money = !string.IsNullOrWhiteSpace((string)reader.Value) ? (int?)int.Parse((string)reader.Value) : null;
            return money;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Convert any negative integer to 0.
            int? d = (int?)value;
            d = d.HasValue ? d.Value < 0 ? 0 : (int?)d.Value : null;
            writer.WriteValue(d);
        }
    }
}
