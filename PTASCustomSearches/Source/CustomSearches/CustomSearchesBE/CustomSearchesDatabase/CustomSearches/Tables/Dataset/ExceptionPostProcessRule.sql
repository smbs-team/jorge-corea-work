CREATE TABLE [cus].[ExceptionPostProcessRule]
(
	[ExceptionPostProcessRuleId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DatasetPostProcessId] INT NOT NULL,    
    [GroupName] NVARCHAR(256),
    [Description] NVARCHAR(4000) NOT NULL,
    [ExecutionOrder] INT NOT NULL DEFAULT 0

    CONSTRAINT [FK_ExceptionPostProcessRule_ToDatasetPostProcess] FOREIGN KEY ([DatasetPostProcessId]) REFERENCES [cus].[DatasetPostProcess]([DatasetPostProcessId]),
)
GO

CREATE INDEX [IX_ExceptionPostProcessRulet_DatasetPostProcessId] ON [cus].[ExceptionPostProcessRule] ([DatasetPostProcessId])
GO