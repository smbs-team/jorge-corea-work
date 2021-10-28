namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomImporters
{
    using Newtonsoft.Json;

    /// <summary>
    /// Model for the data of the schedule adjustment extensions.
    /// </summary>
    public class ScheduleAdjustmentExtensionsData
    {
        /// <summary>
        /// Gets or sets the object type code.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string ObjectTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the charateristic type.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string CharacteristicType { get; set; }

        /// <summary>
        /// Gets or sets the characteristic type id.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Ptas_CharacteristicType { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the maximum adjustment money.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string MaxAdjMoney { get; set; }

        /// <summary>
        /// Gets or sets the maximum adjustment percentaje.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string MaxAdjPercentaje { get; set; }

        /// <summary>
        /// Gets or sets the minimum adjustment money.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string MinAdjMoney { get; set; }

        /// <summary>
        /// Gets or sets the minimum adjustment percentaje.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string MinAdjPercentaje { get; set; }

        /// <summary>
        /// Gets or sets the adjustment value.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the value method calculation.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Ptas_ValueMethodCalculation { get; set; }

        /// <summary>
        /// Gets or sets the view type.
        /// </summary>
        public string ViewType { get; set; }

        /// <summary>
        /// Gets or sets the view type id.
        /// </summary>
        public int Ptas_ViewType { get; set; }

        /// <summary>
        /// Gets or sets the view quality.
        /// </summary>
        public string Quality { get; set; }

        /// <summary>
        /// Gets or sets the view quality id.
        /// </summary>
        public int Ptas_Quality { get; set; }

        /// <summary>
        /// Gets or sets the nuisance type.
        /// </summary>
        public string NuisanceType { get; set; }

        /// <summary>
        /// Gets or sets the nuisance type id.
        /// </summary>
        public int Ptas_NuisanceType { get; set; }

        /// <summary>
        /// Gets or sets the nuisance airport noise level.
        /// </summary>
        public string NoiseLevel { get; set; }

        /// <summary>
        /// Gets or sets the nuisance airport noise level id.
        /// </summary>
        public int Ptas_NoiseLevel { get; set; }

        /// <summary>
        /// Gets a value indicating whether this is a view characteristic type.
        /// </summary>
        /// <returns>True if this is a view characteristic type; otherwise, false.</returns>
        public bool IsViewType()
        {
            return this.CharacteristicType.ToLower() == "view";
        }

        /// <summary>
        /// Gets a value indicating whether this is a nuisance characteristic type.
        /// </summary>
        /// <returns>True if this is a nuisance characteristic type; otherwise, false.</returns>
        public bool IsNuisanceType()
        {
            return this.CharacteristicType.ToLower() == "nuisance";
        }

        /// <summary>
        /// Gets a value indicating whether this is an airport noise nuisance type.
        /// </summary>
        /// <returns>True if this is an airport noise nuisance type; otherwise, false.</returns>
        public bool IsAirportNoiseType()
        {
            return this.NuisanceType.ToLower() == "airport noise";
        }

        /// <summary>
        /// Gets the characteristic subtype id.
        /// </summary>
        /// <returns>The characteristic subtype id.</returns>
        public int GetCharacteristicSubtypeId()
        {
            if (this.IsViewType())
            {
                return this.Ptas_ViewType;
            }

            return this.Ptas_NuisanceType;
        }

        /// <summary>
        /// Gets the characteristic subtype.
        /// </summary>
        /// <returns>The characteristic subtype.</returns>
        public string GetCharacteristicSubtype()
        {
            if (this.IsViewType())
            {
                return this.ViewType;
            }

            return this.NuisanceType;
        }

        /// <summary>
        /// Gets the adjustment attribute name.
        /// </summary>
        /// <returns>The adjustment attribute name.</returns>
        public string GetAdjustmentAttributeName()
        {
            if (this.IsViewType())
            {
                return nameof(this.Quality);
            }
            else if (this.IsAirportNoiseType())
            {
                return nameof(this.NoiseLevel);
            }

            return null;
        }

        /// <summary>
        /// Gets the adjustment attribute id.
        /// </summary>
        /// <returns>The adjustment attribute id.</returns>
        public int? GetAdjustmentAttributeId()
        {
            if (this.CharacteristicType.ToLower() == "view")
            {
                return this.Ptas_Quality;
            }
            else if (this.NuisanceType.ToLower() == "airport noise")
            {
                return this.Ptas_NoiseLevel;
            }

            return null;
        }

        /// <summary>
        /// Gets the adjustment attribute value.
        /// </summary>
        /// <returns>The adjustment attribute value.</returns>
        public string GetAdjustmentAttribute()
        {
            if (this.CharacteristicType.ToLower() == "view")
            {
                return this.Quality;
            }
            else if (this.NuisanceType.ToLower() == "airport noise")
            {
                return this.NoiseLevel;
            }

            return null;
        }
    }
}
