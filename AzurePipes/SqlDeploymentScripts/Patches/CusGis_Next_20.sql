IF TYPE_ID(N'cus.PhysicalInspectionModelTypeV2') IS NULL
BEGIN
	CREATE TYPE [cus].[PhysicalInspectionModelTypeV2] AS TABLE(
		[ParcelId] [nvarchar](300) NULL,
		[Major] [nvarchar](300) NULL,
		[Minor] [nvarchar](300) NULL,
		[NewTrendedPrice] [float] NULL,
		[TimeAdjustment] [float] NULL,
		[NewLandValue] [float] NULL,
		[NewEMV] [float] NULL,	
		[Supplemental] [float] NULL,	
		[SupplementalFormula] [nvarchar](MAX) NULL
	)
END
GO -- 1. Create [cus].[PhysicalInspectionModelTypeV2]

DECLARE @BulkUpdateCSId int = NULL;
SELECT @BulkUpdateCSId = CustomSearchDefinitionId FROM [cus].[CustomSearchDefinition] WHERE [CustomSearchName] = 'Residential Bulk Update'

IF (@BulkUpdateCSId  IS NOT NULL)
BEGIN
	
	UPDATE [cus].[CustomSearchDefinition] SET TableInputParameterDbType = 'PhysicalInspectionModelTypeV2', TableInputParameterName = 'PhysicalInspectionModel'  WHERE CustomSearchDefinitionId = @BulkUpdateCSId
END
GO --  2. Update Physical Inspection Custom Search with the new Table Type
