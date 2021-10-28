  IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_CondoUnitAddr')
	DROP PROCEDURE [cus].[SRCH_R_CondoUnitAddr]  
GO   

CREATE PROCEDURE [cus].[SRCH_R_CondoUnitAddr]
											 @District	NVARCHAR(500)   
											,@Major		NVARCHAR(6)  
                   
AS  
BEGIN
/*  
Author: Mario Umaña  
Date Created:  15/12/2020  
Description:   SP that pulls all records  filtered by parameter @District @Major. 
  
Modifications:  
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]  
*/

  
SET NOCOUNT ON  
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED   
  
IF OBJECT_ID('Tempdb..#PropType') IS NOT NULL
DROP TABLE Tempdb..#PropType;

CREATE TABLE #PropType
(
	 ptas_propertyTypeId UNIQUEIDENTIFIER PRIMARY KEY
	,PropType NVARCHAR(1)
)

INSERT INTO #PropType
SELECT  ptas_propertyTypeId
       ,SUBSTRING(pt.ptas_name,1,1) 
FROM dynamics.ptas_propertytype AS pt  
  
IF ISNULL(@District,'') = '' SELECT @District = ''  
IF ISNULL(@Major,'') = '' SELECT @Major = ''  
  
SELECT   pd.ptas_major			AS Major
		,pd.ptas_minor			AS Minor
		,pd.ptas_parceldetailid AS RealPropId
		,''						AS AddrId							--Mario comment: Column ignored.
		,pt.PropType			AS PropType
		,pd.ptas_applgroup		AS ApplGroup
		,COALESCE(pd.ptas_addr1_streetnumber,'')	AS StreetNbr
		,COALESCE(pd.ptas_nbrfraction,'')			AS NbrFraction
		,COALESCE(sm.value,'')						AS DirPrefix
		,pd.ptas_streetname							AS StreetName
		,pd.ptas_streettype							AS StreetType
		,COALESCE(pd.ptas_dirsuffix,'')  			AS DirSuffix
		,COALESCE(pd.ptas_addr1_line2,'')			AS UnitDescr
		,CONCAT(LTRIM(COALESCE(pd.ptas_addr1_streetnumber,''))
				   ,' ',LTRIM(pd.ptas_nbrfraction)
				   ,' ',LTRIM(pd.ptas_streetname)
				   ,' ',LTRIM(pd.ptas_streettype)
				   ,' ',LTRIM(COALESCE(pd.ptas_dirsuffix,''))
				   ,' ',LTRIM(COALESCE(pd.ptas_addr1_line2,''))
			   )									AS LineAddrId
		 ,UPPER(pd.ptas_district)					AS City			--Mario comment: We need to check if the value has to be taken from city or district.
		 ,COALESCE(pd.ptas_zipcode,'')				AS ZipCode
 		 , pd.ptas_Major+pd.ptas_Minor				AS PIN
		,CASE
			WHEN pt.PropType = 'K' 
			THEN CONVERT(float, pd.ptas_major + '0000')
			ELSE CONVERT(float, pd.ptas_major + pd.ptas_minor)
		END											AS MapPin
		,ROW_NUMBER() OVER (ORDER BY (SELECT 1))	AS RecId
		
FROM dynamics.ptas_parceldetail				AS pd 
JOIN dynamics.ptas_condounit				AS cu ON cu._ptas_parcelid_value = pd.ptas_parceldetailid
JOIN #PropType								AS pt ON pt.ptas_propertytypeid = pd._ptas_propertytypeid_value
LEFT JOIN [dynamics].[stringmap] sm  
    ON sm.attributevalue = pd.ptas_addr1_directionprefix  
   AND sm.objecttypecode = 'ptas_parceldetail'  
   AND sm.attributename  = 'ptas_addr1_directionprefix' 
WHERE pd.ptas_minor <> '0000' 
 AND ((pd.ptas_district		= @District AND @District <> '')	OR @District = '')  
 AND ((pd.ptas_Major		= @Major	AND @Major <> '')		OR @Major = '')  
 AND COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = '' 
ORDER BY pd.ptas_major
        ,pd.ptas_minor              
END  
  
GO
--[cus].[SRCH_R_CondoUnitAddr]  NULL, NULL--'029940'

