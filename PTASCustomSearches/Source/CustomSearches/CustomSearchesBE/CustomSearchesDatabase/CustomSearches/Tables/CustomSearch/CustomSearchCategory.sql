CREATE TABLE [cus].[CustomSearchCategory]
(
	[CustomSearchCategoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CategoryName] NVARCHAR(256) NOT NULL, 
    [CategoryDescription] NVARCHAR(2048) NOT NULL
)

GO

CREATE INDEX [IX_CustomSearchCategory_CategoryName] ON [cus].[CustomSearchCategory] ([CategoryName])

GO