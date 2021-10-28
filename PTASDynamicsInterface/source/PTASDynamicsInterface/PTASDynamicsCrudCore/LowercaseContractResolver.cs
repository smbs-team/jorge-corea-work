// <copyright file="LowercaseContractResolver.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore
{
  using Newtonsoft.Json.Serialization;

    /// <summary>
    /// This class allows to lower the property name for JSON Mappings.
    /// </summary>
  public class LowercaseContractResolver : DefaultContractResolver
  {
        /// <summary>
        /// This method transform the parameter value to lower the string.
        /// </summary>
        /// <param name="propertyName">the value to be lower.</param>
        /// <returns>the lowercase value.</returns>
        protected override string ResolvePropertyName(string propertyName) =>
      propertyName.ToLower();
  }
}
