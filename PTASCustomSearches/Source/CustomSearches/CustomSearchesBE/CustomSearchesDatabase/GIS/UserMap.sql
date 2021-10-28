CREATE TABLE [gis].[UserMap]
(
    [UserMapId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserMapName] NVARCHAR(256) NOT NULL, 
	[CreatedBy] UNIQUEIDENTIFIER NOT NULL, 
    [LastModifiedBy] UNIQUEIDENTIFIER NOT NULL, 
    [ParentFolderId] INT NOT NULL,
    [IsLocked] bit NOT NULL DEFAULT 0,  
    [CreatedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastModifiedTimestamp] DATETIME NOT NULL DEFAULT GETDATE()    

    CONSTRAINT [FK_UserMap_ToFolder] FOREIGN KEY ([ParentFolderId]) REFERENCES [gis].[Folder]([FolderId]),

    CONSTRAINT [FK_UserMap_ToSystemUser_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
    CONSTRAINT [FK_UserMap_ToSystemUser_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid])
)
GO

ALTER TABLE [gis].[UserMap] NOCHECK CONSTRAINT [FK_UserMap_ToSystemUser_CreatedBy]
GO

ALTER TABLE [gis].[UserMap] NOCHECK CONSTRAINT [FK_UserMap_ToSystemUser_LastModifiedBy]
GO


CREATE INDEX [IX_UserMap_UserMapName] ON [gis].[UserMap] ([UserMapName])
GO

CREATE INDEX [IX_UserMap_ParentFolderId] ON [gis].[UserMap] ([ParentFolderId])
GO

CREATE INDEX [IX_UserMap_CreatedBy] ON [gis].[UserMap] ([CreatedBy])
GO



