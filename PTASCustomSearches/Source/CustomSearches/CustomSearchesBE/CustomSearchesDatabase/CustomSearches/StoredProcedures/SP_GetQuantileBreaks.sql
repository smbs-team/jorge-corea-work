CREATE PROCEDURE [cus].[SP_GetQuantileBreaks]
(
    @DatasetView NVARCHAR(MAX),
	@ColumnName NVARCHAR(MAX),
	@EmptyFilter NVARCHAR(MAX),
	@BreakCount INT	
)
AS
BEGIN
    SET NOCOUNT ON

	DECLARE @SqlStatement NVARCHAR(MAX)
	 
	SET @SqlStatement = 'SELECT (NTILE(' + CAST(@BreakCount AS NVARCHAR) + ') OVER(ORDER BY ' + @ColumnName + ' ASC)) AS ClassNumber, ' + @ColumnName + ' FROM ' + @DatasetView  	
	IF ISNULL(@EmptyFilter, '') != ''
	BEGIN
		SET @SqlStatement = @SqlStatement + ' WHERE ' + @EmptyFilter
	END
	SET @SqlStatement = 'SELECT ClassNumber, MIN(' + @ColumnName + ') as MinValue, MAX(' + @ColumnName + ') as MaxValue FROM (' + @SqlStatement + ') detail GROUP BY ClassNumber'
	EXECUTE sp_executesql @SqlStatement
END
GO


