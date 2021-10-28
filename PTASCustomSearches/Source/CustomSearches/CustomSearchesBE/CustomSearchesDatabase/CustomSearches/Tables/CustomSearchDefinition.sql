CREATE TABLE [cus].[CustomSearchDefinition]
(
	[CustomSearchDefinitionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomSearchName] NVARCHAR(256) NOT NULL, 
    [CustomSearchDescription] NVARCHAR(2048) NOT NULL, 
    [StoredProcedureName] NVARCHAR(256) NOT NULL    
)

GO

CREATE INDEX [IX_CustomSearchDefinition_CustomSearchName] ON [cus].[CustomSearchDefinition] ([CustomSearchName])

GO