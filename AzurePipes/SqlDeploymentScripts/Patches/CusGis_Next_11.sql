DROP TABLE IF EXISTS [cus].[BackendUpdate]
CREATE TABLE [cus].[BackendUpdate]
(
    [BackendUpdateId] INT NOT NULL PRIMARY KEY IDENTITY,    	
    [DatasetPostProcessId] INT NULL,
    [UpdatesJson] NVARCHAR(MAX) NOT NULL,
    [ExportState] NVARCHAR(256),
    [ExportError] NVARCHAR(MAX),

    CONSTRAINT [FK_BackendUpdate_PostProcess] FOREIGN KEY ([DatasetPostProcessId]) REFERENCES [cus].[DatasetPostProcess]([DatasetPostProcessId])
)
GO -- 1. Add [BackendUpdate] table

CREATE INDEX [IX_BackendUpdate_PostProcessId] ON [cus].[BackendUpdate] ([DatasetPostProcessId])
GO -- 2. Add [IX_BackendUpdate_PostProcessId] index

CREATE INDEX [IX_BackendUpdate_PostProcessId_ExportState] ON [cus].[BackendUpdate] ([DatasetPostProcessId], [ExportState])
GO -- 3. Add [IX_BackendUpdate_PostProcessId_ExportState] index

EXEC SP_DropColumn_With_Constraints  '[cus].[RScriptModel]', 'LockPrecommitExpressions'
ALTER TABLE [cus].RScriptModel ADD LockPrecommitExpressions BIT NOT NULL DEFAULT 0
GO -- 4. Add [cus].[RScriptModel] LockPrecommitExpressions field
 