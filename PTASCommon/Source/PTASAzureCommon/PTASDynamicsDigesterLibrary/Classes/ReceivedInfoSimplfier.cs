// <copyright file="ReceivedInfoSimplfier.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsDigesterLibrary.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// ReceivedInfoSimplfier: Simplifies access to object.
    /// </summary>
    public class ReceivedInfoSimplfier
    {
        private readonly ReceivedEntityInfo info;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivedInfoSimplfier"/> class.
        /// </summary>
        /// <param name="info">Received information.</param>
        public ReceivedInfoSimplfier(ReceivedEntityInfo info)
        {
            this.info = info;
        }

        /// <summary>
        /// Gets all values.
        /// </summary>
        public IEnumerable<KeyValLower> AllValues
            => (this.FirstItem?.Value.Attributes as JArray)
                    .Select((dynamic v) => new KeyValLower { Key = v.key, Value = v.value })
                   .ToList();

        /// <summary>
        /// Gets id.
        /// </summary>
        public Guid Id => this.info.PrimaryEntityId;

        /// <summary>
        /// Gets a value indicating whether insert.
        /// </summary>
        public bool IsInsert => this.info.MessageName == "Create";

        /// <summary>
        /// Gets a value indicating whether isUpdate.
        /// </summary>
        public bool IsUpdate => this.info.MessageName == "Update";

        /// <summary>
        /// Gets primaryEntityName.
        /// </summary>
        public string PrimaryEntityName => this.info.PrimaryEntityName;

        /// <summary>
        /// Gets values minus primary key.
        /// </summary>
        public IEnumerable<KeyValLower> Values
            => this.AllValues?.Where(p => p.Key != this.GetPrimaryKeyName())
                   .ToList();

        /// <summary>
        /// Gets shorthand for item.
        /// </summary>
        private InputParameters FirstItem =>
            this.info.InputParameters
            .Where(t => t.Key == "Target")
            .FirstOrDefault();

        /// <summary>
        /// Will pass only fields that are included in the validEntries.
        /// </summary>
        /// <param name="validEntries">Valid field names.</param>
        /// <returns>Curated list.</returns>
        public IEnumerable<KeyValLower> AllFilteredValues(IEnumerable<string> validEntries) => validEntries?.Any() == true
                ? this.AllValues.Join(validEntries, d => (string)d.Key, v => v, (d, v) => d).ToList()
                : this.AllValues.ToList();

        /// <summary>
        /// Gets primaryKeyName.
        /// </summary>
        /// <returns>Primary key name.</returns>
        public string GetPrimaryKeyName()
        {
            string entityId = this.info.PrimaryEntityId.ToString();
            return (this.FirstItem?.Value.Attributes as JArray)
                .Where((dynamic p) =>
                {
                    string value = p.value?.ToString() ?? string.Empty;
                    return entityId.Equals(value);
                }).FirstOrDefault()?.key;
        }
    }
}