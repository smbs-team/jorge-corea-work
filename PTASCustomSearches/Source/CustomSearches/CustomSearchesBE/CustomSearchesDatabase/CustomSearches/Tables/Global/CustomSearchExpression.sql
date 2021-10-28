﻿CREATE TABLE [cus].[CustomSearchExpression]
(
	[CustomSearchExpressionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DatasetID] UNIQUEIDENTIFIER,
    [CustomSearchColumnDefinitionId] INT,
    [CustomSearchParameterId] INT,
    [DatasetPostProcessId] INT,
    [ExceptionPostProcessRuleId] INT,
    [CustomSearchValidationRuleId] INT,
    [DatasetChartId] INT,
    [ProjectTypeId] INT,
    [RScriptModelId] INT,
    [ChartTemplateId] INT,
    [OwnerType] NVARCHAR(256) DEFAULT 'Dataset',
    [ExpressionType] NVARCHAR(256) NOT NULL DEFAULT 'T-SQL', 
    [ExpressionRole] NVARCHAR(256) NOT NULL DEFAULT 'ColumnExpression', 
    [Script] NVARCHAR(MAX),
    [ColumnName] NVARCHAR(256),
    [Category] NVARCHAR(256),
    [ExpressionGroup] NVARCHAR(256),
    [Note] NVARCHAR(MAX),
    [ExecutionOrder] INT NOT NULL DEFAULT 0,
    [ExpressionExtensions] NVARCHAR(MAX) NULL,
    [IsAutoGenerated] BIT NOT NULL DEFAULT 0

    CONSTRAINT [FK_CustomSearchExpression_ToDataset] FOREIGN KEY ([DatasetId]) REFERENCES [cus].[Dataset]([DatasetId]),    
    CONSTRAINT [FK_CustomSearchExpression_CustomSearchColumnDefinition] FOREIGN KEY ([CustomSearchColumnDefinitionId]) REFERENCES [cus].[CustomSearchColumnDefinition]([CustomSearchColumnDefinitionId]),
    CONSTRAINT [FK_CustomSearchExpression_CustomSearchParameter] FOREIGN KEY ([CustomSearchParameterId]) REFERENCES [cus].[CustomSearchParameter]([CustomSearchParameterId]),
    CONSTRAINT [FK_CustomSearchExpression_DatasetPostProcess] FOREIGN KEY ([DatasetPostProcessId]) REFERENCES [cus].[DatasetPostProcess]([DatasetPostProcessId]),
    CONSTRAINT [FK_CustomSearchExpression_ExceptionPostProcessRule] FOREIGN KEY ([ExceptionPostProcessRuleId]) REFERENCES [cus].[ExceptionPostProcessRule]([ExceptionPostProcessRuleId]),
    CONSTRAINT [FK_CustomSearchExpression_CustomSearchValidationRule] FOREIGN KEY ([CustomSearchValidationRuleId]) REFERENCES [cus].[CustomSearchValidationRule]([CustomSearchValidationRuleId]),
    CONSTRAINT [FK_CustomSearchExpression_ToInteractiveChart] FOREIGN KEY ([DatasetChartId]) REFERENCES [cus].[InteractiveChart]([InteractiveChartId]),
    CONSTRAINT [FK_CustomSearchExpression_ToProjectType] FOREIGN KEY ([ProjectTypeId]) REFERENCES [cus].[ProjectType]([ProjectTypeId]),
    CONSTRAINT [FK_CustomSearchExpression_ToRScriptModel] FOREIGN KEY ([RScriptModelId]) REFERENCES [cus].[RScriptModel]([RScriptModelId]),
    CONSTRAINT [FK_CustomSearchExpression_ChartTemplate] FOREIGN KEY ([ChartTemplateId]) REFERENCES [cus].[ChartTemplate]([ChartTemplateId]),
    CONSTRAINT [FK_CustomSearchExpression_ToExpressionType] FOREIGN KEY ([ExpressionType]) REFERENCES [cus].[ExpressionType]([ExpressionType]),    
    CONSTRAINT [FK_CustomSearchExpression_ToExpressionRole] FOREIGN KEY ([ExpressionRole]) REFERENCES [cus].[ExpressionRole]([ExpressionRole]),
    CONSTRAINT [FK_CustomSearchExpression_ToOwnerType] FOREIGN KEY ([OwnerType]) REFERENCES [cus].[OwnerType]([OwnerType])
)
GO

CREATE INDEX [IX_CustomSearchExpression_DatasetId] ON [cus].[CustomSearchExpression] ([DatasetId])
GO

CREATE INDEX [IX_CustomSearchExpression_CustomSearchColumnDefinitionID] ON [cus].[CustomSearchExpression] ([CustomSearchColumnDefinitionID])
GO

CREATE INDEX [IX_CustomSearchExpression_CustomSearchParameterID] ON [cus].[CustomSearchExpression] ([CustomSearchParameterID])
GO

CREATE INDEX [IX_CustomSearchExpression_DatasetPostProcessID] ON [cus].[CustomSearchExpression] ([DatasetPostProcessID])
GO

CREATE INDEX [IX_CustomSearchExpression_ExceptionPostProcessRuleID] ON [cus].[CustomSearchExpression] ([ExceptionPostProcessRuleID])
GO

CREATE INDEX [IX_CustomSearchExpression_CustomSearchValidationRuleID] ON [cus].[CustomSearchValidationRule] ([CustomSearchValidationRuleId])
GO

CREATE INDEX [IX_CustomSearchExpression_DatasetChartID] ON [cus].[CustomSearchExpression] ([DatasetChartID])
GO

CREATE INDEX [IX_CustomSearchExpression_ChartTemplateID] ON [cus].[CustomSearchExpression] ([ChartTemplateID])
GO

CREATE INDEX [IX_CustomSearchExpression_ProjectTypeID] ON [cus].[CustomSearchExpression] ([ProjectTypeID])
GO

CREATE INDEX [IX_CustomSearchExpression_RScriptModelID] ON [cus].[CustomSearchExpression] ([RScriptModelID])
GO
