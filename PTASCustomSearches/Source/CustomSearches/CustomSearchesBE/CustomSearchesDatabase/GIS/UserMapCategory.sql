CREATE TABLE [gis].[UserMapCategory]
(
	[UserMapCategoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CategoryName] NVARCHAR(256) NOT NULL, 
    [CategoryDescription] NVARCHAR(2048) NOT NULL
)

GO

CREATE INDEX [IX_UserMapCategory_CategoryName] ON [gis].[UserMapCategory] ([CategoryName])

GO