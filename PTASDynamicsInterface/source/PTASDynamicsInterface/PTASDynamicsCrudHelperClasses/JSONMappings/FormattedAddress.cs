// <copyright file="FormattedAddress.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represent an address formatted and validated by a geocoding service.
    /// </summary>
    public class FormattedAddress
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedAddress"/> class.
        /// </summary>
        public FormattedAddress()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedAddress"/> class.
        /// </summary>
        /// <param name="feature">Mapbox json feature object to automatically populated the FormattedAddress.</param>
        public FormattedAddress(Newtonsoft.Json.Linq.JObject feature)
        {
            if (feature != null)
            {
                if (feature.ContainsKey("relevance"))
                {
                    this.Relevance = feature["relevance"].Value<float>();
                }

                if (feature.ContainsKey("text"))
                {
                    this.StreetName = feature["text"].Value<string>();
                }

                if (feature.ContainsKey("place_name"))
                {
                    this.FormatedAddress = feature["place_name"].Value<string>();
                }

                if (feature.ContainsKey("address"))
                {
                    this.Address = feature["address"].Value<string>();
                }

                if (feature.ContainsKey("context"))
                {
                    var context = feature["context"].Value<JArray>();
                    foreach (JObject item in context)
                    {
                        if (item["id"].Value<string>().StartsWith("neighborhood"))
                        {
                            this.Neighborhood = item["text"].Value<string>();
                        }
                        else if (item["id"].Value<string>().StartsWith("postcode"))
                        {
                            this.Zip = item["text"].Value<string>();
                        }
                        else if (item["id"].Value<string>().StartsWith("place"))
                        {
                            this.City = item["text"].Value<string>();
                        }
                        else if (item["id"].Value<string>().StartsWith("region"))
                        {
                            this.State = item["text"].Value<string>();
                        }
                        else if (item["id"].Value<string>().StartsWith("country"))
                        {
                            this.Country = item["text"].Value<string>();
                        }
                    }
                }

                if (feature.ContainsKey("center"))
                {
                    var center = feature["center"].Value<JArray>();
                    this.Longitude = center[0].Value<float>();
                    this.Latitude = center[1].Value<float>();
                }
            }
        }

        /// <summary>
        /// Gets or sets the relevance or exactitud percentage : 0.8 can be considered a good match.
        /// </summary>
        [JsonProperty("relevance")]
        public float Relevance { get; set; }

        /// <summary>
        /// Gets or sets a the country name.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets a the Address line or street number (address).
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets a the street name (text).
        /// </summary>
        [JsonProperty("streetname")]
        public string StreetName { get; set; }

        /// <summary>
        /// Gets or sets a the full formatted address (place_name).
        /// </summary>
        [JsonProperty("formattedaddr")]
        public string FormatedAddress { get; set; }

        /// <summary>
        /// Gets or sets a the full neighborhood (neighborhood).
        /// </summary>
        [JsonProperty("neighborhood")]
        public string Neighborhood { get; set; }

        /// <summary>
        /// Gets or sets the city (place).
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the city (region).
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the city (postcode).
        /// </summary>
        [JsonProperty("zip")]
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        [JsonProperty("laitude")]
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        [JsonProperty("longitude")]
        public float Longitude { get; set; }
    }
}
