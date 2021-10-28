CREATE TABLE [gis].[LayerSource]
(
    [LayerSourceId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LayerSourceName] NVARCHAR(256) NOT NULL,
    [LayerSourceAlias] NVARCHAR(256) NOT NULL,
    [Jurisdiction] NVARCHAR(256),
    [Organization] NVARCHAR(256),
    [Description] NVARCHAR(MAX),
	[IsParcelSource] BIT NOT NULL DEFAULT 0,
    [DefaultMinZoom] INT NOT NULL DEFAULT 10,
    [DefaultMaxZoom] INT NOT NULL DEFAULT 16,
    [DefaultLabelMinZoom] INT NOT NULL DEFAULT 10,
    [DefaultLabelMaxZoom] INT NOT NULL DEFAULT 16,
    [IsVectorLayer] bit NOT NULL,
    [DataSourceUrl] NVARCHAR(2048) NOT NULL,
    [TileSize] INT NOT NULL DEFAULT 256,
    [DefaultMapboxLayer] NVARCHAR(MAX) NOT NULL,
    [DefaultLabelMapboxLayer] NVARCHAR(MAX) NULL, 
    [NativeMapboxLayers] NVARCHAR(MAX) NOT NULL, 
    [HasOfflineSupport] BIT NOT NULL DEFAULT 0,
    [EmbeddedDataFields] NVARCHAR(MAX) NULL,
    [GisLayerName] NVARCHAR(MAX) NULL,
    [OgrLayerData] NVARCHAR(MAX) NULL,
    [Metadata] NVARCHAR(MAX) NULL,
    [HasMobileSupport] BIT NOT NULL DEFAULT 0,
    [HasOverlapSupport] BIT NOT NULL DEFAULT 0,
    [IsBlobPassThrough] BIT NOT NULL DEFAULT 0,
)
GO

CREATE INDEX [IX_LayerSource_GisLayerName] ON [gis].[LayerSource] ([LayerSourceId])
GO


CREATE INDEX [IX_LayerSource_HasOfflineSupport] ON [gis].[LayerSource] ([HasOfflineSupport])
GO

CREATE INDEX [IX_LayerSource_LayerSourceName] ON [gis].[LayerSource] ([LayerSourceName])
GO

CREATE INDEX [IX_LayerSource_LayerSourceAlias] ON [gis].[LayerSource] ([LayerSourceAlias])
GO

CREATE INDEX [IX_LayerSource_IsParcelSource] ON [gis].[LayerSource] ([IsParcelSource])
GO

CREATE INDEX [IX_LayerSource_IsVectorLayer] ON [gis].[LayerSource] ([IsVectorLayer])
GO

CREATE INDEX [IX_LayerSource_HasOverlapSupport] ON [gis].[LayerSource] ([HasOverlapSupport])
GO

CREATE INDEX [IX_LayerSource_HasMobileSupport] ON [gis].[LayerSource] ([HasMobileSupport])
GO