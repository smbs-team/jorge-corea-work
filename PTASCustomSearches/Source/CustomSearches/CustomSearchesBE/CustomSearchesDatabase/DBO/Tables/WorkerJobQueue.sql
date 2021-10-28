CREATE TABLE [dbo].[WorkerJobQueue]
(
	[JobId] INT NOT NULL PRIMARY KEY IDENTITY,     
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [StartedTimestamp] DATETIME NULL, 
    [CreatedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [QueueName] NVARCHAR(256) NOT NULL,
    [JobType] NVARCHAR(256) NOT NULL,
    [JobPayLoad] NVARCHAR(MAX) NULL,
    [JobResult] NVARCHAR(MAX) NULL,
    [ExecutionTime] NUMERIC NULL,
    [TimeOutInSeconds] INT DEFAULT 3600,
    [RetryCount] INT DEFAULT -1,
    [KeepAliveTimestamp] DATETIME DEFAULT GETDATE()
)

GO

CREATE INDEX [IX_WorkerJobQueue_CreatedTimestamp] ON [dbo].[WorkerJobQueue] ([CreatedTimestamp])

GO

CREATE INDEX [IX_RWorkerJobQueue_StartedTimestamp] ON [dbo].[WorkerJobQueue] ([StartedTimestamp])

GO

CREATE INDEX [IX_WorkerJobQueue_QueueName] ON [dbo].[WorkerJobQueue] ([QueueName])

GO

CREATE INDEX [IX_WorkerJobQueue_UserId] ON [dbo].[WorkerJobQueue] ([UserId])

GO

