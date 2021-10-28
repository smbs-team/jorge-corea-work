CREATE TABLE [gis].[TileStorageJobQueue]
(
	[JobId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [JobTypeId] INT NOT NULL, 
    [StartedTimestamp] DATETIME NULL, 
    [CreatedTimestamp] DATETIME NOT NULL DEFAULT GETDATE() , 
    CONSTRAINT [FK_TileStorageJobQueue_ToJobType] FOREIGN KEY ([JobTypeId]) REFERENCES [gis].[TileStorageJobType]([JobTypeId])
)

GO

CREATE INDEX [IX_TileStorageJobQueue_CreatedTimestamp] ON [gis].[TileStorageJobQueue] ([CreatedTimestamp])

GO

CREATE INDEX [IX_TileStorageJobQueue_StartedTimestamp] ON [gis].[TileStorageJobQueue] ([StartedTimestamp])

GO