IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_CondoList')
	DROP PROCEDURE [cus].[SRCH_R_CondoList]  
GO

CREATE PROCEDURE [cus].[SRCH_R_CondoList]
										 @SuperGroupId		smallint = NULL  
										,@SpecNeighborhood	smallint = NULL  
										,@IncludeRouting	char(1) = NULL  
AS   
/*
Author: Mario Umaña
Date Created:  Dec 07 2020
Description:   Returns, complex, counts of total and residential units in each complex, and other data.

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
BEGIN  
SET NOCOUNT ON  
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED   
  
DECLARE @SuperGroup varchar(50)  
        ,@AssmtYr smallint  
  
SELECT @AssmtYr = 2020  --RpAssmtYr FROM AssmtYr  

/*
Mario comment: SuperGroup is currently empty 
[dynamics].[ptas_parceldetail]. 
  
SELECT @SuperGroup = LUItemShortDesc From LUItem2 WHERE LUTypeId = 347 AND LUItemId = @SuperGroupId  
*/  

IF ISNULL(@SuperGroup,'') = '' SELECT @SuperGroup = ''  
IF ISNULL(@SpecNeighborhood,'') = '' SELECT @SpecNeighborhood = ''  

IF OBJECT_ID('tempdb..#CondoList1')IS NOT NULL
DROP TABLE tempdb..#CondoList1

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

CREATE TABLE #CondoList1  
    (  
     RecId					int  identity(1,1)  
    ,PIN					char(10)  
    ,MapPin					float      
    ,ParcelId				uniqueidentifier  
    ,LndGuid				uniqueidentifier  
    ,xCoord					decimal(18,9)  
    ,yCoord					decimal(18,9)  
    ,ApplGroup				char(1)    
    ,PropType				char(1)    
    ,Fol					char(7)    
    ,Complex				varchar(80)   
    ,Maj					char(6)  
    ,YrBuilt				int  
    ,EffYr					int  
    ,TotUnits				smallint    
    ,RUnits					smallint  
    ,QSec					char(2)    
    ,Sec					char(2)    
    ,Twn					char(2)    
    ,Rng					char(2)   
    ,GeoArea				smallint  
    ,GeoNeighborhood		smallint    
    ,SpecArea				smallint  
    ,SuperGroup				varchar(50)		
    ,SpecNeighborhoodDesc	varchar(50)   
    ,SpecNeighborhood		smallint  
 --,AssignedYr smallint  
    ,AssignedAppr			varchar(4)		
    ,PIRollYr				smallint  
    ,PIInspectYr			smallint  
    --Duplicated for printout in Excel per condo team request   
    ,Folio					char(7)    
    ,ComplexName			varchar(80)   
    ,Major					char(6)  
    ,ResUnits				smallint  
    ,TotalUnits				smallint  
    ,QSTR					char(12)  
    ,AddrLine				nvarchar(max)  
    ,DistrictName			varchar(80)  --select max(len(DistrictName)) from District --16  
    ,ZipCode				varchar(10)  
    ,Comments				varchar(50)  
    ,TotalPcntOwnership		decimal(7,4)   
    ,TotalPcntLandVal		decimal(7,4)  
    ,SortOrder				int  
    ,DistanceFromPrior		decimal(18,9) 
	,CondoComplexId			uniqueidentifier
 )  
INSERT  #CondoList1   
SELECT  
		 pd.ptas_Major+pd.ptas_Minor			AS PIN
		,CASE
			WHEN pt.PropType = 'K' 
			THEN CONVERT(float, pd.ptas_major + '0000')
			ELSE CONVERT(float, pd.ptas_major + pd.ptas_minor)
		END	AS MapPin		
		,pd.ptas_parceldetailid
		,pd.ptas_landalternatekey				AS LndGuid
		,0 xCoord --xCoord decimal(18,9)  
		,0 yCoord--yCoord decimal(18,9)     
		,pd.ptas_applgroup  
		,pt.PropType
		,pd.ptas_folio							AS Folio
		,pd.ptas_propertyname					AS Complex
		,pd.ptas_major
		,yr.yearbuilt							AS YrBuilt  
		,yr.effyear								AS EffYr 
		,''  
		,'' 
		,COALESCE(dsm.value,'')					AS QuarterSection
		,pqstr.ptas_section						AS Section
		,pqstr.ptas_township					AS Township
		,pqstr.ptas_range						AS Range
		,CASE WHEN pt.PropType = 'R' THEN  COALESCE(pd.ptas_resarea,'')
						   WHEN pt.PropType = 'T' THEN  ( ISNULL((pd.ptas_resarea), (pd.ptas_commarea))) 
						   WHEN pt.PropType = 'K' AND COALESCE(pd.ptas_minor,'') = '0000' THEN  pd.ptas_commarea
						   ELSE pd.ptas_commarea
		 END									AS Area
		,CASE WHEN pt.PropType = 'R' THEN  COALESCE(pd.ptas_ressubarea,'')
						   WHEN pt.PropType = 'T' THEN  ( ISNULL((pd.ptas_ressubarea), (pd.ptas_commsubarea))) 
						   WHEN pt.PropType = 'K' AND COALESCE(pd.ptas_minor,'') = '0000' THEN pd.ptas_commsubarea
						   ELSE pd.ptas_commsubarea
		 END									AS SubArea
		,spec.ptas_areanumber					AS Area--SpecArea
		,COALESCE(sg.ptas_name, 'Unassigned')	AS SuperGroup
		, SpecNbr.ptas_description				AS AreaName--NeighborhoodDesc
		, SpecNbr.ptas_nbhdnumber				AS SubArea--NeighborhoodId
		, COALESCE(su.ptas_legacyid,'')			AS AssignedAppr
		, 0 --csa.PhysicalInspYr+1  
		, 0 --csa.PhysicalInspYr   
		, pd.ptas_folio							AS Folio
		,pd.ptas_propertyname					AS ComplexName  
		,pd.ptas_Major							AS Major  
		, ''									AS ResUnits  
		, ''									AS TotalUnits 
		, CONCAT(COALESCE(dsm.value,''),'/',pqstr.ptas_section,'/',pqstr.ptas_township,'/',pqstr.ptas_range) AS QSTR
		, ISNULL(pd.ptas_address,'Addr Unk') AS AddrLine
		, ISNULL(pd.ptas_district,'')			AS DistrictName 
		, ISNULL(pd.ptas_zipcode,'')			AS zipCode
		, ''									AS Comments  
		,0										AS TotalPcntOwnership  
		,0										AS TotalPcntLandVal  
		,0										AS SortOrder  
		,0										AS DistanceFromPrior 
		,cc.ptas_condocomplexid					AS CondoComplexId

FROM dynamics.ptas_parceldetail				AS pd 
JOIN dynamics.ptas_condocomplex				AS cc		ON pd.ptas_parceldetailid = cc._ptas_parcelid_value
JOIN	#PropType							AS pt	ON pt.ptas_propertytypeid = pd._ptas_propertytypeid_value
LEFT JOIN [dynamics].[ptas_qstr]			AS pqstr	ON pqstr.ptas_qstrid  = pd._ptas_qstrid_value
LEFT JOIN [dynamics].[stringmap]			AS dsm		ON dsm.attributevalue = pqstr.ptas_quartersection 
		AND dsm.objecttypecode = 'ptas_qstr'
		AND dsm.attributename  = 'ptas_quartersection'
LEFT JOIN [dynamics].[ptas_specialtyarea]	AS spec		ON spec.ptas_specialtyareaid = pd._ptas_specialtyareaid_value
LEFT JOIN dynamics.ptas_specialtyneighborhood SpecNbr	ON SpecNbr.ptas_specialtyneighborhoodid = pd._ptas_specialtynbhdid_value
LEFT JOIN [dynamics].[ptas_supergroup]		AS sg		ON sg.ptas_supergroupid = pd._ptas_supergroupdid_value
LEFT JOIN (Select  c._ptas_parcelid_value	AS _ptas_parcelid_value
				   ,y.ptas_name				AS yearbuilt
				   ,y2.ptas_name			AS effyear
			FRom dynamics.ptas_condocomplex c
			Join dynamics.ptas_year AS y  on c._ptas_yearbuiltid_value = y.ptas_yearid
			Join dynamics.ptas_year AS y2 on c._ptas_effectiveyearid_value = y2.ptas_yearid) 
											AS yr		ON yr._ptas_parcelid_value = pd.ptas_parceldetailid
LEFT JOIN [dynamics].[systemuser]			AS su		ON su.systemuserid = pd._ptas_parcelinspectedbyid_value
WHERE spec.ptas_areanumber = 700
  AND (pd.ptas_applgroup In ('K','M'))   
  AND COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = ''
ORDER BY SpecNbr.ptas_nbhdnumber
       , pd.ptas_Major  
  
UPDATE cl
SET cl.TotUnits = t.TotUnits
FROM #CondoList1 cl
JOIN (
Select ptas_major as major
	  ,COUNT(pd.ptas_parceldetailid)-1 as TotUnits --Mario Comment: -1 is a legacy calc
FRom dynamics.ptas_parceldetail pd
JOIN	#PropType								AS pt	ON pt.ptas_propertytypeid = pd._ptas_propertytypeid_value
Where pt.PropType='K' AND COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = ''
Group by ptas_major) as t on t.major = cl.major 

UPDATE cl
SET	  TotalUnits	   = tu.TotalUnits
     ,TotalPcntLandVal = tu.TotalPcntLandVal
	 ,TotalPcntOwnership = tu.TotalPcntOwnership
	 ,ResUnits			= tu.ResUnits
	 ,RUnits			= tu.ResUnits
	 ,cl.TotUnits		= tu.TotalUnits
FROM #CondoList1 cl
JOIN (
SELECT cl.CondoComplexId,cl.Major
     , Count(1) AS TotalUnits
	 , SUM(cu.ptas_percentlandvaluedecimal) as TotalPcntLandVal
	 , SUM(cu.ptas_percentownershipdecimal) AS TotalPcntOwnership
	 , SUM(CASE WHEN pt.PropType = 'K' AND pdcu.ptas_applgroup = 'K' THEN 1 ELSE 0 END) AS ResUnits
FROM dynamics.ptas_condounit			AS cu
Join #CondoList1						AS cl	ON cl.CondoComplexId = cu._ptas_complexid_value
JOIN dynamics.ptas_parceldetail			AS pdcu	ON pdcu.ptas_parceldetailid = cu._ptas_parcelid_value AND COALESCE(pdcu.ptas_splitcode,0) = 0 AND (pdcu.statecode  = 0 AND pdcu.statuscode = 1) AND COALESCE(pdcu.ptas_snapshottype,'') = ''
LEFT JOIN	#PropType					AS pt	ON pt.ptas_propertytypeid = pdcu._ptas_propertytypeid_value
GROUP BY cl.CondoComplexId,cl.Major ) AS tu ON tu.CondoComplexId = cl.CondoComplexId

/*

Mario COMMENT: The values need to calculated

UPDATE #CondoList1  
SET xCoord = p.xCoord  
   ,yCoord = p.yCoord  
FROM   
#CondoList1 c   
INNER JOIN ParcelInfo p ON p.Parcel = c.PIN  

*/

IF @IncludeRouting <> 'Y'  
BEGIN  
  UPDATE #CondoList1 SET SortOrder = RecId  
END 

IF @IncludeRouting = 'Y'  
BEGIN  
  
  DECLARE @StartPin char(10)  
  DECLARE @StartxCoord decimal(18,9)  
  DECLARE @StartyCoord decimal(18,9)  
  
 IF OBJECT_ID('tempdb..#CondoXYSorted')IS NOT NULL
 DROP TABLE tempdb..#CondoXYSorted

  CREATE TABLE #CondoXYSorted  
  (   
    RowNum int identity  
   ,PIN char(10)  
   ,xCoord decimal(18,9)  
   ,yCoord decimal(18,9)  
   ,SortOrder int  
   ,Distance1 decimal(18,9)  
   ,PinDistance1 char(10)  
  )     
  
  INSERT #CondoXYSorted (PIN ,xCoord ,yCoord, SortOrder, Distance1)  
  SELECT   
   PIN  
  ,c.xCoord   
  ,c.yCoord  
  ,99999  
  ,0  
  FROM   
  #CondoList1 c   
  ORDER BY c.yCoord desc, c.xCoord --puts NW-most record in row 1  
  
  DECLARE @RowNum int  
  SELECT @RowNum = 1  
    
  DECLARE @TotRows int  
  SELECT @TotRows = COUNT(*) FROM #CondoXYSorted  
    
  DECLARE @LoopNum int  
  SELECT @LoopNum = 1  
    
  
  UPDATE #CondoXYSorted  
  SET SortOrder = 1 --NW-most record   
  WHERE RowNum = 1  
  
  SELECT @StartPin = PIN  
        ,@StartxCoord = xCoord  
        ,@StartyCoord = yCoord  
  FROM #CondoXYSorted   
  WHERE RowNum = @RowNum  
    
  DECLARE @SortOrder int  
  SELECT @SortOrder = 2    
    
    
  WHILE @LoopNum < @TotRows  
  BEGIN  
  
    UPDATE #CondoXYSorted SET Distance1 = 0 WHERE SortOrder >= @SortOrder  
      
    UPDATE #CondoXYSorted  
    SET Distance1 = SQRT(SQUARE(xCoord-@StartxCoord) + SQUARE(yCoord-@StartyCoord))  
    WHERE SortOrder >= @SortOrder  
  
    UPDATE xy  
    SET SortOrder = @SortOrder  
    FROM #CondoXYSorted xy  
    WHERE Distance1 = (select min(Distance1) from #CondoXYSorted xy where SortOrder >= @SortOrder )  
    AND SortOrder >= @SortOrder  
      
    --select top 10 * from #CondoXYSorted order by SortOrder  
      
    SELECT @RowNum = RowNum FROM #CondoXYSorted WHERE SortOrder = @SortOrder  
      
    SELECT @SortOrder = @SortOrder + 1  
      
    SELECT @LoopNum = @LoopNum + 1   
      
    SELECT @StartPin = PIN  
          ,@StartxCoord = xCoord  
          ,@StartyCoord = yCoord  
    FROM #CondoXYSorted   
    WHERE RowNum = @RowNum  
    
  END --@RowNum < @TotRows  
  
  UPDATE #CondoList1   
  SET SortOrder = x.SortOrder  
     ,DistanceFromPrior = x.Distance1  
  FROM #CondoList1 cl  
  INNER JOIN #CondoXYSorted x ON x.PIN = cl.PIN  
  
  --select * from #CondoXYSorted order by RowNum  
  --select * from #CondoXYSorted order by SortOrder  
  --return(1)  
END --@IncludeRouting = 'Y'  

SELECT   
     ParcelId as RealPropId  
    ,LndGuid as LandId  
    ,ApplGroup    
    ,PropType    
    ,Fol    
    ,Complex   
    ,Maj  
    ,TotalPcntOwnership  
    ,TotalPcntLandVal  
    ,YrBuilt  
    ,EffYr  
    ,TotUnits    
    ,RUnits  
    ,QSec    
    ,Sec    
    ,Twn    
    ,Rng   
    ,GeoArea  
    ,GeoNeighborhood    
    ,SpecArea  
    ,SuperGroup					--Mario comment: looks like it is the correct columns but currently all rows are NULL, it is required to re-validate the column later
    ,SpecNeighborhoodDesc   
    ,SpecNeighborhood  
 --,AssignedYr  
    ,AssignedAppr				--Mario comment: We need to doublecheck the value.
    ,PIRollYr  
    ,PIInspectYr  
    ,Folio    
    ,ComplexName   
    ,Major  
    ,ResUnits  
    ,TotalUnits  
    ,QSTR  
    ,AddrLine  
    ,DistrictName  
    ,ZipCode  
    ,Comments  
    ,xCoord						--Mario comment: No column found yet
    ,yCoord						--Mario comment: No column found yet
    ,SortOrder  
    ,DistanceFromPrior  
    ,c.PIN  
    ,MapPin  
    ,RecId  
FROM #CondoList1 c  
WHERE ((SpecNeighborhood = @SpecNeighborhood AND @SpecNeighborhood<>'') OR @SpecNeighborhood = '')  
  AND ((SuperGroup = @SuperGroup AND @SuperGroup <> '') OR @SuperGroup = '')     
ORDER BY SortOrder  

RETURN (0)  
  
END  
GO
