IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_Bookmarks')
	DROP PROCEDURE [cus].[SRCH_R_Bookmarks]  
GO
CREATE PROCEDURE [cus].[SRCH_R_Bookmarks]   
   @ApplDistrict	varchar(3)  
  ,@ResArea			varchar(3)   
  ,@ResSubArea		varchar(3)   
  ,@ResNbhd			varchar(3)     
  ,@Major			char(10)  
  ,@AssignedAppr	varchar(30)  
  ,@ActiveBookmarkOnlyYN varchar(3) --Y or N  
  
AS 
/*
Author: Mario Umaña
Date Created:  Dec 14 2020
Description:   Search for Bookmarked parcels.

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
BEGIN  
SET NOCOUNT ON  
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED  

DROP TABLE IF EXISTS #PropType;

CREATE TABLE #PropType
(
	 ptas_propertyTypeId UNIQUEIDENTIFIER PRIMARY KEY
	,PropType NVARCHAR(1)
)

INSERT INTO #PropType
SELECT  ptas_propertyTypeId
       ,SUBSTRING(pt.ptas_name,1,1) 
FROM dynamics.ptas_propertytype AS pt  
  
--ADDED ISNULL TO PARTS OF RESULT SET TO GET RID OF ANY NULLS.  
--FOR AN UPDATE QUERY, THE NULLS CAUSE DBNULL ERROR IN THE SPREAD.  
DECLARE @Error Int  
  
  
IF ISNULL(@ApplDistrict,'')	 = '' 	SELECT @ApplDistrict = ''  
IF ISNULL(@ResArea,'') = '' 		SELECT @ResArea = ''  
IF ISNULL(@ResSubArea,'')				= '' OR ISNULL(@ResSubArea,'') = '0' SELECT @ResSubArea = ''  
IF ISNULL(@Major,'') = '' 			SELECT @Major = ''  
IF ISNULL(@AssignedAppr,'') = '' 	SELECT @AssignedAppr = ''  
IF ISNULL(@ResNbhd,'') = '' 		SELECT @ResNbhd = '' --12/28/2015  
IF ISNULL(@ActiveBookmarkOnlyYN,'') = '' SELECT @ActiveBookmarkOnlyYN = 'Y'  
IF ISNULL(@ActiveBookmarkOnlyYN,'Yes') = '' SELECT @ActiveBookmarkOnlyYN = 'Y'  
IF ISNULL(@ActiveBookmarkOnlyYN,'No') = '' SELECT @ActiveBookmarkOnlyYN = 'N'  
  
 
 /*

****  Mario comment : This validation wont be required since the Appraisal will be validated in the UI.
 
IF @AssignedAppr <> ''  
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
  
  
  
DECLARE @ApplDistrictFullName  varchar(20) = ''  
IF @ApplDistrict <> ''  
BEGIN  
  SELECT   
  @ApplDistrictFullName = CASE   
                            WHEN @ApplDistrict = 'NE' THEN 'Northeast'  
                            WHEN @ApplDistrict = 'NW' THEN 'Northwest'  
                            WHEN @ApplDistrict = 'SE' THEN 'Southeast'  
                            WHEN @ApplDistrict = 'SW' THEN 'Southwest'  
                            WHEN @ApplDistrict = 'WC' THEN 'West Central '  
                          END  
END  
  
--select @ApplDistrict as ApplDistrict, @ApplDistrictFullName as ApplDistrictFullName, @SpecApplDistrictFullName as SpecApplDistrictFullName  --debug   
  
  
--Don't use @ResArea is @ResSubArea is missing  
IF @ResArea = '' AND @ResSubArea <> '' SELECT @ResSubArea = ''  
  
--TODO this is also declared below in commnented out part  
DECLARE @RPAssmtYr int   --debug to run ad hoc  
SELECT @RPAssmtYr = 2020 --RpAssmtYr FROM AssmtYr    
  
--select * from Bookmark  
--select LUItemId, LUItemShortDesc from luitem2 where lutypeid=371   
  --LUItemId LUItemShortDesc  
  ---------- --------------------------------------------------  
  --0          
  --10       Completed  
  
--Final Result  
SELECT 
  CASE
    WHEN pd.ptas_proptype = 'K' 
	THEN CONVERT(float, pd.ptas_major + '0000')
    ELSE CONVERT(float, pd.ptas_major + pd.ptas_minor)
  END					AS Parcel
,pd.ptas_major			AS Major
,pd.ptas_minor			AS Minor
,CASE WHEN ats.AssignmentTypeItemId = 3 THEN ats.ptas_legacyid ELSE '' END	AS AssignedBoth	
,bt.ptas_name			AS BookMarkType
,bm.ptas_bookmarknote	AS Descr
,''						AS BookMarkStatus									--Mario Comment: There is not such value in none of the 3 bookmark tables
,COALESCE(su.ptas_legacyid,'')						AS BookMarkUpdatedBy	
,bm.modifiedon										AS UpdateDate
,COALESCE(pd.ptas_address,'')						AS AddrLine
,pd.ptas_district									AS DistrictName
,COALESCE(ta.ptas_taxpayername,'')					AS TaxPayerName
,psm.value											AS ApplDistrict	
,RIGHT('000'+ISNULL(CAST(CASE WHEN pd.ptas_proptype = 'R' THEN  COALESCE(pd.ptas_resarea,'')
						   WHEN pd.ptas_proptype = 'T' THEN  ( ISNULL((pd.ptas_resarea), (pd.ptas_commarea))) 
						   WHEN pd.ptas_proptype = 'K' AND COALESCE(pd.ptas_minor,'') = '0000' THEN  pd.ptas_commarea
						   ELSE pd.ptas_commarea
		 END AS NVARCHAR(3)),''),3)									AS ResArea
,RIGHT('000'+ISNULL(CAST(CASE WHEN pd.ptas_proptype = 'R' THEN  COALESCE(pd.ptas_ressubarea,'')
						   WHEN pd.ptas_proptype = 'T' THEN  ( ISNULL((pd.ptas_ressubarea), (pd.ptas_commsubarea))) 
						   WHEN pd.ptas_proptype = 'K' AND COALESCE(pd.ptas_minor,'') = '0000' THEN pd.ptas_commsubarea
						   ELSE pd.ptas_commsubarea
		 END AS NVARCHAR(3)),''),3)				AS SubArea
,pd.ptas_neighborhood							AS ResNbhd
,pd.ptas_proptype								AS PropType
,pd.ptas_applgroup								AS ApplGroup
,pd.ptas_Major+pd.ptas_Minor					AS PIN
,ROW_NUMBER() OVER (ORDER BY (SELECT 1))		AS RecId /*replace the identity of the temp table in previous version*/
,CASE
	WHEN pt.PropType = 'K' 
	THEN CONVERT(float, pd.ptas_major + '0000')
	ELSE CONVERT(float, pd.ptas_major + pd.ptas_minor)
END												AS MapPin		
FROM dynamics.ptas_parceldetail							AS pd 
JOIN dynamics.ptas_bookmark								AS bm	ON pd.ptas_parceldetailid = bm._ptas_parceldetailid_value
JOIN dynamics.ptas_ptas_bookmark_ptas_bookmarktag		AS bmt	ON bmt.ptas_bookmarkid = bm.ptas_bookmarkid
JOIN		#PropType									AS pt	ON pt.ptas_propertytypeid = pd._ptas_propertytypeid_value
JOIN dynamics.ptas_bookmarktag							AS bt	ON bt.ptas_bookmarktagid = bmt.ptas_bookmarktagid
JOIN dynamics.stringmap									AS psm	ON  pd.ptas_residentialdistrict = psm.attributevalue AND psm.objecttypecode = 'ptas_parceldetail' AND psm.attributename = 'ptas_residentialdistrict'
LEFT JOIN [dynamics].[systemuser]						AS su	ON su.systemuserid = bm._modifiedby_value
LEFT JOIN dynamics.ptas_taxaccount						AS ta	ON ta.ptas_taxaccountid = pd._ptas_taxaccountid_value
LEFT JOIN dynamics.vw_AssignedTypes						AS ats  ON ats.ParcelId = pd.ptas_parceldetailid  AND ats.AssmtYr = 2020   
Where pd.ptas_applgroup <> 'A'
	AND ((psm.value				=  @ApplDistrictFullName	AND @ApplDistrictFullName <> '') OR @ApplDistrictFullName = '') 
	AND ((pd.ptas_resarea		= @ResArea					AND @ResArea <>'')				 OR @ResArea = '') 
	AND ((pd.ptas_ressubarea	= @ResSubArea				AND @ResSubArea <>'')			 OR @ResSubArea = '')
	AND ((pd.ptas_neighborhood	= @ResNbhd					AND @ResNbhd <>'')				 OR @ResNbhd = '')
	AND ((pd.ptas_major			= @Major					AND @Major <>'' )				 OR @Major = '')                  
	AND ((ats.ptas_legacyid = @AssignedAppr AND @AssignedAppr <>'' ) OR @AssignedAppr = '')   
	AND (( bm.statecode = 0 AND @ActiveBookmarkOnlyYN = 'Y' ) OR @ActiveBookmarkOnlyYN = 'N')  --Mario comment: The value of bm.statecode needs to be validated
	AND COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = ''
ORDER 
by	 psm.value	
	,pd.ptas_resarea
	,pd.ptas_ressubarea
	,PIN


RETURN(0)  
  
ErrorHandler:  
RETURN (@Error)  
  
  
RETURN(@@ERROR)  
END  
   GO 

