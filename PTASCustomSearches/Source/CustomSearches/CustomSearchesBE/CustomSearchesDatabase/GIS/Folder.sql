CREATE TABLE [gis].[Folder]
(
	[FolderId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ParentFolderId] INT,
    [UserId] UNIQUEIDENTIFIER, 
    [FolderName] NVARCHAR(256) NOT NULL,
    [FolderPath] NVARCHAR(MAX) NOT NULL,
    [FolderType] NVARCHAR(256) NOT NULL Default 'User',
    [FolderItemType] NVARCHAR(256) NOT NULL Default 'UserMap'

    CONSTRAINT [FK_Folder_ToFolder] FOREIGN KEY ([ParentFolderId]) REFERENCES [gis].[Folder]([FolderId]),
    CONSTRAINT [FK_Folder_ToFolderType] FOREIGN KEY ([FolderType]) REFERENCES [gis].[FolderType]([FolderType]),
    CONSTRAINT [FK_Folder_ToFolderItemType] FOREIGN KEY ([FolderItemType]) REFERENCES [gis].[FolderItemType]([FolderItemType]),

    CONSTRAINT [FK_Folder_ToSystemUser] FOREIGN KEY ([UserId]) REFERENCES [dynamics].[SystemUser]([systemuserid])
)
GO
ALTER TABLE [gis].[Folder] NOCHECK CONSTRAINT [FK_Folder_ToSystemUser]
GO

CREATE INDEX [IX_Folder_ParentFolderId] ON [gis].[Folder] ([ParentFolderId])
GO

CREATE INDEX [IX_Folder_UserId] ON [gis].[Folder] ([UserId])
GO

CREATE INDEX [IX_Folder_FolderName] ON [gis].[Folder] ([FolderName])
GO

CREATE INDEX [IX_Folder_FolderType] ON [gis].[Folder] ([FolderType])
GO