ALTER TABLE [cus].[DatasetPostProcess] ALTER COLUMN [PostProcessSubType] NVARCHAR(256) NULL
GO -- 1. Remove not null constraint for PostProcessSubType from DatasetPostProcess

ALTER TABLE [gis].[LayerSource] ALTER COLUMN [DefaultLabelMapboxLayer] NVARCHAR(MAX) NULL
GO -- 2. Remove not null constraint for [DefaultLabelMapboxLayer] from [LayerSource]

EXEC SP_DropColumn_With_Constraints  '[cus].[CustomSearchDefinition]', 'ExecutionRoles'
ALTER TABLE [cus].[CustomSearchDefinition] ADD [ExecutionRoles] NVARCHAR(4000) NULL
GO -- 3. Add ExecutionRoles role field to CustomSearchDefinition
 
EXEC SP_DropColumn_With_Constraints  '[cus].[CustomSearchDefinition]', 'DatasetEditRoles'
ALTER TABLE [cus].[CustomSearchDefinition] ADD [DatasetEditRoles] NVARCHAR(4000) NULL
GO -- 4. Add DatasetEditRoles role field to CustomSearchDefinition

EXEC SP_DropColumn_With_Constraints  '[cus].[CustomSearchDefinition]', 'RowLevelEditRolesColumn'
ALTER TABLE [cus].[CustomSearchDefinition] ADD [RowLevelEditRolesColumn] NVARCHAR(256) NULL
GO -- 5. Add Add RowLevelEditRolesColumn role field to CustomSearchDefinition

EXEC SP_DropColumn_With_Constraints  '[cus].[CustomSearchColumnDefinition]', 'ColumnEditRoles'
ALTER TABLE [cus].[CustomSearchColumnDefinition] ADD [ColumnEditRoles] NVARCHAR(4000) NULL
GO -- 6. Add Add ColumnEditRoles role field to CustomSearchDefinition

