CREATE TABLE [gis].[UserMapSelection]
(
    [UserId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserMapId] INT NOT NULL

    CONSTRAINT [FK_UserMapSelection_ToUserMap] FOREIGN KEY ([UserMapId]) REFERENCES [gis].[UserMap]([UserMapId])    
)
GO
