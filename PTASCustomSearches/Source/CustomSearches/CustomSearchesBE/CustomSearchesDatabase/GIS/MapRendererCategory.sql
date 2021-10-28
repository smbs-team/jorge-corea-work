CREATE TABLE [gis].[MapRendererCategory]
(
	[MapRendererCategoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CategoryName] NVARCHAR(256) NOT NULL, 
    [CategoryDescription] NVARCHAR(2048) NOT NULL
)

GO

CREATE INDEX [IX_MapRendererCategory_CategoryName] ON [gis].[MapRendererCategory] ([CategoryName])

GO