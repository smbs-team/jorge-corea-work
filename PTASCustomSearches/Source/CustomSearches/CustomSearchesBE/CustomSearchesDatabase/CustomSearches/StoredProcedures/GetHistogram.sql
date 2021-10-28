CREATE PROCEDURE [cus].[SP_GetHistogram]
(
	@DatasetTable NVARCHAR(MAX),
	@ValueScript NVARCHAR(MAX),
	@AutoBins BIT = 1,
	@NumBins FLOAT = 5,
	@UseDiscreteBins BIT = 0,
    @UseCategories BIT = 0,
	@CategoryColumn NVARCHAR(MAX) = NULL
)
AS
BEGIN
    -- Discrete test
	DECLARE @NumObservations INT
	DECLARE @MinObservation FLOAT
	DECLARE @MaxObservation FLOAT
	DECLARE @BinWidth FLOAT
	DECLARE @CurrentBin INT
	DECLARE @NonNumericObservations INT
	DECLARE @CurrentBinRangeStart FLOAT
	DECLARE @ReportedBinRangeStart FLOAT
	DECLARE @CurrentBinRangeEnd FLOAT
	DECLARE @ReportedBinRangeEnd FLOAT
	DECLARE @CurrentBinObservations FLOAT
	DECLARE @CurrentCategory NVARCHAR(MAX)
	DECLARE @CurrentCategoryIndex INT
	DECLARE @CategoryCount INT
	DECLARE @GatherDataSQLStatement NVARCHAR(MAX)
	DECLARE @GatherDataSQLParameterDef NVARCHAR(MAX)
	DECLARE @GatherCategoriesSQLStatement NVARCHAR(MAX)
	DECLARE @GetBinCountSQLStatement NVARCHAR(MAX)
	DECLARE @GetBinCountSQLParameterDef NVARCHAR(MAX)
	DECLARE @BinEndRangeCompareOperator NVARCHAR(MAX)

	IF (@DatasetTable IS NULL OR LEN(@DatasetTable) = 0)
	BEGIN
		RAISERROR('DatasetTable parameter is NULL or empty', 16, 1)
	END

	IF (@ValueScript IS NULL OR LEN(@ValueScript) = 0)
	BEGIN
		RAISERROR('ValueScript parameter is NULL or empty', 16, 1)
	END

	IF @UseCategories = 1 AND (@CategoryColumn IS NULL OR LEN(@CategoryColumn) = 0)
	BEGIN
		RAISERROR('Need to provide category column when using categories', 16, 1)
	END	

	-- Create Results table
	CREATE TABLE #BinResults
	(
		BinNumber INT,
		BinRangeStart FLOAT,
		BinRangeEnd FLOAT,
		BinObservations INT,
		BinCategory NVARCHAR(MAX)
	)

	-- Create Categories Table
	CREATE TABLE #Categories
	(
		BinCategory NVARCHAR(MAX)
	)

	IF @NonNumericObservations > 0
	BEGIN
		RAISERROR('Can''t execute histogram on non-numeric data ', 16, 1)
	END

	-- Gather Categories
	IF @UseCategories = 0
	BEGIN
		INSERT INTO #Categories (BinCategory)
			VALUES('Default')
	END ELSE
	BEGIN    		
		SET @GatherCategoriesSQLStatement = 
			'INSERT INTO #Categories (BinCategory)
			SELECT distinct(' + @CategoryColumn + ')
				FROM ' + @DatasetTable

		PRINT 'Executing: ' + @GatherCategoriesSQLStatement

		EXEC(@GatherCategoriesSQLStatement)
	END

	SELECT @CategoryCount = count(1) FROM #Categories
	SET @CurrentCategoryIndex = 0

	WHILE @CurrentCategoryIndex < @CategoryCount 
	BEGIN
		SELECT @CurrentCategory = cat.BinCategory FROM 
			(SELECT ROW_NUMBER() OVER (ORDER BY BinCategory) AS RowNumber, BinCategory FROM #Categories) cat
		WHERE cat.RowNumber = @CurrentCategoryIndex + 1

		PRINT '--------------------------------------------------'
		PRINT 'Current Category: ' + CONVERT(NVARCHAR(MAX), @CurrentCategory)

		-- Gather Data
		SET @GatherDataSQLParameterDef = 
			'@NumObservations INT OUTPUT, ' +
			'@MinObservation FLOAT OUTPUT, ' +
			'@MaxObservation FLOAT OUTPUT, ' +
			'@NonNumericObservations INT OUTPUT'

		SET @GatherDataSQLStatement = 
			'SELECT ' +
				'@NumObservations = COUNT (' + @ValueScript + '), ' +
				'@MinObservation = MIN(' + @ValueScript + '), ' +
				'@MaxObservation = MAX(' + @ValueScript + '), ' +
				'@NonNumericObservations = count(case when IsNumeric(' + @ValueScript + ') is null then 1 else null end)' +
			'FROM ' + @DatasetTable

		IF @UseCategories = 1
		BEGIN
			SET @GatherDataSQLStatement = @GatherDataSQLStatement  + ' WHERE CONVERT(NVARCHAR(MAX), ' + @CategoryColumn + ') = ''' + @CurrentCategory  + ''''			END


		PRINT 'Executing: ' + @GatherDataSQLStatement
		PRINT 'Parameters: ' + @GatherDataSQLParameterDef

		EXEC SP_EXECUTESQL  @GatherDataSQLStatement,
			@GatherDataSQLParameterDef,
			@NumObservations = @NumObservations OUTPUT, 
			@MinObservation = @MinObservation OUTPUT,
			@MaxObservation = @MaxObservation OUTPUT,
			@NonNumericObservations = @NonNumericObservations OUTPUT 

		PRINT '@NumObservations: ' + CONVERT(NVARCHAR(MAX), @NumObservations)
		PRINT '@MinObservation: ' + CONVERT(NVARCHAR(MAX), @MinObservation)
		PRINT '@MaxObservation: ' + CONVERT(NVARCHAR(MAX), @MaxObservation)
		PRINT '@NonNumericObservations: ' + CONVERT(NVARCHAR(MAX), @NonNumericObservations)

		IF @NumObservations > 0 
		BEGIN
			-- Sturges' formula for NumBins
			IF @AutoBins = 1
			BEGIN
				PRINT 'AutoBins: True'
				SET @NumBins = CAST(CEILING(LOG(@NumObservations, 2)) + 1 AS INT)		
			END
			PRINT 'NumBins: ' + CONVERT(NVARCHAR(MAX), @NumBins)

			-- Calculate BinWidth
			SET @BinWidth = (@MaxObservation - @MinObservation) / @NumBins
			PRINT 'MaxObservation: ' + CONVERT(NVARCHAR(MAX), @MaxObservation)
			PRINT 'MinObservation: ' + CONVERT(NVARCHAR(MAX), @MinObservation)
			PRINT 'BinWidth - Before Ceil:' + CONVERT(NVARCHAR(MAX), @BinWidth)
			IF @UseDiscreteBins = 1
			BEGIN
				SET @BinWidth = CEILING(@BinWidth)
				PRINT 'BinWidth: ' + CONVERT(NVARCHAR(MAX), @BinWidth)
			END

			SET @CurrentBin = 0
			WHILE @CurrentBin < @NumBins
			BEGIN
				PRINT '-------------------------'
				PRINT 'Current Bin: ' + CONVERT(NVARCHAR(MAX), @CurrentBin)

				-- Count Observatiosn in BIN
				SET @CurrentBinRangeStart = (@MinObservation + (@BinWidth * @CurrentBin))
				SET @ReportedBinRangeStart = @CurrentBinRangeStart 

				SET @CurrentBinRangeEnd = @CurrentBinRangeStart + @BinWidth
				SET @ReportedBinRangeEnd = @CurrentBinRangeEnd

				SET @BinEndRangeCompareOperator = '<'			

				IF @CurrentBin = 0
				BEGIN
					SET @CurrentBinRangeStart =  CAST('-1.79E+308' AS FLOAT) -- Min Float
				END			
								
				IF @CurrentBin = @NumBins - 1
				BEGIN
					SET @CurrentBinRangeEnd =  CAST('1.79E+308' AS FLOAT) -- Min Float
					SET @BinEndRangeCompareOperator = @BinEndRangeCompareOperator  + '='
				END

				SET @GetBinCountSQLParameterDef = 
					'@CurrentBinObservations INT OUTPUT, ' +
					'@CurrentBinRangeStart FLOAT, ' +
					'@CurrentBinRangeEnd FLOAT'

				SET @GetBinCountSQLStatement = 
					'SELECT @CurrentBinObservations = count(1) ' +
					'FROM ' + @DatasetTable + ' ' +
					'WHERE CAST(' + @ValueScript + ' AS FLOAT) >= @CurrentBinRangeStart ' + 
						' AND CAST(' + @ValueScript + ' AS FLOAT) ' + @BinEndRangeCompareOperator + ' @CurrentBinRangeEnd'

				IF @UseCategories = 1
				BEGIN
					SET @GetBinCountSQLStatement = @GetBinCountSQLStatement  + ' AND CONVERT(NVARCHAR(MAX), ' + @CategoryColumn + ') = ''' + @CurrentCategory  + ''''
				END

				PRINT 'Executing: ' + @GetBinCountSQLStatement
				PRINT 'Parameters: ' + @GetBinCountSQLParameterDef

				EXEC SP_EXECUTESQL  @GetBinCountSQLStatement,
					@GetBinCountSQLParameterDef,
					@CurrentBinObservations = @CurrentBinObservations OUTPUT,
					@CurrentBinRangeStart = @CurrentBinRangeStart,
					@CurrentBinRangeEnd = @CurrentBinRangeEnd					

				INSERT INTO #BinResults
					(BinNumber, BinRangeStart, BinRangeEnd, BinObservations, BinCategory)
				VALUES
					(@CurrentBin,  @ReportedBinRangeStart, @ReportedBinRangeEnd, @CurrentBinObservations, @CurrentCategory)

				PRINT 'Range Start: ' + CONVERT(NVARCHAR(MAX), @CurrentBinRangeStart)
				PRINT 'Range End: ' + CONVERT(NVARCHAR(MAX), @CurrentBinRangeEnd)
				PRINT 'Bin Observations: ' + CONVERT(NVARCHAR(MAX), @CurrentBinObservations)

				SET @CurrentBin = @CurrentBin + 1
			END
		END
		SET @CurrentCategoryIndex = @CurrentCategoryIndex + 1
	END

	SELECT * FROM  #BinResults
END
