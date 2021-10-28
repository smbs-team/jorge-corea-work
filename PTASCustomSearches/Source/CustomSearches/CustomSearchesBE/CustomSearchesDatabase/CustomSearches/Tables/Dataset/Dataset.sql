CREATE TABLE [cus].[Dataset]
(
    [DatasetId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [SourceDatasetId] UNIQUEIDENTIFIER DEFAULT NULL, 
    [CustomSearchDefinitionId] INT NOT NULL, 
	[UserId] UNIQUEIDENTIFIER NOT NULL, 
    [ParentFolderId] INT DEFAULT NULL,
    [DatasetName] NVARCHAR(256) NOT NULL, 
    [ParameterValues] NVARCHAR(MAX),
    [DatasetClientState] NVARCHAR(MAX),
    [TotalRows] INT NOT NULL DEFAULT 0,
    [GeneratedTableName] NVARCHAR(256) NOT NULL,
    [GenerateSchemaElapsedMs] INT NOT NULL, 
    [GenerateIndexesElapsedMs] INT NOT NULL, 
    [ExecuteStoreProcedureElapsedMs] INT NOT NULL,      
    [DbLockType] NVARCHAR(256),    
    [DbLockTime] DATETIME NULL,
    [IsLocked] BIT NOT NULL DEFAULT 0,    
    [IsDataLocked] BIT NOT NULL DEFAULT 0,    
    [DatasetState] NVARCHAR(256) NOT NULL DEFAULT 'NotProcessed',
    [DatasetPostProcessState] NVARCHAR(256) NOT NULL DEFAULT 'NotProcessed',
    [LockingJobId] INT,
    [Comments] NVARCHAR(MAX),
    [CreatedBy] UNIQUEIDENTIFIER NOT NULL, 
    [LastModifiedBy] UNIQUEIDENTIFIER NOT NULL,
    [LastExecutedBy] UNIQUEIDENTIFIER NOT NULL,
    [CreatedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastModifiedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastExecutionTimeStamp] DATETIME

    CONSTRAINT [FK_Dataset_ToDataset] FOREIGN KEY ([SourceDatasetId]) REFERENCES [cus].[Dataset]([DatasetId]),
    CONSTRAINT [FK_Dataset_Definition_ToCustomSearchDefinition] FOREIGN KEY ([CustomSearchDefinitionId]) REFERENCES [cus].[CustomSearchDefinition]([CustomSearchDefinitionId]),
    CONSTRAINT [FK_Dataset_ToParentFolderId] FOREIGN KEY ([ParentFolderId]) REFERENCES [cus].[Folder]([FolderId]),
    CONSTRAINT [FK_Dataset_ToDatasetState] FOREIGN KEY ([DatasetState]) REFERENCES [cus].[DatasetState]([DatasetState]),
    CONSTRAINT [FK_Dataset_ToDbLockType] FOREIGN KEY ([DbLockType]) REFERENCES [cus].[DbLockType]([DbLockType]),
    CONSTRAINT [FK_Dataset_ToDatasetPostProcessState] FOREIGN KEY ([DatasetPostProcessState]) REFERENCES [cus].[DatasetPostProcessState]([DatasetPostProcessState]),

    CONSTRAINT [FK_Dataset_ToSystemUser_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
    CONSTRAINT [FK_Dataset_ToSystemUser_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
    CONSTRAINT [FK_Dataset_ToSystemUser_LastExecutedBy] FOREIGN KEY ([LastExecutedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
    CONSTRAINT [FK_Dataset_ToSystemUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [dynamics].[SystemUser]([systemuserid])
)
GO

ALTER TABLE [cus].[Dataset] NOCHECK CONSTRAINT [FK_Dataset_ToSystemUser_CreatedBy]
GO

ALTER TABLE [cus].[Dataset] NOCHECK CONSTRAINT [FK_Dataset_ToSystemUser_LastModifiedBy]
GO

ALTER TABLE [cus].[Dataset] NOCHECK CONSTRAINT [FK_Dataset_ToSystemUser_UserId]
GO

CREATE INDEX [IX_Dataset_UserId] ON [cus].[Dataset] ([UserId])
GO

CREATE INDEX [IX_Dataset_DatasetName] ON [cus].[Dataset] ([DatasetName])
GO


CREATE INDEX [IX_Dataset_ParentFolderId] ON [cus].[Dataset] ([ParentFolderId])
GO

CREATE INDEX [IX_Dataset_CustomSearchDefinitionId] ON [cus].[Dataset] ([CustomSearchDefinitionId])
GO

CREATE INDEX [IX_Dataset_CreatedTimestamp] ON [cus].[Dataset] ([CreatedTimestamp])
GO




