CREATE PROCEDURE [cus].[SP_ReleaseAlterDatasetLock]
	@datasetId NVARCHAR(256),
    @newPostProcessState NVARCHAR(256),
    @userId NVARCHAR(256)
AS
	set nocount on;

	WITH cte AS (
		SELECT TOP(1) * 
		FROM [cus].[Dataset] AS ds WITH (rowlock, readpast)
		WHERE (([DatasetPostProcessState] = 'Processing') AND ([DatasetId] = @datasetId)))
	update cte SET 				
		DatasetPostProcessState = @newPostProcessState,
		LockingJobId = NULL,
		LastModifiedTimestamp = GETUTCDATE(),
		LastModifiedBy = userId;

SELECT *
    FROM [cus].[Dataset]
    WHERE [DatasetId] = @DatasetId