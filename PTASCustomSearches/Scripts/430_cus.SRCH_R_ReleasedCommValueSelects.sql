IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_ReleasedCommValueSelects')
	DROP PROCEDURE [cus].[SRCH_R_ReleasedCommValueSelects]  
GO

CREATE PROCEDURE [cus].[SRCH_R_ReleasedCommValueSelects]
	@GeoArea		int
	,@GeoNbhd		int
	,@SpecArea		int	
	,@SpecNbhd		int
	,@Major			int
	,@Minor			int
	,@SelectAppr	char(4)
	,@ReleasedBy	char(4)
	,@ReleaseDate	smalldatetime
	,@AssmtYr		int

AS

BEGIN
/*  
Author: Mario Umaña  
Date Created:  25/01/2021  
Description:   SP that pulls all records  filtered by parameter @SpecArea, @Nbhd ,@Major ,@Minor ,@SelectAppr,@ReleasedBy ,@ReleaseDate AND @AssmtYr from comm.
  
Modifications:  
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]  
*/

DECLARE @chMajor	varchar(6)
	,@chMinor	varchar(4)



SET NOCOUNT ON
/*
SET @Nbhd=''
SET @Major=''
SET @Minor=''
SET @SelectAppr=''
SET @ReleasedBy=''
SET @ReleaseDate=''
*/

IF @AssmtYr='' OR @AssmtYr IS NULL
 SELECT @AssmtYr = 2020

--IF @Major='' AND @Minor>''
-- BEGIN
--  SET @ErrMsg = 'If you enter a major number you must also enter a minor number.'
--  Goto SQLError
-- END

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

DROP TABLE IF EXISTS #ReleasedValSel;
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

CREATE TABLE #ReleasedValSel
 (
	 RecId			int identity(1,1)
	 ,PIN			char(10)
	 ,MapPin		float
	 ,GeoArea		int
	 ,GeoNbhd		int
	 ,SpecArea		int
	 ,SpecNbhd		int
	 ,PropType		char(1)
	 ,Major			varchar(6)
	 ,Minor			varchar(4)
	 ,PrevLandVal	int
	 ,PrevImpsVal	int
	 ,PrevTotalVal	int
	 ,SelectLandVal	int
	 ,SelectImpsVal	int
	 ,SelectTotalVal	int
	 ,RevalOrMaint	char(1)
	 ,SelectAppr	char(4)
	 ,SelectReviewed	char(4)
	 ,SelectReviewDate	smalldatetime
	 ,ReviewedBy	char(4)
	 ,ReviewDate	smalldatetime
	 ,Releasedby	char(4)
	 ,ReleaseDate	smalldatetime
	 ,ReleaseNotes	varchar(2000)
	 ,RealPropId	int
	 ,RpGuid		uniqueidentifier
	 ,LandId		int
	 ,MsgId			int
 )

--insert non-condos
INSERT #ReleasedValSel
 (
 PIN
 ,MapPin
 ,GeoArea	
 ,GeoNbhd	
 ,SpecArea	
 ,SpecNbhd	
 ,PropType
 ,Major		
 ,Minor		
 ,PrevLandVal	
 ,PrevImpsVal	
 ,PrevTotalVal	
 ,SelectLandVal	
 ,SelectImpsVal	
 ,SelectTotalVal	
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
END								AS MapPin
 ,0								AS GeoArea
 ,0								AS GeoNbhd
 ,0								AS SpecArea
 ,0								AS SpecNbhd
 ,pt.PropType
 ,pd.ptas_major
 ,pd.ptas_minor
 ,0								AS PrevLandVal
 ,0								AS PrevImpsVal
 ,0								AS PrevTotalVal
  ,0							AS SelectLandVal
 ,0								AS SelectImpsVal
 ,0								AS SelectTotalVal
 ,rvs.ptas_revalormaint			AS RevalOrMaint
  ,COALESCE(su.ptas_legacyid,'')AS SelectAppr
 ,rvs.SelectReviewed			AS SelectReviewed
 ,rvs.SelectReviewDate			AS SelectReviewDate
 ,rvs.ReviewedBy				AS ReviewedBy
 ,rvs.ReviewDate				AS ReviewDate
 ,rvs.ReleasedBy				AS ReleasedBy
 ,rvs.ReleaseDate				AS ReleaseDate
 ,''							AS ReleaseNotes
 ,COALESCE(rvs.RealPropId,0)	AS RealPropId
 ,pd.ptas_parceldetailid		AS RpGuid
 ,rvs.LandId					AS LandId
 ,rvs.MsgId						AS MsgId
From		dynamics.ptas_parceldetail AS pd
JOIN		#PropType								AS pt	ON pt.ptas_propertytypeid = pd._ptas_propertytypeid_value
JOIN (Select 
		 ah.ptas_parcelid
		 ,ah.ptas_appr
		 ,ah.ptas_revalormaint
		 ,rvs.SelectReviewed			AS SelectReviewed
		 ,rvs.SelectReviewDate			AS SelectReviewDate
		 ,rvs.ReviewedBy				AS ReviewedBy
		 ,rvs.ReviewDate				AS ReviewDate
		 ,rvs.ReleasedBy				AS ReleasedBy
		 ,rvs.ReleaseDate				AS ReleaseDate
		 ,rvs.LandId					AS LandId
		 ,rvs.MsgId						AS MsgId
		 ,rvs.RealPropId
	from rp.ReviewValSel				AS rvs
	INNER JOIN ptas.ptas_appraisalhistory	AS ah ON rvs.RealPropId = ah.ptas_realpropid AND ah.ptas_taxyearidname = rvs.RollYr
	Where rvs.ReleasedBy>''
	  AND rvs.RollYr = @AssmtYr + 1
	  ) as rvs ON rvs.ptas_parcelid = pd.ptas_parceldetailid --Mario comment: We need to check this join later because it is using legacylandId
LEFT JOIN dynamics.systemuser			AS su	ON rvs.ptas_appr     = su.systemuserid
Where COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = ''
AND pt.PropType IN ('C','K')
AND   pd.ptas_applgroup in ('C')


--Add Geo and Spec Areas for non-condo units
UPDATE rvs
SET GeoArea		= pdm.GeoArea
   ,GeoNbhd		= pdm.GeoNbhd
   ,SpecArea	= pdm.SpecArea
   ,SpecNbhd	= pdm.SpecNbhd
FROM #ReleasedValSel AS rvs
JOIN (
		SELECT   pd.ptas_major
				,pd.ptas_minor
				,pga.ptas_areanumber				AS GeoArea
				,pgn.ptas_nbhdnumber				AS GeoNbhd
				,spec.ptas_areanumber				AS SpecArea
				,COALESCE(psn.ptas_nbhdnumber,'')	AS SpecNbhd
		FROM	dynamics.ptas_parceldetail			AS pd
			JOIN	#ReleasedValSel						AS rsvt ON rsvt.RpGuid = pd.ptas_parceldetailid AND COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = ''
			JOIN	dynamics.ptas_specialtyarea			AS spec ON spec.ptas_specialtyareaid = pd._ptas_specialtyareaid_value  
			JOIN	dynamics.ptas_specialtyneighborhood	AS psn  ON psn.ptas_specialtyneighborhoodid = pd._ptas_specialtynbhdid_value
			JOIN	dynamics.ptas_geoarea				AS pga	ON pga.ptas_geoareaid = pd._ptas_geoareaid_value
			JOIN	dynamics.ptas_geoneighborhood		AS pgn  ON pgn.ptas_geoneighborhoodid = pd._ptas_geonbhdid_value

) as pdm ON pdm.ptas_major = rvs.Major

------Add Geo and Spec Areas
UPDATE rvs
SET GeoArea		= pdm.GeoArea
   ,GeoNbhd		= pdm.GeoNbhd
   ,SpecArea	= pdm.SpecArea
   ,SpecNbhd	= pdm.SpecNbhd
FROM #ReleasedValSel AS rvs
JOIN (
		SELECT   pd.ptas_major
				,pd.ptas_minor
				,pga.ptas_areanumber				AS GeoArea
				,pgn.ptas_nbhdnumber				AS GeoNbhd
				,spec.ptas_areanumber				AS SpecArea
				,COALESCE(psn.ptas_nbhdnumber,'')	AS SpecNbhd
		FROM	dynamics.ptas_parceldetail			AS pd
			JOIN	#ReleasedValSel						AS rsvt ON rsvt.Major = pd.ptas_major AND COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = ''
			JOIN	dynamics.ptas_specialtyarea			AS spec ON spec.ptas_specialtyareaid = pd._ptas_specialtyareaid_value  
			JOIN	dynamics.ptas_specialtyneighborhood	AS psn  ON psn.ptas_specialtyneighborhoodid = pd._ptas_specialtynbhdid_value
			JOIN	dynamics.ptas_geoarea				AS pga	ON pga.ptas_geoareaid = pd._ptas_geoareaid_value

			JOIN	dynamics.ptas_geoneighborhood		AS pgn  ON pgn.ptas_geoneighborhoodid = pd._ptas_geonbhdid_value
		WHERE pd.ptas_minor = '0000'
) as pdm ON pdm.ptas_major = rvs.Major




--Add selected values
UPDATE rsv    
SET  SelectLandVal	= COALESCE(ah.ptas_landvalue,0) 
	,SelectImpsVal	= COALESCE(ah.ptas_impvalue,0)	 
	,SelectTotalVal	= COALESCE(ah.ptas_totalvalue,0)
	,SelectAppr		= su.ptas_legacyid
FROM  #ReleasedValSel rsv 
INNER JOIN ptas.ptas_appraisalhistory	AS ah	ON ah.ptas_parcelid	= rsv.RpGuid
JOIN  dynamics.systemuser				AS su	ON ah.ptas_appr = su.systemuserid
WHERE ah.ptas_taxyearidname = @AssmtYr+1 
  AND ah.ptas_revalormaint in ('R','M') 
  AND ah.ptas_impvalue > 0
  

--Apply filters inputed by user
IF @GeoArea>''
 BEGIN
  DELETE FROM #ReleasedValSel WHERE GeoArea<>@GeoArea
  IF @GeoNbhd>''
   DELETE FROM #ReleasedValSel WHERE GeoNbhd<>@GeoNbhd
 END
IF @SpecArea>''
 BEGIN
  DELETE FROM #ReleasedValSel WHERE SpecArea<>@SpecArea
  IF @SpecNbhd>''
   DELETE FROM #ReleasedValSel WHERE SpecNbhd<>@SpecNbhd
  END
IF @Major>''
 BEGIN
 DELETE FROM #ReleasedValSel WHERE Major<>@chMajor
  IF @Minor>''
   DELETE FROM #ReleasedValSel WHERE Minor<>@chMinor
 END
IF @SelectAppr>''
 DELETE FROM #ReleasedValSel WHERE SelectAppr<>@SelectAppr
IF @ReleasedBy>''
 DELETE FROM #ReleasedValSel WHERE ReleasedBy<>@ReleasedBy
If @ReleaseDate>''
 DELETE FROM #ReleasedValSel WHERE ReleaseDate<@ReleaseDate

--Add previous values
UPDATE #ReleasedValSel
SET  PrevLandVal = ISNULL(tr.ptas_appraiserlandvalue,0)
    ,PrevImpsVal = ISNULL(tr.ptas_appraiserimpvalue,0)
	,PrevTotalVal= ISNULL(tr.ptas_appraisertotalvalue,0)
FRom ptas.ptas_taxrollhistory	AS tr 
JOIN #ReleasedValSel			AS rvs	ON rvs.RpGuid	= tr.ptas_parcelid
JOIN dynamics.ptas_year			AS y	ON y.ptas_yearid = tr.ptas_taxyearid
Where y.ptas_name = @AssmtYr  AND
tr.ptas_receivabletype = 'R'
AND tr.ptas_omityearidname = 0


--Add Notes
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
 GeoArea	
 ,GeoNbhd	
 ,SpecArea	
 ,SpecNbhd
 ,PropType	
 ,Major		
 ,Minor		
 ,PrevLandVal	
 ,PrevImpsVal	
 ,PrevTotalVal	
 ,SelectLandVal	
 ,SelectImpsVal	
 ,SelectTotalVal	
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
 ORDER BY GeoArea,GeoNbhd,SpecArea,SpecNbhd,Major,Minor

--SET @ErrMsg='Successful, no errors'


RETURN(0)
--SQLError:
-- RETURN(10001)

END


GO
--EXEC [cus].[SRCH_R_ReleasedCommValueSelects] 70,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2019

