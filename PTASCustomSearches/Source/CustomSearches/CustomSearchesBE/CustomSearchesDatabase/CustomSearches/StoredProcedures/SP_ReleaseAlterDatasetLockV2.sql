CREATE PROCEDURE [cus].[SP_ReleaseAlterDatasetLockV2]
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