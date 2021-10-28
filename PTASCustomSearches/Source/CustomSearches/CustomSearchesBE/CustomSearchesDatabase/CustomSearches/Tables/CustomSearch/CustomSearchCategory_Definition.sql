CREATE TABLE [cus].[CustomSearchCategory_Definition]
(
	[CustomSearchCategoryId] INT NOT NULL, 
    [CustomSearchDefinitionId] INT NOT NULL

	PRIMARY KEY ([CustomSearchCategoryId], [CustomSearchDefinitionId]),
	CONSTRAINT [FK_CustomSearchCategory_Definition_ToCustomSearchCategory] FOREIGN KEY ([CustomSearchCategoryId]) REFERENCES [cus].[CustomSearchCategory]([CustomSearchCategoryId]),
	CONSTRAINT [FK_CustomSearchCategory_Definition_ToCustomSearchDefinition] FOREIGN KEY ([CustomSearchDefinitionId]) REFERENCES [cus].[CustomSearchDefinition]([CustomSearchDefinitionId]),
)

GO