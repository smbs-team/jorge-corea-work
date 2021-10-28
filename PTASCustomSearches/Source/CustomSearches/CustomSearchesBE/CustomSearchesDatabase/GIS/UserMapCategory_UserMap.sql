CREATE TABLE [gis].[UserMapCategory_UserMap]
(
	[UserMapCategoryId] INT NOT NULL, 
    [UserMapId] INT NOT NULL

	PRIMARY KEY ([UserMapCategoryId], [UserMapId]),
	CONSTRAINT [FK_UserMapCategory_UserMap_ToUserMapCategory] FOREIGN KEY ([UserMapCategoryId]) REFERENCES [gis].[UserMapCategory]([UserMapCategoryId]),
	CONSTRAINT [FK_UserMapCategory_UserMap_ToUserMap] FOREIGN KEY ([UserMapId]) REFERENCES [gis].[UserMap]([UserMapId]),
)

GO