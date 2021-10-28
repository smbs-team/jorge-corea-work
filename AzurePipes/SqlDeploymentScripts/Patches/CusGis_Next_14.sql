IF (NOT EXISTS(SELECT * FROM [cus].[DatasetState] WHERE [DatasetState] = 'Failed'))
BEGIN
    INSERT [cus].[DatasetState] ([DatasetState]) VALUES (N'Failed')
END
GO --  1. Add failed state to DatasetState

IF (NOT EXISTS(SELECT * FROM [cus].[PostProcessType] WHERE [PostProcessType] = 'CustomModelingStepPostProcess'))
BEGIN
    INSERT [cus].[PostProcessType] ([PostProcessType]) VALUES (N'CustomModelingStepPostProcess')
END
GO --  2. Add CustomModelingStepPostProcess to [DatasetPostProcessType]

IF (NOT EXISTS(SELECT * FROM [cus].[ExpressionType] WHERE [ExpressionType] = 'Imported'))
BEGIN
    INSERT [cus].[ExpressionType] ([ExpressionType]) VALUES (N'Imported')
END
GO --  3. Add Imported to [ExpressionType]

IF (NOT EXISTS(SELECT * FROM [cus].[ExpressionRole] WHERE [ExpressionRole] = 'RangedValuesOverrideExpression'))
BEGIN
    INSERT [cus].[ExpressionRole] ([ExpressionRole]) VALUES (N'RangedValuesOverrideExpression')
END
GO --  4. Add RangedValuesOverrideExpression to [ExpressionRole]

UPDATE cus.CustomSearchDefinition SET IsDeleted = 0 WHERE CustomSearchName = 'GisMapDataWrapper'
GO --  5. Mark GisMapDataWrapper as not deleted.

ALTER PROCEDURE [cus].[SP_GetQuantileBreaks]
(
    @ColumnSelectStatement NVARCHAR(MAX),
	@ColumnName NVARCHAR(MAX),
	@EmptyFilter NVARCHAR(MAX),
	@BreakCount INT	
)
AS
BEGIN
    SET NOCOUNT ON

	DECLARE @SqlStatement NVARCHAR(MAX)
	 
	SET @SqlStatement = 'SELECT (NTILE(' + CAST(@BreakCount AS NVARCHAR) + ') OVER(ORDER BY ' + @ColumnName + ' ASC)) AS ClassNumber, ' + @ColumnName + ' FROM (' + @ColumnSelectStatement + ') dv '
	IF ISNULL(@EmptyFilter, '') != ''
	BEGIN
		SET @SqlStatement = @SqlStatement + ' WHERE ' + @EmptyFilter
	END
	SET @SqlStatement = 'SELECT ClassNumber, MIN(' + @ColumnName + ') as MinValue, MAX(' + @ColumnName + ') as MaxValue FROM (' + @SqlStatement + ') detail GROUP BY ClassNumber'
	EXECUTE sp_executesql @SqlStatement
END
GO -- 6. Modify [SP_GetQuantileBreaks] stored procedure
