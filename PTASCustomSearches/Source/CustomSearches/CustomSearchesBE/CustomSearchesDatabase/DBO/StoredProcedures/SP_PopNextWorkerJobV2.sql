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
	END

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
		RetryCount = RetryCount + 1,
		@JobId = JobId;

IF (@JobId >= 0)
	SELECT * FROM dbo.WorkerJobQueue WHERE JobId = @JobId 
ELSE
	SELECT -1 as JobId, NULL AS StartedTimestamp, NULL AS CreatedTimestamp, @queueName AS QueueName, NULL AS JobPayload, NULL AS JobResult, NULL AS ExecutionTime, NULL AS JobType, (SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)) AS UserId, GETDATE() AS [KeepAliveTimestamp], 0 AS [RetryCount], 0 AS [TimeoutInSeconds] 
GO
