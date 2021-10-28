CREATE TABLE [cus].[ProjectType_CustomSearchDefinition]
(
	[ProjectTypeId] INT NOT NULL, 
	[DatasetRole] NVARCHAR(256) NOT NULL DEFAULT 'Population',
    [CustomSearchDefinitionId] INT NOT NULL,

	PRIMARY KEY ([ProjectTypeId], [DatasetRole]),
	CONSTRAINT [FK_ProjectType_CustomSearchDefinition_ToProjectType] FOREIGN KEY ([ProjectTypeId]) REFERENCES [cus].[ProjectType]([ProjectTypeId]),
	CONSTRAINT [FK_ProjectType_CustomSearchDefinition_ToCustomSearchDefinition] FOREIGN KEY ([CustomSearchDefinitionId]) REFERENCES [cus].[CustomSearchDefinition]([CustomSearchDefinitionId]),
)

GO