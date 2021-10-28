namespace CustomSearchesServicesLibrary.CustomSearches
{
    using System;
    using System.Collections;
    using System.Linq;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;

    /// <summary>
    /// Input validation helper.
    /// </summary>
    public class InputValidationHelper
    {
        /// <summary>
        /// Validates if an array is empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the array.</typeparam>
        /// <param name="values">The values.</param>
        /// <param name="name">The name of the validated property.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Value should not be empty.</exception>
        public static void AssertArrayNotEmpty<T>(T[] values, string name, string message = null)
        {
            if (values != null && values.Length > 0)
            {
                foreach (var value in values)
                {
                    if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        return;
                    }
                }
            }

            throw new CustomSearchesRequestBodyException(
                message ?? $"'{name}' array should not be empty.",
                innerException: null);
        }

        /// <summary>
        /// Validates if the value is greater than.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="name">The name of the value.</param>
        /// <param name="messagerError">The message error.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Value should be empty.</exception>
        public static void AssertShouldBeGreaterThan(int value, int expected, string name, string messagerError = null)
        {
            if (value <= expected)
            {
                throw new CustomSearchesRequestBodyException(
                    messagerError ?? $"'{name}' should be greater than {expected}.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Validates if the value is equal or greater than.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="name">The name of the value.</param>
        /// <param name="messagerError">The message error.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Value should be empty.</exception>
        public static void AssertShouldBeEqualOrGreaterThan(int value, int expected, string name, string messagerError = null)
        {
            if (value < expected)
            {
                throw new CustomSearchesRequestBodyException(
                    messagerError ?? $"'{name}' should be equal or greater than {expected}.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Validates if the value is empty.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="name">The name of the collection.</param>
        /// <param name="messagerError">The message error.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Value should be empty.</exception>
        public static void AssertEmpty(ICollection collection, string name, string messagerError = null)
        {
            if (collection?.Count > 0)
            {
                throw new CustomSearchesRequestBodyException(
                    messagerError ?? $"'{name}' should be empty.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Validates if the value is zero.
        /// </summary>
        /// <param name="value">The collection.</param>
        /// <param name="entityName">The entity name.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="messagerError">The message error.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Value should be empty.</exception>
        public static void AssertZero(int value, string entityName, string propertyName, string messagerError = null)
        {
            if (value > 0)
            {
                throw new CustomSearchesRequestBodyException(
                    messagerError ?? $"The field '{propertyName}' of the entity '{entityName}' should be 0.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Validates if the value is not empty.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="name">The name of the collection.</param>
        /// <param name="messagerError">The message error.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Value should not be empty.</exception>
        public static void AssertNotEmpty(ICollection collection, string name, string messagerError = null)
        {
            if ((collection == null) || (collection.Count == 0))
            {
                throw new CustomSearchesRequestBodyException(
                    messagerError ?? $"'{name}' should not be empty.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Validates if the value is not empty.
        /// </summary>
        /// <param name="value">The value of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="messagerError">The message error.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Value should not be empty.</exception>
        public static void AssertNotEmpty(Guid value, string name, string messagerError = null)
        {
            if (value == Guid.Empty)
            {
                throw new CustomSearchesRequestBodyException(
                    messagerError ?? $"'{name}' should not be empty.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Validates if the value is not empty.
        /// </summary>
        /// <param name="value">The value of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="messagerError">The message error.</param>
        /// <exception cref="CustomSearchesRequestBodyException">Value should not be null or white space.</exception>
        public static void AssertNotEmpty(string value, string name, string messagerError = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new CustomSearchesRequestBodyException(
                    messagerError ?? $"'{name}' should not be empty.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Validates if the entity was found.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="entityId">The id of the entity.</param>
        /// <param name="message">The exception message.</param>
        /// <exception cref="CustomSearchesEntityNotFoundException">Entity should not be null.</exception>
        public static void AssertEntityExists(object entity, string entityName, object entityId, string message = null)
        {
            if (entity == null)
            {
                throw new CustomSearchesEntityNotFoundException(
                    message ?? $"{entityName} '{entityId}' was not found.",
                    innerException: null);
            }
        }

        /// <summary>
        /// Validates if the value corresponds to the enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value of property.</param>
        /// <param name="name">The name of property.</param>
        /// <returns>The enum value.</returns>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public static TEnum ValidateEnum<TEnum>(string value, string name)
            where TEnum : struct
        {
            TEnum enumType;
            if (!Enum.TryParse<TEnum>(value?.Trim(), ignoreCase: true, out enumType))
            {
                string validTypes = string.Join(", ", ((TEnum[])Enum.GetValues(typeof(TEnum))).Select(v => $"'{v}'"));
                throw new CustomSearchesRequestBodyException(
                    $"'{name}' '{value}' is invalid. Valid values are: {validTypes}.",
                    null);
            }

            return enumType;
        }

        /// <summary>
        /// Validates if the dataset data is not locked.
        /// </summary>
        /// <param name="dataset">The name of property.</param>
        /// <exception cref="CustomSearchesConflictException">"The data of dataset '{dataset.DatasetId}' is locked.</exception>
        public static void AssertDatasetDataNotLocked(Dataset dataset)
        {
            if (dataset.IsDataLocked == true)
            {
                throw new CustomSearchesConflictException(
                    $"The data of dataset '{dataset.DatasetId}' is locked.",
                    null);
            }
        }
    }
}
