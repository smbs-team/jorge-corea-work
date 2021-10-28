CREATE TABLE [cus].[CustomSearchBackendEntity]
(
	[CustomSearchBackendEntityId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomSearchDefinitionId] INT NOT NULL,     
    [BackendEntityName] NVARCHAR(4000),
    [BackendEntityKeyFieldName] NVARCHAR(4000),
    [CustomSearchKeyFieldName] NVARCHAR(256),

    CONSTRAINT [FK_CustomSearchBackendEntity_ToCustomSearchDefinition] FOREIGN KEY ([CustomSearchDefinitionId]) REFERENCES [cus].[CustomSearchDefinition]([CustomSearchDefinitionId]),
)
GO

CREATE INDEX [IX_CustomSearchBackendEntity_CustomSearchDefinitionId] ON [cus].[CustomSearchBackendEntity] ([CustomSearchDefinitionId])
GO

CREATE INDEX [IX_CustomSearchBackendEntity_CustomSearchKeyFieldName] ON [cus].[CustomSearchBackendEntity] ([CustomSearchKeyFieldName])
GO
