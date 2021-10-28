// <copyright file="ShortNullableDateConverter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Json converter to transform substring any string that goes over the maximun # of 4000 characters.
    /// </summary>
    public class StringLimiterConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string fileName = (string)reader.Value;
            return fileName;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string fileName = (string)value;

            // Max # of items for SingleLine type in Dynamics CE.
            writer.WriteValue(fileName.Length > 4000 ? fileName.Substring(0, 4000) : fileName);
        }
    }
}
