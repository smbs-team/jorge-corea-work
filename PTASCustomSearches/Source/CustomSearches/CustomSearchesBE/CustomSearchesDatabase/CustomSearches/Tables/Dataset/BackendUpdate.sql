CREATE TABLE [cus].[BackendUpdate]
(
    [BackendUpdateId] INT NOT NULL PRIMARY KEY IDENTITY,    	
    [DatasetPostProcessId] INT NULL,
    [UpdatesJson] NVARCHAR(MAX) NOT NULL,
    [ExportState] NVARCHAR(256),
    [ExportError] NVARCHAR(MAX),

    CONSTRAINT [FK_BackendUpdate_PostProcess] FOREIGN KEY ([DatasetPostProcessId]) REFERENCES [cus].[DatasetPostProcess]([DatasetPostProcessId])
)
GO

CREATE INDEX [IX_BackendUpdate_PostProcessId] ON [cus].[BackendUpdate] ([DatasetPostProcessId])
GO

CREATE INDEX [IX_BackendUpdate_PostProcessId_ExportState] ON [cus].[BackendUpdate] ([DatasetPostProcessId], [ExportState])
GO
