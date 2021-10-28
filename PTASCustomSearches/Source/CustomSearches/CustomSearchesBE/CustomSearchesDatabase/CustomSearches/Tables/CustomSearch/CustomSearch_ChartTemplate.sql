CREATE TABLE [cus].[CustomSearch_ChartTemplate]
(
	[ChartTemplateId] INT NOT NULL,     
    [CustomSearchDefinitionId] INT NOT NULL,

    PRIMARY KEY ([ChartTemplateId], [CustomSearchDefinitionId]),
    CONSTRAINT [FK_CustomSearch_ChartTemplate_To_CustomSearchDefinition] FOREIGN KEY ([CustomSearchDefinitionId]) REFERENCES [cus].[CustomSearchDefinition]([CustomSearchDefinitionId]),
    CONSTRAINT [FK_CustomSearch_ChartTemplate_To_ChartTemplate] FOREIGN KEY ([ChartTemplateId]) REFERENCES [cus].[ChartTemplate]([ChartTemplateId])
)
GO
