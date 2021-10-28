CREATE TABLE [gis].[MapRendererCategory_MapRenderer]
(
	[MapRendererCategoryId] INT NOT NULL, 
    [MapRendererId] INT NOT NULL

	PRIMARY KEY ([MapRendererCategoryId], [MapRendererId]),
	CONSTRAINT [FK_MapRendererCategory_MapRenderer_ToMapRendererCategory] FOREIGN KEY ([MapRendererCategoryId]) REFERENCES [gis].[MapRendererCategory]([MapRendererCategoryId]),
	CONSTRAINT [FK_MapRendererCategory_MapRenderer_ToMapRenderer] FOREIGN KEY ([MapRendererId]) REFERENCES [gis].[MapRenderer]([MapRendererId]),
)

GO