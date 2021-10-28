// <copyright file="JsonHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore.Internal;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// JsonHelper.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Remove Empty Children.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="entityName">Entity to remove the fields from.</param>
        /// <param name="excludeFields">List of fields to exclude.</param>
        /// <param name="onlyPtas">Only ptas fields.</param>
        /// <returns>Object with no empty children.</returns>
        public static JToken RemoveEmptyChildren(JToken token, string entityName, string[] excludeFields = null, bool onlyPtas = true)
        {
            if (excludeFields == null)
            {
                excludeFields = new string[] { };
            }

            if (token.Type == JTokenType.Object)
            {
                JObject copy = new JObject();
                foreach (JProperty prop in token.Children<JProperty>())
                {
                    JToken child = prop.Value;
                    if (IsValidName(prop.Name, entityName, excludeFields, onlyPtas))
                    {
                        if (child.HasValues)
                        {
                            child = RemoveEmptyChildren(child, entityName, excludeFields, onlyPtas);
                        }

                        if (!IsEmpty(child))
                        {
                            copy.Add(prop.Name, child);
                        }
                    }
                }

                return copy;
            }
            else if (token.Type == JTokenType.Array)
            {
                JArray copy = new JArray();
                foreach (JToken item in token.Children())
                {
                    JToken child = item;
                    if (child.HasValues)
                    {
                        child = RemoveEmptyChildren(child, entityName, excludeFields, onlyPtas);
                    }

                    if (!IsEmpty(child))
                    {
                        copy.Add(child);
                    }
                }

                return copy;
            }

            return token;
        }

        /// <summary>
        /// Checks the validity of name.
        /// </summary>
        /// <param name="name">Name to check.</param>
        /// <param name="entityName">Entity Name.</param>
        /// <param name="excludeFields">Excluded fields.</param>
        /// <param name="onlyPtas">Only add ptas_ fields.</param>
        /// <returns>True if name if valid.</returns>
        public static bool IsValidName(string name, string entityName, string[] excludeFields, bool onlyPtas)
        {
            if (name.StartsWith("_"))
            {
                name = name.Substring(1).Replace("_value", string.Empty);
            }

            string thisEntityName = $"{entityName}.{name}";
            bool nameIsExcluded = excludeFields.Any(ef => ef == thisEntityName);

            if (name.StartsWith("@odata"))
            {
                return false;
            }

            return
                name.Equals("statecode") ||
                name.Equals("statuscode") ||
                (!nameIsExcluded &&
                    (!onlyPtas || name.Contains("ptas_")));
        }

        private static bool IsEmpty(JToken token)
        {
            ////if (token.Type == JTokenType.Null)
            ////{
            ////    return true;
            ////}
            return false;
        }
    }
}