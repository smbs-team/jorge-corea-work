


IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_TaxpayerNameSearch')
	DROP PROCEDURE [cus].[SRCH_R_TaxpayerNameSearch]  
GO
CREATE PROCEDURE [cus].[SRCH_R_TaxpayerNameSearch]  @TaxpayerName		varchar(80)

AS BEGIN
/*
Author: Jairo Barquero
Date Created:  11/12/2020
Description:    SP that pulls all Tax Payer names according to the received parameter @TaxpayerName

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/

SET DEADLOCK_PRIORITY HIGH
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

Create Table #TaxpayerNameSearch
	(RecId int Identity(1,1)
    ,ParcelId uniqueidentifier
	,Major char(6) Null 
	,Minor char(4) Null 
    ,Area smallint
    ,SubArea smallint
	,QuarterSection char(2) Null 
	,Section tinyint Null 
	,Township tinyint Null 
	,Range tinyint Null 
	,LevyCode char(4) Null
	,PlatLot char(14) Null
 	,PlatBlock char(7) Null
	,TaxpayerName nvarchar(Max) Null 
	,StreetNbr varchar(5) Null 
	,NbrFraction varchar(3) Null 
	,DirPrefix char(2) Null 
	,StreetName nvarchar(Max) Null 
	,StreetType char(4) Null 
	,DirSuffix char(2) Null 
	,UnitDescr nvarchar(Max) NULL
	,PropType varchar(30) NULL --Hairo comment changed from "char(1)" to varchar(30) due "Mobile Home" and "Floating Home", when finish this search I need to know if I´m not including these in the resultset.
	,AcctNbr char(12) Null
    ,Pin char(10)
    ,MapPin float)

Create Index Ind1 ON #TaxpayerNameSearch (ParcelId,Major)	   

Create Table #SearchValues
	(ParcelId uniqueidentifier)
	
Create Index Indx01 ON #SearchValues (ParcelId)	   
/*
This SP cus.sp_ValuesbyFilter returns a list of values, in this case return all ParcelsID(ptas_parceldetailid) in table "ptas_parceldetail"
where ptas_namesonaccount like @TaxpayerName, but there is a trick, if @TaxpayerName is something like "WILSON JOHN" the SP returns all records
where ptas_namesonaccount like 'WILSON &' AND ptas_namesonaccount like 'JOHN &', the list will contain all ParcelIds where Tax Payer Name contains 
the exact names "WILSON" and "JOHN" anywhere in the name.
*/
--INSERT INTO #TaxpayerNameSearch(ParcelId)
exec cus.sp_ValuesbyFilter @TaxpayerName, ' ','ptas_parceldetail','ptas_namesonaccount','ptas_parceldetailid'

INSERT INTO #TaxpayerNameSearch(ParcelId,Major,Minor,Area,SubArea,PropType,StreetNbr,NbrFraction,DirPrefix,StreetName,StreetType,DirSuffix,UnitDescr,QuarterSection,Section,Township,Range,LevyCode,PlatLot,PlatBlock,TaxpayerName,Pin,MapPin,AcctNbr)
SELECT  ParcelId    =dpd.ptas_parceldetailid
 	   ,Major 		= dpd.ptas_major 
 	   ,Minor 		= dpd.ptas_minor   
--	   ,Area 		= COALESCE(dpa.ptas_areanumber,'')
--	   ,SubArea		= COALESCE(dps.ptas_name,'')
	   ,Area 		= CASE WHEN pt.PropType = 'R' THEN  COALESCE(dpd.ptas_resarea,'')
						   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN  dpd.ptas_commarea
						   ELSE dpd.ptas_commarea
					  END
	   ,SubArea		= CASE WHEN pt.PropType = 'R' THEN  COALESCE(dpd.ptas_ressubarea,'')
						   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN dpd.ptas_commsubarea
						   ELSE dpd.ptas_commsubarea
					  END	   
 	   ,PropType  	= pt.PropType
	   ,StreetNbr   = COALESCE(dpd.ptas_addr1_streetnumber,'')	   
	   ,NbrFraction = COALESCE(dpd.ptas_nbrfraction,'')
	   ,DirPrefix	= COALESCE(dsm02.value,'')
	   ,StreetName  = dpd.ptas_streetname
	   ,StreetType	= dpd.ptas_streettype
	   ,DiRSuffix	= dpd.ptas_dirsuffix
	   ,UnitDescr	= dpd.ptas_addr1_line2
	   ,QuarterSection= COALESCE(dsm.value,'')
	   ,Section		= pqstr.ptas_section	
	   ,Township	= pqstr.ptas_township
	   ,Range		= pqstr.ptas_range		
 	   ,LevyCode 	= dpd.ptas_levyCode    
	   ,PlatLot     = COALESCE(dpd.ptas_platlot,'')
	   ,PlatBlock   = COALESCE(dpd.ptas_platblock,'')
	   ,TaxpayerName= dpd.ptas_namesonaccount
 	   ,PIN   		= COALESCE(dpd.ptas_major,'') + COALESCE(dpd.ptas_minor,'')
	   ,MapPin      = CASE WHEN pt.PropType = 'K' then convert(float, dpd.ptas_major + '0000')
                       ELSE convert(float, dpd.ptas_major + dpd.ptas_minor)
					  END
	   ,AcctNbr		= dpd.ptas_acctnbr					  
  FROM dynamics.ptas_parceldetail dpd 
 INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
 INNER JOIN #SearchValues sv
    ON sv.ParcelId = dpd.ptas_parceldetailid
  LEFT JOIN dynamics.ptas_area dpa
    ON dpd._ptas_areaid_value = dpa.ptas_areaid 
	
  LEFT JOIN dynamics.ptas_subarea dps
    ON dps.ptas_subareaid = dpd._ptas_subareaid_value 
  
  LEFT JOIN [dynamics].[ptas_qstr] pqstr
    ON pqstr.[ptas_qstrid] = dpd._ptas_qstrid_value
  
  LEFT JOIN [dynamics].[stringmap]	dsm
    ON dsm.attributevalue = pqstr.ptas_quartersection 
   AND dsm.objecttypecode = 'ptas_qstr'
   AND dsm.attributename  = 'ptas_quartersection'

  LEFT JOIN [dynamics].[stringmap]	dsm02
    ON dsm02.attributevalue = dpd.ptas_addr1_directionprefix
   AND dsm02.objecttypecode = 'ptas_parceldetail'
   AND dsm02.attributename  = 'ptas_addr1_directionprefix'	
 WHERE COALESCE(dpd.ptas_splitcode,0) = 0
   AND (dpd.statecode  = 0 AND dpd.statuscode = 1)
   AND COALESCE(dpd.ptas_snapshottype,'') = '' 

SELECT TaxpayerName
	  ,Major
	  ,Minor
	  ,PropType
	  ,StreetNbr
	  ,NbrFraction
	  ,DirPrefix
	  ,StreetName
	  ,StreetType
	  ,DirSuffix
	  ,UnitDescr
	  ,Area
	  ,SubArea
	  ,QuarterSection
	  ,Section
	  ,Township
	  ,Range
	  ,LevyCode
	  ,PlatLot
	  ,PlatBlock
	  ,Pin
	  ,MapPin
 FROM #TaxpayerNameSearch
ORDER BY TaxpayerName, Major, Minor

END