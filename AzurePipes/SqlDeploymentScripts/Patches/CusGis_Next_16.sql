
DECLARE @BulkUpdateCSId int = NULL;
SELECT @BulkUpdateCSId = CustomSearchDefinitionId FROM [cus].[CustomSearchDefinition] WHERE [CustomSearchName] = 'Residential Bulk Update'
DECLARE @PhysicalInspectionProjectTypeId int = NULL;
SELECT @PhysicalInspectionProjectTypeId = ProjectTypeId FROM [cus].[ProjectType] WHERE [ProjectTypeName] = 'Physical Inspection'

IF (@BulkUpdateCSId  IS NOT NULL AND @PhysicalInspectionProjectTypeId  IS NOT NULL)
BEGIN
	IF (NOT EXISTS(SELECT * FROM [cus].[ProjectType_CustomSearchDefinition] WHERE ProjectTypeId =  @PhysicalInspectionProjectTypeId AND CustomSearchId = @BulkUpdateCSId))
	BEGIN
		INSERT [cus].[ProjectType_CustomSearchDefinition]  ([ProjectTypeId], [DatasetRole], [CustomSearchDefinitionId]) VALUES (@PhysicalInspectionProjectTypeId , N'ApplyModel', @BulkUpdateCSId)
	END

	UPDATE [cus].[ProjectType] SET BulkUpdateProcedureName = 'SRCH_U_Bulkupdate', ApplyModelUserFilterColumnName = 'AssignedBoth' WHERE ProjectTypeId = @PhysicalInspectionProjectTypeId
	UPDATE [cus].[CustomSearchDefinition] SET TableInputParameterName = 'PhysicalInspectionModel', TableInputParameterDbType = 'PhysicalInspectionModelType' WHERE CustomSearchDefinitionId = @BulkUpdateCSId
END
GO --  1. Update Physical Inspection to add Apply Model process metadata 

EXEC SP_DropColumn_With_Constraints  '[cus].[BackendUpdate]', 'SingleRowMajor'
ALTER TABLE [cus].BackendUpdate ADD SingleRowMajor NVARCHAR(6) NULL
GO -- 2. Add SingleRowMajor field to [BackendUpdate]

EXEC SP_DropColumn_With_Constraints  '[cus].[BackendUpdate]', 'SingleRowMinor'
ALTER TABLE [cus].BackendUpdate ADD SingleRowMinor NVARCHAR(4) NULL
GO -- 3. Add SingleRowMinor field to [BackendUpdate]

