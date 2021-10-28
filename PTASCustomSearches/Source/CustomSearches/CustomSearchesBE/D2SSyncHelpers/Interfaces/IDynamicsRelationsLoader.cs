// <copyright file="DynamicsMetadataReader.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace D2SSyncHelpers.Interfaces
{
    using System.Threading.Tasks;
    using D2SSyncHelpers.Models;

    /// <summary>
    /// Load relationships for an entity.
    /// </summary>
    public interface IDynamicsRelationsLoader
    {
        /// <summary>
        /// Get Related Entities to an entity.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<EntityReference[]> GetRelatedEntities(string entityName);
    }
}