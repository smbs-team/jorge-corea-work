// <copyright file="CurrencyValidationConverter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Json converter to tranform any currency value from a negative value to a minimun of 0.
    /// </summary>
    public class CurrencyValidationConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(double?);
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            double? money = !string.IsNullOrWhiteSpace((string)reader.Value) ? (double?)double.Parse((string)reader.Value) : null;
            return money;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Convert any negative double to 0.
            double? d = (double?)value;
            d = d.HasValue ? d.Value < 0 ? (double?)0D : (double?)d.Value : null;
            writer.WriteValue(d);
        }
    }
}
