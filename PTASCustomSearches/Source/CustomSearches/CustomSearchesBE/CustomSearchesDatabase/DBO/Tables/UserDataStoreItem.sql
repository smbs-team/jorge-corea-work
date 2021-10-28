CREATE TABLE [dbo].[UserDataStoreItem]
(
	[UserDataStoreItemId] INT NOT NULL IDENTITY,     
    [UserId] UNIQUEIDENTIFIER NOT NULL,
	[StoreType] VARCHAR(36) NOT NULL,
	[OwnerType] NVARCHAR(256) NOT NULL DEFAULT 'NoOwnerType',
	[OwnerObjectId] VARCHAR(36) NOT NULL DEFAULT '',	
	[ItemName] NVARCHAR(64) NOT NULL DEFAULT '',
	[Value] NVARCHAR(MAX) NULL,

	PRIMARY KEY ([UserId], [StoreType], [OwnerType], [OwnerObjectId], [ItemName]),
	CONSTRAINT [FK_UserDataStoreItem_ToSystemUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
	CONSTRAINT [FK_UserDataStoreItem_ToOwnerType_OwnerType] FOREIGN KEY ([OwnerType]) REFERENCES [cus].[OwnerType]([OwnerType]),
)

GO


CREATE INDEX [IX_UserDataStoreItem_Secondary] ON [dbo].[UserDataStoreItem] ([UserId], [StoreType], [OwnerType], [OwnerObjectId])

GO
