namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Helper for parameter replacement.
    /// </summary>
    public static class ParameterReplacementHelper
    {
        /// <summary>
        /// Converts a parameter array into a string dictionary.
        /// </summary>
        /// <param name="dependedParameterValues">The depended parameter values.</param>
        /// <param name="extensionWithDefaultParameters">String containing the json extensions with the default value for parameters.</param>
        /// <returns>
        /// A dictionary with parameter values.
        /// </returns>
        public static Dictionary<string, string> ParametersAsDictionary(
            CustomSearchParameterValueData[] dependedParameterValues,
            string extensionWithDefaultParameters)
        {
            Dictionary<string, string> defaultParameters =
                ParameterReplacementHelper.GetDefaultDependedParameterValues(extensionWithDefaultParameters);

            if ((dependedParameterValues == null || dependedParameterValues.Length == 0) &&
                (defaultParameters == null || defaultParameters.Count == 0))
            {
                return null;
            }

            dependedParameterValues = dependedParameterValues ?? new CustomSearchParameterValueData[0];
            var toReturn = (from p in dependedParameterValues select p).ToDictionary(p => p.Name, p => p.Value);

            if (defaultParameters == null)
            {
                return toReturn;
            }

            foreach (var defaultParameterKey in defaultParameters.Keys)
            {
                if (!toReturn.ContainsKey(defaultParameterKey))
                {
                    toReturn.Add(defaultParameterKey, defaultParameters[defaultParameterKey]);
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Gets the default depended parameter values.  Looks into expression extensions to get them.
        /// </summary>
        /// <param name="expressionExtension">String containing the json extensions with the default value for parameters.</param>
        /// <returns>
        /// A dictionary with parameter default values.
        /// </returns>
        public static Dictionary<string, string> GetDefaultDependedParameterValues(string expressionExtension)
        {
            Dictionary<string, string> toReturn = null;
            if (!string.IsNullOrWhiteSpace(expressionExtension))
            {
                var jsonObject = JsonConvert.DeserializeObject<JObject>(expressionExtension);
                var parameterValues = jsonObject.Value<JArray>("parameterValues");
                if (parameterValues != null && parameterValues.Count > 0)
                {
                    toReturn = new Dictionary<string, string>();
                }

                foreach (JObject parameterValue in parameterValues)
                {
                    var parameterName = parameterValue.Value<string>("Name");
                    var defaultValue = parameterValue.Value<string>("DefaultValue");

                    InputValidationHelper.AssertNotEmpty(
                        defaultValue,
                        "DefaultValue",
                        $"Default value must be provided for depended parameter {parameterName}");

                    toReturn.Add(parameterName, defaultValue);
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Makes parameter names unique by adding an integer at the end of the parameter.
        /// </summary>
        /// <param name="parameterNames">The parameter names.</param>
        /// <returns>
        /// The unique parameter list.
        /// </returns>
        public static string[] MakeParameterNamesUnique(string[] parameterNames)
        {
            if (parameterNames == null || parameterNames.Length == 0)
            {
                return parameterNames;
            }

            var uniqueParameters = parameterNames.ToArray();
            var dedupParamaterIndexes = new Dictionary<string, int>();
            for (int i = 0; i < uniqueParameters.Length; i++)
            {
                var parameterName = uniqueParameters[i];
                var originalParameterName = uniqueParameters[i];
                var duplicated = dedupParamaterIndexes.ContainsKey(parameterName) ||
                    (from p in uniqueParameters
                        where p.Equals(parameterName, System.StringComparison.OrdinalIgnoreCase)
                        select p).Count() > 1;

                while (duplicated)
                {
                    if (dedupParamaterIndexes.ContainsKey(parameterName))
                    {
                        int dedupIndex = dedupParamaterIndexes[parameterName];
                        dedupIndex = dedupIndex + 1;
                        dedupParamaterIndexes[parameterName] = dedupIndex;
                        parameterName = $"{originalParameterName}{dedupIndex}";
                    }
                    else
                    {
                        dedupParamaterIndexes[parameterName] = 0;
                        parameterName = $"{originalParameterName}{0}";
                    }

                    uniqueParameters[i] = parameterName;

                    duplicated = dedupParamaterIndexes.ContainsKey(parameterName) ||
                        (from p in uniqueParameters
                         where p.Equals(parameterName, System.StringComparison.OrdinalIgnoreCase)
                         select p).Count() > 1;
                }
            }

            return uniqueParameters;
        }
    }
}
