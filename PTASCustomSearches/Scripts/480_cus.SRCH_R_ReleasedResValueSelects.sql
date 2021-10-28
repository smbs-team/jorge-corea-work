IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_ReleasedResValueSelects')
	DROP PROCEDURE [cus].[SRCH_R_ReleasedResValueSelects]  
GO

CREATE PROCEDURE [cus].[SRCH_R_ReleasedResValueSelects]
	 @ResArea			int
	,@ResSubArea		int
	,@Major				int
	,@Minor				int
	,@SelectAppr		char(4)
	,@ReleasedBy		char(4)
	,@ReleaseDate		smalldatetime
	,@AssmtYr			int

AS

BEGIN
/*  
Author: Mario Umaña  
Date Created:  21/01/2021  
Description:   SP that pulls all records  filtered by parameter @SpecArea, @Nbhd ,@Major ,@Minor ,@SelectAppr,@ReleasedBy ,@ReleaseDate AND @AssmtYr from residentials.
  
Modifications:  
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]  
*/

DECLARE  @chResArea	varchar(3)
		,@chResSubArea	varchar(3)
		,@chMajor	varchar(6)
		,@chMinor	varchar(4)



--SET NOCOUNT ON


IF @AssmtYr='' OR @AssmtYr IS NULL
 SELECT @AssmtYr = 2020

--IF @Major='' AND @Minor>''
-- BEGIN
--  SET @ErrMsg = 'If you enter a major number you must also enter a minor number.'
--  Goto SQLError
-- END

IF @ResArea > ''
 BEGIN
  SET @chResArea = convert(varchar(3),@ResArea)
  SET @chResArea = replicate(0,3-len(@chResArea)) + @chResArea
 END 
IF @ResSubArea > ''
 BEGIN
  SET @chResSubArea = convert(varchar(3),@ResSubArea)
  SET @chResSubArea = replicate(0,3-len(@chResSubArea)) + @chResSubArea
 END

IF @Major>''
 BEGIN
  SET @chMajor = convert(varchar(6),@Major)
  SET @chMajor = replicate(0,6-len(@chMajor)) + @chMajor
 END

IF @Minor>''
 BEGIN
  SET @chMinor = convert(varchar(4),@Minor)
  SET @chMinor = replicate(0,4-len(@chMinor)) + @chMinor
 END

DROP TABLE IF EXISTS #PropType;
DROP TABLE IF EXISTS #ReleasedValSel;

CREATE TABLE #PropType
(
	 ptas_propertyTypeId UNIQUEIDENTIFIER PRIMARY KEY
	,PropType NVARCHAR(1)
)

INSERT INTO #PropType
SELECT  ptas_propertyTypeId
       ,SUBSTRING(pt.ptas_name,1,1) 
FROM dynamics.ptas_propertytype AS pt

CREATE TABLE #ReleasedValSel
 (
 Recid      int identity(1,1)
 ,PIN       char(10)
 ,MapPin    float
 ,ResArea	varchar(3)
 ,ResSubArea	varchar(3)
 ,Major		varchar(6)
 ,Minor		varchar(4)
 ,PropType   char(1)
 ,PrevLand	int
 ,PrevImps	int
 ,PrevTotal	int
 ,SelectLand	int
 ,SelectImps	int
 ,SelectTotal	int
 ,RevalOrMaint	char(1)
 ,SelectAppr    char(4)
 ,SelectReviewed	char(4)
 ,SelectReviewDate	smalldatetime
 ,ReviewedBy	char(4)
 ,ReviewDate	smalldatetime
 ,ReleasedBy	char(4)
 ,ReleaseDate	smalldatetime
 ,ReleaseNotes	varchar(2000)
 ,RealPropId	int
 ,RpGuid    uniqueidentifier
 ,LandId	int
 ,MsgId		int
 )

INSERT #ReleasedValSel
 (
  PIN
 ,MapPin
 ,ResArea
 ,ResSubArea
 ,Major
 ,Minor
 ,PropType
 ,PrevLand
 ,PrevImps
 ,PrevTotal
 ,SelectLand
 ,SelectImps
 ,SelectTotal
 ,RevalOrMaint
 ,SelectAppr
 ,SelectReviewed
 ,SelectReviewDate
 ,ReviewedBy
 ,ReviewDate
 ,ReleasedBy
 ,ReleaseDate
 ,ReleaseNotes
 ,RealPropId
 ,RpGuid
 ,LandId
 ,MsgId
 )
SELECT
  pd.ptas_Major+pd.ptas_Minor	AS PIN
 ,CASE
	WHEN pd.ptas_proptype = 'K' 
	THEN CONVERT(float, pd.ptas_major + '0000')
	ELSE CONVERT(float, pd.ptas_major + pd.ptas_minor)
 END							AS MapPin
 ,CASE WHEN LEN(pd.ptas_resarea) = 1 THEN '00'+CAST(pd.ptas_resarea AS VARCHAR(3))  
         WHEN LEN(pd.ptas_resarea) = 2 THEN '0'+CAST(pd.ptas_resarea AS VARCHAR(3))  
         WHEN LEN(pd.ptas_resarea) = 3 THEN CAST(pd.ptas_resarea AS VARCHAR(3))  
         ELSE ''  
       END						AS ResArea
  ,CASE WHEN LEN(pd.ptas_ressubarea) = 1 THEN '00'+CAST(pd.ptas_ressubarea AS VARCHAR(3))  
         WHEN LEN(pd.ptas_ressubarea) = 2 THEN '0'+CAST(pd.ptas_ressubarea AS VARCHAR(3))  
         WHEN LEN(pd.ptas_ressubarea) = 3 THEN CAST(pd.ptas_ressubarea AS VARCHAR(3))  
         ELSE ''  
       END						AS ResSubArea
 ,pd.ptas_major
 ,pd.ptas_minor
 ,pt.PropType
 ,0								AS PrevLandVal
 ,0								AS PrevImpsVal
 ,0								AS PrevTotalVal
 ,COALESCE(rvs.ptas_landvalue,0)AS SelectLandVal
 ,COALESCE(rvs.ptas_impvalue,0)	AS SelectImpsVal
 ,COALESCE(rvs.ptas_totalvalue,0)AS SelectTotalVal
 ,rvs.ptas_revalormaint 		AS RevalOrMaint
 ,COALESCE(su.ptas_legacyid,'')	AS SelectAppr
 ,rvs.SelectReviewed			AS SelectReviewed
 ,rvs.SelectReviewDate			AS SelectReviewDate
 ,rvs.ReviewedBy				AS ReviewedBy
 ,rvs.ReviewDate				AS ReviewDate
 ,rvs.ReleasedBy				AS ReleasedBy
 ,rvs.ReleaseDate				AS ReleaseDate
 ,''							AS ReleaseNotes
 ,COALESCE(rvs.RealPropId,0)	AS RealPropId
 ,pd.ptas_parceldetailid		AS RpGuid
 ,COALESCE(rvs.landId,0)		AS LandId
 ,rvs.MsgId						AS MsgId
From		dynamics.ptas_parceldetail AS pd
JOIN		#PropType								AS pt	ON pt.ptas_propertytypeid = pd._ptas_propertytypeid_value
JOIN (Select 
		  ah.ptas_parcelid
		 ,ah.ptas_appr
		 ,ah.ptas_revalormaint
		 ,ah.ptas_landvalue
		 ,ah.ptas_impvalue
		 ,ah.ptas_totalvalue
		 ,rvs.RealPropId
		 ,rvs.SelectReviewed			AS SelectReviewed
		 ,rvs.SelectReviewDate			AS SelectReviewDate
		 ,rvs.ReviewedBy				AS ReviewedBy
		 ,rvs.ReviewDate				AS ReviewDate
		 ,rvs.ReleasedBy				AS ReleasedBy
		 ,rvs.ReleaseDate				AS ReleaseDate
		 ,rvs.LandId					AS LandId
		 ,rvs.MsgId						AS MsgId
	from rp.ReviewValSel				AS rvs
	INNER JOIN ptas.ptas_appraisalhistory	AS ah ON rvs.RealPropId = ah.ptas_realpropid AND ah.ptas_taxyearidname = rvs.RollYr and ah.ptas_revalormaint = rvs.RevalOrMaint
	Where rvs.ReleasedBy>''
	  AND rvs.RollYr = @AssmtYr + 1 
	  AND ah.ptas_taxyearidname = @AssmtYr+1 
	  AND ah.ptas_revalormaint in ('R','M')
	  ) as rvs ON rvs.ptas_parcelid = pd.ptas_parceldetailid
LEFT JOIN dynamics.systemuser			AS su	ON rvs.ptas_appr     = su.systemuserid
WHERE COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = '' --This get the ACTIVE parcels. When "statecode  = 1 AND dpd.statuscode = 2" the parcel is Killed 
 AND pt.PropType = 'R' AND pd.ptas_applgroup = 'R'
 
--Apply user input
IF @ResArea>''
 BEGIN
  DELETE FROM #ReleasedValSel WHERE ResArea<>@chResArea
   IF @ResSubArea>''
    DELETE FROM #ReleasedValSel WHERE ResSubArea<>@chResSubArea
 END
IF @Major>''
 BEGIN
 PRINT 'Delete Major'
  DELETE FROM #ReleasedValSel WHERE Major<>@chMajor
   IF @Minor>''
    DELETE FROM #ReleasedValSel WHERE Minor<>@chMinor
 END
IF @SelectAppr>''
 DELETE FROM #ReleasedValSel WHERE SelectAppr<>@SelectAppr
IF @ReleasedBy>''
 DELETE FROM #ReleasedValSel WHERE ReleasedBy<>@ReleasedBy
IF @ReleaseDate>''
 DELETE FROM #ReleasedValSel WHERE ReleaseDate<@ReleaseDate

--Add previous values
UPDATE #ReleasedValSel
SET  PrevLand = ISNULL(tr.ptas_appraiserlandvalue,0)
    ,PrevImps = ISNULL(tr.ptas_appraiserimpvalue,0)
	,PrevTotal= ISNULL(tr.ptas_appraisertotalvalue,0)
FRom ptas.ptas_taxrollhistory	AS tr 
JOIN #ReleasedValSel			AS rvs	ON rvs.RpGuid	= tr.ptas_parcelid
JOIN dynamics.ptas_year			AS y	ON y.ptas_yearid = tr.ptas_taxyearid
Where y.ptas_name = @AssmtYr  AND
tr.ptas_receivabletype = 'R'
AND tr.ptas_omityearidname = 0


----Add Notes
DECLARE @MsgCntr	int

SET @MsgCntr = 0

BEGIN

 CREATE TABLE #MsgID (MsgId int, MaxMsgInstance int)

 CREATE TABLE #ReviewValSelMsg
  (
  MsgId	int
  ,MsgInstance	int
  ,Msg		varchar(500)
  ,UpdatedBy	char(4)
  ,UpdateDate	smalldatetime
  )
 INSERT #ReviewValSelMsg
 SELECT
  rvsm.MsgId
  ,rvsm.MsgInstance
  ,rvsm.Msg
  ,rvsm.UpdatedBy
  ,rvsm.UpdateDate
 FROM rp.ReviewValSelMsg rvsm inner join #ReleasedValSel rel on rvsm.MsgId=rel.MsgId
 WHERE rel.MsgId>0

 WHILE @MsgCntr<3
 BEGIN
  INSERT #MsgId
  SELECT MsgId,max(MsgInstance)
  FROM #ReviewValSelMsg
  GROUP BY MsgId
 
  IF @MsgCntr =2
  BEGIN
   UPDATE #ReleasedValSel
   SET ReleaseNotes = ReleaseNotes + ' AND OTHER NOTES;'
   FROM #ReleasedValSel rel inner join #ReviewValSelMsg rvsm on rel.MsgId=rvsm.MsgId
    inner join #MsgId m on rel.MsgId=m.MsgId
   WHERE rvsm.MsgInstance=m.MaxMsgInstance 
  END
  ELSE
  BEGIN   
   UPDATE #ReleasedValSel
   SET ReleaseNotes = ReleaseNotes + rvsm.Msg + '(' + convert(char(11),rvsm.UpdateDate) + ' ' + rvsm.UpdatedBy + '); '
   FROM #ReleasedValSel rel inner join #ReviewValSelMsg rvsm on rel.MsgId=rvsm.MsgId
    inner join #MsgId m on rel.MsgId=m.MsgId
   WHERE rvsm.MsgInstance=m.MaxMsgInstance
  END
  
  DELETE #ReviewValSelMsg
  FROM #ReviewValSelMsg rvsm inner join #MsgId m on rvsm.MsgId=m.MsgId and rvsm.MsgInstance=m.MaxMsgInstance

  TRUNCATE TABLE #MsgId
 
  SET @MsgCntr = @MsgCntr + 1

 END
END

UPDATE #ReleasedValSel   --added due to QMAS error:  Unknown Error 1004 Application-defined or object-defined error  DSAX 8/02/02 
SET ReleaseNotes = STUFF(ReleaseNotes, 1, 1, 'Equals ') where left(ReleaseNotes,1)='='

--Added due to DTS problem with notes with CR's obtained from this query and merged into BU spreadsheets by users.
UPDATE #ReleasedValSel 
SET ReleaseNotes = REPLACE ( ReleaseNotes ,CHAR(13)+CHAR(10),CHAR(32) ) 
WHERE (CHARINDEX(CHAR(13), ReleaseNotes) > 0 AND CHARINDEX(CHAR(10), ReleaseNotes) > 0)

SELECT 
 ResArea
 ,ResSubArea
 ,Major
 ,Minor
 ,PropType
 ,PrevLand
 ,PrevImps
 ,PrevTotal
 ,SelectLand
 ,SelectImps
 ,SelectTotal
 ,RevalOrMaint
 ,SelectAppr
 ,SelectReviewed
 ,SelectReviewDate
 ,ReviewedBy
 ,ReviewDate
 ,ReleasedBy
 ,ReleaseDate
 ,ReleaseNotes
 ,RealPropId
 ,LandId
 ,MsgId
 ,PIN
 ,MapPin
 ,RecId
FROM #ReleasedValSel
ORDER BY ResArea,ResSubArea,Major,Minor

--SET @ErrMsg='Successful, no errors'


RETURN(0)
--SQLError:
-- RETURN(10001)

END


GO
EXEC [cus].[SRCH_R_ReleasedResValueSelects] 090,004,NULL,NULL,NULL,NULL,NULL,2019
