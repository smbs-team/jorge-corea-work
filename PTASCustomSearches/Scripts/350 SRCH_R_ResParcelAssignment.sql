/*
Hairo comment: Select to get the user by TEAM
select a.fullname, c.name
from [dynamics].[systemuser] A
INNER JOIN [dynamics].[teammembership] B
ON A.[systemuserid] = B.[systemuserid]
INNER JOIN [dynamics].[team] C
ON c.teamid = B.teamid
*/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_ResParcelAssignment')
	DROP PROCEDURE [cus].[SRCH_R_ResParcelAssignment]   
GO
CREATE PROCEDURE [cus].[SRCH_R_ResParcelAssignment]   
   @AssmtYr int   
  ,@ResArea varchar(3)   
  ,@ResSubArea varchar(3)   
  ,@Major char(6) = ''     
  ,@AssignedAppr varchar(30)  --Hairo comment: It is going to recive the Guid column "systemuserid" from [dynamics].[systemuser]
  
--TODO add ResTeam   select distinct Team from GisMapData --need to change how it is captured into GisMapData  
 

  
AS BEGIN
/*Hairo comment: aqui debe haber un slash asterisco
Author: Jairo Barquero
Date Created:  01/26/2020
Description:    SP that search for Residencial Parcel Assignment.

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
Hairo comment: aqui debe haber un slash asterisco
*/

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

/*
--BEGIN ENABLE THIS FOR TESTING PORPUSES
declare 
   @AssmtYr int   
  ,@ResArea varchar(3)   
  ,@ResSubArea varchar(3)   
  ,@Major char(6) = ''     
  ,@AssignedAppr varchar(36)  

SELECT
   @AssmtYr = 2020
  ,@ResArea = '80'
  ,@ResSubArea =NULL
  ,@Major =NULL
  ,@AssignedAppr =NULL --'E921F2E1-1F0B-460E-AACA-4FA374DF42DD'
--7B398956-D7F8-4623-941A-169397FBF9AE
--243F959D-1DD7-4A1A-A76B-4E92CE49FD20
--E921F2E1-1F0B-460E-AACA-4FA374DF42DD

--END ENABLE THIS FOR TESTING PORPUSES
*/
DROP TABLE IF EXISTS #PropType;
DROP TABLE IF EXISTS #RealProp;
DROP TABLE IF EXISTS #AssignedAppr;
DROP TABLE IF EXISTS #Permits; 
DROP TABLE IF EXISTS #ToDoList;
DROP TABLE IF EXISTS #Media;
DROP TABLE IF EXISTS #AppraiserAssignedPclCount;
DROP TABLE IF EXISTS #InspectedDates;

CREATE TABLE #PropType
(
ptas_propertyTypeId UNIQUEIDENTIFIER PRIMARY KEY ,
PropType NVARCHAR(1)
)
INSERT INTO #PropType
SELECT  ptas_propertyTypeId       
		,SUBSTRING(pt.ptas_name,1,1) 
FROM dynamics.ptas_propertytype AS pt
  
--ADDED ISNULL TO RESULT SET TO GET RID OF ANY NULLS.  FOR AN UPDATE QUERY, THE NULLS CAUSE DBNULL ERROR IN THE SPREAD.  
DECLARE @Error Int  
  
  
  
IF ISNULL(@ResArea,'') = '' SELECT @ResArea = ''  
IF ISNULL(@ResSubArea,'') = '' OR ISNULL(@ResSubArea,'') = '0' SELECT @ResSubArea = ''  
IF ISNULL(@AssignedAppr,'') = '' SELECT @AssignedAppr = ''  
IF ISNULL(@Major,'') = '' OR ISNULL(@Major,'') = '0' SELECT @Major = ''  
  
 
  
  
CREATE TABLE #RealProp   
    (  
      RecId int Identity(1,1)   
    , PIN char(10)  
    , MapPin float  
    , ParcelId uniqueidentifier  
    , LandId uniqueidentifier  
    , QuarterSection char(2)  
    , Section tinyint  
    , Township tinyint  
    , [Range] tinyint  
    , Folio varchar(7)  
    , AssignedAppr nvarchar(100)--5/18/2017 DSAX
    , AssmtEntityId uniqueidentifier--char(4)
        )  

  
CREATE TABLE #AssignedAppr   
    (  
      ParcelId uniqueidentifier  
        )  
  

--Revised so assignments can occur before the rollover for the upcoming AssmtYr         
IF @AssignedAppr <> ''  
BEGIN  
  
    INSERT #AssignedAppr  
    SELECT DISTINCT RpGuid   
    FROM rp.ParcelAssignment pa  
    WHERE AssmtYr = @AssmtYr  
    AND AssignmentTypeId = 326 
    AND AssignmentTypeItemId = 3  --BOTH
    AND AssignedToGuid = @AssignedAppr

  
    INSERT #RealProp   
    (
      PIN 
    , MapPin   
    , ParcelId   
    , LandId   
    , QuarterSection   
    , Section   
    , Township   
    , [Range]   
    , Folio   
    , AssignedAppr 
    )
    SELECT 
     COALESCE(pd.ptas_major,'') + COALESCE(pd.ptas_minor,'')
    ,CONVERT(float,(COALESCE(pd.ptas_major,'') + COALESCE(pd.ptas_minor,'')))
    ,pd.ptas_parceldetailid 
    ,dpl.ptas_landid 
	,QuarterSection = COALESCE(dsm.value,'')
	,Section		  = pqstr.ptas_section	
	,Township		  = pqstr.ptas_township
	,[Range]		  = pqstr.ptas_range	
    ,pd.ptas_folio
    ,''
    FROM [dynamics].[ptas_parceldetail] pd  
   INNER JOIN dynamics.ptas_land (NOLOCK) dpl ON pd._ptas_landid_value = dpl.ptas_landid
   INNER JOIN #AssignedAppr a ON a.ParcelId = pd.ptas_parceldetailid
   INNER JOIN #PropType pt ON pd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
	LEFT JOIN [dynamics].[ptas_qstr] pqstr ON pqstr.[ptas_qstrid] = pd._ptas_qstrid_value
    LEFT JOIN [dynamics].[stringmap]	dsm ON dsm.attributevalue = pqstr.ptas_quartersection 
										   AND dsm.objecttypecode = 'ptas_qstr'
										   AND dsm.attributename  = 'ptas_quartersection'	
	LEFT JOIN dynamics.ptas_neighborhood (NOLOCK) dpn ON dpn.ptas_neighborhoodid = pd._ptas_neighborhoodid_value
    WHERE  pt.PropType = 'R'  
    AND ((pd.ptas_resarea    = @ResArea    AND @ResArea    <>'')  OR @ResArea    = '')   
    AND ((pd.ptas_ressubarea = @ResSubArea AND @ResSubArea <>'')  OR @ResSubArea = '')  
    AND ((pd.ptas_major      = @Major      AND @Major      <>'')  OR @Major      = '')               
    ORDER BY  
     pd.ptas_resarea  
    ,pd.ptas_ressubarea   
    ,dpn.ptas_name  
    ,pd.ptas_major  
    ,pd.ptas_minor  
END  
  

  
  
IF @AssignedAppr = ''  
BEGIN  

     INSERT #RealProp   
     (
       PIN 
     , MapPin   
     , ParcelId   
     , LandId   
     , QuarterSection   
     , Section   
     , Township   
     , [Range]   
     , Folio   
     , AssignedAppr 
     )
    SELECT 
     COALESCE(pd.ptas_major,'') + COALESCE(pd.ptas_minor,'')
    ,CONVERT(float,(COALESCE(pd.ptas_major,'') + COALESCE(pd.ptas_minor,'')))
    ,pd.ptas_parceldetailid 
    ,dpl.ptas_landid 
	,QuarterSection = COALESCE(dsm.value,'')
	,Section		  = pqstr.ptas_section	
	,Township		  = pqstr.ptas_township
	,[Range]		  = pqstr.ptas_range	
    ,pd.ptas_folio
    ,''
    FROM [dynamics].[ptas_parceldetail] pd  
   INNER JOIN dynamics.ptas_land (NOLOCK) dpl ON pd._ptas_landid_value = dpl.ptas_landid
   INNER JOIN #PropType pt ON pd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
	LEFT JOIN [dynamics].[ptas_qstr] pqstr ON pqstr.[ptas_qstrid] = pd._ptas_qstrid_value
    LEFT JOIN [dynamics].[stringmap]	dsm ON dsm.attributevalue = pqstr.ptas_quartersection 
										   AND dsm.objecttypecode = 'ptas_qstr'
										   AND dsm.attributename  = 'ptas_quartersection'	
	LEFT JOIN dynamics.ptas_neighborhood (NOLOCK) dpn ON dpn.ptas_neighborhoodid = pd._ptas_neighborhoodid_value
    WHERE  pt.PropType = 'R'  
    AND ((pd.ptas_resarea    = @ResArea    AND @ResArea    <>'')  OR @ResArea    = '')   
    AND ((pd.ptas_ressubarea = @ResSubArea AND @ResSubArea <>'')  OR @ResSubArea = '')  
    AND ((pd.ptas_major      = @Major      AND @Major      <>'')  OR @Major      = '')               
    ORDER BY  
     pd.ptas_resarea  
    ,pd.ptas_ressubarea   
    ,dpn.ptas_name  
    ,pd.ptas_major  
    ,pd.ptas_minor  
 END   
   

      
--added 6/6/2012 Don G      
CREATE TABLE #Permits 
(
	ParcelId uniqueidentifier, 
	MaxPermitVal int, 
	MinPermitDate smalldatetime,
	MaxPermitDate SMALLDATETIME
)  
INSERT #Permits  
SELECT rp.ParcelId,MAX(p.ptas_permitvalue),MIN(p.ptas_issueddate),MAX(p.ptas_issueddate)  
FROM #RealProp rp 
INNER JOIN [dynamics].[ptas_permit] p ON rp.ParcelId = p._ptas_parcelid_value  
WHERE p.statuscode IN(591500004,1)--Revisit,Unsubmitted
 AND p.ptas_issueddate >= '1/1/' + convert(char(4), DATEPART(year, getdate()) - 10)  
GROUP BY rp.ParcelId
      
	
   
  
--CREATE TODO LIST (Y/Blank) 1/3/2012 Don G  
  
CREATE TABLE #ToDoList(  
  ParcelId uniqueidentifier  
 ,LandId uniqueidentifier  
 ,AcctNbr char(10)  
 ,ResArea char(3)  
 ,LandInsp char(1)           DEFAULT ''  
 ,ImpsInsp char(1)           DEFAULT ''  
 ,GenlBookmark char(1)       DEFAULT ''  
 ,RevisitBookmark char(1)    DEFAULT ''  
 ,Permit char(1)             DEFAULT ''  
 ,SegMerg char(1)            DEFAULT ''  
 ,Unkill char(1)             DEFAULT ''  
 ,NewParcel char(1)          DEFAULT ''  
 ,Transf char(1)             DEFAULT ''  
 ,AR char(1)                 DEFAULT ''  
 ,CR char(1)                 DEFAULT ''  
 ,Destruct char(1)           DEFAULT ''  
 ,PI_BLV char(1)             DEFAULT ''  
 ,LandVal char(1)            DEFAULT ''  
 ,TotalVal char(1)           DEFAULT ''  
 )  
   
--SELECT * FROM #ToDoList  

INSERT #ToDoList (ParcelId,LandId)   
SELECT ParcelId,LandId FROM #RealProp  
  
UPDATE #ToDoList   
SET AcctNbr = pd.ptas_Major + pd.ptas_Minor  
    ,ResArea = rap.ptas_resarea
FROM #ToDoList tdl INNER JOIN dynamics.ptas_parceldetail pd ON tdl.ParcelId = pd.ptas_parceldetailid  
                   INNER JOIN dynamics.vw_ResAreaPcl rap ON pd.ptas_parceldetailid = rap.ptas_parceldetailid
  
--LandInsp  
UPDATE #ToDoList  
SET LandInsp = 'Y'  
FROM #ToDoList tdl  
WHERE NOT EXISTS (SELECT 1 FROM [ptas].[ptas_inspectionhistory] ih   
					WHERE tdl.ParcelId = ih.ptas_parcelid   
					AND ih.ptas_inspectiondate > '1/1/1900'  
					AND ih.ptas_inspectiontype in ('Land','Both')  
					AND SUBSTRING(ih.ptas_Name,1,4) = 2018)  
  
--ImpsInsp  
UPDATE #ToDoList  
SET ImpsInsp = 'Y'  
FROM #ToDoList tdl  
WHERE NOT EXISTS (SELECT 1 FROM [ptas].[ptas_inspectionhistory] ih   
					WHERE tdl.ParcelId = ih.ptas_parcelid   
					AND ih.ptas_inspectiondate > '1/1/1900'  
					AND ih.ptas_inspectiontype in ('Imps','Both')  
					AND SUBSTRING(ih.ptas_Name,1,4) = 2018)  
--GenlBookmark  
UPDATE #ToDoList  
SET GenlBookmark = 'Y'  
FROM #ToDoList tdl  
WHERE EXISTS ( SELECT 1
				 FROM dynamics.ptas_bookmark	AS bm						
				INNER JOIN dynamics.ptas_ptas_bookmark_ptas_bookmarktag	AS bmt	ON bmt.ptas_bookmarkid = bm.ptas_bookmarkid
				INNER JOIN dynamics.ptas_bookmarktag					AS bt	ON bt.ptas_bookmarktagid = bmt.ptas_bookmarktagid
				WHERE bt.ptas_name ='General'
				  AND bm._ptas_parceldetailid_value = tdl.ParcelId)


--RevisitBookmark  
UPDATE #ToDoList  
SET RevisitBookmark = 'Y'  
FROM #ToDoList tdl  
WHERE EXISTS ( SELECT 1
				 FROM dynamics.ptas_bookmark	AS bm						
				INNER JOIN dynamics.ptas_ptas_bookmark_ptas_bookmarktag	AS bmt	ON bmt.ptas_bookmarkid = bm.ptas_bookmarkid
				INNER JOIN dynamics.ptas_bookmarktag					AS bt	ON bt.ptas_bookmarktagid = bmt.ptas_bookmarktagid
				WHERE bt.ptas_name ='Revisit'
				  AND bm._ptas_parceldetailid_value = tdl.ParcelId)
--Permit  
UPDATE #ToDoList  
SET Permit = 'Y'  
FROM #ToDoList tdl  
WHERE EXISTS (SELECT 1
				FROM [dynamics].[ptas_permit] p 
				WHERE tdl.ParcelId = p._ptas_parcelid_value  
				  AND p.statuscode = 1 --Unsubmitted
			  )
  
/*Hairo comment: EventType and PropStatus required to do this calculations.
				 ReviewTracking table not available
  
--SegMerg  
UPDATE #ToDoList  
SET SegMerg = 'Y'  
FROM #ToDoList tdl INNER JOIN ChngHist ch ON tdl.RpGuid = ch.RpGuid  
WHERE ch.EventTypeItemId = 4 AND ch.PropStatus = ''  
  
--Unkill  
UPDATE #ToDoList  
SET Unkill = 'Y'  
FROM #ToDoList tdl INNER JOIN ChngHist ch ON tdl.RpGuid = ch.RpGuid  
WHERE ch.EventTypeItemId in (8,9) AND ch.PropStatus = ''  
  
--NewParcel  
UPDATE #ToDoList  
SET NewParcel = 'Y'  
FROM #ToDoList tdl INNER JOIN ChngHist ch ON tdl.RpGuid = ch.RpGuid  
WHERE ch.EventTypeItemId = 10 AND ch.PropStatus = ''  
  
--Transf  
UPDATE #ToDoList  
SET Transf = 'Y'  
FROM #ToDoList tdl INNER JOIN ChngHist ch ON tdl.RpGuid = ch.RpGuid  
WHERE ch.EventTypeItemId = 11 AND ch.PropStatus = ''  


--AR  
UPDATE #ToDoList  
SET AR = 'Y'  
FROM #ToDoList tdl INNER JOIN ReviewTracking rt ON tdl.AcctNbr = rt.AcctNbr  
WHERE rt.ReviewTypeItemId = 4 AND rt.StatusAssmtReviewItemId <> 9  
  
--CR  
UPDATE #ToDoList  
SET CR = 'Y'  
FROM #ToDoList tdl INNER JOIN ReviewTracking rt ON tdl.AcctNbr = rt.AcctNbr  
WHERE rt.ReviewTypeItemId = 5 AND rt.StatusAssmtReviewItemId <> 9  
  
--Destruct  
UPDATE #ToDoList  
SET Destruct = 'Y'  
FROM #ToDoList tdl INNER JOIN ReviewTracking rt ON tdl.AcctNbr = rt.AcctNbr  
WHERE rt.ReviewTypeItemId = 6 AND rt.StatusAssmtReviewItemId <> 9  

*/
 
--LandVal  
UPDATE #ToDoList  
SET LandVal = 'Y'
FROM #ToDoList tdl INNER JOIN dynamics.ptas_Land l ON tdl.LandId = l.ptas_landid
                   INNER JOIN [dynamics].[ptas_inspectionyear] iy
							  INNER JOIN [dynamics].[ptas_area] pa ON iy._ptas_area_value = pa.ptas_areaid 
				   ON tdl.ResArea = pa.ptas_areanumber				   
WHERE iy.ptas_year = @AssmtYr  
  AND iy.ptas_inspectiontype = 1 --Physical inspection
  AND l.ptas_taxyear <> @AssmtYr + 1  

  
--TotalVal  
UPDATE #ToDoList  
SET TotalVal = 'Y'  
FROM #ToDoList tdl   
WHERE NOT EXISTS (SELECT 1 
					FROM [ptas].[ptas_appraisalhistory] ah
					WHERE ah.ptas_parcelid = tdl.ParcelId
					AND ah.ptas_name = @AssmtYr + 1
				  )

--END TODO LIST  
  
----MEDIA  
----Getting a pic count is more involved using the new media tables (compared to using ImageParcel)  
----Getting the most recent pic date is even more involved  
----So use temp table  
--select LUItemId , LUItemShortDesc from LUItem2 where LUTypeId = 309 -- MediaTypeId,     
  
       --LUItemId LUItemShortDesc  
       ---------- --------------------------------------------------  
       --1        Image  
       --2        Video  
       --3        Audio  
       --4        Documentation  
 --so MediaTypeItemId is always 1 at this time  
   
  
   
 --select LUItemId , LUItemShortDesc from LUItem2 where LUTypeId = 301  --ImageTypeItemId   
     -- LUItemId LUItemShortDesc  
     ---------- --------------------------------------------------  
     --1        Pict  
     --2        Fplan  
     --3        EPlan  
--So ImageTypeItemId is always 1  
  
 --select LUItemId , LUItemShortDesc from LUItem2 where LUTypeId = 313  --MediaUsageTypeId   
  
      --LUItemId LUItemShortDesc  
      ---------- --------------------------------------------------  
      --1        Land  
      --2        Bldg  
      --3        Accy  
      --4        Mobile  
      --5        HistoryP  
      --6        Note  
      --7        Float  
  
--so MediaUsageTypeId varies  
  
  
CREATE TABLE #Media  
(   
  ParcelId   uniqueidentifier  
 --,MediaId int  
 ,MediaUsage varchar(20) --select * from luitem2 where lutypeid = 313  
 ,MediaDate smalldatetime  
)  
  
INSERT #Media  
select ad._ptas_parceldetailid_value,'Accy',mr.createdon
from [dynamics].[ptas_accessorydetail] ad
INNER JOIN #RealProp r ON ad._ptas_parceldetailid_value = r.ParcelId  
INNER JOIN [dynamics].[ptas_accessorydetail_ptas_mediarepository] admr ON admr.ptas_accessorydetailid =ad.ptas_accessorydetailid
INNER JOIN [dynamics].[ptas_mediarepository] mr ON mr.ptas_mediarepositoryid = admr.ptas_mediarepositoryid
WHERE mr.ptas_imagetype = 1
AND (mr.statecode = 0 AND mr.statuscode = 1)
              
  
INSERT #Media  
select r.ParcelId,'Land',mr.createdon
from [dynamics].[ptas_land] l
INNER JOIN #RealProp r ON l.ptas_landid = r.LandId
INNER JOIN [dynamics].[ptas_land_ptas_mediarepository] lmr ON lmr.ptas_landid =l.ptas_landid
INNER JOIN [dynamics].[ptas_mediarepository] mr ON mr.ptas_mediarepositoryid = lmr.ptas_mediarepositoryid
WHERE mr.ptas_imagetype = 1
AND (mr.statecode = 0 AND mr.statuscode = 1)

  
INSERT #Media  
select ad._ptas_parceldetailid_value,'Bldg',mr.createdon
from [dynamics].[ptas_buildingdetail] ad
INNER JOIN #RealProp r ON ad._ptas_parceldetailid_value = r.ParcelId  
INNER JOIN [dynamics].[ptas_buildingdetail_ptas_mediarepository] bdmr ON bdmr.ptas_buildingdetailid =ad.ptas_buildingdetailid
INNER JOIN [dynamics].[ptas_mediarepository] mr ON mr.ptas_mediarepositoryid = bdmr.ptas_mediarepositoryid
WHERE mr.ptas_imagetype = 1
AND (mr.statecode = 0 AND mr.statuscode = 1)
  
/*
Hairo comment: need to identify MH in order to get this information, it is
			   related to ptas_condounit_ptas_mediarepository table.
INSERT #Media  
SELECT mh.RpGuid, mc.MediaId, 'MH', mam.MediaDate  
FROM MHAcctMedia mam INNER JOIN Media_cmpt mc ON mam.MHMedGuid = mc.TblRecGuid  
                     INNER JOIN MHAcct mh ON mam.MhGuid = mh.MhGuid   
                     INNER JOIN #RealProp r ON mh.RpGuid = r.RpGuid  
WHERE mam.ImageTypeItemId = 1 AND mam.ActiveFlag = 'Y'  

INSERT #Media  
SELECT mh.RpGuid, mc.MediaId, 'MHPlanPic', mam.MediaDate  
FROM MHAcctMedia mam INNER JOIN Media_cmpt mc ON mam.MHMedGuid = mc.TblRecGuid  
                     INNER JOIN MHAcct mh ON mam.MhGuid = mh.MhGuid  
                     INNER JOIN #RealProp r ON mh.RpGuid = r.RpGuid   
WHERE mam.ImageTypeItemId = 2 AND mam.FileExtension <> 'WMF' AND mam.ActiveFlag = 'Y'  
  
  
INSERT #Media  
SELECT mh.RpGuid, mc.MediaId, 'MHPlanWMF', mam.MediaDate  
FROM MHAcctMedia mam INNER JOIN Media_cmpt mc ON mam.MHMedGuid = mc.TblRecGuid  
                     INNER JOIN MHAcct mh ON mam.MhGuid = mh.MhGuid  
                     INNER JOIN #RealProp r ON mh.RpGuid = r.RpGuid   
WHERE mam.ImageTypeItemId = 2 AND mam.FileExtension = 'WMF' AND mam.ActiveFlag = 'Y'  

*/
--Plans (photo)  
INSERT #Media  
select ad._ptas_parceldetailid_value,'BldgPlanPic',mr.createdon
from [dynamics].[ptas_buildingdetail] ad
INNER JOIN #RealProp r ON ad._ptas_parceldetailid_value = r.ParcelId  
INNER JOIN [dynamics].[ptas_buildingdetail_ptas_mediarepository] bdmr ON bdmr.ptas_buildingdetailid =ad.ptas_buildingdetailid
INNER JOIN [dynamics].[ptas_mediarepository] mr ON mr.ptas_mediarepositoryid = bdmr.ptas_mediarepositoryid
WHERE mr.ptas_imagetype = 2
AND (mr.statecode = 0 AND mr.statuscode = 1)
AND mr.ptas_fileextension <> 'WMF'
  
--Plans (wmf)  
INSERT #Media  
select ad._ptas_parceldetailid_value,'BldgPlanWMF',mr.createdon
from [dynamics].[ptas_buildingdetail] ad
INNER JOIN #RealProp r ON ad._ptas_parceldetailid_value = r.ParcelId  
INNER JOIN [dynamics].[ptas_buildingdetail_ptas_mediarepository] bdmr ON bdmr.ptas_buildingdetailid =ad.ptas_buildingdetailid
INNER JOIN [dynamics].[ptas_mediarepository] mr ON mr.ptas_mediarepositoryid = bdmr.ptas_mediarepositoryid
WHERE mr.ptas_imagetype = 2
AND (mr.statecode = 0 AND mr.statuscode = 1)
AND mr.ptas_fileextension = 'WMF'
 


--LEFT OFF
--5/18/2017 DSAX 
--select * from LuItem2 where LuTypeId = 325
UPDATE #RealProp
SET AssignedAppr = lastname+' '+ firstname +': '+COALESCE(ptas_legacyid,'')
   ,AssmtEntityId = su.systemuserid --COALESCE(ptas_legacyid,'') 
FROM #RealProp rp
INNER JOIN rp.ParcelAssignment pclas ON pclas.RpGuid = rp.ParcelId AND pclas.AssmtYr = @AssmtYr AND pclas.AssignmentTypeItemId = 3
INNER JOIN [dynamics].[systemuser] su ON su.systemuserid = pclas.AssignedToGuid


----5/18/2017 hardcode following so new assignment can be based on old assignment  
----select LastName, FirstName from AssmtEntity where AssmtEntityId = 'DIBR' 
----select LastName, FirstName, AssmtEntityId from AssmtEntity where FirstName = 'Diana' --Ajemian Diana  DAJE
IF (select ptas_legacyid from [dynamics].[systemuser] WHERE systemuserid = @AssignedAppr)='DIBR' AND @AssmtYr <= 2016  UPDATE #RealProp SET AssignedAppr = 'Ajemian, Diana: DAJE'

          
--select  @AssignedAppr
--return(1) 
  

CREATE TABLE #AppraiserAssignedPclCount (AssignedBoth uniqueidentifier, counts int)  
INSERT #AppraiserAssignedPclCount  
	SELECT ae.systemuserid, count(*)   
	FROM rp.ParcelAssignment pa 
	INNER JOIN [dynamics].[systemuser] ae ON pa.AssignedToGuid = ae.systemuserid  
	WHERE AssmtYr = @AssmtYr  
	AND pa.AssignmentTypeItemId = 3
	GROUP BY ae.systemuserid  
	ORDER BY ae.systemuserid  
  
----debug
--select * from #AppraiserAssignedPclCount
--return(0)


CREATE TABLE #InspectedDates
(
  ParcelId uniqueidentifier,
  LandInspectedDate datetime,
  ImpsInspectedDate datetime,
  BothInspectedDate  datetime
)

INSERT INTO #InspectedDates
SELECT rp.ParcelId
		,CASE WHEN  ih.ptas_inspectiontype = 'Land' THEN ih.ptas_inspectiondate ELSE NULL END AS LandInspectedDate
		,CASE WHEN  ih.ptas_inspectiontype = 'Imps' THEN ih.ptas_inspectiondate ELSE NULL END AS ImpsInspectedDate
		,CASE WHEN  ih.ptas_inspectiontype = 'Both' THEN ih.ptas_inspectiondate ELSE NULL END AS BothInspectedDate
		FROM #RealProp rp
INNER JOIN [ptas].[ptas_inspectionhistory] ih ON rp.ParcelId = ih.ptas_parcelid   
WHERE SUBSTRING(ih.ptas_Name,1,4) = @AssmtYr


--FINAL RESULT  
SELECT  
  gmd.Major    
 ,gmd.Minor   
 ,gmd.ApplDistrict   
 --,Team       Hairo comment: needs to be calculated in GisMapData view
 ,gmd.ResArea   
 ,PI_or_AU = CASE
                 WHEN EXISTS (SELECT 1 
								FROM [dynamics].[ptas_inspectionyear] piy
							   INNER JOIN [dynamics].[ptas_area] pa ON  piy._ptas_area_value = pa.ptas_areaid 
							   WHERE piy.ptas_year = @AssmtYr
								 AND piy.ptas_inspectiontype = 1 --Physical inspection
								 AND pa.ptas_areanumber = gmd.ResArea) THEN 'PI'
                 ELSE 'AU' 
               END 			              
 ,gmd.ResSubArea   
 ,gmd.ResNbhd   
 ,Qtr = rp.QuarterSection    
 ,Sec = rp.Section   
 ,Twn = rp.Township   
 ,Rng = rp.[Range]   
 ,rp.Folio  
 ,AssessmentYr = @AssmtYr  
 ,rp.AssignedAppr  
 ,AppraiserAssignedPclCount = (select counts from #AppraiserAssignedPclCount c where c.AssignedBoth = rp.AssmtEntityId)  
 --TODO LIST - 1/3/2012 Don G  
 ,gmd.SalesCountUnverified  
 ,LandInspectedDate = COALESCE(id.LandInspectedDate,'')
 ,ImpsInspectedDate = COALESCE(id.ImpsInspectedDate,'')
 ,BothInspectedDate = COALESCE(id.BothInspectedDate,'')
 ,Bookmark = ISNULL(REVERSE(SUBSTRING(REVERSE((
												SELECT bt.ptas_name + '; '  
												FROM dynamics.ptas_bookmark	AS bm						
												INNER JOIN dynamics.ptas_ptas_bookmark_ptas_bookmarktag	AS bmt	ON bmt.ptas_bookmarkid = bm.ptas_bookmarkid
												INNER JOIN dynamics.ptas_bookmarktag					AS bt	ON bt.ptas_bookmarktagid = bmt.ptas_bookmarktagid
												where rp.ParcelId = bm._ptas_parceldetailid_value
												For XML PATH('')
												)),2,8000)),'')
--,PermitCountIncompl  AS IncomplNbrPrmts  Hairo comment: it comes from view GisMapData but is not calculated yet
--,PermitTypesIncompl  AS PrmtTypes        Hairo comment: it comes from view GisMapData but is not calculated yet
--,PermitCountComplCurCycle AS ComplNbrPrmts      Hairo comment: it comes from view GisMapData but is not calculated yet
--,'IncomplPermitValsTot' = PermitValsTotIncompl  Hairo comment: it comes from view GisMapData but is not calculated yet   
--,'IncomplPrmtDateRng' = PermitDateRngIncompl    Hairo comment: it comes from view GisMapData but is not calculated yet   
,'MaxPermitVal'  = ISNULL((SELECT MaxPermitVal  FROM #Permits p WHERE p.ParcelId = rp.ParcelId),0)  --added 6/6/2012 Don G   
,'MinPermitDate' = ISNULL((SELECT MinPermitDate FROM #Permits p WHERE p.ParcelId = rp.ParcelId),'')  --added 6/6/2012 Don G   
,'MaxPermitDate' = ISNULL((SELECT MaxPermitDate FROM #Permits p WHERE p.ParcelId = rp.ParcelId),'')  --added 6/6/2012 Don G   
,'IncomplSegMerg' = (SELECT SegMerg FROM #ToDoList tdl WHERE gmd.ParcelId = tdl.ParcelId)  
,'IncomplUnkill' = (SELECT Unkill FROM #ToDoList tdl WHERE gmd.ParcelId = tdl.ParcelId)  
,'IncomplNewParcel' = (SELECT NewParcel FROM #ToDoList tdl WHERE gmd.ParcelId = tdl.ParcelId)  
,'IncomplTransf' = (SELECT Transf FROM #ToDoList tdl WHERE gmd.ParcelId = tdl.ParcelId)  
,'IncomplAR' = (SELECT AR FROM #ToDoList tdl WHERE gmd.ParcelId = tdl.ParcelId)  
,'IncomplCR' = (SELECT CR FROM #ToDoList tdl WHERE gmd.ParcelId = tdl.ParcelId)  
,'IncomplDestruct' = (SELECT Destruct FROM #ToDoList tdl WHERE gmd.ParcelId = tdl.ParcelId)  
,'NeedLandVal' = (SELECT LandVal FROM #ToDoList tdl WHERE gmd.ParcelId = tdl.ParcelId)  
,'NeedTotalVal' = (SELECT TotalVal FROM #ToDoList tdl WHERE gmd.ParcelId = tdl.ParcelId)  
/*
Hairo comment: 	No idea how to calculate this, the table Checkout in Real Property is EMPTY.
,CkOutBy = ISNULL((select CkOutBy + ' ' from Checkout ck where ck.RealPropId = rp.RealPropId and CkOutType = 'B'),'')  
          +ISNULL((select 'Lnd to '+ CkOutBy + ' ' from Checkout ck where ck.RealPropId = rp.RealPropId and CkOutType = 'L'),'')  
          +ISNULL((select 'Imps to '+ CkOutBy + ' ' from Checkout ck where ck.RealPropId = rp.RealPropId and CkOutType = 'I'),'')  
*/  
 ,gmd.AddrLine    
 ,gmd.DistrictName 
 ,gmd.TaxPayerName    
 ,gmd.PropType   
 ,gmd.ApplGroup   
 ,gmd.TaxStatus   
 --,VacImpAccy   	Hairo comment: it comes from view GisMapData but is not calculated yet
 --,GeneralClassif  Hairo comment: it comes from view GisMapData but is not calculated yet
 ,gmd.SqFtLot   
 
              
 ,DrySqFt 		=  ISNULL((SELECT pl.ptas_drysqft     FROM  [dynamics].[ptas_land] pl WHERE pl.ptas_landid = gmd .LndGuid),gmd.SqFtLot) 
 ,SubmergedSqFt = ISNULL((SELECT pl.ptas_submergedsqft  FROM  [dynamics].[ptas_land] pl WHERE pl.ptas_landid = gmd.LndGuid),0) 								
 --,LandProblemDescr = LandProbDescrPart1 + LandProbDescrPart2   Hairo comment: these 2 columns comes from view GisMapData but are not calculated yet
 ,gmd.ViewDescr    
 ,gmd.PcntBaseLandValImpacted  
 ,gmd.PcntUnusable  
 ,gmd.Unbuildable  
 ,Zoning = gmd.CurrentZoning                                  
 ,PresentUse = gmd.PresentUse  
 ,gmd.NonWaterViews  
 ,gmd.WaterViews  
 ,gmd.TotalViews  
 ,'BldgPicCount' = isnull((select count(*) from #Media m where m.ParcelId = gmd.ParcelId and m.MediaUsage = 'Bldg'),0)   
                 + isnull((select count(*) from #Media m where m.ParcelId = gmd.ParcelId and m.MediaUsage = 'MH'),0)   
 ,'OtherPicCount' = isnull((select count(*) from #Media m where m.ParcelId = gmd.ParcelId and m.MediaUsage not in ('Bldg','MH','BldgPlanPic','BldgPlanWMF','MHPlanPic','MHPlanWMF')),0)   
 ,'MaxPicDate' =  isnull((select max(MediaDate) from #Media m where m.ParcelId = gmd.ParcelId and m.MediaUsage not like ('%plan%')),'')   
   
 ,'Plan' = CASE   
             WHEN (select count(*) from #Media m where m.ParcelId = gmd.ParcelId and m.MediaUsage IN ('BldgPlanWMF','MHPlanWMF'))  > 0 THEN 'CADD'  
             WHEN (select count(*) from #Media m where m.ParcelId = gmd.ParcelId and m.MediaUsage IN ('BldgPlanPic','MHPlanPic'))  > 0 THEN 'PIC'  
             ELSE ''     
           END            
 ,'MaxPlanDate' =  isnull((select max(MediaDate) from #Media m where m.ParcelId = gmd.ParcelId and m.MediaUsage in ('BldgPlanPic','BldgPlanWMF','MHPlanPic','MHPlanWMF')),'')   
  
-- --Imps  
,gmd.ResNbrImps   
,gmd.BldgGrade as ResBldgGrade  
,gmd.YrBltRen as ResYrBltRen   
,gmd.SqFtTotLiving  as ResSqFtTotLiv  
--,NbrResAccys + NbrCmlAccys as NbrAccys  Hairo comment: column NbrCmlAccys needs to be calculated in GisMapData view first
--,MHOMEREAL as RP_MH_Count  				-- Hairo comment: waiting for more dails on how to calculate MHOMEREAL in GisMapData
--,PP_MH_Count  							-- Hairo comment: waiting for more dails on how to calculate MHOMEREAL in GisMapData
  
,gmd.BaseLandVal  
,gmd.BaseLandValTaxYr   
,gmd.LandVal  
,gmd.ImpsVal  
,gmd.TotVal  
,gmd.NewConstrVal  
,gmd.SelectMethod   
,gmd.PostingStatusDescr  
,gmd.PrevLandVal  
,gmd.PrevImpsVal  
,gmd.PrevTotVal  
,gmd.PrevNewConstrVal  
,gmd.PrevSelectMethod  
,gmd.PcntChgLand  
,gmd.PcntChgImps  
,gmd.PcntChgTotal  
----Sales  
,SalesCountUnverified = isnull(gmd.SalesCountUnverified,'') --added 10/14/2011  
,SalesCountVerifiedThisCycle = isnull(gmd.SalesCountVerifiedThisCycle,'')  --added 10/14/2011  
,SalesCountVerifiedAtMkt = isnull(gmd.SalesCountVerifiedAtMkt,'') --added 10/14/2011  
--,gmd.SalePrice     Hairo comment: have to be calculated in view vw_GisMapData
--,SaleDate = isnull(gmd.SaleDate,'')                 Hairo comment: have to be calculated in view vw_GisMapData
--,ExciseTaxNbr = isnull(gmd.ExciseTaxNbr,'')  		  Hairo comment: have to be calculated in view vw_GisMapData
--,NbrPclsInSale = isnull(gmd.NbrPclsInSale,'')  	  Hairo comment: have to be calculated in view vw_GisMapData
--,VerifAtMkt = ISNULL(gmd.VerifAtMkt,'')   		  Hairo comment: have to be calculated in view vw_GisMapData
,rp.Pin  
,rp.MapPin  
,rp.RecId                              
FROM dynamics.vw_GISMapData gmd  
INNER JOIN #RealProp rp ON gmd.ParcelId = rp.ParcelId
INNER JOIN #InspectedDates id ON rp.ParcelId = id.ParcelId 
  
ORDER BY  
rp.RecId  
  
  
RETURN(0)  
  
ErrorHandler:  
RETURN (@Error)  
  
END  
  




GO


