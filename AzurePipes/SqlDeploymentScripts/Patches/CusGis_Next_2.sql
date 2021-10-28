ALTER TABLE [cus].[Dataset] DROP CONSTRAINT IF EXISTS [FK_Dataset_ToParentFolderId] 

ALTER TABLE [cus].[Dataset]  WITH NOCHECK ADD  CONSTRAINT [FK_Dataset_ToParentFolderId] FOREIGN KEY([ParentFolderId])
REFERENCES [cus].[Folder] ([FolderId])
ALTER TABLE [cus].[Dataset] CHECK CONSTRAINT [FK_Dataset_ToParentFolderId]

GO --0. Re-add [FK_Dataset_ToParentFolderId] constraint

ALTER TABLE [cus].[Dataset] ALTER COLUMN LastExecutedBy UniqueIdentifier NULL

GO --1. Remove null [Dataset].LastExecutedBy NULL constraint

ALTER PROCEDURE [cus].[SP_GetAlterDatasetLockV2]
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
							WHERE (([DatasetId] = @datasetId or [SourceDatasetId] = @datasetId) and [DbLockType] IS NULL)) unlockedDataset
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
				    WHERE (([DatasetId] = @datasetId and [DbLockType] IS NULL) or ([DatasetId] = @parentDatasetId  and ISNULL([DbLockType], '') != 'RootLock'))) unlockedDataset
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
    FROM [cus].[Dataset]
    WHERE [DatasetId] = @resultDatasetId
GO --2. Update [SP_GetAlterDatasetLockV2]

ALTER PROCEDURE [cus].[SP_TestAlterDatasetLock]
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
							WHERE (([DatasetId] = @datasetId or [SourceDatasetId] = @datasetId) and [DbLockType] IS NULL)) unlockedDataset
					ON dataset.DatasetId = unlockedDataset.UnlockedDatasetId
				) datasetWithLockInfo
				WHERE ((datasetCount = unlockedDatasetCount) AND ([DatasetId] = @datasetId)))
			update cte SET 		
				@resultDatasetId = [cte].[DatasetId],
				@previousPostProcessState = [cte].[DatasetPostProcessState],
				@previousDatasetState = [cte].[DatasetState];
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
				    WHERE (([DatasetId] = @datasetId and [DbLockType] IS NULL) or ([DatasetId] = @parentDatasetId  and ISNULL([DbLockType], '') != 'RootLock'))) unlockedDataset
			    ON dataset.DatasetId = unlockedDataset.UnlockedDatasetId
			) datasetWithLockInfo
			WHERE ((datasetCount = unlockedDatasetCount) AND ([DatasetId] = @datasetId)))
	    update cte SET 		
		    @resultDatasetId = [cte].[DatasetId],
		    @previousPostProcessState = [cte].[DatasetPostProcessState],
            @previousDatasetState = [cte].[DatasetState]
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
    FROM [cus].[Dataset]
    WHERE [DatasetId] = @resultDatasetId
GO --3. Update [SP_TestAlterDatasetLock]

DROP PROCEDURE IF EXISTS [gis].[SwitchStagingTable]

GO--4. Delete [SwitchStagingTable] Stored Procedure if it exists



CREATE PROCEDURE [gis].[SwitchStagingTable]
	@TableName VARCHAR(MAX)
AS
DECLARE @StagingTable VARCHAR(MAX) = CONCAT(@TableName, '_staging');
DECLARE @OldTable VARCHAR(MAX) = CONCAT(@TableName, '_old');

EXEC ('DROP TABLE IF EXISTS ' + @TableName)  
EXEC sp_rename @StagingTable, @TableName
RETURN 0



GO--5. Create [SwitchStagingTable] Stored Procedure


