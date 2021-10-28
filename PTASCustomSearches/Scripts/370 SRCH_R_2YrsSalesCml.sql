
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_2YrsSalesCml')
	DROP PROCEDURE [cus].[SRCH_R_2YrsSalesCml]  
GO
GO

ALTER PROCEDURE [cus].[SRCH_R_2YrsSalesCml]
	@CmlDivisionY [varchar](2),
	@ApplDistrict [varchar](3),
	@GeoArea [varchar](3),
	@GeoNbhd [varchar](3),
	@SpecArea [varchar](3),
	@SpecNbhd [varchar](3)
WITH EXECUTE AS CALLER
AS
BEGIN
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

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
DECLARE @ApplDistrictFullName  varchar(20)
IF @ApplDistrict IS NOT NULL
BEGIN
  SELECT 
  @ApplDistrictFullName = CASE 
                            WHEN @ApplDistrict = 'C' THEN 'CmlC'
                            WHEN @ApplDistrict = 'N' THEN 'CmlN'
                            WHEN @ApplDistrict = 'S' THEN 'CmlS'
                          END
END
*/
DECLARE	@AssmtYr smallint

SELECT @AssmtYr = RPAssmtYr FROM AssmtYr 


CREATE TABLE #RealProp 
   (
     RecId int Identity(1,1) 
   , PIN char(10)
   , MapPin float
   --, RealPropId int
   , ParcelId uniqueidentifier
   --, LandId int
   , LndGuid uniqueidentifier
   , QuarterSection char(2)
   , Section tinyint
   , Township tinyint
   , [Range] tinyint
   , Folio varchar(7)
   , Notes varchar(5000)
       )
       
CREATE TABLE #AllSaleData
  (
  RecId int identity(1,1)
 ,PIN char(10)
 ,MapPin Float
 ,ParcelId uniqueidentifier NULL
 ,RpUnitGuid uniqueidentifier NULL
 ,RpCplxGuid uniqueidentifier NULL
 ,LndGuid uniqueidentifier NULL
 ,Major	char(6)	DEFAULT ''
 ,Minor	char(4)	DEFAULT ''
 ,SalePrice	int	DEFAULT 0
 ,VerifiedPrice	int	DEFAULT 0
 ,SaleDate	smalldatetime	DEFAULT ''
 ,ExciseTaxNbr	int	DEFAULT 0
 ,NbrPclsInSale	int	DEFAULT 0
 ,VerifAtMkt	char(1)	DEFAULT ''
 ,SaleWarnings	char(1)	DEFAULT ''
 ,SaleTypeRP	varchar(25)	DEFAULT ''
 ,AVSP	decimal(20,2)	DEFAULT 0
 ,PrevAVSP	decimal(20,2)	DEFAULT 0
 ,SpSqFtLnd	decimal(20,2)	DEFAULT 0
 ,SpNetSqFtImps	decimal(20,2)	DEFAULT 0
 ,SpUnit	decimal(20,2)	DEFAULT 0
 ,TotVal	int	DEFAULT 0
 ,PostingStatusDescr	varchar(33)	DEFAULT ''
 ,PrevTotVal	int	DEFAULT 0
 ,CmlPredominantUse	varchar(40)	DEFAULT ''
 ,CmlNetSqFt	int	DEFAULT 0
 ,CmlGrossSqft  int DEFAULT 0
 ,SqFtLot	int	DEFAULT 0
 ,CurrentZoning	varchar(50)	DEFAULT ''
 ,PresentUse	varchar(50)	DEFAULT ''
 ,LandProbDescrPart1	varchar(80)	DEFAULT ''
 ,LandProbDescrPart2	varchar(80)	DEFAULT ''
 ,ViewDescr	varchar(100)	DEFAULT ''
 ,WfntFootage	int	DEFAULT 0
 ,WfntBank	varchar(20)	DEFAULT ''
 ,PropName	varchar(80)	DEFAULT ''
 ,TaxPayerName	varchar(30)	DEFAULT ''
 ,AddrLine	varchar(50)	DEFAULT ''
 ,DistrictName	varchar(80)	DEFAULT ''
 ,ApplDistrict	varchar(20)	DEFAULT ''
 ,Team	varchar(20)	DEFAULT ''
 ,GeoArea	varchar(3)	DEFAULT ''
 ,GeoNbhd	varchar(3)	DEFAULT ''
 ,SpecArea	varchar(3)	DEFAULT ''
 ,SpecNbhd	varchar(3)	DEFAULT ''
 ,PropType	char(1)	DEFAULT ''
 ,ApplGroup	char(1)	DEFAULT ''
 ,CmlNbrImps int DEFAULT 0
 ,BuyerName varchar(300) DEFAULT ''
 ,SellerName varchar(300) DEFAULT ''
-- ,VacImpAccy varchar(4) DEFAULT '' 
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
		    ,gmd.WfntBank 
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
/*
DECLARE @StartSaleDate smalldatetime
DECLARE @EndSaleDate smalldatetime --Rev 12/2/2011

SELECT @StartSaleDate = '1/1/2017'

  SELECT --DISTINCT 
  TOP 100 
			 Saleid 	= dps.ptas_name
			,SaleGuid 	= dps.ptas_salesid
			,ParcelId 	= spdps.ptas_parceldetailid
			,SaleDate	= dps.ptas_saledate
			,SalePrice	= dps.ptas_saleprice
			,VerifAtMkt	= CASE 
							WHEN dps.ptas_atmarket = '' THEN 'U'
							WHEN dps.ptas_atmarket = 'Y' THEN 'Y'
							WHEN dps.ptas_atmarket IS NULL THEN ''
						  END   			
			,NbrPclsInSale	= dps.ptas_nbrparcels				
			,ExciseTaxNbr = dps.ptas_excisetaxnumber
	 FROM [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
	INNER JOIN [dynamics].[ptas_sales] dps
	   ON dps.ptas_salesid = spdps.ptas_salesid
WHERE dps.ptas_saleprice > 0
AND dps.ptas_atmarket <> 'N'
AND dps.ptas_saledate >= @StartSaleDate
--AND dps.ptas_salesid = ()
AND dps.ptas_salesid = (SELECT ps2.ptas_salesid
				FROM dynamics.ptas_sales ps2
				WHERE dps.ptas_salesid = ps2.ptas_salesid
				AND dps.ptas_saleprice > 0
				AND dps.ptas_atmarket <> 'N'
				AND dps.ptas_saledate >= @StartSaleDate
				AND dps.ptas_saledate > ps2.ptas_saledate
				)


DECLARE @StartSaleDate smalldatetime
DECLARE @EndSaleDate smalldatetime --Rev 12/2/2011

SELECT @StartSaleDate = '1/1/2017'

--173,516
SELECT spdps.ptas_parceldetailid, dps.ptas_salesid, max(dps.ptas_saledate) MaxSaledate   , max(dps.ptas_name) AS ExciseTaxNbr
			FROM [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
			INNER JOIN [dynamics].[ptas_sales] dps
			ON dps.ptas_salesid = spdps.ptas_salesid
			WHERE dps.ptas_saleprice > 0
			AND dps.ptas_atmarket <> 'N'
			AND dps.ptas_saledate >= '1/1/2017'
			GROUP BY spdps.ptas_parceldetailid, dps.ptas_salesid

SELECT COUNT(1),ptas_recordingnumber FROM [dynamics].[ptas_sales] s
GROUP BY ptas_recordingnumber 
HAVING COUNT(1) >1
/*
ptas_parceldetailid						ptas_salesid							MaxSaledate							ExciseTaxNbr
FDFF1BB7-4C20-EB11-A813-001DD8309530	A1079B8E-583E-4141-B372-00004E2CCC9B	2017-09-12 07:00:00.0000000 +00:00	2888979
07FF35EF-9E8E-4D62-B521-EC2FBC90BBC2	1B3034C7-0F07-4B65-92E4-0000D4691862	2019-07-24 07:00:00.0000000 +00:00	3005121
45A8AB3E-4C20-EB11-A813-001DD8309870	84E66922-7DF0-43AD-BBB9-00036217B9DE	2017-07-05 07:00:00.0000000 +00:00	2876771
9A1D29BA-4C20-EB11-A812-001DD8309D89	84E66922-7DF0-43AD-BBB9-00036217B9DE	2017-07-05 07:00:00.0000000 +00:00	2876771
036221C0-4C20-EB11-A812-001DD8309D89	84E66922-7DF0-43AD-BBB9-00036217B9DE	2017-07-05 07:00:00.0000000 +00:00	2876771
C1AEA532-4C20-EB11-A813-001DD8309870	C56872BC-D34C-42B2-A444-000471FE01F4	2019-10-14 07:00:00.0000000 +00:00	3016126
*/

SELECT TOP 10 * FROM [dynamics].[ptas_sales_parceldetail_parcelsinsale]
ptas_sales_parceldetail_parcelsinsale
ptas_sales


SELECT ps.ptas_name,* FROM dynamics.ptas_sales ps
WHERE ps.ptas_salesid = 'A1079B8E-583E-4141-B372-00004E2CCC9B'

/*
INNER JOIN (
			SELECT spdps.ptas_parceldetailid, dps.ptas_salesid, max(dps.ptas_saledate) MaxSaledate   , max(dps.ptas_name) AS ExciseTaxNbr
			FROM [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
			INNER JOIN [dynamics].[ptas_sales] dps
			ON dps.ptas_salesid = spdps.ptas_salesid
			WHERE dps.ptas_saleprice > 0
			AND dps.ptas_atmarket <> 'N'
			AND dps.ptas_saledate >= @StartSaleDate--'1/1/2017'
			GROUP BY spdps.ptas_parceldetailid, dps.ptas_salesid
			) maxsales
ON dps.ptas_saledate = maxsales.MaxSaledate AND  dps.ptas_excisetaxnumber =  maxsales.ExciseTaxNbr
*/
*/						  
VacLandSale

CmlImpSale 
ResImpSale 

CmlLandSale
ResLandSale

CmlAccySale
ResAccySale

ResMHSale
						  
--Also, add QSTR, since someone requested it and I deliberately left it out of GisMapData (figured a QSTR GIS layer would work better)
INSERT #RealProp 
SELECT gmd.PIN, convert(float,gmd.PIN), rp.Id,gmd.RpGuid,  gmd.LandId,gmd.LndGuid
--, n.NoteId
          ,rp.QuarterSection, rp.Section, rp.Township, rp.[Range], rp.Folio ,''
FROM RealProp rp
INNER JOIN GisMapData gmd ON gmd.RpGuid = rp.RpGuid
--INNER JOIN Note_cmpt n ON rp.RpGuid = n.TblRecGuid
WHERE (CmlLandSale = 'Y' OR CmlAccySale = 'Y' OR CmlImpSale = 'Y')
  AND gmd.PropType = 'C'  AND (
      (ISNULL(@CmlDivisionY,'') = 'Y')
   OR (ApplDistrict =  ISNULL(@ApplDistrictFullName,'') AND ISNULL(@ApplDistrictFullName,'') <> '')
   OR (CONVERT(int,gmd.GeoArea) = ISNULL(@GeoArea,'')  AND ISNULL(@GeoArea,'') <>''   AND ISNULL(@GeoNbhd,'') = '') 
   OR (CONVERT(int,gmd.SpecArea) = ISNULL(@SpecArea,'')  AND ISNULL(@SpecArea,'') <>'') AND ISNULL(@SpecNbhd,'') = '' 
   OR (CONVERT(int,gmd.GeoArea) = ISNULL(@GeoArea,'')  AND CONVERT(int,gmd.GeoNbhd) = ISNULL(@GeoNbhd,'')   AND ISNULL(@GeoArea,'') <>''  AND ISNULL(@GeoNbhd,'') <>'')   
   OR (CONVERT(int,gmd.SpecArea) = ISNULL(@SpecArea,'')  AND CONVERT(int,gmd.SpecNbhd) = ISNULL(@SpecNbhd,'') AND ISNULL(@SpecArea,'') <>''  AND ISNULL(@SpecNbhd,'') <>'')         
         )
ORDER BY 
  gmd.Major  
 ,gmd.Minor
 
INSERT #AllSaleData 
SELECT
  Major + Minor
 ,CONVERT(float,Major + Minor) 
 ,gmd.RpGuid
 ,NULL  --RpUnitGuid
 ,NULL  --RpCplxGuid
 ,gmd.LndGuid
 ,Major  
 ,Minor 
 ,SalePrice   
 ,VerifiedPrice 
 ,SaleDate                
 ,ExciseTaxNbr 
 ,NbrPclsInSale 
 ,VerifAtMkt
 ,SaleWarnings = CASE 
                   WHEN EXISTS (select * from SaleWarning sw 
                                where sw.SaleGuid = gmd.SaleGuid and sw.WarningId = 7) THEN 'Y'
                   ELSE 'N'
                 END
 ,SaleTypeRP
 ,AVSP                                    
 ,PrevAVSP 
 
 ,SpSqFtLnd = ISNULL(SpSqFtLnd,0)  
 ,SpNetSqFtImps = ISNULL(SpNetSqFtImps,0)                           
 ,SpUnit = ISNULL(SpUnit,0) 
 
                             
 ,TotVal
 ,PostingStatusDescr 
 ,PrevTotVal
 
  --Imps
 ,CmlPredominantUse = CASE  
                        WHEN FrozPredominantUse <> '' THEN FrozPredominantUse 
                        ELSE CmlPredominantUse
                      END
 ,CmlNetSqFt = FrozBldgNetSqFt
 ,CmlGrossSqft = 0
 --Land
 ,SqFtLot = CASE  
              WHEN FrozSqFtLot > 0 THEN FrozSqFtLot 
              ELSE SqFtLot
            END
 ,CurrentZoning                                 
 ,PresentUse = CASE  
                 WHEN FrozPresentUse <> '' THEN FrozPresentUse
                 ELSE PresentUse
               END
 ,LandProbDescrPart1 
 ,LandProbDescrPart2                
 ,ViewDescr
 ,WfntFootage 
 ,WfntBank 
 
 ,PropName
 ,TaxPayerName 
 ,AddrLine  
,DistrictName = (select d.DistrictName 
                 from District d inner join Zoning z ON z.DistrictId = d.DistrictId 
                                 inner join Land l on l.CurrentZoning = z.ZoneId 
                 where l.LndGuid = gmd.LndGuid)	
 ,ApplDistrict 
 ,Team           
 ,CONVERT(smallint,GeoArea) as GeoArea 
 ,CONVERT(smallint,GeoNbhd) as GeoNbhd 
 --,GeoAreaNbhd 
 --,GeoAreaId   
 ,CONVERT (smallint,SpecArea) as SpecArea 
 ,CONVERT(smallint,SpecNbhd) as SpecNbhd 
 --,SpecAreaNbhd  
 ,PropType 
 ,ApplGroup
 ,CmlNbrImps
 ,''
 ,''
-- ,VacImpAccy 

 --,rp.Pin
 --,rp.MapPin
 --,rp.RecId
                            
FROM GisMapData gmd
INNER JOIN #RealProp rp ON gmd.RpGuid = rp.RpGuid

UPDATE #AllSaleData
SET CmlGrossSqft = rps.BldgGrossSqft
FROM RPSAggregate_V rps INNER JOIN #AllSaleData asd ON rps.ExciseTaxNbr = asd.ExciseTaxNbr

EXEC ASSR_R_CmlCondoComplexSales @CmlDivisionY,@ApplDistrictFullName,@GeoArea,@GeoNbhd,@SpecArea,@SpecNbhd

--AddCmlNbrBldgs
UPDATE #AllSaleData
SET CmlNbrImps = ISNULL((SELECT SUM(NbrBldgs)
                  FROM CommBldg cb INNER JOIN RealProp r ON cb.RpGuid = r.RpGuid
				  WHERE r.PropType = 'K' and r.Major = s.Major),0)   
FROM #AllSaleData s
WHERE s.Proptype = 'K'

--Add Buyer
UPDATE #AllSaleData
SET BuyerName = ISNULL(bi.Name,'')
FROM BuyerInfo bi INNER JOIN Sale s ON bi.SaleGuid = s.SaleGuid
                  INNER JOIN #AllSaleData a ON s.ExciseTaxNbr = a.ExciseTaxNbr

--Add Seller
UPDATE #AllSaleData
SET SellerName = ISNULL(si.Name,'')
FROM SellerInfo si INNER JOIN Sale s ON si.SaleGuid = s.SaleGuid
                  INNER JOIN #AllSaleData a ON s.ExciseTaxNbr = a.ExciseTaxNbr

SELECT 
 Major
 ,Minor
 ,SalePrice
 ,VerifiedPrice
 ,SaleDate
 ,ExciseTaxNbr
 ,NbrPclsInSale
 ,VerifAtMkt
 ,SaleWarnings
 ,SaleTypeRP
 ,AVSP
 ,PrevAVSP
 ,SpSqFtLnd
 ,SpNetSqFtImps
 ,SpUnit
 ,TotVal
 ,PostingStatusDescr
 ,PrevTotVal
 ,CmlPredominantUse
 ,CmlNetSqFt
 ,CmlGrossSqft
 ,SqFtLot
 ,CurrentZoning
 ,PresentUse
 ,LandProbDescrPart1
 ,LandProbDescrPart2
 ,ViewDescr
 ,WfntFootage
 ,WfntBank
 ,PropName
 ,TaxPayerName
 ,AddrLine
 ,DistrictName
 ,ApplDistrict
 ,Team
 ,GeoArea
 ,GeoNbhd
 ,SpecArea
 ,SpecNbhd
 ,PropType
 ,ApplGroup
 ,CmlNbrImps as NbrImps
 ,BuyerName
 ,SellerName
 ,PIN
 ,MapPin
 ,RecId 
FROM #AllSaleData
ORDER BY Major,Minor



RETURN(@@ERROR)
END



