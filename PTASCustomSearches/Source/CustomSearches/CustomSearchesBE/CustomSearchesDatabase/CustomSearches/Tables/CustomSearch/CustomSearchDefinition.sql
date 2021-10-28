CREATE TABLE [cus].[CustomSearchDefinition]
(
	[CustomSearchDefinitionId] INT NOT NULL PRIMARY KEY IDENTITY,
    [CustomSearchName] NVARCHAR(256) NOT NULL, 
    [CustomSearchDescription] NVARCHAR(2048) NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [StoredProcedureName] NVARCHAR(256) NOT NULL,
    [CreatedBy] UNIQUEIDENTIFIER NOT NULL, 
    [LastModifiedBy] UNIQUEIDENTIFIER NOT NULL,
    [CreatedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastModifiedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [Validated] BIT NULL

    CONSTRAINT [FK_CustomSearchDefinition_ToSystemUser_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
    CONSTRAINT [FK_CustomSearchDefinition_ToSystemUser_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid])
)
GO

ALTER TABLE [cus].[CustomSearchDefinition] NOCHECK CONSTRAINT [FK_CustomSearchDefinition_ToSystemUser_CreatedBy]
GO

ALTER TABLE [cus].[CustomSearchDefinition] NOCHECK CONSTRAINT [FK_CustomSearchDefinition_ToSystemUser_LastModifiedBy]
GO

CREATE INDEX [IX_CustomSearchDefinition_CustomSearchName] ON [cus].[CustomSearchDefinition] ([CustomSearchName])

GO