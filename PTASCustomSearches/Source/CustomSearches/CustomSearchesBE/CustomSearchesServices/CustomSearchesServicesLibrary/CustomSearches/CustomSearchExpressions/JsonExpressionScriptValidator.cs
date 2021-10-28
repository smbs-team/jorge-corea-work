namespace CustomSearchesServicesLibrary.CustomSearches.CustomSearchExpressions
{
    using System.Linq;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Validates JSon expressions.
    /// </summary>
    public static class JsonExpressionScriptValidator
    {
        /// <summary>
        /// Validates the JSon payload.
        /// </summary>
        /// <param name="expression">The expression to validate.</param>
        /// <returns>The validation results.</returns>
        public static CustomExpressionValidationResult ValidateJSonPayload(CustomSearchExpression expression)
        {
            if (string.IsNullOrWhiteSpace(expression.Script))
            {
                return new CustomExpressionValidationResult(expression)
                {
                    Success = false,
                    ValidationError = "Script field can't be empty."
                };
            }

            try
            {
                var scriptResults = JsonHelper.DeserializeObject<object[]>(expression.Script);
                if (scriptResults.Length == 0)
                {
                    var validationResult = new CustomExpressionValidationResult(expression)
                    {
                        Success = false,
                        ValidationError = "JSonPayload is empty",
                    };

                    return validationResult;
                }

                if (expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.EditLookupExpression.ToString().ToLower() ||
                    expression.ExpressionRole.ToLower() == CustomSearchExpressionRoleType.LookupExpression.ToString().ToLower())
                {
                    foreach (var result in scriptResults)
                    {
                        var jsonObject = result as JObject;

                        // 'Value' is required, 'Key' is optional.
                        if (jsonObject == null)
                        {
                            string validSchema = "[ { \"Value\": \"string\", \"Key\": \"string\" } ]";
                            var validationResult = new CustomExpressionValidationResult(expression)
                            {
                                Success = false,
                                ValidationError = $"JSonPayload bad format.",
                                ValidationErrorDetails = $"Payload needs to follow this schema: {validSchema}"
                            };

                            return validationResult;
                        }

                        JProperty valueProperty = (
                            from p in jsonObject.Properties()
                            where p.Name.ToLower() == "Value".ToLower()
                            select p).FirstOrDefault();

                        // 'Value' is required, 'Key' is optional.
                        if (valueProperty == null)
                        {
                            var validationResult = new CustomExpressionValidationResult(expression)
                            {
                                Success = false,
                                ValidationError = $"JSonPayload bad format.",
                                ValidationErrorDetails = $"JSonPayload does not contain a 'Value' field. Validation failed for:  {jsonObject.ToString()}"
                            };

                            return validationResult;
                        }
                    }
                }
            }
            catch (Newtonsoft.Json.JsonException jsonSerializationException)
            {
                var validationResult = new CustomExpressionValidationResult(expression)
                {
                    Success = false,
                    ValidationError = "JSonPayload bad format.",
                    ValidationErrorDetails = jsonSerializationException.GetBaseException().Message
                };

                return validationResult;
            }

            return new CustomExpressionValidationResult(expression);
        }
    }
}
