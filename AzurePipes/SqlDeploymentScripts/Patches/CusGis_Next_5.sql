
DROP PROCEDURE IF EXISTS [cus].[SP_GetHistogram] 

GO--0. Delete  [cus].[SP_GetHistogram]  Stored Procedure if it exists

CREATE PROCEDURE [cus].[SP_GetHistogram]
(
	@DatasetTable NVARCHAR(MAX),
	@ValueScript NVARCHAR(MAX),
	@AutoBins BIT = 1,
	@NumBins FLOAT = 5,
	@UseDiscreteBins BIT = 0,
    @UseCategories BIT = 0,
	@CategoryColumn NVARCHAR(MAX) = NULL
)
AS
BEGIN
    -- Discrete test
	DECLARE @NumObservations INT
	DECLARE @MinObservation FLOAT
	DECLARE @MaxObservation FLOAT
	DECLARE @BinWidth FLOAT
	DECLARE @CurrentBin INT
	DECLARE @NonNumericObservations INT
	DECLARE @CurrentBinRangeStart FLOAT
	DECLARE @ReportedBinRangeStart FLOAT
	DECLARE @CurrentBinRangeEnd FLOAT
	DECLARE @ReportedBinRangeEnd FLOAT
	DECLARE @CurrentBinObservations FLOAT
	DECLARE @CurrentCategory NVARCHAR(MAX)
	DECLARE @CurrentCategoryIndex INT
	DECLARE @CategoryCount INT
	DECLARE @GatherDataSQLStatement NVARCHAR(MAX)
	DECLARE @GatherDataSQLParameterDef NVARCHAR(MAX)
	DECLARE @GatherCategoriesSQLStatement NVARCHAR(MAX)
	DECLARE @GetBinCountSQLStatement NVARCHAR(MAX)
	DECLARE @GetBinCountSQLParameterDef NVARCHAR(MAX)
	DECLARE @BinEndRangeCompareOperator NVARCHAR(MAX)

	IF (@DatasetTable IS NULL OR LEN(@DatasetTable) = 0)
	BEGIN
		RAISERROR('DatasetTable parameter is NULL or empty', 16, 1)
	END

	IF (@ValueScript IS NULL OR LEN(@ValueScript) = 0)
	BEGIN
		RAISERROR('ValueScript parameter is NULL or empty', 16, 1)
	END

	IF @UseCategories = 1 AND (@CategoryColumn IS NULL OR LEN(@CategoryColumn) = 0)
	BEGIN
		RAISERROR('Need to provide category column when using categories', 16, 1)
	END	

	-- Create Results table
	CREATE TABLE #BinResults
	(
		BinNumber INT,
		BinRangeStart FLOAT,
		BinRangeEnd FLOAT,
		BinObservations INT,
		BinCategory NVARCHAR(MAX)
	)

	-- Create Categories Table
	CREATE TABLE #Categories
	(
		BinCategory NVARCHAR(MAX)
	)

	IF @NonNumericObservations > 0
	BEGIN
		RAISERROR('Can''t execute histogram on non-numeric data ', 16, 1)
	END

	-- Gather Categories
	IF @UseCategories = 0
	BEGIN
		INSERT INTO #Categories (BinCategory)
			VALUES('Default')
	END ELSE
	BEGIN    		
		SET @GatherCategoriesSQLStatement = 
			'INSERT INTO #Categories (BinCategory)
			SELECT distinct(' + @CategoryColumn + ')
				FROM ' + @DatasetTable

		PRINT 'Executing: ' + @GatherCategoriesSQLStatement

		EXEC(@GatherCategoriesSQLStatement)
	END

	SELECT @CategoryCount = count(1) FROM #Categories
	SET @CurrentCategoryIndex = 0

	WHILE @CurrentCategoryIndex < @CategoryCount 
	BEGIN
		SELECT @CurrentCategory = cat.BinCategory FROM 
			(SELECT ROW_NUMBER() OVER (ORDER BY BinCategory) AS RowNumber, BinCategory FROM #Categories) cat
		WHERE cat.RowNumber = @CurrentCategoryIndex + 1

		PRINT '--------------------------------------------------'
		PRINT 'Current Category: ' + CONVERT(NVARCHAR(MAX), @CurrentCategory)

		-- Gather Data
		SET @GatherDataSQLParameterDef = 
			'@NumObservations INT OUTPUT, ' +
			'@MinObservation FLOAT OUTPUT, ' +
			'@MaxObservation FLOAT OUTPUT, ' +
			'@NonNumericObservations INT OUTPUT'

		SET @GatherDataSQLStatement = 
			'SELECT ' +
				'@NumObservations = COUNT (' + @ValueScript + '), ' +
				'@MinObservation = MIN(' + @ValueScript + '), ' +
				'@MaxObservation = MAX(' + @ValueScript + '), ' +
				'@NonNumericObservations = count(case when IsNumeric(' + @ValueScript + ') is null then 1 else null end)' +
			'FROM ' + @DatasetTable

		IF @UseCategories = 1
		BEGIN
			SET @GatherDataSQLStatement = @GatherDataSQLStatement  + ' WHERE CONVERT(NVARCHAR(MAX), ' + @CategoryColumn + ') = ''' + @CurrentCategory  + ''''			END


		PRINT 'Executing: ' + @GatherDataSQLStatement
		PRINT 'Parameters: ' + @GatherDataSQLParameterDef

		EXEC SP_EXECUTESQL  @GatherDataSQLStatement,
			@GatherDataSQLParameterDef,
			@NumObservations = @NumObservations OUTPUT, 
			@MinObservation = @MinObservation OUTPUT,
			@MaxObservation = @MaxObservation OUTPUT,
			@NonNumericObservations = @NonNumericObservations OUTPUT 

		PRINT '@NumObservations: ' + CONVERT(NVARCHAR(MAX), @NumObservations)
		PRINT '@MinObservation: ' + CONVERT(NVARCHAR(MAX), @MinObservation)
		PRINT '@MaxObservation: ' + CONVERT(NVARCHAR(MAX), @MaxObservation)
		PRINT '@NonNumericObservations: ' + CONVERT(NVARCHAR(MAX), @NonNumericObservations)

		IF @NumObservations > 0 
		BEGIN
			-- Sturges' formula for NumBins
			IF @AutoBins = 1
			BEGIN
				PRINT 'AutoBins: True'
				SET @NumBins = CAST(CEILING(LOG(@NumObservations, 2)) + 1 AS INT)		
			END
			PRINT 'NumBins: ' + CONVERT(NVARCHAR(MAX), @NumBins)

			-- Calculate BinWidth
			SET @BinWidth = (@MaxObservation - @MinObservation) / @NumBins
			PRINT 'MaxObservation: ' + CONVERT(NVARCHAR(MAX), @MaxObservation)
			PRINT 'MinObservation: ' + CONVERT(NVARCHAR(MAX), @MinObservation)
			PRINT 'BinWidth - Before Ceil:' + CONVERT(NVARCHAR(MAX), @BinWidth)
			IF @UseDiscreteBins = 1
			BEGIN
				SET @BinWidth = CEILING(@BinWidth)
				PRINT 'BinWidth: ' + CONVERT(NVARCHAR(MAX), @BinWidth)
			END

			SET @CurrentBin = 0
			WHILE @CurrentBin < @NumBins
			BEGIN
				PRINT '-------------------------'
				PRINT 'Current Bin: ' + CONVERT(NVARCHAR(MAX), @CurrentBin)

				-- Count Observatiosn in BIN
				SET @CurrentBinRangeStart = (@MinObservation + (@BinWidth * @CurrentBin))
				SET @ReportedBinRangeStart = @CurrentBinRangeStart 

				SET @CurrentBinRangeEnd = @CurrentBinRangeStart + @BinWidth
				SET @ReportedBinRangeEnd = @CurrentBinRangeEnd

				SET @BinEndRangeCompareOperator = '<'			

				IF @CurrentBin = 0
				BEGIN
					SET @CurrentBinRangeStart =  CAST('-1.79E+308' AS FLOAT) -- Min Float
				END			
								
				IF @CurrentBin = @NumBins - 1
				BEGIN
					SET @CurrentBinRangeEnd =  CAST('1.79E+308' AS FLOAT) -- Min Float
					SET @BinEndRangeCompareOperator = @BinEndRangeCompareOperator  + '='
				END

				SET @GetBinCountSQLParameterDef = 
					'@CurrentBinObservations INT OUTPUT, ' +
					'@CurrentBinRangeStart FLOAT, ' +
					'@CurrentBinRangeEnd FLOAT'

				SET @GetBinCountSQLStatement = 
					'SELECT @CurrentBinObservations = count(1) ' +
					'FROM ' + @DatasetTable + ' ' +
					'WHERE CAST(' + @ValueScript + ' AS FLOAT) >= @CurrentBinRangeStart ' + 
						' AND CAST(' + @ValueScript + ' AS FLOAT) ' + @BinEndRangeCompareOperator + ' @CurrentBinRangeEnd'

				IF @UseCategories = 1
				BEGIN
					SET @GetBinCountSQLStatement = @GetBinCountSQLStatement  + ' AND CONVERT(NVARCHAR(MAX), ' + @CategoryColumn + ') = ''' + @CurrentCategory  + ''''
				END

				PRINT 'Executing: ' + @GetBinCountSQLStatement
				PRINT 'Parameters: ' + @GetBinCountSQLParameterDef

				EXEC SP_EXECUTESQL  @GetBinCountSQLStatement,
					@GetBinCountSQLParameterDef,
					@CurrentBinObservations = @CurrentBinObservations OUTPUT,
					@CurrentBinRangeStart = @CurrentBinRangeStart,
					@CurrentBinRangeEnd = @CurrentBinRangeEnd					

				INSERT INTO #BinResults
					(BinNumber, BinRangeStart, BinRangeEnd, BinObservations, BinCategory)
				VALUES
					(@CurrentBin,  @ReportedBinRangeStart, @ReportedBinRangeEnd, @CurrentBinObservations, @CurrentCategory)

				PRINT 'Range Start: ' + CONVERT(NVARCHAR(MAX), @CurrentBinRangeStart)
				PRINT 'Range End: ' + CONVERT(NVARCHAR(MAX), @CurrentBinRangeEnd)
				PRINT 'Bin Observations: ' + CONVERT(NVARCHAR(MAX), @CurrentBinObservations)

				SET @CurrentBin = @CurrentBin + 1
			END
		END
		SET @CurrentCategoryIndex = @CurrentCategoryIndex + 1
	END

	SELECT * FROM  #BinResults
END

GO -- #1. Create  [cus].[SP_GetHistogram]  Stored Procedure

EXEC SP_DropColumn_With_Constraints '[dbo].[WorkerJobQueue]', 'TimeoutInSeconds'
ALTER TABLE [dbo].[WorkerJobQueue] ADD [TimeoutInSeconds] [INT] NOT NULL DEFAULT 3600
CREATE INDEX [IX_WorkerJobQueue_TimeoutInSeconds] ON [dbo].[WorkerJobQueue] ([TimeoutInSeconds])
GO -- #2. Add [TimeoutInSeconds] field to [WorkerJobQueue] table

DROP INDEX IF EXISTS [IX_WorkerJobQueue_RetryCount] ON [dbo].[WorkerJobQueue]
EXEC SP_DropColumn_With_Constraints '[dbo].[WorkerJobQueue]', 'RetryCount'
ALTER TABLE [dbo].[WorkerJobQueue] ADD [RetryCount] [INT] NOT NULL DEFAULT -1
CREATE INDEX [IX_WorkerJobQueue_RetryCount] ON [dbo].[WorkerJobQueue] ([RetryCount])
GO -- #3. Update Retry Count Default to -1

DROP PROCEDURE IF EXISTS [dbo].[SP_PopNextWorkerJobV2]

GO -- #4. Delete  [cus].[SP_PopNextWorkerJobV2]  Stored Procedure if it exists

CREATE PROCEDURE [dbo].[SP_PopNextWorkerJobV2]
	@queueName NVARCHAR(256),
	@timeThreshold INT
AS
	SET nocount ON;

	DECLARE @JobId int = -1;
	DECLARE @effectiveTimeThreshold int = 604800;
	DECLARE @keepAliveThreshold int = 300;

	IF @timeThreshold > 0
	BEGIN
		SET @effectiveTimeThreshold = @timeThreshold 
	END;

	WITH cte AS (
		SELECT TOP(1) * 
		FROM dbo.WorkerJobQueue as tsj WITH (ROWLOCK, READPAST)
		WHERE (
		    (QueueName = @queueName) AND 
			(
				(StartedTimestamp IS NULL) OR 
				(StartedTimestamp IS NOT NULL AND (DATEDIFF(second, KeepAliveTimestamp, GETDATE()) > @keepAliveThreshold) AND JobResult IS NULL)
			) AND 
			(TimeoutInSeconds <= @effectiveTimeThreshold)
		)
		ORDER BY CreatedTimestamp ASC)
	UPDATE cte SET 
		StartedTimestamp = GETDATE(),
		KeepAliveTimestamp = GETDATE(),
		RetryCount = RetryCount + 1,
		@JobId = JobId;

IF (@JobId >= 0)
	SELECT * FROM dbo.WorkerJobQueue WHERE JobId = @JobId 
ELSE
	SELECT -1 as JobId, NULL AS StartedTimestamp, NULL AS CreatedTimestamp, @queueName AS QueueName, NULL AS JobPayload, NULL AS JobResult, NULL AS ExecutionTime, NULL AS JobType, (SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)) AS UserId, GETDATE() AS [KeepAliveTimestamp], 0 AS [RetryCount], 0 AS [TimeoutInSeconds] 

GO -- #5. Update Retry Count Default to -1


EXEC SP_DropColumn_With_Constraints '[cus].[CustomSearchDefinition]', 'Validated'
ALTER TABLE [cus].[CustomSearchDefinition] ADD [Validated] [BIT] NULL
GO -- #6. Add [Validated] field to [CustomSearchDefinition] table

DROP INDEX IF EXISTS [IX_LayerSource_HasMobileSupport] ON [gis].[LayerSource]
EXEC SP_DropColumn_With_Constraints '[gis].[LayerSource]', 'HasMobileSupport'
ALTER TABLE [gis].[LayerSource] ADD [HasMobileSupport] [BIT] NOT NULL DEFAULT 0
CREATE INDEX [IX_LayerSource_HasMobileSupport] ON [gis].[LayerSource] ([HasMobileSupport])
GO -- #7. Add [HasMobileSupport] field to [LayerSource] table

DROP INDEX IF EXISTS [IX_LayerSource_HasOverlapSupport] ON [gis].[LayerSource]
EXEC SP_DropColumn_With_Constraints '[gis].[LayerSource]', 'HasOverlapSupport'
ALTER TABLE [gis].[LayerSource] ADD [HasOverlapSupport] [BIT] NOT NULL DEFAULT 0
CREATE INDEX [IX_LayerSource_HasOverlapSupport] ON [gis].[LayerSource] ([HasOverlapSupport])
GO -- #8. Add [HasOverlapSupport] field to [LayerSource] table

EXEC SP_DropColumn_With_Constraints '[gis].[LayerSource]', 'IsBlobPassthrough'
ALTER TABLE [gis].[LayerSource] ADD [IsBlobPassthrough] [BIT] NOT NULL DEFAULT 0
GO -- #9. Add [IsBlobPassthrough] field to [LayerSource] table

EXEC SP_DropColumn_With_Constraints '[gis].[LayerSource]', 'NativeMapBoxLayers'
ALTER TABLE [gis].[LayerSource] ADD [NativeMapBoxLayers] NVARCHAR(MAX) NULL
GO -- #10. Add [NativeMapBoxLayers] field to [LayerSource] table

UPDATE [cus].[CustomSearchDefinition] SET validated = 1 
GO -- #11. All previous custom searches are set to valid


EXEC SP_DropColumn_With_Constraints '[cus].[Dataset]', 'DbLockTime'
ALTER TABLE [cus].[Dataset] ADD [DbLockTime] DATETIME NULL
GO -- #12. Add [DbLockTime] field to [Dataset] table

Drop function if exists [cus].[FN_IsDatasetLocked]
GO -- #13. 

CREATE FUNCTION [cus].[FN_IsDatasetLocked]
(
	@dbLockType NVARCHAR(MAX),
    @dbLockTime DATETIME,
	@LockingJobId INT
)
RETURNS BIT
AS
BEGIN
    -- Declare the return variable here
    DECLARE @Result BIT = 0;
	DECLARE @keepAliveThreshold int = 300;

	IF @dbLockType IS NOT NULL AND @dbLockTime IS NOT NULL
	BEGIN
		IF @LockingJobId IS NULL OR @LockingJobId = -1
		BEGIN
			IF DATEDIFF(second, @dbLockTime, GETDATE()) < @keepAliveThreshold
			BEGIN
				SET @Result = 1;
			END
		END
		ELSE
		BEGIN
			DECLARE @workerKeepAliveTimestamp DATETIME = NULL;
			SELECT  @workerKeepAliveTimestamp = KeepAliveTimestamp FROM dbo.WorkerJobQueue WHERE JobId = @LockingJobId
			IF (@workerKeepAliveTimestamp IS NOT NULL AND
			    DATEDIFF(second,  @workerKeepAliveTimestamp, GETDATE()) < @keepAliveThreshold)
			BEGIN
				SET @Result = 1;
			END
		END
	END
    -- Return the result of the function
    RETURN(@Result)
END
GO -- #14. Create [FN_IsDatasetLocked] function

Drop function if exists [cus].[FN_IsDatasetRootLocked]
GO -- #15. Create [FN_IsDatasetLocked] function

CREATE FUNCTION [cus].[FN_IsDatasetRootLocked]
(
	@dbLockType NVARCHAR(MAX),
    @dbLockTime DATETIME,
	@LockingJobId INT
)
RETURNS BIT
AS
BEGIN
    -- Declare the return variable here
    DECLARE @Result BIT = cus.FN_IsDatasetLocked(@dbLockType, @dbLockTime, @LockingJobId);

	IF @Result = 1 AND ISNULL(@dbLockType, '') != 'RootLock'
	BEGIN
		SET @Result = 0
	END
    -- Return the result of the function
    RETURN(@Result)
END
GO -- #16. Create [[FN_IsDatasetRootLocked]] function

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
GO -- #17. Adding lock recovery to [SP_GetAlterDatasetLockV2]

ALTER PROCEDURE [cus].[SP_ReleaseAlterDatasetLockV2]
	@datasetId NVARCHAR(256),
    @newPostProcessState NVARCHAR(256),
	@newState NVARCHAR(256),
    @userId NVARCHAR(256)
AS
	set nocount on;

	WITH cte AS (
		SELECT TOP(1) * 
		FROM [cus].[Dataset] AS ds WITH (rowlock, readpast)
		WHERE (([DBLockType] IS NOT NULL) AND ([DatasetId] = @datasetId)))
	update cte SET 				
		DatasetPostProcessState = @newPostProcessState,
		DatasetState = @newState,
		DBLockType = NULL,
		DbLockTime = NULL,
		LockingJobId = NULL,
		LastModifiedTimestamp = GETUTCDATE(),
		LastModifiedBy = userId;

SELECT *
    FROM [cus].[Dataset]
    WHERE [DatasetId] = @DatasetId
GO -- #18. Adding lock recovery to [SP_ReleaseAlterDatasetLockV2]

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
							WHERE (([DatasetId] = @datasetId or [SourceDatasetId] = @datasetId) and 
							  cus.FN_IsDatasetLocked([DbLockType], [DbLockTime], [LockingJobId]) = 0)) unlockedDataset
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
				    WHERE (([DatasetId] = @datasetId and cus.FN_IsDatasetLocked([DbLockType], [DbLockTime], [LockingJobId]) = 0) 
					  or ([DatasetId] = @parentDatasetId  and cus.FN_IsDatasetRootLocked([DbLockType], [DbLockTime], [LockingJobId]) = 0))) unlockedDataset
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
	  ,[DbLockTime]
    FROM [cus].[Dataset]
    WHERE [DatasetId] = @resultDatasetId
GO -- #19. Adding lock recovery to [SP_TestAlterDatasetLockV2]









