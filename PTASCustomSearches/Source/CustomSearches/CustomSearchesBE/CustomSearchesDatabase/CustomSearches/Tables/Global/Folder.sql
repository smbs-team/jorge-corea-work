CREATE TABLE [cus].[Folder]
(
	[FolderId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ParentFolderId] INT,
    [UserId] UNIQUEIDENTIFIER, 
    [FolderType] NVARCHAR(256) NOT NULL DEFAULT 'User', 
    [FolderName] NVARCHAR(256) NOT NULL

    CONSTRAINT [FK_Folder_ToFolder] FOREIGN KEY ([ParentFolderId]) REFERENCES [cus].[Folder]([FolderId]),
    CONSTRAINT [FK_Folder_ToFolderType] FOREIGN KEY ([FolderType]) REFERENCES [cus].[FolderType]([FolderType]),

    CONSTRAINT [FK_Folder_ToSystemUser] FOREIGN KEY ([UserId]) REFERENCES [dynamics].[SystemUser]([systemuserid])
)
GO

ALTER TABLE [cus].[Folder] NOCHECK CONSTRAINT [FK_Folder_ToSystemUser]
GO

CREATE INDEX [IX_Folder_ParentFolderId] ON [cus].[Folder] ([ParentFolderId])
GO

CREATE INDEX [IX_Folder_ParentFolderType] ON [cus].[Folder] ([FolderType])
GO

CREATE INDEX [IX_Folder_UserId] ON [cus].[Folder] ([UserId])
GO

CREATE INDEX [IX_Folder_FolderName] ON [cus].[Folder] ([FolderName])
GO