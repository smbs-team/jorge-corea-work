CREATE TABLE [cus].[RScriptModel]
(
    [RScriptModelId] INT NOT NULL PRIMARY KEY IDENTITY,    	
    [RScriptModelName] NVARCHAR(256) NOT NULL,
    [RScriptModelRole] NVARCHAR(256),
    [RScriptFileName] NVARCHAR(2048) NOT NULL,
    [RScriptFolderName] NVARCHAR(2048) NOT NULL,
    [RScript] NVARCHAR(MAX),
    [RScriptResultsDefinition] NVARCHAR(MAX),    
    [PredictedTSqlExpression] NVARCHAR(MAX) NULL,    
    [Description] NVARCHAR(MAX)
)
GO


CREATE INDEX [IX_RScriptModel_RScriptModelName] ON [cus].[RScriptModel] ([RScriptModelName])
GO
