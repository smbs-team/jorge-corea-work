CREATE TABLE [gis].[TileStorageJobType]
(
	[JobTypeId] INT NOT NULL PRIMARY KEY, 
    [SourceLocation] VARCHAR(MAX) NOT NULL, 
    [JobFormat] INT NOT NULL, 
    [TargetLocation] VARCHAR(MAX) NOT NULL, 
    [MaxTimeInSeconds] INT NOT NULL DEFAULT 86400
)
