CREATE TABLE [gis].[MapRendererUserSelection]
(
    [UserId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [MapRendererId] INT NOT NULL

    CONSTRAINT [FK_MapRendererUserSelection_ToMapRenderer] FOREIGN KEY ([MapRendererId]) REFERENCES [gis].[MapRenderer]([MapRendererId])
)
GO
