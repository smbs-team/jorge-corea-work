CREATE PROCEDURE [dbo].[SP_PopNextWorkerJob]
	@queueName NVARCHAR(256)
AS
	set nocount on;

	DECLARE @JobId int = -1;

	with cte as (
		select top(1) * 
		from dbo.WorkerJobQueue as tsj with (rowlock, readpast)
		where ((QueueName = @queueName) AND ((StartedTimestamp is NULL)))
		order by CreatedTimestamp ASC)
	update cte SET 
		StartedTimestamp = GETDATE(),
		@JobId = JobId;

IF (@JobId >= 0)
	Select * from dbo.WorkerJobQueue where JobId = @JobId 
ELSE
	Select -1 as JobId, NULL AS StartedTimestamp, NULL AS CreatedTimestamp, @queueName AS QueueName, NULL as JobPayload, NULL as JobResult, NULL AS ExecutionTime, NULL as JobType, (SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)) as UserId 
GO
