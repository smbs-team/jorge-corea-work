
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_GetAreaData01')
	DROP PROCEDURE [cus].[SRCH_GetAreaData01]  
GO

CREATE PROCEDURE [cus].[SRCH_GetAreaData01]
  @Area		   smallint	= NULL
 ,@SubArea	   smallint	= NULL
 ,@AssmtYr	   smallint	= NULL
 ,@SalesFrom   smalldatetime	= NULL
 ,@SalesTo	   smalldatetime	= NULL
 ,@SalePrice   int		= NULL
 ,@Population  char(3)         = 'N'
 ,@Notes       char(3)         = 'N'
		
AS 
BEGIN
/*
Author: Jairo Barquero
Date Created: 12/04/2020
Description:    SP that Inserts data into #SalesPop, #RealProp, #Sales, #Land, #XLand, #TaxAcctReceivable

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

/*
--BEGIN This is ONLY FOR TEST Purposes
--BEGIN This is ONLY FOR TEST Purposes
--ONLY FOR DEBUG PURPOSES
  declare 
  @Area		   smallint	= '80'
 ,@SubArea	   smallint	= NULL--'3'
 ,@AssmtYr	   smallint	= 2020
 ,@SalesFrom   smalldatetime	= '01/01/2018'
 ,@SalesTo	   smalldatetime	= '03/20/2020'
 ,@SalePrice   int		= 200000
 ,@Population  char(3)         = 'Y'
 ,@Notes       char(3)         = 'Y'

--END This is ONLY FOR TEST Purposes
--END This is ONLY FOR TEST Purposes
*/

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


DECLARE  @ErrMsg		varchar(200)
		,@ApplGroup 	varchar(1)
		,@RequestId 	int
       	,@ActiveCnt 	smallint
		,@leadzero 		tinyint 
        ,@chSubArea 	varchar(3)
        ,@BillYr		smallint
        ,@RPAssmtYr		smallint
        ,@AcctAssmtYr   smallint
        ,@CostBillYr   	smallint
		,@yrMultiplier 	decimal(4,3)
       	,@AreaMultiplier decimal(4,3) 
       	,@TestProd 		char(1)
       	,@FixedCost 	decimal(9,4)
       	,@VariableCost 	decimal(9,4)
       	,@CostIndex 	decimal(6,4)
		,@WarnCntr 		tinyint
		,@STDGRADE		tinyint
    	,@Countdown 	int
		,@NICntr 		tinyint
		
SET  	 @ErrMsg 	= ''
SET		 @ApplGroup = 'R'
/*
Hairo comment: this needs to be replaced with the corresponding query to get the correct year information
SELECT  @RPAssmtYr 	 = RPAssmtYr
       ,@AcctAssmtyr = AcctAssmtYr
  FROM AssmtYr (NOLOCK)
*/
SELECT  @RPAssmtYr 	 = 2020
       ,@AcctAssmtyr = 2020
	   
IF @AssmtYr = @RPAssmtYr -- we need to check to see which Billyr we need to search the taxacctreceivable for
	BEGIN
		IF @RPAssmtYr = @AcctAssmtYr -- the role has been certified on the mainframe and we should have the following billyr on taxacctreceivable
			SET @BillYr = @AssmtYr + 1
		ELSE -- we don't have the next billyr on taxacctreceivable so we will pull this years; only occurs when we close the roll but have not certified the roll on the mainframe
			SET @BillYr = @Assmtyr
	END
ELSE 
	BEGIN
	IF @AssmtYr < @RPAssmtYr
		SET @BillYr = @AssmtYr + 1
	ELSE 
		BEGIN
			SET @ErrMsg =  'Invalid AssmtYr'
			GOTO SQLError
		END
	END
SET @CostBillYr = @AssmtYr + 1

--Create ALL tables

IF 1=1
BEGIN
CREATE TABLE #SalesPop(
  rowID     	int        	not null  identity(1,1)
 ,PIN           char(10)  	DEFAULT ''
 ,MapPin        float     	DEFAULT 0 
 ,SaleGuid 		uniqueidentifier
 ,SaleId		nvarchar(50)DEFAULT 0 
 ,ParcelId 		uniqueidentifier
 ,LandGuid 		uniqueidentifier
 ,ExciseTaxNbr	int 		DEFAULT 0
 ,SaleDate		smalldatetime 	NULL
 ,SalePrice		int  		DEFAULT 0
 ,VerifiedAtMkt	char(1) 	DEFAULT ''
 ,VerifiedBy	char(4) 	DEFAULT ''
 ,SellerName    varchar(300)	DEFAULT ''
 ,BuyerName     varchar(300) 	DEFAULT ''
 ,PropCnt       SmallInt 	DEFAULT 0
-- RealProp
 ,ResArea		char(3)		DEFAULT ''
 ,ResSubarea	char(3)		DEFAULT ''
 ,QSec			char(2)		DEFAULT ''
 ,Sec			tinyint		DEFAULT 0
 ,Twn			tinyint		DEFAULT 0
 ,Rng			tinyint		DEFAULT 0
 ,Neighborhood	smallint	DEFAULT 0
 ,GisSurfaceValue  float    DEFAULT 0 --From GisSurface_V
 ,Folio			char(7)		DEFAULT ''
 ,PlatLot		char(14)	DEFAULT ''
 ,PlatBlock		char(7)		DEFAULT ''
 ,Major			char(6)		DEFAULT '' 
 ,Minor			char(4)		DEFAULT '' 
 ,PropType 		char(1) DEFAULT ''
 ,ZoneClass		varchar(50)	DEFAULT ''  
 ,CurrentZoning	varchar(50)	DEFAULT ''
 ,ZoneDesignation 			varchar(50)	DEFAULT ''                    
 ,ZoningChgDate				smalldatetime	NULL                  
-- land		
 --,LandId		int 		DEFAULT 0
 ,HBUAsIfVacant	tinyint		DEFAULT 0               
 ,HBUAsImproved	tinyint		DEFAULT 0            
 ,PresentUse	smallint	DEFAULT 0
 ,SqFtLot		int			DEFAULT 0
 ,SqFtLotDry 	int    		DEFAULT 0
 ,SqFtLotSubmerged int      DEFAULT 0
 ,WaterSystem	tinyint		DEFAULT 0
 ,SewerSystem	tinyint		DEFAULT 0
 ,Access		tinyint		DEFAULT 0
 ,Topography	tinyint		DEFAULT 0
 ,StreetSurface	tinyint		DEFAULT 0
 ,PcntBaseLandValImpacted	int				DEFAULT 0
 ,RestrictiveSzShape 		tinyint			DEFAULT 0
 ,BaseLandVal				int				DEFAULT 0
 ,BaseLandValTaxYr 			smallint		DEFAULT 0
 ,BaseLandValDate 			smalldatetime	NULL
 ,Unbuildable 		smallint  		DEFAULT 0
 ,MtRainier			int  	DEFAULT 0
 ,Olympics			int  	DEFAULT 0
 ,Cascades			int  	DEFAULT 0
 ,Territorial		int  	DEFAULT 0
 ,SeattleSkyline	int  	DEFAULT 0
 ,PugetSound		int  	DEFAULT 0
 ,LakeWashington	int  	DEFAULT 0
 ,LakeSammamish		int  	DEFAULT 0
 ,SmallLakeRiverCreek		int  	 DEFAULT 0
 ,OtherView			int  	DEFAULT 0
 ,WfntLocation		int  	DEFAULT 0
 ,WfntFootage		int  	DEFAULT 0
 ,WfntPoorQuality	int  	DEFAULT 0
 ,WfntBank			int  	DEFAULT 0
 ,TidelandShoreland	int  	DEFAULT 0
 ,WfntRestrictedAccess		int  	 DEFAULT 0
 ,LotDepthFactor	int  	DEFAULT 0
 ,TrafficNoise		int  	DEFAULT 0
 ,AirportNoise		int  	DEFAULT 0
 ,CommonProperty	int  	DEFAULT 0
 ,CurrentUseDesignation		int  	 DEFAULT 0
 ,NbrBldgSites		int  	DEFAULT 0
 ,Contamination		int  	DEFAULT 0
 ,WfntAccessRights 	tinyint	DEFAULT 0
 ,WfntProximityInfluence 	tinyint  DEFAULT 0
 ,NativeGrowthProtEsmt 		tinyint  DEFAULT 0
 ,PowerLines 				tinyint  DEFAULT 0
 ,OtherNuisances 			tinyint  DEFAULT 0
 ,AdjacentGolfFairway 		tinyint  DEFAULT 0
 ,AdjacentGreenbelt 		tinyint  DEFAULT 0
 ,OtherDesignation 			tinyint  DEFAULT 0
 ,DeedRestrictions 			tinyint  DEFAULT 0
 ,DevelopmentRightsPurch 	tinyint  DEFAULT 0
 ,Easements 				tinyint  DEFAULT 0
 ,SpecialAssessments 		tinyint  DEFAULT 0
 ,CoalMineHazard 			tinyint  DEFAULT 0
 ,CriticalDrainage 			tinyint  DEFAULT 0
 ,ErosionHazard 			tinyint  DEFAULT 0
 ,LandfillBuffer 			tinyint  DEFAULT 0
 ,HundredYrFloodPlain 		tinyint  DEFAULT 0
 ,SeismicHazard 			tinyint  DEFAULT 0
 ,LandslideHazard 			tinyint  DEFAULT 0
 ,SteepSlopeHazard 			tinyint  DEFAULT 0
 ,Stream 					tinyint  DEFAULT 0
 ,Wetland 					tinyint  DEFAULT 0
 ,SpeciesOfConcern 			tinyint  DEFAULT 0
 ,SensitiveAreaTract 		tinyint  DEFAULT 0
 ,WaterProblems 			tinyint  DEFAULT 0
 ,OtherProblems 			tinyint  DEFAULT 0
 ,HistoricSite 				tinyint  DEFAULT 0
-- SaleCodes
 ,SalePropType				tinyint  DEFAULT 0
 ,PrinUse					tinyint  DEFAULT 0
 ,PropClass					tinyint  DEFAULT 0
 ,Reason					tinyint  DEFAULT 0
 ,Instr						tinyint  DEFAULT 0
 ,Verif						tinyint  DEFAULT 0
-- TaxAcctReceivable
 ,TaxPayerName	    		varchar(80)	 DEFAULT ''
 ,ApprLandVal				int	 DEFAULT 0
 ,ApprImpsVal				int	 DEFAULT 0
 ,SrCitFlag					char(1)  DEFAULT ''
 ,TaxStat					char(3)	 DEFAULT ''
-- resbldg
 ,BldgGuid					uniqueidentifier
 ,BldgNbr					tinyint	DEFAULT 0
 ,ImpCnt					int  	DEFAULT 0
 ,StreetNbr             	varchar(5)      DEFAULT ''
 ,NbrFraction           	varchar(3)      DEFAULT ''
 ,DirPrefix             	char(2)         DEFAULT ''
 ,StreetName            	varchar(25)     DEFAULT ''
 ,StreetType            	char(4)         DEFAULT ''
 ,DirSuffix             	char(2)         DEFAULT ''     
 ,DistrictName 				varchar(80)   
 ,NbrLivingUnits			tinyint			DEFAULT 0
 ,Stories					real			DEFAULT 0
 ,BldgGrade					tinyint			DEFAULT 0
 ,BldgGradeVar				tinyint		DEFAULT 0
 ,SqFt1stFloor				int		DEFAULT 0
 ,SqFtHalfFloor				int		DEFAULT 0
 ,SqFt2ndFloor				int		DEFAULT 0
 ,SqFtUpperFloor			int		DEFAULT 0
 ,SqFtUnfinFull				int		DEFAULT 0
 ,SqFtUnfinHalf				int		DEFAULT 0
 ,SqFtTotLiving				int		DEFAULT 0
 ,SqFtAboveGrLiving			int		DEFAULT 0
 ,SqFtTotBasement			int		DEFAULT 0
 ,SqFtFinBasement			int		DEFAULT 0
 ,FinBasementGrade			tinyint		DEFAULT 0
 ,SqFtGarageBasement		int		DEFAULT 0
 ,SqFtGarageAttached		int		DEFAULT 0
 ,DaylightBasement			char(1)		DEFAULT ''              
 ,Bedrooms					tinyint		DEFAULT 0
 ,BathHalfCount				tinyint		DEFAULT 0
 ,Bath3qtrCount				tinyint		DEFAULT 0
 ,BathFullCount				tinyint		DEFAULT 0
 ,BathTotal					real		DEFAULT 0
 ,FpSingleStory				tinyint		DEFAULT 0
 ,FpMultiStory				tinyint		DEFAULT 0
 ,FpFreestanding			tinyint		DEFAULT 0
 ,FpAdditional				tinyint		DEFAULT 0
 ,FpTotal					tinyint		DEFAULT 0
 ,YrBuilt					smallint	DEFAULT 0
 ,YrRenovated				smallint	DEFAULT 0
 ,PcntComplete				tinyint		DEFAULT 0
 ,Obsolescence				tinyint		DEFAULT 0
 ,PcntNetCondition			tinyint		DEFAULT 0
 ,Condition					tinyint		DEFAULT 0
 ,ViewUtilization			char(1)		DEFAULT ''                 
 ,SqFtOpenPorch				int			DEFAULT 0
 ,SqFtEnclosedPorch			int			DEFAULT 0
 ,SqFtDeck					int			DEFAULT 0
 ,HeatSystem				tinyint		DEFAULT 0
 ,HeatSource				tinyint		DEFAULT 0
 ,BrickStone				tinyint		DEFAULT 0
 ,AddnlCost					int			DEFAULT 0
-- accessory		
 ,SqFtCoveredParking		int		 	DEFAULT 0
 ,DetGarArea				int		 	DEFAULT 0
 ,DetGarGrade				tinyint		DEFAULT 0
 ,DetGarEffYr				smallint	DEFAULT 0
 ,DetGarNetCond				tinyint		DEFAULT 0
 ,CarportArea				int		 	DEFAULT 0
 ,CarportEffYr				smallint	DEFAULT 0
 ,CarportNetCond			tinyint     DEFAULT 0
 ,PoolArea					int		 	DEFAULT 0
 ,PoolEffYr					smallint	DEFAULT 0
 ,PoolNetCond				tinyint		DEFAULT 0
 ,Paving					int 	    DEFAULT 0
 ,MiscAccyCost				int			DEFAULT 0
 ,MHomeValue				int			DEFAULT 0
 ,MHomeType					tinyint		DEFAULT 0
 ,DevCost					int	        DEFAULT 0
 ,FlatValue					int		 	DEFAULT 0
 ,PermitNbr					varchar(30) DEFAULT ''
 ,PermitDate				smalldatetime 	 NULL
 ,PermitValue				int		 	DEFAULT 0
 ,PermitType				tinyint		DEFAULT 0
 ,PermitDescr				varchar(1024)
 ,HILastYr					smallint	 DEFAULT 0
 ,HIValue					int	         DEFAULT 0 
-- RealPropApplHist
 ,SelectDate				smalldatetime 	 NULL                     
 ,SelectAppr				char(4)		DEFAULT ''                     
 ,SelectMethod				varchar(50)	DEFAULT ''                 
 ,SelectReason				varchar(50)	DEFAULT ''                  
 ,SelectLandVal				int         DEFAULT 0
 ,SelectImpsVal				int		 	DEFAULT 0
 ,RollLandVal				int		 	DEFAULT 0
 ,RollImpsVal				int		 	DEFAULT 0
 ,RollValTotal				int		 	DEFAULT 0
 ,SegMergeDate				smalldatetime 	 NULL
 ,TaxAcctCnt 				tinyint		DEFAULT 0
 ,LastWarnId            	tinyint     DEFAULT 0
 ,Warnings              	nvarchar(max)     DEFAULT ''  
 ,MFInterfaceFlag       	char(2) DEFAULT ''
 ,NewConstrVal				int		DEFAULT 0	
 ,PossibleLenderSale   		char(1)  DEFAULT ''
-- RCN
 ,UnitsAdj 					decimal(3,2) 	 Default 0
 ,EffYr 					smallint 	 Default 0
 ,Age 						smallint 	 Default 0
 ,BldgGradeCateg 			tinyint 	 Default 0
 ,BldgFormula 				tinyint 	 Default 0
 ,BldgFactor1 				decimal(9,6)  Default 0
 ,BldgFactor2 				decimal(9,6)  Default 0
 ,BsmtGradeCateg 			tinyint 	 Default 0
 ,BsmtFormula 				tinyint 	 Default 0
 ,BsmtFactor1 				decimal(9,6) Default 0
 ,BsmtFactor2 				decimal(9,6) Default 0
 ,PcntGoodBldg 				decimal(9,6) Default 0
 ,PcntGoodBsmt 				decimal(9,6) Default 0
 ,BldgGradeAdj 				decimal(3,2) Default 0
 ,finBsmtCost 				int 		 Default 0
 ,sBldgRCN 					int 		 Default 0
 ,sBldgRCNLD 				int 		 Default 0
 ,sAccyRCN 					int 		 Default 0
 ,sAccyRCNLD 				int 		 Default 0
 ,BldgRCN 					int 		 Default 0
 ,BldgRCNLD 				int 		 Default 0
 ,AccyRCN 					int 		 Default 0
 ,AccyRCNLD 				int 		 Default 0
 ,MHRCN         			int          Default 0
 ,MHRCNLD       			int          Default 0

 ,LastNIId            		tinyint		 DEFAULT 255
 ,Notes 					varchar(5000) 	 DEFAULT ''
 ,HoldoutReason   			varchar(50)
 --Mobile Home Data   --ppmh_char
 ,MHCount 					int   	DEFAULT 0
 ,MHBldgNbr 				tinyint DEFAULT 0
 ,MHType  					tinyint DEFAULT 0
 ,MHClass 					tinyint DEFAULT 0
 ,MHLength 					int    	DEFAULT 0
 ,MHWidth  					int    	DEFAULT 0
 ,MHLivingArea 				int  	DEFAULT 0
 ,MHTipOutArea 				int  	DEFAULT 0
 ,MHRoomAddSqft 			int  	DEFAULT 0
 ,MHSize 					int    	DEFAULT 0
 ,MHYrBuilt 				int  	DEFAULT 0
 ,MHCondition 				tinyint DEFAULT 0
 ,MHPcntNetCondition   		int 	DEFAULT 0 
 ,TrendFactor 				decimal(14,8) 	 DEFAULT 0  
 ,TrendedPrice 				bigint 			 DEFAULT 0  
 ,MHGuid 					uniqueidentifier DEFAULT NULL
 ,MobileHomeId 				int 			 DEFAULT 0  
 --NEW COLUMNS REQUESTED BY Regis
 ,AirportValPct				int 	DEFAULT 0
 ,OtherNuisValPct           int 	DEFAULT 0
 ,OtherProblemsValPct     	int 	DEFAULT 0
 ,PowerLinesValPct        	int 	DEFAULT 0
 ,RoadAccessValPct        	int 	DEFAULT 0
 ,TopoValPct				int 	DEFAULT 0 
 ,WaterProblemsValPct     	int 	DEFAULT 0
 ,TrafficValPct           	int 	DEFAULT 0
 ,AirportValDollars       	Money 	DEFAULT 0
 ,TrafficValDollars       	Money 	DEFAULT 0
 ,HundredYrValPct	     	int 	DEFAULT 0
 ,CoalValPct		        int 	DEFAULT 0 
 ,ContamValPct	         	int 	DEFAULT 0
 ,DrainageValPct 	     	int 	DEFAULT 0
 ,ErosionValPct	         	int 	DEFAULT 0
 ,LandfillValPct 	     	int 	DEFAULT 0
 ,LandslideValPct	     	int 	DEFAULT 0
 ,SeismicValPct	         	int 	DEFAULT 0
 ,SensitiveValPct	     	int 	DEFAULT 0
 ,SpeciesValPct	         	int 	DEFAULT 0
 ,SteepSlopeValPct        	int 	DEFAULT 0
 ,StreamValPct	         	int 	DEFAULT 0
 ,WetlandValPct	         	int 	DEFAULT 0
 ,DevRightsValPct         	int 	DEFAULT 0
 ,AdjacGolfValPct         	int 	DEFAULT 0
 ,AdjacGreenbeltValPct    	int 	DEFAULT 0
 ,DeedRestrictValPct      	int 	DEFAULT 0
 ,EasementsValPct         	int 	DEFAULT 0
 ,DNRLeaseValPct          	int 	DEFAULT 0
 ,NativeGrowthValPct      	int 	DEFAULT 0
 ,OtherDesigValPct        	int 	DEFAULT 0
 ,AdjacGolfDollars        	Money 	DEFAULT 0
 ,AdjacGreenbeltValDollars	Money 	DEFAULT 0
 ,AssignedBoth 		 varchar(100)   DEFAULT ''
 ,PRIMARY KEY(rowID)
 ,UNIQUE(ParcelId, rowID)
 ,UNIQUE(CurrentZoning, rowID)
 ,UNIQUE(ParcelId, BldgGuid, rowID)
 ,UNIQUE(LandGuid, rowID)
 ,UNIQUE(BldgGrade, rowID)
-- ,UNIQUE(NoteId, rowID)
 ,UNIQUE(SaleGuid, rowID)
 ,UNIQUE(Condition, BldgGradeCateg, Age, rowID)
 ,UNIQUE(FinBasementGrade, rowID)
)
Create index Idx_ParcelId On #SalesPop
(
ParcelId
)
Create index Idx_LandGuid On #SalesPop
(
LandGuid
)
Create index Idx_SaleGuid On #SalesPop
(
SaleGuid
)


CREATE TABLE #RealProp (
    rowID  		int	not null  identity(1,1)
   ,ParcelId	uniqueidentifier
   ,LandGuid 	uniqueidentifier
   ,PropType 	char(1)
   ,Minor 		char(4)
   ,PRIMARY KEY(ParcelId)
   ,UNIQUE(ParcelId, LandGuid)
   )

CREATE TABLE #Sales(
  rowID  	int	not null  identity(1,1)
 ,SaleGuid	uniqueidentifier
 ,PRIMARY KEY(rowID)
 ,UNIQUE(SaleGuid, rowID)
)
  
CREATE TABLE #Land (
  rowID  			int	not null  identity(1,1)
 ,LndGuid 			uniqueidentifier
)   

CREATE TABLE #XLand(
  rowID  	    	int	not null  identity(1,1)
 ,LandGuid			uniqueidentifier 
 ,MtRainier			int NULL
 ,Olympics			int NULL
 ,Cascades			int NULL
 ,Territorial		int NULL
 ,SeattleSkyline	int NULL
 ,PugetSound		int NULL
 ,LakeWashington	int NULL
 ,LakeSammamish		int NULL
 ,SmallLakeRiverCreek	int NULL
 ,OtherView			int NULL
 ,WfntLocation		int NULL
 ,WfntFootage		int NULL
 ,WfntPoorQuality	int NULL
 ,WfntBank			int NULL
 ,TidelandShoreland	int NULL
 ,WfntRestrictedAccess	int NULL
 ,LotDepthFactor	int NULL
 ,TrafficNoise		int NULL
 ,AirportNoise		int NULL
 ,CommonProperty	int NULL
 ,CurrentUseDesignation	int NULL
 ,NbrBldgSites		int NULL
 ,Contamination		int NULL
 ,WfntAccessRights 	tinyint NULL
 ,WfntProximityInfluence tinyint NULL
 ,NativeGrowthProtEsmt tinyint NULL
 ,PowerLines 		tinyint NULL
 ,OtherNuisances 	tinyint NULL
 ,AdjacentGolfFairway tinyint NULL
 ,AdjacentGreenbelt tinyint NULL
 ,OtherDesignation 	tinyint NULL
 ,DeedRestrictions 	tinyint NULL
 ,DevelopmentRightsPurch tinyint NULL
 ,Easements 		tinyint NULL
 ,SpecialAssessments tinyint NULL
 ,CoalMineHazard 	tinyint NULL
 ,CriticalDrainage 	tinyint NULL
 ,ErosionHazard 	tinyint NULL
 ,LandfillBuffer 	tinyint NULL
 ,HundredYrFloodPlain tinyint NULL
 ,SeismicHazard 	tinyint NULL
 ,LandslideHazard 	tinyint NULL
 ,SteepSlopeHazard 	tinyint NULL
 ,Stream 			tinyint NULL
 ,Wetland 			tinyint NULL
 ,SpeciesOfConcern 	tinyint NULL
 ,SensitiveAreaTract tinyint NULL
 ,WaterProblems 	tinyint NULL
 ,OtherProblems 	tinyint NULL
 ,HistoricSite 		tinyint NULL
 ,Topography 		tinyint
--***BEGIN ADDING COLUMNS FROM #Land
 ,BaseLandVal 		int
 ,BaseLandValTaxYr 	smallint
 ,BaseLandValDate 	smalldatetime
 ,SqFtLot 			int
 ,CurrentZoning 	nvarchar(50)
 ,ZoningChgDate 	smalldatetime
 ,HBUAsIfVacant 	tinyint
 ,HBUAsImproved 	tinyint
 ,PresentUse 		smallint
 ,WaterSystem 		tinyint
 ,SewerSystem 		tinyint
 ,Access 			tinyint
 ,StreetSurface 	tinyint
 ,PcntBaseLandValImpacted int
 ,Unbuildable 		tinyint
 ,RestrictiveSzShape tinyint
--***END ADDING COLUMNS FROM #Land
 ,PRIMARY KEY(LandGuid)
 ,UNIQUE(LandGuid, rowID)
)

--New tables

CREATE TABLE #GASCodes(	
  rowID  	    int	not null  identity(1,1)
 ,SaleGuid		uniqueidentifier 
 ,SalePropType	smallint NULL
 ,PrinUse		smallint NULL
 ,PropClass		smallint NULL
 ,Reason		smallint NULL
 ,Instr			smallint NULL
 ,Verif			smallint NULL
 ,PRIMARY KEY(rowID)
 ,UNIQUE(SaleGuid, rowID)
)


CREATE TABLE #TaxAcctReceivable( 
  rowID  	    int	not null  identity(1,1)
 ,AcctNbr 		char(12)
 ,ParcelId		uniqueidentifier 
 ,TaxPayerName	varchar(80)	NULL
 ,SrCitFlag     char(1)         NULL
 ,ApprLandVal	int		NULL
 ,ApprImpsVal	int		NULL
 ,TaxStat		char(1)	NULL
 ,PRIMARY KEY(rowID)
 ,UNIQUE(ParcelId, rowID)
 ,UNIQUE(AcctNbr, rowID)
)


CREATE TABLE #Accessory(
  rowID  int 	not null  identity(1,1)
 ,ParcelId uniqueidentifier
 ,AccyType smallint
 ,Size int
 ,Grade smallint
 ,EffYr smallint
 ,PcntNetCondition tinyint
 ,AccyValue int
 ,BldgGuid uniqueidentifier
 ,Component varchar(50)
 ,Age int
 ,Factor decimal(6,5)
 ,RCN decimal(15,6)
 ,RCNLD decimal(15,6)
 ,PRIMARY KEY(rowID)
 ,UNIQUE(ParcelId, rowID)
 ,UNIQUE(BldgGuid, rowID)
 ,UNIQUE(ParcelId, BldgGuid, AccyType, rowID)
)

CREATE TABLE #GotRCN(
  rowID  int 	not null  identity(1,1)
 ,LandId int
 ,BldgId int
 ,PRIMARY KEY(rowID)
 ,UNIQUE(LandId, BldgId, rowID)
)

CREATE TABLE #SaleNote (
    rowID  	    int	not null  identity(1,1)
   ,SaleGuid	    uniqueidentifier
   ,ExciseTaxNbr    int
   ,AssmtEntityId char(4)
   ,UpdateDate smalldatetime
   ,Note varchar(max)
 --  ,NoteInstance int
   ,PRIMARY KEY(rowID)
   ,UNIQUE(SaleGuid, rowID)
  )


CREATE TABLE #IdentAccy (
  rowID  	int	not null  identity(1,1)
 ,ParcelId uniqueidentifier
 ,BldgGuid uniqueidentifier
 ,PRIMARY KEY(rowID)
 ,UNIQUE(ParcelId, BldgGuid, rowID)
)

CREATE TABLE #TotAccy(
  rowID  	int	not null  identity(1,1)
 ,ParcelId uniqueidentifier
 ,BldgGuid uniqueidentifier
 ,AccyType int
 ,Size int
 ,Grade tinyint
 ,EffYr smallint
 ,PcntNetCondition smallint
 ,AccyValue int
 ,PRIMARY KEY(rowID)
 ,UNIQUE(ParcelId, BldgGuid, AccyType, rowID)
)

CREATE TABLE #Events (
  rowID  	int	not null  identity(1,1)
 ,ParcelId uniqueidentifier
 ,EventDate smalldatetime
 ,PRIMARY KEY(rowID)
 ,UNIQUE(ParcelId, rowID)
)

CREATE TABLE #ResCostBldgComponent (
  rowID  	int	not null  identity(1,1)
 ,Bath3qtrCountFixedCost decimal(15,8) 
 ,Bath3qtrCountVariableCost decimal(15,8)
 ,Bath3qtrCountMinArea smallint
 ,BathFullCountFixedCost decimal(15,8)
 ,BathFullCountVariableCost decimal(15,8)
 ,BathFullCountMinArea smallint
 ,BathHalfCountFixedCost decimal(15,8)
 ,BathHalfCountVariableCost decimal(15,8)
 ,BathHalfCountMinArea smallint
 ,BathTotalAdjustFixedCost decimal(15,8)
 ,BathTotalAdjustVariableCost decimal(15,8)
 ,BathTotalAdjustMinArea smallint
 ,EffBrickStoneAreaFixedCost decimal(15,8)
 ,EffBrickStoneAreaVariableCost decimal(15,8)
 ,EffBrickStoneAreaMinArea smallint
 ,FpAdditionalFixedCost decimal(15,8)
 ,FpAdditionalVariableCost decimal(15,8)
 ,FpAdditionalMinArea smallint
 ,FpFreestandingFixedCost decimal(15,8)
 ,FpFreestandingVariableCost decimal(15,8)
 ,FpFreestandingMinArea smallint
 ,FpMultiStoryFixedCost decimal(15,8)
 ,FpMultiStoryVariableCost decimal(15,8)
 ,FpMultiStoryMinArea smallint
 ,FpSingleStoryFixedCost decimal(15,8)
 ,FpSingleStoryVariableCost decimal(15,8)
 ,FpSingleStoryMinArea smallint
 ,SqFt1stFloorFixedCost decimal(15,8)
 ,SqFt1stFloorVariableCost decimal(15,8)
 ,SqFt1stFloorMinArea smallint
 ,SqFt2ndFloorFixedCost decimal(15,8)
 ,SqFt2ndFloorVariableCost decimal(15,8)
 ,SqFt2ndFloorMinArea smallint
 ,SqFtUpperFloorFixedCost decimal(15,8)
 ,SqFtUpperFloorVariableCost decimal(15,8)
 ,SqFtUpperFloorMinArea smallint
 ,SqFtDeckFixedCost decimal(15,8)
 ,SqFtDeckVariableCost decimal(15,8)
 ,SqFtDeckMinArea smallint
 ,SqFtElectricBBHeatedFixedCost decimal(15,8)
 ,SqFtElectricBBHeatedVariableCost decimal(15,8)
 ,SqFtElectricBBHeatedMinArea smallint
 ,SqFtEnclosedPorchFixedCost decimal(15,8)
 ,SqFtEnclosedPorchVariableCost decimal(15,8)
 ,SqFtEnclosedPorchMinArea smallint
 ,SqFtFinBasementFixedCost decimal(15,8)
 ,SqFtFinBasementVariableCost decimal(15,8)
 ,SqFtFinBasementMinArea smallint
 ,SqFtForcedAirHeatedFixedCost decimal(15,8)
 ,SqFtForcedAirHeatedVariableCost decimal(15,8)
 ,SqFtForcedAirHeatedMinArea smallint
 ,SqFtFWFurnaceHeatedFixedCost decimal(15,8)
 ,SqFtFWFurnaceHeatedVariableCost decimal(15,8)
 ,SqFtFWFurnaceHeatedMinArea smallint
 ,SqFtGarageAttachedFixedCost decimal(15,8)
 ,SqFtGarageAttachedVariableCost decimal(15,8)
 ,SqFtGarageAttachedMinArea smallint
 ,SqFtGravityHeatedFixedCost decimal(15,8)
 ,SqFtGravityHeatedVariableCost decimal(15,8)
 ,SqFtGravityHeatedMinArea smallint
 ,SqFtHalfFloorFixedCost decimal(15,8)
 ,SqFtHalfFloorVariableCost decimal(15,8)
 ,SqFtHalfFloorMinArea smallint
 ,SqFtHeatPumpHeatedFixedCost decimal(15,8)
 ,SqFtHeatPumpHeatedVariableCost decimal(15,8)
 ,SqFtHeatPumpHeatedMinArea smallint
 ,SqFtHotWaterHeatedFixedCost decimal(15,8)
 ,SqFtHotWaterHeatedVariableCost decimal(15,8)
 ,SqFtHotWaterHeatedMinArea smallint
 ,SqFtOpenPorchFixedCost decimal(15,8)
 ,SqFtOpenPorchVariableCost decimal(15,8)
 ,SqFtOpenPorchMinArea smallint
 ,SqFtRadiantHeatedFixedCost decimal(15,8)
 ,SqFtRadiantHeatedVariableCost decimal(15,8)
 ,SqFtRadiantHeatedMinArea smallint
 ,SqFtTotBasementFixedCost decimal(15,8)
 ,SqFtTotBasementVariableCost decimal(15,8)
 ,SqFtTotBasementMinArea smallint
 ,SqFtUnfinFullFixedCost decimal(15,8)
 ,SqFtUnfinFullVariableCost decimal(15,8)
 ,SqFtUnfinFullMinArea smallint
 ,SqFtUnfinHalfFixedCost decimal(15,8)
 ,SqFtUnfinHalfVariableCost decimal(15,8)
 ,SqFtUnfinHalfMinArea smallint
 ,SqFtUnheatedLivingFixedCost decimal(15,8)
 ,SqFtUnheatedLivingVariableCost decimal(15,8)
 ,SqFtUnheatedLivingMinArea smallint
 ,PRIMARY KEY(rowID)
)

CREATE TABLE #ResCostBldgGradeFactor (
  rowID  	int	not null  identity(1,1)
 ,Grade tinyint
 ,Bath3qtrCount decimal(15,8)
 ,BathFullCount decimal(15,8)
 ,BathHalfCount decimal(15,8)
 ,BathTotalAdjust decimal(15,8)
 ,EffBrickStoneArea decimal(15,8)
 ,FpAdditional decimal(15,8)
 ,FpFreestanding decimal(15,8)
 ,FpMultiStory decimal(15,8)
 ,FpSingleStory decimal(15,8)
 ,SqFt1stFloor decimal(15,8)
 ,SqFt2ndFloor decimal(15,8)
 ,SqFtUpperFloor decimal(15,8)
 ,SqFtDeck decimal(15,8)
 ,SqFtElectricBBHeated decimal(15,8)
 ,SqFtEnclosedPorch decimal(15,8)
 ,SqFtFinBasement decimal(15,8)
 ,SqFtForcedAirHeated decimal(15,8)
 ,SqFtFWFurnaceHeated decimal(15,8)
 ,SqFtGarageAttached decimal(15,8)
 ,SqFtGravityHeated decimal(15,8)
 ,SqFtHalfFloor decimal(15,8)
 ,SqFtHeatPumpHeated decimal(15,8)
 ,SqFtHotWaterHeated decimal(15,8)
 ,SqFtOpenPorch decimal(15,8)
 ,SqFtRadiantHeated decimal(15,8)
 ,SqFtTotBasement decimal(15,8)
 ,SqFtUnfinFull decimal(15,8)
 ,SqFtUnfinHalf decimal(15,8)
 ,SqFtUnheatedLiving decimal(15,8)
 ,PRIMARY KEY(rowID)
 ,UNIQUE(Grade, rowID)
)

CREATE TABLE #ResCostAccyComponent (
  rowID  	int	not null  identity(1,1)
 ,ConcretePavingFixedCost  decimal(15,8)
 ,ConcretePavingVariableCost    decimal(15,8)
 ,ConcretePavingMinArea  decimal(15,4)
 ,AsphaltPavingFixedCost    decimal(15,8)
 ,AsphaltPavingVariableCost    decimal(15,8)
 ,AsphaltPavingMinArea  decimal(15,4)
 ,SqFtCarportFixedCost    decimal(15,8)
 ,SqFtCarportVariableCost    decimal(15,8)
 ,SqFtCarportMinArea  decimal(15,4)
 ,SqFtDetachGarageFixedCost    decimal(15,8)
 ,SqFtDetachGarageVariableCost    decimal(15,8)
 ,SqFtDetachGarageMinArea  decimal(15,4)
 ,SqFtPoolFixedCost    decimal(15,8)
 ,SqFtPoolVariableCost    decimal(15,8)
 ,SqFtPoolMinArea  decimal(15,4)
 ,StdDrivewayFixedCost  decimal(15,8)  
 ,StdDrivewayVariableCost    decimal(15,8)
 ,StdDrivewayMinArea  decimal(15,4)
 ,PRIMARY KEY(rowID)
)

CREATE TABLE #ResCostAccyGradeFactor (
  rowID  	int	not null  identity(1,1)
 ,Grade tinyint
 ,ConcretePaving decimal(15,6)
 ,AsphaltPaving decimal(15,6)
 ,SqFtCarport decimal(15,6)
 ,SqFtDetachGarage decimal(15,6)
 ,SqFtPool decimal(15,6)
 ,StdDriveway decimal(15,6)
 ,PRIMARY KEY(rowID)
 ,UNIQUE(Grade, rowID)
)

CREATE TABLE #MultAccy (
  rowID  	int	not null  identity(1,1)
 ,ParcelId uniqueidentifier
 ,AccyType int
 ,CountThisType int
 ,AccyDescr varchar(10)
 ,AccyNote  varchar(60)
 ,PRIMARY KEY(rowID)
)

CREATE TABLE #MultMult  (  
  rowID  	int	not null  identity(1,1)
 ,ParcelId uniqueidentifier
 ,AccyType int
 ,PRIMARY KEY(rowID)
)

CREATE TABLE #NoteInstance (
  rowID  	int	not null  identity(1,1)
 ,ParcelId uniqueidentifier
 ,AssmtEntityId char(4)
 ,UpdateDate smalldatetime
 ,Note varchar(max)
 --,NoteInstance int
 ,PRIMARY KEY(rowID)
 ,UNIQUE(ParcelId, rowID)
)

CREATE TABLE #SaleWarnings (
 rowID  int not null identity(1,1)
 ,SaleGuid uniqueidentifier
 ,ParcelId uniqueidentifier
 ,Warnings varchar(200)
 --,[Order] int
)

--New temp table for Sale Warnings 5/16/15 Don G
CREATE TABLE #ConcatenateWarnings (
 rowID int not null identity(1,1)
 ,SaleGuid uniqueidentifier
 ,ParcelId uniqueidentifier
 ,Warnings nvarchar(max)
)

--New temp table for trended sale price
CREATE TABLE #TrendedSales (
  rowID int not null identity(1,1)
  ,SaleGuid uniqueidentifier
  ,ParcelId uniqueidentifier
  ,SaleDate smalldatetime
  ,SalePrice int
  ,TrendFactor decimal(14,8)
  ,TrendedPrice bigint
  )
  
CREATE TABLE #Bldgs (
 rowId int identity(1,1)
 ,ParcelId uniqueidentifier
 ,BldgGuid uniqueidentifier
) --not to be confused with #Bldg table which is for a different purpose
 

CREATE TABLE #MHCount(
 ParcelId uniqueidentifier
 ,MHCount int
 )

CREATE TABLE #AllMobiles(
 ParcelID uniqueidentifier
 ,MhGuid uniqueidentifier
 ,MHType int
 ,AcctStatus int
 ,BldgNbr tinyint
 ,MHCount int
 ,MHClass tinyint
 ,MHLength int
 ,MHWidth int
 ,MHLivingArea int
 ,MHTipOutArea int
 ,MHRoomAddSqft int
 ,MHSize int
 ,MHYrBuilt int
 ,MHCondition tinyint
 ,MHPcntNetCondition int
 ,MHRCN int
 ,MHRCNLD int
 ,MobileHomeId int
 --,LandId int
 )

CREATE TABLE #ShowMobiles (
  ParcelId uniqueidentifier
  ,MhGuid uniqueidentifier
  ,MHType int
  ,AcctStatus int
  ,BldgNbr tinyint
  ,MHCount int
  ,MHClass tinyint
  ,MHLength int
  ,MHWidth int
  ,MHLivingArea int
  ,MHTipOutArea int
  ,MHRoomAddSqft int
  ,MHSize int
  ,MHYrBuilt int
  ,MHCondition tinyint
  ,MHPcntNetCondition int
  ,MHRCN int
  ,MHRCNLD int
  ,MobileHomeId int
  --,LandId int
  )

CREATE TABLE #GetMHCost (
 id int identity(1,1)
 ,MHGuid uniqueidentifier
 ,ParcelId uniqueidentifier
 ,MHId int
 ,MHTypeItemId smallint
 ,AcctStatusItemId smallint
 ,ExpectedLife smallint
 ,PercentGood decimal(5,4)
 ,RCN int
 ,RCNLD int
 )

--To get cost of all buildings. We won't be returning the data but we will calculate and include cost in TotalRCN and TotalRCNLD 
--Use #SalesPop so it can use the cost stored procedures that AreaData uses 
CREATE TABLE #ResBldg (
  RowId                 int identity(1,1)
 ,ParcelId              uniqueidentifier
-- ,RealPropId          int
 ,LandGuid              uniqueidentifier
 ,BldgNbr               int
 ,NbrLivingUnits        smallint        DEFAULT 0 
 ,Stories               real        DEFAULT 0 
 ,BldgGrade             tinyint        DEFAULT 0 
 ,BldgGradeVar          tinyint        DEFAULT 0 
 ,SqFt1stFloor          int        DEFAULT 0 
 ,SqFtHalfFloor         int        DEFAULT 0 
 ,SqFt2ndFloor          int        DEFAULT 0 
 ,SqFtUpperFloor        int        DEFAULT 0  
 ,SqFtUnfinFull         int        DEFAULT 0 
 ,SqFtUnfinHalf         int        DEFAULT 0 
 ,SqFtTotLiving         int        DEFAULT 0 
 ,SqFtTotBasement       int        DEFAULT 0 
 ,SqFtFinBasement       int        DEFAULT 0 
 ,FinBasementGrade      tinyint        DEFAULT 0 
 ,SqFtGarageBasement    int        DEFAULT 0 
 ,SqFtGarageAttached    int        DEFAULT 0 
 ,DaylightBasement      char(1)    DEFAULT ''
 ,Bedrooms              tinyint        DEFAULT 0 
 ,BathHalfCount         tinyint        DEFAULT 0 
 ,Bath3qtrCount         tinyint        DEFAULT 0 
 ,BathFullCount         tinyint        DEFAULT 0 
 ,FpSingleStory         tinyint        DEFAULT 0 
 ,FpMultiStory         tinyint        DEFAULT 0 
 ,FpFreestanding         tinyint        DEFAULT 0 
 ,FpAdditional         tinyint        DEFAULT 0 
 ,YrBuilt               smallint        DEFAULT 0 
 ,YrRenovated           smallint        DEFAULT 0 
 ,PcntComplete         tinyint        DEFAULT 0 
 ,Obsolescence         tinyint        DEFAULT 0 
 ,PcntNetCondition         tinyint        DEFAULT 0 
 ,Condition         tinyint        DEFAULT 0 
 ,ViewUtilization       char(1)    DEFAULT ''
 ,SqFtOpenPorch         int        DEFAULT 0 
 ,SqFtEnclosedPorch     int        DEFAULT 0 
 ,SqFtDeck              int        DEFAULT 0 
 ,HeatSystem        tinyint        DEFAULT 0  
 ,HeatSource        tinyint        DEFAULT 0 
 ,BrickStone            tinyint        DEFAULT 0 
 ,AddnlCost             int        DEFAULT 0 
 --,BldgId                int
 ,BldgGuid              uniqueidentifier
 ,UnitsAdj 		decimal(3,2) 	 Default 0
 ,EffYr 		smallint 	 Default 0
 ,Age 			smallint 	 Default 0
 ,BldgGradeCateg 	tinyint 	 Default 0
 ,BldgFormula 		tinyint 	 Default 0
 ,BldgFactor1 		decimal(9,6) 	 Default 0
 ,BldgFactor2 		decimal(9,6) 	 Default 0
 ,BsmtGradeCateg 	tinyint 	 Default 0
 ,BsmtFormula 		tinyint 	 Default 0
 ,BsmtFactor1 		decimal(9,6) 	 Default 0
 ,BsmtFactor2 		decimal(9,6) 	 Default 0
 ,PcntGoodBldg 		decimal(9,6) 	 Default 0
 ,PcntGoodBsmt 		decimal(9,6) 	 Default 0
 ,BldgGradeAdj 		decimal(3,2) 	 Default 0
 ,finBsmtCost 		int 		 Default 0
 ,sBldgRCN          int 		 Default 0
 ,sBldgRCNLD 		int 		 Default 0
 ,sAccyRCN 		    int 		 Default 0
 ,sAccyRCNLD 		int 		 Default 0
 ,BldgRCNc 		    int 		 Default 0
 ,BldgRCNLDc 		int 		 Default 0
 ,AccyRCNc 		    int 		 Default 0
 ,AccyRCNLDc 		int 		 Default 0
 ,BldgRCN                      int        DEFAULT 0 
 ,BldgRCNLD                    int        DEFAULT 0 
)

END

--*********************************************************************************************************************************************************************************
-- BEGIN of ASSR_R_GetAreaData2b_Inserts_0
--*********************************************************************************************************************************************************************************  

IF @SubArea = 0 OR @SubArea IS NULL
	BEGIN
	    INSERT INTO #SalesPop(PIN,MapPin,ParcelId,SaleGuid,LandGuid, ExciseTaxNbr, SaleDate, SalePrice, VerifiedAtMkt, VerifiedBy, SellerName, BuyerName, PropCnt
	                         ,ResArea, ResSubArea, QSec, Sec, Twn, Rng, Neighborhood, Folio, PlatLot, PlatBlock
	                         , Major, Minor, PropType,Saleid,DistrictName,HoldoutReason) 
		 SELECT      PIN   = COALESCE(dpd.ptas_major,'') + COALESCE(dpd.ptas_minor,'')
					,MapPin = convert(float,dpd.ptas_major+dpd.ptas_minor)
					,dpd.ptas_parceldetailid
					,dps.ptas_salesid
					,dpd._ptas_landid_value
					,dps.ptas_excisetaxnumber								
					,dps.ptas_saledate
					,dps.ptas_saleprice
					,dps.ptas_atmarket					
					,dps.ptas_verifiedby					
					,Seller = COALESCE(dps.ptas_grantorfirstname,'') +' '+COALESCE(dps.ptas_grantorlastname,'') 
					,Buyer  = COALESCE(dps.ptas_granteefirstname,'') +' '+COALESCE(dps.ptas_granteelastname,'')
					,dps.ptas_nbrparcels
					,dpd.ptas_resarea
					,dpd.ptas_ressubarea					
					,QuarterSection = COALESCE(dsm.value,'')
					,Section	    = pqstr.ptas_section	
					,Township	    = pqstr.ptas_township
					,Range		    = pqstr.ptas_range	
					,Neighborhood   = COALESCE(dpn.ptas_name,'')
					,Folio		= dpd.ptas_folio					
					,PlatLot    = COALESCE(dpd.ptas_platlot,'')
					,PlatBlock  = COALESCE(dpd.ptas_platblock,'')
					,Major 		= dpd.ptas_major 
					,Minor 		= dpd.ptas_minor
					,PropType  	= pt.PropType
					,dps.ptas_name as Saleid
					,dpd.ptas_district
					,HoldoutReason =  dynamics.fn_GetValueFromStringMap('ptas_parceldetail', 'ptas_holdoutreason', dpd.ptas_holdoutreason)
			   FROM [dynamics].[ptas_parceldetail] dpd
			  INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
			  INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
			     ON spdps.ptas_parceldetailid = dpd.ptas_parceldetailid
			  INNER JOIN [dynamics].[ptas_sales] dps
				 ON dps.ptas_salesid = spdps.ptas_salesid				 
				 
			   LEFT JOIN [dynamics].[ptas_qstr] pqstr
				 ON pqstr.[ptas_qstrid] = dpd._ptas_qstrid_value
				
			   LEFT JOIN [dynamics].[stringmap]	dsm
			  	 ON dsm.attributevalue = pqstr.ptas_quartersection 
				AND dsm.objecttypecode = 'ptas_qstr'
				AND dsm.attributename  = 'ptas_quartersection'	
			   LEFT JOIN dynamics.ptas_neighborhood dpn
				 ON dpn.ptas_neighborhoodid = dpd._ptas_neighborhoodid_value				
			  WHERE COALESCE(dpd.ptas_splitcode,0) = 0
				AND (dpd.statecode  = 0 AND dpd.statuscode = 1)
				AND COALESCE(dpd.ptas_snapshottype,'') = ''  
				AND dpd.ptas_resarea   = @Area
			    AND pt.PropType  = 'R'
				AND dpd.ptas_applgroup = @ApplGroup
				AND dps.ptas_saledate BETWEEN @SalesFrom AND @SalesTo 
                AND dps.ptas_saleprice >= @SalePrice										 

	IF @Population = 'Y'
	   BEGIN 

		INSERT INTO #SalesPop(PIN,MapPin,ParcelId,LandGuid, ResArea, ResSubArea, QSec, Sec, Twn, Rng, Neighborhood, Folio, PlatLot, PlatBlock
	                         , Major, Minor, PropType,DistrictName,HoldoutReason) 
		 SELECT      PIN   = COALESCE(dpd.ptas_major,'') + COALESCE(dpd.ptas_minor,'')
					,MapPin = convert(float,dpd.ptas_major+dpd.ptas_minor)
					,dpd.ptas_parceldetailid
					,dpd._ptas_landid_value	
					,dpd.ptas_resarea
					,dpd.ptas_ressubarea					
					,QuarterSection = COALESCE(dsm.value,'')
					,Section	    = pqstr.ptas_section	
					,Township	    = pqstr.ptas_township
					,Range		    = pqstr.ptas_range	
					,Neighborhood   = COALESCE(dpn.ptas_name,'')
					,Folio		= dpd.ptas_folio					
					,PlatLot    = COALESCE(dpd.ptas_platlot,'')
					,PlatBlock  = COALESCE(dpd.ptas_platblock,'')
					,Major 		= dpd.ptas_major 
					,Minor 		= dpd.ptas_minor
					,PropType  	= pt.PropType
					,dpd.ptas_district 
					,HoldoutReason =  dynamics.fn_GetValueFromStringMap('ptas_parceldetail', 'ptas_holdoutreason', dpd.ptas_holdoutreason)
			   FROM [dynamics].[ptas_parceldetail] dpd
			  INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
			   LEFT JOIN [dynamics].[ptas_qstr] pqstr
				 ON pqstr.[ptas_qstrid] = dpd._ptas_qstrid_value
				
			   LEFT JOIN [dynamics].[stringmap]	dsm
			  	 ON dsm.attributevalue = pqstr.ptas_quartersection 
				AND dsm.objecttypecode = 'ptas_qstr'
				AND dsm.attributename  = 'ptas_quartersection'
	  		   LEFT JOIN dynamics.ptas_neighborhood dpn
				 ON dpn.ptas_neighborhoodid = dpd._ptas_neighborhoodid_value
			  WHERE COALESCE(dpd.ptas_splitcode,0) = 0
				AND COALESCE(dpd.ptas_snapshottype,'') = '' 
				AND (dpd.statecode  = 0 AND dpd.statuscode = 1)
			    AND dpd.ptas_resarea   = @Area
			    AND pt.PropType  = 'R'
				AND dpd.ptas_applgroup = @ApplGroup
				AND NOT EXISTS (select * from #SalesPop rs WHERE rs.ParcelId = dpd.ptas_parceldetailid)
	   END
	END
ELSE
	BEGIN
		IF @Area > 0
		BEGIN
		
		    INSERT INTO #SalesPop(PIN,MapPin,ParcelId,SaleGuid,LandGuid, ExciseTaxNbr, SaleDate, SalePrice, VerifiedAtMkt, VerifiedBy, SellerName, BuyerName, PropCnt
		                         ,ResArea, ResSubArea, QSec, Sec, Twn, Rng, Neighborhood, Folio, PlatLot, PlatBlock
		                         , Major, Minor, PropType,Saleid,DistrictName,HoldoutReason) 
			 SELECT     PIN   = COALESCE(dpd.ptas_major,'') + COALESCE(dpd.ptas_minor,'')
						,MapPin = convert(float,dpd.ptas_major+dpd.ptas_minor)
						,dpd.ptas_parceldetailid
						,dps.ptas_salesid
						,dpd._ptas_landid_value	
						,dps.ptas_excisetaxnumber								
						,dps.ptas_saledate
						,dps.ptas_saleprice
						,dps.ptas_atmarket					
						,dps.ptas_verifiedby					
						,Seller = COALESCE(dps.ptas_grantorfirstname,'') +' '+COALESCE(dps.ptas_grantorlastname,'') 
						,Buyer  = COALESCE(dps.ptas_granteefirstname,'') +' '+COALESCE(dps.ptas_granteelastname,'')
						,dps.ptas_nbrparcels
						,dpd.ptas_resarea
						,dpd.ptas_ressubarea					
						,QuarterSection = COALESCE(dsm.value,'')
						,Section	    = pqstr.ptas_section	
						,Township	    = pqstr.ptas_township
						,Range		    = pqstr.ptas_range	
						,Neighborhood   = COALESCE(dpn.ptas_name,'')
						,Folio		= dpd.ptas_folio					
						,PlatLot    = COALESCE(dpd.ptas_platlot,'')
						,PlatBlock  = COALESCE(dpd.ptas_platblock,'')
						,Major 		= dpd.ptas_major 
						,Minor 		= dpd.ptas_minor
						,PropType  	= pt.PropType
					    ,dps.ptas_name as Saleid
						,dpd.ptas_district
						,HoldoutReason =  dynamics.fn_GetValueFromStringMap('ptas_parceldetail', 'ptas_holdoutreason', dpd.ptas_holdoutreason)
				   FROM [dynamics].[ptas_parceldetail] dpd
				  INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
				  INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
				     ON spdps.ptas_parceldetailid = dpd.ptas_parceldetailid
				  INNER JOIN [dynamics].[ptas_sales] dps
					 ON dps.ptas_salesid = spdps.ptas_salesid				 
				   LEFT JOIN [dynamics].[ptas_qstr] pqstr
					 ON pqstr.[ptas_qstrid] = dpd._ptas_qstrid_value
				   LEFT JOIN [dynamics].[stringmap]	dsm
				  	 ON dsm.attributevalue = pqstr.ptas_quartersection 
					AND dsm.objecttypecode = 'ptas_qstr'
					AND dsm.attributename  = 'ptas_quartersection'								 
				   LEFT JOIN dynamics.ptas_neighborhood dpn
				     ON dpn.ptas_neighborhoodid = dpd._ptas_neighborhoodid_value
				  WHERE COALESCE(dpd.ptas_splitcode,0) = 0
					AND (dpd.statecode  = 0 AND dpd.statuscode = 1)
					AND COALESCE(dpd.ptas_snapshottype,'') = '' 
				    AND dpd.ptas_resarea    = @Area
				    AND dpd.ptas_ressubarea = @SubArea
				    AND pt.PropType   = 'R'
					AND dpd.ptas_applgroup  = @ApplGroup
					AND dps.ptas_saledate BETWEEN @SalesFrom AND @SalesTo 
	                AND dps.ptas_saleprice >= @SalePrice				
		END
		IF @Population = 'Y'
		BEGIN

		INSERT INTO #SalesPop(PIN,MapPin,ParcelId,LandGuid, ResArea, ResSubArea, QSec, Sec, Twn, Rng, Neighborhood, Folio, PlatLot, PlatBlock
			                     , Major, Minor, PropType,DistrictName,HoldoutReason) 
			 SELECT      PIN   = COALESCE(dpd.ptas_major,'') + COALESCE(dpd.ptas_minor,'')
						,MapPin = convert(float,dpd.ptas_major+dpd.ptas_minor)
						,dpd.ptas_parceldetailid
						,dpd._ptas_landid_value	
						,dpd.ptas_resarea
						,dpd.ptas_ressubarea					
						,QuarterSection = COALESCE(dsm.value,'')
						,Section	    = pqstr.ptas_section	
						,Township	    = pqstr.ptas_township
						,Range		    = pqstr.ptas_range	
						,Neighborhood   = COALESCE(dpn.ptas_name,'')
						,Folio		= dpd.ptas_folio					
						,PlatLot    = COALESCE(dpd.ptas_platlot,'')
						,PlatBlock  = COALESCE(dpd.ptas_platblock,'')
						,Major 		= dpd.ptas_major 
						,Minor 		= dpd.ptas_minor
						,PropType  	= pt.PropType
						,dpd.ptas_district 
						,HoldoutReason =  dynamics.fn_GetValueFromStringMap('ptas_parceldetail', 'ptas_holdoutreason', dpd.ptas_holdoutreason)
				   FROM [dynamics].[ptas_parceldetail] dpd
				  INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
				   LEFT JOIN [dynamics].[ptas_qstr] pqstr
					 ON pqstr.[ptas_qstrid] = dpd._ptas_qstrid_value
				   LEFT JOIN [dynamics].[stringmap]	dsm
				  	 ON dsm.attributevalue = pqstr.ptas_quartersection 
					AND dsm.objecttypecode = 'ptas_qstr'
					AND dsm.attributename  = 'ptas_quartersection'									 
				   LEFT JOIN dynamics.ptas_neighborhood dpn
				     ON dpn.ptas_neighborhoodid = dpd._ptas_neighborhoodid_value
				  WHERE COALESCE(dpd.ptas_splitcode,0) = 0
					AND (dpd.statecode  = 0 AND dpd.statuscode = 1)
					AND COALESCE(dpd.ptas_snapshottype,'') = '' 
				    AND dpd.ptas_resarea    = @Area
				    AND dpd.ptas_ressubarea = @SubArea
				    AND pt.PropType   = 'R'
					AND dpd.ptas_applgroup  = @ApplGroup
					AND NOT EXISTS (select * from #SalesPop rs WHERE rs.ParcelId = dpd.ptas_parceldetailid)
		END
	END	

  INSERT 
    INTO #RealProp
  SELECT DISTINCT ParcelId, LandGuid, PropType, Minor
    FROM #SalesPop								 								 															 

  INSERT 
    INTO #Land
  SELECT DISTINCT r.LandGuid	
    FROM #RealProp r



INSERT 
  INTO #XLand (LandGuid,BaseLandVal,BaseLandValTaxYr,BaseLandValDate,SqFtLot,CurrentZoning,ZoningChgDate,HBUAsIfVacant,HBUAsImproved,PresentUse,WaterSystem,SewerSystem,
			  Access,StreetSurface,PcntBaseLandValImpacted,Unbuildable,NbrBldgSites,HistoricSite,CurrentUseDesignation,
			  MtRainier,Olympics,Cascades,Territorial,SeattleSkyline,PugetSound,LakeWashington,LakeSammamish,SmallLakeRiverCreek,OtherView,
			  WfntLocation,WfntFootage,WfntPoorQuality,WfntBank,TidelandShoreland,WfntRestrictedAccess,LotDepthFactor,WfntAccessRights,WfntProximityInfluence,
			  RestrictiveSzShape,TrafficNoise,AirportNoise,PowerLines,OtherNuisances,WaterProblems,OtherProblems,Topography,
			  CommonProperty,DevelopmentRightsPurch,
			  NativeGrowthProtEsmt,AdjacentGolfFairway,AdjacentGreenbelt,OtherDesignation,DeedRestrictions,Easements,SpecialAssessments,
			  Contamination,CoalMineHazard,CriticalDrainage,ErosionHazard,LandfillBuffer,HundredYrFloodPlain,SeismicHazard,LandslideHazard,SteepSlopeHazard,Stream,Wetland,SpeciesOfConcern,SensitiveAreaTract
			  )
SELECT  dpl.ptas_landid
	   ,dpl.ptas_baselandValue
	   ,dpl.ptas_taxyear
	   ,dpl.ptas_valueDate 
	   ,dpl.ptas_sqftTotal 	
	   ,dpl.ptas_zoning
	   ,dpl.ptas_zoningchangedate
	   ,dpl.ptas_hbuifvacant 	
	   ,dpl.ptas_hbuifimproved 
	   ,dpl.ptas_presentuse	
	   ,dpl.ptas_watersystem 	
	   ,dpl.ptas_sewersystem 	
	   ,dpl.ptas_roadaccess 	
	   ,dpl.ptas_streetSurface 
	   ,dpl.ptas_percentbaseLandValue
	   ,dpl.ptas_unbuildable 
	   ,dpl.ptas_totalsitesperzoning
	   ,dpl.ptas_historicsite
	   ,dpl.ptas_currentuse
	   --VIEWS
	   ,vw_LvVW.VI_Rainier
	   ,vw_LvVW.VI_Olympics
	   ,vw_LvVW.VI_Cascades
	   ,vw_LvVW.VI_Territorial
	   ,vw_LvVW.VI_Seattle
	   ,vw_LvVW.VI_Puget_Sound
	   ,vw_LvVW.VI_Lake_Washington
	   ,vw_LvVW.VI_Lake_Sammamish
	   ,vw_LvVW.VI_Lake_river_or_creek
	   ,vw_LvVW.VI_Other_view
	   --WATERFRONT
	   ,vw_LvWF.WF_Id--(WF location)
	   ,vw_LvWF.WF_ptas_linearfootage
	   ,vw_LvWF.WF_ptas_poorquality
	   ,vw_LvWF.WF_ptas_waterfrontbank
	   ,vw_LvWF.WF_ptas_tidelandorshoreland
	   ,vw_LvWF.WF_ptas_restrictedaccess
	   ,vw_LvWF.WF_ptas_depthfactor
	   ,vw_LvWF.WF_ptas_accessrights
	   ,vw_LvWF.WF_ptas_proximityinfluence				
	   --NUISANCE
	   ,vw_LvNU.NU_Restrictive_size_or_shape
	   ,vw_LvNU.NU_Traffic_noise
	   ,vw_LvNU.NU_Airport_noise
	   ,vw_LvNU.NU_Power_lines
	   ,vw_LvNU.NU_Other_nuisance
	   ,vw_LvNU.NU_Water_problem
	   ,vw_LvNU.NU_Other_Problems
	   ,vw_LvNU.NU_Topography
	   ,0 --CommonProperty
	   ,vw_LvDR.Dev_Purchased
	   --DESIGNATIONS
	   ,vw_LvDes.Desig_Native_growth
	   ,vw_LvDes.Desig_Adjacent_golf
	   ,vw_LvDes.Desig_Adjacent_greenbelt
	   ,vw_LvDes.Desig_Other
	   ,vw_LvDes.Desig_Deed_restrictions
	   ,vw_LvDes.Desig_Easements
	   ,0 --SpecialAssessments
	   --ENVIRONMENTAL RESTRICTIONS
	   ,vw_LvER.ER_Contamination
	   ,vw_LvER.ER_Coal_mine_hazard
	   ,vw_LvER.ER_Critical_drainage
	   ,vw_LvER.ER_Erosion_hazard
	   ,vw_LvER.ER_Landfill_buffer
	   ,vw_LvER.ER_100_year_flood_plain
	   ,vw_LvER.ER_Seismic_hazard
	   ,vw_LvER.ER_Landslide_hazard
	   ,vw_LvER.ER_Steep_slope_hazard 
	   ,vw_LvER.ER_Stream
	   ,vw_LvER.ER_Wetland
	   ,vw_LvER.ER_Species_of_concern 
	   ,vw_LvER.ER_Sensitive_area_tract
	   /*
	   Hairo comment> ADD LAndSchedule characteristic we need the descriptions(as zoning type)
	   
	   */
  FROM dynamics.ptas_land dpl
 INNER JOIN #Land tpl
	ON tpl.LndGuid = dpl.ptas_landid
  LEFT JOIN [dynamics].[vw_LandValueNuisance] vw_LvNU
	ON vw_LvNU._ptas_landid_value = dpl.ptas_landid

  LEFT JOIN [dynamics].[vw_LandValueView] vw_LvVW
	ON vw_LvVW._ptas_landid_value = dpl.ptas_landid
		
  LEFT JOIN [dynamics].[vw_LandValueWaterFront] vw_LvWF
	ON vw_LvWF._ptas_landid_value = dpl.ptas_landid
	
  LEFT JOIN [dynamics].[vw_LandValueEnvironmentalRestriction] vw_LvER
	ON vw_LvER._ptas_landid_value = dpl.ptas_landid
	
  LEFT JOIN [dynamics].[vw_LandValueDesignations] vw_LvDes
	ON vw_LvDes._ptas_landid_value = dpl.ptas_landid	

  LEFT JOIN [dynamics].[vw_LandValueDevelopmentRight] vw_LvDR
	ON vw_LvDR._ptas_landid_value = dpl.ptas_landid	


INSERT 
  INTO #TaxAcctReceivable(ParcelId, AcctNbr, SrCitFlag, ApprLandVal, ApprImpsVal, TaxStat,
						  TaxPayerName)
SELECT  r.ParcelId
	   ,trh.ptas_taxaccountidname
	   ,SrCitizenFlag = '' --Hairo comment I cannot find this colum
	   ,trh.ptas_taxablelandvalue
	   ,trh.ptas_taxableimpvalue
	   ,trh.ptas_TaxStat	   
	   ,ta.ptas_taxpayername
  FROM #RealProp r 
  INNER JOIN [ptas].[ptas_taxrollhistory] trh
     ON r.ParcelId = trh.ptas_parcelid
  LEFT JOIN [dynamics].[ptas_taxaccount] ta
    ON ta.ptas_taxaccountid = trh.ptas_taxaccountid  
 WHERE trh.ptas_receivabletype = 'R'
   AND trh.ptas_taxyearidname = @BillYr
   AND trh.ptas_omityearidname = 0



--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_0a
--*********************************************************************************************************************************************************************************  

--Hairo comment: this code was added in line 370(first insert to #TaxAcctReceivable)


--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Inserts_1
--*********************************************************************************************************************************************************************************  
  INSERT 
    INTO #Sales
  SELECT DISTINCT SaleGuid 
    FROM #SalesPop	

 INSERT 
   INTO #GASCodes	( SaleGuid, SalePropType, PrinUse, PropClass, Reason, Instr, Verif)
SELECT  rs.SaleGuid
	    ,ps.ptas_affidavitpropertytype
	    ,ps.ptas_salesprincipleuse
	    ,ps.ptas_salepropertyclass
	    ,ps.ptas_reason
	    ,ps.ptas_instrument
	    ,ps.ptas_levelofverification
   FROM #Sales rs 
  INNER JOIN [dynamics].[ptas_sales] ps
	ON  rs.SaleGuid = ps.ptas_salesid


--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_1b
--*********************************************************************************************************************************************************************************  

UPDATE #SalesPop
     SET LandGuid          = l.LandGuid
     	,BaseLandVal       = l.BaseLandVal
     	,BaseLandValTaxYr  = l.BaseLandValTaxYr
     	,BaseLandValDate   = l.BaseLandValDate
     	,SqFtLot           = l.SqFtLot
     	,CurrentZoning     = l.CurrentZoning
     	,ZoningChgDate     = l.ZoningChgDate
     	,HBUAsIfVacant     = l.HBUAsIfVacant
     	,HBUAsImproved     = l.HBUAsImproved
     	,PresentUse        = l.PresentUse
     	,WaterSystem       = l.WaterSystem
     	,SewerSystem       = l.SewerSystem
     	,Access            = l.Access
     	,StreetSurface     = l.StreetSurface
     	,PcntBaseLandValImpacted = l.PcntBaseLandValImpacted
     	,Unbuildable       = l.Unbuildable
     	,RestrictiveSzShape= l.RestrictiveSzShape
FROM #SalesPop rs INNER JOIN #XLand l ON rs.LandGuid = l.LandGuid


	

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_1c
--*********************************************************************************************************************************************************************************  
UPDATE #SalesPop
     SET MtRainier             = el.MtRainier
     	,Olympics              = el.Olympics
     	,Cascades              = el.Cascades
     	,Territorial           = el.Territorial
     	,SeattleSkyline        = el.SeattleSkyline
     	,PugetSound            = el.PugetSound
     	,LakeWashington        = el.LakeWashington
     	,LakeSammamish         = el.LakeSammamish
     	,SmallLakeRiverCreek   = el.SmallLakeRiverCreek
     	,OtherView             = el.OtherView
     	,WfntLocation          = el.WfntLocation
     	,WfntFootage           = el.WfntFootage
     	,WfntPoorQuality       = el.WfntPoorQuality
     	,WfntBank			   = el.WfntBank
     	,TidelandShoreland     = el.TidelandShoreland
     	,WfntRestrictedAccess  = el.WfntRestrictedAccess
     	,LotDepthFactor        = el.LotDepthFactor
     	,TrafficNoise          = el.TrafficNoise
     	,AirportNoise          = el.AirportNoise
     	,CommonProperty        = el.CommonProperty
     	,CurrentUseDesignation = el.CurrentUseDesignation
     	,NbrBldgSites          = el.NbrBldgSites
     	,Contamination		   = el.Contamination
     	,WfntAccessRights      = el.WfntAccessRights
     	,WfntProximityInfluence= el.WfntProximityInfluence
     	,NativeGrowthProtEsmt  = el.NativeGrowthProtEsmt
     	,PowerLines			   = el.PowerLines
     	,OtherNuisances        = el.OtherNuisances
     	,AdjacentGolfFairway   = el.AdjacentGolfFairway
     	,AdjacentGreenbelt     = el.AdjacentGreenbelt
     	,OtherDesignation      = el.OtherDesignation
     	,DeedRestrictions      = el.DeedRestrictions
     	,DevelopmentRightsPurch= el.DevelopmentRightsPurch
     	,Easements             = el.Easements
     	,SpecialAssessments    = el.SpecialAssessments
     	,CoalMineHazard        = el.CoalMineHazard
     	,CriticalDrainage      = el.CriticalDrainage
     	,ErosionHazard         = el.ErosionHazard
     	,LandfillBuffer        = el.LandfillBuffer
     	,HundredYrFloodPlain   = el.HundredYrFloodPlain
     	,SeismicHazard         = el.SeismicHazard
     	,LandslideHazard       = el.LandslideHazard
     	,SteepSlopeHazard      = el.SteepSlopeHazard
     	,Stream                = el.Stream
     	,Wetland               = el.Wetland
     	,SpeciesOfConcern      = el.SpeciesOfConcern
     	,SensitiveAreaTract    = el.SensitiveAreaTract
     	,WaterProblems         = el.WaterProblems
     	,OtherProblems         = el.OtherProblems
     	,HistoricSite          = el.HistoricSite
     	,Topography            = el.Topography 
FROM #SalesPop rs INNER JOIN #XLand el ON (rs.LandGuid = el.LandGuid )



--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_1d
--*********************************************************************************************************************************************************************************  

/*
***NOT TESTED ***NOT TESTED 
Missing table
Hairo comment: need to figure out how to cvalculate these values,  Ithink they are related to [dynamics].[ptas_salesaggregate] but the table is empty
UPDATED: Jan/07/2021 I think I have to do a change since column CurrentZoning have the value presented in ZoneDesignation in Real Property system,
					 I find out that CurrentZoning is [ptas_zoning].ptas_zoneid, but there is no way to connect the ptas_Land table with with ptas_zoning

UPDATE #SalesPop
     SET ZoneClass = zl.LuItemShortDesc
     	,ZoneDesignation = z.ZoneDesignation
 FROM #SalesPop rs 
INNER JOIN Zoning z (NOLOCK) 
   ON (z.ZoneId = rs.CurrentZoning and rs.CurrentZoning > 0)
INNER JOIN LuItem2 zl (NOLOCK) 
   ON (zl.LuTypeId = z.ZoneCodeLU and zl.LuItemId = z.ZoneCode)

***NOT TESTED ***NOT TESTED 
*/   
   
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_1e
--*********************************************************************************************************************************************************************************  

  UPDATE #SalesPop
     SET TaxPayerName = ar.TaxPayerName
     	,ApprLandVal = ar.ApprLandVal
     	,ApprImpsVal = ar.ApprImpsVal
     	,SrCitFlag = ar.SrCitFlag --Hairo comment: comes from #TaxAcctReceivable from column SrCitizenFlag  that was not found.
     	,TaxStat = ar.TaxStat
   FROM #SalesPop rs 
  INNER JOIN #TaxAcctReceivable ar 
     ON rs.ParcelId = ar.ParcelId


 
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_1f
--*********************************************************************************************************************************************************************************  

 UPDATE  #SalesPop
    SET  SalePropType = c.SalePropType
     	,PrinUse = c.PrinUse
     	,PropClass = c.PropClass
     	,Reason = c.Reason
     	,Instr = c.Instr
     	,Verif = c.Verif
   FROM #SalesPop rs 
  INNER JOIN #GASCodes c 
     ON rs.SaleGuid = c.SaleGuid
 
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Inserts_1sw
--*********************************************************************************************************************************************************************************  

	INSERT #SaleWarnings
	SELECT  rs.SaleGuid,rs.ParcelId, swc.ptas_name
	  FROM #SalesPop rs
     INNER JOIN [dynamics].[ptas_sales_ptas_saleswarningcode] sswc
        ON rs.SaleGuid = sswc.ptas_salesid
     INNER JOIN [dynamics].[ptas_saleswarningcode] swc
        ON sswc.ptas_saleswarningcodeid = swc.ptas_saleswarningcodeid
     ORDER BY rs.ParcelId

	INSERT #ConcatenateWarnings
	SELECT sw.SaleGuid,sw.ParcelId
			,STUFF((SELECT '; ' + sw2.Warnings
                        FROM #SaleWarnings sw2
                        WHERE sw2.SaleGuid = sw.SaleGuid
                          and sw2.ParcelId = sw.ParcelId
                        FOR XML PATH('')
                        ), 1, 1, '' )
	  FROM #SaleWarnings sw
	 GROUP By sw.ParcelId,sw.SaleGuid

 
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_2a
--*********************************************************************************************************************************************************************************  

UPDATE #SalesPop
SET Warnings = cw.Warnings
FROM #ConcatenateWarnings cw INNER JOIN #SalesPop rs ON cw.SaleGuid = rs.SaleGuid
                                                    AND cw.ParcelId = rs.ParcelId
 
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_3b
--*********************************************************************************************************************************************************************************  


UPDATE #SalesPop
SET PossibleLenderSale = 'Y'
FROM #SalesPop rs
WHERE (SellerName like '%FORECLOSURE%'
     or SellerName like '%TRUST%' 
     or SellerName like '%TITLE%'
     or SellerName like '%SERVICES%'
     or SellerName like '%ACCEPTANCE%'
     or SellerName like '%BENEFICIAL%'
     or SellerName like '%VETERANS%'
     or SellerName like '%MUTUAL%'
     or SellerName like '%COMMERCE%'
     or SellerName like '%SERVICES%'
     or SellerName like '%MONEY%'
     or SellerName like '%BANK%' 
     or SellerName like '%SAVINGS%' 
     or SellerName like '%FINANCIAL%'
     or SellerName like '%FEDERAL%'
     or SellerName like '%EQUITY%'
     or SellerName like '%FINANC%' 
     or SellerName like '%CAPITAL%'
     or SellerName like '%CREDIT%'
     or SellerName like '%MORTGAGE%'
     or SellerName like '%MTG%'
     or SellerName like '%LOAN%')
  and  SellerName NOT like '%RELOC%'
  and  SellerName NOT like '%LIVING%'
  and  SellerName NOT like '%LVG%'
  and  SellerName NOT like '%LAND TRUST%'
  and  SellerName NOT like '%FAMILY TRUST%'
  and  SellerName NOT like '%REV TRUST%'
  and  BuyerName NOT like '%FORECLOSURE%'
  and  BuyerName NOT like '%TRUST%'
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
  and  BuyerName NOT like '%FINANC%'
  and  BuyerName NOT like '%CAPITAL%'
  and  BuyerName NOT like '%CREDIT%'
  and  BuyerName NOT like '%MORTGAGE%'
  and  BuyerName NOT like '%MTG%'
  and  BuyerName NOT like '%LOAN%'
  and  BuyerName NOT like '%RELOC%'
  and NOT EXISTS ( SELECT * 
					 FROM [dynamics].[ptas_sales_ptas_saleswarningcode] sswc
					INNER JOIN [dynamics].[ptas_saleswarningcode] swc
					   ON swc.ptas_saleswarningcodeid = sswc.ptas_saleswarningcodeid
					WHERE sswc.ptas_salesid = rs.SaleGuid
					  AND swc.ptas_id in (12,40,41,51)
				  )
  					

					
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_4a
--*********************************************************************************************************************************************************************************  

   UPDATE #SalesPop
     SET BldgGuid            = b.ptas_buildingdetailid
     	,BldgNbr             = b.ptas_buildingnumber
     	,NbrLivingUnits      = b.ptas_units
     	,YrBuilt             = y1.ptas_name
     	,YrRenovated         = y2.ptas_name
     	,BldgGrade           = b.ptas_buildinggrade
     	,BldgGradeVar        = b.ptas_gradevariance
     	,Condition           = b.ptas_res_buildingcondition
     	,SqFtTotLiving       = b.ptas_totalliving_sqft
     	,SqFtAboveGrLiving   = b.ptas_totalliving_sqft - b.ptas_finbsmt_sqft
     	,BathTotal           = (COALESCE(B.ptas_12baths * 0.5,0)) + (COALESCE(b.ptas_34baths * 0.75,0)) + COALESCE(b.ptas_fullbathnbr,0)
     	,FpTotal             = COALESCE(b.ptas_single_fireplace,0) + COALESCE(b.ptas_multi_fireplace,0) + COALESCE(b.ptas_fr_std_fireplace,0) + COALESCE(b.ptas_addl_fireplace,0)
     	,SqFtCoveredParking  = 0 
     	,Stories             = b.ptas_numberofstoriesdecimal
     	,SqFt1stFloor        = b.ptas_1stflr_sqft
     	,SqFtHalfFloor       = b.ptas_halfflr_sqft
     	,SqFt2ndFloor        = b.ptas_2ndflr_sqft
     	,SqFtUpperFloor      = b.ptas_upperflr_sqft
     	,SqFtUnfinFull       = b.ptas_unfinished_full_sqft
     	,SqFtUnfinHalf       = b.ptas_unfinished_half_sqft
     	,SqFtTotBasement     = b.ptas_totalbsmt_sqft
     	,SqFtFinBasement     = b.ptas_finbsmt_sqft
     	,FinBasementGrade    = b.ptas_res_basementgrade
     	,SqFtGarageBasement  = b.ptas_basementgarage_sqft
     	,SqFtGarageAttached  = b.ptas_attachedgarage_sqft		
     	,DaylightBasement    = CASE WHEN b.ptas_daylightbasement = 1 THEN 'Y' ELSE '' END
		,Bedrooms            = b.ptas_bedroomnbr
     	,BathHalfCount       = b.ptas_12baths
     	,Bath3qtrCount       = b.ptas_34baths
     	,BathFullCount       = b.ptas_fullbathnbr
     	,FpSingleStory       = b.ptas_single_fireplace
     	,FpMultiStory        = b.ptas_multi_fireplace
     	,FpFreestanding      = b.ptas_fr_std_fireplace
     	,FpAdditional        = b.ptas_addl_fireplace
     	,PcntComplete        = b.ptas_percentcomplete
     	,PcntNetCondition    = b.ptas_percentnetcondition
     	,Obsolescence        = b.ptas_buildingobsolescence
     	,SqFtOpenPorch       = b.ptas_openporch_sqft
     	,SqFtEnclosedPorch   = b.ptas_enclosedporch_sqft
     	,SqFtDeck            = b.ptas_deck_sqft
     	,HeatSystem          = b.ptas_residentialheatingsystem
     	,HeatSource          = b.ptas_res_heatsource
     	,BrickStone          = b.ptas_percentbrickorstone
     	,AddnlCost           = b.ptas_additionalcost
     	,ViewUtilization     = CASE WHEN b.ptas_viewutilizationrating IS NOT NULL  THEN 'Y' ELSE '' END
	FROM #SalesPop rs 
   INNER JOIN [dynamics].[vw_BuildingNumberONE] b 
      ON rs.ParcelId = b._ptas_parceldetailid_value
   LEFT JOIN [dynamics].[ptas_year] y1
      ON b._ptas_yearbuiltid_value = y1.ptas_yearid
   LEFT JOIN [dynamics].[ptas_year] y2
      ON b._ptas_yearrenovatedid_value = y2.ptas_yearid

	
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Inserts_1ResBldg
--*********************************************************************************************************************************************************************************  

INSERT #ResBldg (
     ParcelId
	,LandGuid
	,BldgNbr
    ,NbrLivingUnits
    ,Stories
    ,BldgGrade
    ,BldgGradeVar
    ,SqFt1stFloor
    ,SqFtHalfFloor
    ,SqFt2ndFloor
    ,SqFtUpperFloor
    ,SqFtUnfinFull
    ,SqFtUnfinHalf
    ,SqFtTotLiving
    ,SqFtTotBasement
    ,SqFtFinBasement
    ,FinBasementGrade
    ,SqFtGarageBasement
    ,SqFtGarageAttached
    ,DaylightBasement
    ,Bedrooms
    ,BathHalfCount
    ,Bath3qtrCount
    ,BathFullCount
    ,FpSingleStory
    ,FpMultiStory
    ,FpFreestanding
    ,FpAdditional
    ,YrBuilt
    ,YrRenovated
    ,PcntComplete
    ,Obsolescence
    ,PcntNetCondition
    ,Condition
    ,ViewUtilization
    ,SqFtOpenPorch
    ,SqFtEnclosedPorch
    ,SqFtDeck
    ,HeatSystem
    ,HeatSource
    ,BrickStone
    ,AddnlCost
    ,BldgGuid
 )
SELECT
	 bd.ptas_buildingdetailid
	,rbd.LandGuid
	,bd.ptas_buildingnumber
	,bd.ptas_units
	,bd.ptas_numberofstoriesdecimal
	,bd.ptas_buildinggrade
	,COALESCE(bd.ptas_gradevariance,0)
	,COALESCE(bd.ptas_1stflr_sqft,0)
	,COALESCE(bd.ptas_halfflr_sqft,0)
	,COALESCE(bd.ptas_2ndflr_sqft,0)
	,COALESCE(bd.ptas_upperflr_sqft,0)
	,COALESCE(bd.ptas_unfinished_full_sqft,0)
	,COALESCE(bd.ptas_unfinished_half_sqft,0)
	,COALESCE(bd.ptas_totalliving_sqft,0)
	,COALESCE(bd.ptas_totalbsmt_sqft,0)
	,COALESCE(bd.ptas_finbsmt_sqft,0)
	,COALESCE(bd.ptas_res_basementgrade,0)
	,COALESCE(bd.ptas_basementgarage_sqft,0)
	,COALESCE(bd.ptas_attachedgarage_sqft,0)
	,CASE WHEN bd.ptas_daylightbasement = 1 THEN 'Y' ELSE '' END
	,COALESCE(bd.ptas_bedroomnbr,0)
	,COALESCE(bd.ptas_12baths,0)
	,COALESCE(bd.ptas_34baths,0)
	,COALESCE(bd.ptas_fullbathnbr,0)
	,COALESCE(bd.ptas_single_fireplace,0)
	,COALESCE(bd.ptas_multi_fireplace,0)
	,COALESCE(bd.ptas_fr_std_fireplace,0)
	,COALESCE(bd.ptas_addl_fireplace,0)
	,y1.ptas_name
	,y2.ptas_name
	,bd.ptas_percentcomplete
	,bd.ptas_buildingobsolescence
	,bd.ptas_percentnetcondition
	,bd.ptas_res_buildingcondition
	,CASE WHEN bd.ptas_viewutilizationrating IS NOT NULL  THEN 'Y' ELSE '' END
	,COALESCE(bd.ptas_openporch_sqft,0)
	,COALESCE(bd.ptas_enclosedporch_sqft,0)
	,COALESCE(bd.ptas_deck_sqft,0)
	,bd.ptas_residentialheatingsystem
	,bd.ptas_res_heatsource
	,bd.ptas_percentbrickorstone
	,bd.ptas_additionalcost
	,bd.ptas_buildingdetailid
 FROM [dynamics].[ptas_buildingdetail] bd 
INNER JOIN #SalesPop rbd 
   ON bd._ptas_parceldetailid_value = rbd.ParcelId
INNER JOIN [dynamics].[ptas_propertytype] dpt
   ON bd._ptas_propertytypeid_value = dpt.ptas_propertytypeid
  AND dpt.ptas_description = 'Residential'
 LEFT JOIN [dynamics].[ptas_year] y1
   ON bd._ptas_yearbuiltid_value = y1.ptas_yearid
 LEFT JOIN [dynamics].[ptas_year] y2
   ON bd._ptas_yearrenovatedid_value = y2.ptas_yearid

			
		
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Inserts_2
--*********************************************************************************************************************************************************************************  


	INSERT #Bldgs	
    SELECT bd._ptas_parceldetailid_value,bd.ptas_buildingdetailid
	  FROM [dynamics].[ptas_buildingdetail] bd 
     INNER JOIN #SalesPop rbd 
        ON bd._ptas_parceldetailid_value = rbd.ParcelId
     INNER JOIN [dynamics].[ptas_propertytype] dpt
        ON bd._ptas_propertytypeid_value = dpt.ptas_propertytypeid
       AND dpt.ptas_description = 'Residential'		



--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_5a
--*********************************************************************************************************************************************************************************  

	UPDATE #SalesPop
       SET ImpCnt = b.BldngCount
      FROM #SalesPop rs 
	 INNER JOIN [dynamics].[vw_ResBuildings] b 
	    ON b._ptas_parceldetailid_value = rs.ParcelId

		
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Inserts_3
--*********************************************************************************************************************************************************************************
-- identify the records we need to find accessories for. we have to do this to ensure that we are not double counting if their are multiple sales for a parcel
INSERT #IdentAccy (ParcelId)
    SELECT DISTINCT ParcelId
    FROM #SalesPop rs

-- get accessory information

--do not join to #IdentAccy, because we want to count all accessories even if Bldg exists but BldgGuid is Null (Bldgnbr = 0 on improved parcel)
INSERT INTO #Accessory(ParcelId,AccyType,Size,Grade,EffYr,PcntNetCondition,AccyValue,BldgGuid,Component,Age,Factor,RCN,RCNLD)
SELECT a._ptas_parceldetailid_value,a.ptas_resaccessorytype,a.ptas_size,a.ptas_buildinggrade,cast(isnull(y.ptas_name, '0') as int),a.ptas_percentnetcondition,a.ptas_accessoryvalue,a._ptas_buildingdetailid_value,'',0,0,0,0
  FROM dynamics.ptas_accessorydetail a INNER JOIN #IdentAccy ia ON a._ptas_parceldetailid_value = ia.ParcelId
       LEFT JOIN dynamics.ptas_year y ON a._ptas_effectiveyearid_value = y.ptas_yearid
 WHERE a.ptas_resaccessorytype in (1, 2, 3, 6, 7, 8)

--Subquery avoids following error: "An aggregate may not appear in the set list of an UPDATE statement."
INSERT #TotAccy
    SELECT ParcelId, BldgGuid, AccyType, sum(Size), max(Grade), max(EffYr), max(PcntNetCondition), sum(AccyValue)
    FROM #Accessory a (NOLOCK)
    GROUP BY ParcelId, BldgGuid, AccyType


--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6a
--*********************************************************************************************************************************************************************************
UPDATE #SalesPop
     SET DetGarArea = ta.size
   	,DetGarGrade = ta.Grade
   	,DetGarEffYr = ta.EffYr
   	,DetGarNetCond = ta.PcntNetCondition
FROM #SalesPop rs INNER JOIN #TotAccy ta ON ta.ParcelId = rs.ParcelId
WHERE (rs.BldgGuid = ta.BldgGuid OR (rs.BldgGuid IS NULL AND ta.BldgGuid IS NULL)) AND ta.accytype = 1-- 8/7/14 HNN bldgguid can be null

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6b
--*********************************************************************************************************************************************************************************
UPDATE #SalesPop
     SET CarportArea = ta.size
   	,CarportEffYr = ta.EffYr
   	,CarportNetCond = ta.PcntNetCondition
FROM #SalesPop rs INNER JOIN #TotAccy ta ON ta.ParcelId = rs.ParcelId
WHERE (rs.BldgGuid = ta.BldgGuid OR (rs.BldgGuid IS NULL AND ta.BldgGuid IS NULL)) AND ta.accytype = 2-- 8/7/14 HNN bldgguid can be null


--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6c
--*********************************************************************************************************************************************************************************
UPDATE #SalesPop
    SET Paving = ta.size
FROM #SalesPop rs INNER JOIN #TotAccy ta ON (ta.ParcelId = rs.ParcelId AND ta.accytype = 3 AND ((ta.BldgGuid IS NULL AND rs.BldgGuid IS NULL) OR ta.BldgGuid = rs.BldgGuid)) -- 8/7/14 HNN bldgguid can be null

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6d
--*********************************************************************************************************************************************************************************
UPDATE #SalesPop
     SET PoolArea = ta.size
   	,PoolEffYr = ta.EffYr
   	,PoolNetCond = ta.PcntNetCondition
FROM #SalesPop rs INNER JOIN #TotAccy ta ON (ta.ParcelId = rs.ParcelId AND ta.accytype BETWEEN 6 AND 7 AND ((ta.BldgGuid IS NULL AND rs.BldgGuid IS NULL) OR ta.BldgGuid = rs.BldgGuid)) -- 8/7/14 HNN bldgguid can be null

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6e
--*********************************************************************************************************************************************************************************
UPDATE #SalesPop
    SET MiscAccyCost = ta.AccyValue
FROM #SalesPop rs INNER JOIN #TotAccy ta ON (ta.ParcelId = rs.ParcelId AND ta.accytype = 8 AND ((ta.BldgGuid IS NULL AND rs.BldgGuid IS NULL) OR ta.BldgGuid = rs.BldgGuid)) -- 8/7/14 HNN bldgguid can be null




/*  
ACCYTYPE WITH VALUES 9, 10, 12 AND 13 WAS ELIMINATED

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6f
--*********************************************************************************************************************************************************************************
-- mobile home we want the sum of accyvalue for accytype between 9 and 10, but we want to indicate accytype 10 if both 9 and 10 exists
UPDATE #SalesPop
     SET MHomeValue = ta.AccyValue
   	,MHomeType = ta.AccyType
FROM #SalesPop rs INNER JOIN #TotAccy ta ON (ta.ParcelId = rs.ParcelId AND ta.accytype = 9 AND ((ta.BldgGuid IS NULL AND rs.BldgGuid IS NULL) OR ta.BldgGuid = rs.BldgGuid)) -- 8/7/14 HNN bldgguid can be null

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6g
--*********************************************************************************************************************************************************************************
-- by doing this 2nd, we will overwrite any accytype 9 with 10
UPDATE #SalesPop
     SET MHomeValue = MHomeValue + ta.AccyValue
   	,MHomeType = ta.AccyType
FROM #SalesPop rs INNER JOIN #TotAccy ta ON (ta.ParcelId = rs.ParcelId AND ta.accytype = 10 AND ((ta.BldgGuid IS NULL AND rs.BldgGuid IS NULL) OR ta.BldgGuid = rs.BldgGuid)) -- 8/7/14 HNN bldgguid can be null

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6h
--*********************************************************************************************************************************************************************************
UPDATE #SalesPop
    SET DevCost = ta.AccyValue
FROM #SalesPop rs INNER JOIN #TotAccy ta ON (ta.ParcelId = rs.ParcelId AND ta.accytype = 12 AND ((ta.BldgGuid IS NULL AND rs.BldgGuid IS NULL) OR ta.BldgGuid = rs.BldgGuid)) -- 8/7/14 HNN bldgguid can be null

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6i
--*********************************************************************************************************************************************************************************
UPDATE #SalesPop
    SET FlatValue = ta.AccyValue
FROM #SalesPop rs INNER JOIN #TotAccy ta ON (ta.ParcelId = rs.ParcelId AND ta.accytype = 13 AND ((ta.BldgGuid IS NULL AND rs.BldgGuid IS NULL) OR ta.BldgGuid = rs.BldgGuid)) -- 8/7/14 HNN bldgguid can be null
*/

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6j
--*********************************************************************************************************************************************************************************
UPDATE #SalesPop
     SET PermitNbr = p.ptas_permitnumber
	,PermitDate = p.ptas_issueddate
   	,PermitValue = p.ptas_permitvalue
	,PermitType = p.ptas_permittype
	,PermitDescr = p.ptas_description
FROM #SalesPop rs INNER JOIN dynamics.ptas_permit p (NOLOCK) ON (p._ptas_parcelid_value = rs.ParcelId and p.ptas_issueddate >= '1/1/' + convert(char(4), DATEPART(year, getdate()) - 4)) 
WHERE p.ptas_permitvalue = (select max(p2.ptas_permitvalue) from dynamics.ptas_permit p2 where p._ptas_parcelid_value = p2._ptas_parcelid_value)
  and p.ptas_permitstatus <> 2


--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6k
--*********************************************************************************************************************************************************************************

UPDATE #SalesPop
     SET HILastYr = hi.LastBillYr
   	,HIValue = hi.HomeImpVal
FROM #SalesPop rs INNER JOIN rp.HomeImpApp hi (NOLOCK) ON (hi.RpGuid = rs.ParcelId and hi.LastBillYr >= DATEPART(year, getdate()) - 3) 
WHERE  hi.HomeImpVal = (select max(hi2.HomeImpVal) from rp.HomeImpApp hi2 where hi.RpGuid = hi2.RpGuid)


--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6l
--*********************************************************************************************************************************************************************************
UPDATE #SalesPop
     SET SelectDate = ah.ptas_appraiserdate
   	,SelectAppr = su.ptas_legacyid
   	,SelectMethod = dynamics.fn_GetValueFromStringMap('ptas_appraisalhistory', 'ptas_method', ah.ptas_methodid)
   	,SelectReason = dynamics.fn_GetValueFromStringMap('ptas_appraisalhistory', 'ptas_valuationreason', ah.ptas_valuationreasonid)
   	,SelectLandVal = ah.ptas_landvalue
   	,SelectImpsVal = ah.ptas_impvalue
    ,MFInterfaceFlag = ah.ptas_interfaceflag
    ,NewConstrVal = ah.ptas_newconstruction	
  FROM #SalesPop rs INNER JOIN ptas.ptas_appraisalhistory ah (NOLOCK) ON (ah.ptas_parcelid = rs.ParcelId and ah.ptas_name = @AssmtYr + 1 and ah.ptas_revalormaint = 'R')   --rollyr is not found in ptas_appraisalhistory so I used the ptas_appraiserdate
  LEFT JOIN [dynamics].[systemuser] su	 ON ah.ptas_appr = su.systemuserid


/*
Missing table 
Hairo comment: I need this table for the current calculation
Ask which will be the GisSurface_V table
As per Regis email for now we have to ignore this table.

UPDATE #SalesPop
     SET GisSurfaceValue =ISNULL(g.SurfaceValue,0)
FROM #SalesPop rs INNER JOIN GisSurface_V g (NOLOCK) ON rs.ParcelId = g.RealPropId
WHERE g.AssmtYr = @AssmtYr
*/

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_6m
--*********************************************************************************************************************************************************************************
-- set the select date to the maintenance values if it was selected after the revalue values
UPDATE #SalesPop
     SET SelectDate = ah.ptas_appraiserdate
   	,SelectAppr = su.ptas_legacyid
   	,SelectMethod = dynamics.fn_GetValueFromStringMap('ptas_appraisalhistory', 'ptas_method', ah.ptas_methodid)
   	,SelectReason = dynamics.fn_GetValueFromStringMap('ptas_appraisalhistory', 'ptas_valuationreason', ah.ptas_valuationreasonid)
  	,SelectLandVal = ah.ptas_landvalue
   	,SelectImpsVal = ah.ptas_impvalue
FROM #SalesPop rs 
     INNER JOIN ptas.ptas_appraisalhistory ah (NOLOCK) ON (ah.ptas_parcelid = rs.ParcelId and ah.ptas_name = @AssmtYr + 1 and ah.ptas_revalormaint = 'M')  --rollyr is not found in ptas_appraisalhistory so I used the ptas_appraiserdate
		  AND (ah.ptas_appraiserdate > rs.SelectDate OR rs.SelectDate IS NULL)
	 LEFT JOIN [dynamics].[systemuser] su	 ON ah.ptas_appr = su.systemuserid


--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Inserts_4
--*********************************************************************************************************************************************************************************  


/*
Hairo comment : In the original query is used table "ChngHist" and the data is filtered by "EventTypeItemId = 4" but 
				in the current table there is no similar column to filter 
*/
  INSERT #Events
  SELECT e.parcelGuid, max(e.EventDate)
    FROM #RealProp r 
   INNER JOIN [ptas].[ptas_changehistory] e (NOLOCK) 
      ON e.parcelGuid = r.ParcelId
	 AND e.EventDate >= '1/1/' + convert(char(4), DATEPART(year, getdate()) - 3)
    GROUP by e.parcelGuid

	
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_7a
--*********************************************************************************************************************************************************************************  

 UPDATE #SalesPop
    SET SegMergeDate = e.EventDate
   FROM #SalesPop rs 
  INNER JOIN #Events e 
     ON e.ParcelId = rs.ParcelId



--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_7b
--*********************************************************************************************************************************************************************************  
  UPDATE  #SalesPop
     SET StreetNbr = bd.ptas_addr1_streetnumber
       	 ,NbrFraction = bd.ptas_addr1_streetnumberfraction
      	 ,DirPrefix = [dynamics].[fn_GetValueFromStringMap]('ptas_buildingdetail','ptas_addr1_directionprefix', bd.ptas_addr1_directionprefix) 
       	 ,StreetName = sn.ptas_name
       	 ,StreetType = st.ptas_name
       	 ,DirSuffix = [dynamics].[fn_GetValueFromStringMap]('ptas_buildingdetail','ptas_addr1_directionsuffix', bd.ptas_addr1_directionsuffix) 
	  FROM #SalesPop rs
	 INNER JOIN [dynamics].[ptas_buildingdetail] bd 
	    ON rs.ParcelId = bd._ptas_parceldetailid_value
     INNER JOIN [dynamics].[ptas_propertytype] dpt
        ON bd._ptas_propertytypeid_value = dpt.ptas_propertytypeid
       AND dpt.ptas_description = 'Residential'	
	 INNER JOIN [dynamics].[ptas_streetname] sn
	    ON sn.ptas_streetnameid = bd._ptas_addr1_streetnameid_value
	 INNER JOIN [dynamics].[ptas_streettype] st
		ON st.ptas_streettypeid = bd._ptas_addr1_streettypeid_value
		
   UPDATE   #SalesPop
      SET StreetNbr = dpd.ptas_addr1_streetnumber
	       ,NbrFraction = dpd.ptas_addr1_streetnumberfraction
 		   ,DirPrefix = [dynamics].[fn_GetValueFromStringMap]('ptas_parceldetail','ptas_addr1_directionprefix', dpd.ptas_addr1_directionprefix)
		   ,StreetName = sn.ptas_name
		   ,StreetType = st.ptas_name
		   ,DirSuffix = [dynamics].[fn_GetValueFromStringMap]('ptas_parceldetail','ptas_addr1_directionsuffix', dpd.ptas_addr1_directionsuffix) 
	  FROM #SalesPop rs
     INNER JOIN [dynamics].[ptas_parceldetail] dpd 
	    ON rs.ParcelId = dpd.ptas_parceldetailid
	 INNER JOIN [dynamics].[ptas_streetname] sn
	    ON sn.ptas_streetnameid = dpd._ptas_addr1_streetnameid_value
	 INNER JOIN [dynamics].[ptas_streettype] st
		ON st.ptas_streettypeid = dpd._ptas_addr1_streettypeid_value
     WHERE rs.BldgGuid IS NULL
   

/************************************************************************************************************************************************
                                            BLDGRCN & BLDGRCNLD
											
											BLDGRCN - it is the building cost of replacement

											BLDGRNLD -- it is the building cost of replacement LESS depreciation
*************************************************************************************************************************************************/


SET @TestProd = 'P'

SET @yrMultiplier = (	SELECT Multiplier	 
						FROM rp.ResCostMultiplier (NOLOCK)
						WHERE CONVERT(int,ResArea) = 0 AND RollYr = @CostBillYr AND TestProd = @TestProd )
 
SET @AreaMultiplier = (	SELECT Multiplier
						FROM rp.ResCostMultiplier (NOLOCK)
                                 -- All multipliers are 1 except yrMultiplier which use resarea 000
                                 -- If @Area is null, it means we are searching for an appr's value select which they wouldn't specify an area
						WHERE CONVERT(int,ResArea) = isnull(@Area,1) AND RollYr = @CostBillYr AND TestProd = @TestProd )

SET @CostIndex = @yrMultiplier * @areaMultiplier    


--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_8a
--*********************************************************************************************************************************************************************************  

  UPDATE #ResBldg
     SET UnitsAdj = CASE NbrLivingUnits
						 WHEN 3 THEN 1.25
						 WHEN 2 THEN 1.12
						 ELSE 1
					END
  	     ,Age       = CASE WHEN YrRenovated > 0 THEN  CASE WHEN @CostBillYr - 1 - (YrRenovated - 5) < 0 THEN 0
                	                                       ELSE @CostBillYr - 1 - (YrRenovated - 5)
                 	                                  END
              	           WHEN YrBuilt > 0 THEN CASE WHEN @CostBillYr - 1 - YrBuilt < 0 THEN 0
                        	                          ELSE @CostBillYr - 1 - YrBuilt
                 	                                  END
              	           ELSE 0
         	          END
      	,BldgGradeCateg = CASE WHEN BldgGrade < 5 THEN 1
                       	       WHEN BldgGrade > 7 THEN 3
                               ELSE 2
                          END
      	,BsmtGradeCateg = CASE WHEN FinBasementGrade = 0 THEN CASE WHEN BldgGrade < 5 THEN 1
      													       WHEN BldgGrade > 7 THEN 3
      														   ELSE 2
      													  END
      					   WHEN FinBasementGrade < 5 THEN 1
                               WHEN FinBasementGrade > 7 THEN 3
                               ELSE 2
                          END

       
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_8b
--*********************************************************************************************************************************************************************************  


 UPDATE  #ResBldg
	SET  Bldgformula = FormulaType
	    ,Bldgfactor1 = Factor1
    	,Bldgfactor2 = Factor2
   FROM #ResBldg rs, rp.ResDeprSpec rds
  WHERE rs.BldgGradeCateg = rds.GradeCateg
  	AND rs.Condition = rds.Condition
   	AND rs.Age BETWEEN rds.AgeFrom AND rds.AgeTo
   	AND rds.RollYr = @CostBillYr
   	AND rds.TestProd = @TestProd

	
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_8c
--*********************************************************************************************************************************************************************************  


UPDATE #ResBldg
	Set Bsmtformula = FormulaType
   	,Bsmtfactor1 = Factor1
   	,Bsmtfactor2 = Factor2
FROM #ResBldg rs, rp.ResDeprSpec rds
WHERE rs.BsmtGradeCateg = rds.GradeCateg
	AND rs.Condition = rds.Condition
  	AND rs.Age BETWEEN rds.AgeFrom AND rds.AgeTo
  	AND rds.RollYr = @CostBillYr
  	AND rds.TestProd = @TestProd


--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_8d
--*********************************************************************************************************************************************************************************  

UPDATE #ResBldg
     Set PcntGoodBldg = CASE BldgFormula
							WHEN 1 THEN Bldgfactor1 / 100
							WHEN 2 THEN (Bldgfactor1 - age) / 100
							WHEN 3 THEN (Bldgfactor1 * age + Bldgfactor2) / 100
							WHEN 4 THEN EXP(Bldgfactor1 * age + Bldgfactor2) / 100
							ELSE 0
						END
   	,PcntGoodBsmt = CASE BsmtFormula
							WHEN 1 THEN Bsmtfactor1 / 100
							WHEN 2 THEN (Bsmtfactor1 - age) / 100
							WHEN 3 THEN (Bsmtfactor1 * age + Bsmtfactor2) / 100
							WHEN 4 THEN EXP(Bsmtfactor1 * age + Bsmtfactor2) / 100
                            ELSE 0
					END

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_8e
--*********************************************************************************************************************************************************************************  

UPDATE #ResBldg
   SET bldgGradeAdj = CASE WHEN BldgGrade = 13 AND BldgGradeVar > 0 THEN 0.01 * BldgGradeVar ELSE 0 END

/*####################################ASSR_R_GetAreaData2b_Inserts_5 (BEGIN)###############################################*/


INSERT #ResCostBldgComponent
    SELECT
	 SUM(rcc.FixedCost * rccp.Bath3qtrCountFixedCost) Bath3qtrCountFixedCost,SUM(rcc.VariableCost * rccp.Bath3qtrCountVariableCost) Bath3qtrCountVariableCost                                                                           ,SUM(rcc.MinArea * rccp.Bath3qtrCountMinArea) Bath3qtrCountMinArea
	,SUM(rcc.FixedCost * rccp.BathFullCountFixedCost) BathFullCountFixedCost,SUM(rcc.VariableCost * rccp.BathFullCountVariableCost) BathFullCountVariableCost                                                                           ,SUM(rcc.MinArea * rccp.BathFullCountMinArea) BathFullCountMinArea
	,SUM(rcc.FixedCost * rccp.BathHalfCountFixedCost) BathHalfCountFixedCost,SUM(rcc.VariableCost * rccp.BathHalfCountVariableCost) BathHalfCountVariableCost                                                                           ,SUM(rcc.MinArea * rccp.BathHalfCountMinArea) BathHalfCountMinArea
	,SUM(rcc.FixedCost * rccp.BathTotalAdjustFixedCost) BathTotalAdjustFixedCost,SUM(rcc.VariableCost * rccp.BathTotalAdjustVariableCost) BathTotalAdjustVariableCost                                                                       ,SUM(rcc.MinArea * rccp.BathTotalAdjustMinArea) BathTotalAdjustMinArea
	,SUM(rcc.FixedCost * rccp.EffBrickStoneAreaFixedCost) EffBrickStoneAreaFixedCost,SUM(rcc.VariableCost * rccp.EffBrickStoneAreaVariableCost) EffBrickStoneAreaVariableCost                                                                   ,SUM(rcc.MinArea * rccp.EffBrickStoneAreaMinArea) EffBrickStoneAreaMinArea
	,SUM(rcc.FixedCost * rccp.FpAdditionalFixedCost) FpAdditionalFixedCost,SUM(rcc.VariableCost * rccp.FpAdditionalVariableCost) FpAdditionalVariableCost                                                                             ,SUM(rcc.MinArea * rccp.FpAdditionalMinArea) FpAdditionalMinArea
	,SUM(rcc.FixedCost * rccp.FpFreestandingFixedCost) FpFreestandingFixedCost,SUM(rcc.VariableCost * rccp.FpFreestandingVariableCost) FpFreestandingVariableCost                                                                         ,SUM(rcc.MinArea * rccp.FpFreestandingMinArea) FpFreestandingMinArea
	,SUM(rcc.FixedCost * rccp.FpMultiStoryFixedCost) FpMultiStoryFixedCost,SUM(rcc.VariableCost * rccp.FpMultiStoryVariableCost) FpMultiStoryVariableCost                                                                             ,SUM(rcc.MinArea * rccp.FpMultiStoryMinArea) FpMultiStoryMinArea
	,SUM(rcc.FixedCost * rccp.FpSingleStoryFixedCost) FpSingleStoryFixedCost,SUM(rcc.VariableCost * rccp.FpSingleStoryVariableCost) FpSingleStoryVariableCost                                                                           ,SUM(rcc.MinArea * rccp.FpSingleStoryMinArea) FpSingleStoryMinArea
	,SUM(rcc.FixedCost * rccp.SqFt1stFloorFixedCost) SqFt1stFloorFixedCost,SUM(rcc.VariableCost * rccp.SqFt1stFloorVariableCost) SqFt1stFloorVariableCost                                                                             ,SUM(rcc.MinArea * rccp.SqFt1stFloorMinArea) SqFt1stFloorMinArea
	,SUM(rcc.FixedCost * rccp.SqFt2ndFloorFixedCost) SqFt2ndFloorFixedCost,SUM(rcc.VariableCost * rccp.SqFt2ndFloorVariableCost) SqFt2ndFloorVariableCost                                                                             ,SUM(rcc.MinArea * rccp.SqFt2ndFloorMinArea) SqFt2ndFloorMinArea
	,SUM(rcc.FixedCost * rccp.SqFtUpperFloorFixedCost) SqFtUpperFloorFixedCost,SUM(rcc.VariableCost * rccp.SqFtUpperFloorVariableCost) SqFtUpperFloorVariableCost                                                                         ,SUM(rcc.MinArea * rccp.SqFtUpperFloorMinArea) SqFtUpperFloorMinArea
	,SUM(rcc.FixedCost * rccp.SqFtDeckFixedCost) SqFtDeckFixedCost,SUM(rcc.VariableCost * rccp.SqFtDeckVariableCost) SqFtDeckVariableCost                                                                                     ,SUM(rcc.MinArea * rccp.SqFtDeckMinArea) SqFtDeckMinArea
	,SUM(rcc.FixedCost * rccp.SqFtElectricBBHeatedFixedCost) SqFtElectricBBHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtElectricBBHeatedVariableCost) SqFtElectricBBHeatedVariableCost                                                             ,SUM(rcc.MinArea * rccp.SqFtElectricBBHeatedMinArea) SqFtElectricBBHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtEnclosedPorchFixedCost) SqFtEnclosedPorchFixedCost,SUM(rcc.VariableCost * rccp.SqFtEnclosedPorchVariableCost) SqFtEnclosedPorchVariableCost                                                                   ,SUM(rcc.MinArea * rccp.SqFtEnclosedPorchMinArea) SqFtEnclosedPorchMinArea
	,SUM(rcc.FixedCost * rccp.SqFtFinBasementFixedCost) SqFtFinBasementFixedCost,SUM(rcc.VariableCost * rccp.SqFtFinBasementVariableCost) SqFtFinBasementVariableCost                                                                       ,SUM(rcc.MinArea * rccp.SqFtFinBasementMinArea) SqFtFinBasementMinArea
	,SUM(rcc.FixedCost * rccp.SqFtForcedAirHeatedFixedCost) SqFtForcedAirHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtForcedAirHeatedVariableCost) SqFtForcedAirHeatedVariableCost                                                               ,SUM(rcc.MinArea * rccp.SqFtForcedAirHeatedMinArea) SqFtForcedAirHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtFWFurnaceHeatedFixedCost) SqFtFWFurnaceHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtFWFurnaceHeatedVariableCost) SqFtFWFurnaceHeatedVariableCost                                                               ,SUM(rcc.MinArea * rccp.SqFtFWFurnaceHeatedMinArea) SqFtFWFurnaceHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtGarageAttachedFixedCost) SqFtGarageAttachedFixedCost,SUM(rcc.VariableCost * rccp.SqFtGarageAttachedVariableCost) SqFtGarageAttachedVariableCost                                                                 ,SUM(rcc.MinArea * rccp.SqFtGarageAttachedMinArea) SqFtGarageAttachedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtGravityHeatedFixedCost) SqFtGravityHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtGravityHeatedVariableCost) SqFtGravityHeatedVariableCost                                                                   ,SUM(rcc.MinArea * rccp.SqFtGravityHeatedMinArea) SqFtGravityHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtHalfFloorFixedCost) SqFtHalfFloorFixedCost,SUM(rcc.VariableCost * rccp.SqFtHalfFloorVariableCost) SqFtHalfFloorVariableCost                                                                           ,SUM(rcc.MinArea * rccp.SqFtHalfFloorMinArea) SqFtHalfFloorMinArea
	,SUM(rcc.FixedCost * rccp.SqFtHeatPumpHeatedFixedCost) SqFtHeatPumpHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtHeatPumpHeatedVariableCost) SqFtHeatPumpHeatedVariableCost                                                                 ,SUM(rcc.MinArea * rccp.SqFtHeatPumpHeatedMinArea) SqFtHeatPumpHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtHotWaterHeatedFixedCost) SqFtHotWaterHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtHotWaterHeatedVariableCost) SqFtHotWaterHeatedVariableCost                                                                 ,SUM(rcc.MinArea * rccp.SqFtHotWaterHeatedMinArea) SqFtHotWaterHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtOpenPorchFixedCost) SqFtOpenPorchFixedCost,SUM(rcc.VariableCost * rccp.SqFtOpenPorchVariableCost) SqFtOpenPorchVariableCost                                                                           ,SUM(rcc.MinArea * rccp.SqFtOpenPorchMinArea) SqFtOpenPorchMinArea
	,SUM(rcc.FixedCost * rccp.SqFtRadiantHeatedFixedCost) SqFtRadiantHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtRadiantHeatedVariableCost) SqFtRadiantHeatedVariableCost                                                                   ,SUM(rcc.MinArea * rccp.SqFtRadiantHeatedMinArea) SqFtRadiantHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtTotBasementFixedCost) SqFtTotBasementFixedCost,SUM(rcc.VariableCost * rccp.SqFtTotBasementVariableCost) SqFtTotBasementVariableCost                                                                       ,SUM(rcc.MinArea * rccp.SqFtTotBasementMinArea) SqFtTotBasementMinArea
	,SUM(rcc.FixedCost * rccp.SqFtUnfinFullFixedCost) SqFtUnfinFullFixedCost,SUM(rcc.VariableCost * rccp.SqFtUnfinFullVariableCost) SqFtUnfinFullVariableCost                                                                           ,SUM(rcc.MinArea * rccp.SqFtUnfinFullMinArea) SqFtUnfinFullMinArea
	,SUM(rcc.FixedCost * rccp.SqFtUnfinHalfFixedCost) SqFtUnfinHalfFixedCost,SUM(rcc.VariableCost * rccp.SqFtUnfinHalfVariableCost) SqFtUnfinHalfVariableCost                                                                           ,SUM(rcc.MinArea * rccp.SqFtUnfinHalfMinArea) SqFtUnfinHalfMinArea
	,SUM(rcc.FixedCost * rccp.SqFtUnheatedLivingFixedCost) SqFtUnheatedLivingFixedCost,SUM(rcc.VariableCost * rccp.SqFtUnheatedLivingVariableCost) SqFtUnheatedLivingVariableCost                                                                 ,SUM(rcc.MinArea * rccp.SqFtUnheatedLivingMinArea) SqFtUnheatedLivingMinArea
    FROM rp.rescostcomponent rcc (NOLOCK) 
	INNER JOIN rp.rescostBldgcomponent_p rccp(NOLOCK) ON (rcc.component = rccp.component)
    WHERE rcc.rollyr = @CostBillYr AND rcc.testprod = @TestProd AND rcc.bldgaccy = 'b'

	
INSERT #ResCostBldgGradeFactor
    SELECT  
	Grade
	,sum(rcgf.GradeFactor * rcgfp.Bath3qtrCount)Bath3qtrCount
	,sum(rcgf.GradeFactor * rcgfp.BathFullCount)BathFullCount
	,sum(rcgf.GradeFactor * rcgfp.BathHalfCount)BathHalfCount
	,sum(rcgf.GradeFactor * rcgfp.BathTotalAdjust)BathTotalAdjust
	,sum(rcgf.GradeFactor * rcgfp.EffBrickStoneArea)EffBrickStoneArea
	,sum(rcgf.GradeFactor * rcgfp.FpAdditional)FpAdditional
	,sum(rcgf.GradeFactor * rcgfp.FpFreestanding)FpFreestanding
	,sum(rcgf.GradeFactor * rcgfp.FpMultiStory)FpMultiStory
	,sum(rcgf.GradeFactor * rcgfp.FpSingleStory)FpSingleStory
	,sum(rcgf.GradeFactor * rcgfp.SqFt1stFloor)SqFt1stFloor
	,sum(rcgf.GradeFactor * rcgfp.SqFt2ndFloor)SqFt2ndFloor
	,sum(rcgf.GradeFactor * rcgfp.SqFtUpperFloor)SqFtUpperFloor
	,sum(rcgf.GradeFactor * rcgfp.SqFtDeck)SqFtDeck
	,sum(rcgf.GradeFactor * rcgfp.SqFtElectricBBHeated)SqFtElectricBBHeated
	,sum(rcgf.GradeFactor * rcgfp.SqFtEnclosedPorch)SqFtEnclosedPorch
	,sum(rcgf.GradeFactor * rcgfp.SqFtFinBasement)SqFtFinBasement
	,sum(rcgf.GradeFactor * rcgfp.SqFtForcedAirHeated)SqFtForcedAirHeated
	,sum(rcgf.GradeFactor * rcgfp.SqFtFWFurnaceHeated)SqFtFWFurnaceHeated
	,sum(rcgf.GradeFactor * rcgfp.SqFtGarageAttached)SqFtGarageAttached
	,sum(rcgf.GradeFactor * rcgfp.SqFtGravityHeated)SqFtGravityHeated
	,sum(rcgf.GradeFactor * rcgfp.SqFtHalfFloor)SqFtHalfFloor
	,sum(rcgf.GradeFactor * rcgfp.SqFtHeatPumpHeated)SqFtHeatPumpHeated
	,sum(rcgf.GradeFactor * rcgfp.SqFtHotWaterHeated)SqFtHotWaterHeated
	,sum(rcgf.GradeFactor * rcgfp.SqFtOpenPorch)SqFtOpenPorch
	,sum(rcgf.GradeFactor * rcgfp.SqFtRadiantHeated)SqFtRadiantHeated
	,sum(rcgf.GradeFactor * rcgfp.SqFtTotBasement)SqFtTotBasement
	,sum(rcgf.GradeFactor * rcgfp.SqFtUnfinFull)SqFtUnfinFull
	,sum(rcgf.GradeFactor * rcgfp.SqFtUnfinHalf)SqFtUnfinHalf
	,sum(rcgf.GradeFactor * rcgfp.SqFtUnheatedLiving)SqFtUnheatedLiving
    FROM rp.rescostcomponent rcc, rp.rescostgradefactor rcgf, rp.ResCostBldgGradeFactor_P rcgfp where rcc.rollyr=@CostBillYr AND rcc.testprod = @TestProd AND rcc.bldgaccy='b'
	AND rcc.costid = rcgf.costid AND rcc.component = rcgfp.component 
    GROUP BY rcgf.grade

/*####################################ASSR_R_GetAreaData2b_Inserts_5 (END)###############################################*/

-- ALL THIS CONVERSION TO INT IS TO MAINTAIN THE SAME ROUNDING AS THE ORIGINAL SP CalcResBldgCost & CalcResAccyCost
--*********************************************************************************************************************************************************************************
-- BEGIN of ASSR_R_GetAreaData2b_Updates_9a
--*********************************************************************************************************************************************************************************
UPDATE #ResBldg
	Set BldgRCN =  -- Bathrooms
            FLOOR((FLOOR((FLOOR(Bath3qtrCountfixedCost * rs.Bath3qtrCount/(CASE WHEN rs.Bath3QtrCount = 0 THEN 1 
                                                       ELSE rs.Bath3QtrCount 
                                                       END) + Bath3qtrCountvariableCost * rs.Bath3qtrCount * @CostIndex)

          + FLOOR(BathFullCountfixedCost * rs.BathFullCount/(CASE WHEN rs.BathFullCount = 0 THEN 1 
                                                       ELSE rs.BathFullCount 
                                                       END) + BathFullCountvariableCost * rs.BathFullCount * @CostIndex)
          + FLOOR(BathHalfCountfixedCost * rs.BathHalfCount/(CASE WHEN rs.BathHalfCount = 0 THEN 1 
                                                       ELSE rs.BathHalfCount 
                                                       END) + BathHalfCountvariableCost * rs.BathHalfCount * @CostIndex)) * rcgf.BathFullCount
          + rs.NbrLivingUnits * rcgf.BathTotalAdjust * @CostIndex)


          -- BrickStone
          + FLOOR((rs.SqFt1stFloor * 0.64 + 560 * rs.SqFt1stFloor/(CASE WHEN rs.SqFt1stFloor = 0 THEN 1 ELSE rs.SqFt1stFloor END)
          + rs.SqFt2ndFloor * 0.3912 + 513.45 * rs.SqFt2ndFloor/(CASE WHEN rs.SqFt2ndFloor = 0 THEN 1 ELSE rs.SqFt2ndFloor END)
          + rs.SqFtHalfFloor * 0.1208 - 19.63 * rs.SqFtHalfFloor/(CASE WHEN rs.SqFtHalfFloor = 0 THEN 1 ELSE rs.SqFtHalfFloor END)) * rs.BrickStone / 100.00 * @CostIndex * rcgf.EffBrickStoneArea)

          -- Fireplace
          + FLOOR((FpAdditionalfixedCost / (CASE WHEN rs.FpAdditional = 0 THEN 1 -- we don't want to divide by zero. The multiplication of FpAdditional will zero out this section anyway
                                           WHEN rs.FpAdditional > rcc.FpAdditionalMinArea THEN rs.FpAdditional 
                                           ELSE rcc.FpAdditionalMinArea 
                                      END) + FpAdditionalVariableCost) * rs.FpAdditional * @CostIndex * rcgf.FpAdditional)
          + FLOOR((FpFreestandingfixedCost / (CASE WHEN rs.FpFreestanding = 0 THEN 1
                                             WHEN rs.FpFreestanding > rcc.FpFreestandingMinArea THEN rs.FpFreestanding 
                                             ELSE rcc.FpFreestandingMinArea 
                                        END) + FpFreestandingVariableCost) * rs.FpFreestanding * @CostIndex * rcgf.FpFreestanding)
          + FLOOR((FpMultiStoryfixedCost / (CASE WHEN rs.FpMultiStory = 0 THEN 1
                                           WHEN rs.FpMultiStory > rcc.FpMultiStoryMinArea THEN rs.FpMultiStory 
                                           ELSE rcc.FpMultiStoryMinArea 
                                      END) + FpMultiStoryVariableCost) * rs.FpMultiStory * @CostIndex * rcgf.FpMultiStory)
          + FLOOR((FpSingleStoryfixedCost / (CASE WHEN rs.FpSingleStory = 0 THEN 1
                                            WHEN rs.FpSingleStory > rcc.FpSingleStoryMinArea THEN rs.FpSingleStory 
                                            ELSE rcc.FpSingleStoryMinArea 
                                       END) + FpSingleStoryVariableCost) * rs.FpSingleStory * @CostIndex * rcgf.FpSingleStory)

          -- decks/porches
          + FLOOR((SqFtDeckfixedCost / (CASE WHEN rs.SqFtDeck = 0 THEN 1
                                       WHEN rs.SqFtDeck > rcc.SqFtDeckMinArea THEN rs.SqFtDeck 
                                       ELSE rcc.SqFtDeckMinArea 
                                  END) + SqFtDeckVariableCost) * rs.SqFtDeck * @CostIndex * rcgf.SqFtDeck)
          + FLOOR((SqFtEnclosedPorchfixedCost / (CASE WHEN rs.SqFtEnclosedPorch = 0 THEN 1
                                                WHEN rs.SqFtEnclosedPorch > rcc.SqFtEnclosedPorchMinArea THEN rs.SqFtEnclosedPorch 
                                                ELSE rcc.SqFtEnclosedPorchMinArea 
                                           END) + SqFtEnclosedPorchVariableCost) * rs.SqFtEnclosedPorch * @CostIndex * rcgf.SqFtEnclosedPorch)
          + FLOOR((SqFtOpenPorchfixedCost / (CASE WHEN rs.SqFtOpenPorch = 0 THEN 1
                                            WHEN rs.SqFtOpenPorch > rcc.SqFtOpenPorchMinArea THEN rs.SqFtOpenPorch 
                                            ELSE rcc.SqFtOpenPorchMinArea 
                                       END) + SqFtOpenPorchVariableCost) * rs.SqFtOpenPorch * @CostIndex * rcgf.SqFtOpenPorch)

          -- basements
          + ISNULL(FLOOR((SqFtFinBasementfixedCost / (CASE WHEN rs.SqFtFinBasement = 0 THEN 1
                                              WHEN rs.SqFtFinBasement > rcc.SqFtFinBasementMinArea THEN rs.SqFtFinBasement 
                                              ELSE rcc.SqFtFinBasementMinArea 
                                              END) + SqFtFinBasementVariableCost) * rs.SqFtFinBasement * @CostIndex * rcgfbsmt.SqFtFinBasement),0)
          + ISNULL(FLOOR((SqFtTotBasementfixedCost / (CASE WHEN rs.SqFtTotBasement = 0 THEN 1
                                              WHEN rs.SqFtTotBasement > rcc.SqFtTotBasementMinArea THEN rs.SqFtTotBasement 
                                              ELSE rcc.SqFtTotBasementMinArea 
                                         END) + SqFtTotBasementVariableCost) * rs.SqFtTotBasement * @CostIndex * rcgf.SqFtTotBasement),0)

          -- garage
          + FLOOR((SqFtGarageAttachedfixedCost / (CASE WHEN rs.SqFtGarageAttached = 0 THEN 1
                                                 WHEN rs.SqFtGarageAttached > rcc.SqFtGarageAttachedMinArea THEN rs.SqFtGarageAttached 
                                                 ELSE rcc.SqFtGarageAttachedMinArea 
                                            END) + SqFtGarageAttachedVariableCost) * rs.SqFtGarageAttached * @CostIndex * rcgf.SqFtGarageAttached)

          -- heat
          + FLOOR((SqFtUnheatedLivingfixedCost / (CASE WHEN rs.SqFtTotLiving = 0 THEN 1
                                                 WHEN rs.SqFtTotLiving > rcc.SqFtUnheatedLivingMinArea THEN rs.SqFtTotLiving 
                                                 ELSE rcc.SqFtUnheatedLivingMinArea 
                                            END) + SqFtUnheatedLivingVariableCost) * rs.SqFtTotLiving * @CostIndex * rcgf.SqFtUnheatedLiving * (CASE WHEN rs.HeatSystem = 0 THEN 1 ELSE 0 END)
          + (SqFtFWFurnaceHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtFWFurnaceHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtFWFurnaceHeatedMinArea END) + SqFtFWFurnaceHeatedVariableCost) * rs.SqFtTotLiving * @CostIndex * rcgf.SqFtFWFurnaceHeated * (CASE WHEN rs.HeatSystem = 1 THEN 1 ELSE 0 END)
          + (SqFtGravityHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtGravityHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtGravityHeatedMinArea END) + SqFtGravityHeatedVariableCost) * rs.SqFtTotLiving * @CostIndex * rcgf.SqFtGravityHeated * (CASE WHEN rs.HeatSystem = 2 THEN 1 ELSE 0 END)
          + (SqFtRadiantHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtRadiantHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtRadiantHeatedMinArea END) + SqFtRadiantHeatedVariableCost) * rs.SqFtTotLiving * @CostIndex * rcgf.SqFtRadiantHeated * (CASE WHEN rs.HeatSystem = 3 THEN 1 ELSE 0 END)
          + (SqFtElectricBBHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtElectricBBHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtElectricBBHeatedMinArea END) + SqFtElectricBBHeatedVariableCost) * rs.SqFtTotLiving * @CostIndex * rcgf.SqFtElectricBBHeated * (CASE WHEN rs.HeatSystem = 4 THEN 1 ELSE 0 END)
          + (SqFtForcedAirHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtForcedAirHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtForcedAirHeatedMinArea END) + SqFtForcedAirHeatedVariableCost) * rs.SqFtTotLiving * @CostIndex * rcgf.SqFtForcedAirHeated * (CASE WHEN rs.HeatSystem = 5 THEN 1 ELSE 0 END)
          + (SqFtHotWaterHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtHotWaterHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtHotWaterHeatedMinArea END) + SqFtHotWaterHeatedVariableCost) * rs.SqFtTotLiving * @CostIndex * rcgf.SqFtHotWaterHeated * (CASE WHEN rs.HeatSystem = 6 THEN 1 ELSE 0 END)
          + (SqFtHeatPumpHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtHeatPumpHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtHeatPumpHeatedMinArea END) + SqFtHeatPumpHeatedVariableCost) * rs.SqFtTotLiving * @CostIndex * rcgf.SqFtHeatPumpHeated * (CASE WHEN rs.HeatSystem = 7 THEN 1 ELSE 0 END))

          -- SqFt
          + FLOOR((SqFt1stFloorfixedCost / (CASE WHEN rs.SqFt1stFloor > rcc.SqFt1stFloorMinArea THEN rs.SqFt1stFloor ELSE rcc.SqFt1stFloorMinArea END) + SqFt1stFloorVariableCost) * rs.SqFt1stFloor * @CostIndex * (rcgf.SqFt1stFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFt2ndFloorfixedCost / (CASE WHEN rs.SqFt2ndFloor > rcc.SqFt2ndFloorMinArea THEN rs.SqFt2ndFloor ELSE rcc.SqFt2ndFloorMinArea END) + SqFt2ndFloorVariableCost) * rs.SqFt2ndFloor * @CostIndex * (rcgf.SqFt2ndFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtUpperFloorfixedCost / (CASE WHEN rs.SqFtUpperFloor > rcc.SqFtUpperFloorMinArea THEN rs.SqFtUpperFloor ELSE rcc.SqFtUpperFloorMinArea END) + SqFtUpperFloorVariableCost) * rs.SqFtUpperFloor * @CostIndex * (rcgf.SqFtUpperFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtHalfFloorfixedCost / (CASE WHEN rs.SqFtHalfFloor > rcc.SqFtHalfFloorMinArea THEN rs.SqFtHalfFloor ELSE rcc.SqFtHalfFloorMinArea END) + SqFtHalfFloorVariableCost) * rs.SqFtHalfFloor * @CostIndex * (rcgf.SqFtHalfFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtUnfinFullfixedCost / (CASE WHEN rs.SqFtUnfinFull > rcc.SqFtUnfinFullMinArea THEN rs.SqFtUnfinFull ELSE rcc.SqFtUnfinFullMinArea END) + SqFtUnfinFullVariableCost) * rs.SqFtUnfinFull * @CostIndex * (rcgf.SqFtUnfinFull + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtUnfinHalffixedCost / (CASE WHEN rs.SqFtUnfinHalf > rcc.SqFtUnfinHalfMinArea THEN rs.SqFtUnfinHalf ELSE rcc.SqFtUnfinHalfMinArea END) + SqFtUnfinHalfVariableCost) * rs.SqFtUnfinHalf * @CostIndex * (rcgf.SqFtUnfinHalf + bldgGradeAdj) * unitsAdj)

          -- Additional Cost
          + FLOOR(rs.AddnlCost * @CostIndex)) / 100) * 100

   	,finBsmtCost = ISNULL((SqFtFinBasementfixedCost / (CASE WHEN rs.SqFtFinBasement > rcc.SqFtFinBasementMinArea THEN rs.SqFtFinBasement ELSE rcc.SqFtFinBasementMinArea END) + SqFtFinBasementVariableCost) * rs.SqFtFinBasement * @CostIndex * rcgfbsmt.SqFtFinBasement,0)
	FROM #ResBldg rs inner join #ResCostBldgComponent rcc on (1=1)
                  inner join #ResCostBldgGradeFactor rcgf on (rs.BldgGrade = rcgf.Grade)
                  left join #ResCostBldgGradeFactor rcgfbsmt on (rs.FinBasementGrade = rcgfbsmt.Grade)


	UPDATE #ResBldg
	SET BldgRCNLD = FLOOR((CASE WHEN PcntNetCondition > 0 THEN BLDGRCN * PcntNetCondition / 100.00
                                  ELSE ((BLDGRCN - finBsmtCost) * pcntGoodBldg + finBsmtCost * pcntGoodBsmt )
                                       * (1 - (CASE WHEN Obsolescence > 100 THEN 100 
                                               ELSE Obsolescence 
                                               END) / 100.00) 
                                       * (CASE WHEN PcntComplete > 0 THEN PcntComplete 
                                               ELSE 100 
                                               END) / 100.00
                                  END) /100) * 100

	
	UPDATE #SalesPop
	SET BldgRCN = FLOOR(rb.BLDGRCN  / 1000) * 1000
	,BldgRCNLD = FLOOR(rb.BLDGRCNLD / 1000) * 1000
	FROM #ResBldg rb INNER JOIN #SalesPop rs ON rb.ParcelId = rs.ParcelId AND rb.BldgGuid = rs.BldgGuid
	
  	
/************************************************************************************************************************************************
                                            ACCYRCN & ACCYRCNLD
*************************************************************************************************************************************************/

/*####################################ASSR_R_GetAreaData2b_Inserts_6 (BEGIN)###############################################*/

INSERT #ResCostAccyComponent
    SELECT 
	 SUM(rcc.FixedCost * rccp.ConcretePavingFixedCost) ConcretePavingFixedCost  ,SUM(rcc.VariableCost * rccp.ConcretePavingVariableCost) ConcretePavingVariableCost  ,SUM(rcc.MinArea * rccp.ConcretePavingMinArea) ConcretePavingMinArea
	,SUM(rcc.FixedCost * rccp.AsphaltPavingFixedCost) AsphaltPavingFixedCost  ,SUM(rcc.VariableCost * rccp.AsphaltPavingVariableCost) AsphaltPavingVariableCost  ,SUM(rcc.MinArea * rccp.AsphaltPavingMinArea) AsphaltPavingMinArea
	,SUM(rcc.FixedCost * rccp.SqFtCarportFixedCost) SqFtCarportFixedCost  ,SUM(rcc.VariableCost * rccp.SqFtCarportVariableCost) SqFtCarportVariableCost  ,SUM(rcc.MinArea * rccp.SqFtCarportMinArea) SqFtCarportMinArea
	,SUM(rcc.FixedCost * rccp.SqFtDetachGarageFixedCost) SqFtDetachGarageFixedCost  ,SUM(rcc.VariableCost * rccp.SqFtDetachGarageVariableCost) SqFtDetachGarageVariableCost  ,SUM(rcc.MinArea * rccp.SqFtDetachGarageMinArea) SqFtDetachGarageMinArea
	,SUM(rcc.FixedCost * rccp.SqFtPoolFixedCost) SqFtPoolFixedCost  ,SUM(rcc.VariableCost * rccp.SqFtPoolVariableCost) SqFtPoolVariableCost  ,SUM(rcc.MinArea * rccp.SqFtPoolMinArea) SqFtPoolMinArea
	,SUM(rcc.FixedCost * rccp.StdDrivewayFixedCost) StdDrivewayFixedCost  ,SUM(rcc.VariableCost * rccp.StdDrivewayVariableCost) StdDrivewayVariableCost  ,SUM(rcc.MinArea * rccp.StdDrivewayMinArea) StdDrivewayMinArea
    FROM rp.rescostcomponent rcc (NOLOCK) 
	INNER JOIN rp.rescostAccycomponent_p rccp(NOLOCK) ON (rcc.component = rccp.component)
    WHERE rcc.rollyr = @CostBillYr and rcc.testprod = @TestProd and rcc.bldgaccy = 'a'

INSERT INTO #ResCostAccyGradeFactor
    SELECT
	Grade
	,sum(rcgf.GradeFactor * rcgfp.ConcretePaving)ConcretePaving
	,sum(rcgf.GradeFactor * rcgfp.AsphaltPaving)AsphaltPaving
	,sum(rcgf.GradeFactor * rcgfp.SqFtCarport)SqFtCarport
	,sum(rcgf.GradeFactor * rcgfp.SqFtDetachGarage)SqFtDetachGarage
	,sum(rcgf.GradeFactor * rcgfp.SqFtPool)SqFtPool
	,sum(rcgf.GradeFactor * rcgfp.StdDriveway)StdDriveway
    FROM rp.rescostcomponent rcc, rp.rescostgradefactor rcgf, rp.ResCostAccyGradeFactor_P rcgfp where rcc.rollyr=@CostBillYr and rcc.testprod = @TestProd and rcc.bldgaccy='a'
	AND rcc.costid = rcgf.costid and rcc.component = rcgfp.component 
    GROUP BY rcgf.grade

/*####################################ASSR_R_GetAreaData2b_Inserts_6 (END)###############################################*/

SET @STDGRADE = 7

--*********************************************************************************************************************************************************************************
-- BEGIN of ASSR_R_GetAreaData2b_Updates_10a
--*********************************************************************************************************************************************************************************
UPDATE #Accessory
     SET component = case a.AccyType
                WHEN 1 THEN 'SqFtDetachGarage'
                WHEN 2 THEN 'SqFtCarport'
                WHEN 3 THEN 'ConcretePaving'
                WHEN 6 THEN 'SqFtPool'
                WHEN 7 THEN 'SqFtPool'
                END
   	,Grade = CASE WHEN a.AccyType = 3 THEN @STDGRADE
                 WHEN a.Grade = 0 AND rs.BldgGrade > 0 THEN rs.BldgGrade 
                 WHEN a.Grade = 0 THEN @STDGRADE
                 ELSE a.Grade
            END
   	,Age = CASE WHEN (CASE WHEN @CostBillYr - 1 - a.EffYr < 0 THEN 0 ELSE @CostBillYr - 1 - a.EffYr END) > 25 AND a.AccyType BETWEEN 6 AND 7 THEN 25
               ELSE (CASE WHEN @CostBillYr - 1 - a.EffYr < 0 THEN 0 ELSE @CostBillYr - 1 - a.EffYr END)
          END
FROM #Accessory a LEFT JOIN #ResBldg rs on a.ParcelId = rs.ParcelId AND (a.BldgGuid = rs.BldgGuid OR a.BldgGuid IS NULL) 

--*********************************************************************************************************************************************************************************
-- BEGIN of ASSR_R_GetAreaData2b_Updates_10b
--*********************************************************************************************************************************************************************************
UPDATE #Accessory
   SET Factor = CASE WHEN AccyType BETWEEN 6 AND 7 THEN -0.05
                  WHEN Grade < 4 THEN -0.015
                  WHEN Grade BETWEEN 4 AND 7 THEN -0.01
                  ELSE -0.00857
             END

--*********************************************************************************************************************************************************************************
-- BEGIN of ASSR_R_GetAreaData2b_Updates_10c
--*********************************************************************************************************************************************************************************
UPDATE #Accessory
     SET RCN = CASE 
               WHEN AccyType = 1 THEN (rcc.SqFtDetachGarageFixedCost / (CASE WHEN a.Size = 0 THEN 1
                                                                            WHEN a.Size > rcc.SqFtDetachGarageMinArea THEN a.Size 
                                                                            ELSE rcc.SqFtDetachGarageMinArea
                                                                            END)  + rcc.SqFtDetachGarageVariableCost) * @CostIndex * a.Size * rcgf.SqFtDetachGarage
               WHEN AccyType = 2 THEN (rcc.SqFtCarportFixedCost / (CASE WHEN a.Size = 0 THEN 1
                                                                       WHEN a.Size > rcc.SqFtCarportMinArea THEN a.Size 
                                                                       ELSE rcc.SqFtCarportMinArea
                                                                       END)  + rcc.SqFtCarportVariableCost) * @CostIndex * a.Size * rcgf.SqFtCarport
               WHEN AccyType = 3 THEN (rcc.ConcretePavingFixedCost / (CASE WHEN a.Size = 0 THEN 1
                                                                          WHEN a.Size > rcc.ConcretePavingMinArea THEN a.Size 
                                                                          ELSE rcc.ConcretePavingMinArea
                                                                          END)  + rcc.ConcretePavingVariableCost) * @CostIndex * a.Size * rcgf.ConcretePaving
               WHEN AccyType BETWEEN 6 AND 7 THEN (rcc.SqFtPoolFixedCost / (CASE WHEN a.Size = 0 THEN 1
                                                                           WHEN a.Size > rcc.SqFtPoolMinArea THEN a.Size 
                                                                           ELSE rcc.SqFtPoolMinArea
                                                                           END)  + rcc.SqFtPoolVariableCost) * @CostIndex * a.Size * rcgf.SqFtPool
          END
FROM #Accessory a, #ResCostAccyComponent rcc, #ResCostAccyGradeFactor rcgf
WHERE a.Grade=rcgf.Grade

UPDATE #Accessory
SET RCN = AccyValue
WHERE AccyType between 8 and 13

--*********************************************************************************************************************************************************************************
-- BEGIN of ASSR_R_GetAreaData2b_Updates_10d
--*********************************************************************************************************************************************************************************
UPDATE #Accessory
SET RCNLD = CASE WHEN AccyType IN (3, 8, 9, 10, 12, 13) THEN RCN
                 ELSE (FLOOR((CASE WHEN PcntNetCondition > 0 THEN RCN * PcntNetCondition / 100.00
                                   ELSE RCN * EXP(Factor * Age)
                                   END) / 5) * 5) 
            END

--*********************************************************************************************************************************************************************************
-- BEGIN of ASSR_R_GetAreaData2b_Inserts_6Accy
--*********************************************************************************************************************************************************************************
/*

Hairo comment: insert commented since the table is not used anywhere, I also commented the creation of the table

INSERT #AccyForRealPropValEst
SELECT ParcelId,BldgGuid
  ,ISNULL((Select distinct LandId FROM #SalesPop rbd where a.ParcelId = rbd.ParcelId),0)
  ,ISNULL((Select distinct BldgId FROM #Bldgs b where a.ParcelId = b.ParcelId and a.BldgGuid = b.BldgGuid),0)
  ,ISNULL((SELECT FLOOR(SUM(FLOOR(RCN))/1000) * 1000  ),0)
  ,ISNULL((SELECT FLOOR(SUM(a.RCNLD)/1000) * 1000),0)
  ,''
  ,ISNULL((Select distinct ImpCnt FROM #SalesPop rbd where a.ParcelId = rbd.ParcelId),0)
FROM #Accessory a 
GROUP BY ParcelId,BldgGuid

*/

--*********************************************************************************************************************************************************************************
-- BEGIN of ASSR_R_GetAreaData2b_Updates_10e
--*********************************************************************************************************************************************************************************
UPDATE #SalesPop
     SET AccyRCN = ISNULL((SELECT FLOOR(SUM(FLOOR(a.RCN))/1000) * 1000 FROM #Accessory a WHERE a.ParcelId = rs.ParcelId and (a.BldgGuid = rs.BldgGuid OR rs.BldgGuid IS NULL)  ),0)
   	,AccyRCNLD = ISNULL((SELECT FLOOR(SUM(a.RCNLD)/1000) * 1000 FROM #Accessory a WHERE a.ParcelId = rs.ParcelId and (a.BldgGuid = rs.BldgGuid OR rs.BldgGuid IS NULL) ),0)
FROM #SalesPop rs


/*####################################ASSR_R_GetAreaData2b_Updates_10e (END)###############################################*/



-- Get Notes if user specified to return notes as well
-- Add warning to notes if mult same AccyType (gars, cp's, pools or MH's)
-- Need to add nolock and indices to speed up   
IF @Notes IS NOT NULL AND @Notes = 'Y' 
BEGIN   
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Inserts_7
--*********************************************************************************************************************************************************************************  

--ACCYTYPE WITH VALUES 9, 10, 12 AND 13 WAS ELIMINATED
	INSERT #MultAccy (ParcelId, AccyType, CountThisType)
    SELECT ParcelId
		   ,AccyType
		   ,COUNT(AccyType)
      FROM #Accessory 
     WHERE AccyType IN (1,2,6,7)  --Hairo comment: originally were added 9,10
        GROUP BY AccyType, ParcelId
    HAVING (COUNT(AccyType) > 1)
     ORDER BY AccyType

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_11a
--*********************************************************************************************************************************************************************************  

	UPDATE #MultAccy
   	SET AccyDescr =CASE AccyType
                    WHEN 1 THEN 'Det Gars'
                    WHEN 2 THEN 'Carports'
                    WHEN 6 THEN 'Pools'
                    WHEN 7 THEN 'Pools'
                  END

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_11b
--*********************************************************************************************************************************************************************************  
 UPDATE #MultAccy
   	SET AccyNote = 'AREA DATA WARN: Costs reflect ' + CONVERT(VARCHAR(4),CountThisType) + ' ' + AccyDescr + '. '  

--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_11c
--*********************************************************************************************************************************************************************************  
 UPDATE #SalesPop
    SET Notes = m.AccyNote
   FROM #SalesPop s 
  INNER JOIN #MultAccy m 
     ON s.ParcelId=m.ParcelId

--*********************************************************************************************************************************************************************************
    --BEGIN of ASSR_R_GetAreaData2b_Inserts_8
    --*********************************************************************************************************************************************************************************  
    
    	--Account for mult accys of diff types
     INSERT 
       INTO #MultMult  
     SELECT ParcelId
    		,AccyType
       FROM #MultAccy
      GROUP BY AccyType, ParcelId
     HAVING (COUNT(AccyType) > 1)
    
    --*********************************************************************************************************************************************************************************
    --BEGIN of ASSR_R_GetAreaData2b_Updates_12a
    --*********************************************************************************************************************************************************************************  
     UPDATE #SalesPop
        SET Notes = Notes + '.. and other multiple accys. '
       FROM #SalesPop s 
      INNER JOIN #MultMult mm 
         ON (s.ParcelId=mm.ParcelId)
 END
 

 -- Add Sale notes (RealPropSale.NoteId). 
-- Verified via query that is used by appr's and mult NoteInstances exist
IF @Notes IS NOT NULL AND @Notes = 'Y' 
BEGIN
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Inserts_9
--*********************************************************************************************************************************************************************************  

CREATE TABLE #NotesOrderedByDate 
	(
	 rowID  		int	not null  identity(1,1)
	 ,ParcelId uniqueidentifier
	 ,SaleGuid uniqueidentifier 
	 ,ExciseTaxNbr int
	 ,Note nvarchar(max)
	 ,UpdateDate smalldatetime
	 ,AssmtEntityId char(4) 
	 ,PRIMARY KEY(rowID)	
	)

	INSERT INTO #NotesOrderedByDate (ParcelId ,SaleGuid ,ExciseTaxNbr, Note  ,UpdateDate ,AssmtEntityId)
	SELECT s.ParcelId,s.SaleGuid, s.ExciseTaxNbr,sn.ptas_notetext,sn.modifiedon,su.ptas_legacyid
	  FROM #SalesPop s
	 INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
	    ON s.ParcelId = spdps.ptas_parceldetailid
	   AND s.SaleGuid = spdps.ptas_salesid
	 INNER JOIN [dynamics].[ptas_salesnote] sn
	    ON spdps.ptas_salesid = sn._ptas_saleid_value
	 INNER JOIN [dynamics].[systemuser] su
	    ON sn._modifiedby_value = su.systemuserid
	 ORDER BY spdps.ptas_salesid,sn.modifiedon DESC


        INSERT #SaleNote
	           (SaleGuid, Note)
		SELECT  s.SaleGuid
				,SUBSTRING(STRING_AGG( + ' SALE NOTE '+ 'E# ' +  COALESCE(CONVERT(Varchar(8),nbd.ExciseTaxNbr),'') +': ' + nbd.Note + '(' + COALESCE(convert(char(11),nbd.UpdateDate),'') + ' ' + ISNULL(nbd.AssmtEntityId,'') + '); ',' '),1,5000)
			FROM #SalesPop s
	 INNER JOIN  #NotesOrderedByDate nbd
		ON s.ParcelId = nbd.ParcelId
	   AND s.SaleGuid = nbd.SaleGuid
	 GROUP BY s.SaleGuid

		

    UPDATE #SalesPop
       SET Notes = COALESCE(Notes,'') + n.Note
   	  FROM #SalesPop rs 
	 INNER JOIN #SaleNote n 
	    ON rs.SaleGuid = n.SaleGuid
		
END		

SET @NICntr = 0

IF @Notes IS NOT NULL AND @Notes = 'Y' 
BEGIN		
	--*********************************************************************************************************************************************************************************
	--BEGIN of ASSR_R_GetAreaData2b_Inserts_10
	--*********************************************************************************************************************************************************************************  

--****************************

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
		SELECT  s.ParcelId,cn.ptas_notetext,cn.modifiedon,su.ptas_legacyid
	       FROM #SalesPop s 
		  INNER JOIN [dynamics].[ptas_camanotes] cn
			 ON s.ParcelId = cn._ptas_parcelid_value
		  INNER JOIN [dynamics].[systemuser] su
			 ON cn._modifiedby_value = su.systemuserid
		  ORDER BY s.ParcelId ASC, cn.modifiedon  DESC


	    INSERT  #NoteInstance
	            (ParcelId, Note)
		SELECT  s.ParcelId
				,SUBSTRING(STRING_AGG(  COALESCE(rpn.Note,'') + '(' + COALESCE(convert(char(11),rpn.UpdateDate),'') + ' ' + ISNULL(rpn.AssmtEntityId,'') + '); ',' '),1,4950)
	       FROM #SalesPop s
		  INNER JOIN #RPNotesOrderedByDate rpn
			 ON s.ParcelId = rpn.ParcelId
		  GROUP BY s.ParcelId		  
		  

	--*********************************************************************************************************************************************************************************
	--BEGIN of ASSR_R_GetAreaData2b_Updates_14a
	--*********************************************************************************************************************************************************************************  

	UPDATE #SalesPop
       SET Notes = COALESCE(Notes,'') + ' RP NOTE: '
	 WHERE Notes <> ''


	--*********************************************************************************************************************************************************************************
	--BEGIN of ASSR_R_GetAreaData2b_Inserts_11
	--*********************************************************************************************************************************************************************************  
	--Hairo comment: code not required since the concatenation fo notes is completed in insert ASSR_R_GetAreaData2b_Inserts_10
	--*********************************************************************************************************************************************************************************
	--BEGIN of ASSR_R_GetAreaData2b_Updates_15a
	--*********************************************************************************************************************************************************************************  
	--Hairo comment: code not required since the concatenation fo notes is completed in insert ASSR_R_GetAreaData2b_Inserts_10 
	--*********************************************************************************************************************************************************************************
	--BEGIN of ASSR_R_GetAreaData2b_Updates_16a
	--*********************************************************************************************************************************************************************************  
	 --Hairo comment: code not required since the concatenation fo notes is completed in insert ASSR_R_GetAreaData2b_Inserts_10


    UPDATE #SalesPop
       SET Notes = CASE WHEN LEN(Notes + n.Note) >= 5000 THEN SUBSTRING(Notes + n.Note,1,4950) +' AND OTHER NOTES; '
					    ELSE Notes + n.Note
					END
   	  FROM #SalesPop rs 
	 INNER JOIN #NoteInstance n 
	    ON rs.ParcelId = n.ParcelId
 END
 
 
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_17a
--*********************************************************************************************************************************************************************************  

	UPDATE #SalesPop   --added due to QMAS error:  Unknown Error 1004 Application-defined or object-defined error  DSAX 8/02/02 
       SET Notes = STUFF(notes, 1, 1, 'Equals ') 
	 WHERE LEFT(notes,1) = '='
 
--*********************************************************************************************************************************************************************************
--BEGIN of ASSR_R_GetAreaData2b_Updates_17b
--*********************************************************************************************************************************************************************************  

	UPDATE #SalesPop 
       SET Notes = REPLACE ( Notes ,CHAR(13)+CHAR(10),CHAR(32) ) 
	 WHERE (CHARINDEX(CHAR(13), Notes) > 0 
	   AND CHARINDEX(CHAR(10), Notes) > 0)

/*####################################ASSR_R_GetAreaData2b_Inserts_12 (BEGIN)###############################################*/




--Added for trended sale price 3/11/19 Don G
DECLARE @SaleStartDate varchar(12)
        ,@SaleEndDate varchar(12)
        ,@TrendWhere varchar(1000)
		,@Trend nvarchar(max)
		,@AssmtDate smalldatetime

SELECT @SaleStartDate = CONVERT(varchar(12),@SalesFrom)
SELECT @SaleEndDate = CONVERT(varchar(12),@SalesTo)
SELECT @Trend = TranslatedTrendModel From rp.ResModelTrend2 WHERE Area = @Area AND AssmtYr = @AssmtYr
--SELECT@TrendWhere= 'WHERE rap.Area         = ' + @chArea + ' AND s.PropertyClassItemId      <> 7 AND s.SalePrice        > 1000 AND s.SaleDate         BETWEEN ''' + @SaleStartDate + ''' AND ''' + @SaleEndDate + ''''
SELECT @TrendWhere = 'WHERE dpd.ptas_resarea = ' + convert(varchar(3),@Area) + ' AND dps.ptas_salepropertyclass <> 7 AND dps.ptas_saleprice > 1000 AND dps.ptas_saledate  BETWEEN ''' + @SaleStartDate + ''' AND ''' + @SaleEndDate + ''''	
SELECT @AssmtDate = CONVERT(smalldatetime,'1/1/' + convert(varchar(4),@AssmtYr))


SELECT @Trend = REPLACE(@Trend,'SaleDate','DATEDIFF(dd, ''12/31/1899'', ptas_saledate)')

IF @Trend IS NOT NULL
	BEGIN
		EXEC
		('
		INSERT #TrendedSales	
		 SELECT  dps.ptas_salesid
				,dpd.ptas_parceldetailid
				,dps.ptas_saledate
				,dps.ptas_saleprice,' + @Trend + ',ROUND(dps.ptas_SalePrice * ' + @Trend + ',0)
		   FROM [dynamics].[ptas_parceldetail] dpd
		  INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
			 ON spdps.ptas_parceldetailid = dpd.ptas_parceldetailid
		  INNER JOIN [dynamics].[ptas_sales] dps
			 ON dps.ptas_salesid = spdps.ptas_salesid
		 ' + @TrendWhere + '			
			AND COALESCE(dpd.ptas_splitcode,0) = 0
			AND (dpd.statecode  = 0 AND dpd.statuscode = 1)
			AND COALESCE(dpd.ptas_snapshottype,'''') = '''' 		 
		  ORDER BY dps.ptas_saledate
		')	
	
	END 
	

UPDATE #TrendedSales
SET TrendFactor = 1-(TrendFactor - 1)
    ,TrendedPrice = ROUND(SalePrice * (1-(TrendFactor-1)),0)
WHERE SaleDate > @AssmtDate
/*####################################ASSR_R_GetAreaData2b_Inserts_12 (END)###############################################*/



--*********************************************************************************************************************************************************************************
-- BEGIN of ASSR_R_GetAreaData2b_Updates_18
--*********************************************************************************************************************************************************************************
UPDATE #SalesPop
SET TrendFactor = ts.TrendFactor
    ,TrendedPrice = ts.TrendedPrice
FROM #TrendedSales ts INNER JOIN #SalesPop sp ON ts.SaleGuid = sp.SaleGuid

----*********************************************************************************************************************************************************************************
---- BEGIN of ASSR_R_GetAreaData2b_Updates_19
----*********************************************************************************************************************************************************************************
--UPDATE #SalesPop
--SET DistrictName = p.ptas_district
--FROM #SalesPop sp 
--INNER JOIN dynamics.ptas_parceldetail p ON (p.ptas_parceldetailid = sp.ParcelId)


--*********************************************************************************************************************************************************************************
--BEGIN of Other UPDATES
--*********************************************************************************************************************************************************************************  

--Hairo comment: Lines commented since the value is calculated at the begining
--	UPDATE #SalesPop
--	   SET HoldoutReason =  dynamics.fn_GetValueFromStringMap('ptas_parceldetail', 'ptas_holdoutreason', pd.ptas_holdoutreason)
--	  FROM #SalesPop sp
--	 INNER JOIN [dynamics].[ptas_parceldetail] pd
--	    ON sp.ParcelId = pd.ptas_parceldetailid

	UPDATE  #SalesPop
	   SET  SqFtLotDry        = CASE WHEN ISNULL(pl.ptas_drysqft,0) > 0 THEN pl.ptas_drysqft ELSE sp.SqFtLot END
	       ,SqFtLotSubmerged  =	ISNULL(pl.ptas_submergedsqft,0)
	  FROM #SalesPop sp
	 INNER JOIN [dynamics].[ptas_land] pl
	    ON sp.LandGuid = pl.ptas_landid



		
				
--*********************************************************************************************************************************************************************************
--BEGIN of Mobile Homes Section /***************************Mobile Homes Section****************************************************/ 
--*********************************************************************************************************************************************************************************  

/***************************Mobile Homes Section****************************************************/ 
--If multiple mobile homes, we'll get MHType=4, AcctStatus not in (7,8), Lowest BldgNbr 

/*
Hairo comment:
			  I´m using [dynamics].[ptas_condounit] to calculate the MOBILE Homes, but I still dont know 
			  how to apply this filter "a.AcctStatusItemId IN (1,4,10)" in the new database, it is related
			  to the Status of the  account, for reference run this query in Real Property database 
			  "select * from luitem2 where  lutypeid = 230"

INSERT #MHCount
SELECT r.RpGuid,count(*)
FROM #SalesPop r INNER JOIN ResAreaPcl rap ON r.RpGuid = rap.RpGuid
                    INNER JOIN MHAcct a ON r.ParcelId = a.RpGuid
WHERE a.AcctStatusItemId IN (1,4,10) AND a.RpGuid IS NOT NULL
GROUP BY r.RpGuid
*/

INSERT #MHCount
SELECT r.ParcelId, COUNT(1) 
FROM #SalesPop r 
INNER JOIN [dynamics].[ptas_condounit] cu  ON r.ParcelId = cu._ptas_parcelid_value
WHERE cu.ptas_mobilehometype > 0
and cu._ptas_parcelid_value is not null
GROUP BY r.ParcelId
/*
Hairo comment:
			  I´m using [dynamics].[ptas_condounit] to calculate the MOBILE Homes, but I still dont know 
			  how to apply this filter "AcctStatusItemId NOT IN (7,8)" in the new database, it is related
			  to the Status of the  account, for reference run this query in Real Property database 
			  "select * from luitem2 where  lutypeid = 230"

INSERT #AllMobiles
SELECT a.RpGuid, a.MhGuid, a.MHTypeItemId, a.AcctStatusItemId, ISNULL(a.BldgNbr,0)
   ,0,c.ClassItemId,c.Length,c.Width,c.LivingArea,ISNULL(c.TipOutArea,0),ISNULL(c.RoomAddSqft,0),c.SizeItemId
   ,c.YrBuilt,c.ConditionItemId,ISNULL(c.PcntNetCondition,0),0,0,a.Id,rbd.LandId
FROM MHAcct a INNER JOIN #SalesPop rbd ON a.RpGuid=rbd.RpGuid
              INNER JOIN MHChar c ON a.MhGuid = c.MhGuid
WHERE AcctStatusItemId NOT IN (7,8) and a.RpGuid IS NOT NULL
*/

INSERT #AllMobiles
SELECT  r.ParcelId
	   ,cu.ptas_condounitid
	   ,cu.ptas_mobilehometype
	   ,NULL AS AccuuntStatusItemId
	   ,COALESCE(cu.ptas_buildingnumber,0)
	   ,0
	   ,cu.ptas_mobilehomeclass
	   ,cu.ptas_length
	   ,cu.ptas_width
	   ,cu.ptas_totalliving
	   ,COALESCE(cu.ptas_tipoutarea,0)
	   ,COALESCE(cu.ptas_roomadditionalsqft,0)
	   ,cu.ptas_size
	   ,py01.ptas_name
	   ,cu.ptas_mobilehomecondition
	   ,COALESCE(cu.ptas_percentnetcondition,0)
	   ,0
	   ,0
	   ,cu.ptas_legacyunitid
FROM #SalesPop r 
INNER JOIN [dynamics].[ptas_condounit] cu   ON r.ParcelId = cu._ptas_parcelid_value
LEFT JOIN [dynamics].[ptas_year] 	   py01 ON py01.ptas_yearid = cu._ptas_yearbuildid_value
WHERE cu.ptas_mobilehometype > 0
and cu._ptas_parcelid_value is not null


UPDATE #AllMobiles
SET MHCount = mc.MHCount
FROM #MHCount mc INNER JOIN #AllMobiles am ON mc.ParcelId = am.ParcelId

--Get RpMobiles
INSERT #ShowMobiles
SELECT am.ParcelId, MhGuid, MHType, AcctStatus, BldgNbr
   ,MHCount,MHClass,MHLength,MHWidth,MHLivingArea,MHTipOutArea,MHRoomAddSqft,MHSize
   ,MHYrBuilt,MHCondition,MHPcntNetCondition,MHRCN,MHRCNLD,MobileHomeId
   --,LandId 
FROM #AllMobiles am
WHERE BldgNbr = (SELECT MIN(BldgNbr) FROM #AllMobiles am2 WHERE am2.ParcelId = am.ParcelId AND MHType=4 )

--If no RpMobile, get PPMobile with lowest MobileHomeId
INSERT #ShowMobiles
SELECT am.ParcelId, MhGuid, MHType, AcctStatus, BldgNbr
   ,MHCount,MHClass,MHLength,MHWidth,MHLivingArea,MHTipOutArea,MHRoomAddSqft,MHSize
   ,MHYrBuilt,MHCondition,MHPcntNetCondition,MHRCN,MHRCNLD,MobileHomeId
   --,LandId 
FROM #AllMobiles am
WHERE MHType<>4
 AND NOT EXISTS (SELECT * FROM #ShowMobiles sm WHERE sm.ParcelId = am.ParcelId) 
 AND MobileHomeId = (SELECT MIN(MobileHomeId) FROM #AllMobiles am2 WHERE am2.ParcelId = am.ParcelId)

UPDATE #SalesPop
SET MHCount = mym.MHCount
    ,MHBldgNbr = mym.BldgNbr
    ,MHType = mym.MHType
    ,MHClass = mym.MHClass
    ,MHLength = mym.MHLength
    ,MHWidth = mym.MHWidth
    ,MHLivingArea = mym.MHLivingArea
    ,MHTipOutArea = ISNULL(mym.MHTipOutArea,0)
    ,MHRoomAddSqft =ISNULL(mym.MHRoomAddSqft,0)
    ,MHSize = mym.MHSize
    ,MHYrBuilt = mym.MHYrBuilt
    ,MHCondition = mym.MHCondition
    ,MHPcntNetCondition = ISNULL(mym.MHPcntNetCondition,0)
    ,MhGuid = mym.MhGuid
	,MobileHomeId = mym.MobileHomeId
FROM #SalesPop rbd INNER JOIN #ShowMobiles mym ON rbd.ParcelId = mym.ParcelId


/***************BEGINNING OF MARSHALL & SWIFT COST SECTION***************************************/


INSERT #GetMHCost
SELECT MhGuid,ParcelId,MobileHomeId,MHType,AcctStatus,0,0,0,0
FROM #AllMobiles



/*####################################ASSR_R_CalcMHCost (BEGIN)###############################################*/



DECLARE @QuarterlyMultiplier  decimal(3,2)
        ,@LocalMultiplier  decimal(3,2) 

SET @QuarterlyMultiplier = (SELECT QuarterlyMultiplier FROM rp.MHCostIndex2 WHERE AssmtYr=@AssmtYr)
SET @LocalMultiplier = (SELECT LocalMultiplier FROM rp.MHCostIndex2 WHERE AssmtYr=@AssmtYr)



CREATE TABLE #TotalSqftCost
 (
 MhGuid			uniqueidentifier
 ,MissingData	tinyint
 ,Class			tinyint
 ,Size			tinyint
 ,Width			smallint
 ,MainWidth		smallint
 ,TagWidth		smallint
 ,[Length]		smallint
 ,LivingArea	int
 ,TipoutArea	int
 ,RoomAddSqft	int
 ,MainSqftCost	decimal(9,5)
 ,MainBoxCost	decimal(11,5)
 ,TagSqftCost	decimal(9,5)
 ,TagBoxCost	decimal(11,5)
 ,TagAlongFactor decimal(4,2)
 ,SqftCost		decimal(9,5)
 ,TotalBoxRCN	decimal(11,5)
 )

CREATE TABLE #MainBoxCost
 (
 MhGuid  uniqueidentifier
 ,Class  tinyint
 ,Width  int
 ,Length  int
 ,LowerWidth decimal(9,5)
 ,UpperWidth decimal(9,5)
 ,WidthPercent  decimal(9,5)
 ,LowerLength decimal(9,5)
 ,UpperLength decimal(9,5)
 ,LengthPercent  decimal(9,5)
 ,LowerWidthLowerLengthCost decimal(9,5)
 ,UpperWidthLowerLengthCost decimal(9,5)
 ,LowerWidthUpperLengthCost decimal(9,5)
 ,UpperWidthUpperLengthCost decimal(9,5)
 ,WidthLowerLengthSqftCost decimal(9,5)
 ,WidthUpperLengthSqftCost decimal(9,5)
 ,SqftCost decimal(9,5)
 )

CREATE TABLE #TagBoxCost
 (
 MhGuid  uniqueidentifier
 ,Class  tinyint
 ,Width  int
 ,Length  int
 ,LowerWidth decimal(9,5)
 ,UpperWidth decimal(9,5)
 ,WidthPercent  decimal(9,5)
 ,LowerLength decimal(9,5)
 ,UpperLength decimal(9,5)
 ,LengthPercent  decimal(9,5)
 ,LowerWidthLowerLengthCost decimal(9,5)
 ,UpperWidthLowerLengthCost decimal(9,5)
 ,LowerWidthUpperLengthCost decimal(9,5)
 ,UpperWidthUpperLengthCost decimal(9,5)
 ,WidthLowerLengthSqftCost decimal(9,5)
 ,WidthUpperLengthSqftCost decimal(9,5)
 ,SqftCost decimal(9,5)
 )

Create Table #RCN_RCNLD
 (MhGuid  uniqueidentifier
  ,RCN decimal(11,5)
  ,Class tinyint
  ,Size  tinyint
  ,YrBuilt int
  ,Condition tinyint
  ,PcntNetCondition int
  ,ExpectedLife smallint
  ,PercentGood decimal(5,4)  
  ,AdjPercentGood decimal(5,4)  
  ,RCNLD decimal(11,5))

--Do intial inserts
INSERT #TotalSqftCost
SELECT cu.ptas_condounitid
       ,0    --MissingData
	   ,cu.ptas_mobilehomeclass	
	   ,cu.ptas_size			
	   ,cu.ptas_width
	   ,0  --MainWidth
	   ,0  --TagWidth
       ,cu.ptas_length
	   ,cu.ptas_totalliving
	   ,ISNULL(cu.ptas_tipoutarea,0)
	   ,ISNULL(cu.ptas_roomadditionalsqft,0)
	   ,0  --MainSqftCost
	   ,0  --MainBoxCost
	   ,0  --TagSqftCost
	   ,0  --TagBoxCost
	   ,0  --TagAlongFactor
	   ,0  --SqftCost
	   ,0  --TotalBoxRCN
FROM [dynamics].[ptas_condounit] cu 
INNER JOIN #GetMHCost mh ON cu.ptas_condounitid = mh.MhGuid 
--WHERE a.AcctStatusItemId in (1,4,10) Hairo comment I need to find this column AcctStatusItemId, currently it is not available


--Find any missing Data-RCN and RCNLD will be 0 for these accounts
UPDATE #TotalSqftCost
SET MissingData = 1
WHERE Class = 0 OR Class IS NULL
 OR Width = 0 OR Width IS NULL
 OR Length = 0 OR Length IS NULL
 OR Size = 0 or Size IS NULL
 OR LivingArea = 0 OR LivingArea IS NULL


--1. Set MainWidth & TagWidths - do TagWidth first - TagWidth will round down and MainWidth will round up
--to resolve: it looks like in the MH application tag widths round up if fractional foot is >=.5
UPDATE #TotalSqftCost
SET TagWidth = CASE
                WHEN SIZE IN (1,2) THEN 0 --single & double-wides - these have no tagalong
                WHEN SIZE=3 THEN Width/3  --for triple-wides
                WHEN SIZE=4 THEN Width/2  --for quad-wides
                ELSE 0
                END
UPDATE #TotalSqftCost
SET MainWidth = CASE
                WHEN SIZE>2 THEN Round(Width - TagWidth,0)
                ELSE Width
                END
--2. Convert Main and Tag Width if outside boundaries of MHBoxCost2 table
UPDATE #TotalSqftCost
SET MainWidth = CASE
                WHEN Class BETWEEN 1 AND 4 AND MainWidth BETWEEN 1 AND 7 THEN 8
                WHEN Class IN (5,6) AND MainWidth BETWEEN 1 AND 9 THEN 10
                WHEN MainWidth=19 THEN 18
                WHEN MainWidth>36 THEN 36
                ELSE MainWidth
                END
    ,TagWidth = CASE
                WHEN Class BETWEEN 1 AND 4 AND TagWidth BETWEEN 1 AND 7 THEN 8
                WHEN Class IN (5,6) AND TagWidth BETWEEN 1 AND 9 THEN 10
                WHEN TagWidth=19 THEN 18
                WHEN TagWidth>36 THEN 36
                ELSE TagWidth
   END
--3. Convert Length if outside boundaries of MHBoxCost2 table
UPDATE #TotalSqftCost
SET Length = CASE
             WHEN Class BETWEEN 1 AND 3 AND Length BETWEEN 1 AND 19 THEN 20
             WHEN Class BETWEEN 4 AND 6 AND Length BETWEEN 1 AND 27 THEN 28 
			 WHEN Length > 80 THEN 80
             ELSE Length
             END

--Insert Data into Main section table - interpolation of Main section will be done here
INSERT #MainBoxCost
SELECT tot.MhGuid,tot.Class
 ,tot.MainWidth
 ,tot.Length
 ,0,0,0,0,0,0,0,0,0,0,0,0,0
FROM #TotalSqftCost tot


--Fill in columns to set up interpolation for main box
UPDATE #MainBoxCost
SET LowerWidth = (SELECT MAX(Width) FROM rp.MHBoxCost2 cost
                  WHERE AssmtYr=@AssmtYr AND cost.Class=mbc.Class
                        AND cost.Width<=mbc.Width)
    ,UpperWidth = (SELECT MIN(Width) FROM rp.MHBoxCost2 cost
                  WHERE AssmtYr=@AssmtYr AND cost.Class=mbc.Class
                        AND cost.Width>=mbc.Width)
    ,LowerLength = (SELECT MAX(Length) FROM rp.MHBoxCost2 cost
                    WHERE AssmtYr=@AssmtYr AND cost.Class=mbc.Class
                        AND cost.Length<=mbc.Length)
    ,UpperLength = (SELECT MIN(Length) FROM rp.MHBoxCost2 cost
                    WHERE AssmtYr=@AssmtYr AND cost.Class=mbc.Class
                        AND cost.Length>=mbc.Length) 
FROM #MainBoxCost mbc 


UPDATE #MainBoxCost
SET WidthPercent = CASE
                   WHEN Width = LowerWidth THEN 0
                   WHEN Width = UpperWidth THEN 0
                   ELSE (CONVERT(decimal(8,4),Width)-LowerWidth)/(UpperWidth-LowerWidth)
                   END
    ,LengthPercent = CASE
                     WHEN Length = LowerLength THEN 0
                     WHEN Length = UpperLength THEN 0
                     ELSE (CONVERT(decimal(8,4),Length)-LowerLength)/(UpperLength-LowerLength)
                     END


UPDATE #MainBoxCost
SET LowerWidthLowerLengthCost = (SELECT SqftCost FROM rp.MHBoxCost2 cost
                                 WHERE AssmtYr=@AssmtYr AND cost.Class=mbc.Class AND cost.Width=mbc.LowerWidth and cost.Length=mbc.LowerLength)
    ,UpperWidthLowerLengthCost = (SELECT SqftCost FROM rp.MHBoxCost2 cost
                                 WHERE AssmtYr=@AssmtYr AND cost.Class=mbc.Class AND cost.Width=mbc.UpperWidth and cost.Length=mbc.LowerLength)
    ,LowerWidthUpperLengthCost = (SELECT SqftCost FROM rp.MHBoxCost2 cost
                                 WHERE AssmtYr=@AssmtYr AND cost.Class=mbc.Class AND cost.Width=mbc.LowerWidth and cost.Length=mbc.UpperLength)
    ,UpperWidthUpperLengthCost = (SELECT SqftCost FROM rp.MHBoxCost2 cost
                                 WHERE AssmtYr=@AssmtYr AND cost.Class=mbc.Class AND cost.Width=mbc.UpperWidth and cost.Length=mbc.UpperLength)
FROM #MainBoxCost mbc



UPDATE #MainBoxCost
SET WidthLowerLengthSqftCost = (UpperWidthLowerLengthCost-LowerWidthLowerLengthCost)*WidthPercent + LowerWidthLowerLengthCost
    ,WidthUpperLengthSqftCost = (UpperWidthUpperLengthCost-LowerWidthUpperLengthCost)*WidthPercent + LowerWidthUpperLengthCost

UPDATE #MainBoxCost
SET SqftCost = (WidthUpperLengthSqftCost-WidthLowerLengthSqftCost)*LengthPercent + WidthLowerLengthSqftCost
--End interpolation of main section

--Insert Data into Tag Section if Size>2 - interpolation for Tagalong section, if there is one, will be done here
INSERT #TagBoxCost
SELECT tot.MhGuid,tot.Class
 ,tot.TagWidth
 ,tot.Length
 ,0,0,0,0,0,0,0,0,0,0,0,0,0
FROM #TotalSqftCost tot
WHERE tot.Size>2


--Fill in columns to set up interpolation
UPDATE #TagBoxCost
SET LowerWidth = (SELECT MAX(Width) FROM rp.MHBoxCost2 cost
                  WHERE AssmtYr=@AssmtYr AND cost.Class=tbc.Class
                        AND cost.Width<=tbc.Width)
    ,UpperWidth = (SELECT MIN(Width) FROM rp.MHBoxCost2 cost
                  WHERE AssmtYr=@AssmtYr AND cost.Class=tbc.Class
                        AND cost.Width>=tbc.Width)
    ,LowerLength = (SELECT MAX(Length) FROM rp.MHBoxCost2 cost
                    WHERE AssmtYr=@AssmtYr AND cost.Class=tbc.Class
                        AND cost.Length<=tbc.Length)
    ,UpperLength = (SELECT MIN(Length) FROM rp.MHBoxCost2 cost
                    WHERE AssmtYr=@AssmtYr AND cost.Class=tbc.Class
                        AND cost.Length>=tbc.Length) 
FROM #TagBoxCost tbc 


UPDATE #TagBoxCost
SET WidthPercent = CASE
                   WHEN Width = LowerWidth THEN 0
                   WHEN Width = UpperWidth THEN 0
                   ELSE (CONVERT(decimal(8,4),Width)-LowerWidth)/(UpperWidth-LowerWidth)
                   END
    ,LengthPercent = CASE
                     WHEN Length = LowerLength THEN 0
                     WHEN Length = UpperLength THEN 0
                     ELSE (CONVERT(decimal(8,4),Length)-LowerLength)/(UpperLength-LowerLength)
                     END




UPDATE #TagBoxCost
SET LowerWidthLowerLengthCost = (SELECT SqftCost FROM rp.MHBoxCost2 cost
                                 WHERE AssmtYr=@AssmtYr AND cost.Class=tbc.Class AND cost.Width=tbc.LowerWidth and cost.Length=tbc.LowerLength)
    ,UpperWidthLowerLengthCost = (SELECT SqftCost FROM rp.MHBoxCost2 cost
                                 WHERE AssmtYr=@AssmtYr AND cost.Class=tbc.Class AND cost.Width=tbc.UpperWidth and cost.Length=tbc.LowerLength)
    ,LowerWidthUpperLengthCost = (SELECT SqftCost FROM rp.MHBoxCost2 cost
                                 WHERE AssmtYr=@AssmtYr AND cost.Class=tbc.Class AND cost.Width=tbc.LowerWidth and cost.Length=tbc.UpperLength)
    ,UpperWidthUpperLengthCost = (SELECT SqftCost FROM rp.MHBoxCost2 cost
                                 WHERE AssmtYr=@AssmtYr AND cost.Class=tbc.Class AND cost.Width=tbc.UpperWidth and cost.Length=tbc.UpperLength)
FROM #TagBoxCost tbc



UPDATE #TagBoxCost
SET WidthLowerLengthSqftCost = (UpperWidthLowerLengthCost-LowerWidthLowerLengthCost)*WidthPercent + LowerWidthLowerLengthCost
    ,WidthUpperLengthSqftCost = (UpperWidthUpperLengthCost-LowerWidthUpperLengthCost)*WidthPercent + LowerWidthUpperLengthCost

UPDATE #TagBoxCost
SET SqftCost = (WidthUpperLengthSqftCost-WidthLowerLengthSqftCost)*LengthPercent + WidthLowerLengthSqftCost
--End interpolation of Tagalong section

UPDATE #TotalSqftCost
SET MainSqftCost=mbc.SqftCost
FROM #MainBoxCost mbc INNER JOIN #TotalSqftCost tot ON mbc.MhGuid=tot.MhGuid

UPDATE #TotalSqftCost
SET MainBoxCost=MainSqftCost*MainWidth*Length

UPDATE #TotalSqftCost
SET TagSqftCost=tbc.SqftCost
FROM #TagBoxCost tbc INNER JOIN #TotalSqftCost tot ON tbc.MhGuid=tot.MhGuid


UPDATE #TotalSqftCost
SET TagAlongFactor = t.TagalongFactor
FROM #TotalSqftCost tot INNER JOIN rp.MHTagalongFactor t ON tot.Class=t.Class
WHERE t.AssmtYr=@AssmtYr AND tot.Size in (3,4)



UPDATE #TotalSqftCost
SET TagBoxCost= TagSqftCost*TagWidth*Length*TagalongFactor

UPDATE #TotalSqftCost
SET SqftCost = (MainBoxCost + TagBoxCost)/((MainWidth + TagWidth) * Length)


UPDATE #TotalSqftCost
SET TotalBoxRCN = SqftCost * (LivingArea + TipoutArea + RoomAddSqft) * @QuarterlyMultiplier * @LocalMultiplier
WHERE MissingData=0
--End of RCN calculation


INSERT #RCN_RCNLD
SELECT tot.MhGuid,tot.TotalBoxRCN,tot.Class,tot.Size,y.ptas_name,cu.ptas_mobilehomecondition,cu.ptas_percentnetcondition,0,0,0,0
 FROM #TotalSqftCost tot 
INNER JOIN [dynamics].[ptas_condounit] cu 
   ON tot.MhGuid=cu.ptas_condounitid
LEFT JOIN [dynamics].[ptas_year] y
  ON cu._ptas_yearbuildid_value = y.ptas_yearid



UPDATE #RCN_RCNLD
SET ExpectedLife = el.ExpectedLife
FROM rp.MHExpectedLife el (nolock) INNER JOIN #RCN_RCNLD rr ON el.Class=rr.Class AND el.Size=rr.Size
WHERE el.AssmtYr=@AssmtYr

UPDATE #RCN_RCNLD
SET PercentGood = pg.PcntGoodFactor
FROM rp.MHPercentGood2 pg (nolock) INNER JOIN #RCN_RCNLD rr ON pg.ExpectedLife=rr.ExpectedLife AND pg.Age=@AssmtYr-rr.YrBuilt
Where pg.AssmtYr=@AssmtYr


UPDATE #RCN_RCNLD
SET PercentGood = 0.20
WHERE @AssmtYr - YrBuilt > 60

UPDATE #RCN_RCNLD
SET AdjPercentGood = CASE
                     WHEN rr.PercentGood=0 then 0
                     WHEN rr.PercentGood * cm.MultAdj + AddAdj > 1 THEN 1
                     WHEN (@AssmtYr-rr.YrBuilt) * cm.AddAdj > cm.MaxAddAdj THEN
                       rr.PercentGood * cm.MultAdj + cm.MaxAddAdj
                     ELSE rr.PercentGood * cm.MultAdj + (@AssmtYr-rr.YrBuilt) * cm.AddAdj
                     END
FROM #RCN_RCNLD rr INNER JOIN rp.MHConditionModifier cm (nolock) ON rr.Condition = cm.Condition  


--If AdjPercentGood>1 (happens if Class=6 Condition=6) then set it to 1  6/20/2011 Don G
UPDATE #RCN_RCNLD
SET AdjPercentGood = 1
Where AdjPercentGood > 1  

UPDATE #RCN_RCNLD
SET RCNLD = CASE
            WHEN PcntNetCondition>0 THEN RCN * (PcntNetCondition * .01)
            WHEN AdjPercentGood = 0 THEN 0
            ELSE round(RCN * AdjPercentGood,0)
            END

--END RCNLD calculation

UPDATE #GetMHCost
SET RCN = ISNULL(rr.RCN,0)
    ,RCNLD = ISNULL(rr.RCNLD,0)
	,ExpectedLife = rr.ExpectedLife
	,PercentGood = rr.PercentGood
FROM #RCN_RCNLD rr INNER JOIN #GetMHCost mh ON rr.MhGuid = mh.MhGuid


UPDATE #GetMHCost
SET RCN = ROUND(rr.RCN,0)
    ,RCNLD = ROUND(rr.RCNLD,0)
FROM #RCN_RCNLD rr INNER JOIN #GetMHCost mh ON rr.MhGuid = mh.MhGuid
WHERE MHTypeItemId in (1,2,5)

UPDATE #GetMHCost
SET RCN = CONVERT(int,rr.RCN/1000)*1000
    ,RCNLD = CONVERT(int,rr.RCNLD/1000)*1000
FROM #RCN_RCNLD rr INNER JOIN #GetMHCost mh ON rr.MhGuid = mh.MhGuid
WHERE MHTypeItemId = 4
/*####################################ASSR_R_CalcMHCost (END)###############################################*/

UPDATE #AllMobiles
SET MHRCN = g.RCN
    ,MHRCNLD = g.RCNLD
FROM #GetMHCost g INNER JOIN #AllMobiles am ON g.ParcelId = am.ParcelID and g.MhGuid = am.MhGuid

UPDATE #SalesPop
SET MHRCN = mh.RCN
    ,MHRCNLD = mh.RCNLD
FROM #GetMHCost mh INNER JOIN #SalesPop rbd ON mh.ParcelId = rbd.ParcelId
                   INNER JOIN #ShowMobiles sm ON mh.MhGuid = sm.MhGuid
WHERE mh.MhTypeItemId = 4

/***************END OF MARSHALL & SWIFT COST SECTION*******************************************/ 

/*
NEW CODE
Get AssignedBoth, coming from vw_GisMapData
*/

UPDATE #SalesPop
   SET AssignedBoth = gmd.AssignedBoth
  FROM [dynamics].[vw_GISMapData] gmd
 INNER JOIN #SalesPop  sp ON sp.ParcelId = gmd.ParcelId


--****************************************************************************************************

/*####################################BEGIN ADD ADDICIONAL COLUMNS REQUESTED BY REGIS###############################################*/

	UPDATE #SalesPop
	SET  
	 AirportValPct            = CAST(LvNU.AirportValPct AS INT)
	,OtherNuisValPct          = CAST(LvNU.OtherNuisValPct AS INT)
	,OtherProblemsValPct      = CAST(LvNU.OtherProblemsValPct AS INT)
	,PowerLinesValPct         = CAST(LvNU.PowerLinesValPct AS INT)
	,RoadAccessValPct         = CAST(LvNU.RoadAccessValPct AS INT)
	,TopoValPct				  = CAST(LvNU.TopoValPct AS INT)
	,WaterProblemsValPct      = CAST(LvNU.WaterProblemsValPct AS INT)
	,TrafficValPct            = CAST(LvNU.TrafficValPct AS INT)
	,AirportValDollars        = CAST(LvNU.AirportValDollars AS MONEY)
	,TrafficValDollars        = CAST(LvNU.TrafficValDollars AS MONEY)
	,HundredYrValPct	      = CAST(LvER.HundredYrValPct	 AS INT)
	,CoalValPct		          = CAST(LvER.CoalValPct		 AS INT)
	,ContamValPct	          = CAST(LvER.ContamValPct	 AS INT)
	,DrainageValPct 	      = CAST(LvER.DrainageValPct 	 AS INT)
	,ErosionValPct	          = CAST(LvER.ErosionValPct	 AS INT)
	,LandfillValPct 	      = CAST(LvER.LandfillValPct 	 AS INT)
	,LandslideValPct	      = CAST(LvER.LandslideValPct	 AS INT)
	,SeismicValPct	          = CAST(LvER.SeismicValPct	 AS INT)
	,SensitiveValPct	      = CAST(LvER.SensitiveValPct	 AS INT)
	,SpeciesValPct	          = CAST(LvER.SpeciesValPct	 AS INT)
	,SteepSlopeValPct         = CAST(LvER.SteepSlopeValPct AS INT)
	,StreamValPct	          = CAST(LvER.StreamValPct	 AS INT)
	,WetlandValPct	          = CAST(LvER.WetlandValPct	 AS INT)
	,DevRightsValPct          = CAST(LvDEV.DevRightsValPct AS INT)
	,AdjacGolfValPct          = CAST(LvDES.AdjacGolfValPct AS INT)
	,AdjacGreenbeltValPct     = CAST(LvDES.AdjacGreenbeltValPct AS INT)
	,DeedRestrictValPct       = CAST(LvDES.DeedRestrictValPct AS INT)
	,EasementsValPct          = CAST(LvDES.EasementsValPct AS INT)
	,DNRLeaseValPct           = CAST(LvDES.DNRLeaseValPct AS INT)
	,NativeGrowthValPct       = CAST(LvDES.NativeGrowthValPct AS INT)
	,OtherDesigValPct         = CAST(LvDES.OtherDesigValPct AS INT)
	,AdjacGolfDollars         = CAST(LvDES.AdjacGolfDollars AS MONEY)
	,AdjacGreenbeltValDollars = CAST(LvDES.AdjacGreenbeltValDollars AS MONEY)
	FROM #SalesPop sp
	INNER JOIN [dynamics].[ptas_land] pl
	ON sp.LandGuid = pl.ptas_landid
	LEFT JOIN dynamics.vw_LandValueNuisance LvNU
	ON pl.ptas_landid = LvNU._ptas_landid_value
	LEFT JOIN dynamics.vw_LandValueEnvironmentalRestriction LvER
	ON pl.ptas_landid = LvER._ptas_landid_value
	LEFT JOIN dynamics.vw_LandValueDevelopmentRight LvDEV
	ON pl.ptas_landid = LvDEV._ptas_landid_value
	LEFT JOIN dynamics.vw_LandValueDesignations LvDES
	ON pl.ptas_landid = LvDES._ptas_landid_value
	

/*####################################END ADD ADDICIONAL COLUMNS REQUESTED BY REGIS###############################################*/


	--Insert all data in the main table to be presented in the Custom Search
	INSERT INTO #SalesPopFinal
	SELECT 
	 ROW_NUMBER() OVER(ORDER BY Pin ASC) as ID
	,ParcelId
	,LandGuid
	,SaleGuid
	,BldgGuid	
	,SaleId
	,ResArea
	,ResSubarea
	,PropType='R'
	,QSec
	,Sec
	,Twn
	,Rng
	,Neighborhood
    ,'GisSurfaceValue' = STR(GisSurfaceValue,14,12)
	,Folio
	,PlatLot
	,PlatBlock
	,Major
	,Minor
	,'LocationAddr' =CONCAT(COALESCE(TRIM(StreetNbr)+' ',''), COALESCE(TRIM(NbrFraction)+' ',''),COALESCE(TRIM(DirPrefix)+' ',''),COALESCE(TRIM(StreetName)+' ',''),COALESCE(TRIM(StreetType)+' ',''),COALESCE(TRIM(DirSuffix)+' ',''))	
	,StreetNbr
	,NbrFraction
	,DirPrefix
	,StreetName
	,StreetType
	,DirSuffix
    ,DistrictName
	,TaxpayerName
	,SellerName
	,BuyerName
	,ExciseTaxNbr
	,'SaleYear'  = DATEPART(year, SaleDate)
	,'SaleMonth' = DATEPART(month, SaleDate)
	,'SaleDate'  = convert(smalldatetime,convert(char(11), SaleDate))
	,SalePrice
	,VerifiedAtMkt
	,VerifiedBy
	,Reason
	,Instr
	,PrinUse
	,PropClass
	,'Warnings' =LTRIM(Warnings)
    ,PossibleLenderSale
    ,'HoldoutReason' = ISNULL(HoldoutReason,'')
	,'SelectDate'    = convert(smalldatetime,convert(char(11),SelectDate))
	,SelectAppr
	,SelectMethod
	,SelectReason
	,SelectLandVal
	,SelectImpsVal
	,'SelectValTotal' = SelectLandVal + SelectImpsVal
    ,'RollLandVal'    = ApprLandVal
    ,'RollImpsVal'    = ApprImpsVal
	,'RollValTotal'   = ApprLandVal   + ApprImpsVal
	,BaseLandVal
	,BaseLandValTaxYr
	,'BaseLandValDate' =convert(smalldatetime,convert(char(11),BaseLandValDate))
	,BldgRCN
	,BldgRCNLD
	,AccyRCN
	,AccyRCNLD
	,MHRCN
	,MHRCNLD
	,'TotalRCN'   =BldgRCN   + AccyRCN   + MHRCN
	,'TotalRCNLD' =BldgRCNLD + AccyRCNLD + MHRCNLD
	,ImpCnt
	,BldgNbr
	,NbrLivingUnits
	,SqFtLot
	,SqFtLotDry
	,SqFtLotSubmerged
	,'Age' =@AssmtYr + 1 - CASE 
							 WHEN YrRenovated > YrBuilt THEN YrRenovated
							 WHEN YrBuilt     > 0       THEN YrBuilt
							 ELSE @AssmtYr + 1
                           END
	,YrBuilt
	,YrRenovated
	,BldgGrade
	,BldgGradeVar
	,Condition
	,SqFtTotLiving
	,'SqFtAboveGrLiving' =SqFtTotLiving - SqFtFinBasement
	,'BathTotal' =(BathHalfCount * 0.5) + (Bath3qtrCount * 0.75) + BathFullCount
	,'FpTotal' =FpSingleStory + FpMultiStory + FpFreestanding + FpAdditional
	,'SqFtCoveredParking' =DetGarArea + CarportArea + SqFtGarageBasement + SqFtGarageAttached
	,Stories
	,SqFt1stFloor
	,SqFtHalfFloor
	,SqFt2ndFloor
	,SqFtUpperFloor
	,SqFtUnfinFull
	,SqFtUnfinHalf
	,SqFtTotBasement
	,SqFtFinBasement
	,FinBasementGrade
	,SqFtGarageBasement
	,SqFtGarageAttached
	,DaylightBasement
	,Bedrooms
	,BathHalfCount
	,Bath3qtrCount
	,BathFullCount
	,FpSingleStory
	,FpMultiStory
	,FpFreestanding
	,FpAdditional
	,PcntComplete
	,PcntNetCondition
	,Obsolescence
	,SqFtOpenPorch
	,SqFtEnclosedPorch
	,SqFtDeck
	,HeatSystem
	,HeatSource
	,BrickStone
	,AddnlCost
	,DetGarArea
	,DetGarGrade
	,DetGarEffYr
	,DetGarNetCond
	,CarportArea
	,CarportEffYr
	,CarportNetCond
	,PoolArea
	,PoolEffYr
	,PoolNetCond
	,paving
	,MiscAccyCost
	--,MHomeValue  	Hairo comment: Not calculated since the corresponding accesories ID were not migrated to the new database
	--,MHomeType	Hairo comment: Not calculated since the corresponding accesories ID were not migrated to the new database
	,DevCost
	,Flatvalue
    ,MHCount
    ,MHBldgNbr
    ,'MHType' = CASE MHType
                WHEN 4 THEN 'Real Prop'
                WHEN 0 THEN '' 
                ELSE 'Pers Prop' END
    ,MHClass
    ,MHLength
    ,MHWidth
    ,MHLivingArea
    ,MHTipOutArea
    ,MHRoomAddSqft
    ,MHSize
    ,MHYrBuilt
    ,MHCondition
    ,MHPcntNetCondition     	
	,ZoneClass
    ,CurrentZoning      --we'll need to enable this when we have zoning in EMV
	,ZoneDesignation
	,'ZoningChgDate' =convert(smalldatetime,convert(char(11),ZoningChgDate))
	,HBUAsIfVacant
	,HBUasImproved
	,PresentUse
	,WaterSystem
	,SewerSystem
	,Access
	,Topography
	,StreetSurface
	,PcntBaseLandValImpacted
	,Unbuildable
	,RestrictiveSzShape
	,MtRainier
	,Olympics
	,Cascades
	,Territorial
	,SeattleSkyline
	,PugetSound
	,LakeWashington
	,LakeSammamish
	,SmallLakeRiverCreek
	,OtherView
	,'TotalViews' =MtRainier + Olympics + Cascades + Territorial + SeattleSkyline + PugetSound 
               + LakeWashington + LakeSammamish + SmallLakeRiverCreek + OtherView
	,ViewUtilization
	,WfntLocation
	,WfntFootage
	,WfntPoorQuality
	,WfntBank
	,TidelandShoreland
	,WfntRestrictedAccess
	,LotDepthFactor
	,TrafficNoise
	,AirportNoise
	,CommonProperty
	,CurrentUseDesignation
	,HistoricSite
	,NbrBldgSites
	,Contamination
	,WfntAccessRights
	,WfntProximityInfluence
	,NativeGrowthProtEsmt
	,PowerLines
	,OtherNuisances
	,AdjacentGolfFairway
	,AdjacentGreenbelt
	,OtherDesignation
	,DeedRestrictions
	,DevelopmentRightsPurch
	,Easements
	,SpecialAssessments
	,CoalMineHazard
	,CriticalDrainage
	,ErosionHazard
	,LandfillBuffer
	,HundredYrFloodPlain
	,SeismicHazard
	,LandslideHazard
	,SteepSlopeHazard
	,Stream
	,Wetland
	,SpeciesOfConcern
	,SensitiveAreaTract
	,WaterProblems
	,OtherProblems
	,SrCitFlag    --Hairo comment NOT FOUND, No idea where to get it from, by now I added a blank
	,'TaxStatus' = TaxStat
	,'SegMergeDate' =convert(smalldatetime,convert(char(11),SegMergeDate))
	,HILastYr
	,HIValue
	,PermitNbr
	,'PermitDate' =convert(smalldatetime,convert(char(11),PermitDate))
	,PermitValue
	,PermitType
	,PermitDescr
	,Notes
/*
Missing table 
Hairo comment: I need ParcelGrpItem table to calculate this column
,CASE WHEN EXISTS (SELECT * FROM ParcelGrpItem pgi INNER JOIN ParcelGrp pg ON pgi.PgGuid = pg.PgGuid
	                   WHERE Pgi.RpGuid = sp.RpGuid and pg.PgTypeItemId = 2) THEN 'Y'
          ELSE '' END as 'GovtOwned'
*/
    ,CONVERT(varchar(16),TrendFactor) AS TrendFactor
	,TrendedPrice
    ,AssignedBoth 
	,NewConstrVal 
	,sp.PIN
	,MapPin
	--Hairo comment: ADDING NEW COLUMNS REQUESTED By REGIS
	,AirportValPct           
	,OtherNuisValPct         
	,OtherProblemsValPct     
	,PowerLinesValPct        
	,RoadAccessValPct        
	,TopoValPct				 
	,WaterProblemsValPct     
	,TrafficValPct           
	,AirportValDollars       
	,TrafficValDollars       
	,HundredYrValPct	     
	,CoalValPct		         
	,ContamValPct	         
	,DrainageValPct 	     
	,ErosionValPct	         
	,LandfillValPct 	     
	,LandslideValPct	     
	,SeismicValPct	         
	,SensitiveValPct	     
	,SpeciesValPct	         
	,SteepSlopeValPct        
	,StreamValPct	         
	,WetlandValPct	         
	,DevRightsValPct         
	,AdjacGolfValPct         
	,AdjacGreenbeltValPct    
	,DeedRestrictValPct      
	,EasementsValPct         
	,DNRLeaseValPct          
	,NativeGrowthValPct      
	,OtherDesigValPct        
	,AdjacGolfDollars        
	,AdjacGreenbeltValDollars
    FROM #SalesPop sp
   ORDER BY ExciseTaxNbr DESC, ResSubarea, Major, Minor


RETURN(0)

SQLError:
  RETURN(100001)   

END