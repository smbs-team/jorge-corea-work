CREATE TABLE [cus].[CustomSearchValidationRule]
(
	[CustomSearchValidationRuleId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomSearchDefinitionId] INT NOT NULL,    
    [Description] NVARCHAR(4000) NOT NULL,
    [ExecutionOrder] INT NOT NULL DEFAULT 0

    CONSTRAINT [FK_CustomSearchValidationRule_ToCustomSearch] FOREIGN KEY ([CustomSearchDefinitionId]) REFERENCES [cus].[CustomSearchDefinition]([CustomSearchDefinitionId])
)
GO

CREATE INDEX [IX_CustomSearchValidationRule_CustomSearchDefinitionId] ON [cus].[CustomSearchDefinition] ([CustomSearchDefinitionId])
GO