DECLARE @GisMapDataCsdId INT
SELECT @GisMapDataCsdId = CustomSearchDefinitionId FROM cus.CustomSearchDefinition where CustomSearchName = 'GisMapDataWrapper'

PRINT 'Custom Search Id:' + CONVERT(NVARCHAR(MAX), @GisMapDataCsdId)

DELETE [cus].[CustomSearchExpression] FROM [cus].[CustomSearchExpression] ce INNER JOIN [cus].[CustomSearchColumnDefinition] csc ON ce.CustomSearchColumnDefinitionId = csc.CustomSearchColumnDefinitionId WHERE csc.CustomSearchDefinitionId = @GisMapDataCsdId 
DELETE FROM [cus].[CustomSearchColumnDefinition] WHERE CustomSearchDefinitionId = @GisMapDataCsdId 


DECLARE @bldGradeColumnId INT
SELECT @bldGradeColumnId = CustomSearchColumnDefinitionId FROM cus.CustomSearchColumnDefinition where [ColumnName] = 'bldgGrade' and [CustomSearchDefinitionId] = @GisMapDataCsdId

INSERT [cus].[CustomSearchColumnDefinition] ([CustomSearchDefinitionId], [ColumnName], [ColumnType], [ColumnTypeLength], [CanBeUsedAsLookup], [ColumnCategory], [IsEditable], [BackendEntityName], [BackendEntityFieldName], [ForceEditLookupExpression]) VALUES (@GisMapDataCsdId, N'bldgGrade', N'Int32', 0, 1, N'Grade', 1, NULL, NULL, 0)
SELECT @bldGradeColumnId = CustomSearchColumnDefinitionId FROM cus.CustomSearchColumnDefinition where [ColumnName] = 'bldgGrade' and [CustomSearchDefinitionId] = @GisMapDataCsdId
INSERT [cus].[CustomSearchExpression] ([DatasetID], [CustomSearchColumnDefinitionId], [CustomSearchParameterId], [DatasetPostProcessId], [DatasetChartId], [ProjectTypeId], [OwnerType], [ExpressionType], [ExpressionRole], [Script], [ColumnName], [ExceptionPostProcessRuleId], [Note], [Category], [RScriptModelId], [ExpressionGroup], [ExecutionOrder], [CustomSearchValidationRuleId], [ChartTemplateId], [ExpressionExtensions]) VALUES (NULL, @bldGradeColumnId, NULL, NULL, NULL, NULL, N'CustomSearchColumnDefinition', N'TSQL', N'EditLookupExpression', N'SELECT sm.value as [Key], sm.attributevalue as [Value] FROM dynamics.stringmap sm WHERE sm.objecttypecode = ''ptas_buildingdetail'' AND sm.attributename = ''ptas_buildinggrade''', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)

GO --  1. Create GisMapData dataset column definitions.


DROP PROCEDURE IF EXISTS [gis].[SwitchStagingTable]

GO -- 2. Delete [SwitchStagingTable] Stored Procedure if it exists



CREATE PROCEDURE [gis].[SwitchStagingTable]
	@TableName VARCHAR(MAX)
AS
DECLARE @TableNameWithSchema VARCHAR(MAX) = CONCAT('gis.', @TableName);
DECLARE @StagingTable VARCHAR(MAX) = CONCAT(@TableNameWithSchema, '_staging');
DECLARE @OldTable VARCHAR(MAX) = CONCAT(@TableName, '_old');

EXEC ('DROP TABLE IF EXISTS ' + @TableNameWithSchema)  
EXEC sp_rename @StagingTable, @TableName
RETURN 0
GO -- 3. Create [SwitchStagingTable] Stored Procedure

EXEC SP_DropColumn_With_Constraints '[cus].[ProjectType]', 'ProjectType'
ALTER TABLE [cus].[ProjectType] ADD [BulkUpdateProcedureName] NVARCHAR(256) NULL
GO -- 4. Add BulkUpdateProcedureName role field to [ProjectType]

EXEC SP_DropColumn_With_Constraints '[cus].[ProjectType]', 'ApplyModelUserFilterColumnName'
ALTER TABLE [cus].[ProjectType] ADD [ApplyModelUserFilterColumnName] NVARCHAR(256) NULL
GO -- 5. Add ApplyModelUserFilterColumnName role field to [ProjectType]

EXEC SP_DropColumn_With_Constraints '[cus].[CustomSearchDefinition]', 'TableInputParameterDbType'
ALTER TABLE [cus].[CustomSearchDefinition] ADD [TableInputParameterDbType] NVARCHAR(256) NULL
GO -- 6. Add TableInputParameterDbType field to CustomSearchDefinition

EXEC SP_DropColumn_With_Constraints '[cus].[CustomSearchDefinition]', 'TableInputParameterName'
ALTER TABLE [cus].[CustomSearchDefinition] ADD [TableInputParameterName] NVARCHAR(256) NULL
GO -- 7. Add TableInputParameterName field to CustomSearchDefinition

EXEC SP_DropColumn_With_Constraints  '[cus].[CustomSearchColumnDefinition]', 'ColumDefinitionExtensions'
ALTER TABLE [cus].[CustomSearchColumnDefinition] ADD [ColumDefinitionExtensions] NVARCHAR(4000) NULL
GO -- 8. Add ColumDefinitionExtensions field to CustomSearchColumnDefinition

EXEC SP_DropColumn_With_Constraints  '[cus].[CustomSearchColumnDefinition]', 'DependsOnColumn'
ALTER TABLE [cus].[CustomSearchColumnDefinition] ADD [DependsOnColumn] NVARCHAR(256) NULL
GO -- 9. Add DependsOnColumn field to CustomSearchColumnDefinition

EXEC SP_DropColumn_With_Constraints  '[gis].[Parcel_Geom_Area]', 'Long'
ALTER TABLE [gis].[Parcel_Geom_Area] ADD [Long] FLOAT DEFAULT 0.0
GO -- 10. Add Long field to Parcel_Geom_Area

EXEC SP_DropColumn_With_Constraints  '[gis].[Parcel_Geom_Area]', 'Lat'
ALTER TABLE [gis].[Parcel_Geom_Area] ADD [Lat] FLOAT DEFAULT 0.0
GO -- 11. Add Lat field to Parcel_Geom_Area

EXEC SP_DropColumn_With_Constraints  '[gis].[Parcel_Geom_Area]', 'InSurfaceLong'
ALTER TABLE [gis].[Parcel_Geom_Area] ADD [InSurfaceLong] FLOAT DEFAULT 0.0
GO -- 12. Add InSurfaceLong field to Parcel_Geom_Area

EXEC SP_DropColumn_With_Constraints  '[gis].[Parcel_Geom_Area]', 'InSurfaceLat'
ALTER TABLE [gis].[Parcel_Geom_Area] ADD [InSurfaceLat] FLOAT DEFAULT 0.0
GO -- 13. Add InSurfaceLat field to Parcel_Geom_Area


