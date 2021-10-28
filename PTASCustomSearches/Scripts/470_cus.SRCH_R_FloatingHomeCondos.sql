IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_FloatingHomeCondos')
	DROP PROCEDURE [cus].[SRCH_R_CondoAreaData]  
GO

CREATE PROCEDURE [cus].[SRCH_R_FloatingHomeCondos]
                  @SalesFrom smalldatetime = NULL
                 ,@SalesTo smalldatetime   = NULL

AS 
BEGIN
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @AssmtYr int

SELECT @AssmtYr = 2020--RPAssmtYr FROM AssmtYr

DROP TABLE IF EXISTS #Parcels;
DROP TABLE IF EXISTS #PropType;
DROP TABLE IF EXISTS #Majors;

CREATE TABLE #PropType
(
	 ptas_propertyTypeId UNIQUEIDENTIFIER PRIMARY KEY
	,PropType NVARCHAR(1)
)

INSERT INTO #PropType
SELECT  ptas_propertyTypeId
       ,SUBSTRING(pt.ptas_name,1,1) 
FROM dynamics.ptas_propertytype AS pt


CREATE TABLE #Majors(
	 Major			char(6)
	,Minor			char(4)
	,ParcelDetailId Uniqueidentifier
	,LandId			Uniqueidentifier	
	,Specialtyarea  Uniqueidentifier NULL
	,Specialtynbhdid Uniqueidentifier NULL
	,Geoareaid		Uniqueidentifier NULL
	,Geonbhdid		Uniqueidentifier NULL
	,Propertyname   varchar(80) NULL
	,PRIMARY KEY (Major,ParcelDetailId)
)

CREATE TABLE #Parcels
 (
	 RecId		int identity(1,1)
	 ,MapPin	char(10) 
	 ,PIN		float
	 ,Major		char(6)
	 ,Minor		char(4)
	 ,GeoArea   int
	 ,GeoNbhd   int
	 ,SpecArea  int
	 ,SpecNbhd  int
	 ,PropType  char(1)
	 ,ApplGroup char(1)
	 ,PropName		varchar(80)
	 ,TaxpayerName	varchar(80)
	 ,SitusAddr		varchar(50)
	 ,SqftLotDry	int
	 ,SqftLotSubmerged int
	 ,SqftLotTotal	int
	 ,PrevLandVal	int
	 ,PrevImpsVal	int
	 ,PrevTotVal	int
	 ,BaseLandVal	int
	 ,BaseLandValTaxYr  int
	 ,BaseLandValDate  smalldatetime
	 ,SelectLand   int
	 ,SelectImps   int
	 ,SelectTotal  int
	 ,RollYr   int
	 ,SelectMethod  varchar(20)
	 ,SelectReason  varchar(20)
	 ,MFInterfaceFlag varchar(40)
	 ,SelectDate  smalldatetime
	 ,SelectAppr  char(4)
	 ,RealPropId  int
	 ,RpGuid  uniqueidentifier
	 ,LandId  int
	 ,LndGuid uniqueidentifier NULL
	 ,PersPropAcctNbr char(8)
	 ,NoteQty int
	 ,Notes  varchar(6000)  
	 ------
	 ,ExciseTaxNbr		int  DEFAULT 0
	 ,SalePrice			int  DEFAULT 0
	 ,SaleDate			smalldatetime  DEFAULT ''
	 ,AtMarket			char(1)  DEFAULT ''
	 ,VerifiedBy		char(4)  DEFAULT ''
	 ,VerifDate			smalldatetime	DEFAULT ''
	 ,SellerName		varchar(300)	DEFAULT ''
	 ,BuyerName			varchar(300)	DEFAULT ''
	 ,PropCnt			smallint		DEFAULT 0
	 ,SaleWarnings		nvarchar(max)   DEFAULT ''
	 ,PropertyType		varchar(50)		DEFAULT ''
	 ,PrinUse			varchar(50)     DEFAULT ''
	 ,PropClass			varchar(50)     DEFAULT ''
	 ,Reason			varchar(50)     DEFAULT ''
	 ,Instr				varchar(50)     DEFAULT ''
	 ,VerifLevel		varchar(50)     DEFAULT ''
 )
   
 
INSERT #Parcels
 ( MapPin
 ,PIN
 ,Major
 ,Minor
 ,GeoArea
 ,GeoNbhd
 ,SpecArea
 ,SpecNbhd
 ,PropType
 ,ApplGroup
 ,PropName
 ,TaxpayerName
 ,SitusAddr
 ,SqftLotDry
 ,SqftLotSubmerged
 ,SqftLotTotal
 ,PrevLandVal
 ,PrevImpsVal
 ,PrevTotVal
 ,BaseLandVal
 ,BaseLandValTaxYr
 ,BaseLandValDate
 ,SelectLand
 ,SelectImps
 ,SelectTotal
 ,RollYr
 ,SelectMethod
 ,SelectReason
 ,MFInterfaceFlag
 ,SelectDate
 ,SelectAppr
 ,RealPropId
 ,RpGuid
 ,LandId
 ,LndGuid
 ,PersPropAcctNbr
 ,NoteQty
 ,Notes
 )
 SELECT
pd.ptas_Major+pd.ptas_Minor	
,CASE
		WHEN pd.ptas_proptype = 'K' 
		THEN CONVERT(float, pd.ptas_major + '0000')
		ELSE CONVERT(float, pd.ptas_major + pd.ptas_minor)
	END							
 
 ,pd.ptas_major					AS Major 
 ,pd.ptas_minor					AS Minor
 ,0								AS GeoArea
 ,0								AS GeoNbhd
 ,0								AS SpecArea
 ,0								AS SpecNbhd
 ,pt.PropType					AS PropType
 ,pd.ptas_applgroup				AS ApplGroup
 ,pd.ptas_propertyname			AS PropName				
 ,COALESCE(pd.ptas_namesonaccount,'')		AS Taxpayername --COALESCE(ta.ptas_taxpayerName, '')
 ,COALESCE(pd.ptas_address,'') AS SitusAddr
 ,0  --SqftLotDry
 ,0  --SqftLotSubmerged
 ,0  --SqftLotTotl
 ,0  --PrevLandVal
 ,0  --PrevImpsVal
 ,0  --PrevTotVal
 ,0  --BaseLandVal
 ,0  --BaseLandValTaxYr
 ,0  --BaseLandValDate
 ,0  --SelectLand
 ,0  --SelectImps
 ,0  --SelectTotal
 ,0  --RollYr
 ,''  --SelectMethod
 ,''  --SelectReason
 ,''  --MFInterfaceFlag
 ,'1/1/1900'  --SelectDate
 ,''  --SelectAppr
 ,0								AS RealPropId
 ,pd.ptas_parceldetailid		AS RpGuid
 ,0  --LandId
 ,pd._ptas_landid_value			AS LndGui --Need to change it
 ,''
 ,0	--RealProp NoteId
 ,''
FROM dynamics.ptas_parceldetail pd
JOIN		#PropType								AS pt	ON pt.ptas_propertytypeid = pd._ptas_propertytypeid_value
WHERE COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = '' 
AND pt.PropType = 'K' and pd.ptas_applgroup = 'F'


------Insert Majors
INSERT INTO #Majors(Major,Minor,ParcelDetailId,LandId,Specialtyarea,Specialtynbhdid,Geoareaid,Geonbhdid,Propertyname)
Select  DISTINCT
		pd.ptas_major
       ,pd.ptas_minor
       ,pd.ptas_parceldetailid
	   ,pd._ptas_landid_value
	   ,pd._ptas_specialtyareaid_value
	   ,pd._ptas_specialtynbhdid_value
	   ,pd._ptas_geoareaid_value
	   ,pd._ptas_geonbhdid_value
	   ,pd.ptas_propertyname
FROM	dynamics.ptas_parceldetail			AS pd
JOIN	#Parcels							AS p  ON p.Major = pd.ptas_major AND COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = ''
WHERE pd.ptas_minor = '0000'

------Add Geo and Spec Areas
UPDATE rvs
SET GeoArea		= pdm.GeoArea
   ,GeoNbhd		= pdm.GeoNbhd
   ,SpecArea	= pdm.SpecArea
   ,SpecNbhd	= pdm.SpecNbhd
   ,PropName	= pdm.Propertyname
   ,LndGuid     = pdm.LndGuid
FROM #Parcels AS rvs
JOIN (
		SELECT   m.Major
				,pga.ptas_areanumber				AS GeoArea
				,pgn.ptas_nbhdnumber				AS GeoNbhd
				,spec.ptas_areanumber				AS SpecArea
				,COALESCE(psn.ptas_nbhdnumber,'')	AS SpecNbhd
				,m.Propertyname						AS Propertyname
				,m.LandId							AS LndGuid
		FROM	#Majors m
			JOIN	dynamics.ptas_specialtyarea			AS spec ON spec.ptas_specialtyareaid		= m.Specialtyarea  
			JOIN	dynamics.ptas_specialtyneighborhood	AS psn  ON psn.ptas_specialtyneighborhoodid = m.Specialtynbhdid
			JOIN	dynamics.ptas_geoarea				AS pga	ON pga.ptas_geoareaid				= m.Geoareaid
            JOIN	dynamics.ptas_geoneighborhood		AS pgn  ON pgn.ptas_geoneighborhoodid		= m.Geonbhdid
) as pdm ON pdm.Major = rvs.Major


UPDATE #Parcels
SET  BaseLandVal	  = l.ptas_baselandvalue
    ,BaseLandValTaxYr = l.ptas_taxyear
    ,BaseLandValDate  = l.ptas_valueDate
	,SqftLotDry		  = l.ptas_drysqft
	,SqftLotSubmerged = l.ptas_submergedsqft
	,SqftLotTotal	  = l.ptas_sqfttotal
FROM dynamics.ptas_land l INNER JOIN #Majors m ON l.ptas_landid = m.LandId

--SELECT TOP 100 *
--FRom ptas.ptas_taxrollhistory	AS tr 



----Add previous values
UPDATE rvs
SET  PrevLandVal = COALESCE(tr.ptas_appraiserlandvalue,0)
    ,PrevImpsVal = COALESCE(tr.ptas_appraiserimpvalue,0)
	,PrevTotVal  = COALESCE(tr.ptas_appraisertotalvalue,0)
FRom ptas.ptas_taxrollhistory	AS tr 
JOIN #Parcels			AS rvs	ON rvs.RpGuid	= tr.ptas_parcelid
JOIN dynamics.ptas_year			AS y	ON y.ptas_yearid = tr.ptas_taxyearid
Where y.ptas_name = @AssmtYr  AND
tr.ptas_receivabletype = 'R'
AND tr.ptas_omityearidname = 0

--Add selected values
UPDATE p    
SET  SelectLand	= COALESCE(ah.ptas_landvalue,0) 
	,SelectImps	= COALESCE(ah.ptas_impvalue,0)	 
	,SelectTotal= COALESCE(ah.ptas_totalvalue,0)
	,SelectAppr	= COALESCE(ah.ptas_legacyid,'')
	,RollYr     = COALESCE(ah.ptas_taxyearidname,'')
	,SelectMethod	= COALESCE(ah.ptas_method,'')					--Hairo Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,SelectReason	= COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_appraisalhistory','ptas_valuationreason',	 ah.ptas_valuationreason) 		,'') 
	,SelectDate		= COALESCE(ah.createdon,'')						--Hairo Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
FROM  #Parcels p 
CROSS APPLY(Select TOP 1 ah.ptas_landvalue,ah.ptas_impvalue,ah.ptas_totalvalue,su.ptas_legacyid,ah.ptas_method,ah.ptas_valuationreason,ah.createdon,ah.ptas_taxyearidname	
			FROM ptas.ptas_appraisalhistory	AS ah	
			JOIN  dynamics.systemuser		AS su	ON ah.ptas_appr = su.systemuserid
			WHERE ah.ptas_parcelid	= p.RpGuid
			AND ah.ptas_taxyearidname = @AssmtYr+1 
			AND ah.ptas_revalormaint in ('R','M') 
			AND ah.ptas_impvalue > 0
			--AND ah.ptas_revalormaint = p.revalormaint				--HairoBF Comment: This flag is not in ptas_parcelDetail
			AND ah.ptas_interfaceflag <> 15
			ORDER BY ah.ptas_appraiserdate DESC
			) ah


UPDATE P
SET p.ExciseTaxNbr	= COALESCE(dps.ptas_excisetaxnumber,0)		
,p.SalePrice		=  dps.ptas_saleprice
,p.SaleDate			=  dps.ptas_saledate
,p.AtMarket			= COALESCE(dps.ptas_atmarket	,'')
,p.VerifiedBy		= COALESCE(dps.ptas_verifiedby,'')
,p.VerifDate		= COALESCE(dps.ptas_verifiedbydate,'')
,p.SellerName		= COALESCE(dps.ptas_grantorfirstname,'') +' '+COALESCE(dps.ptas_grantorlastname,'') /*COALESCE(dps.ptas_sellerName, '')*/
,p.BuyerName		= COALESCE(dps.ptas_granteefirstname,'') +' '+COALESCE(dps.ptas_granteelastname,'')/*COALESCE(dps.ptas_granteelastName, '')*/
,p.PropCnt			= COALESCE(dps.ptas_nbrparcels, '')		
,p.PropertyType		= COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_affidavitpropertytype', dps.ptas_affidavitpropertytype),'0')
,p.PrinUse			= COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_salesprincipleuse'	 , dps.ptas_salesprincipleuse	 ),'0')		
,p.PropClass	    = COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_salepropertyclass'	 , dps.ptas_salepropertyclass	 ),'0')
,p.Reason			= COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_reason'				 , dps.ptas_reason				 ),'0') 
,p.Instr			= COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_instrument'			 , dps.ptas_instrument			 ),'0')
,p.VerifLevel		= COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_levelofverification'	 , dps.ptas_levelofverification	 ),'0')	
,p.SaleWarnings		= COALESCE([dynamics].[fn_ConcatNotes](dps.ptas_salesid,3,2,1024),'')
FROM #Parcels p
INNER JOIN	dynamics.ptas_sales_parceldetail_parcelsinsale AS spdps          
INNER JOIN  dynamics.ptas_sales						AS dps  ON dps.ptas_salesid = spdps.ptas_salesid AND dps.ptas_saleprice > 0 AND dps.ptas_saledate BETWEEN @SalesFrom AND @SalesTo
															ON spdps.ptas_parceldetailid = p.RpGuid



                        
--Get RealProp Notes, adapted from Get Area Data and Get Comm Estimates
--Get Notes .
UPDATE p
SET p.NoteQty = n.NotesQty
	,p.Notes = COALESCE(
						STUFF(
								(select top (5)
										   cn.ptas_notetext+ '(' + convert(char(11),cn.createdon,0)  + ' ' + su.ptas_legacyid + '); ' 
  									  from dynamics.ptas_camanotes cn
									   left join dynamics.systemuser su	on cn.[_modifiedby_value] = su.systemuserid
									 where ptas_attachedentitydisplayname = 'Parcel detail'
									 AND _ptas_parcelid_value = p.RpGuid
									 order by cn.modifiedon desc
								FOR XML PATH('')), 1, 0, ''
							),'')
FROM #Parcels p
CROSS APPLY(select  Count(1) AS NotesQty
  		      from dynamics.ptas_camanotes cn	
			  left join dynamics.systemuser su	on cn.[_modifiedby_value] = su.systemuserid
			where _ptas_parcelid_value = p.RpGuid
AND  ptas_attachedentitydisplayname = 'Parcel detail') as n


UPDATE #Parcels   --added due to QMAS error:  Unknown Error 1004 Application-defined or object-defined error  DSAX 8/02/02 
SET Notes = STUFF(notes, 1, 1, 'Equals ') where left(notes,1)='='

--Added due to DTS problem with notes with CR's obtained from this query and merged into BU spreadsheets by users.
UPDATE #Parcels
SET Notes = REPLACE ( Notes ,CHAR(13)+CHAR(10),CHAR(32) ) 
WHERE (CHARINDEX(CHAR(13), Notes) > 0 AND CHARINDEX(CHAR(10), Notes) > 0)

--Final Select 
SELECT
 Major
 ,Minor
 ,GeoArea
 ,GeoNbhd
 ,SpecArea
 ,SpecNbhd
 ,PropType
 ,ApplGroup
 ,PropName
 ,TaxpayerName
 ,SitusAddr
 ,SqftLotTotal
 ,SqftLotDry
 ,SqftLotSubmerged
 ,PrevLandVal
 ,PrevImpsVal
 ,PrevTotVal
 ,BaseLandVal
 ,BaseLandValTaxYr
 ,BaseLandValDate
 ,SelectLand
 ,SelectImps
 ,SelectTotal
 ,RollYr
 ,SelectMethod
 ,SelectReason
 ,MFInterfaceFlag
 ,SelectDate
 ,SelectAppr
 ,ISNULL(p.ExciseTaxNbr,0) as ExciseTaxNbr
 ,ISNULL(p.SalePrice,0) as SalePrice
 ,ISNULL(p.SaleDate,0) as SaleDate
 ,ISNULL(p.AtMarket,'') as AtMarket
 ,ISNULL(p.VerifiedBy,'') as VerifiedBy
 ,ISNULL(p.VerifDate,'') as VerifDate
 ,ISNULL(p.SellerName,'') as SellerName
 ,ISNULL(p.BuyerName,'') as BuyerName
 ,ISNULL(p.PropCnt,0) as PropCnt
 ,ISNULL(p.SaleWarnings,'') as SaleWarnings
 ,ISNULL(p.PropertyType,'') as PropertyType
 ,ISNULL(p.PrinUse,'') as PrinUse
 ,ISNULL(p.PropClass,'') as PropClass
 ,ISNULL(p.Reason,'') as Reason
 ,ISNULL(p.Instr,'') as Instr
 ,ISNULL(p.VerifLevel,'') as VerifLevel
 ,PersPropAcctNbr
 ,CASE WHEN p.NoteQty >= 5 THEN p.Notes + ' AND OTHER NOTES; ' ELSE p.Notes END AS RealPropNotes
 ,PIN
 ,MapPin
 ,RecId
FROM #Parcels p 
JOIN dynamics.ptas_parceldetail pd on pd.ptas_parceldetailid = p.RpGuid
ORDER BY Major,Minor              
 

END


GO
--
--EXEC [cus].[SRCH_R_FloatingHomeCondos] NULL,NULL
