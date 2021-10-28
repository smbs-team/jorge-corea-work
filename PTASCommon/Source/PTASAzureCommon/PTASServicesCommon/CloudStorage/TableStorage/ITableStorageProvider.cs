namespace PTASServicesCommon.CloudStorage
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Interface that defines the contract to provide basic Table Storage operations.
    /// </summary>
    public interface ITableStorageProvider
    {
        /// <summary>
        /// Ensures the table exists.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>Nothing.</returns>
        Task EnsureTableExists(string tableName);

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <returns>A single Entity.</returns>
        Task<T> GetEntityAsync<T>(
            string partitionKey,
            string rowKey,
            string tableName,
            List<string> selectedColumns = null)
            where T : ITableEntity, new();

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="take">The take.</param>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <param name="partitionKeyQueryComparison">The partition key query comparison.</param>
        /// <returns>A list of entities.</returns>
        Task<List<T>> GetEntitiesAsync<T>(
            string partitionKey,
            string tableName,
            int? take = null,
            List<string> selectedColumns = null,
            string partitionKeyQueryComparison = QueryComparisons.Equal)
            where T : ITableEntity, new();

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKeyPrefix">The row key prefix.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="take">The take.</param>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <param name="partitionKeyQueryComparison">The partition key query comparison.</param>
        /// <returns>A list of entities.</returns>
        Task<List<T>> GetEntitiesAsync<T>(
            string partitionKey,
            string rowKeyPrefix,
            string tableName,
            int? take = null,
            List<string> selectedColumns = null,
            string partitionKeyQueryComparison = QueryComparisons.Equal)
            where T : ITableEntity, new();

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The inserted entity.</returns>
        Task<T> InsertAsync<T>(T entity, string tableName)
            where T : ITableEntity, new();

        /// <summary>
        /// Inserts the or replaces an entity.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The inserted entity.</returns>
        Task<T> InsertOrReplaceAsync<T>(T entity, string tableName)
            where T : ITableEntity, new();

        /// <summary>
        /// Inserts the or merge an entity.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The inserted entity.</returns>
        Task<T> InsertOrMergeAsync<T>(T entity, string tableName)
            where T : ITableEntity, new();

        /// <summary>
        /// Merges the asynchronous.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The inserted entity.</returns>
        Task<T> MergeAsync<T>(T entity, string tableName)
            where T : ITableEntity, new();

        /// <summary>
        /// Replaces the entity.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The replaced entity.</returns>
        Task<T> ReplaceAsync<T>(T entity, string tableName)
            where T : ITableEntity, new();

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The deleted entity.</returns>
        Task<T> DeleteAsync<T>(T entity, string tableName)
            where T : ITableEntity, new();

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The deleted entity.</returns>
        Task<TableEntity> DeleteAsync(string partitionKey, string rowKey, string tableName);

        /// <summary>
        /// Executes the batch asynchronous.
        /// </summary>
        /// <param name="tableBatchOperation">The table batch operation.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The table result of the batch.</returns>
        Task<IList<TableResult>> ExecuteBatchAsync(TableBatchOperation tableBatchOperation, string tableName);
    }
}