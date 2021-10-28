CREATE TABLE [gis].[MapRenderer]
(
    [MapRendererId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserMapId] INT NOT NULL,
    [LayerSourceId] INT NOT NULL,
    [MapRendererName] NVARCHAR(256) NOT NULL, 
	[CreatedBy] UNIQUEIDENTIFIER NOT NULL, 
    [LastModifiedBy] UNIQUEIDENTIFIER NOT NULL,     
    [HasLabelRenderer] bit NOT NULL DEFAULT 0,
    [DatasetId] UNIQUEIDENTIFIER NULL,
    [MapRendererType] NVARCHAR(256) NOT NULL DEFAULT 'Parcel',
    [MapRendererLogicType] NVARCHAR(256) NOT NULL DEFAULT 'Simple',
    [RendererRules] NVARCHAR(MAX) NOT NULL,
    [CreatedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastModifiedTimestamp] DATETIME NOT NULL DEFAULT GETDATE()

    CONSTRAINT [FK_MapRenderer_ToUserMap] FOREIGN KEY ([UserMapId]) REFERENCES [gis].[UserMap]([UsermapId]),
    CONSTRAINT [FK_MapRenderer_ToLayerSource] FOREIGN KEY ([LayerSourceId]) REFERENCES [gis].[LayerSource]([LayerSourceId]),
    CONSTRAINT [FK_MapRenderer_ToMapRendererType] FOREIGN KEY ([MapRendererType]) REFERENCES [gis].[MapRendererType]([MapRendererType]),
    CONSTRAINT [FK_MapRenderer_ToMapRendererLogicType] FOREIGN KEY ([MapRendererLogicType]) REFERENCES [gis].[MapRendererLogicType]([MapRendererLogicType]),

    CONSTRAINT [FK_MapRenderer_ToSystemUser_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
    CONSTRAINT [FK_MapRenderer_ToSystemUser_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid])
)
GO

ALTER TABLE [gis].[MapRenderer] NOCHECK CONSTRAINT [FK_MapRenderer_ToSystemUser_CreatedBy]
GO

ALTER TABLE [gis].[MapRenderer] NOCHECK CONSTRAINT [FK_MapRenderer_ToSystemUser_LastModifiedBy]
GO

CREATE INDEX [IX_MapRenderer_UserMapId] ON [gis].[MapRenderer] ([UserMapId])
GO

CREATE INDEX [IX_MapRenderer_LayerSourceId] ON [gis].[MapRenderer] ([LayerSourceId])
GO


CREATE INDEX [IX_MapRenderer_MapRendererType] ON [gis].[MapRenderer] ([MapRendererType])
GO

CREATE INDEX [IX_MapRenderer_MapRendererLogicType] ON [gis].[MapRenderer] ([MapRendererLogicType])
GO

CREATE INDEX [IX_Map_CreatedBy] ON [gis].[UserMap] ([CreatedBy])
GO




