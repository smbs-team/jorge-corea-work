CREATE PROCEDURE [cus].[SP_GetAlterDatasetLockV2]
	@datasetId NVARCHAR(256),
    @newPostProcessState NVARCHAR(256),
    @newState NVARCHAR(256),
    @isRootLock bit,
	@userId NVARCHAR(256),
	@lockingJobId INT
AS
	set nocount on;

	DECLARE @resultDatasetId UNIQUEIDENTIFIER = NULL;
	DECLARE @previousPostProcessState NVARCHAR(256) = NULL;
    DECLARE @previousDatasetState NVARCHAR(256) = NULL;
    declare @parentDatasetId NVARCHAR(256)

    SELECT @parentDatasetId = SourceDataSetId from [cus].[Dataset] where ([DatasetId] = @datasetId);

    IF (@isRootLock != 0)
    BEGIN
        IF (@parentDatasetId IS NULL)
        BEGIN
			WITH cte AS (
				SELECT TOP(1) * 
				FROM (
					SELECT dataset.*, unlockedDataset.UnlockedDatasetId, unlockedDataset.unlockedDatasetCount
					FROM 
						(SELECT *, count(datasetId) Over (Partition by ParentFolderId) as datasetCount
							FROM [cus].[Dataset] WITH (rowlock, readpast)
							WHERE ([DatasetId] = @datasetId) or [SourceDatasetId] = @datasetId) dataset 
					LEFT JOIN 
						(SELECT DatasetId as UnlockedDatasetId, count(datasetId) Over (Partition by ParentFolderId) as unlockedDatasetCount
							FROM [cus].[Dataset] WITH (rowlock, readpast)
							WHERE (([DatasetId] = @datasetId or [SourceDatasetId] = @datasetId) and 
							  cus.FN_IsDatasetLocked([DbLockType], [DbLockTime], [LockingJobId]) = 0)) unlockedDataset
					ON dataset.DatasetId = unlockedDataset.UnlockedDatasetId
				) datasetWithLockInfo
				WHERE ((datasetCount = unlockedDatasetCount) AND ([DatasetId] = @datasetId)))
			update cte SET 		
				@resultDatasetId = [cte].[DatasetId],
				@previousPostProcessState = [cte].[DatasetPostProcessState],
				@previousDatasetState = [cte].[DatasetState],
				DatasetPostProcessState = @newPostProcessState,
				DatasetState = @newState,
				DbLockType = 'RootLock',
				DbLockTime = GETUTCDATE(),
				LockingJobId = @lockingJobId,        
				LastModifiedTimestamp = GETUTCDATE(),
				LastModifiedBy = userId;
        END
    END
    ELSE
    BEGIN
        WITH cte AS (
	    SELECT TOP(1) * 
		   FROM (
			    SELECT dataset.*, unlockedDataset.UnlockedDatasetId, unlockedDataset.unlockedDatasetCount
			    FROM 
				    (SELECT *, count(datasetId) Over (Partition by ParentFolderId) as datasetCount
				    FROM [cus].[Dataset] WITH (rowlock, readpast)
				    WHERE ([DatasetId] = @datasetId) or [DatasetId] = @parentDatasetId) dataset
			    LEFT JOIN 
				    (SELECT DatasetId as UnlockedDatasetId, count(datasetId) Over (Partition by ParentFolderId) as unlockedDatasetCount
				    FROM [cus].[Dataset] WITH (rowlock, readpast)
				    WHERE (([DatasetId] = @datasetId and cus.FN_IsDatasetLocked([DbLockType], [DbLockTime], [LockingJobId]) = 0) 
					  or ([DatasetId] = @parentDatasetId and cus.FN_IsDatasetRootLocked([DbLockType], [DbLockTime], [LockingJobId]) = 0))) unlockedDataset
			    ON dataset.DatasetId = unlockedDataset.UnlockedDatasetId
			) datasetWithLockInfo
			WHERE ((datasetCount = unlockedDatasetCount) AND ([DatasetId] = @datasetId)))
	    update cte SET 		
		    @resultDatasetId = [cte].[DatasetId],
		    @previousPostProcessState = [cte].[DatasetPostProcessState],
            @previousDatasetState = [cte].[DatasetState],
		    DatasetPostProcessState = @newPostProcessState,
            DatasetState = @newState,
            DbLockType = 'Lock',
			DbLockTime = GETUTCDATE(),
		    LockingJobId = @lockingJobId,        
		    LastModifiedTimestamp = GETUTCDATE(),
		    LastModifiedBy = userId;
    END	

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
      ,@previousDatasetState as DataSetState
      ,[GenerateIndexesElapsedMs]
      ,[TotalRows]
      ,[LastModifiedTimestamp]
      ,[CreatedBy]
      ,[LastModifiedBy]
      ,[LastExecutionTimestamp]
      ,@previousPostProcessState as DataSetPostProcessState
      ,[LockingJobId]
	  ,[SourceDatasetId]
	  ,[DbLockType]
	  ,[Comments]
	  ,[LastExecutedBy]
	  ,[IsDataLocked]
	  ,[DbLockTime]
    FROM [cus].[Dataset]
    WHERE [DatasetId] = @resultDatasetId