IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_2YrsSalesRes')
	DROP PROCEDURE [cus].[SRCH_R_2YrsSalesRes]  
GO

CREATE PROCEDURE [cus].[SRCH_R_2YrsSalesRes]
   @CountywideY varchar(3),
   --@ApplDistrictCode varchar(20),
   @ApplDistrict varchar(20),
   @ResArea varchar(100),
   @ResSubArea varchar(3),   
   @Major char(6),           
   @AssignedAppr varchar(30),
   @StartSaleDate smalldatetime,												--Must be obligatory 
   @EndSaleDate smalldatetime,													--Must be obligatory 
   @TrendToDate smalldatetime,                                                  --Must be obligatory 
   @MarketTurnDate date, --default to COUNTYWIDE -209
   @MinRatioUsedInTrend decimal(12,2),
   @MaxRatioUsedInTrend decimal(12,2),
   @MinSalePrice int,
   @MaxSalePrice int,
   @IncludeNotAtMktY varchar(3),
   @ExcludeSaleWarnings varchar (3),--TODO ADD DEFAULT = 'Y' TO SRCHPARAM TABLE
   @ExcludeBldgWarnings varchar (3),--TODO ADD DEFAULT = 'Y' TO SRCHPARAM TABLE
   @ExcludeLandWarnings varchar (3),--TODO ADD DEFAULT = 'Y' TO SRCHPARAM TABLE
   @RatioWarningStdDev decimal(12,2),
   @LandOnlyY varchar(3),
   @MinSqFtLot int,
   @MaxSqFtLot int,
   @MinSqFtTotLiving int,
   @MaxSqFtTotLiving int,
   @MinGrade int,
   @MaxGrade int,
   @MinYrBuilt int,
   @MaxYrBuilt int,
   @MinYrRenov int,
   @MinCondition int,
   @MaxCondition int,
   @MinNbrLivingUnits int,
   @MinNbrBldgs int,
   @WtrViewYN varchar(3),
   @NonWtrViewYN varchar(3),
   @WftFootageYN varchar(3),
   @WfntLocation varchar(20), --added 12/27/2018
   @LandProblemDescr varchar(20),
   @MtchGrpSrchY varchar(3)

AS BEGIN
/*Hairo comment: aqui debe haber un slash asterisco
Author: Jairo Barquero
Date Created:  12/22/2020
Description:    SP that search residential sales data.

Modifications:
29/01/2021 - Hairo Barquero: Modification for PI_or_AU eliminates the ApplGroup filter
24/02/2021 - Hairo Barquero: Enable column LandProbDescr
24/02/2021 - Hairo Barquero: added this "cn.ptas_notetype = 591500001" to filter notes by type
mm/dd/yyyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @Error	Int
/*
Commented for TEST porpuses 

--****************************************************************************************************************************************************************
--*********ONLY FOR TEST ONLY FOR TEST*******************************************************************************************************************************************************
--****************************************************************************************************************************************************************
declare
   @CountywideY varchar(3),
   @ApplDistrictCode varchar(20), - Hairo comment eliminated since I´m going to send the Key(@ApplDistrictCode) dirrectly from the CS UI
   @ApplDistrict varchar(20),
   @ResArea varchar(100),
   @ResSubArea varchar(3),   
   @Major char(6),           
   @AssignedAppr varchar(30),
   @StartSaleDate smalldatetime,												--Must be obligatory 
   @EndSaleDate smalldatetime,													--Must be obligatory 
   @TrendToDate smalldatetime,                                                  --Must be obligatory 
   @MarketTurnDate date, --default to COUNTYWIDE -209
   @MinRatioUsedInTrend decimal(12,2),
   @MaxRatioUsedInTrend decimal(12,2),
   @MinSalePrice int,
   @MaxSalePrice int,
   @IncludeNotAtMktY varchar(3),
   @ExcludeSaleWarnings varchar (3),--TODO ADD DEFAULT = 'Y' TO SRCHPARAM TABLE
   @ExcludeBldgWarnings varchar (3),--TODO ADD DEFAULT = 'Y' TO SRCHPARAM TABLE
   @ExcludeLandWarnings varchar (3),--TODO ADD DEFAULT = 'Y' TO SRCHPARAM TABLE
   @RatioWarningStdDev decimal(12,2),
   @LandOnlyY varchar(3),
   @MinSqFtLot int,
   @MaxSqFtLot int,
   @MinSqFtTotLiving int,
   @MaxSqFtTotLiving int,
   @MinGrade int,
   @MaxGrade int,
   @MinYrBuilt int,
   @MaxYrBuilt int,
   @MinYrRenov int,
   @MinCondition int,
   @MaxCondition int,
   @MinNbrLivingUnits int,
   @MinNbrBldgs int,
   @WtrViewYN varchar(3),
   @NonWtrViewYN varchar(3),
   @WftFootageYN varchar(3),
   @WfntLocation varchar(20), --added 12/27/2018
   @LandProblemDescr varchar(20),
   @MtchGrpSrchY varchar(3)

   
select  
   @CountywideY ='',
   @ApplDistrict ='',
   @ApplDistrictCode ='',
   @ResArea = NULL--'032,058,080,440',
   @ResSubArea ='',   
   @Major ='',           
   @AssignedAppr ='',
   @StartSaleDate ='01/01/2020',												--Must be obligatory 
   @EndSaleDate ='12/31/2020',													--Must be obligatory 
   @TrendToDate ='12/31/2020',                                                  --Must be obligatory 
   @MarketTurnDate ='12/31/2020', --default to COUNTYWIDE -209
   @MinRatioUsedInTrend =0.00,
   @MaxRatioUsedInTrend =0.00,
   @MinSalePrice =10000,
   @MaxSalePrice =2000000,
   @IncludeNotAtMktY ='',
   @ExcludeSaleWarnings ='',--TODO ADD DEFAULT = 'Y' TO SRCHPARAM TABLE
   @ExcludeBldgWarnings ='',--TODO ADD DEFAULT = 'Y' TO SRCHPARAM TABLE
   @ExcludeLandWarnings ='',--TODO ADD DEFAULT = 'Y' TO SRCHPARAM TABLE
   @RatioWarningStdDev =0.00,
   @LandOnlyY ='',
   @MinSqFtLot= 0,
   @MaxSqFtLot =20000,
   @MinSqFtTotLiving =0,
   @MaxSqFtTotLiving =0,
   @MinGrade =0,
   @MaxGrade= 0,
   @MinYrBuilt =0,
   @MaxYrBuilt =0,
   @MinYrRenov =0,
   @MinCondition =0,
   @MaxCondition =0,
   @MinNbrLivingUnits =0,
   @MinNbrBldgs =0,
   @WtrViewYN ='',
   @NonWtrViewYN ='',
   @WftFootageYN ='',
   @WfntLocation ='', --added 12/27/2018
   @LandProblemDescr ='',
   @MtchGrpSrchY =''
   --@ApplDistrictFullName ='',
   --@ImpSearchOnly  ='' 
*/
--****************************************************************************************************************************************************************
--*********ONLY FOR TEST ONLY FOR TEST*******************************************************************************************************************************************************
--****************************************************************************************************************************************************************

--IF OBJECT_ID('Tempdb..#PropType') IS NOT NULL
--DROP TABLE Tempdb..#PropType;

CREATE TABLE #PropType
(
ptas_propertyTypeId UNIQUEIDENTIFIER PRIMARY KEY ,
PropType NVARCHAR(1)
)
INSERT INTO #PropType
SELECT  ptas_propertyTypeId       
		,SUBSTRING(pt.ptas_name,1,1) 
FROM dynamics.ptas_propertytype AS pt


--select * from AssmtYr 
DECLARE @RPAssmtYr int  --debug to run ad hoc
--SELECT @RPAssmtYr = RPAssmtYr FROM AssmtYr Hairo comment: this needs to be uncommented as we determinate where the Assesment year is coming 

--Hairo comment: this line should be deleted after we determinate where the year is coming from
SELECT @RPAssmtYr = 2020

DECLARE @AdminFactorApplied float
SELECT @AdminFactorApplied = 0.925 --Hairo comment: this value was hardcoded in the original SP 


IF ISNULL(@CountywideY,'') = '' SELECT @CountywideY = ''
IF ISNULL(@ApplDistrict,'') = '' SELECT @ApplDistrict = ''
--IF ISNULL(@ApplDistrictCode,'') = '' SELECT @ApplDistrictCode = ''
IF ISNULL(@ResArea,'') = '' SELECT @ResArea = ''
IF ISNULL(@ResSubArea,'') = '' OR ISNULL(@ResSubArea,'') = '0' SELECT @ResSubArea = ''
IF ISNULL(@Major,'') = '' OR ISNULL(@Major,'') = '0' SELECT @Major = ''
IF ISNULL(@AssignedAppr,'') = '' SELECT @AssignedAppr = ''
IF ISNULL(@StartSaleDate,'') = '' SELECT '1/'+'1/'+ (select convert(varchar(2),datepart(yy,RpAssmtYr)) - 3 from AssmtYr)
IF ISNULL(@EndSaleDate,'') = '' SELECT GETDATE()
IF ISNULL(@TrendToDate,'') = '' SELECT GETDATE()
IF ISNULL(@MarketTurnDate,'') = '' SELECT @MarketTurnDate = '1/1/1900'
IF ISNULL(@MinRatioUsedInTrend,0) = 0 SELECT @MinRatioUsedInTrend = 0.50
IF ISNULL(@MaxRatioUsedInTrend,0) = 0 SELECT @MaxRatioUsedInTrend = 1.75
IF ISNULL(@MinSalePrice,'') = '' SELECT @MinSalePrice = 0
IF ISNULL(@MaxSalePrice,'') = '' SELECT @MaxSalePrice = 100000000
IF ISNULL(@IncludeNotAtMktY,'') = '' SELECT @IncludeNotAtMktY = 'N'
IF ISNULL(@ExcludeSaleWarnings,'') = '' SELECT @ExcludeSaleWarnings = ''
IF ISNULL(@ExcludeBldgWarnings,'') = '' SELECT @ExcludeBldgWarnings = ''
IF ISNULL(@ExcludeLandWarnings,'') = '' SELECT @ExcludeLandWarnings = ''
IF ISNULL(@LandOnlyY,'') = '' SELECT @LandOnlyY = 'N' 
IF ISNULL(@MinSqFtLot,'') = '' SELECT @MinSqFtLot = 0
IF ISNULL(@MaxSqFtLot,'') = '' SELECT @MaxSqFtLot = 100000000 
IF ISNULL(@MinSqFtTotLiving,'') = '' SELECT @MinSqFtTotLiving = 0
IF ISNULL(@MaxSqFtTotLiving,'') = '' SELECT @MaxSqFtTotLiving = 1000000 
IF ISNULL(@MinGrade,'') = '' SELECT @MinGrade = 0
IF ISNULL(@MaxGrade,'') = '' SELECT @MaxGrade = 21
IF ISNULL(@MinYrBuilt,'') = '' SELECT @MinYrBuilt = 0
IF ISNULL(@MaxYrBuilt,'') = '' SELECT @MaxYrBuilt = 3000
IF ISNULL(@MinYrRenov,'') = '' SELECT @MinYrRenov = 0 
IF ISNULL(@MinCondition,'') = '' SELECT @MinCondition = 0
IF ISNULL(@MaxCondition,'') = '' SELECT @MaxCondition = 10
IF ISNULL(@MinNbrLivingUnits,'') = '' SELECT @MinNbrLivingUnits = 0
IF ISNULL(@MinNbrBldgs,'') = '' SELECT @MinNbrBldgs = 0  --ResNbrImps
IF ISNULL(@WtrViewYN,'') = '' SELECT @WtrViewYN = ''
IF ISNULL(@NonWtrViewYN,'') = '' SELECT @NonWtrViewYN = ''
IF ISNULL(@WftFootageYN,'') = '' SELECT @WftFootageYN = ''
IF ISNULL(@WfntLocation,'') = '' SELECT @WfntLocation = ''
IF ISNULL(@LandProblemDescr,'') = '' SELECT @LandProblemDescr = ''
IF ISNULL(@MtchGrpSrchY,'') = '' SELECT @MtchGrpSrchY = ''


--search control does not display correctly with varchar1, so trim varchar 3 if anyone added a space to their input
IF @CountywideY <> '' SELECT @CountywideY= RTRIM(LTRIM(@CountywideY))
IF @ApplDistrict <> '' SELECT @ApplDistrict= RTRIM(LTRIM(@ApplDistrict))
--IF @ApplDistrictCode <> '' SELECT @ApplDistrictCode= RTRIM(LTRIM(@ApplDistrictCode))
IF @ResSubArea <> '' SELECT @ResSubArea= RTRIM(LTRIM(@ResSubArea))





DECLARE @ResAreaParam TABLE ([ResAreaId] int)

DECLARE @pos INT,
		@AreaNumber int
		
		
IF @ResArea <> '' 
BEGIN 
	WHILE CHARINDEX(',', @ResArea) > 0
		 BEGIN
		  SELECT @pos  = CHARINDEX(',', @ResArea)  
		  SELECT @AreaNumber = cast(SUBSTRING(@ResArea, 1, @pos-1) as int)

		  INSERT INTO @ResAreaParam 
		  SELECT @AreaNumber

		  SELECT @ResArea = SUBSTRING(@ResArea, @pos+1, LEN(@ResArea)-@pos)
		 END

	INSERT INTO @ResAreaParam
	SELECT @ResArea
END


	IF (select count(*) FROM @ResAreaParam) = 0
	BEGIN
	  INSERT @ResAreaParam
	  SELECT DISTINCT CONVERT(int,ResArea) 
	  FROM dynamics.vw_GisMapData  
	  WHERE ISNULL(ResArea,'') <> ''
	  ORDER BY CONVERT(int,ResArea)
	END



--Some of the criteria below brings in vacant parcels (e.g. want to see all poor cond imps, but Condition <= 1 also brings in 0 (vacant).
--Set a flag if only want imp pcls
DECLARE @ImpSearchOnly  char(1)
SELECT @ImpSearchOnly = 'N'

IF    @MinSqFtTotLiving > 0
   OR @MaxSqFtTotLiving <> 1000000      
   OR @MinGrade > 0
   OR @MaxGrade <> 21
   OR @MinYrBuilt > 0
   OR @MaxYrBuilt <> 3000   
   OR @MinYrRenov > 0   
   OR @MinCondition > 0   
   OR @MaxCondition <> 10
   OR @MinNbrLivingUnits > 0   
   OR @MinNbrBldgs > 0   
SELECT @ImpSearchOnly = 'Y'



/*
Hairo comment this code is not required anymore since I´m going to send the Key(@ApplDistrictCode) directly from the CS UI 
DECLARE @ApplDistrictCode  varchar(20)

IF @ApplDistrict = '' SELECT @ApplDistrictCode = ''


IF @ApplDistrict IS NOT NULL OR @ApplDistrict <> ''
BEGIN
	SELECT @ApplDistrictCode = attributevalue
	  FROM dynamics.stringmap dsm
	 WHERE dsm.objecttypecode = 'ptas_parceldetail'
	   AND dsm.attributename  = 'ptas_residentialdistrict'
	   AND dsm.value 		  = @ApplDistrict
END
*/

--Following is for analyzing only parcels with a certain characteristic, especially WFT
DECLARE @SubGroupingType  varchar(100)
SELECT @SubGroupingType = ''

IF 
   	@LandOnlyY <> ''
OR	 @MinSqFtLot > 0
OR		@MaxSqFtLot < 100000000
OR		@MinSqFtTotLiving > 0
OR		@MaxSqFtTotLiving < 1000000
OR		@MinGrade > 0
OR		@MaxGrade < 21
OR		@MinYrBuilt > 0
OR		@MaxYrBuilt < 3000
OR		@MinYrRenov > 0
OR		@MinCondition > 0
OR		@MaxCondition < 10
OR		@MinNbrLivingUnits > 0
OR		@MinNbrBldgs > 0
OR		@WtrViewYN <> ''
OR		@NonWtrViewYN <> ''
OR		@WftFootageYN <> ''
--OR	 @WfntLocation <> ''
OR		@LandProblemDescr <> ''
BEGIN 
SELECT @SubGroupingType = 
    CASE WHEN @MinSqFtLot > 0 THEN '@MinSqFtLot=' + CONVERT(VARCHAR(12),@MinSqFtLot) + ' ' ELSE '' END
  + CASE WHEN @MaxSqFtLot < 100000000 THEN '@MaxSqFtLot=' + CONVERT(VARCHAR(12),@MaxSqFtLot) + ' ' ELSE '' END
  + CASE WHEN @MinSqFtTotLiving > 0 THEN '@MinSqFtTotLiving=' + CONVERT(VARCHAR(12),@MinSqFtTotLiving) + ' ' ELSE '' END
  + CASE WHEN @MaxSqFtTotLiving < 1000000 THEN '@MaxSqFtTotLiving=' + CONVERT(VARCHAR(12),@MaxSqFtTotLiving) + ' ' ELSE '' END
  + CASE WHEN @MinGrade > 0 THEN '@MinGrade=' + CONVERT(VARCHAR(12),@MinGrade) + ' ' ELSE '' END
  + CASE WHEN @MaxGrade < 21 THEN '@MaxGrade=' + CONVERT(VARCHAR(12),@MaxGrade) + ' ' ELSE '' END  
  + CASE WHEN @MinYrBuilt > 0 THEN '@MinYrBuilt=' + CONVERT(VARCHAR(12),@MinYrBuilt) + ' ' ELSE '' END  
  + CASE WHEN @MaxYrBuilt < 3000 THEN '@MaxYrBuilt=' + CONVERT(VARCHAR(12),@MaxYrBuilt) + ' ' ELSE '' END  
  + CASE WHEN @MinYrRenov > 0 THEN '@MinYrRenov=' + CONVERT(VARCHAR(12),@MinYrRenov) + ' ' ELSE '' END  
  + CASE WHEN @MinCondition > 0 THEN '@MinCondition=' + CONVERT(VARCHAR(12),@MinCondition) + ' ' ELSE '' END 
  + CASE WHEN @MaxCondition < 10 THEN '@MaxCondition=' + CONVERT(VARCHAR(12),@MaxCondition) + ' ' ELSE '' END  
  + CASE WHEN @MinNbrLivingUnits > 0 THEN '@MinNbrLivingUnits=' + CONVERT(VARCHAR(12),@MinNbrLivingUnits) + ' ' ELSE '' END  
  + CASE WHEN @MinNbrBldgs > 0 THEN '@MinNbrBldgs=' + CONVERT(VARCHAR(12),@MinNbrBldgs) + ' ' ELSE '' END  
  + CASE WHEN 	@WtrViewYN <> '' THEN '@WtrViewYN=' + @WtrViewYN + ' ' ELSE '' END 
  + CASE WHEN 	@NonWtrViewYN <> '' THEN '@NonWtrViewYN=' + @NonWtrViewYN + ' ' ELSE '' END   
  + CASE WHEN  @WftFootageYN <> '' THEN '@WftFootageYN=' + @WftFootageYN + ' ' ELSE '' END    
  + CASE WHEN  @WfntLocation <> '' THEN '@WfntLocation=' + @WfntLocation + ' ' ELSE '' END  
  + CASE WHEN 	@LandProblemDescr <> '' THEN '@LandProblemDescr=' + @LandProblemDescr + ' ' ELSE '' END  
END



  CREATE TABLE #SaleId 
  (
    RecId int Identity(1,1) 
  , SaleGuid uniqueidentifier
  , SaleId Varchar(100)  
  , ParcelId uniqueidentifier
  , SaleDate smalldatetime
  , SaleDay int
  , SalePrice int
  , AtMarket varchar(10) default '' 
  , PropCnt int
  , SaleTypeRP varchar(25)  default ''
  , SpSqFtLnd decimal(20,2) default 0
  , SpTotLivImps decimal(20,2) default 0
  , AVSP decimal(20,2) default 0
  , PrevAVSP decimal(20,2) default 0
  , SellerName varchar(300)
  , BuyerName varchar(300)    
  , PossibleLenderSale char (1) default 'N'
  , PossibleLenderSaleDetails varchar (50)
  , SaleWarnings varchar (300)
  , MultipleSales char (1) default ''
  , SellerNameWarning varchar (255) default ''
  , BuyerNameWarning varchar (255) default ''
  , SaleWarningToReview varchar (255) default '' 
  , BuildingWarning varchar (255) default ''  
  , LandWarning varchar (255) default ''
  , RatioWarning varchar (255) default ''
  , ExciseTaxNbr int
  )
CREATE INDEX Idx_SaleGuid on #SaleId	  
(
SaleGuid
)

	SELECT   
			gmd.PIN	
			,gmd.Major
			,gmd.Minor
			,gmd.ParcelId
			,gmd.LndGuid
			,gmd.ResArea
			,gmd.Folio
			,gmd.AssignedBoth
			,gmd.ApplDistrict
			--,gmd.GeneralClassif --Hairo comment: this have to be activated when calculated in GisMapData view
			,gmd.SqFtLot
			,gmd.SqFtTotLiving 
			,gmd.BldgGrade
			,gmd.YrBuiltRes
			,gmd.YrRenovated
			,gmd.Condition
			,gmd.NbrLivingUnits
			,gmd.ResNbrImps
			,gmd.WaterViews
			,gmd.NonWaterViews
			,gmd.WfntFootage 
			,gmd.WfntLocation
			--,gmd.LandProbDescrPart1	--Hairo comment: this have to be activated when calculated in GisMapData view
			--,gmd.LandProbDescrPart2   --Hairo comment: this have to be activated when calculated in GisMapData view
			,gmd.LandVal
			,gmd.PrevLandVal
			,gmd.TotVal
			,gmd.PrevTotVal
			,gmd.SalesCountUnverified
			,gmd.SalesCountVerifiedThisCycle
			,gmd.ResSubArea 
			,gmd.ResNbhd 
			,gmd.TaxPayerName
			,gmd.DistrictName
			,gmd.PropType
			,gmd.ApplGroup
			,gmd.TaxStatus
			,gmd.YrBltRen
		    ,gmd.UnfinArea 
		    ,gmd.PcntComplRes		
		    ,gmd.Obsolescence	
		    ,gmd.PcntNetCondition	
		    ,gmd.CondDescr		
		    ,gmd.NbrResAccys 
		    ,gmd.AccyPRKDETGAR 
		    ,gmd.AccyPRKCARPORT 
		    ,gmd.AccyPVCONCRETE 
		    ,gmd.AccyPOOL
		    ,gmd.AccyMISCIMP 
		    ,gmd.WfntBank_value 
		    ,gmd.AdjacentGolfFairway 
		    ,gmd.AdjacentGreenbelt 
		    ,gmd.BaseLandVal
		    ,gmd.PcntBaseLandValImpacted
		    ,gmd.BaseLandValTaxYr
		    ,gmd.BaseLandValDate 
		    ,gmd.BLVSqFtCalc
		    ,gmd.ImpsVal
		    ,gmd.NewConstrVal
			,gmd.SelectMethod 
			,gmd.PrevImpsVal
			,gmd.PrevNewConstrVal
			,gmd.PrevSelectMethod
			,gmd.PcntChgLand
			,gmd.PcntChgImps
			,gmd.PcntChgTotal		
			,gmd.AddrLine
			--,gmd.FrozPresentUse 	--Hairo comment: this have to be activated when calculated in GisMapData view
			--,gmd.FrozSqFtLot	    --Hairo comment: this have to be activated when calculated in GisMapData view
			,gmd.PresentUse			
			,gmd.CurrentZoning
			,gmd.ViewDescr		
			,gmd.LandProbDescrPart1
			,gmd.LandProbDescrPart2
			INTO #GisMapDataSubSet
		FROM [dynamics].[vw_GisMapData] gmd
		WHERE EXISTS ( SELECT 1
					     FROM dynamics.ptas_parceldetail dpd 
						INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
						INNER JOIN @ResAreaParam rap
						ON rap.ResAreaId = dpd.ptas_resarea
					    INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
						   ON spdps.ptas_parceldetailid = dpd.ptas_parceldetailid
					    INNER JOIN [dynamics].[ptas_sales] dps
						   ON dps.ptas_salesid = spdps.ptas_salesid
					      AND ((dps.ptas_atmarket <> 'N' AND @IncludeNotAtMktY = 'N' ) OR @IncludeNotAtMktY = 'Y')        
					      AND dps.ptas_saledate >= @StartSaleDate
					      AND dps.ptas_saledate <= @EndSaleDate
					      AND dps.ptas_saleprice >= @MinSalePrice
					      AND dps.ptas_saleprice <= @MaxSalePrice  
				        WHERE pt.PropType  = 'R'
					      AND dpd.ptas_applgroup = 'R'
					      AND gmd.ParcelId = dpd.ptas_parceldetailid)


	  
  INSERT #SaleId (SaleId, SaleGuid, ParcelId, SaleDate, SaleDay, SalePrice, AtMarket ,PropCnt, SellerName, BuyerName, ExciseTaxNbr)
  SELECT DISTINCT 
			 Saleid 	= dps.ptas_name
			,SaleGuid 	= dps.ptas_salesid
			,ParcelId 	= gmd.ParcelId
			,SaleDate	= dps.ptas_saledate
			,SaleDay  	= DATEDIFF(day,@TrendToDate,dps.ptas_saledate)   
			,SalePrice	= dps.ptas_saleprice
			,AtMarket	= COALESCE(dps.ptas_atmarket,'(none)')			
			,PropCnt	= dps.ptas_nbrparcels				
			,SellerName = COALESCE(dps.ptas_grantorfirstname,'') +' '+COALESCE(dps.ptas_grantorlastname,'') 
			,BuyerName  = COALESCE(dps.ptas_granteefirstname,'') +' '+COALESCE(dps.ptas_granteelastname,'')
			,ExciseTaxNbr = dps.ptas_name
	 FROM #GisMapDataSubSet gmd 
	INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
	   ON spdps.ptas_parceldetailid = gmd.ParcelId
	INNER JOIN [dynamics].[ptas_sales] dps
	   ON dps.ptas_salesid = spdps.ptas_salesid
	
	WHERE ((gmd.AssignedBoth = @AssignedAppr AND @AssignedAppr <>'' ) OR @AssignedAppr = '')
	AND ((dps.ptas_atmarket <> 'N' AND @IncludeNotAtMktY = 'N' ) OR @IncludeNotAtMktY = 'Y')        
	AND dps.ptas_saledate >= @StartSaleDate
	AND dps.ptas_saledate <= @EndSaleDate
	AND dps.ptas_saleprice >= @MinSalePrice
	AND dps.ptas_saleprice <= @MaxSalePrice  
	AND ((gmd.ParcelId IS NOT NULL AND @CountywideY <> '') OR @CountywideY = '')       
	AND ((gmd.ApplDistrict =  @ApplDistrict AND @ApplDistrict <> '') OR @ApplDistrict = '')    
	--AND  ((@ImpSearchOnly = 'Y' AND gmd.GeneralClassif = 'ResImp') OR @ImpSearchOnly  <> 'Y')   --Hairo comment: this have to be activated when calculated in GisMapData view
	AND  ((@LandOnlyY = 'Y' AND dps.ptas_salepropertyclass = 7  OR EXISTS (SELECT  *
																		     FROM [dynamics].[ptas_sales_ptas_saleswarningcode] sswc
																			INNER JOIN [dynamics].[ptas_saleswarningcode] swc
																			   ON sswc.ptas_saleswarningcodeid = swc.ptas_saleswarningcodeid
																		      AND swc.ptas_id = '10' --TEAR DOWN
																			WHERE sswc.ptas_salesid = dps.ptas_salesid
																		    )) 
																		    OR @LandOnlyY  <> 'Y')   
	AND   gmd.SqFtLot >= @MinSqFtLot
	AND   gmd.SqFtLot <= @MaxSqFtLot  
	AND  (gmd.SqFtTotLiving >= @MinSqFtTotLiving)--0
	AND  (gmd.SqFtTotLiving <= @MaxSqFtTotLiving)--1000000          
	AND  (gmd.BldgGrade >= @MinGrade)-- 0
	AND  (gmd.BldgGrade <= @MaxGrade) -- 21
	AND  (gmd.YrBuiltRes >= @MinYrBuilt)--0
	AND  (gmd.YrBuiltRes <= @MaxYrBuilt)
	AND  (gmd.YrRenovated >= @MinYrRenov)--0     
	AND  (gmd.Condition >= @MinCondition) --0
	AND  (gmd.Condition <= @MaxCondition) --10
	AND  (gmd.NbrLivingUnits >= @MinNbrLivingUnits) --0
	AND  (gmd.ResNbrImps >= @MinNbrBldgs) --0   
	AND  ((gmd.WaterViews > 0 AND @WtrViewYN = 'Y') OR (gmd.WaterViews = 0 AND @WtrViewYN = 'N') OR @WtrViewYN = '')   
	AND  ((gmd.NonWaterViews > 0 AND @NonWtrViewYN = 'Y') OR (NonWaterViews = 0 AND @NonWtrViewYN = 'N') OR @NonWtrViewYN = '') 
	AND  ((@WftFootageYN = 'Y' AND gmd.WfntFootage > 0) OR (@WftFootageYN = 'N' AND gmd.WfntFootage = 0) OR @WftFootageYN  <> 'Y')
	AND  ((@WfntLocation <> '' AND gmd.WfntLocation Like '%'+@WfntLocation+'%') OR @WfntLocation = '') 
	--AND  ((@LandProblemDescr <> '' AND gmd.LandProbDescrPart1+gmd.LandProbDescrPart2 Like '%'+@LandProblemDescr+'%') OR @LandProblemDescr = '')  --Hairo comment: this have to be activated when calculated in GisMapData view

	ORDER BY dps.ptas_saledate
  


 
 
 
--prop class is the only frozen characteristic for res.  7= vac land at time of sale.   Tear down code also indicates a land sale
--Set SaleTypeRP same for all parcels in sale.  If there is a land parcel and imp/mh/accy parcel in multiparcel sale, then set as imp/mh/accy

/* 
Hairo comment: in order to calculate this is required to get MHOMEREAL from GisMapData view 

UPDATE #SaleId  SET SaleTypeRP	= 'ResImpSale' FROM #SaleId s WHERE EXISTS (select * from GisMapData gmd where gmd.RpGuid = s.RpGuid and gmd.ResNbrImps > 0)
UPDATE #SaleId  SET SaleTypeRP	= 'ResMHSale' FROM #SaleId s WHERE EXISTS (select * from GisMapData gmd where gmd.RpGuid = s.RpGuid and gmd.ResNbrImps = 0 and MHOMEREAL > 0)
UPDATE #SaleId  SET SaleTypeRP	= 'ResAccySale' FROM #SaleId s WHERE EXISTS (select * from GisMapData gmd where gmd.RpGuid = s.RpGuid and gmd.ResNbrImps = 0 and MHOMEREAL = 0
                                                                            and  AccyPRKDETGAR + AccyPRKCARPORT + AccyPOOL + AccyMISCIMP > 0  )
*/

 
 --prop class is the only frozen characteristic for res.  7= vac land at time of sale.   Tear down code also indicates a land sale
 UPDATE #SaleId 
	  SET SaleTypeRP = CASE WHEN dps.ptas_salepropertyclass = 8 AND gmd.ResNbrImps > 0			THEN 'ResImpSale'
							WHEN dps.ptas_salepropertyclass = 7 								THEN 'ResLandSale'
							--WHEN dps.ptas_salepropertyclass = 9 AND gmd.ResNbrImps = 0	THEN 'ResMHSale'
							WHEN gmd.ResNbrImps = 0	AND ptas_mobilehometype = 4		THEN 'ResMHSale'
							WHEN AccyPRKDETGAR + AccyPRKCARPORT + AccyPOOL + AccyMISCIMP > 0	THEN 'ResAccySale'
						ELSE ''
					END
	 FROM #SaleId s 
 	INNER JOIN #GisMapDataSubSet gmd ON s.ParcelId = gmd.ParcelID
	INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps ON spdps.ptas_parceldetailid = s.ParcelId AND s.SaleGuid = spdps.ptas_salesid
	INNER JOIN [dynamics].[ptas_sales] dps ON dps.ptas_salesid = spdps.ptas_salesid
	LEFT JOIN dynamics.ptas_condounit pc ON s.ParcelId = pc._ptas_parcelid_value
/*
   UPDATE #SaleId 
	  SET SaleTypeRP	= 'ResLandSale'
	 FROM #SaleId s 
	INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
	   ON spdps.ptas_parceldetailid = s.ParcelId
	INNER JOIN [dynamics].[ptas_sales] dps
	   ON dps.ptas_salesid = spdps.ptas_salesid
	WHERE dps.ptas_salepropertyclass = 7
*/



--Get all res sales for calcs below, but use sum of SqFtLot etc. for multi-pcl
--drop table #ResMultiAggr
CREATE TABLE #ResMultiAggr 
 (
SaleGuid uniqueidentifier,
ExciseTaxNbr int,
SalePrice int,
SaleTypeRP varchar(25),
PropCnt int, 
ResNbrImps int default 0,
SqFtTotLiving	int	default 0,
NbrLivingUnits	smallint	default 0, 		 	
SqFtLot	 int	default 0,
LandVal bigint 	default 0,
PrevLandVal bigint 	default 0,
TotVal bigint 	default 0,
PrevTotVal bigint 	default 0
  )
  
INSERT #ResMultiAggr
SELECT 
 s.SaleGuid
,s.ExciseTaxNbr
,s.SalePrice
,SaleTypeRP = ''
,s.PropCnt 
,SUM(gmd.ResNbrImps)
,SUM(gmd.SqFtTotLiving)
,SUM(gmd.NbrLivingUnits)
,SUM(gmd.SqFtLot)
,SUM(gmd.LandVal)
,SUM(gmd.PrevLandVal)
,SUM(gmd.TotVal)
,SUM(gmd.PrevTotVal)
FROM #GisMapDataSubSet gmd 
INNER JOIN #SaleId s ON gmd.ParcelId = s.ParcelId
GROUP BY s.SaleGuid, s.ExciseTaxNbr ,s.SalePrice, s.PropCnt


UPDATE #ResMultiAggr
SET SaleTypeRP = s.SaleTypeRP
FROM #ResMultiAggr rma
INNER JOIN #SaleId s ON rma.SaleGuid =  s.SaleGuid

--If there is an land sale and non-land sale, call the package a non-land sale
UPDATE #ResMultiAggr
SET SaleTypeRP = s.SaleTypeRP
FROM #ResMultiAggr rma
INNER JOIN #SaleId s ON rma.SaleGuid =  s.SaleGuid
WHERE s.SaleTypeRP <> 'ResLandSale'
AND EXISTS (select * from #SaleId s2 where s2.SaleGuid = s.SaleGuid and s2.ParcelId <> s.ParcelId and s2.SaleTypeRP = 'ResLandSale')



UPDATE #SaleId
SET AVSP = convert(decimal(20,2),rma.LandVal)/convert(decimal(20,2),rma.SalePrice)
FROM #SaleId s 
INNER JOIN #ResMultiAggr rma ON rma.SaleGuid =  s.SaleGuid
WHERE rma.LandVal > 0 AND rma.SalePrice > 0
AND rma.SaleTypeRP	= 'ResLandSale'


UPDATE #SaleId
SET PrevAVSP = convert(decimal(20,2),rma.PrevLandVal)/convert(decimal(20,2),rma.SalePrice)
FROM #SaleId s 
INNER JOIN #ResMultiAggr rma ON rma.SaleGuid =  s.SaleGuid
WHERE rma.PrevLandVal > 0 AND rma.SalePrice > 0
AND rma.SaleTypeRP	= 'ResLandSale'


UPDATE #SaleId
SET AVSP = convert(decimal(20,2),rma.TotVal)/convert(decimal(20,2),rma.SalePrice)
FROM #SaleId s 
INNER JOIN #ResMultiAggr rma ON rma.SaleGuid =  s.SaleGuid
WHERE rma.TotVal > 0 AND rma.SalePrice > 0
AND rma.SaleTypeRP	<> 'ResLandSale'


UPDATE #SaleId
SET PrevAVSP = convert(decimal(20,2),rma.PrevTotVal)/convert(decimal(20,2),rma.SalePrice)
FROM #SaleId s 
INNER JOIN #ResMultiAggr rma ON rma.SaleGuid =  s.SaleGuid
WHERE rma.PrevTotVal > 0 AND rma.SalePrice > 0
AND rma.SaleTypeRP	<> 'ResLandSale'


UPDATE #SaleId
SET PossibleLenderSale = 'Y'
FROM #SaleId s
WHERE (SellerName like '%FORECLOSURE%'
     or SellerName like '%TRUST%' --TRUST or TRUSTEE  --oh-oh, will this let estate sales in?
     or SellerName like '%TITLE%'
     or SellerName like '%SERVICES%'
     or SellerName like '%ACCEPTANCE%'
     or SellerName like '%BENEFICIAL%'
     or SellerName like '%VETERANS%'
     or SellerName like '%MUTUAL%'
     or SellerName like '%COMMERCE%'
     or SellerName like '%SERVICES%'
     or SellerName like '%MONEY%'
     or SellerName like '%BANK%' --BANK or BANKRUP
     or SellerName like '%SAVINGS%' 
     or SellerName like '%FINANCIAL%'
     or SellerName like '%FEDERAL%'
     or SellerName like '%EQUITY%'
     or SellerName like '%FINANC%' --FINANCIAL or FINANCE
     or SellerName like '%CAPITAL%'
     or SellerName like '%CREDIT%'
     or SellerName like '%MORTGAGE%'
     or SellerName like '%MTG%'
     or SellerName like '%LOAN%'
  and  SellerName NOT like '%RELOC%'
  and  SellerName NOT like '%LIVING%'
  and  SellerName NOT like '%LVG%'
  and  SellerName NOT like '%LAND TRUST%'
  and  SellerName NOT like '%FAMILY TRUST%'
  and  SellerName NOT like '%REV TRUST%'
  and  SellerName NOT like '%REVOCABLE%'

  and  BuyerName NOT like '%FORECLOSURE%'
  and  BuyerName NOT like '%TRUST%' --TRUST or TRUSTEE
  and  BuyerName NOT like '%TITLE%'
  and  BuyerName NOT like '%SERVICES%'
  and  BuyerName NOT like '%MONEY%'
  and  BuyerName NOT like '%ACCEPTANCE%'
  and  BuyerName NOT like '%BENEFICIAL%'
  and  BuyerName NOT like '%VETERANS%'
  and  BuyerName NOT like '%MUTUAL%'
  and  BuyerName NOT like '%COMMERCE%'
  and  BuyerName NOT like '%SERVICES%'
  and  BuyerName NOT like '%BANK%'
  and  BuyerName NOT like '%SAVINGS%'
  and  BuyerName NOT like '%FINANCIAL%'
  and  BuyerName NOT like '%FEDERAL%'
  and  BuyerName NOT like '%EQUITY%'
  and  BuyerName NOT like '%FINANC%' --FINANCIAL or FINANCE
  and  BuyerName NOT like '%CAPITAL%'
  and  BuyerName NOT like '%CREDIT%'
  and  BuyerName NOT like '%MORTGAGE%'
  and  BuyerName NOT like '%MTG%'
  and  BuyerName NOT like '%LOAN%'
  and  BuyerName NOT like '%RELOC%'
  )


UPDATE #SaleId
SET MultipleSales = 
      (CASE WHEN (select count(*) from #SaleId s2 WHERE s2.ParcelId = s.ParcelId) > 1 THEN 'Y' ELSE '' END)
FROM #SaleId s


UPDATE #SaleId
SET SellerNameWarning = 
 --------------------------RELOCATION-------------------------------------------------------
      (CASE      
      WHEN SellerName like '%NATIONAL RESIDENTIAL NOMINE%'
        or SellerName like '%NATIONAL RESIDENTIAL SERVICE%' 
        or SellerName like '%NATL RESIDENTIAL NOMINE%' 
        or SellerName like '%NATIONWIDE MUTUAL%' 
        or SellerName like '%RELOCATION%' 
        or SellerName like '%MOBILITY%' 
        or SellerName like '%CARTUS FINANC%' 
        or SellerName like '%CARTUS CORP%'
        or SellerName like '%PHH R%'
        or SellerName like '%PHH H%'
        or SellerName like '%P H H H%'
        or SellerName like '%FRANCONIA%' 
        or SellerName like '%NATIONAL TRANSFER SERVICE%' 
        or SellerName like '%GRSW STEWART%' 
        or SellerName like '%DODGE NP%'
        or SellerName like '%DODGE N P%'
        or SellerName like '%NATIONAL EQUITY%'
        and SellerName not like '%INTERNATIONAL%' 
        or SellerName like '%LEXICON GOVERNMENT%' 
        or SellerName like '%OLD REPUBLIC DIVERSIFIED%' 
        or SellerName like '%RAS CLOSING%' 
        or SellerName like '%RELO DIRECT%'  
        or SellerName like '%STONE FINANC%' 
        or SellerName like '%WEICHERT WORKFORCE%' 
        or SellerName like '%AMERICAN ESCROW & CLOSING%' 
        or SellerName like '%BGRS LLC%' 
        or SellerName like '%PCL CONSTRUCTION ENTERPRISES%' 
        or SellerName like '%SIRVA RELO%' 
        THEN 'RELOCATION'
 --------------------------RELO or FORECLOSURE------------------------------------------------------------     
      WHEN SellerName like '%OLD REPUBLIC%' 
        THEN 'RELO/FORECLOSURE?'
---------------------------AFFORDABLE HOUSING-------------------------------------------------------------
      WHEN SellerName like '%HABITAT FOR HUMANITY%' 
        or SellerName like '%HABITAT OF HUMANITY%' 
        or SellerName like '%HOMESTEAD COMMUNITY LAND%' 
        or SellerName like '%HOUSING AUTHORITY%' 
        or SellerName like '%HOMESIGHT%'
        THEN 'AFFORDABLE HOUSING'
---------------------------ESTATE---------------------------------------------------------------------------
      WHEN SellerName like '%ADMINISTRATOR%' 
        and SellerName not like 'VETS AFFAIRS'
        and SellerName not like 'VETERAN' 
        or SellerName like '%ESTATE OF%'  
        or SellerName like '%PERS REP%' 
        or SellerName like '%-PR%' 
        and SellerName not like '%-PRO%'
        and SellerName not like '%-PR2%'
        or SellerName like '%-SOLE HEIR%'
        or SellerName like '%-HEIR%' 
        or SellerName like '% HEIR %'
        or SellerName like '%DEVISE%'
        or SellerName like '%EXECUTRIX%'
        THEN 'ESTATE'
----------------------------GUARDIAN-------------------------------------------------------------------------
      WHEN SellerName like '%PUGET SOUND GUARDIAN%' 
        or SellerName like '%GUARDIANSHIP%' 
        THEN 'GUARDIAN'
--------------------DID NOT ADD FORECLOSURE LIST TO BUYERNAMEWARNING -----------------------------------------
      WHEN SellerName like '%QUALITY LOAN%' 
        or SellerName like '%RAINIER FOR%' 
        or SellerName like '%PREMIER M%'
        or SellerName like '%PETIPRIN BENJAMIN%'  
        or SellerName like '%GIBBON KAREN L%' 
        or SellerName like '%REED LONGYEAR MALNATI%'  
        or SellerName like '%BISHOP MARSHALL & WEIBEL%' 
        or SellerName like '%WEINSTEIN & RILEY%' 
        or SellerName like '%SEASIDE TRUSTEE%'  
        or SellerName like '%NATIONWIDE TRUSTEE%' 
        or SellerName like '%NORTH CASCADE TRUSTEE%' 
        or SellerName like '%NORTHWEST TRUSTEE%'
        or SellerName like '%NW TRUSTEE%' 
        or SellerName like '%LSI TITLE%' 
        or SellerName like '%LANDSAFE%'
        or SellerName like '%LAND SAFE%'
        or SellerName like '%FIRST AMER%'
        or SellerName like '%FIDELITY NAT%' 
        or SellerName like '%LAWYERS TITLE%'
        or SellerName like '%LAWYER TITLE%'
        or SellerName like '%CHICAGO TITLE%'
        or SellerName like '%PIONEER TITLE%'
        or SellerName like '%TRANSAMERICA TITLE%'
        or SellerName like '%PEELLE F%'
        or SellerName like '%ATTY AT%'
        or SellerName like '%ATTY-AT%'
        or SellerName like '%WARREN & DUGGAN%'
        or SellerName like '%T D SER%'
        or SellerName like '%REGIONAL TRUST%'
        or SellerName like '%ORDAL TRUSTEE%' 
        or SellerName like '%MTC FIN%' 
        or SellerName like '%TRUSTEE CORPS%'
        or SellerName like '%TRUSTEE SERVICES%'
        or SellerName like '%RECONT%'
        or SellerName like '%RECON T%'
        or SellerName like '%CLEAR RECON%'
        or SellerName like '%CELAR RECON%' 
        or SellerName like '%NORTH STAR TRUST%' 
        or SellerName like '%RTS PACIFIC%' 
        or SellerName like '%AZTEC FOR%'
        or SellerName like '%CAL-WEST%'
        or SellerName like '%CAL WEST%' 
        or SellerName like '%CIMARRON%'
        and SellerName not like '%CIMARRON 8%'
        or SellerName like '%PEAK FOR%' 
        or SellerName like '%FORECLOSURE%' 
        THEN 'FORECLOSURE'
--------------------------------FINANCIAL INSTITUTION-----------------------------------------------
      WHEN SellerName like '%WILMINGTON SAVINGS FUND SOCIETY%'
        or SellerName like '%CHRISTIANA TRUST%'
        or SellerName like '%CHRISTINA TRUST%' 
        or SellerName like '%WILMINGTON TRUST%' 
        or SellerName like '%DEUTSCHE B%'
        or SellerName like '%WASHINGTON FEDERAL%' 
        or SellerName like '%MORTGAGE EQUITY CONVERSION%' 
        or SellerName like '%MORTGAGE LENDER SERVICES%'
        or SellerName like '%TAYLOR BEAN%'
        or SellerName like '%DITECH%' 
        or SellerName like '%CITI %'
        or SellerName like '%GMAC M%'
        or SellerName like '%CHASE MAN%'
        or SellerName like '%CHASE HOM%'
        or SellerName like '%CHASE HAN%'
        or SellerName like '%HOUSEHOLD FIN%'
        or SellerName like '%CITIM%'
        or SellerName like '%CITIFI%'
        or SellerName like '%SPRINGLEAF F%'
        or SellerName like '%SPRINGLEAF H%'
        or SellerName like '%MOREQ%'
        or SellerName like '%AMERICAN GENERAL %'
        or SellerName like '%NORWEST M%'
        or SellerName like '%HSBC M%'
        or SellerName like '%FLEET M%'
        or SellerName like 'RESIDENTIAL CR%'
        or SellerName like '%PNC M%'
        or SellerName like '%NATIONAL CI%'
        or SellerName like '%COMMERCIAL F%'
        or SellerName like '%DLJ M%'
        or SellerName like '%CAPITAL O%'
        or SellerName like '%AAMES%'
        or SellerName like '%KONDAUR%'
        or SellerName like '%EMC M%'
        or SellerName like '%PRINCIPAL R%'
        or SellerName like '%CHASE MO%'
        or SellerName like '%PHH M%'
        or SellerName like '%MORTGAGE ELECTRONIC%'
        or SellerName like '%NATIONSTAR%'
        or SellerName like '%FIRST MUTUAL%'
        or SellerName like '%WASHINGTON MUTUAL%'
        or SellerName like '%WELLS FARGO%'
        or SellerName like '%PENNYMAC%'
        or SellerName like '%AURORA LOAN%'
        or SellerName like '%GUILD M%'
        or SellerName like '%SAVINGS%'
        or SellerName like '%FEDERAL S&L%'
        or SellerName like '%CREDIT UNION%'
        or SellerName like '%FIRST FEDERAL%'
        or SellerName like '%FEDERAL ASSET%'
        or SellerName like '%FEDERAL RECOVERY%'
        or SellerName like '%ALASKA USA%'
        or SellerName like '%SAV & LOAN%'
        or SellerName like '%FREEDOM M%'
        or SellerName like '%SUNTRUST M%'
        or SellerName like '%BENEFICIAL%'
        or SellerName like '%BAC HOME LOAN%'
        and SellerName not like '%BENEFICIAL DEVELOPMENT%'
        and SellerName not like '%BRICKLAYERS BENEFICIAL%'
        or SellerName like '%BANK%' 
        and SellerName not like '%EUBANK%' 
        and SellerName not like '%FAIRBANK%'
        and SellerName not like '%BURBANK%'
        and SellerName not like '%BANKER%'
        and SellerName not like '%BANKS%'
        and SellerName not like '%BANKHEAD%' 
        and SellerName not like '%BANKRUPTCY%' 
        and SellerName not like '%EWBANK%' 
        and SellerName not like '%WEST BANK%'
        and SellerName not like '%BANKE%' 
        and SellerName not like '%BANK LLC%' 
        THEN 'FINANCIAL INSTITUTION'
-------------------------------VA--------------------------------------------------------------------
      WHEN SellerName like '%VETERAN%' 
        or SellerName like '%VETS AFFAIRS%' 
        and SellerName not like '%VETERANS OF%'
        and SellerName not like '%VETERANS FOREIGN%' 
        and SellerName not like '%VETERANE%' 
        and SellerName not like '%VALLEY VETERANS%'
        and SellerName not like '%NISEI VETERANS%' 
        and SellerName not like '%VETERANS ADMIN%' 
        and SellerName not like '%ADM OF VETERAN%'
        and SellerName not like '%ADMIN OF VETERANS%'
        and SellerName not like '%ADMINISTRATOR VETERAN%' 
        THEN 'VA'
-------------------------------HUD-------------------------------------------------------------------
      WHEN SellerName like '%SECRETARY HOUSING%'
        or SellerName like '%SECRETARY OF HOUSING%' 
        THEN 'HUD'
-------------------------------FANNIE MAE-------------------------------------------------------------
      WHEN SellerName like '%FANNIE MAE%' 
        or SellerName like '%FEDERAL NAT%'
        or SellerName like '%FEDERALNATI%'
        or SellerName like '%FED NAT%'
        THEN 'FANNIE MAE'
-------------------------------FREDDIE MAC------------------------------------------------------------
      WHEN SellerName like '%FEDERAL H%'
        or SellerName like '%FED HO%' 
        THEN 'FREDDIE MAC'
---------------------------------GOVT-----------------------------------------------------------------
      WHEN SellerName like '%SEATTLE CITY OF%'
        or SellerName like '%FEDERAL WAY CI%' 
        or SellerName like '%FEDERAL WAY SC%'
        and SellerName not like '%HOUSING AUTHORITY%' 
        or SellerName like '%KING COUNTY SHERIFF%' 
        or SellerName like '%KING COUNTY-SHERIFF%' 
        or SellerName like '%KC-SHERIFF%'
        or SellerName like '%-KC %'
        and SellerName not like '%YOUNG WOMEN%'
        and SellerName not like '% HABITAT FOR%'
        or SellerName like '%KING COUNTY%' 
        or SellerName like '%KING-COUNTY%' 
        or SellerName like '%STATE OF WASHINGTON%'
        or SellerName like '%WASHINGTON STATE%' 
        or SellerName like '%DEPARTMENT OF TRANSPORTATION%'
        or SellerName like '%INTERNAL REVENUE SERVICE%'
        or SellerName like '%UNITED STATES OF AMERICA%'
        or SellerName like '%VETERANS OF%' 
        or SellerName like '%VETERANS FOREIGN%' 
        or SellerName like '%VETERANS ADMIN%' 
        or SellerName like '%ADM OF VETERAN%' 
        or SellerName like '%ADMIN OF VETERANS%' 
        or SellerName like '%ADMINISTRATOR VETERAN%' 
        or SellerName like '%GOVT%' 
        THEN 'GOVT'
--------------------Bankruptcy Info from --https://www.seattle-bankruptcyattorney.net/seattle-trustee-information/ ----------------
      WHEN SellerName like '%BANKRUPTCY%' 
        or SellerName like '%CHAPTER 7%'
        or SellerName like '%CHAPTER 11%'
        or SellerName like '%BK-TRUSTEE%' 
        or SellerName like 'BROWN RONALD'
        or SellerName like '%BROWN RONALD G%' 
        or SellerName like '%BURDETTE VIRGINIA%' 
        or SellerName like 'BURMAN DENNIS' 
        or SellerName like '%BURMAN DENNIS L%' 
        or SellerName like 'ELLIS KATHRYN' 
        or SellerName like '%ELLIS KATHRYN A%'
        or SellerName like '%NANCY JAMES%' 
        or SellerName like '%NANCY L JAMES%'
        or SellerName like 'JAMES NANCY'
        or SellerName like '%JAMES NANCY L%'
        or SellerName like 'KLEIN MICHAEL'
        or SellerName like '%KLEIN MICHAEL P%' 
        or SellerName like 'MCCARTY MICHAEL'
        or SellerName like '%MCCARTY MICHAEL B%'
        or SellerName like 'PETERSON JOHN'
        or SellerName like '%PETERSON JOHN S%'
        and SellerName not like '%PETERSON JOHN S+JOYCE%' 
        or SellerName like '%RIGBY JAMES%' 
        and SellerName not like '%RIGBY JAMES+BETTE%'
        or SellerName like '%EDMUND J WOOD%' 
        or SellerName like '%WOOD EDMUND%' 
        or SellerName like '%BUDSBERG BRIAN%'
        or SellerName like '%DONAHUE TERRENCE J%'
        or SellerName like '%WALDRON MARK D%' 
        THEN 'BANKRUPTCY'
-------------------------------RECEIVER------------------------------------------------------------------------------
      WHEN SellerName like '%FEDERAL DEPOSIT%' 
        THEN 'FDIC(RECEIVER)'
--------------------------------TRUSTEE--------------------------------------------------------------------------------
      WHEN SellerName like '%TRUST%' 
        or SellerName like '%TTEE%'
        or SellerName like '%-TTE%'
        and SellerName not like '%COMMITTEE%'
        or SellerName like '%(TR)%'
        or SellerName like '%SUCCESSOR%'
        or SellerName like '%OHANA FIN%' --Estate, Guardian, Executor, or Trustee under a will.
        or Sellername like '%PIONEER FED%'
        and SellerName not like '%RELOC%'
        and  SellerName NOT like '%RELOC%'
        and  SellerName NOT like '%LIVING%'
        and  SellerName NOT like '%LVG%'
        and  SellerName NOT like '%LAND TRUST%'
        and  SellerName NOT like '%FAMILY TRUST%'
        and SellerName NOT like '%REV TRUST%'
        and  SellerName NOT like '%REVOCABLE%' 
        THEN 'TRUSTEE'       
--------------------------------MISC-------------------------------------------------------------------------------------
      WHEN SellerName like '%TITLE%' THEN 'TITLE'   

---------------------------------1031 EXCHANGE-----------------------------------------------------------------------------
      WHEN SellerName like '%1031%' 
        or SellerName like '%TAX DEFERRED EXC%'
        or SellerName like '%WASHINGTON EXCHANGE%' 
        or SellerName like '%EQUITY PRE%'
        or SellerName like '%TAX DEF%'
        or SellerName like '%EXCHANGE A%'
        or SellerName like '%SIMPLIFIED E%'
        or SellerName like '%EXCHANGE F%'
        or SellerName like '%LSPI E%'
        or SellerName like '%EXCHANGE COU%'
        or SellerName like '%REVERSE E%' 
        or SellerName like '%DELAYED E%'
        or SellerName like '%EXCHANGE P%'
        THEN '1031 EXCHG'
---------------------------------BUSINESS----------------------------------------------------------------------------------         
      WHEN SellerName like '% LP%' 
        or SellerName like '% LLP%'
        or SellerName like '%LIMITED PARTNERSHIP%' 
        THEN 'LTD PARTNER'
      WHEN SellerName like '%LLC%' 
        THEN 'LLC'
      WHEN SellerName like '% INC%' 
        or SellerName like '% CORP%' 
        or SellerName like '% INCORP%' 
        THEN 'INC'
      WHEN SellerName like '%COMPANY%' 
        or SellerName like '%ENTERPRISE%' 
        or SellerName like '% CO %' 
        THEN 'BUSINESS'
---------------------------------LAST OPTION---------------------------------------------------------------------------
      WHEN SellerName like '%FINANC%' THEN 'FINANCE'
      WHEN SellerName like '%MORTGAGE%' THEN 'MORTGAGE'
      WHEN SellerName like '%MTG%' THEN 'MORTGAGE'
      WHEN SellerName like '%SERVICES%' THEN 'SERVICES'
      WHEN SellerName like '%ACCEPTANCE%' THEN 'ACCEPTANCE'
      --WHEN SellerName like '%LOAN%' THEN 'LOAN' --Removed because it is a Vietnamese name. 
      WHEN SellerName like '%FUNDING%' THEN 'FUNDING'  
      WHEN SellerName like '%NATIONAL%' THEN 'NATIONAL' 
      WHEN SellerName like '%MUTUAL%' THEN 'MUTUAL'
      WHEN SellerName like '%COMMERCE%' THEN 'COMMERCE' 
      WHEN SellerName like '%FEDERAL%' THEN 'FEDERAL'  
      WHEN SellerName like '%CREDIT%' THEN 'CREDIT'
      WHEN SellerName like '%CAPITAL%' THEN 'CAPITAL'
      WHEN SellerName like '%EQUITY%' THEN 'EQUITY'
      WHEN SellerName like '%EXCHANGE%' THEN 'EXCHANGE'
      ELSE '' END
      )
FROM #SaleId s

UPDATE #SaleId
SET BuyerNameWarning = 
 --------------------------RELOCATION-------------------------------------------------------
      (CASE      
      WHEN BuyerName like '%NATIONAL RESIDENTIAL NOMINE%'
        or BuyerName like '%NATIONAL RESIDENTIAL SERVICE%' 
        or BuyerName like '%NATL RESIDENTIAL NOMINE%' 
        or BuyerName like '%NATIONWIDE MUTUAL%' 
        or BuyerName like '%RELOCATION%' 
        or BuyerName like '%MOBILITY%' 
        or BuyerName like '%CARTUS FINANC%' 
        or BuyerName like '%CARTUS CORP%'
        or BuyerName like '%PHH R%'
        or BuyerName like '%PHH H%'
        or BuyerName like '%P H H H%'
        or BuyerName like '%FRANCONIA%' 
        or BuyerName like '%NATIONAL TRANSFER SERVICE%' 
        or BuyerName like '%GRSW STEWART%' 
        or BuyerName like '%DODGE NP%'
        or BuyerName like '%DODGE N P%'
        or BuyerName like '%NATIONAL EQUITY%'
        and BuyerName not like '%INTERNATIONAL%' 
        or BuyerName like '%LEXICON GOVERNMENT%' 
        or BuyerName like '%OLD REPUBLIC DIVERSIFIED%' 
        or BuyerName like '%RAS CLOSING%' 
        or BuyerName like '%RELO DIRECT%'  
        or BuyerName like '%STONE FINANC%' 
        or BuyerName like '%WEICHERT WORKFORCE%' 
        or BuyerName like '%AMERICAN ESCROW & CLOSING%' 
        or BuyerName like '%BGRS LLC%' 
        or BuyerName like '%PCL CONSTRUCTION ENTERPRISES%' 
        or BuyerName like '%SIRVA RELO%' 
        THEN 'RELOCATION'
 --------------------------RELO or FORECLOSURE------------------------------------------------------------     
      WHEN BuyerName like '%OLD REPUBLIC%' 
        THEN 'RELO/FORECLOSURE?'
---------------------------AFFORDABLE HOUSING-------------------------------------------------------------
      WHEN BuyerName like '%HABITAT FOR HUMANITY%' 
        or BuyerName like '%HABITAT OF HUMANITY%' 
        or BuyerName like '%HOMESTEAD COMMUNITY LAND%' 
        or BuyerName like '%HOUSING AUTHORITY%' 
        or BuyerName like '%HOMESIGHT%'
        THEN 'AFFORDABLE HOUSING'
---------------------------ESTATE---------------------------------------------------------------------------
      WHEN BuyerName like '%ADMINISTRATOR%' 
        and BuyerName not like 'VETS AFFAIRS'
        and BuyerName not like 'VETERAN' 
        or BuyerName like '%ESTATE OF%'  
        or BuyerName like '%PERS REP%' 
        or BuyerName like '%-PR%' 
        and BuyerName not like '%-PRO%'
        and BuyerName not like '%-PR2%'
        or BuyerName like '%-SOLE HEIR%' 
        or BuyerName like '%-HEIR%' 
        or BuyerName like '% HEIR %' 
        or BuyerName like '%DEVISE%'
        or BuyerName like '%EXECUTRIX%'
        THEN 'ESTATE'
----------------------------GUARDIAN-------------------------------------------------------------------------
      WHEN BuyerName like '%PUGET SOUND GUARDIAN%' 
        or BuyerName like '%GUARDIANSHIP%' 
        THEN 'GUARDIAN'
--------------------------------FINANCIAL INSTITUTION-----------------------------------------------
      WHEN BuyerName like '%WILMINGTON SAVINGS FUND SOCIETY%'
        or BuyerName like '%CHRISTIANA TRUST%'
        or BuyerName like '%CHRISTINA TRUST%' 
        or BuyerName like '%WILMINGTON TRUST%' 
        or BuyerName like '%DEUTSCHE B%'
        or BuyerName like '%WASHINGTON FEDERAL%' 
        or BuyerName like '%MORTGAGE EQUITY CONVERSION%' 
        or BuyerName like '%MORTGAGE LENDER SERVICES%'
        or BuyerName like '%TAYLOR BEAN%'
        or BuyerName like '%DITECH%' 
        or BuyerName like '%CITI %'
        or BuyerName like '%GMAC M%'
        or BuyerName like '%CHASE MAN%'
        or BuyerName like '%CHASE HOM%'
        or BuyerName like '%CHASE HAN%'
        or BuyerName like '%HOUSEHOLD FIN%'
        or BuyerName like '%CITIM%'
        or BuyerName like '%CITIFI%'
        or BuyerName like '%SPRINGLEAF F%'
        or BuyerName like '%SPRINGLEAF H%'
        or BuyerName like '%MOREQ%'
        or BuyerName like '%AMERICAN GENERAL %'
        or BuyerName like '%NORWEST M%'
        or BuyerName like '%HSBC M%'
        or BuyerName like '%FLEET M%'
        or BuyerName like 'RESIDENTIAL CR%'
        or BuyerName like '%PNC M%'
        or BuyerName like '%NATIONAL CI%'
        or BuyerName like '%COMMERCIAL F%'
        or BuyerName like '%DLJ M%'
        or BuyerName like '%CAPITAL O%'
        or BuyerName like '%AAMES%'
        or BuyerName like '%KONDAUR%'
        or BuyerName like '%EMC M%'
        or BuyerName like '%PRINCIPAL R%'
        or BuyerName like '%CHASE MO%'
        or BuyerName like '%PHH M%'
        or BuyerName like '%MORTGAGE ELECTRONIC%'
        or BuyerName like '%NATIONSTAR%'
        or BuyerName like '%FIRST MUTUAL%'
        or BuyerName like '%WASHINGTON MUTUAL%'
        or BuyerName like '%WELLS FARGO%'
        or BuyerName like '%PENNYMAC%'
        or BuyerName like '%AURORA LOAN%'
        or BuyerName like '%GUILD M%'
        or BuyerName like '%SAVINGS%'
        or BuyerName like '%FEDERAL S&L%'
        or BuyerName like '%CREDIT UNION%'
        or BuyerName like '%FIRST FEDERAL%'
        or BuyerName like '%FEDERAL ASSET%'
        or BuyerName like '%FEDERAL RECOVERY%'
      or BuyerName like '%ALASKA USA%'
        or BuyerName like '%SAV & LOAN%'
        or BuyerName like '%FREEDOM M%'
        or BuyerName like '%SUNTRUST M%'
        or BuyerName like '%BENEFICIAL%'
        or BuyerName like '%BAC HOME LOAN%'
        and BuyerName not like '%BENEFICIAL DEVELOPMENT%'
        and BuyerName not like '%BRICKLAYERS BENEFICIAL%'
        or BuyerName like '%BANK%' 
        and BuyerName not like '%EUBANK%' 
        and BuyerName not like '%FAIRBANK%'
        and BuyerName not like '%BURBANK%'
        and BuyerName not like '%BANKER%'
        and BuyerName not like '%BANKS%'
        and BuyerName not like '%BANKHEAD%' 
        and BuyerName not like '%BANKRUPTCY%' 
        and BuyerName not like '%EWBANK%' 
        and BuyerName not like '%WEST BANK%'
        and BuyerName not like '%BANKE%' 
        and BuyerName not like '%BANK LLC%' 
        THEN 'FINANCIAL INSTITUTION'
-------------------------------VA--------------------------------------------------------------------
      WHEN BuyerName like '%VETERAN%' 
        or BuyerName like '%VETS AFFAIRS%' 
        and BuyerName not like '%VETERANS OF%'
        and BuyerName not like '%VETERANS FOREIGN%' 
        and BuyerName not like '%VETERANE%' 
        and BuyerName not like '%VALLEY VETERANS%'
        and BuyerName not like '%NISEI VETERANS%' 
        and BuyerName not like '%VETERANS ADMIN%' 
        and BuyerName not like '%ADM OF VETERAN%'
        and BuyerName not like '%ADMIN OF VETERANS%'
        and BuyerName not like '%ADMINISTRATOR VETERAN%' 
        THEN 'VA'
-------------------------------HUD-------------------------------------------------------------------
      WHEN BuyerName like '%SECRETARY HOUSING%'
        or BuyerName like '%SECRETARY OF HOUSING%' 
        THEN 'HUD'
-------------------------------FANNIE MAE-------------------------------------------------------------
      WHEN BuyerName like '%FANNIE MAE%' 
        or BuyerName like '%FEDERAL NAT%'
        or BuyerName like '%FEDERALNATI%'
        or BuyerName like '%FED NAT%'
        THEN 'FANNIE MAE'
-------------------------------FREDDIE MAC------------------------------------------------------------
      WHEN BuyerName like '%FEDERAL H%'
        or BuyerName like '%FED HO%' 
        THEN 'FREDDIE MAC'
---------------------------------GOVT-----------------------------------------------------------------
      WHEN BuyerName like '%SEATTLE CITY OF%'
        or BuyerName like '%FEDERAL WAY CI%' 
        or BuyerName like '%FEDERAL WAY SC%'
        and BuyerName not like '%HOUSING AUTHORITY%' 
        or BuyerName like '%KING COUNTY SHERIFF%' 
        or BuyerName like '%KING COUNTY-SHERIFF%' 
        or BuyerName like '%KC-SHERIFF%'
        or BuyerName like '%-KC %'
        and BuyerName not like '%YOUNG WOMEN%'
        and BuyerName not like '% HABITAT FOR%'
        or BuyerName like '%KING COUNTY%' 
        or BuyerName like '%KING-COUNTY%' 
        or BuyerName like '%STATE OF WASHINGTON%'
        or BuyerName like '%WASHINGTON STATE%'  
        or BuyerName like '%DEPARTMENT OF TRANSPORTATION%'
        or BuyerName like '%INTERNAL REVENUE SERVICE%'
        or BuyerName like '%UNITED STATES OF AMERICA%'
        or BuyerName like '%VETERANS OF%' 
        or BuyerName like '%VETERANS FOREIGN%' 
        or BuyerName like '%VETERANS ADMIN%' 
        or BuyerName like '%ADM OF VETERAN%' 
        or BuyerName like '%ADMIN OF VETERANS%' 
        or BuyerName like '%ADMINISTRATOR VETERAN%' 
        or BuyerName like '%GOVT%' 
        THEN 'GOVT'
--------------------Bankruptcy Info from --https://www.seattle-bankruptcyattorney.net/seattle-trustee-information/ ----------------
      WHEN BuyerName like '%BANKRUPTCY%' 
        or BuyerName like '%CHAPTER 7%'
        or BuyerName like '%CHAPTER 11%'
        or BuyerName like '%BK-TRUSTEE%' 
        or BuyerName like 'BROWN RONALD'
        or BuyerName like '%BROWN RONALD G%' 
        or BuyerName like '%BURDETTE VIRGINIA%' 
        or BuyerName like 'BURMAN DENNIS' 
        or BuyerName like '%BURMAN DENNIS L%' 
        or BuyerName like 'ELLIS KATHRYN' 
        or BuyerName like '%ELLIS KATHRYN A%'
        or BuyerName like '%NANCY JAMES%' 
        or BuyerName like '%NANCY L JAMES%'
        or BuyerName like 'JAMES NANCY'
        or BuyerName like '%JAMES NANCY L%'
        or BuyerName like 'KLEIN MICHAEL'
        or BuyerName like '%KLEIN MICHAEL P%' 
        or BuyerName like 'MCCARTY MICHAEL'
        or BuyerName like '%MCCARTY MICHAEL B%'
        or BuyerName like 'PETERSON JOHN'
        or BuyerName like '%PETERSON JOHN S%'
        and BuyerName not like '%PETERSON JOHN S+JOYCE%' 
        or BuyerName like '%RIGBY JAMES%' 
        and BuyerName not like '%RIGBY JAMES+BETTE%'
        or BuyerName like '%EDMUND J WOOD%' 
        or BuyerName like '%WOOD EDMUND%' 
        or BuyerName like '%BUDSBERG BRIAN%'
        or BuyerName like '%DONAHUE TERRENCE J%'
        or BuyerName like '%WALDRON MARK D%' 
        THEN 'BANKRUPTCY'
-------------------------------RECEIVER------------------------------------------------------------------------------
      WHEN BuyerName like '%FEDERAL DEPOSIT%' 
        THEN 'FDIC(RECEIVER)'
--------------------------------TRUSTEE--------------------------------------------------------------------------------
      WHEN BuyerName like '%TRUST%' 
        or BuyerName like '%TTEE%'
        or BuyerName like '%-TTE%'
        and BuyerName not like '%COMMITTEE%'
        or BuyerName like '%(TR)%'
        or BuyerName like '%SUCCESSOR%'
        or BuyerName like '%OHANA FIN%' --Estate, Guardian, Executor, or Trustee under a will.
        or BuyerName like '%PIONEER FED%'
        and BuyerName not like '%RELOC%'
        and  BuyerName NOT like '%RELOC%'
        and  BuyerName NOT like '%LIVING%'
        and  BuyerName NOT like '%LVG%'
        and  BuyerName NOT like '%LAND TRUST%'
        and  BuyerName NOT like '%FAMILY TRUST%'
        and  BuyerName NOT like '%REV TRUST%'
        and  BuyerName NOT like '%REVOCABLE%' 
        THEN 'TRUSTEE'       
--------------------------------MISC-------------------------------------------------------------------------------------
      WHEN BuyerName like '%TITLE%' THEN 'TITLE'   

---------------------------------1031 EXCHANGE-----------------------------------------------------------------------------
      WHEN BuyerName like '%1031%' 
        or BuyerName like '%TAX DEFERRED EXC%'
        or BuyerName like '%WASHINGTON EXCHANGE%' 
        or BuyerName like '%EQUITY PRE%'
        or BuyerName like '%TAX DEF%'
        or BuyerName like '%EXCHANGE A%'
        or BuyerName like '%SIMPLIFIED E%'
        or BuyerName like '%EXCHANGE F%'
        or BuyerName like '%LSPI E%'
        or BuyerName like '%EXCHANGE COU%'
        or BuyerName like '%REVERSE E%' 
        or BuyerName like '%DELAYED E%'
        or BuyerName like '%EXCHANGE P%'
        THEN '1031 EXCHG'
---------------------------------BUSINESS----------------------------------------------------------------------------------         
      WHEN BuyerName like '% LP%' 
        or BuyerName like '% LLP%'
        or BuyerName like '%LIMITED PARTNERSHIP%' 
        THEN 'LTD PARTNER'
      WHEN BuyerName like '%LLC%' 
        THEN 'LLC'
      WHEN BuyerName like '% INC%' 
        or BuyerName like '% CORP%'
        or BuyerName like '% INCORP%' 
        THEN 'INC'
      WHEN BuyerName like '%COMPANY%' 
        or BuyerName like '%ENTERPRISE%' 
        or BuyerName like '% CO %' 
        THEN 'BUSINESS'
---------------------------------LAST OPTION---------------------------------------------------------------------------
      WHEN BuyerName like '%FINANC%' THEN 'FINANCE'
      WHEN BuyerName like '%MORTGAGE%' THEN 'MORTGAGE'
      WHEN BuyerName like '%MTG%' THEN 'MORTGAGE'
  WHEN BuyerName like '%SERVICES%' THEN 'SERVICES'
      WHEN BuyerName like '%ACCEPTANCE%' THEN 'ACCEPTANCE'
      --WHEN BuyerName like '%LOAN%' THEN 'LOAN' --Removed because it is a Vietnamese name. 
      WHEN BuyerName like '%FUNDING%' THEN 'FUNDING'  
      WHEN BuyerName like '%NATIONAL%' THEN 'NATIONAL' 
      WHEN BuyerName like '%MUTUAL%' THEN 'MUTUAL'
      WHEN BuyerName like '%COMMERCE%' THEN 'COMMERCE' 
      WHEN BuyerName like '%FEDERAL%' THEN 'FEDERAL'  
      WHEN BuyerName like '%CREDIT%' THEN 'CREDIT'
      WHEN BuyerName like '%CAPITAL%' THEN 'CAPITAL'
      WHEN BuyerName like '%EQUITY%' THEN 'EQUITY'
      WHEN BuyerName like '%EXCHANGE%' THEN 'EXCHANGE'
      WHEN BuyerName like '%FORECLOSURE%' THEN 'FORECLOSURE'
      ELSE '' END
      )
FROM #SaleId s


	UPDATE #SaleId
	   SET SaleWarningToReview = 'Y'
	  FROM #SaleId s
     INNER JOIN [dynamics].[ptas_sales_ptas_saleswarningcode] sswc
        ON s.SaleGuid = sswc.ptas_salesid
     INNER JOIN [dynamics].[ptas_saleswarningcode] swc
        ON sswc.ptas_saleswarningcodeid = swc.ptas_saleswarningcodeid
	 WHERE (   swc.ptas_id = 60  --SHORT SALE
			or swc.ptas_id = 61  --FINANCIAL INSTITUTION RESALE
			or swc.ptas_id = 62  --AUCTION SALE
			or swc.ptas_id = 13  --BANKRUPTCY - RECEIVER OR TRUSTEE
			or swc.ptas_id = 14  --SHERIFF / TAX SALE
			or swc.ptas_id = 51  --RELATED PARTY, FRIEND, OR NEIGHBOR
			or swc.ptas_id = 12  --ESTATE ADMINISTRATOR, GUARDIAN, OR EXECUTOR
			or swc.ptas_id = 26  --IMP. CHARACTERISTICS CHANGED SINCE SALE
			or swc.ptas_id = 29  --SEGREGATION AND/OR MERGER
			)


--Populate BuildingWarning field
--select top 100 * from ResBldg
--A parcel can have multiple Res buildings, any one of which can have PctCompl, NetCond, etc. flags.  So use subqueries to check all bldg records of a given parcel for those flags.

--drop table #BuildingWarning
CREATE TABLE #BuildingWarning 
(
SaleGuid uniqueidentifier
,ParcelId uniqueidentifier
,PIN char(10)--for double checking in RealProperty  
,PctCompl varchar(15) default '' 
,NetCond varchar(15) default '' 
,Obsol varchar(15) default ''
,UnfinArea varchar(15) default '' 
,MultiImps varchar(15) default '' 
,MH_Real varchar(15) default ''
,MH_Pers varchar(15) default ''
,LowPrevImpsVal varchar(15) default ''
,ActivePermitBeforeSale varchar(50) default ''
,BuildingWarning varchar (300)  
)



INSERT #BuildingWarning
(SaleGuid, ParcelId, PIN, PctCompl, NetCond, Obsol, UnfinArea, MultiImps, MH_Real, MH_Pers, LowPrevImpsVal, ActivePermitBeforeSale)
SELECT DISTINCT
 s.SaleGuid
,s.ParcelId
,PIN = dpd.ptas_Major + dpd.ptas_Minor						   
,PctCompl 	= CASE WHEN bd.ptas_percentcomplete > 0 AND bd.ptas_percentcomplete < 100 THEN 'PctCompl' ELSE '' END 
,NetCond 	= CASE WHEN bd.ptas_percentnetcondition >  0  THEN 'NetCond' ELSE '' END
,Obsol 		= CASE WHEN bd.ptas_buildingobsolescence < 0 THEN 'Obsol' ELSE '' END 
,UnfinArea 	= CASE WHEN (COALESCE(bd.ptas_unfinished_full_sqft,0) + COALESCE(bd.ptas_unfinished_half_sqft,0)) > 0 THEN 'UnfinArea' ELSE '' END
,MultiImps 	= CASE WHEN (select count(*) from dynamics.vw_ResBldg rb where rb.ptas_buildingdetailid = s.ParcelId) > 1 THEN 'MultiImps' ELSE '' END
,MH_Real 	= CASE WHEN cu.ptas_mobilehometype  = 4 THEN 'MH_Real' ELSE '' END
,MH_Pers 	= CASE WHEN cu.ptas_mobilehometype <> 4 THEN 'MH_Pers' ELSE '' END
,LowPrevImpsVal = CASE
                     WHEN EXISTS (select * 
									FROM [ptas].[ptas_appraisalhistory] rah
									INNER JOIN [dynamics].[ptas_parceldetail] rp2 ON rp2.ptas_parceldetailid = rah.ParcelGuid
									WHERE s.ParcelId = rp2.ptas_parceldetailid
									and rah.RollYear = @RPAssmtYr
									AND rah.interfaceFlag <> 15
    AND rah.impsValue > 0 AND rah.impsValue <= 25000
                           AND rah.appraisedDate = (select max(appraisedDate) from [ptas].[ptas_appraisalhistory] rah2 where rah2.ParcelGuid = rah.ParcelGuid
                                                 and rah2.RollYear = rah.RollYear and rah2.appraisedDate >= rah.appraisedDate and rah2.interfaceFlag <> 15
                                                 and rah2.impsValue > 0 and rah2.impsValue <= 25000)   
                                                 ) THEN 'LowPrevImpsVal' ELSE '' END
,ActivePermitBeforeSale = CASE 
                           WHEN EXISTS (select * 
										FROM [dynamics].[ptas_permit] bp 
									   WHERE bp._ptas_parcelid_value = s.ParcelId
									     AND bp.statuscode IN(591500004,591500005,591500006,591500001)--Revisit / Revisit again / Screened out / Incomplete										 
									     AND bp.ptas_issueddate < s.SaleDate
									 	 AND bp.ptas_permitvalue > 25000
                           ) THEN 'ActivePermitBeforeSale' ELSE '' END									 												 
FROM #SaleId s
INNER JOIN dynamics.ptas_parceldetail dpd  		ON dpd.ptas_parceldetailid = s.ParcelId
LEFT JOIN dynamics.vw_ResBldg bd
ON dpd.ptas_parceldetailid = bd._ptas_parceldetailid_value
LEFT JOIN [dynamics].[ptas_condounit] cu 		ON cu._ptas_parcelid_value = dpd.ptas_parceldetailid
ORDER BY s.ParcelId



--LEFT OFF
-- select top 10 * from BldgPermit  --Has RpGuid for join to SaleId, PermitGuid for join to BldgPermitStat, and IssueDate for logic. PermitVal is also available
-- select top 10 * from BldgPermitStat --PermitGuid for join.  PermitStatusItemId, PcntComplete, and ReviewedDate for logic
-- select * from LuItem2 where LuTypeId = 164

----Use repeated subqueries against temp #BuildingWarning table to update BuildingWarning
UPDATE #BuildingWarning 
SET BuildingWarning = 
  (   isnull((select distinct ', PctCompl'  from #BuildingWarning b2 where b2.ParcelId = b.ParcelId and PctCompl  <> ''),'')
    + isnull((select distinct ', NetCond'  from #BuildingWarning b2 where b2.ParcelId = b.ParcelId and NetCond  <> ''),'')
    + isnull((select distinct ', Obsol'  from #BuildingWarning b2 where b2.ParcelId = b.ParcelId and Obsol  <> ''),'')
    + isnull((select distinct ', UnfinArea'  from #BuildingWarning b2 where b2.ParcelId = b.ParcelId and UnfinArea  <> ''),'')
    + isnull((select distinct ', MultiImps'  from #BuildingWarning b2 where b2.ParcelId = b.ParcelId and MultiImps  <> ''),'') 
    + isnull((select distinct ', MH_Real'  from #BuildingWarning b2 where b2.ParcelId = b.ParcelId and MH_Real  <> ''),'')
    + isnull((select distinct ', MH_Pers'  from #BuildingWarning b2 where b2.ParcelId = b.ParcelId and MH_Pers  <> ''),'') 
    + isnull((select distinct ', LowPrevImpsVal'  from #BuildingWarning b2 where b2.ParcelId = b.ParcelId and LowPrevImpsVal  <> ''),'') 
    + isnull((select distinct ', ActivePermitBeforeSale'  from #BuildingWarning b2 where b2.ParcelId = b.ParcelId and ActivePermitBeforeSale  <> ''),'')        
    )
FROM #BuildingWarning b

----Remove leading blanks
UPDATE #BuildingWarning SET BuildingWarning = substring(BuildingWarning,2,300)
UPDATE #BuildingWarning SET BuildingWarning = Ltrim(BuildingWarning)

UPDATE #SaleId 
SET BuildingWarning = b.BuildingWarning
FROM #BuildingWarning b
INNER JOIN #SaleId s ON s.ParcelId = b.ParcelId



UPDATE #SaleId
SET LandWarning = CASE
                     WHEN EXISTS (SELECT * 
									FROM [ptas].[ptas_appraisalhistory] rah
								   INNER JOIN [dynamics].[ptas_parceldetail] rp2 ON rp2.ptas_parceldetailid = rah.ParcelGuid
								   WHERE s.ParcelId = rp2.ptas_parceldetailid
								     AND rah.RollYear = @RPAssmtYr
									 AND rah.interfaceFlag <> 15
                                     AND rah.landvalue > 0 AND rah.landvalue <= 25000
                                     AND rah.appraisedDate = (SELECT max(appraisedDate) from [ptas].[ptas_appraisalhistory] rah2 where rah2.ParcelGuid = rah.ParcelGuid
																      AND rah2.RollYear = rah.RollYear and rah2.appraisedDate >= rah.appraisedDate and rah2.interfaceFlag <> 15
																      AND rah2.landvalue > 0 and rah2.landvalue <= 25000)   
                                  ) THEN 'LowPrevLandVal' 
							  
                  WHEN EXISTS (SELECT * FROM dynamics.ptas_parceldetail dpd
							    WHERE dpd.ptas_parceldetailid = s.ParcelId
							      AND dpd.ptas_currentuse > 0
							   ) 
								 THEN 'Open Space'                            
                  ELSE '' END  
FROM #SaleId s




-- Get Sale Warnings
UPDATE #SaleId
SET SaleWarnings = dynamics.fn_ConcatNotes(s.SaleGuid ,10, 2, 500)
FROM #SaleId s




--Get Sale Notes
	--This talbe is used to order the Notes by date 
	CREATE TABLE #NotesOrderedByDate 
	(
	 rowID  		int	not null  identity(1,1)
	 ,ParcelId uniqueidentifier
	 ,SaleGuid uniqueidentifier 
	 ,Note nvarchar(max)
	 ,UpdateDate smalldatetime
	 ,AssmtEntityId char(4) 
	 ,PRIMARY KEY(rowID)	
	)

	INSERT INTO #NotesOrderedByDate (ParcelId ,SaleGuid ,Note  ,UpdateDate ,AssmtEntityId)
	SELECT s.ParcelId,s.SaleGuid, sn.ptas_notetext,sn.modifiedon,su.ptas_legacyid
	  FROM #SaleId s
	 INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
	    ON s.ParcelId = spdps.ptas_parceldetailid
	   AND s.SaleGuid = spdps.ptas_salesid
	 INNER JOIN [dynamics].[ptas_salesnote] sn
	    ON spdps.ptas_salesid = sn._ptas_saleid_value
	 INNER JOIN [dynamics].[systemuser] su
	    ON sn._modifiedby_value = su.systemuserid
	 ORDER BY spdps.ptas_salesid,sn.modifiedon DESC


	CREATE TABLE #SaleNote (SaleGuid uniqueidentifier, Notes nvarchar(max) default '')
	INSERT #SaleNote
		   (SaleGuid, Notes)
	SELECT  s.SaleGuid, SUBSTRING(STRING_AGG( + ' SALE NOTE '+ nbd.Note + '(' + COALESCE(convert(char(11),nbd.UpdateDate),'') + ' ' + ISNULL(nbd.AssmtEntityId,'') + '); ',' '),1,4500)
	  FROM #SaleId s
	 INNER JOIN  #NotesOrderedByDate nbd
		ON s.ParcelId = nbd.ParcelId
	   AND s.SaleGuid = nbd.SaleGuid
	 GROUP BY s.SaleGuid


UPDATE #SaleNote 
SET Notes = STUFF(notes, 1, 1, 'Equals ') where left(notes,1)='='


UPDATE #SaleNote
SET Notes = REPLACE ( Notes ,CHAR(13)+CHAR(10),CHAR(32) ) 
WHERE (CHARINDEX(CHAR(13), Notes) > 0 AND CHARINDEX(CHAR(10), Notes) > 0)



CREATE TABLE #RealProp 
(
  PIN char(10)
, MapPin float
, ParcelId uniqueidentifier
, LandId uniqueidentifier
, ResArea varchar(3)
, ApplDistrict varchar(20)
, QuarterSection char(2)
, Section tinyint
, Township tinyint
, [Range] tinyint
, Folio varchar(7)
, Notes varchar(5000)
)



--distinct, because when join with #SaleId will have multiple records for same Parcel
	INSERT #RealProp 
	SELECT DISTINCT gmd.PIN
		   ,convert(float,gmd.PIN)
		   ,s.ParcelId
		   ,gmd.LndGuid
		   ,gmd.ResArea
		   ,gmd.ApplDistrict
		   ,QuarterSection = COALESCE(dsm.value,'')
		   ,Section		   = pqstr.ptas_section	
		   ,Township	   = pqstr.ptas_township
		   ,Range		   = pqstr.ptas_range		
		   ,Folio		   = gmd.Folio
		   ,'' --Notes
	FROM #SaleId s
   INNER JOIN #GisMapDataSubSet gmd
      ON s.ParcelId = gmd.ParcelId  
	INNER JOIN [dynamics].[ptas_parceldetail] dpd
	ON s.ParcelId = dpd.ptas_parceldetailid
	LEFT JOIN [dynamics].[ptas_qstr] pqstr
      ON pqstr.[ptas_qstrid] = dpd._ptas_qstrid_value
    LEFT JOIN [dynamics].[stringmap]	dsm
      ON dsm.attributevalue = pqstr.ptas_quartersection 
     AND dsm.objecttypecode = 'ptas_qstr'
     AND dsm.attributename  = 'ptas_quartersection'


	 
--RP Notes.  At this point, only condo complexes are returned 
	CREATE TABLE #RPNotesOrderedByDate 
	(
	  rowID    int	not null  identity(1,1)
	 ,ParcelId uniqueidentifier
	 ,Note nvarchar(max)
	 ,UpdateDate smalldatetime
	 ,AssmtEntityId char(4) 
	 ,PRIMARY KEY(rowID)	
	)
	
	CREATE TABLE #Notes
	(
	  ParcelId uniqueidentifier
	 ,Notes varchar(max)
	 ,PRIMARY KEY(ParcelId)
	)

		 INSERT INTO #RPNotesOrderedByDate (ParcelId, Note, UpdateDate, AssmtEntityId)
		SELECT  rp.ParcelId,cn.ptas_notetext,cn.modifiedon,su.ptas_legacyid
	       FROM #RealProp rp
		  INNER JOIN [dynamics].[ptas_camanotes] cn
			 ON rp.ParcelId = cn._ptas_parcelid_value
			AND cn.ptas_notetype = 591500001 --Parcel Notes
		  INNER JOIN [dynamics].[systemuser] su
			 ON cn._modifiedby_value = su.systemuserid
		  ORDER BY rp.ParcelId ASC, cn.modifiedon  DESC


    	INSERT  #Notes
	            (ParcelId, Notes)
		SELECT  rp.ParcelId
				,SUBSTRING(STRING_AGG(  COALESCE(rpn.Note,'') + '(' + COALESCE(convert(char(11),rpn.UpdateDate),'') + ' ' + ISNULL(rpn.AssmtEntityId,'') + '); ',' '),1,4950)
	       FROM #RealProp rp
		  INNER JOIN #RPNotesOrderedByDate rpn
			 ON rp.ParcelId = rpn.ParcelId
		  GROUP BY rp.ParcelId

    UPDATE #RealProp
       SET Notes = CASE WHEN LEN(' RP NOTE: ' + n.Notes) >= 5000 THEN SUBSTRING(' RP NOTE: ' + n.Notes,1,4950) +' AND OTHER NOTES; '
					    ELSE ' RP NOTE: '+ n.Notes 
					END
	  FROM #RealProp rp
	 INNER JOIN #Notes n
	    ON  rp.ParcelId = n.ParcelId
	 

	UPDATE #RealProp   
	SET Notes = STUFF(notes, 1, 1, 'Equals ') where left(notes,1)='='


	UPDATE #RealProp
	SET Notes = REPLACE ( Notes ,CHAR(13)+CHAR(10),CHAR(32) ) 
	WHERE (CHARINDEX(CHAR(13), Notes) > 0 AND CHARINDEX(CHAR(10), Notes) > 0)



--DROP TABLE #CompletedWork 
CREATE TABLE #CompletedWork 
   (
	ParcelId 		  uniqueidentifier
	,CompletedWork 	  varchar(100)
	,AttnRequiredDesc varchar(80)
	)
INSERT #CompletedWork 
SELECT DISTINCT ParcelId, '','' FROM #SaleId


UPDATE #CompletedWork 
SET AttnRequiredDesc = CASE
                         WHEN cw.AttnRequiredDesc = '' THEN CONVERT(VARCHAR(3),gmd.SalesCountUnverified) + ' Sale Unverified' 
                         ELSE cw.AttnRequiredDesc + ';  ' + CONVERT(VARCHAR(3),gmd.SalesCountUnverified) + ' Sale Unverified' 
                       END
FROM #GisMapDataSubSet gmd        
INNER JOIN #CompletedWork cw ON gmd.ParcelId = cw.ParcelId        
WHERE gmd.SalesCountUnverified = 1


UPDATE #CompletedWork 
SET AttnRequiredDesc = CASE
                         WHEN cw.AttnRequiredDesc = '' THEN CONVERT(VARCHAR(3),gmd.SalesCountUnverified) + ' Sales Unverified' 
                         ELSE cw.AttnRequiredDesc + ';  ' + CONVERT(VARCHAR(3),gmd.SalesCountUnverified) + ' Sales Unverified' 
                       END
FROM #GisMapDataSubSet gmd        
INNER JOIN #CompletedWork cw ON gmd.ParcelId = cw.ParcelId        
WHERE gmd.SalesCountUnverified > 1



/*
Hairo Comment: I need to find the way to calculate this, for Inspection I have a view(vw_Inspections) that required to be modified 
since the results are not accurate, could be a problem with the data but I need to find out

UPDATE #CompletedWork 
SET CompletedWork = 'Compl Inspect = ' + gmd.Inspection 
FROM GisMapData gmd 
INNER JOIN #CompletedWork cw ON gmd.ParcelId = cw.ParcelId 
WHERE EXISTS (select * from ResPhysInsp rpi inner join AssmtYr ay on ay.RPAssmtYr = rpi.AssmtYr where rpi.ApplGroup = 'R' and rpi.ResArea = gmd.ResArea)
AND gmd.Inspection <> ''

*/

UPDATE #CompletedWork 
SET CompletedWork = CASE
                      WHEN CompletedWork = '' THEN CONVERT(VARCHAR(3),gmd.SalesCountVerifiedThisCycle) + ' Sale Verified' 
                      ELSE CompletedWork + ';  ' + CONVERT(VARCHAR(3),gmd.SalesCountVerifiedThisCycle) + ' Sale Verified' 
                    END
FROM #GisMapDataSubSet gmd        
INNER JOIN #CompletedWork cw ON gmd.ParcelId = cw.ParcelId        
WHERE gmd.SalesCountVerifiedThisCycle = 1

UPDATE #CompletedWork 
SET CompletedWork = CASE
                      WHEN CompletedWork = '' THEN CONVERT(VARCHAR(3),gmd.SalesCountVerifiedThisCycle) + ' Sales Verified' 
                      ELSE CompletedWork + ';  ' + CONVERT(VARCHAR(3),gmd.SalesCountVerifiedThisCycle) + ' Sales Verified' 
                    END
FROM #GisMapDataSubSet gmd   
INNER JOIN #CompletedWork cw ON gmd.ParcelId = cw.ParcelId             
WHERE gmd.SalesCountVerifiedThisCycle > 1


/*
Hairo comment: in order to calculate this columns, I need to calculate first the Permits data for GisMapData

UPDATE #CompletedWork 
SET CompletedWork = CASE
                      WHEN CompletedWork = '' THEN CONVERT(VARCHAR(3),gmd.PermitCountComplCurCycle) + ' Permit Completed' 
                      ELSE CompletedWork + ';  ' + CONVERT(VARCHAR(3),gmd.PermitCountComplCurCycle) + ' Permit Completed' 
                    END
FROM GisMapData gmd
INNER JOIN #CompletedWork cw ON gmd.RpGuid = cw.RpGuid
WHERE gmd.PermitCountComplCurCycle = 1

UPDATE #CompletedWork 
SET CompletedWork = CASE
                      WHEN CompletedWork = '' THEN CONVERT(VARCHAR(3),gmd.PermitCountComplCurCycle) + ' Permits Completed' 
                      ELSE CompletedWork + ';  ' + CONVERT(VARCHAR(3),gmd.PermitCountComplCurCycle) + ' Permits Completed' 
                    END
FROM GisMapData gmd
INNER JOIN #CompletedWork cw ON gmd.RpGuid = cw.RpGuid
WHERE gmd.PermitCountComplCurCycle > 1

*/


	CREATE TABLE #SaleWarningId (RowNum int identity, SaleGuid uniqueidentifier,Warning Varchar(500), WarningOrder int)
	
	
	INSERT INTO #SaleWarningId(SaleGuid,Warning,WarningOrder )
	SELECT  sswc.ptas_salesid
		   ,swc.ptas_name
		   ,ROW_NUMBER() OVER (PARTITION BY sswc.ptas_salesid ORDER BY sswc.modifiedon) AS WarningOrder
	  FROM #SaleId s
	 INNER JOIN dynamics.ptas_sales_ptas_saleswarningcode sswc
	    ON s.SaleGuid = sswc.ptas_salesid
	 INNER JOIN dynamics.ptas_saleswarningcode swc
	    ON swc.ptas_saleswarningcodeid = sswc.ptas_saleswarningcodeid
	 ORDER BY sswc.ptas_salesid


IF @MtchGrpSrchY = 'Y' 
BEGIN
   DELETE #SaleId 
   FROM #RealProp rp
   INNER JOIN #SaleId s ON rp.ParcelId = s.ParcelId
   INNER JOIN [dynamics].[ptas_sales] ps ON s.SaleGuid = ps.ptas_salesid
   WHERE s.AtMarket = 'N'  
   OR s.SaleTypeRP <> 'ResImpSale' 
   OR ps.ptas_nbrparcels <> 1 
   OR s.PrevAVSP <= .333 
   OR s.PrevAVSP >= 3
END


CREATE TABLE #TurningPointInDistricts 
(
 ApplDistrict varchar(20)
 ,TurningDay float
 ,TurningDate date
 )
 
INSERT #TurningPointInDistricts SELECT 'ResNortheast',-206, '6/9/2018'
INSERT #TurningPointInDistricts SELECT 'ResNorthwest',-230, '5/16/2018'
INSERT #TurningPointInDistricts SELECT 'ResSoutheast',-202, '6/13/2018'
INSERT #TurningPointInDistricts SELECT 'ResSouthwest',-182, '7/3/2018' -- (constrained to Spring 2018 during NCSS analysis - otherwise the Linear-Linear proc found a nearly straigh line with a 'turning point' in Fall 2018)
INSERT #TurningPointInDistricts SELECT 'ResWest Central',-207, '6/8/2018'
INSERT #TurningPointInDistricts SELECT 'COUNTYWIDE',-209, '6/6/2018'

CREATE TABLE #ResAreaCounts 
(
 RowNum int identity
 ,ApplDistrict varchar(20)
 ,ResArea varchar(3)
 ,Counts int
)
INSERT #ResAreaCounts (ApplDistrict, ResArea, Counts)
SELECT rp.ApplDistrict
      ,rp.ResArea
      ,COUNT(*)
FROM #SaleId s
INNER JOIN #RealProp rp ON s.ParcelId = rp.ParcelId
GROUP BY rp.ApplDistrict ,rp.ResArea
ORDER BY COUNT(*) DESC

--Handle input where user selects @CountywideY = 'Y' and then selects additional location parameters
DECLARE @CountywideIsTheOnlyLocationParameter char(1) --Y or blank
SELECT @CountywideIsTheOnlyLocationParameter = ''

DECLARE @CountOfAllResAreasInKC int
SELECT @CountOfAllResAreasInKC = (  SELECT COUNT(DISTINCT dpd.ptas_resarea) FROM dynamics.ptas_parceldetail dpd
									INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
									WHERE ISNULL(dpd.ptas_resarea,'') <> ''
									AND dpd._ptas_landid_value IS NOT NULL
									AND pt.PropType <> 'K' -- K = Condominium
									OR (pt.PropType = 'K' AND dpd.ptas_Minor = '0000'))
								  


								  

IF (select count(*) from @ResAreaParam) <> @CountOfAllResAreasInKC  
SELECT @CountywideIsTheOnlyLocationParameter = 'Y'


 
IF @MarketTurnDate = '1/1/1900' AND @CountywideIsTheOnlyLocationParameter = 'Y' SELECT @MarketTurnDate = (select TurningDate from #TurningPointInDistricts where ApplDistrict = 'COUNTYWIDE')
IF @MarketTurnDate = '1/1/1900' AND @ApplDistrict <> '' SELECT @MarketTurnDate = (select TurningDate from #TurningPointInDistricts where ApplDistrict = @ApplDistrict)

IF @MarketTurnDate = '1/1/1900' AND @CountywideIsTheOnlyLocationParameter = '' AND @ApplDistrict = '' 
BEGIN
	SELECT @MarketTurnDate = TurningDate 
	FROM #TurningPointInDistricts t
	INNER JOIN #ResAreaCounts c ON c.ApplDistrict = t.ApplDistrict
	WHERE c.RowNum = 1
END



 DECLARE
  @R_Square float 
 ,@Slope float 
 ,@Y_Intercept float 
 ,@X_Counts float
 ,@TrendConclusion varchar(200)
 ,@Exact_Match_or_Interpolate_T_Table varchar(120) 
 ,@Y_Average float 
 ,@X_Average float 
 ,@t_Of_Slope float 
 ,@Critical_T_Alpha05 float 
 ,@Critical_T_Alpha10 float 
 ,@Critical_T_Alpha20 float
 ,@T_Of_Slope_to_Critical_T_Alpha05 float  
 ,@T_Of_Slope_to_Critical_T_Alpha10 float  
 ,@T_Of_Slope_to_Critical_T_Alpha20 float     
 ,@DegreesOfFreedom float 
 ,@LowerLimit_DegFree_T_Table_For_Interpol float
 ,@UpperLimit_DegFree_T_Table_For_Interpol float
 ,@Lower_Crit_T_For_Interpol_Alpha05 float 
 ,@Upper_Crit_T_For_Interpol_Alpha05 float 
 ,@Lower_Crit_T_For_Interpol_Alpha10 float 
 ,@Upper_Crit_T_For_Interpol_Alpha10 float 
 ,@Lower_Crit_T_For_Interpol_Alpha20 float  
 ,@Upper_Crit_T_For_Interpol_Alpha20 float  

DECLARE @AvgTrendedRatio float
DECLARE @TrendedRatioCounts int
DECLARE @StdDevTrendedRatio float
DECLARE @UpperLimit float
DECLARE @LowerLimit float

DECLARE @TrendCounter int



CREATE TABLE #SalesUsedInRegress  
(
  RegressionRunNumber int
 ,TimePeriodForPart1TimeAdjustment varchar(30)
 ,PrevAVSP decimal(20,2)
 ,SalePrice float
 ,ExciseTaxNbr int
 ,SaleDay int
 ,SaleDate date
 )

CREATE TABLE #AllRatiosTrended --trended whether or not part of regression sample 
(
  RegressionRunNumber int
 ,TimePeriodForRegress1TimeAdjustment varchar(100)
 ,TimePeriodForRegress2TimeAdjustment varchar(100)
 ,Regress1TimeAdjustment float
 ,Regress2TimeAdjustment float
 ,FinalTimeAdjustment float
 ,PredictedRatio float
 ,PrevAVSP decimal(20,2)
 ,TrendedRatio float
 ,SalePrice float
 ,TrendedSalePrice float
 ,ExciseTaxNbr int
 ,SaleDay int
 ,SaleDate date
 ,AdminFactorApplied  decimal(20,3)
)


--Since more than 1 simple linear regression run is needed to capture non-linear trend, keep track of separate runs
CREATE TABLE #StartEndDates 
(Rownum int identity
, StartDate date
, EndDate date
, StartDay float
, EndDay float
, Slope float
, Y_Intercept float
, X_Counts float
, TrendConclusion varchar(200) 
, Y_Average float 
, X_Average float 
, t_Of_Slope float 
, Critical_T_Alpha05 float 
, Critical_T_Alpha10 float 
, Critical_T_Alpha20 float
, T_Of_Slope_to_Critical_T_Alpha20 float --apply trend if this threshold is met (T_Of_Slope_to_Critical_T_Alpha20 > 1 )
, SaleDayAtCommonDate float
, PredictedAtCommonDate float
, CorrectionFactor float
, AdjustedSlope float --adjust later shorter less reliable line to match up with prior line
, AdjustedY_Intercept float --ditto
 )


--User could specify @TrendToDate that is after the latest sale (i.e. extrapolation) or sooner than the latest sale (i.e. extra data to get a better line).
--Keep track of that and inform the user 
DECLARE @LatestSaleDateInSalesSample date  
SELECT @LatestSaleDateInSalesSample = MAX(SaleDate)
FROM #SaleId s
WHERE SalePrice > 0


DECLARE @LatestSaleDateForRegression date
SELECT @LatestSaleDateForRegression = MAX(SaleDate)
FROM #SaleId s
WHERE SalePrice > 0
AND PrevAVSP > @MinRatioUsedInTrend
AND PrevAVSP < @MaxRatioUsedInTrend
AND AtMarket <> 'N'
AND PropCnt = 1




INSERT #StartEndDates (StartDate, EndDate)
SELECT @StartSaleDate, @MarketTurnDate --derived from countywide and district graphs

INSERT #StartEndDates (StartDate, EndDate)
SELECT @MarketTurnDate, @LatestSaleDateForRegression


UPDATE #StartEndDates
SET SaleDayAtCommonDate = DATEDIFF(day,@TrendToDate, EndDate)
WHERE Rownum = 1

UPDATE #StartEndDates
SET SaleDayAtCommonDate = DATEDIFF(day,@TrendToDate, StartDate)
WHERE Rownum = 2

UPDATE #StartEndDates
SET StartDay = DATEDIFF(day,@TrendToDate, StartDate)
   ,EndDay =  DATEDIFF(day,@TrendToDate, EndDate)


----debug
--print 'select * from #SaleId'
--select * from #SaleId

--select StartSaleDate = @StartSaleDate
--select LatestSaleDateForRegression = @LatestSaleDateForRegression

--print 'select * from #StartEndDates'
--select * from #StartEndDates
--print 'select * from #ResAreaCounts'
--select * from #ResAreaCounts



SELECT @TrendCounter = (select max(Rownum) from #StartEndDates)


--In order to call the procedure ASSR_SimpleLinearRegression, #SimpleLinearRegression MUST include columns noted below. Any other columns can be added as desired. 
CREATE TABLE #SimpleLinearRegression 
( DependentVariable float --required     
, IndependentVariable float --required
, PredictedValue float --required
, ObservedMinusPredicted float --required
, ExciseTaxNbr int
, SalePrice int
, SaleDate smalldatetime
, AtMarket varchar(10)
, PropCnt int
, SaleGuid uniqueidentifier
, SaleId int
 )

WHILE @TrendCounter > 0
  BEGIN

  TRUNCATE TABLE #SimpleLinearRegression

  INSERT #SimpleLinearRegression
  SELECT 
   DependentVariable = PrevAVSP  
  ,IndependentVariable = DATEDIFF(day,@TrendToDate,SaleDate)  
  ,PredictedValue = 0  
  ,ObservedMinusPredicted = 0  
  ,ExciseTaxNbr
  ,SalePrice
  ,SaleDate
  ,AtMarket
  ,PropCnt
  ,SaleGuid  
  ,SaleId  
  FROM #SaleId s
  WHERE SalePrice > 0
  AND PrevAVSP > @MinRatioUsedInTrend
  AND PrevAVSP < @MaxRatioUsedInTrend
  AND AtMarket <> 'N'
  AND PropCnt = 1
  AND SaleDate >= (select StartDate from #StartEndDates where Rownum = @TrendCounter)
  AND SaleDate <= (select EndDate from #StartEndDates where Rownum = @TrendCounter)
  ORDER BY DATEDIFF(day,@TrendToDate,SaleDate) DESC

  ----debug 
  --print '#SimpleLinearRegression'
  --select * from #SimpleLinearRegression


  EXEC cus.ASSR_SimpleLinearRegression
   @R_Square  output
  ,@Slope  output
  ,@Y_Intercept  output
  ,@X_Counts  output
  ,@TrendConclusion output
  ,@Exact_Match_or_Interpolate_T_Table  output 
  ,@Y_Average  output
  ,@X_Average  output
  ,@t_Of_Slope  output
  ,@Critical_T_Alpha05  output
  ,@Critical_T_Alpha10  output
  ,@Critical_T_Alpha20  output  
  ,@T_Of_Slope_to_Critical_T_Alpha05  output 
  ,@T_Of_Slope_to_Critical_T_Alpha10  output 
  ,@T_Of_Slope_to_Critical_T_Alpha20  output 
  ,@DegreesOfFreedom  output 
  ,@LowerLimit_DegFree_T_Table_For_Interpol output
  ,@UpperLimit_DegFree_T_Table_For_Interpol output
  ,@Lower_Crit_T_For_Interpol_Alpha05  output
  ,@Upper_Crit_T_For_Interpol_Alpha05  output
  ,@Lower_Crit_T_For_Interpol_Alpha10  output
  ,@Upper_Crit_T_For_Interpol_Alpha10  output
  ,@Lower_Crit_T_For_Interpol_Alpha20  output 
  ,@Upper_Crit_T_For_Interpol_Alpha20  output 


  ----debug
  --SELECT
  -- R_Square = @R_Square  
  --,Slope   =  @Slope  
  --,Y_Intercept   =  @Y_Intercept  
  --,X_Counts   =  @X_Counts  
  --,TrendConclusion = @TrendConclusion 
  --,Exact_Match_or_Interpolate_T_Table = @Exact_Match_or_Interpolate_T_Table   
  --,Y_Average   =  @Y_Average  
  --,X_Average   =  @X_Average  
  --,t_Of_Slope   =  @t_Of_Slope  
  --,Critical_T_Alpha05   =  @Critical_T_Alpha05  
  --,Critical_T_Alpha10   =  @Critical_T_Alpha10  
  --,Critical_T_Alpha20 =   @Critical_T_Alpha20   
  --,T_Of_Slope_to_Critical_T_Alpha05 = @T_Of_Slope_to_Critical_T_Alpha05   
  --,T_Of_Slope_to_Critical_T_Alpha10 = @T_Of_Slope_to_Critical_T_Alpha10   
  --,T_Of_Slope_to_Critical_T_Alpha20 = @T_Of_Slope_to_Critical_T_Alpha20  
  --,DegreesOfFreedom   =  @DegreesOfFreedom 
  --,LowerLimit_DegFree_T_Table_For_Interpol = @LowerLimit_DegFree_T_Table_For_Interpol
  --,UpperLimit_DegFree_T_Table_For_Interpol = @UpperLimit_DegFree_T_Table_For_Interpol
  --,Lower_Crit_T_For_Interpol_Alpha05   =  @Lower_Crit_T_For_Interpol_Alpha05 
  --,Upper_Crit_T_For_Interpol_Alpha05   =  @Upper_Crit_T_For_Interpol_Alpha05   
  --,Lower_Crit_T_For_Interpol_Alpha10   =  @Lower_Crit_T_For_Interpol_Alpha10  
  --,Upper_Crit_T_For_Interpol_Alpha10   =  @Upper_Crit_T_For_Interpol_Alpha10  
  --,Lower_Crit_T_For_Interpol_Alpha20   = @Lower_Crit_T_For_Interpol_Alpha20   
  --,Upper_Crit_T_For_Interpol_Alpha20   = @Upper_Crit_T_For_Interpol_Alpha20   


  ----debug
  --select * from #SimpleLinearRegression

  --The dependent variable PrevAVSP is simply the intercept at SaleDay 0.
  --All other ratios must be trended up to SaleDay 0
  
  UPDATE #StartEndDates 
  SET Slope = @Slope
     ,Y_Intercept = @Y_Intercept
     ,X_Counts = @X_Counts
     ,TrendConclusion = @TrendConclusion
     ,Y_Average = @Y_Average
     ,X_Average = @X_Average
     ,t_Of_Slope = @t_Of_Slope
     ,Critical_T_Alpha05 = @Critical_T_Alpha05 
     ,Critical_T_Alpha10 = @Critical_T_Alpha10
     ,Critical_T_Alpha20 = @Critical_T_Alpha20
     ,T_Of_Slope_to_Critical_T_Alpha20 = @T_Of_Slope_to_Critical_T_Alpha20
  WHERE RowNum = @TrendCounter



  --Keep track of which sales were used in each step of regression, provided that the results are worth considering (@T_Of_Slope_to_Critical_T_Alpha20 > 1)
  IF @T_Of_Slope_to_Critical_T_Alpha20 > 1 
     BEGIN
     INSERT #SalesUsedInRegress  
     SELECT DISTINCT
      RegressionRunNumber = @TrendCounter
     ,TimePeriodForPart1TimeAdjustment = (select convert(varchar(12),StartDate) + ' - ' + convert(varchar(12),EndDate) from #StartEndDates where @TrendCounter = Rownum) 
     ,PrevAVSP = DependentVariable
     ,SalePrice 
     ,ExciseTaxNbr 
     ,SaleDay = IndependentVariable  
     ,SaleDate 
     FROM #SimpleLinearRegression r
     WHERE SalePrice > 0
     AND DependentVariable > @MinRatioUsedInTrend
     AND DependentVariable < @MaxRatioUsedInTrend
     AND AtMarket <> 'N'
     AND PropCnt = 1
     ORDER BY IndependentVariable
   END

  SELECT @TrendCounter =  @TrendCounter - 1

END --if @TrendCounter > 0

UPDATE #StartEndDates 
SET PredictedAtCommonDate = Y_Intercept + Slope * SaleDayAtCommonDate 

--For this particular cycle, adjust the shorter line to the longer line with more reliable data. TODO adjust both proportional to relative uncertainties
UPDATE #StartEndDates 
SET CorrectionFactor = 1 
   ,AdjustedSlope = Slope
   ,AdjustedY_Intercept = Y_Intercept
WHERE RowNum = 1

UPDATE #StartEndDates 
SET CorrectionFactor = (select PredictedAtCommonDate from #StartEndDates where RowNum = 1)/(select PredictedAtCommonDate from #StartEndDates where RowNum = 2)
WHERE RowNum = 2

UPDATE #StartEndDates 
SET AdjustedSlope = Slope * CorrectionFactor
   ,AdjustedY_Intercept = Y_Intercept * CorrectionFactor
WHERE RowNum = 2

DECLARE @TrendConclusionPart1Summary varchar(300)
DECLARE @TrendConclusionPart2Summary varchar(300)


SELECT  @TrendConclusionPart1Summary = 
        CASE
          WHEN (select T_Of_Slope_to_Critical_T_Alpha20 from #StartEndDates where RowNum = 1) > 1
          THEN  (select convert(varchar(20),StartDate) + ' - ' + convert(varchar(20),EndDate) + ':' 
                + ' Sales Count = ' + convert(varchar(20),X_Counts) + ', '  
                + TrendConclusion 
                + ' Slope = ' + convert(varchar(20),Slope) +  ', Intercept = ' + convert(varchar(20),Y_Intercept) 
                from #StartEndDates where Rownum = 1) 
          ELSE 'No significant trend for this time period'
        END

SELECT  @TrendConclusionPart2Summary = 
        CASE
          WHEN (select T_Of_Slope_to_Critical_T_Alpha20 from #StartEndDates where RowNum = 2) > 1
          THEN  (select convert(varchar(20),StartDate) + ' - ' + convert(varchar(20),EndDate) + ':' 
                + ' Sales Count = ' + convert(varchar(20),X_Counts) + ', '  
                + TrendConclusion 
                + ' Slope = ' + convert(varchar(20),Slope) +  ', Intercept = ' + convert(varchar(20),Y_Intercept) 
                from #StartEndDates where Rownum = 2) 
          ELSE 'No significant trend for this time period'
        END


--Created this as a single overall conclusion, but will try above approach 1st and see how it is accepted by users
--DECLARE @TrendConclusionOverallSummary varchar(300)
--SET @TrendConclusionOverallSummary = 
--       (select convert(varchar(12),StartDate) + ' - ' + convert(varchar(12),EndDate) + ':' 
--        + ' Sales Count = ' + convert(varchar(12),X_Counts) + ', '  
--        + TrendConclusion 
--        + ' Slope = ' + convert(varchar(12),Slope) +  ', Intercept = ' + convert(varchar(12),Y_Intercept) 
--        from #StartEndDates where Rownum = 1) 
--        + '       '
--        + (select convert(varchar(12),StartDate) + ' - ' + convert(varchar(12),EndDate) + ':'
--        + ' Sales Count = ' + convert(varchar(12),X_Counts) + ', '            
--        + TrendConclusion 
--        + ' Slope = ' + convert(varchar(12),Slope) +  ', Intercept = ' + convert(varchar(12),Y_Intercept) 
--        from #StartEndDates where Rownum = 2) 


DECLARE @TrendOverTimePeriod decimal(20,2)



--INSERT #AllRatiosTrended  
--In steps above, #SaleId was updated with aggregate data for multi parcel sales, so can get that data too in  DISTINCT SELECT.  
INSERT #AllRatiosTrended (PrevAVSP, SalePrice, ExciseTaxNbr, SaleDay, SaleDate, AdminFactorApplied)
SELECT DISTINCT
  PrevAVSP 
 ,SalePrice  
 ,ExciseTaxNbr  
 ,SaleDay 
 ,SaleDate  
 ,AdminFactorApplied = @AdminFactorApplied
FROM #SaleId
WHERE PropCnt = 1

INSERT #AllRatiosTrended (PrevAVSP, SalePrice, ExciseTaxNbr, SaleDay, SaleDate, AdminFactorApplied)
SELECT DISTINCT
  PrevAVSP 
 ,SalePrice  
 ,ExciseTaxNbr  
 ,SaleDay 
 ,SaleDate  
 ,AdminFactorApplied = @AdminFactorApplied
FROM #SaleId
WHERE PropCnt > 1



UPDATE #AllRatiosTrended  
SET RegressionRunNumber = 1 
FROM #AllRatiosTrended t
WHERE EXISTS (select * from #StartEndDates where t.SaleDay >= StartDay and t.SaleDay <= EndDay and RowNum = 1) 

UPDATE #AllRatiosTrended  
SET RegressionRunNumber = 2
FROM #AllRatiosTrended t
WHERE EXISTS (select * from #StartEndDates where t.SaleDay > StartDay and t.SaleDay <= EndDay and RowNum = 2) --  > instead of >= on this one



 --,FinalPredictedRatio float  ??? is this needed?


UPDATE #AllRatiosTrended 
SET  TimePeriodForRegress1TimeAdjustment = (select convert(varchar(12),StartDate) + ' - ' + convert(varchar(12),EndDate) from #StartEndDates where Rownum = 1) 
    ,TimePeriodForRegress2TimeAdjustment = ''
    ,Regress1TimeAdjustment = (select 1/((AdjustedSlope * t.SaleDay + AdjustedY_Intercept)/ (AdjustedSlope * ed.EndDay + AdjustedY_Intercept)) from #StartEndDates ed where Rownum = 1) 
    ,Regress2TimeAdjustment = (select 1/((AdjustedSlope * ed.StartDay + AdjustedY_Intercept)/ (AdjustedSlope * ed.EndDay + AdjustedY_Intercept)) from #StartEndDates ed where Rownum = 2)  
    ,PredictedRatio = (select AdjustedSlope * t.SaleDay + AdjustedY_Intercept from #StartEndDates ed where Rownum = 1) 
FROM #AllRatiosTrended t
WHERE RegressionRunNumber = 1


UPDATE #AllRatiosTrended 
SET  TimePeriodForRegress1TimeAdjustment = ''
    ,TimePeriodForRegress2TimeAdjustment = (select convert(varchar(12),StartDate) + ' - ' + convert(varchar(12),EndDate) from #StartEndDates where Rownum = 2) 
    ,Regress1TimeAdjustment = 1
    ,Regress2TimeAdjustment = (select 1/((AdjustedSlope * t.SaleDay + AdjustedY_Intercept)/ (AdjustedSlope * ed.EndDay + AdjustedY_Intercept)) from #StartEndDates ed where Rownum = 2)  
    ,PredictedRatio = (select AdjustedSlope * t.SaleDay + AdjustedY_Intercept from #StartEndDates ed where Rownum = 2) 
FROM #AllRatiosTrended t
WHERE RegressionRunNumber = 2



IF @LatestSaleDateInSalesSample > @LatestSaleDateForRegression 
BEGIN
  UPDATE #AllRatiosTrended 
  SET  RegressionRunNumber = 0
      ,TimePeriodForRegress1TimeAdjustment = ''
      ,TimePeriodForRegress2TimeAdjustment = ('Extrapolated beyond latest regression SaleDate of' + convert(varchar(12),@LatestSaleDateForRegression)) 
      ,Regress1TimeAdjustment = 1
      ,Regress2TimeAdjustment = (select 1/((AdjustedSlope * t.SaleDay + AdjustedY_Intercept)/ (AdjustedSlope * ed.EndDay + AdjustedY_Intercept)) from #StartEndDates ed where Rownum = 2)  
  FROM #AllRatiosTrended t
  WHERE t.SaleDate > @LatestSaleDateForRegression
END





UPDATE #AllRatiosTrended  
SET FinalTimeAdjustment = Regress1TimeAdjustment * Regress2TimeAdjustment

 
UPDATE #AllRatiosTrended  
SET TrendedRatio = PrevAVSP * FinalTimeAdjustment

--e.g., upward trend in sales is the reciprical of the downward trend in ratios
UPDATE #AllRatiosTrended  
SET TrendedSalePrice = SalePrice * 1/FinalTimeAdjustment 
WHERE FinalTimeAdjustment > 0


------debug
-- print 'select * from #StartEndDates'
-- select * from #StartEndDates

--print 'select * from #SaleId'
-- select * from #SaleId where ExciseTaxNbr = 2814288


--print 'select * from #SalesUsedInRegress'
--select * from #SalesUsedInRegress where ExciseTaxNbr = 2814288 order by SaleDay

--print 'select * from #AllRatiosTrended'
--select * from #AllRatiosTrended where ExciseTaxNbr = 2814288 order by SaleDay

--print 'select * from #AllRatiosTrended if used in regression'
--select a.* from #AllRatiosTrended a inner join #SalesUsedInRegress s on s.ExciseTaxNbr = a.ExciseTaxNbr where a.ExciseTaxNbr = 2814288 order by a.SaleDay

--return(0)


SELECT 
 @AvgTrendedRatio = AVG(TrendedRatio)
,@TrendedRatioCounts = COUNT(*) 
,@StdDevTrendedRatio = STDEV(TrendedRatio)
FROM #AllRatiosTrended t
INNER JOIN #SalesUsedInRegress r ON t.ExciseTaxNbr = r.ExciseTaxNbr --join to this table so stats are based on regression data

--@RatioWarningStdDev is param set by user
SELECT 
 @UpperLimit = @AvgTrendedRatio + @StdDevTrendedRatio * @RatioWarningStdDev 
,@LowerLimit = @AvgTrendedRatio - @StdDevTrendedRatio * @RatioWarningStdDev



UPDATE #SaleId 
SET RatioWarning = 'High Ratio' 
FROM #SaleId s
INNER JOIN #AllRatiosTrended art ON art.ExciseTaxNbr = s.ExciseTaxNbr
WHERE TrendedRatio > @UpperLimit


UPDATE #SaleId 
SET RatioWarning = 'Low Ratio' 
FROM #SaleId s
INNER JOIN #AllRatiosTrended art ON art.ExciseTaxNbr = s.ExciseTaxNbr
WHERE TrendedRatio < @LowerLimit
   

UPDATE #SaleId 
SET RatioWarning = 'High Ratio' 
FROM #SaleId s
INNER JOIN #AllRatiosTrended art ON art.ExciseTaxNbr = s.ExciseTaxNbr
WHERE TrendedRatio > @UpperLimit

UPDATE #SaleId 
SET RatioWarning = 'Low Ratio' 
FROM #SaleId s
INNER JOIN #AllRatiosTrended art ON art.ExciseTaxNbr = s.ExciseTaxNbr
WHERE TrendedRatio < @LowerLimit   
   


--Report on sales removed

DECLARE @AdditionalSalesRemoved varchar (200)
SET @AdditionalSalesRemoved = ''

DECLARE @CountSaleWarningParcelsRemoved int
       ,@CountBldgWarningParcelsRemoved int
       ,@CountLandWarningParcelsRemoved int
       ,@CountSaleParcelsTotal int

SET @CountSaleWarningParcelsRemoved = (select count(*) from #SaleId WHERE LEN(SaleWarnings) > 0 and AtMarket <> 'Y')
SET @CountBldgWarningParcelsRemoved = (select count(*) from #SaleId WHERE LEN(BuildingWarning) > 0 and AtMarket = 'Y')
SET @CountLandWarningParcelsRemoved = (select count(*) from #SaleId WHERE LEN(LandWarning) > 0 and AtMarket = 'Y') 
SET @CountSaleParcelsTotal = (select count(*) from #SaleId) 


SET @AdditionalSalesRemoved = CASE WHEN @ExcludeSaleWarnings = 'Y' OR @ExcludeBldgWarnings = 'Y' OR @ExcludeLandWarnings = 'Y' 
                                  THEN @AdditionalSalesRemoved + 'Sales removed out of '  + CONVERT(varchar(20),@CountSaleParcelsTotal) + ' total: '
                                  ELSE @AdditionalSalesRemoved
                             END   

                           + CASE WHEN @ExcludeSaleWarnings = 'Y' THEN @AdditionalSalesRemoved + CONVERT(varchar(20),@CountSaleWarningParcelsRemoved) + ' sales with sales warnings; ' ELSE @AdditionalSalesRemoved END
                           + CASE WHEN @ExcludeBldgWarnings = 'Y' THEN @AdditionalSalesRemoved + CONVERT(varchar(20),@CountBldgWarningParcelsRemoved) + ' sales with building warnings; ' ELSE @AdditionalSalesRemoved END
                           + CASE WHEN @ExcludeLandWarnings = 'Y' THEN @AdditionalSalesRemoved + CONVERT(varchar(20),@CountLandWarningParcelsRemoved) + ' sales with land warnings' ELSE @AdditionalSalesRemoved END
                           
                               
                                
--Delete the sales to be removed
IF @ExcludeSaleWarnings = 'Y' OR @MtchGrpSrchY = 'Y'
BEGIN 
  DELETE #SaleId 
  WHERE 
  LEN(SaleWarnings) > 0 --default ''
  AND (AtMarket = '' OR AtMarket <>  'Y')
END


IF @ExcludeBldgWarnings = 'Y' OR @MtchGrpSrchY = 'Y' DELETE #SaleId WHERE LEN(BuildingWarning) > 0 --default ''

IF @ExcludeLandWarnings = 'Y' OR @MtchGrpSrchY = 'Y' DELETE #SaleId WHERE LEN(LandWarning) > 0 --default ''



------DEBUG subset for analysis - used in regresssion only
------SEE ALSO LINE 1840 IN REGRESSION RUN FOR ADDL DEBUG TO BE REMOVED
--  SELECT DISTINCT
--    --Core Char 
--    Major  
--   ,Minor

--   ,rp.ApplDistrict
--   ,rp.ResArea
--   ,s.ExciseTaxNbr
--   ,rps.SaleDate 
--   ,s.SaleDay
--   ,rps.SalePrice 
--   ,TrendedSalePrice = CONVERT(int,1/tr.FinalTimeAdjustment * rps.SalePrice)
--   ,TrendAppliedToSalePrice = CONVERT(decimal(3),1/tr.FinalTimeAdjustment)
--   ,s.PrevAVSP
--   ,TrendedRatio = ISNULL(convert(varchar(10),convert(Decimal(20,2),tr.TrendedRatio)),'')
--   ,TrendAppliedToRatio = tr.FinalTimeAdjustment
--   ,s.AtMarket 
--   ,RatioWarning  
--   ,BldgGrade
--   ,TrendConclusion = CASE
--                        WHEN EXISTS (select * from #StartEndDates where s.SaleDay > StartDay and s.SaleDay <= EndDay and RowNum = 1) 
--                        THEN @TrendConclusionPart1Summary
--                        WHEN EXISTS (select * from #StartEndDates where s.SaleDay > StartDay and s.SaleDay <= EndDay and RowNum = 2) 
--                        THEN @TrendConclusionPart2Summary
--                        ELSE ''
--                      END
--FROM GisMapData gmd
--INNER JOIN #RealProp rp ON gmd.RpGuid = rp.RpGuid
--INNER JOIN #SaleId s ON gmd.RpGuid = s.RpGuid
--INNER JOIN #CompletedWork cw ON gmd.RpGuid = cw.RpGuid
--INNER JOIN Sale rps ON s.SaleGuid = rps.SaleGuid
--INNER JOIN SaleVerif sv ON rps.SaleGuid = sv.SaleGuid
--INNER JOIN #SalesUsedInRegress r ON r.ExciseTaxNBr = s.ExciseTaxNBr --added this for debug
--LEFT JOIN #SaleWarnings sw ON s.SaleGuid = s.SaleGuid 
--LEFT JOIN #SaleNotes sn ON s.SaleGuid = sn.SaleGuid 
--LEFT JOIN #AllRatiosTrended tr ON s.ExciseTaxNbr  = tr.ExciseTaxNbr
--ORDER BY rps.SaleDate
--RETURN(0)
----DEBUG



  --Final result
  SELECT DISTINCT
    --Core Char 
    gmd.Major  
   ,gmd.Minor
   ,gmd.AssignedBoth
   ,gmd.ResArea
   ,gmd.ResSubArea 
   ,gmd.ResNbhd 
   ,SubGroupingType = @SubGroupingType
   ,gmd.AddrLine
   ,gmd.YrBuiltRes	
   ,gmd.YrRenovated
   ,gmd.BldgGrade
   ,gmd.Condition
   ,gmd.SqFtTotLiving AS ResTotLiv
   
   ,gmd.PresentUse
   ,gmd.SqFtLot
   ,gmd.CurrentZoning
   ,gmd.ViewDescr
   ,LandProbDescr = CASE WHEN COALESCE(LandProbDescrPart1,'') + COALESCE(LandProbDescrPart2,'')='' THEN 'No Land Problems'
						ELSE COALESCE(LandProbDescrPart1,'') + COALESCE(LandProbDescrPart2,'')
					END
  
     --Sale data
   ,TrendedSalePrice = COALESCE(CONVERT(int,1/tr.FinalTimeAdjustment * s.SalePrice),'')   
   ,s.SalePrice     
   ,s.SaleDate 
   ,TrendAppliedToSalePrice = COALESCE(CONVERT(decimal(3),1/tr.FinalTimeAdjustment),0)	    
   ,PropClass = [dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_salepropertyclass',dps.ptas_salepropertyclass)
   ,AtMarket = COALESCE(s.AtMarket ,'(unknown)')
   ,VerifLevel =  COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_levelofverification',dps.ptas_levelofverification),'')
   ,VerifiedBy = COALESCE(dps.ptas_verifiedby,'')
   ,VerifDate  = COALESCE(FORMAT(dps.ptas_verifiedbydate ,'d','en-us'),'')   
   ,SaleReason =  COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_reason',dps.ptas_reason),'')
   ,SaleInstrument  = COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_instrument',dps.ptas_instrument),'')
   ,IdentifiedBy    = COALESCE(dps.ptas_identifiedby,'')
   ,IdentifiedDate  = COALESCE(FORMAT(dps.ptas_identifiedbydate,'d','en-us'),'')   
   ,SaleNotes 		= ISNULL(sn.Notes,'')
   ,RealPropNotes 	= rp.Notes 
   ,NewSaleNote 	= ''
   ,s.ExciseTaxNbr
   --,sv.NonRepComp1 Hairo comment: not found
   --,sv.NonRepComp2 Hairo comment: not found                     
   ,SaleWarnings = COALESCE(s.SaleWarnings,'')  --= ISNULL(sw.Warnings,'')
   ,SaleWarning1 = COALESCE((SELECT swid.Warning FROM #SaleWarningId swid WHERE swid.WarningOrder = 1 AND s.SaleGuid = swid.SaleGuid),'')
   ,SaleWarning2 = COALESCE((SELECT swid.Warning FROM #SaleWarningId swid WHERE swid.WarningOrder = 2 AND s.SaleGuid = swid.SaleGuid),'')
   ,SaleWarning3 = COALESCE((SELECT swid.Warning FROM #SaleWarningId swid WHERE swid.WarningOrder = 3 AND s.SaleGuid = swid.SaleGuid),'')
   ,SaleWarning4 = COALESCE((SELECT swid.Warning FROM #SaleWarningId swid WHERE swid.WarningOrder = 4 AND s.SaleGuid = swid.SaleGuid),'')
   ,SaleWarning5 = COALESCE((SELECT swid.Warning FROM #SaleWarningId swid WHERE swid.WarningOrder = 5 AND s.SaleGuid = swid.SaleGuid),'')
   ,SaleWarning6 = COALESCE((SELECT swid.Warning FROM #SaleWarningId swid WHERE swid.WarningOrder = 6 AND s.SaleGuid = swid.SaleGuid),'')
   ,GreaterThan6Warnings = CASE WHEN (SELECT COUNT(1) FROM #SaleWarningId swid WHERE s.SaleGuid = swid.SaleGuid) > 6 THEN 'See RealProperty for additional warnings'  ELSE '' END
   ,ISNULL(s.SellerName,'') AS SellerName
   ,ISNULL(s.BuyerName,'')  AS BuyerName
   --Warning columns    
   ,MultipleSales 
   ,SellerNameWarning
   ,BuyerNameWarning  
   ,SaleWarningToReview 
   ,BuildingWarning 
   ,LandWarning
   ,RatioWarning  
   ,SalesRemoved = @AdditionalSalesRemoved
   --Regression
   ,SaleMonth = RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,s.SaleDate)),2) + '/' + '16/' + RIGHT(CONVERT(VARCHAR(4),DATEPART(YYYY,s.SaleDate)),4) 
   ,SaleMonthNum = DATEDIFF(MONTH,s.SaleDate,'1/1/2020') * -1 
   ,s.SaleDay
   ,TrendedRatio = ISNULL(convert(varchar(10),convert(Decimal(20,2),tr.TrendedRatio)),'')
   ,s.PrevAVSP
   ,TrendAppliedToRatio = tr.FinalTimeAdjustment
   ,tr.PredictedRatio 
   ,SaleUsedInTrendRegression = CASE 
                                  WHEN EXISTS (select * from #SalesUsedInRegress r where r.ExciseTaxNbr = s.ExciseTaxNbr ) THEN 'Yes'
                                  ELSE 'No'
                                END
   ,AvgTrendedRatio = ISNULL(convert(varchar(10),@AvgTrendedRatio),'')
   ,UpperLimit = ISNULL(convert(varchar(10),@UpperLimit),'')
   ,LowerLimit = ISNULL(convert(varchar(10),@LowerLimit),'')    
   ,TrendConclusion = CASE
                        WHEN EXISTS (select * from #StartEndDates where s.SaleDay > StartDay and s.SaleDay <= EndDay and RowNum = 1) 
                        THEN @TrendConclusionPart1Summary
                        WHEN EXISTS (select * from #StartEndDates where s.SaleDay > StartDay and s.SaleDay <= EndDay and RowNum = 2) 
                        THEN @TrendConclusionPart2Summary
                        ELSE ''
                      END
   ,s.PossibleLenderSale
   ,s.PropCnt                           
 
   ,PropClassPerSalesAffid  = COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_salepropertyclass',dps.ptas_salepropertyclass),'')
   ,PropTypePerSalesAffid  = COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_affidavitpropertytype',dps.ptas_affidavitpropertytype),'')
   ,s.SaleTypeRP                                          

    --General data
   ,gmd.TaxPayerName  
   ,gmd.DistrictName
   ,gmd.ApplDistrict 
   --,Team      Hairo comment: it comes from View vw_GisMapData, but it not calculated yet
   
   ,PI_or_AU = CASE
                 WHEN EXISTS (SELECT 1 
								FROM [dynamics].[ptas_inspectionyear] piy
							   INNER JOIN [dynamics].[ptas_area] pa ON  piy._ptas_area_value = pa.ptas_areaid 
							   WHERE piy.ptas_year = @RPAssmtYr
								 --AND gmd.ApplGroup = 'R' 
								 AND piy.ptas_inspectiontype = 1 --Physical inspection
								 AND pa.ptas_areanumber = gmd.ResArea) THEN 'PI'
                 ELSE 'AU' 
               END   
   ,Qtr = rp.QuarterSection  
   ,Sec = rp.Section 
   ,Twn = rp.Township 
   ,Rng = rp.[Range] 
   ,rp.Folio



  ,LandInspectedDate = isnull((select ptas_inspectiondate from [dynamics].[ptas_inspectionhistory] insp where insp._ptas_parcelid_value = gmd.ParcelId and ptas_inspectiontype = 'Land' and SUBSTRING(insp.ptas_name,1,4) = @RPAssmtYr),'') 
  ,ImpsInspectedDate = isnull((select ptas_inspectiondate from [dynamics].[ptas_inspectionhistory] insp where insp._ptas_parcelid_value = gmd.ParcelId and ptas_inspectiontype = 'Imps' and SUBSTRING(insp.ptas_name,1,4) = @RPAssmtYr),'')
  ,BothInspectedDate = isnull((select ptas_inspectiondate from [dynamics].[ptas_inspectionhistory] insp where insp._ptas_parcelid_value = gmd.ParcelId and ptas_inspectiontype = 'Both' and SUBSTRING(insp.ptas_name,1,4) = @RPAssmtYr),'')
  ,Bookmark = ISNULL(REVERSE(SUBSTRING(REVERSE((
												SELECT bt.ptas_name + '; '  
												FROM dynamics.ptas_bookmark	AS bm						
												INNER JOIN dynamics.ptas_ptas_bookmark_ptas_bookmarktag	AS bmt	ON bmt.ptas_bookmarkid = bm.ptas_bookmarkid
												INNER JOIN dynamics.ptas_bookmarktag					AS bt	ON bt.ptas_bookmarktagid = bmt.ptas_bookmarktagid
												where s.ParcelId = bm._ptas_parceldetailid_value
												For XML PATH('')
												)),2,8000)),'')
   ,gmd.PropType
   ,gmd.ApplGroup
   ,gmd.TaxStatus  
   --,VacImpAccy 		Hairo comment: needs to be calculated in GismapData view to add it here
   --,GeneralClassif 	Hairo comment: needs to be calculated in GismapData view to add it here
     
    --Imps
   ,gmd.ResNbrImps		
   ,gmd.YrBltRen 	
   ,gmd.UnfinArea 
   ,gmd.PcntComplRes		
   ,gmd.Obsolescence	
   ,gmd.PcntNetCondition	
   ,gmd.CondDescr		

   --,RP_MH_Count = MHOMEREAL  Hairo comment "MHOMEREAL" needs to be calculated in GismapData view to add it here
   --,PP_MH_Count Hairo comment: needs to be calculated in GismapData view to add it here

   ,gmd.NbrResAccys 
   ,gmd.AccyPRKDETGAR 
   ,gmd.AccyPRKCARPORT 
   ,gmd.AccyPVCONCRETE 
   ,gmd.AccyPOOL
   ,gmd.AccyMISCIMP 
   --,gmd.AccyONSITEDEVCST   --HAIRO Comment YA NO EXISTE, se convirtio en MiscImp
   --,gmd.AccyFLATVALUEDIMP  --HAIRO Comment YA NO EXISTE, se convirtio en MiscImp
   
   --Land char                                                            
   ,gmd.WfntFootage 
   ,WfntBank = gmd.WfntBank_value
   ,gmd.AdjacentGolfFairway 
   ,gmd.AdjacentGreenbelt 
  
   ,CurrentUseExmpt = CASE 
                        WHEN EXISTS (SELECT * FROM dynamics.ptas_parceldetail dpd 
						             WHERE dpd.ptas_parceldetailid = s.ParcelId  AND dpd.ptas_currentuse > 0)  THEN 'Y'
                        ELSE ''
                       END
  
   ,gmd.BaseLandVal
   ,gmd.PcntBaseLandValImpacted
   ,gmd.BaseLandValTaxYr AS LastBLVTaxYr
   ,gmd.BaseLandValDate 
   ,gmd.BLVSqFtCalc
   ,PcntBLVChg = CASE 
                WHEN gmd.BaseLandVal > 0 AND gmd.PrevLandVal > 0 AND gmd.BaseLandValTaxYr = (select @RPAssmtYr + 1 )--(select RPAssmtYr + 1 from AssmtYr) 
                   THEN 100 * (( convert(decimal(20,2),gmd.BaseLandVal)/convert(decimal(20,2),gmd.PrevLandVal)-1) ) 
                   ELSE 0
                END
    --Values
   ,gmd.LandVal
   ,gmd.ImpsVal
   ,gmd.TotVal
   ,gmd.NewConstrVal
   ,s.AVSP  
   
   ,gmd.SelectMethod 
   --,PostingStatusDescr Hairo comment: needs to be calculated in GismapData view to add it here

   ,gmd.PrevLandVal
   ,gmd.PrevImpsVal
   ,gmd.PrevTotVal
   ,gmd.PrevNewConstrVal
   ,gmd.PrevSelectMethod

   ,gmd.PcntChgLand
   ,gmd.PcntChgImps
   ,gmd.PcntChgTotal
  
   --,PermitTypesIncompl			Hairo comment: needs to be calculated in GismapData view to add it here
   --,PermitTypesComplCurCycle      Hairo comment: needs to be calculated in GismapData view to add it here
   --,PermitTypesComplPrevCycle     Hairo comment: needs to be calculated in GismapData view to add it here
   ,s.SaleId
     
   ,rp.Pin
   ,rp.MapPin
   ,s.RecId
      
FROM #GisMapDataSubSet gmd
INNER JOIN #RealProp rp ON gmd.ParcelId = rp.ParcelId
INNER JOIN #SaleId s ON gmd.ParcelId = s.ParcelId
INNER JOIN #CompletedWork cw ON gmd.ParcelId = cw.ParcelId
INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps ON spdps.ptas_parceldetailid = gmd.ParcelId AND spdps.ptas_salesid = s.Saleguid 
INNER JOIN [dynamics].[ptas_sales] dps ON dps.ptas_salesid = spdps.ptas_salesid
LEFT JOIN #SaleWarningId swid ON s.SaleGuid = swid.SaleGuid 
LEFT JOIN #SaleNote sn ON s.SaleGuid = sn.SaleGuid 
LEFT JOIN #AllRatiosTrended tr ON s.ExciseTaxNbr  = tr.ExciseTaxNbr
ORDER BY rp.PIN




RETURN(0)

ErrorHandler:
RETURN (@Error)

RETURN(@@ERROR)
END


