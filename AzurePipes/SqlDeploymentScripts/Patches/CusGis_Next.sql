-- #0.  Add dataset post-process states

IF (NOT EXISTS(SELECT * FROM [cus].[DatasetPostProcessState] ))
BEGIN
    INSERT [cus].[DatasetPostProcessState] ([DatasetPostProcessState]) VALUES (N'NeedsPostProcessUpdate')
    INSERT [cus].[DatasetPostProcessState] ([DatasetPostProcessState]) VALUES (N'NotProcessed')
    INSERT [cus].[DatasetPostProcessState] ([DatasetPostProcessState]) VALUES (N'Processed')
    INSERT [cus].[DatasetPostProcessState] ([DatasetPostProcessState]) VALUES (N'Processing')
END

IF (NOT EXISTS(SELECT * FROM [cus].[DatasetState] ))
BEGIN
    INSERT [cus].[DatasetState] ([DatasetState]) VALUES (N'NotProcessed')
    INSERT [cus].[DatasetState] ([DatasetState]) VALUES (N'GeneratingDataset')
    INSERT [cus].[DatasetState] ([DatasetState]) VALUES (N'GeneratingIndexes')
    INSERT [cus].[DatasetState] ([DatasetState]) VALUES (N'Processed')
    INSERT [cus].[DatasetState] ([DatasetState]) VALUES (N'ExecutingPostProcess')
END
GO
-- #1. Stored Procedure to drop columns with constraints

DROP procedure IF EXISTS dbo.SP_DropColumn_With_Constraints;
GO

-- #2. Stored Procedure to drop columns with constraints
CREATE PROCEDURE dbo.SP_DropColumn_With_Constraints
(
    -- Add the parameters for the stored procedure here
    @TwoPartTableNameQuoted NVARCHAR(MAX),
    @ColumnNameUnQuoted NVARCHAR(MAX)
)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON

DECLARE @DynSQL NVARCHAR(MAX);

SELECT @DynSQL =
     'ALTER TABLE ' + @TwoPartTableNameQuoted + ' DROP' + 
      ISNULL(' CONSTRAINT ' + QUOTENAME(OBJECT_NAME(c.default_object_id)) + ',','') + 
      ISNULL(check_constraints,'') + 
      '  COLUMN ' + QUOTENAME(@ColumnNameUnQuoted)
FROM   sys.columns c
       CROSS APPLY (SELECT ' CONSTRAINT ' + QUOTENAME(OBJECT_NAME(referencing_id)) + ','
                    FROM   sys.sql_expression_dependencies
                    WHERE  referenced_id = c.object_id
                           AND referenced_minor_id = c.column_id
                           AND OBJECTPROPERTYEX(referencing_id, 'BaseType') = 'C'
                    FOR XML PATH('')) ck(check_constraints)
WHERE  c.object_id = object_id(@TwoPartTableNameQuoted)
       AND c.name = @ColumnNameUnQuoted;

PRINT @DynSQL;
EXEC (@DynSQL); 
END
GO

-- #3. Add Db Table field to LayerSource table
EXEC SP_DropColumn_With_Constraints '[gis].[LayerSource]', 'DbTableName'
ALTER TABLE [gis].[LayerSource] ADD [DbTableName] [nvarchar](max) NULL
GO

-- #4. Update wetlands db table
UPDATE [gis].[LayerSource] set [DbTableName] = 'wetlands(ogr_geometry)' where LayerSourceName = 'wetlandsSource'
GO

-- #5. Mock GisMapDataView if it doesn't exist
IF OBJECT_ID('dynamics.vw_GISMapData') IS NULL
BEGIN
	EXEC('CREATE VIEW [dynamics].[vw_GISMapData]	AS SELECT 1 as ParcelId')
END
GO

-- #6. Drop GisMapData dataset wrapper view
DROP VIEW IF EXISTS [cus].[Dataset_5F6333E3_6ACB_48F0_9268_B865E521AA08_View] 
GO

-- #7 Create GisMapData dataset wrapper view
CREATE VIEW [cus].[Dataset_5F6333E3_6ACB_48F0_9268_B865E521AA08_View] AS
    SELECT 0 as CustomSearchResultId, *
    FROM [dynamics].[vw_GISMapData]
GO

--  #8. Create GisMapData dataset wrapper
IF (NOT EXISTS(SELECT * FROM [dynamics].[systemuser] where systemuserid = '00000000-0000-0000-0000-000000000000'))
BEGIN
	INSERT INTO [dynamics].[systemuser] (systemuserid, fullname, azureactivedirectoryobjectid, Internalemailaddress) VALUES ('00000000-0000-0000-0000-000000000000', 'NULL User', '00000000-0000-0000-0000-000000000000', 'nulluser@kingcounty.gov')
END

DECLARE @GisMapDataCsdId INT
SELECT @GisMapDataCsdId = CustomSearchDefinitionId FROM cus.CustomSearchDefinition where CustomSearchName = 'GisMapDataWrapper'

IF @GisMapDataCsdId IS NULL
BEGIN
    INSERT [cus].[CustomSearchDefinition] ([CustomSearchName], [CustomSearchDescription], [StoredProcedureName], [IsDeleted], [CreatedBy], [LastModifiedBy], [CreatedTimestamp], [LastModifiedTimestamp]) VALUES (N'GisMapDataWrapper', N'Fake custom search that wraps GisMapData view', N'SP_Fake', 1, N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2020-11-16T04:49:42.670' AS DateTime), CAST(N'2020-11-16T04:49:42.670' AS DateTime))
END
GO

--  #9. Create GisMapData dataset wrapper (continued).
DECLARE @GisMapDataCsdId INT
SELECT @GisMapDataCsdId = CustomSearchDefinitionId FROM cus.CustomSearchDefinition where CustomSearchName = 'GisMapDataWrapper'

PRINT 'Custom Search Id:' + CONVERT(NVARCHAR(MAX), @GisMapDataCsdId)

DECLARE @GisMapDatasetId UNIQUEIDENTIFIER
SELECT @GisMapDatasetId = DatasetId FROM cus.Dataset where DatasetId = '5f6333e3-6acb-48f0-9268-b865e521aa08'
IF @GisMapDatasetId IS NULL
BEGIN
	INSERT [cus].[Dataset] ([DatasetId], [CustomSearchDefinitionId], [UserId], [ParentFolderId], [DatasetName], [ParameterValues], [GeneratedTableName], [GenerateSchemaElapsedMs], [ExecuteStoreProcedureElapsedMs], [IsLocked], [CreatedTimestamp], [DatasetClientState], [DataSetState], [GenerateIndexesElapsedMs], [TotalRows], [LastModifiedTimestamp], [CreatedBy], [LastModifiedBy], [LastExecutionTimestamp], [DataSetPostProcessState], [LockingJobId], [SourceDatasetId], [DbLockType], [Comments], [LastExecutedBy]) VALUES (N'5f6333e3-6acb-48f0-9268-b865e521aa08', @GisMapDataCsdId, N'00000000-0000-0000-0000-000000000000', NULL, N'GisMapData Dataset', N'[]', N'Dataset_5F6333E3_6ACB_48F0_9268_B865E521AA08', 0, 0, 0, CAST(N'2020-11-16T05:00:41.347' AS DateTime), NULL, N'Processed', 0, 0, CAST(N'2020-11-16T05:00:41.347' AS DateTime), '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', NULL, N'NotProcessed', -1, NULL, NULL, N'Fake dataset used to wrap GIS Map Data View', '00000000-0000-0000-0000-000000000000')
END

DELETE [cus].[CustomSearchExpression] FROM [cus].[CustomSearchExpression] ce INNER JOIN [cus].[CustomSearchColumnDefinition] csc ON ce.CustomSearchColumnDefinitionId = csc.CustomSearchColumnDefinitionId WHERE csc.CustomSearchDefinitionId = @GisMapDataCsdId 
DELETE FROM [cus].[CustomSearchColumnDefinition] WHERE CustomSearchDefinitionId = @GisMapDataCsdId 


DECLARE @bldGradeColumnId INT
SELECT @bldGradeColumnId = CustomSearchColumnDefinitionId FROM cus.CustomSearchColumnDefinition where [ColumnName] = 'bldgGrade' and [CustomSearchDefinitionId] = @GisMapDataCsdId

INSERT [cus].[CustomSearchColumnDefinition] ([CustomSearchDefinitionId], [ColumnName], [ColumnType], [ColumnTypeLength], [CanBeUsedAsLookup], [ColumnCategory], [IsEditable], [BackendEntityName], [BackendEntityFieldName], [ForceEditLookupExpression]) VALUES (@GisMapDataCsdId, N'bldgGrade', N'Int32', 0, 1, N'Grade', 1, NULL, NULL, 0)
SELECT @bldGradeColumnId = CustomSearchColumnDefinitionId FROM cus.CustomSearchColumnDefinition where [ColumnName] = 'bldgGrade' and [CustomSearchDefinitionId] = @GisMapDataCsdId
INSERT [cus].[CustomSearchExpression] ([DatasetID], [CustomSearchColumnDefinitionId], [CustomSearchParameterId], [DatasetPostProcessId], [DatasetChartId], [ProjectTypeId], [OwnerType], [ExpressionType], [ExpressionRole], [Script], [ColumnName], [ExceptionPostProcessRuleId], [Note], [Category], [RScriptModelId], [ExpressionGroup], [ExecutionOrder], [CustomSearchValidationRuleId], [ChartTemplateId], [ExpressionExtensions]) VALUES (NULL, @bldGradeColumnId, NULL, NULL, NULL, NULL, N'CustomSearchColumnDefinition', N'TSQL', N'EditLookupExpression', N'SELECT sm.value as [Key], sm.attributevalue as [Value] FROM dynamics.stringmap sm WHERE sm.objecttypecode = ''ptas_buildingdetail'' AND sm.attributename = ''ptas_buildinggrade''', NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)

GO

-- #10. Drop FolderPath column in gis.Folder table
EXEC SP_DropColumn_With_Constraints '[gis].[Folder]', 'FolderPath'

DROP TABLE IF EXISTS gis.UserMapLayer

DELETE FROM gis.MapRendererUserSelection
DELETE FROM gis.MapRendererCategory
DELETE FROM gis.MapRendererCategory_MapRenderer
DELETE FROM gis.MapRenderer
DELETE FROM gis.Folder where FolderItemType = 'MapRenderer'
GO

-- #11. Modify gis schema for new refactor
ALTER TABLE [gis].[MapRenderer] DROP CONSTRAINT IF EXISTS [FK_MapRenderer_ToFolder]
GO

-- #12. Modify gis schema for new refactor
DROP INDEX IF EXISTS [IX_MapRenderer_ParentFolderId] ON [gis].[MapRenderer]
DROP INDEX IF EXISTS [IX_MapRenderer_UserMapId] ON [gis].[MapRenderer]
ALTER TABLE gis.MapRenderer DROP CONSTRAINT IF EXISTS [FK_MapRenderer_ToUserMap]
GO

-- #13. Modify gis schema for new refactor
EXEC SP_DropColumn_With_Constraints '[gis].[MapRenderer]', 'ParentFolderId'
GO

-- #14. Modify gis schema for new refactor
EXEC SP_DropColumn_With_Constraints '[gis].[MapRenderer]', 'UserMapId'
ALTER TABLE gis.MapRenderer ADD UserMapId INT NOT NULL
GO

-- #15. Modify gis schema for new refactor
ALTER TABLE [gis].[MapRenderer]  WITH NOCHECK ADD CONSTRAINT [FK_MapRenderer_ToUserMap] FOREIGN KEY([UserMapId])
REFERENCES [gis].[UserMap] ([UserMapId])
GO

-- #16. Modify gis schema for new refactor
ALTER TABLE [gis].[MapRenderer] CHECK CONSTRAINT [FK_MapRenderer_ToUserMap]
GO

-- #17. Modify gis schema for new refactor
CREATE INDEX [IX_MapRenderer_UserMapId] ON [gis].[MapRenderer] ([UserMapId])
GO

-- #18. Modify gis schema for new refactor
DROP INDEX IF EXISTS [IX_MapRenderer_LayerSourceId] ON [gis].[MapRenderer]
ALTER TABLE gis.MapRenderer DROP CONSTRAINT IF EXISTS [FK_MapRenderer_ToLayerSource]
EXEC SP_DropColumn_With_Constraints '[gis].[MapRenderer]', 'LayerSourceId'
ALTER TABLE gis.MapRenderer ADD LayerSourceId INT NOT NULL
GO

-- #19. Modify gis schema for new refactor
ALTER TABLE [gis].[MapRenderer]  WITH NOCHECK ADD  CONSTRAINT [FK_MapRenderer_ToLayerSource] FOREIGN KEY([LayerSourceId])
REFERENCES [gis].[LayerSource] ([LayerSourceId])
GO

-- #20. Modify gis schema for new refactor
ALTER TABLE [gis].[MapRenderer] CHECK CONSTRAINT [FK_MapRenderer_ToLayerSource]
GO

-- #21. Modify gis schema for new refactor
CREATE INDEX [IX_MapRenderer_LayerSourceId] ON [gis].[MapRenderer] ([LayerSourceId])
GO

-- #22. Modify gis schema for new refactor
DELETE FROM gis.MapRendererType

INSERT [gis].[MapRendererType] ([MapRendererType]) VALUES (N'Label')
INSERT [gis].[MapRendererType] ([MapRendererType]) VALUES (N'Color')
INSERT [gis].[MapRendererType] ([MapRendererType]) VALUES (N'Default')
GO

-- #23. Modify gis schema for new refactor
EXEC SP_DropColumn_With_Constraints '[gis].[MapRenderer]', 'IsLocked'

-- Change default tile size to 256
EXEC SP_DropColumn_With_Constraints '[gis].[LayerSource]', 'TileSize'
ALTER TABLE [gis].[LayerSource] ADD TileSize INT NOT NULL DEFAULT 256

-- Make DatasetId not required in MapRenderer
EXEC SP_DropColumn_With_Constraints '[gis].[MapRenderer]', 'DatasetId'
ALTER TABLE [gis].[Maprenderer] ADD DatasetId UNIQUEIDENTIFIER NULL
GO

-- #24. Create [UserDataStoreItem
DROP TABLE IF EXISTS [dbo].[UserDataStoreItem]

CREATE TABLE [dbo].[UserDataStoreItem]
(
	[UserDataStoreItemId] INT NOT NULL IDENTITY,     
    [UserId] UNIQUEIDENTIFIER NOT NULL,
	[StoreType] VARCHAR(36) NOT NULL,
	[OwnerType] NVARCHAR(256) NOT NULL DEFAULT 'NoOwnerType',
	[OwnerObjectId] VARCHAR(36) NOT NULL DEFAULT '',	
	[ItemName] NVARCHAR(64) NOT NULL DEFAULT '',
	[Value] NVARCHAR(MAX) NULL,

	PRIMARY KEY ([UserDataStoreItemId], [UserId], [StoreType], [OwnerType], [OwnerObjectId], [ItemName]),
	CONSTRAINT [FK_UserDataStoreItem_ToSystemUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
	CONSTRAINT [FK_UserDataStoreItem_ToOwnerType_OwnerType] FOREIGN KEY ([OwnerType]) REFERENCES [cus].[OwnerType]([OwnerType]),
)

GO

-- #25. Create UserDataStoreItem
DROP INDEX IF EXISTS [IX_UserDataStoreItem_Secondary] ON [gis].[UserDataStoreItem]
CREATE INDEX [IX_UserDataStoreItem_Secondary] ON [dbo].[UserDataStoreItem] ([UserId], [StoreType], [OwnerType], [OwnerObjectId])

GO

-- #26. Drop FolderItemType column in CUS
ALTER TABLE [cus].[Folder] DROP CONSTRAINT IF EXISTS [FK_Folder_ToFolderItemType]
EXEC SP_DropColumn_With_Constraints '[cus].[Folder]', 'FolderItemType'

-- Adding traceEnabledFields to DatasetPostProcess
EXEC SP_DropColumn_With_Constraints '[cus].[DatasetPostProcess]', 'TraceEnabledFields'
ALTER TABLE [cus].[DatasetPostProcess] ADD [TraceEnabledFields] [NVARCHAR](MAX) NULL

GO

-- #27. FN_GetLandScheduleRangedAdjustment Function
DROP FUNCTION IF EXISTS cus.FN_GetLandScheduleRangedAdjustment
GO

-- #28. FN_GetLandScheduleRangedAdjustment Function
CREATE FUNCTION cus.FN_GetLandScheduleRangedAdjustment
(
    -- Add the parameters for the function here
	@Major NVARCHAR(10),
	@Minor NVARCHAR(10),
	@AdjustmentAttribute NVARCHAR(MAX)
)
RETURNS float
AS
BEGIN
    -- Declare the return variable here
    -- Return the result of the function
    RETURN 0.75
END
GO

-- #29. PK Name fix for scaffolding

DECLARE @PkRenameStatement NVARCHAR(MAX)

CREATE TABLE #SqlStatements (
    SqlStatement NVARCHAR(MAX)
)

;WITH PKNames
AS (
SELECT name AS IndexName
,OBJECT_NAME(object_id) AS TableName
,OBJECT_SCHEMA_NAME(object_id) as SchemaName
,(SELECT '' + c.name
FROM sys.index_columns ic
INNER JOIN sys.columns c
ON ic.object_id = c.object_id
AND ic.index_column_id = c.column_id
WHERE ic.object_id = i.object_id
AND ic.index_id = i.index_id
AND ic.is_included_column = 0
ORDER BY ic.key_ordinal
FOR XML PATH('')) AS Columns
FROM sys.indexes i
WHERE i.is_primary_key = 1 and ((OBJECT_SCHEMA_NAME(object_id) = 'cus' or OBJECT_SCHEMA_NAME(object_id) = 'gis') and (not OBJECT_NAME(object_id) like 'Dataset_%') or (OBJECT_NAME(object_id) like 'UserDataStoreItem'))
)
Insert Into #SqlStatements
SELECT 'EXEC sp_rename ''' + QUOTENAME(SchemaName) + '.' + QUOTENAME(IndexName) + ''', ''PK_' + TableName + '_' + Columns + ''''
FROM PKNames

PRINT 'Renaming PKs'
-- Iterate over all customers
WHILE (1 = 1) 
BEGIN  

  -- Get next customerId
  SELECT TOP 1 @PkRenameStatement = SqlStatement
  FROM #SqlStatements

  -- Exit loop if no more customers
  IF @@ROWCOUNT = 0 BREAK;

  -- call your sproc
  EXEC sp_executesql @PkRenameStatement;
  PRINT @PkRenameStatement
  delete top(1) from  #SqlStatements  

END
GO

-- #30. Insert Donut type
DECLARE @DonutCount INT
SELECT @DonutCount = Count(*) from [cus].[ChartType] where [ChartType] = 'Donut'
if (@DonutCount  = 0) INSERT [cus].[ChartType] ([ChartType]) VALUES (N'Donut')
GO

-- #31. Add [EmbededDataFields] columns to [LayerSource]  
EXEC SP_DropColumn_With_Constraints '[gis].[LayerSource]', 'EmbeddedDataFields'
ALTER TABLE gis.[LayerSource] ADD [EmbeddedDataFields] NVARCHAR(MAX) NULL
GO

