

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_ResSalesVerification')
	DROP PROCEDURE [cus].[SRCH_R_ResSalesVerification]
GO


CREATE PROCEDURE [cus].[SRCH_R_ResSalesVerification]
	@ApplDistrictCode [varchar](20),
	@ResArea [varchar](3),
	@ResSubArea [varchar](3),
	@AssignedAppr [varchar](30),
	@IncludeNotAtMktY [varchar](3),
	@StartSaleDate [smalldatetime]
WITH EXECUTE AS CALLER
AS
BEGIN
/*
Author: Jairo Barquero
Date Created:  12/01/2020
Description:    SP that pulls all Seridencial sales verifications

Modifications:
29/01/2021 - Hairo Barquero: Modification for PI_or_AU eliminates the ApplGroup filter
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

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

 
/*
Hairo comment: not sure if this is goign to be part on PTAS
INSERT SPUseLog
VALUES(OBJECT_NAME(@@PROCID),sUser_sName(),APP_NAME(),GETDATE())
*/


IF ISNULL(@ApplDistrictCode,'') 	= '' SELECT @ApplDistrictCode 		= ''
IF ISNULL(@ResArea,'') 			= '' SELECT @ResArea 			= ''
IF ISNULL(@ResSubArea,'') 		= '' OR ISNULL(@ResSubArea,'') 	= '0' SELECT @ResSubArea = ''
IF ISNULL(@AssignedAppr,'') 	= '' SELECT @AssignedAppr 		= ''
IF ISNULL(@IncludeNotAtMktY,'') = '' SELECT @IncludeNotAtMktY 	= 'NO'

/*
IF  @IncludeNotAtMktY = 'YES' 
	SET @IncludeNotAtMktY = 'Y' 
ELSE 
	SET @IncludeNotAtMktY = 'N' 
*/
--DECLARE @ApplDistrictCode  varchar(20)
--
--IF @ApplDistrict IS NOT NULL OR @ApplDistrict <> ''
--BEGIN
--	SELECT @ApplDistrictCode = attributevalue
--	  FROM dynamics.stringmap dsm
--	 WHERE dsm.objecttypecode = 'ptas_parceldetail'
--	   AND dsm.attributename  = 'ptas_residentialdistrict'
--	   AND dsm.value 		  = @ApplDistrict
--END


DECLARE @RPAssmtYr int  
/*
Hairo comment: I need to find out how to get the Assesment year
SELECT @RPAssmtYr = RPAssmtYr FROM AssmtYr  
*/
SELECT @RPAssmtYr = 2020

DECLARE @EarlyStartDateInspectionAndSalesVer date
SELECT @EarlyStartDateInspectionAndSalesVer = '9/1/2016' --Hairo comment: already came hardcoded from REalProperty version


DECLARE @Error	Int

IF @AssignedAppr <> ''
/*
Hairo comment: this piece of code have to be replaced with a function or SP that returns an Id and that way I know what was the result of the validation
BEGIN
  IF (select count(*) from  LuItem2 where LuTypeId = (select LuTypeId from LuType2 where LUTypeDesc = 'AssmtEntityRes') and LUItemShortDesc like  '%'+@AssignedAppr+'%' ) = 0 
  BEGIN
    SELECT @Error = 10001
    RAISERROR ('Assigned appraiser input does not match any names or IDs in database', 11, 1)
    GOTO ErrorHandler
  END
  IF (select count(*) from  LuItem2 where LuTypeId = (select LuTypeId from LuType2 where LUTypeDesc = 'AssmtEntityRes') and LUItemShortDesc like  '%'+@AssignedAppr+'%' ) > 1 
  BEGIN
    SELECT @Error = 10002 
    RAISERROR ('Assigned appraiser input matches more than 1 name or ID in database. Please be more specific', 11, 1)
    GOTO ErrorHandler
  END  
  IF (select count(*) from  LuItem2 where LuTypeId = (select LuTypeId from LuType2 where LUTypeDesc = 'AssmtEntityRes') and LUItemShortDesc like  '%'+@AssignedAppr+'%' ) = 1 
  BEGIN
    SELECT @AssignedAppr = (select Abbrv from LuItem2 where LuTypeId = (select LuTypeId from LuType2 where LUTypeDesc = 'AssmtEntityRes') and LUItemShortDesc like  '%'+@AssignedAppr+'%' )
  END  

END
*/

  IF @StartSaleDate IS NULL
  BEGIN
    SELECT @Error = 10001
    RAISERROR ('Please set StartSaleDate', 11, 1)
    GOTO ErrorHandler
  END

  CREATE TABLE #SaleId 
  (
    SaleIdx int
   --,Pin Varchar(10)
   ,ParcelId uniqueidentifier
   ,SalesId uniqueidentifier
   ,SaleDate smalldatetime
   ,SalePrice int
   ,SaleTypeRP varchar(25)  default ''
   ,SpSqFtLnd decimal(20,2) default 0
   ,SpTotLivImps decimal(20,2) default 0
   ,AVSP decimal(20,2) default 0
   ,PrevAVSP decimal(20,2) default 0
   ,SellerName varchar(300)
   ,BuyerName varchar(300)    
   ,PossibleLenderSale char (1) default 'N'
   ,VYVerifiedBy char(4)
   ,VYVerifDate date
   ,VYVerifiedAtMarket char(1)
   ,DistrictName nvarchar(Max)
   ,ExciseTaxNbr int
   ,VYVerifiedLevel nvarchar(Max)
   ,Reason int
   ,Instrument int
   ,IdentifiedBy char(4)
   ,IdentifiedByDate date
   ,NumberParcels int
   ,AffidavitPropertyType int
   ,SalePropertyClass int
   --,TaxPayerName nvarchar(max)
   --,AddrLine nvarchar(max)
   --,ResidentialDistrict int
   --,ResArea smallint
   --,ResSubArea smallint
   --,ResNbhd  varchar(3) 
   )

  IF @IncludeNotAtMktY = 'YES' 
  BEGIN   
     INSERT #SaleId (SaleIdx, ParcelId, SalesId, SaleDate, SalePrice, SellerName, BuyerName, VYVerifiedBy, VYVerifDate, VYVerifiedAtMarket,SaleTypeRP,DistrictName,
					 ExciseTaxNbr,VYVerifiedLevel,Reason,Instrument,IdentifiedBy,IdentifiedByDate,NumberParcels,AffidavitPropertyType,SalePropertyClass
					 --,TaxPayerName,AddrLine,ResidentialDistrict,ResArea,ResSubArea,ResNbhd
					 )
	 SELECT dps.importsequencenumber
			--,PIN = dpd.ptas_name
			,dpd.ptas_parceldetailid
			,dps.ptas_salesid
			,dps.ptas_saledate
			,dps.ptas_saleprice
			,Seller = COALESCE(dps.ptas_grantorfirstname,'') +' '+COALESCE(dps.ptas_grantorlastname,'') 
			,Buyer  = COALESCE(dps.ptas_granteefirstname,'') +' '+COALESCE(dps.ptas_granteelastname,'')
			,dps.ptas_verifiedby
			,dps.ptas_verifiedbydate
			,dps.ptas_atmarket
			,SaleTypeRP = CASE
							WHEN dps.ptas_salepropertyclass in(7,10) THEN 'ResLandSale'
							ELSE ''
						END
			,dpd.ptas_district
			,dps.ptas_excisetaxnumber
			,dps.ptas_verificationlevel
			,dps.ptas_reason
			,dps.ptas_instrument
			,dps.ptas_identifiedby
			,dps.ptas_identifiedbydate
			,dps.ptas_nbrparcels
			,dps.ptas_affidavitpropertytype
			,dps.ptas_salepropertyclass
			--,dpd.ptas_namesonaccount
			--,COALESCE(dpd.ptas_address,''),
			--,dpd.ptas_residentialdistrict
			--,dpd.ptas_resarea
			--,dpd.ptas_ressubarea
	   FROM [dynamics].[ptas_parceldetail] dpd
	  INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
	  INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
		 ON spdps.ptas_parceldetailid = dpd.ptas_parceldetailid
	  INNER JOIN [dynamics].[ptas_sales] dps
	     ON dps.ptas_salesid = spdps.ptas_salesid
		--AND dps.ptas_atmarket = @IncludeNotAtMktY
		AND dps.ptas_saleprice >  0
		AND dps.ptas_saledate  >= @StartSaleDate
	  WHERE pt.PropType = 'R' AND dpd.ptas_applgroup  = 'R'
		AND ((dpd.ptas_residentialdistrict  = @ApplDistrictCode AND @ApplDistrictCode <> '') OR @ApplDistrictCode = '')
		AND ((dpd.ptas_resarea    = @ResArea    AND @ResArea    <>'') OR @ResArea = '')
		AND ((dpd.ptas_ressubarea = @ResSubArea AND @ResSubArea <>'') OR @ResSubArea = '')        
      --AND ((dpd.AssignedBoth = @AssignedAppr AND @AssignedAppr <>'' ) OR @AssignedAppr = '') --Hairo comment I need to find out how calculate AssignedBoth	  
	  ORDER BY dps.ptas_saledate, dpd.ptas_parceldetailid
	END
	
  IF @IncludeNotAtMktY <> 'YES' 
  BEGIN   
 INSERT #SaleId (SaleIdx, ParcelId, SalesId, SaleDate, SalePrice, SellerName, BuyerName, VYVerifiedBy, VYVerifDate, VYVerifiedAtMarket,SaleTypeRP,DistrictName,
					 ExciseTaxNbr,VYVerifiedLevel,Reason,Instrument,IdentifiedBy,IdentifiedByDate,NumberParcels,AffidavitPropertyType,SalePropertyClass
					 --,TaxPayerName,AddrLine,ResidentialDistrict,ResArea,ResSubArea,ResNbhd
					 )
	 SELECT dps.importsequencenumber
			--,PIN = dpd.ptas_name
			,dpd.ptas_parceldetailid
			,dps.ptas_salesid
			,dps.ptas_saledate
			,dps.ptas_saleprice
			,Seller = COALESCE(dps.ptas_grantorfirstname,'') +' '+COALESCE(dps.ptas_grantorlastname,'') 
			,Buyer  = COALESCE(dps.ptas_granteefirstname,'') +' '+COALESCE(dps.ptas_granteelastname,'')
			,dps.ptas_verifiedby
			,dps.ptas_verifiedbydate
			,dps.ptas_atmarket
			,SaleTypeRP = CASE
							WHEN dps.ptas_salepropertyclass in(7,10) THEN 'ResLandSale'
							ELSE ''
						END
			,dpd.ptas_district
			,dps.ptas_excisetaxnumber
			,dps.ptas_verificationlevel
			,dps.ptas_reason
			,dps.ptas_instrument
			,dps.ptas_identifiedby
			,dps.ptas_identifiedbydate
			,dps.ptas_nbrparcels
			,dps.ptas_affidavitpropertytype
			,dps.ptas_salepropertyclass
			--,dpd.ptas_namesonaccount
			--,COALESCE(dpd.ptas_address,''),
			--,dpd.ptas_residentialdistrict
			--,dpd.ptas_resarea
			--,dpd.ptas_ressubarea
			--,ResNbhd = COALESCE(dpn.ptas_name,'')
	   FROM [dynamics].[ptas_parceldetail] dpd
	  INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
	  INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
		 ON spdps.ptas_parceldetailid = dpd.ptas_parceldetailid
	  INNER JOIN [dynamics].[ptas_sales] dps
	     ON dps.ptas_salesid = spdps.ptas_salesid
		--AND dps.ptas_atmarket = @IncludeNotAtMktY
		AND dps.ptas_saleprice >  0
		AND dps.ptas_atmarket <> 'N'
		AND dps.ptas_saledate  >= @StartSaleDate
	   LEFT JOIN dynamics.ptas_neighborhood dpn
    ON dpn.ptas_neighborhoodid = dpd._ptas_neighborhoodid_value
	  WHERE pt.PropType = 'R' AND dpd.ptas_applgroup  = 'R'
		AND ((dpd.ptas_residentialdistrict  = @ApplDistrictCode AND @ApplDistrictCode <> '') OR @ApplDistrictCode = '')
		AND ((dpd.ptas_resarea    = @ResArea    AND @ResArea    <>'') OR @ResArea = '')
		AND ((dpd.ptas_ressubarea = @ResSubArea AND @ResSubArea <>'') OR @ResSubArea = '')        
      --AND ((dpd.AssignedBoth = @AssignedAppr AND @AssignedAppr <>'' ) OR @AssignedAppr = '') --Hairo comment I need to find out how calculate AssignedBoth	  
	  ORDER BY dps.ptas_saledate, dpd.ptas_parceldetailid
	  
	END	

UPDATE #SaleId  
   SET SaleTypeRP	= 'ResImpSale' 
  FROM #SaleId dps
 INNER JOIN dynamics.vw_ResBuildings vrb
 ON vrb._ptas_parceldetailid_value = dps.ParcelId


/*
Hairo Comment: I need to find the way to detect the MHOMEREAL(the calculation is required for GisMapData too) in order to perform the below 2 UPDATES, 
I just need to find where is mapped columns AcctStatusItemId:
MHOMEREAL =
select COUNT(1),dpd.ptas_parceldetailid
from [dynamics].[ptas_parceldetail] dpd
INNER JOIN [dynamics].[ptas_condounit] cu
ON cu._ptas_parcelid_value = dpd.ptas_parceldetailid
and a.AcctStatusItemId in (1,4,10) --Hairo comment: this columns doesn´t exist in PTAS I need to find it in order to do the correct filter 
AND cu.ptas_mobilehometype  = 4
WHERE dpd.ptas_parceldetailid = '517C8DD2-B08E-4083-9DC2-FECBD1B54509'
GROUP BY dpd.ptas_parceldetailid


UPDATE #SaleId  SET SaleTypeRP	= 'ResMHSale' FROM #SaleId s WHERE EXISTS (select * from GisMapData gmd where gmd.RealPropId = s.RealPropId and gmd.ResNbrImps = 0 and MHOMEREAL > 0)
UPDATE #SaleId  SET SaleTypeRP	= 'ResAccySale' FROM #SaleId s WHERE EXISTS (select * from GisMapData gmd where gmd.RealPropId = s.RealPropId and gmd.ResNbrImps = 0 and MHOMEREAL = 0
                                                                            and  AccyPRKDETGAR + AccyPRKCARPORT + AccyPOOL + AccyMISCIMP > 0  )
*/


--Get all res sales for calcs below, but use sum of SqFtLot etc. for multi-pcl
--drop table #ResMultiAggr
CREATE TABLE #ResMultiAggr 
 (
SalesId uniqueidentifier,
SalePrice int, 
ResNbrImps int default 0,
SqFtTotLiving	int	default 0,
NbrLivingUnits	smallint	default 0, 		 	
SqFtLot	 int	default 0,
LandVal bigint 	default 0,
PrevLandVal bigint 	default 0,
TotVal bigint 	default 0,
PrevTotVal bigint 	default 0,
--SaleGuid uniqueidentifier
  )
  
INSERT #ResMultiAggr
SELECT 
 s.SalesId
,s.SalePrice
,SUM(ResNbrImps)
,SUM(SqFtTotLiving)
,SUM(NbrLivingUnits)
,SUM(SqFtLot)
,SUM(LandVal)
,SUM(PrevLandVal)
,SUM(TotVal)
,SUM(PrevTotVal)
FROM dynamics.vw_GisMapData gmd 
INNER JOIN #SaleId s ON gmd.ParcelId = s.ParcelId
GROUP BY s.SalesId ,s.SalePrice


UPDATE #SaleId
SET AVSP = convert(decimal(20,2),rma.LandVal)/convert(decimal(20,2),rma.SalePrice)
FROM #SaleId s 
INNER JOIN #ResMultiAggr rma ON rma.SalesId =  s.SalesId
WHERE rma.LandVal > 0 AND rma.SalePrice > 0
AND SaleTypeRP	= 'ResLandSale'


UPDATE #SaleId
SET PrevAVSP = convert(decimal(20,2),rma.PrevLandVal)/convert(decimal(20,2),rma.SalePrice)
FROM #SaleId s 
INNER JOIN #ResMultiAggr rma ON rma.SalesId =  s.SalesId
WHERE rma.PrevLandVal > 0 AND rma.SalePrice > 0
AND SaleTypeRP	= 'ResLandSale'


UPDATE #SaleId
SET AVSP = convert(decimal(20,2),rma.TotVal)/convert(decimal(20,2),rma.SalePrice)
FROM #SaleId s 
INNER JOIN #ResMultiAggr rma ON rma.SalesId =  s.SalesId
WHERE rma.TotVal > 0 AND rma.SalePrice > 0
AND SaleTypeRP	<> 'ResLandSale'



UPDATE #SaleId
SET PrevAVSP = convert(decimal(20,2),rma.PrevTotVal)/convert(decimal(20,2),rma.SalePrice)
FROM #SaleId s 
INNER JOIN #ResMultiAggr rma ON rma.SalesId =  s.SalesId
WHERE rma.PrevTotVal > 0 AND rma.SalePrice > 0
AND SaleTypeRP	<> 'ResLandSale'


UPDATE #SaleId
SET PossibleLenderSale = 'Y'
FROM #SaleId s
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
					WHERE sswc.ptas_salesid = s.SalesId
					  AND swc.ptas_id in (12,40,41,51)
				  )
  
  




--Get RP data not in GisMapData, including NoteId for RP notes.  Also, add QSTR, since someone requested it and I deliberately left it out of GisMapData (figured a QSTR GIS layer would work better)

CREATE TABLE #ParcelDetails 
(
  RecId int Identity(1,1) 
--, PIN char(10)
--, MapPin float
--, Major Char(6)
--, Minor Char(4)
, ParcelId uniqueidentifier
, NoteId int
, QuarterSection char(2)
, Section tinyint
, Township tinyint
, [Range] tinyint
, Folio varchar(7)
--, Notes varchar(5000)
--, PropType char(1)
--, ApplGroup char(1)
--, TaxStatus varchar(20) 
)

CREATE INDEX idx_Pin01 ON #ParcelDetails 
(
ParcelId
)

	----DISTINCT, because #SaleId will have multiple records with same ParcelId
	INSERT INTO #ParcelDetails
	SELECT DISTINCT 
		   --PIN      = COALESCE(dpd.ptas_major,'') + COALESCE(dpd.ptas_minor,'')
		  --,MapPin   = CASE WHEN pt.PropType = 'K' THEN convert(float, dpd.ptas_major + '0000')
          --                 ELSE convert(float, dpd.ptas_major + dpd.ptas_minor)
	      --        	END
		  --,Major    = dpd.ptas_major
		  --,Minor 	= dpd.ptas_minor
		   ParcelId = ts.ParcelId
		  ,NoteId 	= NULL
		  ,QuarterSection = COALESCE(dsm.value,'')
		  ,Section		  = pqstr.ptas_section	
		  ,Township		  = pqstr.ptas_township
		  ,Range		  = pqstr.ptas_range		
		  ,Folio		  = dpd.ptas_folio
		  --,Notes 		  = ''
		  --,pt.PropType
		  --,dpd.ptas_applgroup
		  --,TaxStatus = [dynamics].[fn_GetValueFromStringMap]('ptas_parceldetail','ptas_taxstatus',dpd.ptas_taxstatus)  --Hairo comment Not sure about this calculation
	  FROM dynamics.ptas_parceldetail dpd
	 INNER JOIN #SaleId ts
	 ON dpd.ptas_parceldetailid = ts.ParcelId

   LEFT JOIN [dynamics].[ptas_qstr] pqstr
     ON pqstr.[ptas_qstrid] = dpd._ptas_qstrid_value
  
   LEFT JOIN [dynamics].[stringmap]	dsm
     ON dsm.attributevalue = pqstr.ptas_quartersection 
    AND dsm.objecttypecode = 'ptas_qstr'
    AND dsm.attributename  = 'ptas_quartersection'


--DROP TABLE #CompletedWork 
CREATE TABLE #CompletedWork 
(
  ParcelId uniqueidentifier
 ,SalesId uniqueidentifier
 ,CompletedWork varchar(100)
 ,AttnRequiredDesc varchar(80)
)
INSERT #CompletedWork 
SELECT DISTINCT ParcelId, SalesId,'','' FROM #SaleId


UPDATE #CompletedWork 
SET AttnRequiredDesc = CASE
                         WHEN cw.AttnRequiredDesc = '' THEN ' Sale Unverified' 
                         ELSE cw.AttnRequiredDesc + ';  ' + ' Sale Unverified' 
                       END
FROM #SaleId  s      
INNER JOIN #CompletedWork cw ON s.ParcelId = cw.ParcelId AND s.SalesId = cw.SalesId       
WHERE s.VYVerifiedAtMarket = '' AND s.SalePrice > 0

UPDATE #CompletedWork 
SET AttnRequiredDesc = CASE
                         WHEN cw.CompletedWork = '' THEN ' Sale Verified' 
                         ELSE cw.CompletedWork + ';  ' + ' Sale Verified' 
                       END
FROM #SaleId  s      
INNER JOIN #CompletedWork cw ON s.ParcelId = cw.ParcelId AND s.SalesId = cw.SalesId       
WHERE s.VYVerifiedAtMarket <> '' AND s.SalePrice > 0 AND s.VYVerifDate > @EarlyStartDateInspectionAndSalesVer


UPDATE #CompletedWork 
SET CompletedWork = 'Compl Inspect = ' + Inspection 
FROM dynamics.vw_GISMapData gmd 
INNER JOIN #CompletedWork cw ON gmd.ParcelId = cw.ParcelId 
WHERE EXISTS (SELECT 1 
								FROM [dynamics].[ptas_inspectionyear] piy
							   INNER JOIN [dynamics].[ptas_area] pa ON  piy._ptas_area_value = pa.ptas_areaid 
							   WHERE piy.ptas_year = @RPAssmtYr
								 AND gmd.ApplGroup = 'R' 
								 AND pa.ptas_areanumber = gmd.ResArea)
AND gmd.Inspection <> ''

/*
Hairo Comment: In order to calculate this I need to have this values from GisMapData view first: PermitCountComplCurCycle


UPDATE #CompletedWork 
SET CompletedWork = CASE
                      WHEN CompletedWork = '' THEN CONVERT(VARCHAR(3),gmd.PermitCountComplCurCycle) + ' Permit Completed' 
                      ELSE CompletedWork + ';  ' + CONVERT(VARCHAR(3),gmd.PermitCountComplCurCycle) + ' Permit Completed' 
                    END
FROM GisMapData gmd
INNER JOIN #CompletedWork cw ON gmd.RealPropId = cw.RealPropId
WHERE gmd.PermitCountComplCurCycle = 1

UPDATE #CompletedWork 
SET CompletedWork = CASE
                      WHEN CompletedWork = '' THEN CONVERT(VARCHAR(3),gmd.PermitCountComplCurCycle) + ' Permits Completed' 
                      ELSE CompletedWork + ';  ' + CONVERT(VARCHAR(3),gmd.PermitCountComplCurCycle) + ' Permits Completed' 
                    END
FROM GisMapData gmd
INNER JOIN #CompletedWork cw ON gmd.RealPropId = cw.RealPropId
WHERE gmd.PermitCountComplCurCycle > 1

*/
--select * from #CompletedWork



  --Final result
  SELECT 
    gmd.Major  
   ,gmd.Minor    
   ,s.SalePrice     
   ,s.SaleDate    
   ,s.ExciseTaxNbr   
   ,s.SaleTypeRP                 
   ,VerifAtMarket = s.VYVerifiedAtMarket
   ,VerifLevel = s.VYVerifiedLevel
   ,VYVerifiedBy = s.VYVerifiedBy
   ,VYVerifDate  = s.VYVerifDate
   ,s.AVSP                                    
   ,s.PrevAVSP       
   ,s.PossibleLenderSale
   ,SaleWarnings 	= dynamics.fn_ConcatNotes(s.SalesId ,3, 2, 5000)
   ,SaleReason  	= [dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_reason'    , s.Reason)  
   ,SaleInstrument  = [dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_instrument', s.Instrument)   
   ,s.IdentifiedBy
   ,s.IdentifiedByDate
   ,s.NumberParcels
   ,s.SellerName 
   ,s.BuyerName    
   ,SaleNotes 	  = dynamics.fn_ConcatNotes(s.SalesId ,3, 1, 5000)
   ,RealPropNotes = dynamics.fn_ConcatNotes(gmd.ParcelID ,3, 0, 5000)
   ,SalePropertyClass     = [dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_salepropertyclass'    , s.SalePropertyClass)  
   ,AffidavitPropertyType = [dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_affidavitpropertytype', s.AffidavitPropertyType)    
   ,gmd.TaxPayerName 
   ,gmd.AddrLine  
   ,gmd.DistrictName
   ,gmd.ApplDistrict
 --,Team     Hairo comment: needs to be calculated, here and in GisMapData, I need to be sure the columnsis required.
   ,gmd.ResArea 
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
   ,gmd.ResSubArea 
   ,gmd.ResNbhd 
   ,Qtr = pd.QuarterSection  
   ,Sec = pd.Section 
   ,Twn = pd.Township 
   ,Rng = pd.[Range] 
   ,pd.Folio
   ,gmd.AssignedBoth
   ,LandInspectedDate = isnull((select ptas_inspectiondate from [ptas].[ptas_inspectionhistory] insp where insp.ptas_parcelid = gmd.ParcelId and insp.ptas_inspectiontype = 'Land' and insp.AssmtYr = @RPAssmtYr),'') 
   ,ImpsInspectedDate = isnull((select ptas_inspectiondate from [ptas].[ptas_inspectionhistory] insp where insp.ptas_parcelid = gmd.ParcelId and insp.ptas_inspectiontype = 'Imps' and insp.AssmtYr = @RPAssmtYr),'')
   ,BothInspectedDate = isnull((select ptas_inspectiondate from [ptas].[ptas_inspectionhistory] insp where insp.ptas_parcelid = gmd.ParcelId and insp.ptas_inspectiontype = 'Both' and insp.AssmtYr = @RPAssmtYr),'')   
   ,Bookmark = ISNULL(REVERSE(SUBSTRING(REVERSE((
												SELECT bt.ptas_name + '; '  
												FROM dynamics.ptas_bookmark	AS bm						
												INNER JOIN dynamics.ptas_ptas_bookmark_ptas_bookmarktag	AS bmt	ON bmt.ptas_bookmarkid = bm.ptas_bookmarkid
												INNER JOIN dynamics.ptas_bookmarktag					AS bt	ON bt.ptas_bookmarktagid = bmt.ptas_bookmarktagid
												where s.ParcelId = bm._ptas_parceldetailid_value
												For XML PATH('')
												)),2,8000)),'')
  
/*
Hairo comment: 	No idea how to calculate this, the table Checkout in Real Property is EMPTY.
  ,CkOutBy = ISNULL((select CkOutBy + ' ' from Checkout ck where ck.RealPropId = gmd.RealPropId and CkOutType = 'B'),'')
            +ISNULL((select 'Lnd to '+ CkOutBy + ' ' from Checkout ck where ck.RealPropId = gmd.RealPropId and CkOutType = 'L'),'')
            +ISNULL((select 'Imps to '+ CkOutBy + ' ' from Checkout ck where ck.RealPropId = gmd.RealPropId and CkOutType = 'I'),'')
*/
   ,CompletedWork = cw.CompletedWork --Hairo comment: It was parcially calculated
   ,AttentionRequired = cw.AttnRequiredDesc  
   ,gmd.PropType 
   ,gmd.ApplGroup 
   ,gmd.TaxStatus 
 
 --,VacImpAccy 		Hairo comment: needs to be calculated in GisMapData view
 --,GeneralClassif	Hairo comment: depends on VacImpAccy it is calculated in GisMapData too
   
   --List of Improvements
   ,gmd.ResNbrImps	
   ,gmd.BldgGrade	
   ,gmd.YrBltRen 
   ,gmd.YrBuiltRes	
   ,gmd.YrRenovated	
   ,ResTotLiv = gmd.SqFtTotLiving
   ,gmd.UnfinArea 

   ,gmd.PcntComplRes		
   ,gmd.Obsolescence	
   ,gmd.PcntNetCondition
   ,gmd.Condition	
   ,gmd.CondDescr		

 --,RP_MH_Count = MHOMEREAL Hairo comment: needs to be calculated first in GisMapData view
 --,PP_MH_Count  			Hairo comment: needs to be calculated first in GisMapData view

   ,gmd.NbrResAccys 
   ,gmd.AccyPRKDETGAR 
   ,gmd.AccyPRKCARPORT 
   ,gmd.AccyPVCONCRETE 
   ,gmd.AccyPOOL
   ,gmd.AccyMISCIMP 
 --,AccyONSITEDEVCST  Hairo comment: doesnt exits anymore, it become MiscImp
 --,AccyFLATVALUEDIMP Hairo comment: doesnt exits anymore, it become MiscImp
   
   --Land characteristics
/*
Hairo comment: requires the calculation of FrozSqFtLot, and it is not yet in GisMapData view
   ,SqFtLot = CASE  
                WHEN FrozSqFtLot > 0 THEN FrozSqFtLot 
                ELSE SqFtLot
              END
*/             
   ,gmd.CurrentZoning  
/*
Hairo comment: requires the calculation of FrozPresentUse, and it is not yet in GisMapData view  
   ,PresentUse = CASE  
                   WHEN FrozPresentUse <> '' THEN FrozPresentUse 
                   ELSE PresentUse
                 END
*/				 
 --,LandProbDescr = LandProbDescrPart1 + LandProbDescrPart2 Hairo Comment: these 2 columns needs to be calculated in GisMapData view
   ,gmd.ViewDescr
   ,gmd.WfntFootage 
   ,gmd.WfntBank 
   ,gmd.AdjacentGolfFairway 
   ,gmd.AdjacentGreenbelt 

   ,CurrentUseExmpt = CASE 
                       WHEN vwLd._ptas_landid_value IS NOT NULL THEN 'Y'
                       ELSE ''
                      END
   ,gmd.BaseLandVal
   ,LastBLVTaxYr = gmd.BaseLandValTaxYr 
   ,gmd.BaseLandValDate 
   ,gmd.BLVSqFtCalc
   ,PcntBLVChg = CASE 
                WHEN gmd.BaseLandVal > 0 AND gmd.PrevLandVal > 0 AND gmd.BaseLandValTaxYr = (select 2020 + 1 ) --Hairo comment, all related to Assesment year need to be calculated when the year table will be published
                   THEN 100 * (( convert(decimal(20,2),gmd.BaseLandVal)/convert(decimal(20,2),gmd.PrevLandVal)-1) ) 
                   ELSE 0
                END
    --Values
   ,gmd.LandVal
   ,gmd.ImpsVal
   ,gmd.TotVal
   ,gmd.NewConstrVal

   ,gmd.SelectMethod 
 --,PostingStatusDescr Hairo comment: needs to be calculated in GisMapData view

   ,gmd.PrevLandVal
   ,gmd.PrevImpsVal
   ,gmd.PrevTotVal
   ,gmd.PrevNewConstrVal
   ,gmd.PrevSelectMethod

   ,gmd.PcntChgLand
   ,gmd.PcntChgImps
   ,gmd.PcntChgTotal
  
 --,PermitTypesIncompl        Hairo comment: the calculation of this colums depends on GisMapData
 --,PermitTypesComplCurCycle  Hairo comment: the calculation of this colums depends on GisMapData
 --,PermitTypesComplPrevCycle Hairo comment: the calculation of this colums depends on GisMapData
  
   ,gmd.PIN
   ,MapPin = convert(float,gmd.PIN)
   ,pd.RecId
       
 FROM dynamics.vw_GISMapData gmd
INNER JOIN #ParcelDetails pd ON gmd.ParcelID = pd.ParcelID
INNER JOIN #SaleId s ON gmd.ParcelID = s.ParcelID
INNER JOIN #CompletedWork cw ON gmd.ParcelID = cw.ParcelID  AND s.SalesId = cw.SalesId 
--LEFT JOIN #SaleWarnings sw ON s.SalesId = sw.SalesId -- Hairo comment: we can improve the way this table is loaded creating a function that return the warings notes, like Sales Notes
--LEFT JOIN #SaleNotes sn ON s.SaleGuid = sn.SaleGuid  
 LEFT JOIN dynamics.vw_LandValueDesignations vwLd 
   ON vwLd._ptas_landid_value = gmd.LndGuid
ORDER BY pd.RecId

RETURN(0)

ErrorHandler:
RETURN (@Error)

RETURN(@@ERROR)
END


GO


