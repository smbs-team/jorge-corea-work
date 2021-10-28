CREATE TABLE [cus].[CustomSearchColumnDefinition]
(
	[CustomSearchColumnDefinitionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomSearchDefinitionId] INT NOT NULL,     
    [ColumnName] NVARCHAR(256),
    [ColumnType] NVARCHAR(256),
    [ColumnCategory] NVARCHAR(256),
    [ColumnTypeLength] INT NOT NULL DEFAULT 0,
    [CanBeUsedAsLookup] BIT DEFAULT 0,
    [IsEditable] BIT NOT NULL DEFAULT 0,
    [BackendEntityName] NVARCHAR(4000),
    [BackendEntityFieldName] NVARCHAR(4000),
    [ForceEditLookupExpression] BIT NOT NULL DEFAULT 0

    CONSTRAINT [FK_CustomSearchColumnDefinition_ToCustomSearchDefinition] FOREIGN KEY ([CustomSearchDefinitionId]) REFERENCES [cus].[CustomSearchDefinition]([CustomSearchDefinitionId]),
    CONSTRAINT [FK_CustomSearchColumnDefinition_ToColumnType] FOREIGN KEY ([ColumnType]) REFERENCES [cus].[DataType]([DataType]),
)
GO

CREATE INDEX [IX_CustomSearchColumnDefinition_CustomSearchDefinitionId] ON [cus].[CustomSearchColumnDefinition] ([CustomSearchDefinitionId])
GO

CREATE INDEX [IX_CustomSearchColumnDefinition_ColumnType] ON [cus].[CustomSearchColumnDefinition] ([ColumnType])
GO
