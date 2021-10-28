
IF NOT EXISTS (SELECT [PostProcessType] FROM [cus].[PostProcessType] WHERE [PostProcessType] = 'StoredProcedureUpdatePostProcess')
	INSERT [cus].[PostProcessType] ([PostProcessType]) VALUES (N'StoredProcedureUpdatePostProcess')

GO --1. Add StoredProcedureUpdatePostProcess post-process type


IF NOT EXISTS (SELECT [ExpressionRole] FROM [cus].[ExpressionRole] WHERE [ExpressionRole] = 'RScriptCsvOutputColumn')
	INSERT [cus].[ExpressionRole] ([ExpressionRole]) VALUES (N'RScriptCsvOutputColumn')

GO --2. Add RScriptCsvOutputColumn expression role


EXEC SP_DropColumn_With_Constraints '[cus].[RscriptModel]', 'RScriptModelTemplate'
EXEC SP_DropColumn_With_Constraints '[cus].[RscriptModel]', 'PredictedTSqlExpression'
ALTER TABLE [cus].[RscriptModel] ADD [PredictedTSqlExpression] [nvarchar](max) NULL
GO --3. Replace RScriptModelTemplate with PredictedTSqlExpression field in RScriptModel table

DROP INDEX IF EXISTS [IX_CustomSearchExpression_ExceptionPostProcessRuleId] ON [cus].[CustomSearchExpression]
CREATE INDEX [IX_CustomSearchExpression_ExceptionPostProcessRuleId] ON [cus].[CustomSearchExpression] ([ExceptionPostProcessRuleId])

DROP INDEX IF EXISTS [IX_CustomSearchExpression_CustomSearchValidationRuleId] ON [cus].[CustomSearchExpression]
CREATE INDEX [IX_CustomSearchExpression_CustomSearchValidationRuleId] ON [cus].[CustomSearchExpression] ([CustomSearchValidationRuleId])

DROP INDEX IF EXISTS [IX_CustomSearchExpression_RScriptModelId] ON [cus].[CustomSearchExpression]
CREATE INDEX [IX_CustomSearchExpression_RScriptModelId] ON [cus].[CustomSearchExpression] ([RScriptModelId])

DROP INDEX IF EXISTS [IX_CustomSearchExpression_ChartTemplateId] ON [cus].[CustomSearchExpression]
CREATE INDEX [IX_CustomSearchExpression_ChartTemplateId] ON [cus].[CustomSearchExpression] ([ChartTemplateId])

DROP INDEX IF EXISTS [IX_DatasetPostProcess_DatasetId] ON [cus].[DatasetPostProcess]
CREATE INDEX [IX_DatasetPostProcess_DatasetId] ON [cus].[DatasetPostProcess] ([DatasetId])
GO --4. CustomSearchExpression missing indexes
