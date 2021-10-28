
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_GetAreaDataFiltered')
	DROP PROCEDURE [cus].[SRCH_GetAreaDataFiltered]  
GO

CREATE PROCEDURE [cus].[SRCH_GetAreaDataFiltered]
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


DECLARE @MinLandValPop AS INT = 10000;
DECLARE @MinImpsValPop As INT = 10000;
DECLARE @MinLandValSale As INT = 25000;
DECLARE @MinImpsValSale As INT = 25000;
DECLARE @MinPermitVal AS INT = 25000;
DECLARE @FactorSalesAdj As FLOAT = 1; 
DECLARE @FactorFormulaAdj As FLOAT = 0.1;
DECLARE @SaleStartDate As DATE = '20170101'; --will have to reset this each year

UPDATE #SalesPopFinal
	SET Warnings = REPLACE(Warnings, '; SALE PRICE UPDATED BY SALES ID GROUP', '')
	WHERE Warnings LIKE '%; SALE PRICE UPDATED BY SALES ID GROUP%';

CREATE TABLE #AreaData (
	ID INT PRIMARY KEY,
	PIN           char(10),
	AVRatio NVARCHAR(4000),
	LandCat NVARCHAR(4000),
	ImpCat NVARCHAR(4000),
	TotCat NVARCHAR(4000),
	[Remove?] NVARCHAR(4000),
	ParcelId uniqueidentifier,
	ResArea NVARCHAR(4000),
	ResSubArea NVARCHAR(4000),
	QSTR NVARCHAR(4000),
	Neighborhood INT,
	GisSurfaceValue NVARCHAR(4000),
	Folio NVARCHAR(4000),
	Major NVARCHAR(100),
	Minor NVARCHAR(100),
	LocationAddr NVARCHAR(4000), 
	SellerName NVARCHAR(4000),
	BuyerName  NVARCHAR(4000),
	Enum INT,
	Syr INT,
	Smo INT,
	SaleQtr INT,
	SaleDate DATE,
	Sprice FLOAT,
	TrendFactor NVARCHAR(4000),
	TrendedPrice FLOAT,
	Verif NVARCHAR(4000),
	VerBy NVARCHAR(4000),
	PropClass INT, 
	MyClass INT,
	Warnings NVARCHAR(4000),
	PossibleLenderSale NVARCHAR(4000),
	GovtOwned NVARCHAR(4000),
	HoldoutReason NVARCHAR(4000),
	SelDate DATE,
	SelApr NVARCHAR(4000), 
	SelMeth NVARCHAR(4000),
	SelRea NVARCHAR(4000),
	SelLnd FLOAT,
	SelImp FLOAT,
	SelTot FLOAT,
	RollLand FLOAT,
	RollImp FLOAT,
	RollTot FLOAT,
	BaseLand FLOAT,
	BaseYr INT,
	BaseDate DATE,
	BldgRCN INT, 
	BldgRCNLD INT,
	AccyRCN INT,
	AccyRCNLD INT,
	MHRCN INT,
	MHRCNLD INT,
	TotalRCN INT,
	TotalRCNLD INT,
	ImpCnt INT,
	BldgNbr INT, 
	NbrLivingUnits FLOAT,
	SqFtLot FLOAT,
	SqFtLotDry FLOAT,
	SqFtLotSubmerged FLOAT,
	LotGroup NVARCHAR(4000), 
	Age INT,
	YrBuilt INT,
	YrBltGroup INT,
	YrRen INT,
	YrBltRen INT,
	Grade INT,
	GRADEQC FLOAT,
	GRADEQCDIFF FLOAT,
	PlusGrade INT,
	Cond INT,
	TotLiv INT,
	AGLAGroup NVARCHAR(1000),
	AGLA INT,
	Baths FLOAT,
	FPTot INT,
	CovPkg INT,
	Stry FLOAT,
	FstFlr FLOAT,
	HlfFlr FLOAT,
	SndFlr FLOAT,
	UprFlr FLOAT,
	TotBsmt FLOAT,
	FinBsmt FLOAT,
	FinBGrade FLOAT,
	BsmtGar FLOAT,
	AttGar INT,
	DayBsmt NVARCHAR(100),
	BrickStone INT,
	AddCst INT,
	DetGar INT,
	DetGGrade INT,
	DetGEffYr INT,
	DetGarNet INT,
	Carport INT,
	CarportYr INT,
	CarportNet INT,
	PoolArea INT, 
	Paving INT,
	MiscAccyCost INT,
	FlatValue INT,
	MHCount INT,
	MHBldgNbr INT,
	MHType NVARCHAR(4000),
	MHClass INT,
	MHLength INT,
	MHWidth INT,
	MHLivingArea INT,
	MHTipOutArea INT,
	MHRoomAddSqft FLOAT,
	MHSize INT,
	MHYrBuilt INT,
	MHCondition INT,
	MHPcntNetCondition INT,
	CurrentZoning Nvarchar(50),
	ZoneDesignation NVARCHAR(4000),
	WtrSys INT,
	SewSys INT,
	Access INT,
	Topo INT,
	StrtSurf INT,
	PcntBaseLandValImpacted FLOAT,
	Unbuildable INT,
	SizeShp FLOAT,
	MtR INT,
	Oly INT,
	Cascades INT,
	Terr INT,
	SeaSky INT,
	PugSnd INT,
	LkWa INT, 
	LkSam INT,
	SmLkRvCr INT,
	OthView INT,
	TotView INT,
	ViewUtil NVARCHAR(100),
	WftLoc INT,
	WftFoot INT,
	WftPoorQual INT,
	WftBank INT,
	TideShore INT,
	WftRestAccess INT,
	HvyTraf INT,
	AirPort INT,
	Contamination INT,
	WftAccRght INT,
	WftProxInflu INT,
	OthNuis INT,
	AdjGolf INT,
	AdjGreen INT,
	LandSlide INT,
	Slope INT,
	Stream INT,
	Wetland INT,
	WaterProb INT,
	OthProb INT,
	SrCitFlag NVARCHAR(100),
	TaxStatus NVARCHAR(100),
	PresentUse INT,
	SegMergeDate DATE,
	PermitNbr NVARCHAR(255),
	PermitDate DATE,
	PermitValue FLOAT,
	PermitType FLOAT,
	PermitDescr NVARCHAR(255),
	NotASale NVARCHAR(255),
	NoYB NVARCHAR(255),
	ImpCount NVARCHAR(255),
	Mobile NVARCHAR(255), 
	[%Complete] NVARCHAR(255),
	Obsolescence NVARCHAR(255),
	UnfinFloor NVARCHAR(255),
	[%NetCond] NVARCHAR(255),
	[PrevLand<=10000] NVARCHAR(255),
	[PrevImp<=10000] NVARCHAR(255),
	BadRatio NVARCHAR(255),
	Permits NVARCHAR(255),
	NoPercNoWater NVARCHAR(255),
	AcctNo FLOAT,
	TrendedAVRatio NVARCHAR(255),
	AssignedBoth NVARCHAR(255),
	NewConstrVal FLOAT,
	NbrBldgSites INT,
	PowerLines INT,
	DistrictName NVARCHAR(255),  
	--**** NEW FIELDS ADDED IN THE ANOTHER TABLES (IMPSALES, VAC, ACC, ETC)*****
	RemBefore NVARCHAR(1000),
	RemDuring NVARCHAR(1000),
	Notes1 NVARCHAR(1000),
	Notes2 NVARCHAR(1000),
	OutlierType NVARCHAR(1000),
	[DoubleSale?] NVARCHAR(1000),
	YrBltRenGroup NVARCHAR(1000),
	ChangeInSelectVal FLOAT,
	--**** NEW FIELDS ADDED IN THE ANOTHER TABLES *****
	NOTES NVARCHAR(MAX),
	BATH3QTRCOUNT FLOAT,
	BATHFULLCOUNT FLOAT,
	BATHHALFCOUNT FLOAT,
	BEDROOMS FLOAT,
	COALMINEHAZARD FLOAT,
	COMMONPROPERTY FLOAT,
	CRITICALDRAINAGE FLOAT,
	CURRENTUSEDESIGNATION FLOAT,
	DEEDRESTRICTIONS FLOAT,
	DEVCOST FLOAT,
	DEVELOPMENTRIGHTSPURCH FLOAT,
	DIRPREFIX NVARCHAR(255),
	DIRSUFFIX NVARCHAR(255),
	EASEMENTS FLOAT,
	EROSIONHAZARD FLOAT,
	FPADDITIONAL FLOAT,
	FPFREESTANDING FLOAT,
	FPMULTISTORY FLOAT,
	FPSINGLESTORY FLOAT,
	HBUASIFVACANT FLOAT,
	HBUASIMPROVED FLOAT,
	HEATSOURCE FLOAT,
	HEATSYSTEM FLOAT,
	HILASTYR FLOAT,
	HISTORICSITE FLOAT,
	HIVALUE FLOAT,
	HUNDREDYRFLOODPLAIN FLOAT,
	INSTR FLOAT,
	LANDFILLBUFFER FLOAT,
	LOTDEPTHFACTOR FLOAT,
	NATIVEGROWTHPROTESMT FLOAT,
	NBRFRACTION NVARCHAR(255),
	orOBSOLESCENCE FLOAT,
	OTHERDESIGNATION FLOAT,
	PCNTCOMPLETE FLOAT,
	PCNTNETCONDITION FLOAT,
	PLATBLOCK NVARCHAR(255),
	PLATLOT NVARCHAR(255),
	POOLEFFYR FLOAT,
	POOLNETCOND FLOAT,
	PRINUSE FLOAT,
	PROPTYPE NVARCHAR(255),
	QSEC NVARCHAR(255),
	REASON FLOAT,
	RNG FLOAT,
	SALEID FLOAT,
	SEC FLOAT,
	SEISMICHAZARD FLOAT,
	SENSITIVEAREATRACT FLOAT,
	SPECIALASSESSMENTS FLOAT,
	SPECIESOFCONCERN FLOAT,
	SQFTDECK FLOAT,
	SQFTENCLOSEDPORCH FLOAT,
	SQFTOPENPORCH FLOAT,
	SQFTUNFINFULL FLOAT,
	SQFTUNFINHALF FLOAT,
	STREETNAME NVARCHAR(255),
	STREETNBR NVARCHAR(255),
	STREETTYPE NVARCHAR(255),
	TAXPAYERNAME NVARCHAR(255),
	TWN FLOAT,
	ZONECLASS NVARCHAR(255),
	ZONINGCHGDATE DATE
	);
		
--************ AVRatio, LandCat, ImpCat *********************
INSERT INTO #AreaData (ID,PIN, AVRatio, LandCat, ImpCat, BATH3QTRCOUNT, BATHFULLCOUNT, BATHHALFCOUNT, BEDROOMS, COALMINEHAZARD, COMMONPROPERTY, 
		CRITICALDRAINAGE, CURRENTUSEDESIGNATION, DEEDRESTRICTIONS, DEVCOST, DEVELOPMENTRIGHTSPURCH, DIRPREFIX, DIRSUFFIX, EASEMENTS,
		EROSIONHAZARD, FPADDITIONAL, FPFREESTANDING, FPMULTISTORY, FPSINGLESTORY, HBUASIFVACANT, HBUASIMPROVED, HEATSOURCE, HEATSYSTEM, 
		HILASTYR, HISTORICSITE, HIVALUE, HUNDREDYRFLOODPLAIN, INSTR, LANDFILLBUFFER, LOTDEPTHFACTOR, NATIVEGROWTHPROTESMT, NBRFRACTION, 
		orOBSOLESCENCE, OTHERDESIGNATION, PCNTCOMPLETE, PCNTNETCONDITION, PLATBLOCK, PLATLOT, POOLEFFYR, POOLNETCOND, PRINUSE, PROPTYPE, 
		QSEC, REASON, RNG, SALEID, SEC, SEISMICHAZARD, SENSITIVEAREATRACT, SPECIALASSESSMENTS, SPECIESOFCONCERN, SQFTDECK, SQFTENCLOSEDPORCH,
		SQFTOPENPORCH, SQFTUNFINFULL, SQFTUNFINHALF, STREETNAME, STREETNBR, STREETTYPE,TAXPAYERNAME, TWN, ZONECLASS, ZONINGCHGDATE, NOTES) 
		SELECT ID,PIN,
		CASE WHEN (SalePrice > 0) THEN CAST(RollValTotal / SalePrice AS NVARCHAR) ELSE 'NotASale' END AS AVRatio,
		CASE WHEN (SewerSystem < 3 AND WaterSystem < 3 AND RollLandVal > @MinLandValPop AND Unbuildable = 0) THEN 'NewLand' ELSE 'PrevLand' END AS LandCat,
		CASE WHEN (RollImpsVal <= @MinImpsValPop OR Condition = 1 OR PcntNetCondition > 0 OR SewerSystem > 2) THEN 'PrevImp' ELSE 'NewImp' END AS ImpCat,
		BATH3QTRCOUNT, BATHFULLCOUNT, BATHHALFCOUNT, BEDROOMS, COALMINEHAZARD, COMMONPROPERTY, 
		CRITICALDRAINAGE, CURRENTUSEDESIGNATION, DEEDRESTRICTIONS, DEVCOST, DEVELOPMENTRIGHTSPURCH, DIRPREFIX, DIRSUFFIX, EASEMENTS,
		EROSIONHAZARD, FPADDITIONAL, FPFREESTANDING, FPMULTISTORY, FPSINGLESTORY, HBUASIFVACANT, HBUASIMPROVED, HEATSOURCE, HEATSYSTEM, 
		HILASTYR, HISTORICSITE, HIVALUE, HUNDREDYRFLOODPLAIN, INSTR, LANDFILLBUFFER, LOTDEPTHFACTOR, NATIVEGROWTHPROTESMT, NBRFRACTION, 
		OBSOLESCENCE AS orOBSOLESCENCE, OTHERDESIGNATION, PCNTCOMPLETE, PCNTNETCONDITION, PLATBLOCK, PLATLOT, POOLEFFYR, POOLNETCOND, PRINUSE, PROPTYPE, 
		QSEC, REASON, RNG, SALEID, SEC, SEISMICHAZARD, SENSITIVEAREATRACT, SPECIALASSESSMENTS, SPECIESOFCONCERN, SQFTDECK, SQFTENCLOSEDPORCH,
		SQFTOPENPORCH, SQFTUNFINFULL, SQFTUNFINHALF, STREETNAME, STREETNBR, STREETTYPE,TAXPAYERNAME, TWN, ZONECLASS, ZONINGCHGDATE, Notes
  FROM #SalesPopFinal;

--************ TOTCAT *********************

UPDATE #AreaData
   SET TotCat = 'Holdout'
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.HoldoutReason IS NOT NULL AND e.HoldoutReason <> '')

UPDATE #AreaData
   SET TotCat = 'EMV'
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND LandCat = 'NewLand' 
		 AND ImpCat = 'NewImp' 
		 AND e.ImpCnt = 1 
		 AND BldgGrade > 0 
		 AND RollImpsVal > @MinImpsValPop

UPDATE #AreaData
   SET TotCat = 'MultiImp'
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND LandCat = 'NewLand' 
		 AND ImpCat = 'NewImp' 
		 AND e.ImpCnt > 1 
		 AND BldgGrade > 0

UPDATE #AreaData
   SET TotCat = 'AccyOnly'
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID 
		  AND e.ImpCnt = 0 
		  AND RollImpsVal > 0
          AND BldgGrade = 0 
		  AND e.MHType <> 'REAL PROP' 
		  AND e.MHType <> 'PERS PROP'

UPDATE #AreaData
   SET TotCat = 'MobH'
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID 
		  AND e.ImpCnt = 0 
		  AND BldgGrade = 0
          AND e.MHType = 'REAL PROP'

UPDATE #AreaData
   SET TotCat = 'Vacant'
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID 
		  AND RollImpsVal = 0

UPDATE #AreaData
   SET TotCat = 'NewL+PrevI'
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID 
		  AND LandCat = 'NewLand' 
		  AND RollImpsVal > 0 
		  AND RollImpsVal <= @MinImpsValPop

UPDATE #AreaData
   SET TotCat = 'NewL+PrevI'
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID 
		  AND LandCat = 'NewLand' 
		  AND (Condition = 1 OR e.PcntNetCondition > 0)

UPDATE #AreaData
   SET TotCat = 'Exception'
   WHERE (TotCat IS NULL OR TotCat = '');

-- ************ [Remove?] ****************
UPDATE #AreaData
   SET [Remove?] = AVRatio
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND AVRatio = 'NotASale'

UPDATE #AreaData
   SET [Remove?] = 'NEED TO ADD A SALES WARNING!!!'
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
	     AND (e.Warnings = '0' OR (e.Warnings IS NULL OR e.Warnings = ''))
		 AND VerifiedAtMkt = 'N'

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + '; ' + 'PERCENT COMPLETE' + '; ' + e.Warnings
							       ELSE
								        [Remove?] + '; ' + 'PERCENT COMPLETE'
								   END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'PERCENT COMPLETE' + '; ' + e.Warnings
							       ELSE
								        'PERCENT COMPLETE'
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND CONVERT(INT, e.PcntComplete) > 0

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + '; ' + 'PERCENT NET CONDITION'
							       ELSE
								        [Remove?] + '; ' + 'PERCENT NET CONDITION'
								   END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'PERCENT NET CONDITION' + '; ' + e.Warnings
							       ELSE
								        'PERCENT NET CONDITION'
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND e.PcntNetCondition > 0 

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + '; ' + 'ACTIVE PERMIT BEFORE SALE >' + LEFT(@MinPermitVal, 2) + 'K'
							       ELSE
								        [Remove?] + '; ' + 'ACTIVE PERMIT BEFORE SALE >' + LEFT(@MinPermitVal, 2) + 'K'
								   END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'ACTIVE PERMIT BEFORE SALE >' + LEFT(@MinPermitVal, 2) + 'K' + '; ' + e.Warnings
							       ELSE
								        'ACTIVE PERMIT BEFORE SALE >' + LEFT(@MinPermitVal, 2) + 'K' 
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		AND (CONVERT(INT, e.PermitValue) > @MinPermitVal AND e.PermitDate < e.SaleDate)

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
							  [Remove?] + '; ' + 'SALES DATA DOES NOT MATCH ASSESSOR DATA'
							  ELSE
							  'SALES DATA DOES NOT MATCH ASSESSOR DATA'
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.PermitNbr LIKE '%DNQ%' OR e.PermitNbr LIKE '%MDF%')
            
UPDATE #AreaData
   SET [Remove?] = [Remove?] + '; ' + e.HoldoutReason
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND e.HoldoutReason = 'AFFORDABLE HOUSING' 
		 AND ([Remove?] IS NOT NULL AND [Remove?] <> '')
		 AND (e.Warnings IS NOT NULL AND e.Warnings <> '')

UPDATE #AreaData
   SET [Remove?] = CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							  e.HoldoutReason + '; ' + e.Warnings
							  ELSE
							  e.HoldoutReason
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.HoldoutReason = 'AFFORDABLE HOUSING' AND ([Remove?] IS NULL OR [Remove?] = ''))

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + '; DOR RATIO'
							       ELSE
								        [Remove?] + '; DOR RATIO'
								   END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'DOR RATIO' + '; ' + e.Warnings
							       ELSE
								        'DOR RATIO'
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (AVRatio <> 'NotASale' AND (CAST(AVRatio AS FLOAT) < 0.25 OR CAST(AVRatio AS FLOAT) > 1.75)) 

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + '; IMP. COUNT > 1'
								   ELSE 
										[Remove?]
							 END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'IMP. COUNT >1' + '; ' + e.Warnings
							       ELSE
								        'IMP. COUNT >1'
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND e.ImpCnt > 1 

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + '; PERSONAL PROPERTY MH'
							       ELSE
								        [Remove?] + '; PERSONAL PROPERTY MH'
								   END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'PERSONAL PROPERTY MH' + '; ' + e.Warnings
							       ELSE
								        'PERSONAL PROPERTY MH'
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND e.MHType = 'PERS PROP' 
						
UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + '; REAL PROPERTY MH'
							       ELSE
								        [Remove?] + '; REAL PROPERTY MH'
								   END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'REAL PROPERTY MH' + '; ' + e.Warnings
							       ELSE
								        'REAL PROPERTY MH'
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND e.MHType = 'REAL PROP'

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + '; OBSOLESCENCE'
							       ELSE
								        [Remove?] + '; OBSOLESCENCE'
								   END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'OBSOLESCENCE' + '; ' + e.Warnings
							       ELSE
								        'OBSOLESCENCE'
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND e.Obsolescence > 0 

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + '; OPEN SPACE'
							       ELSE
								        [Remove?] + '; OPEN SPACE'
								   END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'OPEN SPACE' + '; ' + e.Warnings
							       ELSE
								        'OPEN SPACE'
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND e.CurrentUseDesignation > 0  

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + ';  PREVOUS LAND VALUE <=' + Left(@MinLandValSale, 2) + 'K' --FIX PREVOUS TO PREVIOUS
							       ELSE
								        [Remove?] + '; PREVIOUS LAND VALUE <=' + Left(@MinLandValSale, 2) + 'K'
								   END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'PREVIOUS LAND VALUE <=' + Left(@MinLandValSale, 2) + 'K' + '; ' + e.Warnings
							       ELSE
								        'PREVIOUS LAND VALUE <=' + Left(@MinLandValSale, 2) + 'K'
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND RollLandVal <= @MinLandValSale 

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + '; PREVIOUS IMP. VALUE <= ' + LEFT(@MinImpsValSale, 2) + 'K'
							       ELSE
								        [Remove?] + '; PREVIOUS IMP. VALUE <= ' + LEFT(@MinImpsValSale, 2) + 'K'
								   END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'PREVIOUS IMP. VALUE <= ' + Left(@MinLandValSale, 2) + 'K' + '; ' + e.Warnings
							       ELSE
								        'PREVIOUS IMP. VALUE <= ' + Left(@MinImpsValSale, 2) + 'K'
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND RollImpsVal <= @MinImpsValSale

UPDATE #AreaData
   SET [Remove?] = CASE WHEN ([Remove?] IS NOT NULL AND [Remove?] <> '') THEN
                             (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            [Remove?] + '; UNFINISHED AREA'
							       ELSE
								        [Remove?] + '; UNFINISHED AREA'
								   END)
						ELSE 
							 (CASE WHEN (e.Warnings IS NOT NULL AND e.Warnings <> '') THEN
							            'UNFINISHED AREA' + '; ' + e.Warnings
							       ELSE
								        'UNFINISHED AREA'
								   END)
				   END 
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.SqFtUnfinFull > 0 OR e.SqFtUnfinHalf > 0) 	
 
UPDATE #AreaData
   SET [Remove?] = e.Warnings
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%IMP%')
		 AND VerifiedAtMkt <> 'N'
		 AND ([Remove?] IS NULL OR [Remove?] = '') 
            
UPDATE #AreaData
   SET [Remove?] = [Remove?]
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%IMP%')
		 AND VerifiedAtMkt = 'N'

UPDATE #AreaData
   SET [Remove?] = e.Warnings
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%SEGREGATION%')
		 AND (VerifiedAtMkt IS NOT NULL AND VerifiedAtMkt <> '')
		 AND ([Remove?] IS NULL OR [Remove?] = '')
		 
UPDATE #AreaData
   SET [Remove?] = [Remove?]
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%SEGREGATION%')
		 AND VerifiedAtMkt = 'N'

UPDATE #AreaData
   SET [Remove?] = [Remove?]
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%FINANCIAL%')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NOT NULL AND [Remove?] <> '')
		 AND (e.Warnings IS NOT NULL AND e.Warnings <> '')

UPDATE #AreaData
   SET [Remove?] = e.Warnings
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%FINANCIAL%')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NULL OR [Remove?] = '')
		 AND (e.Warnings IS NOT NULL AND e.Warnings <> '')
		             
UPDATE #AreaData
   SET [Remove?] = [Remove?]
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%SHORT%')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NOT NULL AND [Remove?] <> '')
		                         
UPDATE #AreaData
   SET [Remove?] = e.Warnings
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%SHORT%')
		 AND VerifiedAtMkt <> ' '
		 AND ([Remove?] IS NULL OR [Remove?] = '')
		    
UPDATE #AreaData
   SET [Remove?] = [Remove?]
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%AUCTION%')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NOT NULL AND [Remove?] <> '')

UPDATE #AreaData
   SET [Remove?] = e.Warnings
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%AUCTION%')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NULL OR [Remove?] = '')
		                
UPDATE #AreaData
   SET [Remove?] = [Remove?]
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%BANKRUPTCY%')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NOT NULL AND [Remove?] <> '')
		            
UPDATE #AreaData
   SET [Remove?] = e.Warnings
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%BANKRUPTCY%')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NULL OR [Remove?] = '')
		    
UPDATE #AreaData
   SET [Remove?] = [Remove?]
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%SHERIFF%')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NOT NULL AND [Remove?] <> '')

UPDATE #AreaData
   SET [Remove?] = e.Warnings
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%SHERIFF%')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NULL OR [Remove?] = '')
		    		                
UPDATE #AreaData
   SET [Remove?] = [Remove?]
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%EXEMPT%')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NOT NULL AND [Remove?] <> '')

UPDATE #AreaData
   SET [Remove?] = e.Warnings
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings LIKE '%EXEMPT%')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NULL OR [Remove?] = '')

UPDATE #AreaData
   SET [Remove?] = [Remove?]
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings IS NOT NULL AND e.Warnings <> '')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NOT NULL AND [Remove?] <> '')

UPDATE #AreaData
   SET [Remove?] = e.Warnings
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND (e.Warnings IS NOT NULL AND e.Warnings <> '')
		 AND VerifiedAtMkt <> 'Y'
		 AND ([Remove?] IS NULL OR [Remove?] = '')
		             
UPDATE #AreaData
   SET [Remove?] = ''
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND ([Remove?] = 'STATEMENT TO DOR;' OR [Remove?] = 'STATEMENT TO DOR')
		 AND LEN([Remove?]) <= 18
			
UPDATE #AreaData
   SET [Remove?] = ''
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND ([Remove?] = 'ESTATE ADMINISTRATOR, GUARDIAN, OR EXECUTOR' AND LEN([Remove?]) <= 44)
		 AND VerifiedAtMkt <> 'N'
			
UPDATE #AreaData
   SET [Remove?] = ''
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND ([Remove?] = 'RELATED PARTY, FRIEND, OR NEIGHBOR' AND LEN([Remove?]) <= 34)
		 AND VerifiedAtMkt <> 'N'
			
UPDATE #AreaData
   SET [Remove?] = ''
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND ([Remove?] = 'RELOCATION - SALE BY SERVICE;' OR [Remove?] = 'RELOCATION - SALE BY SERVICE')
		 AND LEN([Remove?]) <= 29
		 AND VerifiedAtMkt <> 'N'
			
UPDATE #AreaData
   SET [Remove?] = Left([Remove?], Len([Remove?]) - 1)
  FROM #SalesPopFinal e
       INNER JOIN #AreaData a
          ON e.ID = a.ID
		 AND Right([Remove?], 1) = ';'

UPDATE #AreaData
   SET [Remove?] = '' 
 WHERE [Remove?] IS NULL;

--************ RealProp *********************
UPDATE #AreaData 
	SET ParcelId = e.ParcelId
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ RAREA *********************
UPDATE #AreaData 
	SET ResArea = e.ResArea
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ RESUBAREA *********************
UPDATE #AreaData 
	SET ResSubArea = e.ResSubarea
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ QSTR *********************
UPDATE #AreaData 
	SET QSTR = e.QSec + '-' + CONVERT(nvarchar, e.Sec) + '-' + CONVERT(nvarchar, e.Twn) + '-' + CONVERT(nvarchar, e.Rng)
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ NEIGHBORHOOD *********************
UPDATE #AreaData 
	SET Neighborhood = e.Neighborhood
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
	
--************ GisSurfaceValue *********************
UPDATE #AreaData 
	SET GisSurfaceValue = e.GisSurfaceValue
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID


--************ Folio *********************
UPDATE #AreaData 
	SET Folio = e.Folio
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Major *********************
UPDATE #AreaData 
	SET Major = e.Major
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Minor *********************
UPDATE #AreaData 
	SET Minor = e.Minor
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ LocationAddr *********************
UPDATE #AreaData 
	SET LocationAddr = e.LocationAddr
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ SellerName *********************
UPDATE #AreaData 
	SET SellerName = e.SellerName
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ BuyerName *********************
UPDATE #AreaData 
	SET BuyerName = e.BuyerName
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Enum *********************
UPDATE #AreaData 
	SET Enum = e.ExciseTaxNbr
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Syr *********************
UPDATE #AreaData 
	SET Syr = e.SaleYear
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Smo *********************
UPDATE #AreaData 
	SET Smo = e.SaleMonth
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

UPDATE #AreaData 
	SET Smo = Month(e.SaleDate) + (Year(e.SaleDate) - Year(@SaleStartDate)) * 12
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
	AND e.SaleDate >= @SaleStartDate

UPDATE #AreaData 
	SET Smo = 0
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
	AND e.SaleDate < @SaleStartDate

--************ SaleQtr *********************
UPDATE #AreaData 
	SET SaleQtr = CEILING((Month(e.SaleDate) - Month(@SaleStartDate) + 1) / 3.0) + (Year(e.SaleDate) - Year(@SaleStartDate)) * 4
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
	AND e.SaleDate >= @SaleStartDate

UPDATE #AreaData 
	SET SaleQtr = 0
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
	AND e.SaleDate  < @SaleStartDate

--************ SaleDate *********************
UPDATE #AreaData 
	SET SaleDate = e.SaleDate
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Sprice *********************
UPDATE #AreaData 
	SET Sprice = SalePrice
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ TrendFactor *********************
UPDATE #AreaData 
	SET TrendFactor = e.TrendFactor
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ TrendedPrice *********************
UPDATE #AreaData 
	SET TrendedPrice = e.TrendedPrice
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Verif *********************
UPDATE #AreaData 
	SET Verif = e.VerifiedAtMkt
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ VerBy *********************
UPDATE #AreaData 
	SET VerBy = e.VerifiedBy
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Warnings *********************
UPDATE #AreaData 
	SET Warnings = e.Warnings
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ PropClass *********************
UPDATE #AreaData 
	SET PropClass = e.PropClass
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ MyClass ********************* 
UPDATE #AreaData 
	SET MyClass = 0

UPDATE #AreaData 
	SET MyClass = CASE WHEN (e.VerifiedAtMkt = 'Y' OR e.VerifiedAtMkt = 'N') THEN e.PropClass  ELSE 
					 CASE WHEN (e.YrBuilt >= e.SaleYear AND e.SalePrice < 2 * e.RollLandVal AND (e.MHType <> 'REAL PROP' OR e.MHType IS NULL OR e.MHType = ''))  THEN 7  ELSE 
						 CASE WHEN (e.YrBuilt = 0 AND RollImpsVal = 0 AND (e.MHType <> 'REAL PROP' OR e.MHType IS NULL OR e.MHType = ''))  THEN 7  ELSE 
						 	 CASE WHEN (e.YrBuilt = e.SaleYear AND e.SalePrice >= 2 * e.RollLandVal AND (e.MHType <> 'REAL PROP' OR e.MHType IS NULL OR e.MHType = '') AND e.PropClass <> 7)  THEN 8  ELSE 
								 CASE WHEN (e.YrBuilt > 0 AND e.YrBuilt < e.SaleYear AND (e.MHType <> 'REAL PROP' OR e.MHType IS NULL OR e.MHType = '') AND e.PropClass <> 7)  THEN 8  ELSE 
									 CASE WHEN (e.YrBuilt > e.SaleYear AND (e.MHType <> 'REAL PROP' OR e.MHType IS NULL OR e.MHType = '') AND e.PropClass <> 7)  THEN 8  ELSE 
										CASE WHEN (e.RollImpsVal > 0 AND e.MHType = 'REAL PROP')  THEN 9  ELSE 
											CASE WHEN (e.RollImpsVal > 0 AND e.YrBuilt = 0 AND (e.MHType <> 'REAL PROP' OR e.MHType IS NULL OR e.MHType = ''))  THEN 11 ELSE 12				
		END END END END END END END END 
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
	AND e.ExciseTaxNbr > 0
               
--************ PossibleLenderSale *********************

UPDATE #AreaData 
	SET PossibleLenderSale = e.PossibleLenderSale
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

/*
Hairo comment: this needs to be calculated in [cus].[SRCH_GetAreaData01] to be used here	
--************ GovtOwned *********************

UPDATE #AreaData 
	SET GovtOwned = e.GovtOwned
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
*/
--************ HoldoutReason *********************

UPDATE #AreaData 
	SET HoldoutReason = e.HoldoutReason
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID


--************ SelDate *********************

UPDATE #AreaData 
	SET SelDate = e.SelectDate
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID


--************ SelApr *********************

UPDATE #AreaData 
	SET SelApr = e.SelectAppr
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID


--************ SelMeth *********************

UPDATE #AreaData 
	SET SelMeth = e.SelectMethod
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID


--************ SelRea *********************

UPDATE #AreaData 
	SET SelRea = e.SelectReason
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID


--************ SelLnd *********************

UPDATE #AreaData 
	SET SelLnd = e.SelectLandVal
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
	

--************ SelImp *********************

UPDATE #AreaData 
	SET SelImp = e.SelectImpsVal
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
	

--************ SelTot *********************

UPDATE #AreaData 
	SET SelTot = e.SelectValTotal
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
	         

--************ RollLand *********************

UPDATE #AreaData 
	SET RollLand = e.RollLandVal
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID     
        
--************ RollImp *********************

UPDATE #AreaData 
	SET RollImp = e.RollImpsVal
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
	            
--************ RollTot *********************

UPDATE #AreaData 
	SET RollTot = e.RollValTotal
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID        

--************ BaseLand *********************

UPDATE #AreaData 
	SET BaseLand = e.BaseLandVal
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID        


--************ BaseYr *********************

UPDATE #AreaData 
	SET BaseYr = e.BaseLandValTaxYr
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID        

--************ BaseDate *********************

UPDATE #AreaData 
	SET BaseDate = e.BaseLandValDate
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID        

--************ BldgRCN *********************

UPDATE #AreaData 
	SET BldgRCN = e.BldgRCN
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID        

--************ BldgRCNLD *********************

UPDATE #AreaData 
	SET BldgRCNLD = e.BldgRCNLD
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID        

--************ AccyRCN *********************

UPDATE #AreaData 
	SET AccyRCN = e.AccyRCN
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ AccyRCNLD *********************

UPDATE #AreaData 
	SET AccyRCNLD = e.AccyRCNLD
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ MHRCN *********************

UPDATE #AreaData 
	SET MHRCN = e.MHRCN
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ MHRCNLD *********************

UPDATE #AreaData 
	SET MHRCNLD = e.MHRCNLD
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ TotalRCN *********************

UPDATE #AreaData 
	SET TotalRCN = e.TotalRCN
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ TotalRCNLD *********************

UPDATE #AreaData 
	SET TotalRCNLD = e.TotalRCNLD
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ ImpCnt *********************

UPDATE #AreaData 
	SET ImpCnt = e.ImpCnt
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ BldgNbr *********************

UPDATE #AreaData 
	SET BldgNbr = e.BldgNbr
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ NbrLivingUnits *********************

UPDATE #AreaData 
	SET NbrLivingUnits = e.NbrLivingUnits
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ SqFtLot *********************

UPDATE #AreaData 
	SET SqFtLot = e.SqFtLot
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ SqFtLotDry *********************

UPDATE #AreaData 
	SET SqFtLotDry = e.SqFtLotDry
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ SqFtLotSubmerged *********************

UPDATE #AreaData 
	SET SqFtLotSubmerged = e.SqFtLotSubmerged
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ LotGroup *********************

UPDATE #AreaData 
	SET LotGroup = CASE WHEN e.SqFtLot < 3000 THEN '<3000'
				   WHEN e.SqFtLot BETWEEN 3000 AND 5000 THEN '3000-5000'
				   WHEN(e.SqFtLot BETWEEN 5001 AND 8000) THEN '5001-8000'
				   WHEN(e.SqFtLot BETWEEN 8001 AND 12000) THEN '8001-12000'
				   WHEN(e.SqFtLot BETWEEN 12001 AND 16000) THEN '12001-16000'
				   WHEN(e.SqFtLot BETWEEN 16001 AND 20000) THEN '16001-20000'
				   WHEN(e.SqFtLot BETWEEN 20001 AND 30000) THEN '20001-30000'
				   WHEN(e.SqFtLot BETWEEN 30001 AND 43559) THEN '30001-43559'
				   WHEN(e.SqFtLot BETWEEN 43560 AND 130680) THEN '1AC-3AC'
				   WHEN(e.SqFtLot BETWEEN 130681 AND 217800) THEN '3.01AC-5AC'
				   WHEN(e.SqFtLot BETWEEN 217801 AND 435600) THEN '5.1AC-10AC'
				   WHEN(e.SqFtLot > 435600) THEN '>10AC'
				   ELSE
				   '0'																			
				   END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID


--************ Age *********************

UPDATE #AreaData 
	SET Age = e.Age
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ YrBuilt *********************

UPDATE #AreaData 
	SET YrBuilt = e.YrBuilt
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ YrBltGroup *********************

UPDATE #AreaData 
	SET YrBltGroup = 0
	FROM #SalesPopFinal e
       
--************ YrRen *********************

UPDATE #AreaData 
	SET YrRen = e.YrRenovated
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ YrBltRen *********************

UPDATE #AreaData 
	SET YrBltRen = CASE WHEN (YrRenovated > 0) THEN 
						YrRenovated
						ELSE
						e.YrBuilt
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ Grade *********************

UPDATE #AreaData 
	SET Grade = e.BldgGrade
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ PlusGrade *********************

UPDATE #AreaData 
	SET PlusGrade = e.BldgGradeVar
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ Cond *********************

UPDATE #AreaData 
	SET Cond = e.Condition
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ TotLiv *********************

UPDATE #AreaData 
	SET TotLiv = e.SqFtTotLiving
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ AGLAGroup *********************

UPDATE #AreaData 
	SET AGLAGroup = 0
	FROM #SalesPopFinal e

--************ AGLA *********************

UPDATE #AreaData 
	SET AGLA = e.SqFtAboveGrLiving
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ Baths *********************

UPDATE #AreaData 
	SET Baths = e.Bathtotal
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ FPTot *********************

UPDATE #AreaData 
	SET FPTot = e.FpTotal
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ CovPkg *********************

UPDATE #AreaData 
	SET CovPkg = e.SqFtCoveredParking
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ Stry *********************

UPDATE #AreaData 
	SET Stry = e.Stories
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ FstFlr *********************

UPDATE #AreaData 
	SET FstFlr = e.SqFt1stFloor
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ HlfFlr *********************

UPDATE #AreaData 
	SET HlfFlr = e.SqFtHalfFloor
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ HlfFlr *********************

UPDATE #AreaData 
	SET SndFlr = e.SqFt2ndFloor
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ UprFlr *********************

UPDATE #AreaData 
	SET UprFlr = e.SqFtUpperFloor
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ TotBsmt *********************

UPDATE #AreaData 
	SET TotBsmt = e.SqFtTotBasement
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ FinBsmt *********************

UPDATE #AreaData 
	SET FinBsmt = e.SqFtFinBasement
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ FinBGrade *********************

UPDATE #AreaData 
	SET FinBGrade = e.FinBasementGrade
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ BsmtGar *********************

UPDATE #AreaData 
	SET BsmtGar = e.SqFtGarageBasement
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ AttGar *********************

UPDATE #AreaData 
	SET AttGar = e.SqFtGarageAttached
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID  

--************ DayBsmt *********************

UPDATE #AreaData 
	SET DayBsmt = e.DaylightBasement
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ BrickStone *********************

UPDATE #AreaData 
	SET BrickStone = e.BrickStone
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ AddCst *********************

UPDATE #AreaData 
	SET AddCst = e.AddnlCost
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ DetGar *********************

UPDATE #AreaData 
	SET DetGar = e.DetGarArea
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
        
--************ DetGGrade *********************

UPDATE #AreaData 
	SET DetGGrade = e.DetGarGrade
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ DetGEffYr *********************

UPDATE #AreaData 
	SET DetGEffYr = e.DetGarEffYr
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ DetGarNet *********************

UPDATE #AreaData 
	SET DetGarNet = e.DetGarNetCond
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Carport *********************

UPDATE #AreaData 
	SET Carport = e.CarportArea
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ CarportYr *********************

UPDATE #AreaData 
	SET CarportYr = e.CarportEffYr
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ CarportNet *********************

UPDATE #AreaData 
	SET CarportNet = e.CarportNetCond
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ PoolArea *********************

UPDATE #AreaData 
	SET PoolArea = e.PoolArea
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Paving *********************

UPDATE #AreaData 
	SET Paving = e.Paving
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ MiscAccyCost *********************

UPDATE #AreaData 
	SET MiscAccyCost = e.MiscAccyCost
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ FlatValue *********************

UPDATE #AreaData 
	SET FlatValue = e.FlatValue
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ MHCount *********************

UPDATE #AreaData 
	SET MHCount = e.MHCount
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ MHBldgNbr *********************

UPDATE #AreaData 
	SET MHBldgNbr = e.MHBldgNbr
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ MHType *********************

UPDATE #AreaData 
	SET MHType = e.MHType
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
    
--************ MHClass *********************

UPDATE #AreaData 
	SET MHClass = e.MHClass
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
  
--************ MHLength *********************

UPDATE #AreaData 
	SET MHLength = e.MHLength
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
   
--************ MHWidth *********************

UPDATE #AreaData 
	SET MHWidth = e.MHWidth
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ MHLivingArea *********************

UPDATE #AreaData 
	SET MHLivingArea = e.MHLivingArea
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
 
--************ MHTipOutArea *********************

UPDATE #AreaData 
	SET MHTipOutArea = e.MHTipOutArea
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
 
--************ MHRoomAddSqft *********************

UPDATE #AreaData 
	SET MHRoomAddSqft = e.MHRoomAddSqft
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
 
--************ MHSize *********************

UPDATE #AreaData 
	SET MHSize = e.MHSize
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ MHYrBuilt *********************

UPDATE #AreaData 
	SET MHYrBuilt = e.MHYrBuilt
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
   
--************ MHCondition *********************

UPDATE #AreaData 
	SET MHCondition = e.MHCondition
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ MHPcntNetCondition *********************

UPDATE #AreaData 
	SET MHPcntNetCondition = e.MHPcntNetCondition
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ CurrentZoning *********************

UPDATE #AreaData 
	SET CurrentZoning = e.CurrentZoning
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ ZoneDesignation *********************

UPDATE #AreaData 
	SET ZoneDesignation = e.ZoneDesignation
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ WtrSys *********************

UPDATE #AreaData 
	SET WtrSys = e.WaterSystem
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ SewSys *********************

UPDATE #AreaData 
	SET SewSys = e.SewerSystem
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Access *********************

UPDATE #AreaData 
	SET Access = e.Access
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Topo *********************

UPDATE #AreaData 
	SET Topo = e.Topography
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ StrtSurf *********************

UPDATE #AreaData 
	SET StrtSurf = e.StreetSurface
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ PcntBaseLandValImpacted *********************

UPDATE #AreaData 
	SET PcntBaseLandValImpacted = e.PcntBaseLandValImpacted
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Unbuildable *********************

UPDATE #AreaData 
	SET Unbuildable = e.Unbuildable
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ SizeShp *********************

UPDATE #AreaData 
	SET SizeShp = e.RestrictiveSzShape
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ MtR *********************

UPDATE #AreaData 
	SET MtR = e.MtRainier
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Oly *********************

UPDATE #AreaData 
	SET Oly = e.Olympics
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Cascades *********************

UPDATE #AreaData 
	SET Cascades = e.Cascades
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Terr *********************

UPDATE #AreaData 
	SET Terr = e.Territorial
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ SeaSky *********************

UPDATE #AreaData 
	SET SeaSky = e.SeattleSkyline
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ PugSnd *********************

UPDATE #AreaData 
	SET PugSnd = e.PugetSound
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ LkWa *********************

UPDATE #AreaData 
	SET LkWa = e.LakeWashington
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID


--************ LkSam *********************

UPDATE #AreaData 
	SET LkSam = e.LakeSammamish
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ SmLkRvCr *********************

UPDATE #AreaData 
	SET SmLkRvCr = e.SmallLakeRiverCreek
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ OthView *********************

UPDATE #AreaData 
	SET OthView = e.OtherView
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ TotView *********************

UPDATE #AreaData 
	SET TotView = e.TotalViews
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ ViewUtil *********************

UPDATE #AreaData 
	SET ViewUtil = e.ViewUtilization
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ WftLoc *********************

UPDATE #AreaData 
	SET WftLoc = e.WfntLocation
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ WftFoot *********************

UPDATE #AreaData 
	SET WftFoot = e.WfntFootage
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ WftPoorQual *********************

UPDATE #AreaData 
	SET WftPoorQual = e.WfntPoorQuality
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ WftBank *********************

UPDATE #AreaData 
	SET WftBank = e.WfntBank
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ TideShore *********************

UPDATE #AreaData 
	SET TideShore = e.TidelandShoreland
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ WftRestAccess *********************

UPDATE #AreaData 
	SET WftRestAccess = e.WfntRestrictedAccess
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ HvyTraf *********************

UPDATE #AreaData 
	SET HvyTraf = e.TrafficNoise
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ AirPort *********************

UPDATE #AreaData 
	SET AirPort = e.AirportNoise
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Contamination *********************

UPDATE #AreaData 
	SET Contamination = e.Contamination
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ WftAccRght *********************

UPDATE #AreaData 
	SET WftAccRght = e.WfntAccessRights
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ WftProxInflu *********************

UPDATE #AreaData 
	SET WftProxInflu = e.WfntProximityInfluence
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ OthNuis *********************

UPDATE #AreaData 
	SET OthNuis = e.OtherNuisances
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ AdjGolf *********************

UPDATE #AreaData 
	SET AdjGolf = e.AdjacentGolfFairway
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ AdjGreen *********************

UPDATE #AreaData 
	SET AdjGreen = e.AdjacentGreenbelt
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ LandSlide *********************

UPDATE #AreaData 
	SET LandSlide = e.LandslideHazard
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Slope *********************

UPDATE #AreaData 
	SET Slope = e.SteepSlopeHazard
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Stream *********************

UPDATE #AreaData 
	SET Stream = e.Stream
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Wetland *********************

UPDATE #AreaData 
	SET Wetland = e.Wetland
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ WaterProb *********************

UPDATE #AreaData 
	SET WaterProb = e.WaterProblems
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ OthProb *********************

UPDATE #AreaData 
	SET OthProb = e.OtherProblems
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ SrCitFlag *********************

UPDATE #AreaData 
	SET SrCitFlag = e.SrCitFlag
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ TaxStatus *********************

UPDATE #AreaData 
	SET TaxStatus = e.TaxStatus
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ PresentUse *********************

UPDATE #AreaData 
	SET PresentUse = e.PresentUse
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ SegMergeDate *********************

UPDATE #AreaData 
	SET SegMergeDate = e.SegMergeDate
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ PermitNbr *********************

UPDATE #AreaData 
	SET PermitNbr = e.PermitNbr
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ PermitDate *********************

UPDATE #AreaData 
	SET PermitDate = e.PermitDate
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ PermitValue *********************

UPDATE #AreaData 
	SET PermitValue = e.PermitValue
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ PermitType *********************

UPDATE #AreaData 
	SET PermitType = e.PermitType
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ PermitDescr *********************

UPDATE #AreaData 
	SET PermitDescr = e.PermitDescr
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ NotASale *********************

UPDATE #AreaData 
	SET NotASale = CASE WHEN(AVRatio = 'NotASale') THEN
						AVRatio
						ELSE
						'0'
				  END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ NoYB *********************

UPDATE #AreaData 
	SET NoYB = CASE WHEN(e.YrBuilt > 1900) THEN
						'0'
						ELSE
						'NoYB'
				  END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ ImpCount *********************

UPDATE #AreaData 
	SET ImpCount = CASE WHEN(e.ImpCnt > 1) THEN
						'ImpCount'
						ELSE
						'0'
				  END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Mobile *********************

UPDATE #AreaData 
	SET Mobile = CASE WHEN(e.MHType = 'Pers Prop') THEN
						'PersMH'
						ELSE (CASE WHEN(e.MHType = 'Real Prop') THEN
								'RealMH'
								ELSE
								'0'
						END)
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--IN THIS PART, VERIFY THE COLUMN WARNING AGAIN **** orWarnings.Copy .Range("FL1") *****


--************ [%Complete] *********************

UPDATE #AreaData 
	SET [%Complete] = CASE WHEN(e.PcntComplete > 0) THEN
								'%Compl'
								ELSE
								'0'
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Obsolescence *********************

UPDATE #AreaData 
	SET Obsolescence = CASE WHEN(e.Obsolescence > 0) THEN
								'Obsol'
								ELSE
								'0'
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ UnfinFloor *********************

UPDATE #AreaData 
	SET UnfinFloor = CASE WHEN(e.SqFtUnfinFull > 0 OR e.SqFtUnfinHalf > 0) THEN
								'UnfinArea'
								ELSE
								'0'
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ [%NetCond] *********************

UPDATE #AreaData 
	SET [%NetCond] = CASE WHEN(e.PcntNetCondition > 0) THEN
								'%NetCond'
								ELSE
								'0'
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ [PrevLand<=10000] *********************

UPDATE #AreaData 
	SET [PrevLand<=10000] = CASE WHEN(e.RollLandVal <= @MinLandValPop) THEN
										'PrevLand<=' + CONVERT(NVARCHAR, @MinLandValPop / 1000) + 'K'
										ELSE
										'0'
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ [PrevImp<=10000] *********************

UPDATE #AreaData 
	SET [PrevImp<=10000] = CASE WHEN(e.RollImpsVal <= @MinImpsValPop) THEN
										'PrevImp<=' + CONVERT(NVARCHAR, @MinImpsValPop / 1000) + 'K'
										ELSE
										'0'
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ BadRatio *********************

UPDATE #AreaData 
	SET BadRatio = CASE WHEN(AVRatio = 'NotASale' OR CONVERT(FLOAT, AVRatio) < 0.25 OR CONVERT(FLOAT, AVRatio) > 1.75) THEN
										'DORRatio'
										ELSE
										'0'
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ Permits *********************

UPDATE #AreaData 
	SET Permits = CASE WHEN(e.PermitValue > @MinPermitVal AND e.PermitDate < e.SaleDate) THEN
						'ActivePermitBeforeSale>' + CONVERT(NVARCHAR, @MinPermitVal / 1000) + 'K'
						ELSE
						'0'
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ NoPercNoWater *********************

UPDATE #AreaData 
	SET NoPercNoWater = CASE WHEN(e.SewerSystem >= 3 OR WaterSystem >= 3) THEN
						'NoPercNoWater'
						ELSE
						'0'
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ AcctNo ********************* 

UPDATE #AreaData 
	SET AcctNo = CONVERT(FLOAT, e.Major) * 10000 + CONVERT(FLOAT, e.Minor)
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID
		
--************ TrendedAVRatio *********************

UPDATE #AreaData 
	SET TrendedAVRatio = CASE WHEN(e.SalePrice = 0 ) THEN
							'NotASale'
						ELSE (CASE WHEN(e.TrendedPrice = 0) THEN
								'NoTrendedPrice'
								ELSE
								CONVERT(NVARCHAR, e.RollValTotal / e.TrendedPrice)
						END)
					END
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ AssignedBoth *********************

UPDATE #AreaData 
	SET AssignedBoth = e.AssignedBoth
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ NewConstrVal *********************

UPDATE #AreaData 
	SET NewConstrVal = e.NewConstrVal
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ NbrBldgSites *********************

UPDATE #AreaData 
	SET NbrBldgSites = e.NbrBldgSites
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ PowerLines *********************

UPDATE #AreaData 
	SET PowerLines = e.PowerLines
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ DistrictName *********************

UPDATE #AreaData 
	SET DistrictName = e.DistrictName
	FROM #SalesPopFinal e
       INNER JOIN #AreaData a
	ON e.ID = a.ID

--************ GRADEQC *********************
UPDATE #AreaData 
	SET GRADEQC = IIF(e.IMPCNT > 0,ROUND((5.344) + (0.6843 * (IIF(e.PresentUse=29,1,0))) + (0.2319 * (IIF(e.SewerSystem=1,1,0))) + (-2.045 * (IIF(e.SewerSystem=3,1,0))) + (0.4225 * (IIF(e.SewerSystem=2,1,0))) + (0.0005946 * (IIF(e.SewerSystem=4,1,0))) + (0.1203 * e.BathTotal) + 
	(0.1756 * FPTot) + (0.0002592 * (e.SqFtOpenPorch +  e.SqFtEnclosedPorch + e.SqFtDeck)) + (0.0004625 * (e.SqFtGarageBasement + e.SqFtGarageAttached)) + (0.0003974 * e.SqFtFinBasement) + (0.0008237 * e.SqFt1stFloor) + (0.0008575 * e.SqFt2ndFloor) + 
	(0.0008625 * e.SqFtHalfFloor) + (0.001049 * e.SqFtUpperFloor) + (0.002634 * (e.YrBuilt-1900)) + (-0.1109 * LOG(e.Bedrooms+1)) + (-0.2927 * e.NbrLivingUnits) + (0.004997 * e.BrickStone), 1),0)
	FROM #SalesPopFinal e
        JOIN #AreaData a
	ON e.ID = a.ID

--************ GRADEQCDIFF *********************
UPDATE #AreaData 
	SET GRADEQCDIFF = IIF(e.IMPCNT > 0,ROUND(GRADEQC-e.BldgGrade, 1),0)
	FROM #SalesPopFinal e
        JOIN #AreaData a
	ON e.ID = a.ID

--************ Notes *********************
UPDATE #AreaData 
	SET Notes = e.Notes
	FROM #SalesPopFinal e
        JOIN #AreaData a
	ON e.ID = a.ID

--************* MULTI-PARCEL ENUM (MyClass)*************
UPDATE #AreaData
   SET MyClass = 1
 WHERE Enum IN (SELECT Enum FROM #AreaData WHERE Enum > 0 GROUP BY Enum HAVING COUNT(1) > 1);

--************* [DoubleSale?] Update **********************
UPDATE #AreaData
	SET [DoubleSale?] = 'No'
	WHERE MyClass = 8;


MERGE INTO #AreaData a
   USING (
          SELECT ID FROM   (SELECT *,
               COUNT(*) OVER (PARTITION BY Major, Minor) AS cnt
        FROM #AreaData WHERE MyClass = 8) e
		WHERE cnt > 1
         ) S
      ON a.ID = S.ID
WHEN MATCHED THEN
   UPDATE 
      SET [DoubleSale?] = 'Yes';


 --************ Double Repeat RealProp Delete  *********************
--DELETE FROM A
--FROM #AreaData AS A
--INNER JOIN #AreaData AS B
--  ON A.Major = B.Major AND A.Minor = B.Minor AND A.ID > B.ID


--WITH cte AS (
--    SELECT 
--        Major,
--		Minor,
--        ROW_NUMBER() OVER (
--            PARTITION BY Major, Minor
--            ORDER BY Major DESC, Minor DESC, Enum DESC) rownum
--    FROM 
--        #AreaData
--) 
--DELETE FROM cte WHERE rownum > 1;


--************ OTHER FIELDS SHEET *************

--************* RemBefore **********************
UPDATE #AreaData
	SET RemBefore = CASE WHEN ([Remove?] <> '' AND [Remove?] IS NOT NULL) THEN
						[Remove?]
						ELSE
						'0'
					END
	
--************ RemDuring *********************
UPDATE #AreaData
	SET	RemDuring = '0'
	

--************* Notes1 **********************
UPDATE #AreaData
	SET Notes1 = '0'
	
--************* Notes2 **********************
UPDATE #AreaData
	SET Notes2 = '0'
	WHERE MyClass = 8;

UPDATE #AreaData
	SET Notes2 = CASE WHEN (e.CurrentUseDesignation > 0 ) THEN
					'OpenSpace'
					ELSE
					CONVERT(NVARCHAR, e.CurrentUseDesignation)
					END
	FROM #AreaData a
	INNER JOIN #SalesPopFinal e
	ON a.ID = e.ID
	WHERE a.MyClass != 8
	AND a.MyClass != 0
	AND a.Enum = e.ExciseTaxNbr

--************* OutlierType **********************
UPDATE #AreaData
	SET OutlierType = '0'

--************* [DoubleSale?] **********************
UPDATE #AreaData
	SET [DoubleSale?] = '0'
	WHERE MyClass != 8
	AND MyClass != 0;
	
--************* YrBltRenGroup **********************
UPDATE #AreaData
	SET YrBltRenGroup = CASE WHEN YrBltRen  < 1900 THEN '0'
							 WHEN YrBltRen BETWEEN 1900 AND 1909 THEN '1900-1909'
							 WHEN YrBltRen BETWEEN 1910 AND 1919 THEN '1910-1919'
							 WHEN(YrBltRen BETWEEN 1920 AND 1929) THEN '1920-1929'
							 WHEN(YrBltRen BETWEEN 1930 AND 1939) THEN '1930-1939'
							 WHEN(YrBltRen BETWEEN 1940 AND 1949) THEN '1940-1949'
							 WHEN(YrBltRen BETWEEN 1950 AND 1959) THEN '1950-1959'
							 WHEN(YrBltRen BETWEEN 1960 AND 1969) THEN '1960-1969'
							 WHEN(YrBltRen BETWEEN 1970 AND 1979) THEN '1970-1979'
							 WHEN(YrBltRen BETWEEN 1980 AND 1989) THEN '1980-1989'
							 WHEN(YrBltRen BETWEEN 1990 AND 1999) THEN '1990-1999'
							 WHEN(YrBltRen BETWEEN 2000 AND 2009) THEN '2000-2009'
							 WHEN(YrBltRen BETWEEN 2010 AND 2015) THEN '2010-2015'
							 WHEN(YrBltRen > 2015) THEN '>2015'
							 ELSE
							 '0'																			
							 END
	WHERE MyClass != 0;

--************* AGLAGroup **********************
UPDATE #AreaData
	SET AGLAGroup = CASE WHEN AGLA = 0 THEN '0'
						 WHEN AGLA BETWEEN 1 AND 800 THEN '<801'
						 WHEN AGLA BETWEEN 801 AND 1000 THEN '0801-1000'
						 WHEN(AGLA BETWEEN 1001 AND 1500) THEN '1001-1500'
						 WHEN(AGLA BETWEEN 1501 AND 2000) THEN '1501-2000'
						 WHEN(AGLA BETWEEN 2001 AND 2500) THEN '2001-2500'
						 WHEN(AGLA BETWEEN 2501 AND 3000) THEN '2501-3000'
						 WHEN(AGLA BETWEEN 3001 AND 4000) THEN '3001-4000'
						 WHEN(AGLA BETWEEN 4001 AND 5000) THEN '4001-5000'
						 WHEN(AGLA > 5000) THEN '>5000'
						 ELSE
						 '0'																			
						 END
		WHERE MyClass != 0;

--************* ChangeInSelectVal **********************
UPDATE #AreaData
	SET ChangeInSelectVal = CASE WHEN(SelTot > 0) THEN
								SelTot - RollTot
								ELSE
								0
							END
	WHERE MyClass = 8;


SELECT * FROM #AreaData ORDER BY PIN, Enum, MyClass DESC; 

END