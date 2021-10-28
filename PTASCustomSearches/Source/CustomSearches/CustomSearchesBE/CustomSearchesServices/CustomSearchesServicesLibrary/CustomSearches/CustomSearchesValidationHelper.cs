namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using DocumentFormat.OpenXml.Drawing.Charts;
    using DocumentFormat.OpenXml.Drawing.Diagrams;
    using DocumentFormat.OpenXml.Office2010.ExcelAc;
    using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;

    /// <summary>
    /// Dataset state helper.
    /// </summary>
    public class CustomSearchesValidationHelper
    {
        /// <summary>
        /// The multiple selection custom search parameter separator.
        /// </summary>
        private const string MultipleSelectionSeparator = ";";

        /// <summary>
        /// Validates if the parameters values are valid for the custom search definition.
        /// </summary>
        /// <param name="customSearchDefinition">The custom search definition.</param>
        /// <param name="parametersData">The custom search parameters.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="CustomSearchesEntityNotFoundException">Parameter was not found.</exception>
        public static void AssertParameterValuesAreValid(CustomSearchDefinition customSearchDefinition, CustomSearchParameterValueData[] parametersData)
        {
            if (parametersData.Length > 0)
            {
                foreach (var parameterData in parametersData)
                {
                    var parameter = customSearchDefinition.CustomSearchParameter
                        .FirstOrDefault(p => ((parameterData.Id != null) && (p.CustomSearchParameterId == parameterData.Id)) ||
                            ((!string.IsNullOrWhiteSpace(parameterData.Name)) && (p.ParameterName.ToLower() == parameterData.Name.ToLower())));

                    // Validates if parameter exists in the definition
                    if (parameter == null)
                    {
                        throw new CustomSearchesEntityNotFoundException(
                            $"Custom search definition '{customSearchDefinition.CustomSearchDefinitionId}' does not contain the parameter '{parameterData.Name}'.",
                            innerException: null);
                    }

                    // Validates if parameter can be converted to the expected type
                    string parameterName = parameterData.Id > 0 ? parameterData.Id.ToString() : parameterData.Name;
                    AssertParameterValueIsAssignable(parameterName, parameterData.Value, parameter.ParameterDataType, parameter.AllowMultipleSelection);
                }
            }

            // Validates required parameters
            var requiredParameters = customSearchDefinition.CustomSearchParameter.Where(p => p.ParameterIsRequired && (p.ParameterDefaultValue == null));
            foreach (var requiredParameter in requiredParameters)
            {
                var parameterData = parametersData.FirstOrDefault(p => ((p.Id != null) && (requiredParameter.CustomSearchParameterId == p.Id)) ||
                            ((!string.IsNullOrWhiteSpace(p.Name)) && (requiredParameter.ParameterName.ToLower() == p.Name.ToLower())));
                if (parameterData == null)
                {
                    throw new CustomSearchesRequestBodyException(
                        $"Should be included a parameter '{requiredParameter.ParameterName}' of type '{requiredParameter.ParameterDataType}'.",
                        innerException: null);
                }
            }
        }

        /// <summary>
        /// Validates if parameter can be converted to the expected type, taking into account if multiple selection
        /// is allowed.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="parameterValue">The parameter value to validate.</param>
        /// <param name="parameterTypeName">The parameter type name.</param>
        /// <param name="allowMultipleSelection">Indicates whether this parameter allows for multiple selection.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Parameter has invalid value.</exception>
        public static void AssertParameterValueIsAssignable(
            string parameterName,
            object parameterValue,
            string parameterTypeName,
            bool allowMultipleSelection)
        {
            var parameterValues = new List<object>();

            if (allowMultipleSelection && parameterValue.GetType() == typeof(string))
            {
                string[] values = ((string)parameterValue).Split(CustomSearchesValidationHelper.MultipleSelectionSeparator);
                foreach (var value in values)
                {
                    parameterValues.Add(value);
                }
            }
            else
            {
                parameterValues.Add(parameterValue);
            }

            foreach (var value in parameterValues)
            {
                CustomSearchesValidationHelper.AssertParameterValueIsAssignable(
                    parameterName,
                    value,
                    parameterTypeName);
            }
        }

        /// <summary>
        /// Validates if parameter can be converted to the expected type.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="parameterValue">The parameter value to validate.</param>
        /// <param name="parameterTypeName">The parameter type name.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Parameter has invalid value.</exception>
        public static void AssertParameterValueIsAssignable(string parameterName, object parameterValue, string parameterTypeName)
        {
            try
            {
                Convert.ChangeType(parameterValue, Type.GetType($"System.{parameterTypeName}"));
            }
            catch (Exception ex)
            {
                if ((ex is InvalidCastException) || (ex is FormatException) || (ex is OverflowException) || (ex is ArgumentNullException))
                {
                    throw new CustomSearchesRequestBodyException(
                        $"The parameter '{parameterName}' with value '{parameterValue}' can't be converted to '{parameterTypeName}'.",
                        innerException: ex);
                }
            }
        }
    }
}
