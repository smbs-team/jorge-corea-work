CREATE TABLE [cus].[DatasetPostProcess]
(
    [DatasetPostProcessId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PostProcessName] NVARCHAR(256) NOT NULL DEFAULT '',
    [DatasetId] UNIQUEIDENTIFIER NOT NULL,    	
    [ParameterValues] NVARCHAR(MAX) DEFAULT NULL,
    [Priority] INT NOT NULL DEFAULT 0,
    [PostProcessType] NVARCHAR(256) NOT NULL, 
    [PostProcessRole] NVARCHAR(256), 
    [PostProcessSubType] NVARCHAR(256) NOT NULL, 
    [RScriptModelId] INT,
    [PostProcessDefinition] NVARCHAR(MAX),
    [ResultPayload] NVARCHAR(MAX),
    [TraceEnabledFields] NVARCHAR(MAX),
    [CreatedBy] UNIQUEIDENTIFIER NOT NULL, 
    [LastModifiedBy] UNIQUEIDENTIFIER NOT NULL,
    [CreatedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastModifiedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastExecutionTimeStamp] DATETIME,
    [IsDirty] BIT NOT NULL DEFAULT 0,
    [CalculatedView] NVARCHAR(MAX),
    [ExecutionOrder] INT NOT NULL DEFAULT 0

    CONSTRAINT [FK_DatasetPostProcess_ToDataset] FOREIGN KEY ([DatasetId]) REFERENCES [cus].[Dataset]([DatasetId]),
    CONSTRAINT [FK_DatasetPostProcess_ToRScriptModel] FOREIGN KEY ([RScriptModelId]) REFERENCES [cus].[RScriptModel]([RScriptModelId]),

    CONSTRAINT [FK_DatasetPostProcess_ToSystemUser_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
    CONSTRAINT [FK_DatasetPostProcess_ToSystemUser_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid])
)
GO

ALTER TABLE [cus].[DatasetPostProcess] NOCHECK CONSTRAINT [FK_DatasetPostProcess_ToSystemUser_CreatedBy]
GO

ALTER TABLE [cus].[DatasetPostProcess] NOCHECK CONSTRAINT [FK_DatasetPostProcess_ToSystemUser_LastModifiedBy]
GO


CREATE INDEX [IX_DatasetPostProcess_PostProcessName] ON [cus].[DatasetPostProcess] ([PostProcessName])
GO

CREATE INDEX [IX_DatasetPostProcess_Priority] ON [cus].[DatasetPostProcess] ([Priority])
GO

CREATE INDEX [IX_DatasetPostProcess_PostProcessType] ON [cus].[DatasetPostProcess] ([PostProcessType])
GO

CREATE INDEX [IX_DatasetPostProcess_PostProcessSubType] ON [cus].[DatasetPostProcess] ([PostProcessSubType])
GO
