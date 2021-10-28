CREATE TABLE [dbo].[MetadataStoreItem]
(
	[MetadataStoreItemId] INT NOT NULL IDENTITY,     
	[Version] INT NOT NULL DEFAULT -1,
	[StoreType] VARCHAR(36) NOT NULL,
	[ItemName] NVARCHAR(64) NOT NULL DEFAULT '',
	[Value] NVARCHAR(MAX) NULL,

	PRIMARY KEY ([StoreType], [ItemName], [Version])
)

GO