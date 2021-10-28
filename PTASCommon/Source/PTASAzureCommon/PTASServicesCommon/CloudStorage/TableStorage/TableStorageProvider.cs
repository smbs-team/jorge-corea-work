namespace PTASServicesCommon.CloudStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Interface that defines the contract to provide basic Table Storage operations.
    /// </summary>
    public class TableStorageProvider : ITableStorageProvider
    {
        /// <summary>
        /// The batch size for batch operations.
        /// </summary>
        public const int BatchOperationsSize = 100;

        /// <summary>
        /// The cloud storage provider.
        /// </summary>
        private CloudTableClient cloudTableClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableStorageProvider"/> class.
        /// </summary>
        /// <param name="cloudStorageProvider">The cloud storage provider.</param>
        /// <exception cref="ArgumentNullException">cloudStorageProvider.</exception>
        public TableStorageProvider(ICloudStorageProvider cloudStorageProvider)
        {
            if (cloudStorageProvider == null)
            {
                throw new ArgumentNullException(nameof(cloudStorageProvider));
            }

            this.cloudTableClient = cloudStorageProvider.GetCloudTableClient().Result;
        }

        /// <summary>
        /// Ensures the table exists.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>Nothing.</returns>
        public async Task EnsureTableExists(string tableName)
        {
            CloudTable table = this.cloudTableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <returns>A single Entity.</returns>
        public async Task<T> GetEntityAsync<T>(
            string partitionKey,
            string rowKey,
            string tableName,
            List<string> selectedColumns = null)
            where T : ITableEntity, new()
        {
            T result = default(T);

            CloudTable table = this.cloudTableClient.GetTableReference(tableName);
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey, selectedColumns);

            // Execute the operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                result = (T)retrievedResult.Result;
            }

            return result;
        }

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
        public async Task<List<T>> GetEntitiesAsync<T>(
            string partitionKey,
            string tableName,
            int? take = null,
            List<string> selectedColumns = null,
            string partitionKeyQueryComparison = QueryComparisons.Equal)
            where T : ITableEntity, new()
        {
            return await this.GetEntitiesAsync<T>(partitionKey, null /*rowKeyPrefix*/, tableName, take, selectedColumns, partitionKeyQueryComparison);
        }

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
        public async Task<List<T>> GetEntitiesAsync<T>(
            string partitionKey,
            string rowKeyPrefix,
            string tableName,
            int? take = null,
            List<string> selectedColumns = null,
            string partitionKeyQueryComparison = QueryComparisons.Equal)
            where T : ITableEntity, new()
        {
            List<T> result = new List<T>();
            CloudTable table = this.cloudTableClient.GetTableReference(tableName);

            string filter = null;

            if (partitionKey != null)
            {
                filter = TableQuery.GenerateFilterCondition("PartitionKey", partitionKeyQueryComparison, partitionKey);
                if (rowKeyPrefix != null)
                {
                    string prefixCondition = TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, rowKeyPrefix),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, rowKeyPrefix + '{'));

                    filter = TableQuery.CombineFilters(filter, TableOperators.And, prefixCondition);
                }
            }

            TableQuery<T> query = new TableQuery<T>().Where(filter).Select(selectedColumns).Take(take);
            int resultsCount = 0;
            TableContinuationToken token = null;
            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, token);
                resultsCount += segment.Results.Count;
                if ((take == null) || (resultsCount < take))
                {
                    // If a continuation token is required,
                    // there are going to be more entities than expected in the take parameter.
                    token = segment.ContinuationToken;
                }
                else
                {
                    token = null;
                }

                if (segment.Results.Count > 0)
                {
                    result.AddRange(segment.Results);
                }
            }
            while (token != null);

            return result;
        }

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The inserted entity.</returns>
        public async Task<T> InsertAsync<T>(T entity, string tableName)
            where T : ITableEntity, new()
        {
            return await this.SaveAsync<T>(TableOperation.Insert(entity), tableName);
        }

        /// <summary>
        /// Inserts the or replaces an entity.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The inserted entity.</returns>
        public async Task<T> InsertOrReplaceAsync<T>(T entity, string tableName)
            where T : ITableEntity, new()
        {
            return await this.SaveAsync<T>(TableOperation.InsertOrReplace(entity), tableName);
        }

        /// <summary>
        /// Inserts the or merge an entity.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The inserted entity.</returns>
        public async Task<T> InsertOrMergeAsync<T>(T entity, string tableName)
            where T : ITableEntity, new()
        {
            return await this.SaveAsync<T>(TableOperation.InsertOrMerge(entity), tableName);
        }

        /// <summary>
        /// Merges the asynchronous.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The inserted entity.</returns>
        public async Task<T> MergeAsync<T>(T entity, string tableName)
            where T : ITableEntity, new()
        {
            return await this.SaveAsync<T>(TableOperation.Merge(entity), tableName);
        }

        /// <summary>
        /// Replaces the entity.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The replaced entity.</returns>
        public async Task<T> ReplaceAsync<T>(T entity, string tableName)
            where T : ITableEntity, new()
        {
            return await this.SaveAsync<T>(TableOperation.Replace(entity), tableName);
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="T">Table entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The deleted entity.</returns>
        public async Task<T> DeleteAsync<T>(T entity, string tableName)
            where T : ITableEntity, new()
        {
            return await this.SaveAsync<T>(TableOperation.Delete(entity), tableName);
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The deleted entity.</returns>
        public async Task<TableEntity> DeleteAsync(string partitionKey, string rowKey, string tableName)
        {
            var entity = new DynamicTableEntity(partitionKey, rowKey);
            entity.ETag = "*";
            return await this.SaveAsync<TableEntity>(TableOperation.Delete(entity), tableName);
        }

        /// <summary>
        /// Executes the batch asynchronous.
        /// </summary>
        /// <param name="tableBatchOperation">The table batch operation.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The table result of the batch.</returns>
        public async Task<IList<TableResult>> ExecuteBatchAsync(TableBatchOperation tableBatchOperation, string tableName)
        {
            List<TableResult> retrievedResult = new List<TableResult>();
            var tasks = new List<Task<IList<TableResult>>>();

            var operationsList = tableBatchOperation.ToList();

            for (int i = 0; i < operationsList.Count; i += TableStorageProvider.BatchOperationsSize)
            {
                var blockList = operationsList.GetRange(i, Math.Min(TableStorageProvider.BatchOperationsSize, tableBatchOperation.Count - i));
                TableBatchOperation newTableBatchOperation = new TableBatchOperation();
                blockList.ForEach(o => newTableBatchOperation.Add(o));
                Task<IList<TableResult>> executeBatchTask = this.cloudTableClient.GetTableReference(tableName).
                    ExecuteBatchAsync(newTableBatchOperation);
                tasks.Add(executeBatchTask);
            }

            Task whenAllTask = Task.WhenAll(tasks);
            await whenAllTask;

            tasks.ForEach(t => retrievedResult.AddRange(t.Result));

            return retrievedResult;
        }

        private async Task<T> SaveAsync<T>(TableOperation tableOperation, string tableName)
            where T : ITableEntity, new()
        {
            T result = default(T);
            TableResult retrievedResult =
                await this.cloudTableClient.GetTableReference(tableName).ExecuteAsync(tableOperation);

            if (retrievedResult.Result != null)
            {
                result = (T)retrievedResult.Result;
            }

            return result;
        }
    }
}