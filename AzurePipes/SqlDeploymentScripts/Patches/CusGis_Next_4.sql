
EXEC SP_DropColumn_With_Constraints '[gis].[LayerSource]', 'OgrLayerData'
ALTER TABLE [gis].[LayerSource] ADD [OgrLayerData] [nvarchar](max) NULL
GO -- #1. Add OgrLayerData field to LayerSource table


EXEC SP_DropColumn_With_Constraints '[gis].[LayerSource]', 'GisLayerName'
ALTER TABLE [gis].[LayerSource] ADD [GisLayerName] [nvarchar](max) NULL
CREATE INDEX [IX_LayerSource_GisLayerName] ON [gis].[LayerSource] ([LayerSourceId])
GO -- #2. Add [GisLayerName] field to LayerSource table

EXEC SP_DropColumn_With_Constraints '[gis].[LayerSource]', 'Metadata'
ALTER TABLE [gis].[LayerSource] ADD [Metadata] [nvarchar](max) NULL
GO -- #3. Add [Metadata] field to LayerSource table

EXEC SP_DropColumn_With_Constraints '[gis].[LayerSource]', 'ServeFromFileShare'
ALTER TABLE [gis].[LayerSource] ADD [ServeFromFileShare] [bit] NOT NULL DEFAULT 0
GO -- #4. Add [ServerFromFileShare] field to LayerSource table

EXEC SP_DropColumn_With_Constraints '[dbo].[WorkerJobQueue]', 'KeepAliveTimestamp'
ALTER TABLE [dbo].[WorkerJobQueue] ADD [KeepAliveTimestamp] [DATETIME] NOT NULL DEFAULT GETDATE()
CREATE INDEX [IX_WorkerJobQueue_KeepAliveTimestamp] ON [dbo].[WorkerJobQueue] ([KeepAliveTimestamp])
GO -- #5. Add [KeepAliveTimestamp] field to LayerSource table

EXEC SP_DropColumn_With_Constraints '[dbo].[WorkerJobQueue]', 'RetryCount'
ALTER TABLE [dbo].[WorkerJobQueue] ADD [RetryCount] [INT] NOT NULL DEFAULT 0
CREATE INDEX [IX_WorkerJobQueue_RetryCount] ON [dbo].[WorkerJobQueue] ([RetryCount])
GO -- #6. Add [RetryCount] field to LayerSource table

EXEC SP_DropColumn_With_Constraints '[cus].[InteractiveChart]', 'ChartExtensions'
ALTER TABLE [cus].[InteractiveChart] ADD [ChartExtensions] [nvarchar](max) NULL
GO -- #7. Add [ChartExtensions] field to InteractiveChart table
