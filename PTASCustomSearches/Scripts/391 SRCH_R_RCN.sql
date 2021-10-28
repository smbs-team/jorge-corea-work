

/*
XXXXXX PROCEDURE [dbo].[SRCH_R_ResBulkData]
        @ParcelId Nvarchar(3000)
        ,@AssmtYr	   smallint	= NULL


AS
BEGIN
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
*/
DROP TABLE IF EXISTS #Parcels ;
DROP TABLE IF EXISTS #Accessory;
DROP TABLE IF EXISTS #IdentAccy;
DROP TABLE IF EXISTS #TotAccy;
DROP TABLE IF EXISTS #MultAccy; 
DROP TABLE IF EXISTS #Bldgs ;
DROP TABLE IF EXISTS #ResBldg;
DROP TABLE IF EXISTS #BldgsForRealPropValEst;
DROP TABLE IF EXISTS #PropType;
DROP TABLE IF EXISTS #ResCostBldgComponent;
DROP TABLE IF EXISTS #ResCostBldgGradeFactor;
DROP TABLE IF EXISTS #ResCostAccyComponent;
DROP TABLE IF EXISTS #ResCostAccyGradeFactor;
DROP TABLE IF EXISTS #MHCount;
DROP TABLE IF EXISTS #AllMobiles;
DROP TABLE IF EXISTS #ShowMobiles;
DROP TABLE IF EXISTS #GetMHCost;
DROP TABLE IF EXISTS #TotalSqftCost;
DROP TABLE IF EXISTS #MainBoxCost;
DROP TABLE IF EXISTS #TagBoxCost;
DROP TABLE IF EXISTS #RCN_RCNLD;
DROP TABLE IF EXISTS #RequiredParcels;
DROP TABLE IF EXISTS #input;

CREATE TABLE #PropType
	(
	ptas_propertyTypeId UNIQUEIDENTIFIER PRIMARY KEY ,
	PropType NVARCHAR(1)
	)
	INSERT INTO #PropType
	SELECT  ptas_propertyTypeId       
			,SUBSTRING(pt.ptas_name,1,1) 
	FROM dynamics.ptas_propertytype AS pt


--ONLY ACTIVE FOR TEST PORPUSES - ONLY ACTIVE FOR TEST PORPUSES - 

DECLARE @Parcels varchar(3000)   
	   ,@AssmtYr	   smallint	= NULL

--CHECK accy RCNLD
--'042506-9022' 
--5589300250
SELECT @Parcels ='5589300250' 
--'025960-0010,025960-0020,025960-0030,025960-0040,025960-0050,025960-0060,025960-0070,025960-0080,025960-0090,025960-0100,025960-0110,025960-0120,025960-0130,025960-0140,025960-0150,025960-0160,025960-0170,025960-0180,025960-0190,025960-0200,025960-0210,025960-0220,025960-0230,025960-0240,025960-0250,025960-0260,025960-0270,025960-0280,025960-0290,025960-0300'
	   ,@AssmtYr = 2021-- NULL
--SELECT * FROM #parcels

--ONLY ACTIVE FOR TEST PORPUSES - ONLY ACTIVE FOR TEST PORPUSES - 


CREATE TABLE #Input
(Parcel char(10))

INSERT INTO #Input
Select * from dynamics.fn_ParseValue(REPLACE(@Parcels,'-',''),',')

--hairo comment: Here I´m loading a certain amount of records to work with
CREATE TABLE #RequiredParcels
(
  RecId int Identity(1,1)
 ,ParcelId uniqueidentifier
 ,Major char(6)
 ,Minor char(4)
 )


INSERT #RequiredParcels (Major, Minor)
SELECT SUBSTRING(Parcel,1,6),SUBSTRING(Parcel,7,4)
  FROM #Input


UPDATE #RequiredParcels
SET ParcelId = pd.ptas_parceldetailid 
FROM dynamics.ptas_parceldetail pd
INNER JOIN #PropType pt ON pd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
INNER JOIN #RequiredParcels rp ON pd.ptas_major = rp.Major AND pd.ptas_minor = rp.Minor
WHERE pt.PropType = 'R'
AND pd._ptas_areaid_value IS NOT null
  AND pd.ptas_applgroup = 'R'
  AND pd.ptas_splitcode = 0
  AND pd.statecode = 0
  AND pd.statuscode = 1
  AND pd.ptas_snapshottype  IS NULL

--SELECT * FROM #RequiredParcels


/*
INSERT INTO #RequiredParcels
SELECT DISTINCT TOP 500 pd.ptas_parceldetailid 
FROM dynamics.ptas_parceldetail pd
INNER JOIN #PropType pt ON pd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
INNER JOIN dynamics.ptas_accessorydetail pa1 ON pd.ptas_parceldetailid = pa1._ptas_parceldetailid_value
WHERE pt.PropType = 'R'
AND pd._ptas_areaid_value IS NOT null
  AND pd.ptas_applgroup = 'R'
  AND pd.ptas_splitcode = 0
  AND pa1.statecode = 0
  AND pa1.statuscode = 1
  AND pa1.ptas_snapshottype  IS NULL

INSERT INTO #RequiredParcels
SELECT DISTINCT TOP 500 pd.ptas_parceldetailid 
FROM dynamics.ptas_parceldetail pd
INNER JOIN #PropType pt ON pd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
INNER JOIN dynamics.ptas_accessorydetail pa1 ON pd.ptas_parceldetailid = pa1._ptas_parceldetailid_value
WHERE pt.PropType = 'R'
  AND pd.ptas_applgroup = 'R'
AND pd.ptas_parceldetailid IN 
(
'5923DE50-D08A-4F35-80F0-AB82BF27013D',
'EB05CD8A-107D-4CBF-830E-898CF8B83911',
'430DF702-B44D-4F6D-9C3D-BC1F6F978037',
'30A424D9-B3FC-47B9-9EE8-A5DEEE1AE639',
'34F6FF72-BCE5-4BA6-AC62-0006F782B5B5',
'DD581127-8552-43DD-9014-9A3D398C920C',
'E8B763AB-9604-4D3E-B2C7-CF02213E77B7',
'1FBBEF94-D875-481A-A607-0784F08AFDF7'
)
  AND pd.ptas_splitcode = 0
  AND pa1.statecode = 0
  AND pa1.statuscode = 1
  AND pa1.ptas_snapshottype  IS NULL
  AND pd._ptas_areaid_value IS NOT null
*/

DECLARE 
        @CostBillYr   		smallint
       ,@yrMultiplier 		decimal(4,3)
       --,@AreaMultiplier	decimal(4,3) 
       ,@TestProd 		char(1)
       ,@FixedCost 		decimal(9,4)
       ,@VariableCost 		decimal(9,4)
       --,@CostIndex 		decimal(6,4)
       ,@WarnCntr 		tinyint
       ,@STDGRADE		tinyint
       ,@Countdown 		int
       ,@NICntr 		tinyint
       ,@PhysInsp       char(1)
       ,@AssmtLevel     decimal(4,3)
       ,@AssmtDate      smalldatetime
	   ,@AssmtEntityId char(4)

       
--declare @ResArea varchar(3)
--        ,@ResSubArea varchar(3)
--        ,@Major varchar(6)
--        ,@AssignedTo char(4)     
       
--set @ResArea = '060'
--set @ResSubArea = ''
--set @Major = '202206'
--set @AssignedTo = ''      

DECLARE @Error int
        
SELECT @CostBillYr = @AssmtYr + 1
SELECT @AssmtLevel = CASE WHEN @AssmtYr = 2012 THEN 0.925
                          WHEN @AssmtYr = 2013 THEN 0.925
                          WHEN @AssmtYr = 2014 THEN 0.94
                          WHEN @AssmtYr = 2015 THEN 0.95
                          WHEN @AssmtYr = 2016 THEN 0.925
                          WHEN @AssmtYr = 2017 THEN 0.925
                          WHEN @AssmtYr = 2018 THEN 0.925
                          WHEN @AssmtYr = 2019 THEN 0.925
                          WHEN @AssmtYr = 2020 THEN 0.9
                          ELSE 1 END
		
if 1 = 1
begin 


CREATE TABLE #Parcels
 (
  RecId            int identity(1,1)
 ,PIN           char(10)  	DEFAULT ''
 ,MapPin        float     	DEFAULT 0 
 ,ParcelId 		uniqueidentifier
 ,LandGuid 		uniqueidentifier
 ,PropType         char(1)    DEFAULT ''
 ,ApplGroup        char(1)    DEFAULT ''
 ,ResArea          varchar(3)    DEFAULT ''
 ,ResSubArea       varchar(3)    DEFAULT ''
 ,Major			char(6)		DEFAULT '' 
 ,Minor			char(4)		DEFAULT '' 
 ,BldgGuid        uniqueidentifier     NULL 
--,PlatLot          char(14)    DEFAULT ''
--,PlatBlock        char(7)    DEFAULT ''
--,QSec             char(2)    DEFAULT ''
--,Sec              tinyint    DEFAULT 0
--,Twn              tinyint    DEFAULT 0
--,Rng              tinyint    DEFAULT 0
--,Neighborhood     int        DEFAULT 0
--,AssignedTo       char(4)    DEFAULT ''
--,GisSurfaceValue  float      DEFAULT 0 
--,TaxPayerName     varchar(80)    DEFAULT ''     
--,Folio            varchar(7)    DEFAULT ''
--,SaleId           int           DEFAULT 0 
--,SaleGuid     uniqueidentifier  NULL
--,SalePrice        int           DEFAULT 0
--,SaleDate         smalldatetime  DEFAULT ''
--,TimeAdj          decimal(19,15) DEFAULT 0
--,TrendedSalePrice int           DEFAULT ''
--,ExciseTaxNbr     int           DEFAULT 0
--,NbrPclsInSale     int           DEFAULT 0
--,VerifAtMkt        char(1)       DEFAULT ''
--,VerifiedBy        char(4)       DEFAULT ''
--,SalePropertyClass tinyint       DEFAULT 0
--,SaleWarnings     varchar(200)  DEFAULT ''
--,LastWarnId        tinyint       DEFAULT 0
--,GeneralClassif    varchar(50)    DEFAULT ''   
--,HBUAsIfVacantCode tinyint        DEFAULT 0            
--,HBUAsIfVacant      varchar(50)    DEFAULT ''     --get descr
--,HBUAsImprovedCode tinyint        DEFAULT 0
--,HBUAsImproved      varchar(50)    DEFAULT ''     --get descr
--,PresentUseCode     smallint        DEFAULT 0
--,PresentUse         varchar(50)    DEFAULT ''     --get descr
--,SqFtLot            int        DEFAULT 0
--,SqFtLotDry         int        DEFAULT 0  
--,SqFtLotSubmerged   int        DEFAULT 0
--,WaterSystem        tinyint        DEFAULT 0
--,SewerSystem        tinyint        DEFAULT 0
--,Access             tinyint        DEFAULT 0
--,Topography         tinyint        DEFAULT 0
--,StreetSurface      tinyint        DEFAULT 0
--,PcntBaseLandValImpacted int        DEFAULT 0
--,Unbuildable              tinyint        DEFAULT 0
--,RestrictiveSzShape       tinyint        DEFAULT 0
--,CurrentZoning            smallint        DEFAULT 0
--,ZoneDesignation     varchar(50)    DEFAULT ''
--,BaseLandVal              int        DEFAULT 0
--,BaseLandValTaxYr         smallint        DEFAULT 0
--,BaseLandValDate          smalldatetime        DEFAULT ''
--,XCoord              int        DEFAULT 0
--,YCoord              int        DEFAULT 0 
--,MtRainier           char(1)        DEFAULT ''
--,Olympics            char(1)        DEFAULT ''
--,Cascades            char(1)        DEFAULT ''
--,Territorial         char(1)        DEFAULT ''
--,SeattleSkyline      char(1)        DEFAULT ''
--,PugetSound          char(1)        DEFAULT ''
--,LakeWashington      char(1)        DEFAULT ''
--,LakeSammamish       char(1)        DEFAULT ''
--,SmallLakeRiverCreek char(1)        DEFAULT ''
--,OtherView           char(1)        DEFAULT ''
--,WfntLocation        char(1)        DEFAULT ''     
--,WfntFootage         int DEFAULT 0 --char(1)        DEFAULT '' 
--,WfntPoorQuality     char(1)    DEFAULT ''
--,WfntBank            int        DEFAULT ''          
--,TidelandShoreland    varchar(1)        DEFAULT ''      
--,WfntRestrictedAccess  varchar(1)        DEFAULT ''      
--,LotDepthFactor        smallint        DEFAULT 0 
--,TrafficNoise          varchar(1)        DEFAULT '' 
--,AirportNoise          int        DEFAULT 0 
--,NbrBldgSites          smallint        DEFAULT 0 
--,WfntAccessRights           char(1)    DEFAULT ''
--,WfntProximityInfluence     char(1)    DEFAULT ''
--,NativeGrowthProtEsmt       char(1)    DEFAULT ''
--,PowerLines                 char(1)    DEFAULT ''
--,OtherNuisances             char(1)    DEFAULT ''
--,AdjacentGolfFairway        char(1)    DEFAULT ''
--,AdjacentGreenbelt          char(1)    DEFAULT ''
--,OtherDesignation           char(1)    DEFAULT '' 
--,DeedRestrictions           char(1)    DEFAULT ''
--,DevelopmentRightsPurchased char(1)    DEFAULT ''
--,Easements                  char(1)    DEFAULT ''
--,CoalMineHazard                  char(1)    DEFAULT ''
--,CriticalDrainage                  char(1)    DEFAULT ''
--,ErosionHazard                  char(1)    DEFAULT ''
--,LandfillBuffer                  char(1)    DEFAULT ''
--,HundredYrFloodPlain                  char(1)    DEFAULT ''
--,SeismicHazard                  char(1)    DEFAULT ''
--,LandslideHazard                  char(1)    DEFAULT ''
--,SteepSlopeHazard                  char(1)    DEFAULT ''
--,Stream                  char(1)    DEFAULT ''
--,Wetland                  char(1)    DEFAULT ''
--,SpeciesOfConcern                  char(1)    DEFAULT ''
--,SensitiveAreaTract                  char(1)    DEFAULT ''
--,WaterProblems                  char(1)    DEFAULT ''
--,TransportationConcurrency                  char(1)    DEFAULT ''
--,OtherProblems                  char(1)    DEFAULT ''
--,ImpCntWarning         varchar(65)   DEFAULT '' 
--,ImpCnt                tinyint        DEFAULT 0 
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
,HeatSystemDesc            varchar(50)    DEFAULT ''
,HeatSource        tinyint        DEFAULT 0 
,HeatSourceDesc            varchar(50)    DEFAULT ''
,BrickStone            tinyint        DEFAULT 0 
,AddnlCost             int        DEFAULT 0 
---- accessory		
 ,SqFtCoveredParking	int		 DEFAULT 0
-- ,DetGarCount       smallint DEFAULT 0
-- ,DetGarArea		int		 DEFAULT 0
-- ,DetGarGrade		tinyint		 DEFAULT 0
-- ,DetGarEffYr		smallint	 DEFAULT 0
-- ,DetGarNetCond		tinyint		 DEFAULT 0
-- ,CarportCount      smallint   DEFAULT 0
-- ,CarportArea		int		 DEFAULT 0
-- ,CarportEffYr		smallint	 DEFAULT 0
-- ,CarportNetCond	tinyint          DEFAULT 0
-- ,PoolCount     smallint  DEFAULT 0
-- ,PoolArea		int		 DEFAULT 0
-- ,PoolEffYr		smallint	 DEFAULT 0
-- ,PoolNetCond		tinyint		 DEFAULT 0
-- ,PavingCount   smallint       DEFAULT 0
-- ,Paving		int 	         DEFAULT 0
-- ,MiscAccyCount   smallint    DEFAULT 0
-- ,MiscAccyCost		int		 DEFAULT 0
-- --,MobileHome		int		 DEFAULT 0
-- --,MHomeType		tinyint		 DEFAULT 0
-- ,DevCostCount  smallint     DEFAULT 0
-- ,DevCost		int	         DEFAULT 0
-- ,FlatValueCount smallint    DEFAULT 0
-- ,FlatValue     int          DEFAULT 0
 ,MHCountWarning varchar(75) DEFAULT ''
 ,MHCount       int        DEFAULT 0
 ,MHBldgNbr     tinyint    DEFAULT 0
 ,MHType       tinyint    DEFAULT 0
 ,MHClass       tinyint    DEFAULT 0
 ,MHLength      int        DEFAULT 0
 ,MHWidth       int        DEFAULT 0
 ,MHLivingArea  int        DEFAULT 0
 ,MHTipOutArea  int        DEFAULT 0
 ,MHRoomAddSqft int        DEFAULT 0
 ,MHSize        int        DEFAULT 0
 ,MHYrBuilt     int        DEFAULT 0
 ,MHCondition   tinyint    DEFAULT 0
 ,MHPcntNetCondition     int        DEFAULT 0
 --,EMV                      int        DEFAULT 0 
 ,BldgRCN                      int        DEFAULT 0 
 ,BldgRCNLD                      int        DEFAULT 0 
 ,AccyRCN                      int        DEFAULT 0 
 ,AccyRCNLD                      int        DEFAULT 0 
 ,MHRCN                      int        DEFAULT 0 
 ,MHRCNLD                      int        DEFAULT 0 
 ,TotalRCN                      int        DEFAULT 0 
 ,TotalRCNLD                      int        DEFAULT 0 
-- ,RevalOrMaint             char(1)    DEFAULT ''
-- ,SelectRollYr             smallint        DEFAULT 0             
-- ,SelectDate               smalldatetime    DEFAULT ''
-- ,SelectAppr               char(4)    DEFAULT ''
-- ,SelectMethodCode             tinyint        DEFAULT 0 
-- ,SelectMethod             varchar(50)   DEFAULT ''
-- ,SelectReasonCode             tinyint        DEFAULT 0 
-- ,SelectReason             varchar(50)     DEFAULT '' 
-- ,ExceptionReason         varchar(1000)   DEFAULT ''
-- ,ApprValue                   int        DEFAULT 0
-- ,EMVOverrideValPcnt         decimal(3,2)        DEFAULT 0 
-- ,EMVOverrideValDollars       int      DEFAULT 0 
-- ,AltApprModel                   varchar(100) DEFAULT ''
-- ,NewConstrValue              int        DEFAULT 0 
-- ,SelectLandVal              int        DEFAULT 0 
-- ,SelectImpsVal              int        DEFAULT 0 
-- ,SelectValTotal              int        DEFAULT 0 
-- ,RollLandVal              int        DEFAULT 0 
-- ,RollImpsVal              int        DEFAULT 0 
-- ,RollValTotal              int        DEFAULT 0 
-- ,NoteId                int   DEFAULT 0
-- ,Notes               varchar(6000)   DEFAULT ''
-- ,NewNote                  varchar(255)    DEFAULT ''
-- ,MFInterfaceFlagCode          tinyint        DEFAULT 0
-- ,MFInterfaceFlag          varchar(50)  DEFAULT '' 
-- ,RealPropId                int        DEFAULT 0 
-- ,LandID                    int        DEFAULT 0 
-- ,BldgID                    int        DEFAULT 0
 ,MobileHomeId              int        DEFAULT 0
-- ,RpGuid          uniqueidentifier     NULL      
-- ,LndGuid         uniqueidentifier     NULL     
-- ,BldgGuid        uniqueidentifier     NULL 
 ,MhGuid          uniqueidentifier     NULL 
-- ,CheckedOutTo              char(4)    DEFAULT ''
---- RCN
-- ,UnitsAdj 		decimal(3,2) 	 Default 0
-- ,EffYr 		smallint 	 Default 0
-- ,Age 			smallint 	 Default 0
-- ,BldgGradeCateg 	tinyint 	 Default 0
-- ,BldgFormula 		tinyint 	 Default 0
-- ,BldgFactor1 		decimal(9,6) 	 Default 0
-- ,BldgFactor2 		decimal(9,6) 	 Default 0
-- ,BsmtGradeCateg 	tinyint 	 Default 0
-- ,BsmtFormula 		tinyint 	 Default 0
-- ,BsmtFactor1 		decimal(9,6) 	 Default 0
-- ,BsmtFactor2 		decimal(9,6) 	 Default 0
-- ,PcntGoodBldg 		decimal(9,6) 	 Default 0
-- ,PcntGoodBsmt 		decimal(9,6) 	 Default 0
-- ,BldgGradeAdj 		decimal(3,2) 	 Default 0
-- ,finBsmtCost 		int 		 Default 0
 ,sBldgRCN 		int 		 Default 0
 ,sBldgRCNLD 		int 		 Default 0
 ,sAccyRCN 		int 		 Default 0
 ,sAccyRCNLD 		int 		 Default 0
 ,BldgRCNc 		int 		 Default 0
 ,BldgRCNLDc 		int 		 Default 0
 ,AccyRCNc 		int 		 Default 0
 ,AccyRCNLDc 		int 		 Default 0
 ,MHRCNc         int          Default 0
 ,MHRCNLDc      int          Default 0
 ,YrBltRen int Default 0
 --,EMV1 real  Default 0
 --,AltApprModelValue int Default 0
 --,EMVExceptionMsg  varchar(100)  Default ''
 --,AccyWarningMsg varchar(100) Default ''
 --,InspectionDate smalldatetime  Default ''
 --,InspectionType char(4)  Default ''
 --,IncomplMaint varchar(35) Default ''
 --,ActiveAppeals varchar(40) Default ''
 --,BldgCondWarning varchar(30) Default ''
 --,ValueSelectWarning varchar(50) Default ''
 ,AllBldgRCN int DEFAULT 0
 ,AllBldgRCNLD int DEFAULT 0
 ,AllMHRCN int DEFAULT 0
 ,AllMHRCNLD int DEFAULT 0 
 --,InadequateParkingItemId smallint DEFAULT 0
 --,InadequateParking char(1)  DEFAULT ''
)
/*
CREATE TABLE #ExtendedLand
 (
 RowId int identity(1,1)
,Landid int
,LndGuid  uniqueidentifier
,MtRainier  int  NULL
,Olympics  int  NULL
,Cascades  int  NULL
,Territorial  int  NULL
,SeattleSkyline  int  NULL
,PugetSound  int  NULL
,LakeWashington  int  NULL
,LakeSammamish  int  NULL
,SmallLakeRiverCreek  int  NULL
,OtherView  int  NULL
,WfntLocation  int  NULL
,WfntFootage  int  NULL
,WfntPoorQuality  int  NULL
,WfntBank  int  NULL
,TidelandShoreland  int  NULL
,WfntRestrictedAccess  int  NULL
,LotDepthFactor  int  NULL
,TrafficNoise  int  NULL
,AirportNoise  int  NULL
,NbrBldgSites  int  NULL
,WfntAccessRights  int  NULL
,WfntProximityInfluence  int  NULL
,NativeGrowthProtEsmt  tinyint  NULL
,PowerLines  tinyint  NULL
,OtherNuisances  tinyint  NULL
,AdjacentGolfFairway  tinyint  NULL
,AdjacentGreenbelt  tinyint  NULL
,OtherDesignation  tinyint  NULL
,DeedRestrictions  tinyint  NULL
,DevelopmentRightsPurchased  tinyint  NULL
,Easements  tinyint  NULL
,CoalMineHazard  tinyint  NULL
,CriticalDrainage  tinyint  NULL
,ErosionHazard  tinyint  NULL
,LandfillBuffer  tinyint  NULL
,HundredYrFloodPlain  tinyint  NULL
,SeismicHazard  tinyint  NULL
,LandslideHazard  tinyint  NULL
,SteepSlopeHazard  tinyint  NULL
,Stream  tinyint  NULL
,Wetland  tinyint  NULL
,SpeciesOfConcern  tinyint  NULL
,SensitiveAreaTract  tinyint  NULL
,WaterProblems  tinyint  NULL
,TransportationConcurrency  tinyint  NULL
,OtherProblems  tinyint  NULL
,Topography smallint NULL
)

--CREATE TABLE #TaxAcctReceivable
-- (
-- RowId int identity(1,1)
-- ,RpGuid uniqueidentifier
-- ,AcctNbr char(12)
-- ,ApprLandVal int
-- ,ApprImpsVal int
-- )
 */
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
 ,ResArea          varchar(3)    DEFAULT ''
 ,AreaCostIndex 		decimal(6,4) --Hairo comment: since we are going TO calculate parcels FROM different Areas, we need TO ADD this VALUE calculated BY Area
 ,PRIMARY KEY(rowID)
 ,UNIQUE(ParcelId, rowID)
 ,UNIQUE(BldgGuid, rowID)
 ,UNIQUE(ParcelId, BldgGuid, AccyType, rowID)
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

 
CREATE TABLE #IdentAccy (
  rowID  	int	not null  identity(1,1)
 ,ParcelId UNIQUEIDENTIFIER
 ,ResArea               varchar(3)    DEFAULT ''
 ,BldgGuid uniqueidentifier
 ,PRIMARY KEY(rowID)
 ,UNIQUE(ParcelId, BldgGuid, rowID)
)


CREATE TABLE #TotAccy(
  rowID  	int	not null  identity(1,1)
 ,ParcelId uniqueidentifier
 ,BldgGuid uniqueidentifier
 ,AccyType int
 ,AccyCount int 
 ,Size int
 ,Grade tinyint
 ,EffYr smallint
 ,PcntNetCondition smallint
 ,AccyValue int
 ,PRIMARY KEY(rowID)
 ,UNIQUE(ParcelId, BldgGuid, AccyType, rowID)
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
/*
CREATE TABLE #MultMult  (  
  rowID  	int	not null  identity(1,1)
 ,RpGuid uniqueidentifier
 ,AccyType int
 ,PRIMARY KEY(rowID)
)

CREATE TABLE #NoteId(
  NoteId int
 ,MaxNoteInstance int
)


CREATE TABLE #NoteInstance (
    NoteId int
   ,NoteInstance tinyint
   ,AssmtEntityId char(4)
   ,UpdateDate smalldatetime
   ,Note nvarchar(max)
  )
  
CREATE TABLE #AltApprModelValues (
    Propid int identity(1,1)
    ,RealpropId int
    ,RpGuid uniqueidentifier
    ,BaseLandVal int
    ,PcntComplete tinyint
    ,Obsolescence tinyint
    ,PcntNetCondition  tinyint
    ,EMV int
    ,BldgRCNLD int
    ,AccyRCNLD int
    ,MHRCNLD int
    ,TotalRCNLD int
    ,AltApprModelSaved varchar(100)
    ,AltApprModelValue int
  )
  
CREATE TABLE #SaleWarnings (
 rowID  int not null identity(1,1)
 ,SaleGuid uniqueidentifier
 ,WarningId int
 ,WarningItemId int
)
*/

CREATE TABLE #Bldgs (
 rowId int identity(1,1)
 ,ParcelId uniqueidentifier
 ,BldgGuid uniqueidentifier
-- ,BldgId int
 )
 
 

--To get cost of all buildings. We won't be returning the data but we will calculate and include cost in TotalRCN and TotalRCNLD 
--Use #Parcels so it can use the cost stored procedures that AreaData uses 
CREATE TABLE #ResBldg (
  RowId                 int identity(1,1)
 ,ParcelId              uniqueidentifier
-- ,RealPropId          int
 ,LandGuid              UNIQUEIDENTIFIER
 ,ResArea               varchar(3)    DEFAULT ''
 ,AreaCostIndex 		decimal(6,4) --Hairo comment: since we are going TO calculate parcels FROM different Areas, we need TO ADD this VALUE calculated BY Area
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

CREATE TABLE #BldgsForRealPropValEst(
 rowId int identity(1,1)
 ,ParcelId uniqueidentifier
 ,BldgGuid uniqueidentifier
 ,LandId uniqueidentifier
 ,BldgId int
 ,sBldgRCN int
 ,sBldgRCNLD int
 )   				       
 
END 

 
 
INSERT #Parcels
 (
 PIN
,MapPin 
,Major
,Minor
,PropType
,ApplGroup
,ParcelId
,LandGuid
,ResArea
,ResSubArea
 )
SELECT pd.ptas_major+pd.ptas_minor
,CONVERT(FLOAT,pd.ptas_major+pd.ptas_minor)
,pd.ptas_major
,pd.ptas_minor
,pt.PropType
,pd.ptas_applgroup
,pd.ptas_parceldetailid
,pd._ptas_landid_value
,CAST(pa.ptas_areanumber AS INT) 
,CAST(sa.ptas_name AS INT) 
FROM dynamics.ptas_parceldetail pd
INNER JOIN #RequiredParcels rp ON pd.ptas_parceldetailid = rp.ParcelId
INNER JOIN #PropType pt  ON pd._ptas_propertytypeid_value = pt.ptas_propertytypeid
INNER JOIN [dynamics].[ptas_area] pa ON  pd._ptas_areaid_value= pa.ptas_areaid 
INNER JOIN dynamics.ptas_subarea sa ON pa.ptas_areaid = sa._ptas_areaid_value AND sa.ptas_subareaid = pd._ptas_subareaid_value
--WHERE pt.PropType = 'R' 
--AND pd.ptas_applgroup = 'R'
--AND pd.ptas_parceldetailid  IN ('7D8B68FA-D09B-45CC-990A-07508E3E1957','369F98DB-952E-448F-BE05-AD2DA9F90E63')
--hay que crear una tabla para cargar los parcelid por medio del PIN
--puede ser intermedia, ya que va de 1 o miles de parcelas


/*
SELECT * FROM #RequiredParcels rp ORDER BY rp.ParcelId
SELECT * FROM #Parcels ORDER BY ParcelId
*/

/*
residential
parcel major +minor
building id
accesory ID
123456-0000,
456321-5555
*/
UPDATE #Parcels
     SET BldgGuid            = b.ptas_buildingdetailid
     	--,BldgNbr             = b.ptas_buildingnumber
     	,NbrLivingUnits      = COALESCE(b.ptas_units,0)
     	,YrBuilt             = COALESCE(y1.ptas_name,0)
     	,YrRenovated         = COALESCE(y2.ptas_name,0)
     	,BldgGrade           = COALESCE(b.ptas_buildinggrade,0)
     	,BldgGradeVar        = COALESCE(b.ptas_gradevariance,0)
     	--,Condition           = b.ptas_res_buildingcondition
     	,SqFtTotLiving       = COALESCE(b.ptas_totalliving_sqft,0)
     	--,SqFtAboveGrLiving   = b.ptas_totalliving_sqft - b.ptas_finbsmt_sqft
     	--,BathTotal           = (COALESCE(B.ptas_12baths * 0.5,0)) + (COALESCE(b.ptas_34baths * 0.75,0)) + COALESCE(b.ptas_fullbathnbr,0)
     	--,FpTotal             = COALESCE(b.ptas_single_fireplace,0) + COALESCE(b.ptas_multi_fireplace,0) + COALESCE(b.ptas_fr_std_fireplace,0) + COALESCE(b.ptas_addl_fireplace,0)
     	,SqFtCoveredParking  = 0 
     	,Stories             = COALESCE(b.ptas_numberofstoriesdecimal,0)
     	,SqFt1stFloor        = COALESCE(b.ptas_1stflr_sqft,0)
     	,SqFtHalfFloor       = COALESCE(b.ptas_halfflr_sqft,0)
     	,SqFt2ndFloor        = COALESCE(b.ptas_2ndflr_sqft,0)
     	,SqFtUpperFloor      = COALESCE(b.ptas_upperflr_sqft,0)
     	,SqFtUnfinFull       = COALESCE(b.ptas_unfinished_full_sqft,0)
     	,SqFtUnfinHalf       = COALESCE(b.ptas_unfinished_half_sqft,0)
     	,SqFtTotBasement     = COALESCE(b.ptas_totalbsmt_sqft,0)
     	,SqFtFinBasement     = COALESCE(b.ptas_finbsmt_sqft,0)
     	,FinBasementGrade    = COALESCE(b.ptas_res_basementgrade,0)
     	,SqFtGarageBasement  = COALESCE(b.ptas_basementgarage_sqft,0)
     	,SqFtGarageAttached  = COALESCE(b.ptas_attachedgarage_sqft,0)	
     	,DaylightBasement    = CASE WHEN b.ptas_daylightbasement = 1 THEN 'Y' ELSE '' END
		,Bedrooms            = COALESCE(b.ptas_bedroomnbr,0)
     	,BathHalfCount       = COALESCE(b.ptas_12baths,0)
     	,Bath3qtrCount       = COALESCE(b.ptas_34baths,0)
     	,BathFullCount       = COALESCE(b.ptas_fullbathnbr,0)
     	,FpSingleStory       = COALESCE(b.ptas_single_fireplace,0)
     	,FpMultiStory        = COALESCE(b.ptas_multi_fireplace,0)
     	,FpFreestanding      = COALESCE(b.ptas_fr_std_fireplace,0)
     	,FpAdditional        = COALESCE(b.ptas_addl_fireplace,0)
     	,PcntComplete        = COALESCE(b.ptas_percentcomplete,0)
     	,PcntNetCondition    = COALESCE(b.ptas_percentnetcondition,0)
     	,Obsolescence        = COALESCE(b.ptas_buildingobsolescence,0)
     	,SqFtOpenPorch       = COALESCE(b.ptas_openporch_sqft,0)
     	,SqFtEnclosedPorch   = COALESCE(b.ptas_enclosedporch_sqft,0)
     	,SqFtDeck            = COALESCE(b.ptas_deck_sqft,0)
     	,HeatSystem          = COALESCE(b.ptas_residentialheatingsystem,0)
     	,HeatSource          = COALESCE(b.ptas_res_heatsource,0)
     	,BrickStone          = COALESCE(b.ptas_percentbrickorstone,0)
     	,AddnlCost           = COALESCE(b.ptas_additionalcost,0)
     	,ViewUtilization     = CASE WHEN b.ptas_viewutilizationrating IS NOT NULL  THEN 'Y' ELSE '' END
	FROM #Parcels rs 
   INNER JOIN [dynamics].[vw_BuildingNumberONE] b 
      ON rs.ParcelId = b._ptas_parceldetailid_value
   LEFT JOIN [dynamics].[ptas_year] y1
      ON b._ptas_yearbuiltid_value = y1.ptas_yearid
   LEFT JOIN [dynamics].[ptas_year] y2
      ON b._ptas_yearrenovatedid_value = y2.ptas_yearid

/*
insert into #Parcels 
select rs.ParcelId, b.ptas_buildingdetailid
	FROM #Parcels rs 
   INNER JOIN dynamics.vw_ResBldg b 
      ON rs.ParcelId = b._ptas_parceldetailid_value
   LEFT JOIN [dynamics].[ptas_year] y1
      ON b._ptas_yearbuiltid_value = y1.ptas_yearid
   LEFT JOIN [dynamics].[ptas_year] y2
      ON b._ptas_yearrenovatedid_value = y2.ptas_yearid

*/	  

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
	,ResArea
 )
SELECT
	 rbd.ParcelId
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
	,COALESCE(y1.ptas_name,0)
	,COALESCE(y2.ptas_name,0)
	,COALESCE(bd.ptas_percentcomplete,0)
	,COALESCE(bd.ptas_buildingobsolescence,0)
	,COALESCE(bd.ptas_percentnetcondition,0)
	,COALESCE(bd.ptas_res_buildingcondition,0)
	,CASE WHEN bd.ptas_viewutilizationrating IS NOT NULL  THEN 'Y' ELSE '' END
	,COALESCE(bd.ptas_openporch_sqft,0)
	,COALESCE(bd.ptas_enclosedporch_sqft,0)
	,COALESCE(bd.ptas_deck_sqft,0)
	,COALESCE(bd.ptas_residentialheatingsystem,0)
	,COALESCE(bd.ptas_res_heatsource,0)
	,COALESCE(bd.ptas_percentbrickorstone,0)
	,COALESCE(bd.ptas_additionalcost,0)
	,bd.ptas_buildingdetailid
	,rbd.ResArea
 --SELECT  bd._ptas_parceldetailid_value,rbd.parcelid 
 FROM [dynamics].[ptas_buildingdetail] bd 
INNER JOIN #Parcels rbd 
   ON bd._ptas_parceldetailid_value = rbd.ParcelId
INNER JOIN [dynamics].[ptas_propertytype] dpt
   ON bd._ptas_propertytypeid_value = dpt.ptas_propertytypeid
  AND dpt.ptas_description = 'Residential'
 LEFT JOIN [dynamics].[ptas_year] y1
   ON bd._ptas_yearbuiltid_value = y1.ptas_yearid
 LEFT JOIN [dynamics].[ptas_year] y2
   ON bd._ptas_yearrenovatedid_value = y2.ptas_yearid
 

INSERT #Bldgs	
SELECT bd._ptas_parceldetailid_value,bd.ptas_buildingdetailid
	FROM [dynamics].[ptas_buildingdetail] bd 
    INNER JOIN #Parcels rbd 
    ON bd._ptas_parceldetailid_value = rbd.ParcelId
    INNER JOIN [dynamics].[ptas_propertytype] dpt
    ON bd._ptas_propertytypeid_value = dpt.ptas_propertytypeid
    AND dpt.ptas_description = 'Residential'		


INSERT #IdentAccy (ParcelId,ResArea)
    SELECT DISTINCT rs.ParcelId, rs.ResArea
    FROM #Parcels rs



INSERT INTO #Accessory(ParcelId,ResArea,AccyType,Size,Grade,EffYr,PcntNetCondition,AccyValue,BldgGuid,Component,Age,Factor,RCN,RCNLD)
SELECT a._ptas_parceldetailid_value
	  ,ia.ResArea
	  ,a.ptas_resaccessorytype
	  ,COALESCE(a.ptas_size,0)
	  ,COALESCE(a.ptas_buildinggrade,0)
	  ,COALESCE(a.ptas_effectiveyear,0)
	  ,COALESCE(a.ptas_percentnetcondition,0)
	  ,COALESCE(a.ptas_accessoryvalue,0)
	  ,a._ptas_buildingdetailid_value
	  ,''
	  ,0
	  ,0
	  ,0
	  ,0
  FROM dynamics.ptas_accessorydetail a INNER JOIN #IdentAccy ia ON a._ptas_parceldetailid_value = ia.ParcelId
 WHERE a.ptas_resaccessorytype in (1, 2, 3, 6, 7, 8)
 
 SELECT * FROM #ResBldg rb

 SELECT * FROM #Accessory
 ORDER BY BldgGuid
 

--SELECT TOP 1000 a._ptas_effectiveyearid_value,a.ptas_effectiveyear
--FROM dynamics.ptas_accessorydetail a


--WHERE a._ptas_effectiveyearid_value is NOT null

 INSERT #TotAccy
    SELECT ParcelId, BldgGuid, AccyType, Count(*) ,sum(Size), max(Grade), max(EffYr), max(PcntNetCondition), sum(AccyValue)
    FROM #Accessory a (NOLOCK)
    GROUP BY ParcelId, BldgGuid, AccyType

--SELECT * FROM #TotAccy
	

/******************************************************************************/
-----------BldgRCN and BldgRCNLD
/******************************************************************************/

     

SET @TestProd = 'P'

SET @yrMultiplier = (	SELECT Multiplier	 
			FROM rp.ResCostMultiplier (NOLOCK)
			WHERE ResArea = '000' AND RollYr = @CostBillYr AND TestProd = @TestProd )

--Hairo comment: this is not required anymore, since we are calculating this value for each area in each record
--SET @AreaMultiplier = (	SELECT Multiplier
--			FROM rp.ResCostMultiplier (NOLOCK)
--			--WHERE convert(int,ResArea) = @ResArea AND RollYr = @CostBillYr AND TestProd = @TestProd )
--			WHERE ResArea = @ResArea AND RollYr = @CostBillYr AND TestProd = @TestProd )

--Set @CostIndex by area for each parcel, since we are evaluating several parcels from diferent Areas
UPDATE #ResBldg
   SET AreaCostIndex = @yrMultiplier * Multiplier
  FROM #ResBldg rb 
 INNER JOIN rp.ResCostMultiplier rcm (NOLOCK) ON rb.ResArea = CAST(rcm.ResArea AS INT)
 WHERE RollYr = @CostBillYr AND TestProd = @TestProd

--Set @CostIndex by area for each parcel, since we are evaluating several parcels from diferent Areas
UPDATE #Accessory
   SET AreaCostIndex = @yrMultiplier * Multiplier
  FROM #Accessory rb 
 INNER JOIN rp.ResCostMultiplier rcm (NOLOCK) ON rb.ResArea = CAST(rcm.ResArea AS INT)
 WHERE RollYr = @CostBillYr AND TestProd = @TestProd

--SET @CostIndex = @yrMultiplier * @areaMultiplier

--SELECT @CostIndex , @yrMultiplier , @areaMultiplier

UPDATE #ResBldg
     Set UnitsAdj = CASE NbrLivingUnits
					WHEN 3 THEN 1.25
					WHEN 2 THEN 1.12
					ELSE 1
					END
  	,Age = CASE WHEN YrRenovated > 0 THEN
                    CASE WHEN @CostBillYr - 1 - (YrRenovated - 5) < 0 THEN 0
                	ELSE @CostBillYr - 1 - (YrRenovated - 5)
                 	END
              	WHEN YrBuilt > 0 THEN
                    CASE WHEN @CostBillYr - 1 - YrBuilt < 0 THEN 0
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


UPDATE #ResBldg
	Set Bldgformula = FormulaType
	,Bldgfactor1 = Factor1
   	,Bldgfactor2 = Factor2
FROM #ResBldg rs, rp.ResDeprSpec rds
WHERE rs.BldgGradeCateg = rds.GradeCateg
	AND rs.Condition = rds.Condition
  	AND rs.Age BETWEEN rds.AgeFrom AND rds.AgeTo
  	AND rds.RollYr = @CostBillYr
  	AND rds.TestProd = @TestProd


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


--EXEC DBO.QMAS_R_GetAreaData2b_Updates_8e 
UPDATE #ResBldg
    Set bldgGradeAdj = CASE WHEN BldgGrade = 13 and BldgGradeVar > 0 THEN 0.01 * BldgGradeVar
                    ELSE 0
                    END


--EXEC DBO.QMAS_R_GetAreaData2b_Inserts_5 @CostBillYr, @TestProd
INSERT #ResCostBldgComponent
    SELECT
	 SUM(rcc.FixedCost * rccp.Bath3qtrCountFixedCost) Bath3qtrCountFixedCost,SUM(rcc.VariableCost * rccp.Bath3qtrCountVariableCost) Bath3qtrCountVariableCost                         

    ,SUM(rcc.MinArea * rccp.Bath3qtrCountMinArea) Bath3qtrCountMinArea
	,SUM(rcc.FixedCost * rccp.BathFullCountFixedCost) BathFullCountFixedCost,SUM(rcc.VariableCost * rccp.BathFullCountVariableCost) BathFullCountVariableCost                          

    ,SUM(rcc.MinArea * rccp.BathFullCountMinArea) BathFullCountMinArea
	,SUM(rcc.FixedCost * rccp.BathHalfCountFixedCost) BathHalfCountFixedCost,SUM(rcc.VariableCost * rccp.BathHalfCountVariableCost) BathHalfCountVariableCost                          

    ,SUM(rcc.MinArea * rccp.BathHalfCountMinArea) BathHalfCountMinArea
	,SUM(rcc.FixedCost * rccp.BathTotalAdjustFixedCost) BathTotalAdjustFixedCost,SUM(rcc.VariableCost * rccp.BathTotalAdjustVariableCost) BathTotalAdjustVariableCost                      

    ,SUM(rcc.MinArea * rccp.BathTotalAdjustMinArea) BathTotalAdjustMinArea
	,SUM(rcc.FixedCost * rccp.EffBrickStoneAreaFixedCost) EffBrickStoneAreaFixedCost,SUM(rcc.VariableCost * rccp.EffBrickStoneAreaVariableCost) EffBrickStoneAreaVariableCost                  

    ,SUM(rcc.MinArea * rccp.EffBrickStoneAreaMinArea) EffBrickStoneAreaMinArea
	,SUM(rcc.FixedCost * rccp.FpAdditionalFixedCost) FpAdditionalFixedCost,SUM(rcc.VariableCost * rccp.FpAdditionalVariableCost) FpAdditionalVariableCost                            

    ,SUM(rcc.MinArea * rccp.FpAdditionalMinArea) FpAdditionalMinArea
	,SUM(rcc.FixedCost * rccp.FpFreestandingFixedCost) FpFreestandingFixedCost,SUM(rcc.VariableCost * rccp.FpFreestandingVariableCost) FpFreestandingVariableCost                        

    ,SUM(rcc.MinArea * rccp.FpFreestandingMinArea) FpFreestandingMinArea
	,SUM(rcc.FixedCost * rccp.FpMultiStoryFixedCost) FpMultiStoryFixedCost,SUM(rcc.VariableCost * rccp.FpMultiStoryVariableCost) FpMultiStoryVariableCost                            

    ,SUM(rcc.MinArea * rccp.FpMultiStoryMinArea) FpMultiStoryMinArea
	,SUM(rcc.FixedCost * rccp.FpSingleStoryFixedCost) FpSingleStoryFixedCost,SUM(rcc.VariableCost * rccp.FpSingleStoryVariableCost) FpSingleStoryVariableCost                          

    ,SUM(rcc.MinArea * rccp.FpSingleStoryMinArea) FpSingleStoryMinArea
	,SUM(rcc.FixedCost * rccp.SqFt1stFloorFixedCost) SqFt1stFloorFixedCost,SUM(rcc.VariableCost * rccp.SqFt1stFloorVariableCost) SqFt1stFloorVariableCost                            

    ,SUM(rcc.MinArea * rccp.SqFt1stFloorMinArea) SqFt1stFloorMinArea
	,SUM(rcc.FixedCost * rccp.SqFt2ndFloorFixedCost) SqFt2ndFloorFixedCost,SUM(rcc.VariableCost * rccp.SqFt2ndFloorVariableCost) SqFt2ndFloorVariableCost                            

    ,SUM(rcc.MinArea * rccp.SqFt2ndFloorMinArea) SqFt2ndFloorMinArea
	,SUM(rcc.FixedCost * rccp.SqFtUpperFloorFixedCost) SqFtUpperFloorFixedCost,SUM(rcc.VariableCost * rccp.SqFtUpperFloorVariableCost) SqFtUpperFloorVariableCost                        

    ,SUM(rcc.MinArea * rccp.SqFtUpperFloorMinArea) SqFtUpperFloorMinArea
	,SUM(rcc.FixedCost * rccp.SqFtDeckFixedCost) SqFtDeckFixedCost,SUM(rcc.VariableCost * rccp.SqFtDeckVariableCost) SqFtDeckVariableCost                                    

    ,SUM(rcc.MinArea * rccp.SqFtDeckMinArea) SqFtDeckMinArea
	,SUM(rcc.FixedCost * rccp.SqFtElectricBBHeatedFixedCost) SqFtElectricBBHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtElectricBBHeatedVariableCost) SqFtElectricBBHeatedVariableCost            

    ,SUM(rcc.MinArea * rccp.SqFtElectricBBHeatedMinArea) SqFtElectricBBHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtEnclosedPorchFixedCost) SqFtEnclosedPorchFixedCost,SUM(rcc.VariableCost * rccp.SqFtEnclosedPorchVariableCost) SqFtEnclosedPorchVariableCost                  

    ,SUM(rcc.MinArea * rccp.SqFtEnclosedPorchMinArea) SqFtEnclosedPorchMinArea
	,SUM(rcc.FixedCost * rccp.SqFtFinBasementFixedCost) SqFtFinBasementFixedCost,SUM(rcc.VariableCost * rccp.SqFtFinBasementVariableCost) SqFtFinBasementVariableCost                      

    ,SUM(rcc.MinArea * rccp.SqFtFinBasementMinArea) SqFtFinBasementMinArea
	,SUM(rcc.FixedCost * rccp.SqFtForcedAirHeatedFixedCost) SqFtForcedAirHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtForcedAirHeatedVariableCost) SqFtForcedAirHeatedVariableCost              

    ,SUM(rcc.MinArea * rccp.SqFtForcedAirHeatedMinArea) SqFtForcedAirHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtFWFurnaceHeatedFixedCost) SqFtFWFurnaceHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtFWFurnaceHeatedVariableCost) SqFtFWFurnaceHeatedVariableCost              

    ,SUM(rcc.MinArea * rccp.SqFtFWFurnaceHeatedMinArea) SqFtFWFurnaceHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtGarageAttachedFixedCost) SqFtGarageAttachedFixedCost,SUM(rcc.VariableCost * rccp.SqFtGarageAttachedVariableCost) SqFtGarageAttachedVariableCost                

    ,SUM(rcc.MinArea * rccp.SqFtGarageAttachedMinArea) SqFtGarageAttachedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtGravityHeatedFixedCost) SqFtGravityHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtGravityHeatedVariableCost) SqFtGravityHeatedVariableCost                  

    ,SUM(rcc.MinArea * rccp.SqFtGravityHeatedMinArea) SqFtGravityHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtHalfFloorFixedCost) SqFtHalfFloorFixedCost,SUM(rcc.VariableCost * rccp.SqFtHalfFloorVariableCost) SqFtHalfFloorVariableCost                          

    ,SUM(rcc.MinArea * rccp.SqFtHalfFloorMinArea) SqFtHalfFloorMinArea
	,SUM(rcc.FixedCost * rccp.SqFtHeatPumpHeatedFixedCost) SqFtHeatPumpHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtHeatPumpHeatedVariableCost) SqFtHeatPumpHeatedVariableCost                

    ,SUM(rcc.MinArea * rccp.SqFtHeatPumpHeatedMinArea) SqFtHeatPumpHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtHotWaterHeatedFixedCost) SqFtHotWaterHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtHotWaterHeatedVariableCost) SqFtHotWaterHeatedVariableCost                

    ,SUM(rcc.MinArea * rccp.SqFtHotWaterHeatedMinArea) SqFtHotWaterHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtOpenPorchFixedCost) SqFtOpenPorchFixedCost,SUM(rcc.VariableCost * rccp.SqFtOpenPorchVariableCost) SqFtOpenPorchVariableCost                          

    ,SUM(rcc.MinArea * rccp.SqFtOpenPorchMinArea) SqFtOpenPorchMinArea
	,SUM(rcc.FixedCost * rccp.SqFtRadiantHeatedFixedCost) SqFtRadiantHeatedFixedCost,SUM(rcc.VariableCost * rccp.SqFtRadiantHeatedVariableCost) SqFtRadiantHeatedVariableCost                  

    ,SUM(rcc.MinArea * rccp.SqFtRadiantHeatedMinArea) SqFtRadiantHeatedMinArea
	,SUM(rcc.FixedCost * rccp.SqFtTotBasementFixedCost) SqFtTotBasementFixedCost,SUM(rcc.VariableCost * rccp.SqFtTotBasementVariableCost) SqFtTotBasementVariableCost                      

    ,SUM(rcc.MinArea * rccp.SqFtTotBasementMinArea) SqFtTotBasementMinArea
	,SUM(rcc.FixedCost * rccp.SqFtUnfinFullFixedCost) SqFtUnfinFullFixedCost        
	,SUM(rcc.VariableCost * rccp.SqFtUnfinFullVariableCost) SqFtUnfinFullVariableCost,SUM(rcc.MinArea *rccp.SqFtUnfinFullMinArea) SqFtUnfinFullMinArea
	,SUM(rcc.FixedCost * rccp.SqFtUnfinHalfFixedCost) SqFtUnfinHalfFixedCost,SUM(rcc.VariableCost * rccp.SqFtUnfinHalfVariableCost) SqFtUnfinHalfVariableCost                          

    ,SUM(rcc.MinArea * rccp.SqFtUnfinHalfMinArea) SqFtUnfinHalfMinArea
	,SUM(rcc.FixedCost * rccp.SqFtUnheatedLivingFixedCost) SqFtUnheatedLivingFixedCost,SUM(rcc.VariableCost * rccp.SqFtUnheatedLivingVariableCost) SqFtUnheatedLivingVariableCost                

    ,SUM(rcc.MinArea * rccp.SqFtUnheatedLivingMinArea) SqFtUnheatedLivingMinArea
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

	
-- ALL THIS CONVERSION TO INT IS TO MAINTAIN THE SAME ROUNDING AS THE ORIGINAL SP CalcResBldgCost & CalcResAccyCost
--EXEC DBO.QMAS_R_GetAreaData2b_Updates_9a @CostIndex
UPDATE #ResBldg
	Set BldgRCNc =  -- Bathrooms
            FLOOR((FLOOR((FLOOR(Bath3qtrCountfixedCost * rs.Bath3qtrCount/(CASE WHEN rs.Bath3QtrCount = 0 THEN 1 
                                                       ELSE rs.Bath3QtrCount 
                                                       END) + Bath3qtrCountvariableCost * rs.Bath3qtrCount * rs.AreaCostIndex)

          + FLOOR(BathFullCountfixedCost * rs.BathFullCount/(CASE WHEN rs.BathFullCount = 0 THEN 1 
                                                       ELSE rs.BathFullCount 
                                                       END) + BathFullCountvariableCost * rs.BathFullCount * rs.AreaCostIndex)
          + FLOOR(BathHalfCountfixedCost * rs.BathHalfCount/(CASE WHEN rs.BathHalfCount = 0 THEN 1 
                                                       ELSE rs.BathHalfCount 
                                                       END) + BathHalfCountvariableCost * rs.BathHalfCount * rs.AreaCostIndex)) * rcgf.BathFullCount
          + rs.NbrLivingUnits * rcgf.BathTotalAdjust * rs.AreaCostIndex)


          -- BrickStone
          + FLOOR((rs.SqFt1stFloor * 0.64 + 560 * rs.SqFt1stFloor/(CASE WHEN rs.SqFt1stFloor = 0 THEN 1 ELSE rs.SqFt1stFloor END)
          + rs.SqFt2ndFloor * 0.3912 + 513.45 * rs.SqFt2ndFloor/(CASE WHEN rs.SqFt2ndFloor = 0 THEN 1 ELSE rs.SqFt2ndFloor END)
          + rs.SqFtHalfFloor * 0.1208 - 19.63 * rs.SqFtHalfFloor/(CASE WHEN rs.SqFtHalfFloor = 0 THEN 1 ELSE rs.SqFtHalfFloor END)) * rs.BrickStone / 100.00 * rs.AreaCostIndex * rcgf.EffBrickStoneArea)

          -- Fireplace
          + FLOOR((FpAdditionalfixedCost / (CASE WHEN rs.FpAdditional = 0 THEN 1 -- we don't want to divide by zero. The multiplication of FpAdditional will zero out this section anyway
                                           WHEN rs.FpAdditional > rcc.FpAdditionalMinArea THEN rs.FpAdditional 
                                           ELSE rcc.FpAdditionalMinArea 
                                      END) + FpAdditionalVariableCost) * rs.FpAdditional * rs.AreaCostIndex * rcgf.FpAdditional)
          + FLOOR((FpFreestandingfixedCost / (CASE WHEN rs.FpFreestanding = 0 THEN 1
                                             WHEN rs.FpFreestanding > rcc.FpFreestandingMinArea THEN rs.FpFreestanding 
                                             ELSE rcc.FpFreestandingMinArea 
                                        END) + FpFreestandingVariableCost) * rs.FpFreestanding * rs.AreaCostIndex * rcgf.FpFreestanding)
          + FLOOR((FpMultiStoryfixedCost / (CASE WHEN rs.FpMultiStory = 0 THEN 1
                                           WHEN rs.FpMultiStory > rcc.FpMultiStoryMinArea THEN rs.FpMultiStory 
                                           ELSE rcc.FpMultiStoryMinArea 
                                      END) + FpMultiStoryVariableCost) * rs.FpMultiStory * rs.AreaCostIndex * rcgf.FpMultiStory)
          + FLOOR((FpSingleStoryfixedCost / (CASE WHEN rs.FpSingleStory = 0 THEN 1
                                            WHEN rs.FpSingleStory > rcc.FpSingleStoryMinArea THEN rs.FpSingleStory 
                                            ELSE rcc.FpSingleStoryMinArea 
                                       END) + FpSingleStoryVariableCost) * rs.FpSingleStory * rs.AreaCostIndex * rcgf.FpSingleStory)

          -- decks/porches
          + FLOOR((SqFtDeckfixedCost / (CASE WHEN rs.SqFtDeck = 0 THEN 1
                                       WHEN rs.SqFtDeck > rcc.SqFtDeckMinArea THEN rs.SqFtDeck 
                                       ELSE rcc.SqFtDeckMinArea 
                                  END) + SqFtDeckVariableCost) * rs.SqFtDeck * rs.AreaCostIndex * rcgf.SqFtDeck)
          + FLOOR((SqFtEnclosedPorchfixedCost / (CASE WHEN rs.SqFtEnclosedPorch = 0 THEN 1
                                                WHEN rs.SqFtEnclosedPorch > rcc.SqFtEnclosedPorchMinArea THEN rs.SqFtEnclosedPorch 
                                                ELSE rcc.SqFtEnclosedPorchMinArea 
                                           END) + SqFtEnclosedPorchVariableCost) * rs.SqFtEnclosedPorch * rs.AreaCostIndex * rcgf.SqFtEnclosedPorch)
          + FLOOR((SqFtOpenPorchfixedCost / (CASE WHEN rs.SqFtOpenPorch = 0 THEN 1
                                            WHEN rs.SqFtOpenPorch > rcc.SqFtOpenPorchMinArea THEN rs.SqFtOpenPorch 
                                            ELSE rcc.SqFtOpenPorchMinArea 
                                       END) + SqFtOpenPorchVariableCost) * rs.SqFtOpenPorch * rs.AreaCostIndex * rcgf.SqFtOpenPorch)

          -- basements
          + ISNULL(FLOOR((SqFtFinBasementfixedCost / (CASE WHEN rs.SqFtFinBasement = 0 THEN 1
                                              WHEN rs.SqFtFinBasement > rcc.SqFtFinBasementMinArea THEN rs.SqFtFinBasement 
                                              ELSE rcc.SqFtFinBasementMinArea 
                                              END) + SqFtFinBasementVariableCost) * rs.SqFtFinBasement * rs.AreaCostIndex * rcgfbsmt.SqFtFinBasement),0)
          + ISNULL(FLOOR((SqFtTotBasementfixedCost / (CASE WHEN rs.SqFtTotBasement = 0 THEN 1
                                              WHEN rs.SqFtTotBasement > rcc.SqFtTotBasementMinArea THEN rs.SqFtTotBasement 
                                              ELSE rcc.SqFtTotBasementMinArea 
                                         END) + SqFtTotBasementVariableCost) * rs.SqFtTotBasement * rs.AreaCostIndex * rcgf.SqFtTotBasement),0)

          -- garage
          + FLOOR((SqFtGarageAttachedfixedCost / (CASE WHEN rs.SqFtGarageAttached = 0 THEN 1
                                                 WHEN rs.SqFtGarageAttached > rcc.SqFtGarageAttachedMinArea THEN rs.SqFtGarageAttached 
                                                 ELSE rcc.SqFtGarageAttachedMinArea 
                                            END) + SqFtGarageAttachedVariableCost) * rs.SqFtGarageAttached * rs.AreaCostIndex * rcgf.SqFtGarageAttached)

          -- heat
          + FLOOR((SqFtUnheatedLivingfixedCost / (CASE WHEN rs.SqFtTotLiving = 0 THEN 1
                                                 WHEN rs.SqFtTotLiving > rcc.SqFtUnheatedLivingMinArea THEN rs.SqFtTotLiving 
                                                 ELSE rcc.SqFtUnheatedLivingMinArea 
                                            END) + SqFtUnheatedLivingVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtUnheatedLiving * (CASE WHEN rs.HeatSystem = 0 THEN 1 ELSE 0 END)
          + (SqFtFWFurnaceHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtFWFurnaceHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtFWFurnaceHeatedMinArea END) + SqFtFWFurnaceHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtFWFurnaceHeated * (CASE WHEN rs.HeatSystem = 1 THEN 1 ELSE 0 END)
          + (SqFtGravityHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtGravityHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtGravityHeatedMinArea END) + SqFtGravityHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtGravityHeated * (

CASE WHEN rs.HeatSystem = 2 THEN 1 ELSE 0 END)
          + (SqFtRadiantHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtRadiantHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtRadiantHeatedMinArea END) + SqFtRadiantHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtRadiantHeated * (

CASE WHEN rs.HeatSystem = 3 THEN 1 ELSE 0 END)
          + (SqFtElectricBBHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtElectricBBHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtElectricBBHeatedMinArea END) + SqFtElectricBBHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtElectricBBHeated * (CASE WHEN rs.HeatSystem = 4 THEN 1 ELSE 0 END)
          + (SqFtForcedAirHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtForcedAirHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtForcedAirHeatedMinArea END) + SqFtForcedAirHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtForcedAirHeated * (CASE WHEN rs.HeatSystem = 5 THEN 1 ELSE 0 END)
          + (SqFtHotWaterHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtHotWaterHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtHotWaterHeatedMinArea END) + SqFtHotWaterHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtHotWaterHeated * (CASE WHEN rs.HeatSystem = 6 THEN 1 ELSE 0 END)
          + (SqFtHeatPumpHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtHeatPumpHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtHeatPumpHeatedMinArea END) + SqFtHeatPumpHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtHeatPumpHeated * (CASE WHEN rs.HeatSystem = 7 THEN 1 ELSE 0 END))

          -- SqFt
          + FLOOR((SqFt1stFloorfixedCost / (CASE WHEN rs.SqFt1stFloor > rcc.SqFt1stFloorMinArea THEN rs.SqFt1stFloor ELSE rcc.SqFt1stFloorMinArea END) + SqFt1stFloorVariableCost) * rs.SqFt1stFloor * rs.AreaCostIndex * (rcgf.SqFt1stFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFt2ndFloorfixedCost / (CASE WHEN rs.SqFt2ndFloor > rcc.SqFt2ndFloorMinArea THEN rs.SqFt2ndFloor ELSE rcc.SqFt2ndFloorMinArea END) + SqFt2ndFloorVariableCost) * rs.SqFt2ndFloor * rs.AreaCostIndex * (rcgf.SqFt2ndFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtUpperFloorfixedCost / (CASE WHEN rs.SqFtUpperFloor > rcc.SqFtUpperFloorMinArea THEN rs.SqFtUpperFloor ELSE rcc.SqFtUpperFloorMinArea END) + SqFtUpperFloorVariableCost) * rs.SqFtUpperFloor * rs.AreaCostIndex * (rcgf.SqFtUpperFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtHalfFloorfixedCost / (CASE WHEN rs.SqFtHalfFloor > rcc.SqFtHalfFloorMinArea THEN rs.SqFtHalfFloor ELSE rcc.SqFtHalfFloorMinArea END) + SqFtHalfFloorVariableCost) * rs.SqFtHalfFloor * rs.AreaCostIndex * (rcgf.SqFtHalfFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtUnfinFullfixedCost / (CASE WHEN rs.SqFtUnfinFull > rcc.SqFtUnfinFullMinArea THEN rs.SqFtUnfinFull ELSE rcc.SqFtUnfinFullMinArea END) + SqFtUnfinFullVariableCost) * rs.SqFtUnfinFull * rs.AreaCostIndex * (rcgf.SqFtUnfinFull + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtUnfinHalffixedCost / (CASE WHEN rs.SqFtUnfinHalf > rcc.SqFtUnfinHalfMinArea THEN rs.SqFtUnfinHalf ELSE rcc.SqFtUnfinHalfMinArea END) + SqFtUnfinHalfVariableCost) * rs.SqFtUnfinHalf * rs.AreaCostIndex * (rcgf.SqFtUnfinHalf + bldgGradeAdj) * unitsAdj)

          -- Additional Cost
          + FLOOR(rs.AddnlCost * rs.AreaCostIndex)) / 100) * 100

   	,finBsmtCost = ISNULL((SqFtFinBasementfixedCost / (CASE WHEN rs.SqFtFinBasement > rcc.SqFtFinBasementMinArea THEN rs.SqFtFinBasement ELSE rcc.SqFtFinBasementMinArea END) + SqFtFinBasementVariableCost) * rs.SqFtFinBasement * rs.AreaCostIndex * rcgfbsmt.SqFtFinBasement,0)
	FROM #ResBldg rs inner join #ResCostBldgComponent rcc on (1=1)
                  inner join #ResCostBldgGradeFactor rcgf on (rs.BldgGrade = rcgf.Grade)
                  left join #ResCostBldgGradeFactor rcgfbsmt on (rs.FinBasementGrade = rcgfbsmt.Grade)


----------------------------------------------------------------------------------------------------------------
/*
SELECT 
        FLOOR((FLOOR(Bath3qtrCountfixedCost * rs.Bath3qtrCount/(CASE WHEN rs.Bath3QtrCount = 0 THEN 1 
                                                       ELSE rs.Bath3QtrCount 
                                                       END) + Bath3qtrCountvariableCost * rs.Bath3qtrCount * rs.AreaCostIndex)

          + FLOOR(BathFullCountfixedCost * rs.BathFullCount/(CASE WHEN rs.BathFullCount = 0 THEN 1 
                                                       ELSE rs.BathFullCount 
                                                       END) + BathFullCountvariableCost * rs.BathFullCount * rs.AreaCostIndex)
          + FLOOR(BathHalfCountfixedCost * rs.BathHalfCount/(CASE WHEN rs.BathHalfCount = 0 THEN 1 
                                                       ELSE rs.BathHalfCount 
                                                       END) + BathHalfCountvariableCost * rs.BathHalfCount * rs.AreaCostIndex)) * rcgf.BathFullCount
          + rs.NbrLivingUnits * rcgf.BathTotalAdjust * rs.AreaCostIndex)
		  bath
		  ,
          -- BrickStone
           FLOOR((rs.SqFt1stFloor * 0.64 + 560 * rs.SqFt1stFloor/(CASE WHEN rs.SqFt1stFloor = 0 THEN 1 ELSE rs.SqFt1stFloor END)
          + rs.SqFt2ndFloor * 0.3912 + 513.45 * rs.SqFt2ndFloor/(CASE WHEN rs.SqFt2ndFloor = 0 THEN 1 ELSE rs.SqFt2ndFloor END)
          + rs.SqFtHalfFloor * 0.1208 - 19.63 * rs.SqFtHalfFloor/(CASE WHEN rs.SqFtHalfFloor = 0 THEN 1 ELSE rs.SqFtHalfFloor END)) * rs.BrickStone / 100.00 * rs.AreaCostIndex * rcgf.EffBrickStoneArea)
		  BrickStone,
          -- Fireplace
          + FLOOR((FpAdditionalfixedCost / (CASE WHEN rs.FpAdditional = 0 THEN 1 -- we don't want to divide by zero. The multiplication of FpAdditional will zero out this section anyway
                                           WHEN rs.FpAdditional > rcc.FpAdditionalMinArea THEN rs.FpAdditional 
                                           ELSE rcc.FpAdditionalMinArea 
                                      END) + FpAdditionalVariableCost) * rs.FpAdditional * rs.AreaCostIndex * rcgf.FpAdditional)
          + FLOOR((FpFreestandingfixedCost / (CASE WHEN rs.FpFreestanding = 0 THEN 1
                                             WHEN rs.FpFreestanding > rcc.FpFreestandingMinArea THEN rs.FpFreestanding 
                                             ELSE rcc.FpFreestandingMinArea 
                                        END) + FpFreestandingVariableCost) * rs.FpFreestanding * rs.AreaCostIndex * rcgf.FpFreestanding)
          + FLOOR((FpMultiStoryfixedCost / (CASE WHEN rs.FpMultiStory = 0 THEN 1
                                           WHEN rs.FpMultiStory > rcc.FpMultiStoryMinArea THEN rs.FpMultiStory 
                                           ELSE rcc.FpMultiStoryMinArea 
                                      END) + FpMultiStoryVariableCost) * rs.FpMultiStory * rs.AreaCostIndex * rcgf.FpMultiStory)
          + FLOOR((FpSingleStoryfixedCost / (CASE WHEN rs.FpSingleStory = 0 THEN 1
                                            WHEN rs.FpSingleStory > rcc.FpSingleStoryMinArea THEN rs.FpSingleStory 
                                            ELSE rcc.FpSingleStoryMinArea 
                                       END) + FpSingleStoryVariableCost) * rs.FpSingleStory * rs.AreaCostIndex * rcgf.FpSingleStory)

          Fireplace,
		  -- decks/porches
           FLOOR((SqFtDeckfixedCost / (CASE WHEN rs.SqFtDeck = 0 THEN 1
                                       WHEN rs.SqFtDeck > rcc.SqFtDeckMinArea THEN rs.SqFtDeck 
                                       ELSE rcc.SqFtDeckMinArea 
                                  END) + SqFtDeckVariableCost) * rs.SqFtDeck * rs.AreaCostIndex * rcgf.SqFtDeck)
          + FLOOR((SqFtEnclosedPorchfixedCost / (CASE WHEN rs.SqFtEnclosedPorch = 0 THEN 1
                                                WHEN rs.SqFtEnclosedPorch > rcc.SqFtEnclosedPorchMinArea THEN rs.SqFtEnclosedPorch 
                                                ELSE rcc.SqFtEnclosedPorchMinArea 
                                           END) + SqFtEnclosedPorchVariableCost) * rs.SqFtEnclosedPorch * rs.AreaCostIndex * rcgf.SqFtEnclosedPorch)
          + FLOOR((SqFtOpenPorchfixedCost / (CASE WHEN rs.SqFtOpenPorch = 0 THEN 1
                                            WHEN rs.SqFtOpenPorch > rcc.SqFtOpenPorchMinArea THEN rs.SqFtOpenPorch 
                                            ELSE rcc.SqFtOpenPorchMinArea 
                                       END) + SqFtOpenPorchVariableCost) * rs.SqFtOpenPorch * rs.AreaCostIndex * rcgf.SqFtOpenPorch)
			decks,
          -- basements
           ISNULL(FLOOR((SqFtFinBasementfixedCost / (CASE WHEN rs.SqFtFinBasement = 0 THEN 1
                                              WHEN rs.SqFtFinBasement > rcc.SqFtFinBasementMinArea THEN rs.SqFtFinBasement 
                                              ELSE rcc.SqFtFinBasementMinArea 
                                              END) + SqFtFinBasementVariableCost) * rs.SqFtFinBasement * rs.AreaCostIndex * rcgfbsmt.SqFtFinBasement),0)
          + ISNULL(FLOOR((SqFtTotBasementfixedCost / (CASE WHEN rs.SqFtTotBasement = 0 THEN 1
                                              WHEN rs.SqFtTotBasement > rcc.SqFtTotBasementMinArea THEN rs.SqFtTotBasement 
                                              ELSE rcc.SqFtTotBasementMinArea 
                                         END) + SqFtTotBasementVariableCost) * rs.SqFtTotBasement * rs.AreaCostIndex * rcgf.SqFtTotBasement),0)
			basements,
          -- garage
           FLOOR((SqFtGarageAttachedfixedCost / (CASE WHEN rs.SqFtGarageAttached = 0 THEN 1
                                                 WHEN rs.SqFtGarageAttached > rcc.SqFtGarageAttachedMinArea THEN rs.SqFtGarageAttached 
                                                 ELSE rcc.SqFtGarageAttachedMinArea 
                                            END) + SqFtGarageAttachedVariableCost) * rs.SqFtGarageAttached * rs.AreaCostIndex * rcgf.SqFtGarageAttached)
			garage,
          -- heat
           FLOOR((SqFtUnheatedLivingfixedCost / (CASE WHEN rs.SqFtTotLiving = 0 THEN 1
                                                 WHEN rs.SqFtTotLiving > rcc.SqFtUnheatedLivingMinArea THEN rs.SqFtTotLiving 
                                                 ELSE rcc.SqFtUnheatedLivingMinArea 
                                            END) + SqFtUnheatedLivingVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtUnheatedLiving * (CASE WHEN rs.HeatSystem = 0 THEN 1 ELSE 0 END)
          + (SqFtFWFurnaceHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtFWFurnaceHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtFWFurnaceHeatedMinArea END) + SqFtFWFurnaceHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtFWFurnaceHeated * (CASE WHEN rs.HeatSystem = 1 THEN 1 ELSE 0 END)
          + (SqFtGravityHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtGravityHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtGravityHeatedMinArea END) + SqFtGravityHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtGravityHeated * (

CASE WHEN rs.HeatSystem = 2 THEN 1 ELSE 0 END)
          + (SqFtRadiantHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtRadiantHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtRadiantHeatedMinArea END) + SqFtRadiantHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtRadiantHeated * (

CASE WHEN rs.HeatSystem = 3 THEN 1 ELSE 0 END)
          + (SqFtElectricBBHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtElectricBBHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtElectricBBHeatedMinArea END) + SqFtElectricBBHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtElectricBBHeated * (CASE WHEN rs.HeatSystem = 4 THEN 1 ELSE 0 END)
          + (SqFtForcedAirHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtForcedAirHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtForcedAirHeatedMinArea END) + SqFtForcedAirHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtForcedAirHeated * (CASE WHEN rs.HeatSystem = 5 THEN 1 ELSE 0 END)
          + (SqFtHotWaterHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtHotWaterHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtHotWaterHeatedMinArea END) + SqFtHotWaterHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtHotWaterHeated * (CASE WHEN rs.HeatSystem = 6 THEN 1 ELSE 0 END)
          + (SqFtHeatPumpHeatedfixedCost / (CASE WHEN rs.SqFtTotLiving > rcc.SqFtHeatPumpHeatedMinArea THEN rs.SqFtTotLiving ELSE rcc.SqFtHeatPumpHeatedMinArea END) + SqFtHeatPumpHeatedVariableCost) * rs.SqFtTotLiving * rs.AreaCostIndex * rcgf.SqFtHeatPumpHeated * (CASE WHEN rs.HeatSystem = 7 THEN 1 ELSE 0 END))

         heat ,
		  -- SqFt
           FLOOR((SqFt1stFloorfixedCost / (CASE WHEN rs.SqFt1stFloor > rcc.SqFt1stFloorMinArea THEN rs.SqFt1stFloor ELSE rcc.SqFt1stFloorMinArea END) + SqFt1stFloorVariableCost) * rs.SqFt1stFloor * rs.AreaCostIndex * (rcgf.SqFt1stFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFt2ndFloorfixedCost / (CASE WHEN rs.SqFt2ndFloor > rcc.SqFt2ndFloorMinArea THEN rs.SqFt2ndFloor ELSE rcc.SqFt2ndFloorMinArea END) + SqFt2ndFloorVariableCost) * rs.SqFt2ndFloor * rs.AreaCostIndex * (rcgf.SqFt2ndFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtUpperFloorfixedCost / (CASE WHEN rs.SqFtUpperFloor > rcc.SqFtUpperFloorMinArea THEN rs.SqFtUpperFloor ELSE rcc.SqFtUpperFloorMinArea END) + SqFtUpperFloorVariableCost) * rs.SqFtUpperFloor * rs.AreaCostIndex * (rcgf.SqFtUpperFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtHalfFloorfixedCost / (CASE WHEN rs.SqFtHalfFloor > rcc.SqFtHalfFloorMinArea THEN rs.SqFtHalfFloor ELSE rcc.SqFtHalfFloorMinArea END) + SqFtHalfFloorVariableCost) * rs.SqFtHalfFloor * rs.AreaCostIndex * (rcgf.SqFtHalfFloor + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtUnfinFullfixedCost / (CASE WHEN rs.SqFtUnfinFull > rcc.SqFtUnfinFullMinArea THEN rs.SqFtUnfinFull ELSE rcc.SqFtUnfinFullMinArea END) + SqFtUnfinFullVariableCost) * rs.SqFtUnfinFull * rs.AreaCostIndex * (rcgf.SqFtUnfinFull + bldgGradeAdj) * unitsAdj)
          + FLOOR((SqFtUnfinHalffixedCost / (CASE WHEN rs.SqFtUnfinHalf > rcc.SqFtUnfinHalfMinArea THEN rs.SqFtUnfinHalf ELSE rcc.SqFtUnfinHalfMinArea END) + SqFtUnfinHalfVariableCost) * rs.SqFtUnfinHalf * rs.AreaCostIndex * (rcgf.SqFtUnfinHalf + bldgGradeAdj) * unitsAdj)

          SqFt,
		  -- Additional Cost
           FLOOR(rs.AddnlCost * rs.AreaCostIndex) AS Additional_Cost
		   ,rs.AddnlCost col9
		   ,rs.AreaCostIndex col10 
		  --,') / 100) * 100' colx

   	,finBsmtCost = ISNULL((SqFtFinBasementfixedCost / (CASE WHEN rs.SqFtFinBasement > rcc.SqFtFinBasementMinArea THEN rs.SqFtFinBasement ELSE rcc.SqFtFinBasementMinArea END) + SqFtFinBasementVariableCost) * rs.SqFtFinBasement * rs.AreaCostIndex * rcgfbsmt.SqFtFinBasement,0)
	INTO #tempo
	FROM #ResBldg rs inner join #ResCostBldgComponent rcc on (1=1)
                  inner join #ResCostBldgGradeFactor rcgf on (rs.BldgGrade = rcgf.Grade)
                  left join #ResCostBldgGradeFactor rcgfbsmt on (rs.FinBasementGrade = rcgfbsmt.Grade)
*/
--SELECT *--, FLOOR((col1+col2+col3+col4+col5+col6+col7+col8) / 100) * 100,FLOOR((col1+col2+col3+col4+col5+col6+col7+col8)) FROM #tempo
--SELECT col10 AS costIndex,bath, garage AS GarCost,SqFt AS FirstFlCost,basements AS TotBsmCost,Fireplace AS TotFPCost,'----->',* FROM #tempo

/*
SELECT 
    ISNULL(FLOOR((SqFtFinBasementfixedCost / (CASE WHEN rs.SqFtFinBasement = 0 THEN 1
                                              WHEN rs.SqFtFinBasement > rcc.SqFtFinBasementMinArea THEN rs.SqFtFinBasement 
                                              ELSE rcc.SqFtFinBasementMinArea 
                                              END) + SqFtFinBasementVariableCost) * rs.SqFtFinBasement * rs.AreaCostIndex * rcgfbsmt.SqFtFinBasement
											  ),0)
          , ISNULL(FLOOR((SqFtTotBasementfixedCost / (CASE WHEN rs.SqFtTotBasement = 0 THEN 1
                                              WHEN rs.SqFtTotBasement > rcc.SqFtTotBasementMinArea THEN rs.SqFtTotBasement 
                                              ELSE rcc.SqFtTotBasementMinArea 
                                         END) + SqFtTotBasementVariableCost) * rs.SqFtTotBasement * rs.AreaCostIndex * rcgf.SqFtTotBasement),0) 
-- SqFtFinBasementfixedCost ,rs.SqFtFinBasement,rcc.SqFtFinBasementMinArea 
-- ,SqFtFinBasementVariableCost,rs.AreaCostIndex ,rcgfbsmt.SqFtFinBasement
--,SqFtTotBasementfixedCost,rs.SqFtTotBasement,rcc.SqFtTotBasementMinArea 
--,SqFtTotBasementVariableCost, rcgf.SqFtTotBasement
FROM #ResBldg rs inner join #ResCostBldgComponent rcc on (1=1)
                  inner join #ResCostBldgGradeFactor rcgf on (rs.BldgGrade = rcgf.Grade)
                  left join #ResCostBldgGradeFactor rcgfbsmt on (rs.FinBasementGrade = rcgfbsmt.Grade)
*/
--SELECT * FROM #ResCostBldgComponent
--SELECT * FROM #ResBldg

	UPDATE #ResBldg
	SET BldgRCNLDc = FLOOR((CASE WHEN PcntNetCondition > 0 THEN BLDGRCNc * PcntNetCondition / 100.00
                                  ELSE ((BLDGRCNc - finBsmtCost) * pcntGoodBldg + finBsmtCost * pcntGoodBsmt )
                                       * (1 - (CASE WHEN Obsolescence > 100 THEN 100 
                                               ELSE Obsolescence 
                                               END) / 100.00) 
                                       * (CASE WHEN PcntComplete > 0 THEN PcntComplete 
                                               ELSE 100 
                                               END) / 100.00
                                  END) /100) * 100

--SELECT '('+BLDGRCNc +' - '+ finBsmtCost+') * '+ pcntGoodBldg +' + '+finBsmtCost+' * '+ pcntGoodBsmt +'*'+
--SELECT PcntNetCondition,BLDGRCNc , finBsmtCost, pcntGoodBldg ,finBsmtCost, pcntGoodBsmt, Obsolescence,PcntComplete FROM #ResBldg

--SELECT BldgRCNLDc,sBldgRCNLD,BLDGRCNc,sBldgRCN  FROM #ResBldg

	
	UPDATE #ResBldg
	SET sBldgRCN   = COALESCE(FLOOR(BLDGRCNc  / 1000) * 1000,0)
	   ,sBldgRCNLD = COALESCE(FLOOR(BLDGRCNLDc / 1000) * 1000,0)
	
--SELECT sBldgRCN,sBldgRCNLD,* FROM #ResBldg
--TESTING 

	UPDATE #Parcels
    SET BldgRCN   = rb.sBldgRCN
       ,BldgRCNLD = rb.sBldgRCNLD
    FROM #ResBldg rb INNER JOIN #Parcels rs ON rb.ParcelId = rs.ParcelId AND rb.BldgGuid = rs.BldgGuid

/*
SELECT BldgRCN,	   BldgRCNLD,*FROM #Parcels a
SELECT ParcelId ,BldgGuid ,* FROM #Parcels 
SELECT rb.sBldgRCN, rb.sBldgRCNLD
    FROM #ResBldg rb INNER JOIN #Parcels rs ON rb.ParcelId = rs.ParcelId AND rb.BldgGuid = rs.BldgGuid
*/

--SELECT BldgRCN ,BldgRCNLD FROM  #ResBldg 
--SELECT * FROM #BldgsForRealPropValEst

	INSERT #BldgsForRealPropValEst
	SELECT ParcelId,BldgGuid,LandGuid,BldgNbr,sBldgRCN,sBldgRCNLD FROM #ResBldg

	UPDATE #Parcels
	SET AllBldgRCN = ISNULL((SELECT SUM(sBldgRCN) FROM #BldgsForRealPropValEst e
	                  WHERE e.ParcelId = rbd.ParcelId),0)
	    ,AllBldgRCNLD = ISNULL((SELECT SUM(sBldgRCNLD) FROM #BldgsForRealPropValEst e
	                    WHERE e.ParcelId = rbd.ParcelId),0)
    FROM #Parcels rbd


/**********END BldgRCN and BLDGRCNLD*******************/

/************************************************************************************************************************************************
                                            ACCYRCN & ACCYRCNLD
*************************************************************************************************************************************************/

--EXEC DBO.QMAS_R_GetAreaData2b_Inserts_6 @CostBillYr, @TestProd


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
    
SET @STDGRADE = 7



--EXEC DBO.QMAS_R_GetAreaData2b_Updates_10a @CostBillYr, @STDGRADE
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

--SELECT @CostBillYr , a.EffYr ,a.AccyType FROM #Accessory a

--EXEC DBO.QMAS_R_GetAreaData2b_Updates_10b 
UPDATE #Accessory
	SET Factor = CASE WHEN AccyType BETWEEN 6 AND 7 THEN -0.05
                  WHEN Grade < 4 THEN -0.015
                  WHEN Grade BETWEEN 4 AND 7 THEN -0.01
                  ELSE -0.00857
             END
--EXEC DBO.QMAS_R_GetAreaData2b_Updates_10c @CostIndex

--SELECT * FROM #Accessory

UPDATE #Accessory
     SET RCN = CASE 
	           WHEN AccyType = 1 THEN (rcc.SqFtDetachGarageFixedCost / (CASE WHEN a.Size = 0 THEN 1
                                                                            WHEN a.Size > rcc.SqFtDetachGarageMinArea THEN a.Size 
                                                                            ELSE rcc.SqFtDetachGarageMinArea
                                                                            END)  + rcc.SqFtDetachGarageVariableCost) * a.AreaCostIndex * a.Size * rcgf.SqFtDetachGarage
               WHEN AccyType = 2 THEN (rcc.SqFtCarportFixedCost / (CASE WHEN a.Size = 0 THEN 1
                                                                       WHEN a.Size > rcc.SqFtCarportMinArea THEN a.Size 
                                                                       ELSE rcc.SqFtCarportMinArea
                                                                       END)  + rcc.SqFtCarportVariableCost) * a.AreaCostIndex * a.Size * rcgf.SqFtCarport
               WHEN AccyType = 3 THEN (rcc.ConcretePavingFixedCost / (CASE WHEN a.Size = 0 THEN 1
                                                                          WHEN a.Size > rcc.ConcretePavingMinArea THEN a.Size 
                                                                          ELSE rcc.ConcretePavingMinArea
                                                                          END)  + rcc.ConcretePavingVariableCost) * a.AreaCostIndex * a.Size * rcgf.ConcretePaving
               WHEN AccyType BETWEEN 6 AND 7 THEN (rcc.SqFtPoolFixedCost / (CASE WHEN a.Size = 0 THEN 1
                                                                           WHEN a.Size > rcc.SqFtPoolMinArea THEN a.Size 
                                                                           ELSE rcc.SqFtPoolMinArea
                                                                           END)  + rcc.SqFtPoolVariableCost) * a.AreaCostIndex * a.Size * rcgf.SqFtPool
          END
FROM #Accessory a, #ResCostAccyComponent rcc, #ResCostAccyGradeFactor rcgf
WHERE a.Grade=rcgf.Grade

--SELECT rcc.SqFtDetachGarageFixedCost , a.Size,rcc.SqFtDetachGarageMinArea ,rcc.SqFtDetachGarageVariableCost
--,a.AreaCostIndex ,rcgf.SqFtDetachGarage
--FROM #Accessory a, #ResCostAccyComponent rcc, #ResCostAccyGradeFactor rcgf
--WHERE a.Grade=rcgf.Grade
--

--SELECT * FROM #Accessory


--SELECT * FROM #Accessory
--testing 

UPDATE #Accessory
SET RCN = AccyValue
WHERE AccyType between 8 and 13

--SELECT * FROM #Accessory

--EXEC DBO.QMAS_R_GetAreaData2b_Updates_10d
UPDATE #Accessory
	SET RCNLD = CASE WHEN AccyType IN (3, 8, 9, 10, 12, 13) THEN RCN
                 ELSE (FLOOR((CASE WHEN PcntNetCondition > 0 THEN RCN * PcntNetCondition / 100.00
                                         ELSE RCN * EXP(Factor * Age)
                               END) / 5) * 5) 
            END
			
--This is for updating RealPropValEst for Accessories

--SELECT * FROM #Accessory
--SELECT * FROM #Parcels p

--SELECT a.*--a.RCN 
--FROM #Accessory a 
--INNER JOIN #Parcels rs
--ON a.ParcelId = rs.ParcelId and (a.BldgGuid = rs.BldgGuid OR rs.BldgGuid IS NULL)


--SELECT 
--     AccyRCN = ISNULL((SELECT FLOOR(SUM(FLOOR(a.RCN))/1000) * 1000 FROM #Accessory a WHERE a.ParcelId = rs.ParcelId and (a.BldgGuid = rs.BldgGuid OR rs.BldgGuid IS NULL)  ),0)
--   	,AccyRCNLD = ISNULL((SELECT FLOOR(SUM(a.RCNLD)/1000) * 1000 FROM #Accessory a WHERE a.ParcelId = rs.ParcelId and (a.BldgGuid = rs.BldgGuid OR rs.BldgGuid IS NULL) ),0)
--FROM #Parcels rs

UPDATE #Parcels
     SET AccyRCN = ISNULL((SELECT FLOOR(SUM(FLOOR(a.RCN))/1000) * 1000 FROM #Accessory a WHERE a.ParcelId = rs.ParcelId and (a.BldgGuid = rs.BldgGuid OR rs.BldgGuid IS NULL)  ),0)
   	,AccyRCNLD = ISNULL((SELECT FLOOR(SUM(a.RCNLD)/1000) * 1000 FROM #Accessory a WHERE a.ParcelId = rs.ParcelId and (a.BldgGuid = rs.BldgGuid OR rs.BldgGuid IS NULL) ),0)
FROM #Parcels rs


/******************END AccyRCN and AccyRCNLD**********************/


/***************************MOBILE HOMES DATA SECTION****************************************************/ 
--If multiple mobile homes, we'll get MHType=4, AcctStatus not in (7,8), BldgNbr=1  

/*
Hairo comment:
			  I´m using [dynamics].[ptas_condounit] to calculate the MOBILE Homes, but I still dont know 
			  how to apply this filter "a.AcctStatusItemId IN (1,4,10)" in the new database, it is related
			  to the Status of the  account, for reference run this query in Real Property database 
			  "select * from luitem2 where  lutypeid = 230"

INSERT #MHCount
SELECT r.RpGuid,count(*)
FROM #Parcels r INNER JOIN ResAreaPcl rap ON r.RpGuid = rap.RpGuid
                    INNER JOIN MHAcct a ON r.ParcelId = a.RpGuid
WHERE a.AcctStatusItemId IN (1,4,10) AND a.RpGuid IS NOT NULL
GROUP BY r.RpGuid
*/

INSERT #MHCount
SELECT r.ParcelId, COUNT(1) 
FROM #Parcels r 
INNER JOIN [dynamics].[ptas_condounit] cu  ON r.ParcelId = cu._ptas_parcelid_value
WHERE cu.ptas_mobilehometype > 0
and cu._ptas_parcelid_value is not null
GROUP BY r.ParcelId

/*CREATE TABLE #AllMobiles(RpGuid uniqueidentifier, MhGuid uniqueidentifier, MHType int
   ,AcctStatus int, BldgNbr tinyint
   ,MHCount int,MHClass tinyint, MHLength int, MHWidth int, MHLivingArea int
   ,MHTipOutArea int, MHRoomAddSqft int, MHSize int, MHYrBuilt int, MHCondition tinyint, MHPcntNetCondition int
   ,MHRCN int, MHRCNLD int, MobileHomeId int,LandId int)
   */
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
FROM MHAcct a INNER JOIN #Parcels rbd ON a.RpGuid=rbd.RpGuid
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
FROM #Parcels r 
INNER JOIN [dynamics].[ptas_condounit] cu   ON r.ParcelId = cu._ptas_parcelid_value
LEFT JOIN [dynamics].[ptas_year] 	   py01 ON py01.ptas_yearid = cu._ptas_yearbuildid_value
WHERE cu.ptas_mobilehometype > 0
and cu._ptas_parcelid_value is not null

UPDATE #AllMobiles
SET MHCount = mc.MHCount
FROM #MHCount mc INNER JOIN #AllMobiles am ON mc.ParcelId = am.ParcelId

/*
CREATE TABLE #ShowMobiles (RpGuid uniqueidentifier, MhGuid uniqueidentifier, MHType int
   ,AcctStatus int, BldgNbr tinyint
   ,MHCount int,MHClass tinyint, MHLength int, MHWidth int, MHLivingArea int
   ,MHTipOutArea int, MHRoomAddSqft int, MHSize int, MHYrBuilt int, MHCondition tinyint, MHPcntNetCondition int
   ,MHRCN int, MHRCNLD int, MobileHomeId int, LandId int)
  */

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

UPDATE #Parcels
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
FROM #Parcels rbd INNER JOIN #ShowMobiles mym ON rbd.ParcelId = mym.ParcelId
/*
UPDATE #Parcels
SET MHCountWarning = 'RealProp MHCount>1, val est for First MH only - select value in Real Prop'
    ,MobileHomeId = 0
FROM #Parcels rbd
WHERE EXISTS (SELECT COUNT(*) FROM MHAcct acc 
              WHERE acc.RpGuid = rbd.RpGuid AND acc.AcctStatusItemId IN (1,4,10) AND acc.MHTypeItemId = 4
              GROUP BY acc.RpGuid
              HAVING COUNT(*)>1) 

*/


/***************BEGINNING OF MARSHALL & SWIFT COST SECTION***************************************/

/*
CREATE TABLE #GetMHCost (id int identity(1,1),MHGuid uniqueidentifier,RpGuid uniqueidentifier,MHId int,MHTypeItemId smallint,AcctStatusItemId smallint
                         ,ExpectedLife smallint,PercentGood decimal(5,4),RCN int,RCNLD int)
						 */
INSERT #GetMHCost
SELECT MhGuid,ParcelId,MobileHomeId,MHType,AcctStatus,0,0,0,0
FROM #AllMobiles


/*#################################### ASSR_R_CalcMHCost (BEGIN)###############################################*/
--EXEC ASSR_R_CalcMHCost @AssmtYr


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

/********************************** ASSR_R_CalcMHCost (END)*******************************************************************************************************************/

UPDATE #AllMobiles
SET MHRCN = g.RCN
    ,MHRCNLD = g.RCNLD
FROM #GetMHCost g INNER JOIN #AllMobiles am ON g.ParcelID = am.ParcelID and g.MhGuid = am.MhGuid

UPDATE #Parcels
SET MHRCN = mh.RCN
    ,MHRCNLD = mh.RCNLD
FROM #GetMHCost mh INNER JOIN #Parcels rbd ON mh.ParcelID = rbd.ParcelID
                   INNER JOIN #ShowMobiles sm ON mh.MhGuid = sm.MhGuid
WHERE mh.MhTypeItemId = 4

UPDATE #Parcels
SET AllMHRCN = ISNULL((SELECT SUM(MHRCN) FROM #AllMobiles am WHERE am.ParcelID = rbd.ParcelID AND am.MHType = 4),0)
   ,AllMHRCNLD = ISNULL((SELECT SUM(MHRCNLD) FROM #AllMobiles am WHERE am.ParcelID = rbd.ParcelID AND am.MHType = 4),0)
FROM #Parcels rbd


/***************END OF MARSHALL & SWIFT COST SECTION*******************************************/  




UPDATE #Parcels
SET YrBltRen = YrBuilt
WHERE YrRenovated <= YrBuilt  --changed from YrRenovated = 0 to cover cases where YrRenovated <= YrBuilt 5/15/17 Don G

UPDATE #Parcels
SET YrBltRen = YrRenovated
WHERE YrRenovated > YrBuilt

UPDATE #Parcels
SET TotalRCN = BldgRCN + AccyRCN + MHRCN
    ,TotalRCNLD = BldgRCNLD + AccyRCNLD + MHRCNLD

	

UPDATE #Parcels
SET --EMV = 0
    BldgRCN = 0
    ,BldgRCNLD = 0
    ,AccyRCN = 0
    ,AccyRCNLD = 0
    ,MHRCN = 0
    ,MHRCNLD = 0
    ,TotalRCN = 0
    ,TotalRCNLD = 0
    --,EMVExceptionMsg = 'EMV = 0 when BldgGrade = 20'
WHERE BldgGrade = 20



/*******Return final result**********/                                                                  

--select
-- Major,Minor,EMV,BldgRCN,BldgRCNLD,AccyRCN,AccyRCNLD,MHRCN,MHRCNLD,TotalRCN,TotalRCNLD
--From #Parcels where ResArea = @ResArea 
                    
/*
SELECT
--  @BldgCondWarning = BldgCondWarning
-- ,@EMVExceptionMsg = EMVExceptionMsg
-- ,@EMV		= EMV
 ,@BldgRCN		= BldgRCN
 ,@BldgRCNLD	= BldgRCNLD
 ,@AccyRCN		= AccyRCN
 ,@AccyRCNLD	= AccyRCNLD
 ,@MHRCN		= MHRCN
 ,@MHRCNLD		= MHRCNLD
 ,@TotalRCN		= TotalRCN
 ,@TotalRCNLD	= TotalRCNLD
 --,
 --,@ValueSelectWarning = ValueSelectWarning
From #Parcels where ResArea = @ResArea AND RealPropId = @RealPropId
*/

--SELECT * FROM #ResBldg
SELECT 
	    PIN	
	   ,MapPin	
	   ,ParcelId	
	   --,LandGuid	
	   ,PropType	
	   ,ApplGroup	
	   ,ResArea	
	   ,ResSubArea	
	   ,Major	
	   ,Minor	
	   ,BldgGuid
	   ,MhGuid
	   --,EMV
	   ,BldgRCN
	   ,BldgRCNLD
	   ,AccyRCN
	   ,AccyRCNLD
	   ,MHRCN
	   ,MHRCNLD
	   ,TotalRCN
	   ,TotalRCNLD
 FROM #Parcels

 SELECT 
	    p.PIN	
	   ,p.MapPin	
	   ,p.ParcelId	
	   --,p.LandGuid	
	   ,p.PropType	
	   ,p.ApplGroup	
	   ,p.ResArea	
	   ,p.ResSubArea	
	   ,p.Major	
	   ,p.Minor	
	   ,p.BldgGuid
	   ,p.MhGuid
	   ,rb.BldgNbr
	   --,p.EMV
	   ,p.BldgRCN
	   ,p.BldgRCNLD
	   ,p.AccyRCN
	   ,p.AccyRCNLD
	   ,p.MHRCN
	   ,p.MHRCNLD
	   ,p.TotalRCN
	   ,p.TotalRCNLD
 FROM #Parcels p
 INNER JOIN #ResBldg rb ON p.ParcelId = rb.ParcelId --AND p.BldgGuid = rb.BldgGuid

 SELECT * FROM #ResBldg
/*
RETURN(0)

ErrorHandler:
RETURN(1000)

END


*/