namespace CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches
{
    /// <summary>
    /// Model for the data of the custom search parameter value.
    /// </summary>
    public class CustomSearchParameterValueData
    {
        /// <summary>
        /// Gets or sets the identifier of the custom search parameter.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the custom search parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the custom search parameter.
        /// </summary>
        public string Value { get; set; }
    }
}
