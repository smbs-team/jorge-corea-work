IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ASSR_SimpleLinearRegression')
	DROP PROCEDURE [cus].[ASSR_SimpleLinearRegression]  
GO


CREATE PROCEDURE [cus].[ASSR_SimpleLinearRegression]
 @R_Square float = null output  --i.e., coefficient of determination
,@Slope float = null output
,@Y_Intercept float = null output
,@X_Counts float = null output
,@TrendConclusion varchar(200) = null output 
--Output to let the user know if interpolation was needed.
--That way, if the user compares with other stats software they would know why this particular stat might not match exacly.
,@Exact_Match_or_Interpolate_T_Table varchar(120) = null output 

,@Y_Average float = null output
,@X_Average float = null output


,@t_Of_Slope float = null output
,@Critical_T_Alpha05 float = null output
,@Critical_T_Alpha10 float = null output
,@Critical_T_Alpha20 float = null output   

,@T_Of_Slope_to_Critical_T_Alpha05 float = null output 
,@T_Of_Slope_to_Critical_T_Alpha10 float = null output 
,@T_Of_Slope_to_Critical_T_Alpha20 float = null output 


,@DegreesOfFreedom float = null output


  



--Lower and upper bracketing value it table where interpolation is needed  
,@LowerLimit_DegFree_T_Table_For_Interpol float = null output
,@UpperLimit_DegFree_T_Table_For_Interpol float = null output


,@Lower_Crit_T_For_Interpol_Alpha05 float = null output
,@Upper_Crit_T_For_Interpol_Alpha05 float = null output

,@Lower_Crit_T_For_Interpol_Alpha10 float = null output
,@Upper_Crit_T_For_Interpol_Alpha10 float = null output

,@Lower_Crit_T_For_Interpol_Alpha20 float = null output 
,@Upper_Crit_T_For_Interpol_Alpha20 float = null output 


  
 
AS
BEGIN

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED


--Basic tests to manipulate data in table and return an output param.
--In calling SP, create #SimpleLinearRegression table, insert records, then call this SP



------#SimpleLinearRegression gets inserted from calling procedure
------drop table #SimpleLinearRegression
----CREATE TABLE #SimpleLinearRegression 
----   ( 
----     DependentVariable float     
----    ,IndependentVariable float
----    ,PredictedValue float
----    ,ObservedMinusPredicted float
----    )
------For test and debug, populate #SimpleLinearRegression here instead of from Calling procedure
----insert #SimpleLinearRegression (DependentVariable, IndependentVariable)
----select
----  PrevAVSP
---- ,datediff(day,'1/1/2018',SaleDate)
----from GisMapData 
----where PrevAVSP > 0 
----and PrevAVSP > .5 --a bit arbitrary
----and PrevAVSP < 2 --a bit arbitrary
----and SalePrice > 0
----and VerifAtMkt <> 'N'
----and NbrPclsInSale = 1
----and ResArea = '017'
----and SaleDate <= '1/1/2018' 
----order by datediff(day,'1/1/2018',SaleDate) 


SELECT 
@X_Average = AVG(IndependentVariable) 
FROM #SimpleLinearRegression


SELECT 
@Y_Average = AVG(DependentVariable) 
FROM #SimpleLinearRegression


SELECT 
@slope = SUM((IndependentVariable - @X_Average) * (DependentVariable - @Y_Average))/SUM(POWER(IndependentVariable - @X_Average, 2))
FROM #SimpleLinearRegression


SELECT @Y_Intercept = @Y_Average - (@slope * @X_Average) 


SELECT @R_Square = 1 - (SUM(POWER(DependentVariable - (@Y_Intercept + @slope * IndependentVariable), 2))
                     /(SUM(POWER(DependentVariable - (@Y_Intercept + @slope * IndependentVariable), 2))  
                     + SUM(POWER(((@Y_Intercept + @slope * IndependentVariable) - @Y_Average), 2))))
FROM #SimpleLinearRegression


--         Slope                  Intercept              rSquare
--         ---------------------- ---------------------- ----------------------
--         -0.000278934401850511  0.817219331184755      0.135105164154055

--FROM EXCEL: y = -0.0003x       + 0.8172           R² = 0.1351


--left off this looks wrong from SRCH_R_2YrsSalesRes:
    --UPDATE #SimpleLinearRegression
    --SET PredictedRatio = Ratio * @slope + @intercept

    --UPDATE #SimpleLinearRegression
    --SET ObservedMinusPredicted = Ratio - PredictedRatio

--but try it here just to see
UPDATE #SimpleLinearRegression
SET PredictedValue = IndependentVariable * @slope + @Y_Intercept

--Residual – the difference between the observed value and the predicted value.
UPDATE #SimpleLinearRegression
SET ObservedMinusPredicted = DependentVariable - PredictedValue






SELECT @X_Counts = COUNT(*) FROM #SimpleLinearRegression

SELECT @DegreesOfFreedom = @X_Counts-1



--Sum of Sq. in ANOVA and Regression
--For reference, sum of squares in regression uses the equation:
--And in ANOVA it is calculated with:
--The total SS = treatment sum of squares (SST) + SS of the residual error (SSE)


DECLARE @RawSumOfX float
SELECT @RawSumOfX =  SUM(DependentVariable) FROM #SimpleLinearRegression

DECLARE @RawSumOfY float
SELECT @RawSumOfY =  SUM(IndependentVariable) FROM #SimpleLinearRegression

DECLARE @RawSumOfSquaresX float
SELECT @RawSumOfSquaresX =  SUM(SQUARE(DependentVariable)) FROM #SimpleLinearRegression

DECLARE @RawSumOfSquaresY float
SELECT @RawSumOfSquaresY =  SUM(SQUARE(IndependentVariable)) FROM #SimpleLinearRegression

DECLARE @RawSumOfCrossProducts float
SELECT @RawSumOfCrossProducts =  SUM(DependentVariable * IndependentVariable) FROM #SimpleLinearRegression

--Confirmed 2 diff ways below to calculate CorrectedSumOfSquaresX 
DECLARE @CorrectedSumOfSquaresX float
SELECT @CorrectedSumOfSquaresX = SUM(SQUARE(DependentVariable - @Y_Average)) FROM #SimpleLinearRegression
DECLARE @CorrectedSumOfSquaresX_b float
SELECT @CorrectedSumOfSquaresX_b =  @RawSumOfSquaresX - (@X_Counts * SQUARE(@Y_Average))

DECLARE @TotalSumOfSquaresY float
SELECT @TotalSumOfSquaresY = SUM(SQUARE(IndependentVariable - @X_Average)) FROM #SimpleLinearRegression

DECLARE @CorrectedSumOfCrossProducts float
SELECT @CorrectedSumOfCrossProducts =  SUM((DependentVariable - @Y_Average)*(IndependentVariable - @X_Average)) 
FROM #SimpleLinearRegression

DECLARE @CalcSlope float
SELECT @CalcSlope = 0
IF @CorrectedSumOfSquaresX > 0 SELECT @CalcSlope = @CorrectedSumOfCrossProducts / @CorrectedSumOfSquaresX
--select CalcSlope = @CalcSlope  --YES!! This matches!

DECLARE @CalcIntercept float
SELECT @CalcIntercept = @X_Average - @CalcSlope * @Y_Average
--select CalcIntercept = @CalcIntercept  --YES!! This matches!

DECLARE @ErrorSumOfSquares float
SELECT @ErrorSumOfSquares =  SUM(SQUARE(IndependentVariable - PredictedValue))
FROM #SimpleLinearRegression

DECLARE @MeanSquareError float
SELECT @MeanSquareError = 0
IF (@DegreesOfFreedom - 1) > 0 SELECT @MeanSquareError = @ErrorSumOfSquares/(@DegreesOfFreedom - 1)


DECLARE @StdErr_Of_Slope float
SELECT @StdErr_Of_Slope = SQRT(@MeanSquareError/@CorrectedSumOfSquaresX)

DECLARE @RegressionExplainedSumOfSquares float
SELECT @RegressionExplainedSumOfSquares =  SUM(SQUARE(PredictedValue - @X_Average))
FROM #SimpleLinearRegression


DECLARE @R_CorrelationCoefficient float
SELECT @R_CorrelationCoefficient = 0
IF SQRT((@X_Counts * @RawSumOfSquaresX - SQUARE(@RawSumOfX)) * (@X_Counts * @RawSumOfSquaresY - SQUARE(@RawSumOfY))) > 0
BEGIN
  SELECT @R_CorrelationCoefficient = 
         (@X_Counts * @RawSumOfCrossProducts - @RawSumOfX * @RawSumOfY) 
                                        / 
          SQRT((@X_Counts * @RawSumOfSquaresX - SQUARE(@RawSumOfX)) * (@X_Counts * @RawSumOfSquaresY - SQUARE(@RawSumOfY)))
END


DECLARE @R_SQUARE_Calc float
SELECT @R_SQUARE_Calc = SQUARE(@R_CorrelationCoefficient)
--select R_SQUARE_Calc = @R_SQUARE_Calc --YES!! This matches!


--Task: test H : true slope is 0
--DECLARE @t_Of_Slope float

SELECT @t_Of_Slope = 0
IF (@DegreesOfFreedom - 1) > 0 AND (1- @R_SQUARE_Calc) > 0 SELECT @t_Of_Slope =  SQRT(@R_Square) * SQRT((@DegreesOfFreedom - 1)/(1- @R_SQUARE_Calc))
--SELECT @t_Of_Slope =  @R_CorrelationCoefficient * SQRT((@DegreesOfFreedom - 1)/(1- @R_SQUARE_Calc))
----select t_Of_Slope = @t_Of_Slope  --YES!! This matches!

--Area 17
--t_Of_b
------------------------
---21.2388568080542

--Slope                  Intercept              rSquare                SalesCountInRegression AvgIndependentVariable
------------------------ ---------------------- ---------------------- ---------------------- ----------------------
---0.00030352272113265   0.81598638010921       0.295778821512494      1076                   0.972490706319703



--Critical T test.  If T IndependentVariable below > 1 then reject null hypothesis (i.e., conclude that slope is statistically significant i.e. real)
--Try to take sample size into account so the T-test has a better chance of matching the results for stats software.
--Google search did not reveal a way to calculate probability values (not sure of how to apply Calculus in SQL.
--Try to interpolate between values in the T-table, using straight line interpolation even though the actual line is curved. 


--http://math.wikia.com/wiki/T_table 
--drop table #Critical_T_Values
CREATE TABLE #Critical_T_Values (AlphaProb decimal(20,2), DegreesOfFreedom float, CriticalT float )--float for division
--AlphaProb = .05 2 tailed test

INSERT #Critical_T_Values SELECT 0.05, 1, 12.71
INSERT #Critical_T_Values SELECT 0.05, 2, 4.303
INSERT #Critical_T_Values SELECT 0.05, 3, 3.182
INSERT #Critical_T_Values SELECT 0.05, 4, 2.776
INSERT #Critical_T_Values SELECT 0.05, 5, 2.571
INSERT #Critical_T_Values SELECT 0.05, 6, 2.447
INSERT #Critical_T_Values SELECT 0.05, 7, 2.365
INSERT #Critical_T_Values SELECT 0.05, 8, 2.306
INSERT #Critical_T_Values SELECT 0.05, 9, 2.262
INSERT #Critical_T_Values SELECT 0.05, 10, 2.228
INSERT #Critical_T_Values SELECT 0.05, 11, 2.201
INSERT #Critical_T_Values SELECT 0.05, 12, 2.179
INSERT #Critical_T_Values SELECT 0.05, 13, 2.16
INSERT #Critical_T_Values SELECT 0.05, 14, 2.145
INSERT #Critical_T_Values SELECT 0.05, 15, 2.131
INSERT #Critical_T_Values SELECT 0.05, 16, 2.12
INSERT #Critical_T_Values SELECT 0.05, 17, 2.11
INSERT #Critical_T_Values SELECT 0.05, 18, 2.101
INSERT #Critical_T_Values SELECT 0.05, 19, 2.093
INSERT #Critical_T_Values SELECT 0.05, 20, 2.086
INSERT #Critical_T_Values SELECT 0.05, 21, 2.08
INSERT #Critical_T_Values SELECT 0.05, 22, 2.074
INSERT #Critical_T_Values SELECT 0.05, 23, 2.069
INSERT #Critical_T_Values SELECT 0.05, 24, 2.064
INSERT #Critical_T_Values SELECT 0.05, 25, 2.06
INSERT #Critical_T_Values SELECT 0.05, 26, 2.056
INSERT #Critical_T_Values SELECT 0.05, 27, 2.052
INSERT #Critical_T_Values SELECT 0.05, 28, 2.048
INSERT #Critical_T_Values SELECT 0.05, 29, 2.045
INSERT #Critical_T_Values SELECT 0.05, 30, 2.042
INSERT #Critical_T_Values SELECT 0.05, 31, 2.04
INSERT #Critical_T_Values SELECT 0.05, 32, 2.037
INSERT #Critical_T_Values SELECT 0.05, 33, 2.035
INSERT #Critical_T_Values SELECT 0.05, 34, 2.032
INSERT #Critical_T_Values SELECT 0.05, 35, 2.03
INSERT #Critical_T_Values SELECT 0.05, 36, 2.028
INSERT #Critical_T_Values SELECT 0.05, 37, 2.026
INSERT #Critical_T_Values SELECT 0.05, 38, 2.024
INSERT #Critical_T_Values SELECT 0.05, 39, 2.023
INSERT #Critical_T_Values SELECT 0.05, 40, 2.021
INSERT #Critical_T_Values SELECT 0.05, 41, 2.02
INSERT #Critical_T_Values SELECT 0.05, 42, 2.018
INSERT #Critical_T_Values SELECT 0.05, 43, 2.017
INSERT #Critical_T_Values SELECT 0.05, 44, 2.015
INSERT #Critical_T_Values SELECT 0.05, 45, 2.014
INSERT #Critical_T_Values SELECT 0.05, 46, 2.013
INSERT #Critical_T_Values SELECT 0.05, 47, 2.012
INSERT #Critical_T_Values SELECT 0.05, 48, 2.011
INSERT #Critical_T_Values SELECT 0.05, 49, 2.01
INSERT #Critical_T_Values SELECT 0.05, 50, 2.009
INSERT #Critical_T_Values SELECT 0.05, 55, 2.004
INSERT #Critical_T_Values SELECT 0.05, 60, 2
INSERT #Critical_T_Values SELECT 0.05, 65, 1.997
INSERT #Critical_T_Values SELECT 0.05, 70, 1.994
INSERT #Critical_T_Values SELECT 0.05, 75, 1.992
INSERT #Critical_T_Values SELECT 0.05, 80, 1.99
INSERT #Critical_T_Values SELECT 0.05, 85, 1.988
INSERT #Critical_T_Values SELECT 0.05, 90, 1.987
INSERT #Critical_T_Values SELECT 0.05, 95, 1.985
INSERT #Critical_T_Values SELECT 0.05, 100, 1.984
INSERT #Critical_T_Values SELECT 0.05, 120, 1.98
INSERT #Critical_T_Values SELECT 0.05, 140, 1.977
INSERT #Critical_T_Values SELECT 0.05, 160, 1.975
INSERT #Critical_T_Values SELECT 0.05, 180, 1.973
INSERT #Critical_T_Values SELECT 0.05, 200, 1.972
INSERT #Critical_T_Values SELECT 0.05, 250, 1.969
INSERT #Critical_T_Values SELECT 0.05, 300, 1.968
INSERT #Critical_T_Values SELECT 0.05, 350, 1.967
INSERT #Critical_T_Values SELECT 0.05, 400, 1.966
INSERT #Critical_T_Values SELECT 0.05, 450, 1.965
INSERT #Critical_T_Values SELECT 0.05, 500, 1.965
INSERT #Critical_T_Values SELECT 0.05, 600, 1.964
INSERT #Critical_T_Values SELECT 0.05, 700, 1.963
INSERT #Critical_T_Values SELECT 0.05, 800, 1.963
INSERT #Critical_T_Values SELECT 0.05, 900, 1.963
INSERT #Critical_T_Values SELECT 0.05, 1000, 1.962
INSERT #Critical_T_Values SELECT 0.05, 2000, 1.961
INSERT #Critical_T_Values SELECT 0.05, 3000, 1.961
INSERT #Critical_T_Values SELECT 0.05, 4000, 1.961
INSERT #Critical_T_Values SELECT 0.05, 5000, 1.96
INSERT #Critical_T_Values SELECT 0.05, 1000000, 1.96

--AlphaProb = .10 2 tailed test
INSERT #Critical_T_Values SELECT 0.10, 1, 6.314
INSERT #Critical_T_Values SELECT 0.10, 2, 2.92
INSERT #Critical_T_Values SELECT 0.10, 3, 2.353
INSERT #Critical_T_Values SELECT 0.10, 4, 2.132
INSERT #Critical_T_Values SELECT 0.10, 5, 2.015
INSERT #Critical_T_Values SELECT 0.10, 6, 1.943
INSERT #Critical_T_Values SELECT 0.10, 7, 1.895
INSERT #Critical_T_Values SELECT 0.10, 8, 1.86
INSERT #Critical_T_Values SELECT 0.10, 9, 1.833
INSERT #Critical_T_Values SELECT 0.10, 10, 1.812
INSERT #Critical_T_Values SELECT 0.10, 11, 1.796
INSERT #Critical_T_Values SELECT 0.10, 12, 1.782
INSERT #Critical_T_Values SELECT 0.10, 13, 1.771
INSERT #Critical_T_Values SELECT 0.10, 14, 1.761
INSERT #Critical_T_Values SELECT 0.10, 15, 1.753
INSERT #Critical_T_Values SELECT 0.10, 16, 1.746
INSERT #Critical_T_Values SELECT 0.10, 17, 1.74
INSERT #Critical_T_Values SELECT 0.10, 18, 1.734
INSERT #Critical_T_Values SELECT 0.10, 19, 1.729
INSERT #Critical_T_Values SELECT 0.10, 20, 1.725
INSERT #Critical_T_Values SELECT 0.10, 21, 1.721
INSERT #Critical_T_Values SELECT 0.10, 22, 1.717
INSERT #Critical_T_Values SELECT 0.10, 23, 1.714
INSERT #Critical_T_Values SELECT 0.10, 24, 1.711
INSERT #Critical_T_Values SELECT 0.10, 25, 1.708
INSERT #Critical_T_Values SELECT 0.10, 26, 1.706
INSERT #Critical_T_Values SELECT 0.10, 27, 1.703
INSERT #Critical_T_Values SELECT 0.10, 28, 1.701
INSERT #Critical_T_Values SELECT 0.10, 29, 1.699
INSERT #Critical_T_Values SELECT 0.10, 30, 1.697
INSERT #Critical_T_Values SELECT 0.10, 31, 1.696
INSERT #Critical_T_Values SELECT 0.10, 32, 1.694
INSERT #Critical_T_Values SELECT 0.10, 33, 1.692
INSERT #Critical_T_Values SELECT 0.10, 34, 1.691
INSERT #Critical_T_Values SELECT 0.10, 35, 1.69
INSERT #Critical_T_Values SELECT 0.10, 36, 1.688
INSERT #Critical_T_Values SELECT 0.10, 37, 1.687
INSERT #Critical_T_Values SELECT 0.10, 38, 1.686
INSERT #Critical_T_Values SELECT 0.10, 39, 1.685
INSERT #Critical_T_Values SELECT 0.10, 40, 1.684
INSERT #Critical_T_Values SELECT 0.10, 41, 1.683
INSERT #Critical_T_Values SELECT 0.10, 42, 1.682
INSERT #Critical_T_Values SELECT 0.10, 43, 1.681
INSERT #Critical_T_Values SELECT 0.10, 44, 1.68
INSERT #Critical_T_Values SELECT 0.10, 45, 1.679
INSERT #Critical_T_Values SELECT 0.10, 46, 1.679
INSERT #Critical_T_Values SELECT 0.10, 47, 1.678
INSERT #Critical_T_Values SELECT 0.10, 48, 1.677
INSERT #Critical_T_Values SELECT 0.10, 49, 1.677
INSERT #Critical_T_Values SELECT 0.10, 50, 1.676
INSERT #Critical_T_Values SELECT 0.10, 55, 1.673
INSERT #Critical_T_Values SELECT 0.10, 60, 1.671
INSERT #Critical_T_Values SELECT 0.10, 65, 1.669
INSERT #Critical_T_Values SELECT 0.10, 70, 1.667
INSERT #Critical_T_Values SELECT 0.10, 75, 1.665
INSERT #Critical_T_Values SELECT 0.10, 80, 1.664
INSERT #Critical_T_Values SELECT 0.10, 85, 1.663
INSERT #Critical_T_Values SELECT 0.10, 90, 1.662
INSERT #Critical_T_Values SELECT 0.10, 95, 1.661
INSERT #Critical_T_Values SELECT 0.10, 100, 1.66
INSERT #Critical_T_Values SELECT 0.10, 120, 1.658
INSERT #Critical_T_Values SELECT 0.10, 140, 1.656
INSERT #Critical_T_Values SELECT 0.10, 160, 1.654
INSERT #Critical_T_Values SELECT 0.10, 180, 1.653
INSERT #Critical_T_Values SELECT 0.10, 200, 1.653
INSERT #Critical_T_Values SELECT 0.10, 250, 1.651
INSERT #Critical_T_Values SELECT 0.10, 300, 1.65
INSERT #Critical_T_Values SELECT 0.10, 350, 1.649
INSERT #Critical_T_Values SELECT 0.10, 400, 1.649
INSERT #Critical_T_Values SELECT 0.10, 450, 1.648
INSERT #Critical_T_Values SELECT 0.10, 500, 1.648
INSERT #Critical_T_Values SELECT 0.10, 600, 1.647
INSERT #Critical_T_Values SELECT 0.10, 700, 1.647
INSERT #Critical_T_Values SELECT 0.10, 800, 1.647
INSERT #Critical_T_Values SELECT 0.10, 900, 1.647
INSERT #Critical_T_Values SELECT 0.10, 1000, 1.646
INSERT #Critical_T_Values SELECT 0.10, 2000, 1.646
INSERT #Critical_T_Values SELECT 0.10, 3000, 1.645
INSERT #Critical_T_Values SELECT 0.10, 4000, 1.645
INSERT #Critical_T_Values SELECT 0.10, 5000, 1.645
INSERT #Critical_T_Values SELECT 0.10, 1000000, 1.645

--AlphaProb = .20 2 tailed test
INSERT #Critical_T_Values SELECT 0.20, 1, 3.078
INSERT #Critical_T_Values SELECT 0.20, 2, 1.886
INSERT #Critical_T_Values SELECT 0.20, 3, 1.638
INSERT #Critical_T_Values SELECT 0.20, 4, 1.533
INSERT #Critical_T_Values SELECT 0.20, 5, 1.476
INSERT #Critical_T_Values SELECT 0.20, 6, 1.44
INSERT #Critical_T_Values SELECT 0.20, 7, 1.415
INSERT #Critical_T_Values SELECT 0.20, 8, 1.397
INSERT #Critical_T_Values SELECT 0.20, 9, 1.383
INSERT #Critical_T_Values SELECT 0.20, 10, 1.372
INSERT #Critical_T_Values SELECT 0.20, 11, 1.363
INSERT #Critical_T_Values SELECT 0.20, 12, 1.356
INSERT #Critical_T_Values SELECT 0.20, 13, 1.35
INSERT #Critical_T_Values SELECT 0.20, 14, 1.345
INSERT #Critical_T_Values SELECT 0.20, 15, 1.341
INSERT #Critical_T_Values SELECT 0.20, 16, 1.337
INSERT #Critical_T_Values SELECT 0.20, 17, 1.333
INSERT #Critical_T_Values SELECT 0.20, 18, 1.33
INSERT #Critical_T_Values SELECT 0.20, 19, 1.328
INSERT #Critical_T_Values SELECT 0.20, 20, 1.325
INSERT #Critical_T_Values SELECT 0.20, 21, 1.323
INSERT #Critical_T_Values SELECT 0.20, 22, 1.321
INSERT #Critical_T_Values SELECT 0.20, 23, 1.319
INSERT #Critical_T_Values SELECT 0.20, 24, 1.318
INSERT #Critical_T_Values SELECT 0.20, 25, 1.316
INSERT #Critical_T_Values SELECT 0.20, 26, 1.315
INSERT #Critical_T_Values SELECT 0.20, 27, 1.314
INSERT #Critical_T_Values SELECT 0.20, 28, 1.313
INSERT #Critical_T_Values SELECT 0.20, 29, 1.311
INSERT #Critical_T_Values SELECT 0.20, 30, 1.31
INSERT #Critical_T_Values SELECT 0.20, 31, 1.309
INSERT #Critical_T_Values SELECT 0.20, 32, 1.309
INSERT #Critical_T_Values SELECT 0.20, 33, 1.308
INSERT #Critical_T_Values SELECT 0.20, 34, 1.307
INSERT #Critical_T_Values SELECT 0.20, 35, 1.306
INSERT #Critical_T_Values SELECT 0.20, 36, 1.306
INSERT #Critical_T_Values SELECT 0.20, 37, 1.305
INSERT #Critical_T_Values SELECT 0.20, 38, 1.304
INSERT #Critical_T_Values SELECT 0.20, 39, 1.304
INSERT #Critical_T_Values SELECT 0.20, 40, 1.303
INSERT #Critical_T_Values SELECT 0.20, 41, 1.303
INSERT #Critical_T_Values SELECT 0.20, 42, 1.302
INSERT #Critical_T_Values SELECT 0.20, 43, 1.302
INSERT #Critical_T_Values SELECT 0.20, 44, 1.301
INSERT #Critical_T_Values SELECT 0.20, 45, 1.301
INSERT #Critical_T_Values SELECT 0.20, 46, 1.3
INSERT #Critical_T_Values SELECT 0.20, 47, 1.3
INSERT #Critical_T_Values SELECT 0.20, 48, 1.299
INSERT #Critical_T_Values SELECT 0.20, 49, 1.299
INSERT #Critical_T_Values SELECT 0.20, 50, 1.299
INSERT #Critical_T_Values SELECT 0.20, 55, 1.297
INSERT #Critical_T_Values SELECT 0.20, 60, 1.296
INSERT #Critical_T_Values SELECT 0.20, 65, 1.295
INSERT #Critical_T_Values SELECT 0.20, 70, 1.294
INSERT #Critical_T_Values SELECT 0.20, 75, 1.293
INSERT #Critical_T_Values SELECT 0.20, 80, 1.292
INSERT #Critical_T_Values SELECT 0.20, 85, 1.292
INSERT #Critical_T_Values SELECT 0.20, 90, 1.291
INSERT #Critical_T_Values SELECT 0.20, 95, 1.291
INSERT #Critical_T_Values SELECT 0.20, 100, 1.29
INSERT #Critical_T_Values SELECT 0.20, 120, 1.289
INSERT #Critical_T_Values SELECT 0.20, 140, 1.288
INSERT #Critical_T_Values SELECT 0.20, 160, 1.287
INSERT #Critical_T_Values SELECT 0.20, 180, 1.286
INSERT #Critical_T_Values SELECT 0.20, 200, 1.286
INSERT #Critical_T_Values SELECT 0.20, 250, 1.285
INSERT #Critical_T_Values SELECT 0.20, 300, 1.284
INSERT #Critical_T_Values SELECT 0.20, 350, 1.284
INSERT #Critical_T_Values SELECT 0.20, 400, 1.284
INSERT #Critical_T_Values SELECT 0.20, 450, 1.283
INSERT #Critical_T_Values SELECT 0.20, 500, 1.283
INSERT #Critical_T_Values SELECT 0.20, 600, 1.283
INSERT #Critical_T_Values SELECT 0.20, 700, 1.283
INSERT #Critical_T_Values SELECT 0.20, 800, 1.283
INSERT #Critical_T_Values SELECT 0.20, 900, 1.282
INSERT #Critical_T_Values SELECT 0.20, 1000, 1.282
INSERT #Critical_T_Values SELECT 0.20, 2000, 1.282
INSERT #Critical_T_Values SELECT 0.20, 3000, 1.282
INSERT #Critical_T_Values SELECT 0.20, 4000, 1.282
INSERT #Critical_T_Values SELECT 0.20, 5000, 1.282
INSERT #Critical_T_Values SELECT 0.20, 1000000, 1.282

--select * from #Critical_T_Values

--Final critical T values returned as output
DECLARE @Proportion_ActualDF_To_UpperDF float  
  

  
--Determine if interpolation is needed.  If not, just pull the matching critical T values
IF EXISTS (select * from #Critical_T_Values where DegreesOfFreedom = @DegreesOfFreedom)
BEGIN
  SELECT @Exact_Match_or_Interpolate_T_Table = 'Degrees of Freedom and Critical t exact match from T Table.  No interpolation needed.'
  SELECT @Critical_T_Alpha05 = CriticalT FROM #Critical_T_Values WHERE DegreesOfFreedom = @DegreesOfFreedom AND AlphaProb = .05
  SELECT @Critical_T_Alpha10 = CriticalT FROM #Critical_T_Values WHERE DegreesOfFreedom = @DegreesOfFreedom AND AlphaProb = .10
  SELECT @Critical_T_Alpha20 = CriticalT FROM #Critical_T_Values WHERE DegreesOfFreedom = @DegreesOfFreedom AND AlphaProb = .20
END

--Interpolate T values where needed  
IF NOT EXISTS (select * from #Critical_T_Values where DegreesOfFreedom = @DegreesOfFreedom)
BEGIN

  SELECT @Exact_Match_or_Interpolate_T_Table = 'Critical t interpolated from T Table.  Results might differ slightly from stats software packages'

  --Can use any alpha to bracket upper and lower degrees of freedom from T table  
  SELECT @LowerLimit_DegFree_T_Table_For_Interpol = (select max(DegreesOfFreedom) from #Critical_T_Values where AlphaProb = .05 and DegreesOfFreedom <= @DegreesOfFreedom)
  SELECT @UpperLimit_DegFree_T_Table_For_Interpol = (select min(DegreesOfFreedom) from #Critical_T_Values where AlphaProb = .05 and DegreesOfFreedom >= @DegreesOfFreedom)


  SELECT @Lower_Crit_T_For_Interpol_Alpha05 = (select min(CriticalT) from #Critical_T_Values where AlphaProb = .05 and DegreesOfFreedom <= @DegreesOfFreedom)
  SELECT @Lower_Crit_T_For_Interpol_Alpha10 = (select min(CriticalT) from #Critical_T_Values where AlphaProb = .10 and DegreesOfFreedom <= @DegreesOfFreedom) 
  SELECT @Lower_Crit_T_For_Interpol_Alpha20 = (select min(CriticalT) from #Critical_T_Values where AlphaProb = .20 and DegreesOfFreedom <= @DegreesOfFreedom)  

  SELECT @Upper_Crit_T_For_Interpol_Alpha05 = (select max(CriticalT) from #Critical_T_Values where AlphaProb = .05 and DegreesOfFreedom >= @DegreesOfFreedom)
  SELECT @Upper_Crit_T_For_Interpol_Alpha10 = (select max(CriticalT) from #Critical_T_Values where AlphaProb = .10 and DegreesOfFreedom >= @DegreesOfFreedom) 
  SELECT @Upper_Crit_T_For_Interpol_Alpha20 = (select max(CriticalT) from #Critical_T_Values where AlphaProb = .20 and DegreesOfFreedom >= @DegreesOfFreedom)


  SELECT @Proportion_ActualDF_To_UpperDF = (@DegreesOfFreedom - @LowerLimit_DegFree_T_Table_For_Interpol) / (@UpperLimit_DegFree_T_Table_For_Interpol - @LowerLimit_DegFree_T_Table_For_Interpol)

  SELECT @Critical_T_Alpha05 = @Proportion_ActualDF_To_UpperDF * (@Upper_Crit_T_For_Interpol_Alpha05-@Lower_Crit_T_For_Interpol_Alpha05) + @Lower_Crit_T_For_Interpol_Alpha05
  SELECT @Critical_T_Alpha10 = @Proportion_ActualDF_To_UpperDF * (@Upper_Crit_T_For_Interpol_Alpha10-@Lower_Crit_T_For_Interpol_Alpha10) + @Lower_Crit_T_For_Interpol_Alpha10 
  SELECT @Critical_T_Alpha20 = @Proportion_ActualDF_To_UpperDF * (@Upper_Crit_T_For_Interpol_Alpha20-@Lower_Crit_T_For_Interpol_Alpha20) + @Lower_Crit_T_For_Interpol_Alpha20 

END  


  


SELECT @T_Of_Slope_to_Critical_T_Alpha05 = ABS(@t_Of_Slope)/@Critical_T_Alpha05 
SELECT @T_Of_Slope_to_Critical_T_Alpha10 = ABS(@t_Of_Slope)/@Critical_T_Alpha10 
SELECT @T_Of_Slope_to_Critical_T_Alpha20 = ABS(@t_Of_Slope)/@Critical_T_Alpha20 






SELECT @TrendConclusion = CASE
                            WHEN @T_Of_Slope_to_Critical_T_Alpha05 > 1 
                                   THEN 'Trend is statistically significant at P = 0.05'
                            WHEN @T_Of_Slope_to_Critical_T_Alpha05 <= 1 AND @T_Of_Slope_to_Critical_T_Alpha10 > 1 
                                   THEN 'Trend is NOT statistically significant at P = 0.05 but IS significant at P = 0.10'
                            WHEN @T_Of_Slope_to_Critical_T_Alpha10 <= 1 AND @T_Of_Slope_to_Critical_T_Alpha20 > 1 
                                   THEN 'Trend is NOT statistically significant at P = 0.05 or 0.10 but IS significant at P = 0.20'    
                            WHEN @T_Of_Slope_to_Critical_T_Alpha20 <= 1 OR @X_Counts < 25
                                   THEN 'Trend is NOT statistically significant at P = 0.05, 0.10 or 0.20.  Either no relationship, on need a larger and/or more homogeneous sample.'                                                              
                            ELSE ''
                          END


----DEBUG
--select R_SQUARE_Calc = @R_SQUARE_Calc
--select rSquare = @R_Square
--select DegreesOfFreedom = @DegreesOfFreedom
--select Critical_T_Alpha05 = @Critical_T_Alpha05
--select Critical_T_Alpha10 = @Critical_T_Alpha10
--select Critical_T_Alpha20 = @Critical_T_Alpha20
--select t_Of_Slope = @t_Of_Slope
--select T_Of_Slope_to_Critical_T_Alpha05 = @T_Of_Slope_to_Critical_T_Alpha05
--select T_Of_Slope_to_Critical_T_Alpha10 = @T_Of_Slope_to_Critical_T_Alpha10
--select T_Of_Slope_to_Critical_T_Alpha20 = @T_Of_Slope_to_Critical_T_Alpha20
--select TrendConclusion = @TrendConclusion

--return(1)  --DEBUG


----DEBUG 
--SELECT
-- Slope = @slope
--,Intercept = @Y_Intercept
--,rSquare = @R_Square
--,SalesCountInRegression = (select count(*) from #SimpleLinearRegression)
--,AvgIndependentVariable = @AvgIndependentVariable



--SELECT 
-- AvgTrendedIndependentVariable = @AvgTrendedIndependentVariable
--,AvgDependentVariable = @AvgDependentVariable
--,StdDevTrendedIndependentVariable = @StdDevTrendedIndependentVariable
--,NbrStdDeviations = @IndependentVariableWarningStdDev
--,UpperLimit = @UpperLimit
--,LowerLimit = @LowerLimit
--,TrendedIndependentVariableCounts = @TrendedIndependentVariableCounts
--,OneYearTrend = @OneYearTrend 




--return(0)
------END DEBUG


RETURN(0)

END


