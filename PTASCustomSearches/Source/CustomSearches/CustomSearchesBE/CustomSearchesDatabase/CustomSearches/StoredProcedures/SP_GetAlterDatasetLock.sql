CREATE PROCEDURE [cus].[SP_GetAlterDatasetLock]
	@datasetId NVARCHAR(256),
	@userId NVARCHAR(256),
	@lockingJobId INT

AS
	set nocount on;

	DECLARE @resultDatasetId UNIQUEIDENTIFIER = NULL;
	DECLARE @previousPostProcessState NVARCHAR(256) = NULL;

	WITH cte AS (
		SELECT TOP(1) * 
		FROM [cus].[Dataset] AS ds WITH (rowlock, readpast)
		WHERE (([DatasetState] = 'Processed') AND ([DatasetPostProcessState] != 'Processing') AND ([DatasetId] = @datasetId)))
	update cte SET 		
		@resultDatasetId = [cte].[DatasetId],
		@previousPostProcessState = [cte].[DatasetPostProcessState],
		DatasetPostProcessState = 'Processing',
		LockingJobId = @lockingJobId,
		LastModifiedTimestamp = GETUTCDATE(),
		LastModifiedBy = userId;

SELECT [DatasetId]
      ,[CustomSearchDefinitionId]
      ,[UserId]
      ,[ParentFolderId]
      ,[DatasetName]
      ,[ParameterValues]
      ,[GeneratedTableName]
      ,[GenerateSchemaElapsedMs]
      ,[ExecuteStoreProcedureElapsedMs]
      ,[IsLocked]
      ,[CreatedTimestamp]
      ,[DatasetClientState]
      ,[DataSetState]
      ,[GenerateIndexesElapsedMs]
      ,[TotalRows]
      ,[LastModifiedTimestamp]
      ,[CreatedBy]
      ,[LastModifiedBy]
      ,[LastExecutionTimestamp]
      ,@previousPostProcessState as DataSetPostProcessState
      ,[LockingJobId]
    FROM [cus].[Dataset]
    WHERE [DatasetId] = @DatasetId