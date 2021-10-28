namespace CustomSearchesServicesLibrary.CustomSearches.Model.RScript
{
    /// <summary>
    /// Model for RScript result definition.
    /// </summary>
    public class RScriptResultDefinitionData
    {
        /// <summary>
        /// Gets or sets the name of the result definition.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets the description of the result definition.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the result definition.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///  Gets or sets the default value of the result definition.
        /// </summary>
        public string DefaultValue { get; set; }
    }
}
