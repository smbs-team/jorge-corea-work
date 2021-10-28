CREATE TABLE [cus].[CustomSearchParameter]
(
	[CustomSearchParameterId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OwnerType] NVARCHAR(256) DEFAULT 'CustomSearchDefinition',
    [CustomSearchDefinitionId] INT DEFAULT NULL, 
    [RScriptModelId] INT DEFAULT NULL, 
    [ParameterGroupName] NVARCHAR(256) NOT NULL, 
    [ParameterName] NVARCHAR(256) NOT NULL, 
    [ParameterDescription] NVARCHAR(2048) NOT NULL, 
    [ParameterDataType] NVARCHAR(256) NOT NULL DEFAULT 'int', 
    [ParameterRangeType] NVARCHAR(256) NOT NULL DEFAULT 'NotRange', 
    [ParameterTypeLength] INT NOT NULL DEFAULT 0, 
    [ParameterDefaultValue] NVARCHAR(MAX) NULL DEFAULT '', 
    [ParameterIsRequired] [bit] NOT NULL DEFAULT 0,
    [ForceEditLookupExpression] [bit] NOT NULL DEFAULT 0,
    [AllowMultipleSelection] [bit] NOT NULL DEFAULT 0,

    CONSTRAINT [FK_CustomSearchParameter_ToCustomSearchDefinition] FOREIGN KEY ([CustomSearchDefinitionId]) REFERENCES [cus].[CustomSearchDefinition]([CustomSearchDefinitionId]),
    CONSTRAINT [FK_CustomSearchParameter_ToRScriptModel] FOREIGN KEY ([RScriptModelId]) REFERENCES [cus].[RScriptModel]([RScriptModelId]),
    CONSTRAINT [FK_CustomSearchParameter_ToDataType] FOREIGN KEY ([ParameterDataType]) REFERENCES [cus].[DataType]([DataType]),    
    CONSTRAINT [FK_CustomSearchParameter_ToRangeType] FOREIGN KEY ([ParameterRangeType]) REFERENCES [cus].[RangeType]([RangeType]),    
    CONSTRAINT [FK_CustomSearchParameter_ToOwnerType] FOREIGN KEY ([OwnerType]) REFERENCES [cus].[OwnerType]([OwnerType])

)
GO

CREATE INDEX [IX_CustomSearchParameter_CustomSearchDefinitionId] ON [cus].[CustomSearchParameter] ([CustomSearchDefinitionId])
GO

CREATE INDEX [IX_CustomSearchParameter_ParameterName] ON [cus].[CustomSearchParameter] ([ParameterName])
GO
