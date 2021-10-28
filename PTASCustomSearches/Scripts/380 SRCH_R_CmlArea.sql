SELECT dynamics.fn_ReturnMetaData('AssessmentYear','GlobalConstant')

/*
CREATE PROCEDURE [cus].[SRCH_R_CmlArea2] 
   @GeoArea varchar(3) 
  ,@GeoNbhd varchar(3) 
  ,@IncludeSpecY varchar(3)  
  ,@SpecArea varchar(3) 
  ,@SpecNbhd varchar(3)
  ,@PropName varchar(80)
  ,@TaxpayerName varchar(80)

  
AS
BEGIN
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
*/
------BEGIN FOR TEST PORPUSES - FOR TEST PORPUSES----------------------

DECLARE
   @GeoArea varchar(3) 
  ,@GeoNbhd varchar(3) 
  ,@IncludeSpecY varchar(3)  
  ,@SpecArea varchar(3) 
  ,@SpecNbhd varchar(3)
  ,@PropName varchar(80)
  ,@TaxpayerName varchar(80)

SELECT 
   @GeoArea		 =''
  ,@GeoNbhd		 =''
  ,@IncludeSpecY ='' 
  ,@SpecArea	 =''
  ,@SpecNbhd	 =''
  ,@PropName	 =''
  ,@TaxpayerName =''

------END FOR TEST PORPUSES - FOR TEST PORPUSES----------------------



DECLARE @AssmtYr int
SELECT @AssmtYr = (select RPAssmtYr from dynamics.AssmtYr_Dev)


--SELECT * FROM LevyRateDistribution

--TODO rename these to clearly show last year and 2 years back
DECLARE @BillYr int
SELECT @BillYr = @AssmtYr + 1

DECLARE @MostRecentBillYrWithTaxesAvailable int
SELECT @MostRecentBillYrWithTaxesAvailable = (select max(BillYr) from LevyRateDistribution)

DECLARE @SecondMostRecentBillYrWithTaxesAvailable int
SELECT @SecondMostRecentBillYrWithTaxesAvailable = @MostRecentBillYrWithTaxesAvailable - 1

--TODO --Populate AccyDescrAll in GisMapData   
--TODO Add Jurisdiction
--Notes.  At this point,  sales data etc are returned only condo complexes, not units.  But CondoValPostDescrPrt1 + CondoValPostDescrPrt2 lists unit breakdowns
CREATE TABLE #RealProp 
   (
     RecId int Identity(1,1) 
   , PIN char(10)
   , MapPin float
   , RpGuid uniqueidentifier
   , LndGuid uniqueidentifier
   , LandId int
   , PropName varchar(80)
   , TaxpayerName varchar(80)
   , NoteId int
   , QuarterSection char(2)
   , Section tinyint
   , Township tinyint
   , [Range] tinyint
   , Folio varchar(7)
   , Notes nvarchar(max)
   , Income	int
   , RCN	int
   , RCNLD int   
       )


--Also, add QSTR, since someone requested it and I deliberately left it out of GisMapData (figured a QSTR GIS layer would work better)
--added search by PropName and TaxpayerName 6/4/12 Don G
INSERT #RealProp  (PIN, MapPin, RpGuid, LndGuid, LandId, PropName, TaxpayerName, NoteId, QuarterSection, Section, Township, [Range], Folio, Notes) 
SELECT gmd.PIN, convert(float,gmd.PIN), gmd.RpGuid, gmd.LndGuid, gmd.LandId, gmd.PropName, gmd.TaxpayerName, 0, rp.QuarterSection, rp.Section, rp.Township, rp.[Range], rp.Folio ,''
FROM RealProp rp
INNER JOIN GisMapData gmd ON gmd.RpGuid = rp.RpGuid
WHERE   
        (
      (CONVERT(int,GeoArea) = ISNULL(@GeoArea,'')  AND ISNULL(@GeoArea,'') <>''   AND ISNULL(@GeoNbhd,'') = '' AND ISNULL(@IncludeSpecY,'') <> 'Y' AND (SpecArea = 0 OR (SpecArea = 700 AND gmd.ApplGroup IN ('C','M'))) ) 
   OR (CONVERT(int,GeoArea) = ISNULL(@GeoArea,'')  AND ISNULL(@GeoArea,'') <>''   AND ISNULL(@GeoNbhd,'') = ''  AND ISNULL(@IncludeSpecY,'') = 'Y' AND ISNULL(@PropName,'') = ''  )       
   OR (CONVERT(int,GeoArea) = ISNULL(@GeoArea,'')  AND CONVERT(int,GeoNbhd) = ISNULL(@GeoNbhd,'')   AND ISNULL(@GeoArea,'') <>''  AND ISNULL(@GeoNbhd,'') <>'' AND ISNULL(@IncludeSpecY,'') <> 'Y' AND  (SpecArea = 0 OR (SpecArea = 700 AND gmd.ApplGroup IN (
'C','M')))  )
   OR (CONVERT(int,GeoArea) = ISNULL(@GeoArea,'')  AND CONVERT(int,GeoNbhd) = ISNULL(@GeoNbhd,'')   AND ISNULL(@GeoArea,'') <>''  AND ISNULL(@GeoNbhd,'') <>'' AND ISNULL(@IncludeSpecY,'') = 'Y'  )        
   OR (CONVERT(int,SpecArea) = ISNULL(@SpecArea,'')  AND ISNULL(@SpecArea,'') <>'') AND ISNULL(@SpecNbhd,'') = '' 
   OR (CONVERT(int,SpecArea) = ISNULL(@SpecArea,'')  AND CONVERT(int,SpecNbhd) = ISNULL(@SpecNbhd,'') AND ISNULL(@SpecArea,'') <>''  AND ISNULL(@SpecNbhd,'') <>'' )         
   OR ISNULL(@PropName,'') <> '' AND gmd.PropName like '%' + @PropName + '%' and gmd.PropType<>'R' 
   OR ISNULL(@TaxpayerName,'') <> '' AND gmd.TaxpayerName like '%' + @TaxpayerName + '%' and gmd.PropType<>'R'        
         )
ORDER BY         
  gmd.Major  
 ,gmd.Minor
         

----start debug
--print 'select * from #RealProp'
--select * from #RealProp


--print 'insert #SecondMostRecentAvailableTaxes'
--SELECT top 10 * 
--FROM #RealProp rp
--INNER JOIN RealPropAcct acc ON acc.RpGuid = rp.RpGuid
--INNER JOIN TaxAcctReceivable tar ON tar.AcctNbr = acc.AcctNbr
--INNER JOIN LevyCodeYr lcy ON lcy.BillYr = tar.BillYr AND lcy.LevyCode = tar.LevyCode
--INNER JOIN LevyCodeDistYr lcdy ON lcdy.BillYr = tar.BillYr AND lcdy.LevyCode = tar.LevyCode
--INNER JOIN District di ON di.DistrictId = lcdy.DistrictId
--INNER JOIN LevyRateDistribution d ON d.BillYr = tar.BillYr AND d.Levy = tar.LevyCode     
--AND d.[Type] = CASE 
--                 WHEN ISNULL(tar.TaxValReason,'') = 'FS' THEN 'F'                        
--                 ELSE 'R'
--               END
--WHERE tar.OmitYr = 0 AND tar.ReceivableType = 'R' 
--AND tar.BillYr = 2018  --@PrevBillYr

--return(0)
----end debug

---- Add condo units that are commercial responsibility
--INSERT #RealProp
--SELECT p.RealPropId, p.Major, p.Minor, p.ApplGroup, t.Area, 
--       t.Neighborhood, t.Qtr, t.Sect, t.Town, t.Range, t.Folio, p.PropType, p.LevyCode, 
--       t.PropName, t.LandId, 0, 0, 0, 0, 0, 0,' ',' ',' ',
--       0, 0, ' ', 0, 0, ' ', 0, 0, ' ', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, ' ', ' ', ' ', ' ', ' ',0,
--       0, 0, 0, ' ', ' ', 0, 0,'', 0,'', '', 0,'','','','No Situs Address',p.NoteId,' ','',''
--  FROM #commParcel t 
--           INNER JOIN RealProp c (nolock) ON t.RealPropId = c.RealPropId
--           INNER JOIN RealProp p (nolock) ON c.Major = p.Major
-- WHERE p.PropType = 'K' AND p.ApplGroup = 'C' AND p.Minor > '0000'
--   AND c.PropType = 'K'  --jcy 6/30/00 guards against PROPTYPE C condo units


--use this ptas.ptas_estimatehistory replaced by RealPropValEst
--Estimates 
UPDATE #RealProp
   SET  Income	= e.ImpsVal
  FROM #RealProp rp
           INNER JOIN RealPropValEst e (nolock) ON rp.LandId = e.LandId
 WHERE e.EstType = 8 AND e.RollYr = @AssmtYr + 1

UPDATE #RealProp
   SET RCN 	= e.ImpsVal
  FROM #RealProp rp
           INNER JOIN RealPropValEst e (nolock) ON rp.LandId = e.LandId
 WHERE e.EstType = 2 AND e.RollYr = @AssmtYr + 1

UPDATE #RealProp
   SET  RCNLD 	= e.ImpsVal
  FROM #RealProp rp
           INNER JOIN RealPropValEst e (nolock) ON rp.LandId = e.LandId
 WHERE e.EstType = 3 AND e.RollYr = @AssmtYr + 1
 


--NOTES  Get 3 most recent notes
--hairo comment: probably cammanotes
/*
UPDATE #RealProp
   SET NoteId = n.NoteId
  FROM Note_cmpt n INNER JOIN #RealProp r ON n.TblRecGuid = r.RpGuid 

CREATE TABLE #NoteId(
  NoteId int
 ,MaxNoteInstance int
)

DECLARE @NICntr tinyint
       ,@Notes char(1) --Dont really need it as a variable, but keep this for now in case it needs to be a param, as it is in Estimates query.  

SET @Notes = 'Y'  --This goes away if @Notes becomes a param

SET @NICntr = 0

IF @Notes IS NOT NULL AND @Notes = 'Y'
BEGIN         

  CREATE TABLE #NoteInstance (
    NoteId int
   ,NoteInstance tinyint
   ,AssmtEntityId char(4)
   ,UpdateDate smalldatetime
   ,Note nvarchar(max)
  )

  INSERT #NoteInstance
  SELECT ni.NoteId,ni.NoteInstance,(SELECT AssmtEntityId FROM AssmtEntity ae WHERE ae.AeGuid = rpn.UpdatedByGuid),rpn.UpdateDate
         ,rpn.Note
    FROM NoteInstance_cmpt ni (NOLOCK) INNER JOIN #RealProp rp (NOLOCK) ON (ni.NoteId = rp.NoteId)
                                       INNER JOIN RPNote rpn ON ni.Guid = rpn.RpnGuid
   WHERE rp.NoteId > 0

  CREATE INDEX XIENoteInstance ON #NoteInstance
  (NoteId, NoteInstance)

  UPDATE #RealProp
     SET Notes = Notes + ' RP NOTE: '
   WHERE Notes <> ''

  WHILE @NICntr < 4
  BEGIN
    INSERT INTO #NoteId (NoteId, MaxNoteInstance)
    SELECT NoteId, Max(NoteInstance)
      FROM #NoteInstance (NOLOCK)
    GROUP BY NoteId

    CREATE INDEX XIENoteIId ON #NoteId
    (NoteId, MaxNoteInstance)

    IF @NICntr = 3
      UPDATE #RealProp
         SET Notes = Notes + ' AND OTHER NOTES; '
        FROM #RealProp cp INNER JOIN #NoteId nid (NOLOCK) ON (cp.NoteId = nid.NoteId)
                          INNER JOIN #NoteInstance ni (NOLOCK) ON (cp.NoteId = ni.NoteId AND nid.MaxNoteInstance = ni.NoteInstance)
    ELSE  
      UPDATE #RealProp
         SET Notes = Notes + ni.Note 
                     + '(' + convert(char(11),ni.UpdateDate) + ' ' + ni.AssmtEntityId + '); '
        FROM #RealProp cp INNER JOIN #NoteId nid (NOLOCK) ON (cp.NoteId = nid.NoteId)
                          INNER JOIN #NoteInstance ni (NOLOCK) ON (cp.NoteId = ni.NoteId AND nid.MaxNoteInstance = ni.NoteInstance)
       --WHERE (ISNULL(LEN(ni.ShortNote), 0) 
       --      + ISNULL(LEN(CONVERT(varchar(4500), ni.LongNote)), 0) 
       --      + ISNULL(LEN(cp.Notes), 0) 
       --      + 11 + 4 ) --UpdateDate + AssmtEntityId
       --      < 4500

    DELETE #NoteInstance
      FROM #NoteInstance ni INNER JOIN #NoteId nid ON (ni.NoteId = nid.NoteId AND ni.NoteInstance = nid.MaxNoteInstance)

    DROP INDEX #NoteId.XIENoteIId
    TRUNCATE TABLE #NoteId
    SET @NICntr = @NICntr + 1
  END
END

UPDATE #RealProp   --added due to QMAS error:  Unknown Error 1004 Application-defined or object-defined error  DSAX 8/02/02 
SET Notes = STUFF(notes, 1, 1, 'Equals ') where left(notes,1)='='

--Added due to DTS problem with notes with CR's obtained from this query and merged into BU spreadsheets by users.
UPDATE #RealProp
SET Notes = REPLACE ( Notes ,CHAR(13)+CHAR(10),CHAR(32) ) 
WHERE (CHARINDEX(CHAR(13), Notes) > 0 AND CHARINDEX(CHAR(10), Notes) > 0)

This up IS ALL notes
*/
 
 --Accessories
 Create Table #Accy
(RpGuid uniqueidentifier
,LandId int
,LastUsedId int
,StartIndex int
,MaxRows smallint
,SumAccyValue int
,AccyTypeDescr varchar(1200)) --should allow on avg up to 100 accy type descr; truncate later or change logic to loop and stop after x.


Create Table #TopValAccy
(LandId int
,UniqueId int identity(1,1)
,RpGuid uniqueidentifier
,AccyValue int
,NbrThisType int
,AccyTypeDescr varchar(40)) --40 is length allowed, actual existing max=17, avg=12


Insert #Accy
Select A.RpGuid
      ,ac.LandId
      ,0
      ,0
      ,max(ac.LineNbr)
      ,sum(a.AccyValue)
      ,''
FROM #RealProp rp
INNER JOIN Accy a ON a.RpGuid = rp.RpGuid
INNER JOIN Accy_Cmpt ac ON a.AccyGuid = ac.AccyGuid
Group by a.RpGuid,ac.LandId
Order by ac.LandId
  
  
Insert #TopValAccy (LandId ,RpGuid, AccyValue ,NbrThisType ,AccyTypeDescr)
Select t.LandId
      ,a.RpGuid 
      ,A.AccyValue
      ,A.NbrThisType
      ,lui.LUItemShortDesc
From Accy a INNER JOIN #Accy t on t.RpGuid = a.RpGuid
            INNER JOIN LUItem2 lui ON a.AccyTypeId = lui.LUTypeId AND a.AccyTypeItemId = lui.LUItemId
Order by t.LandId, A.AccyValue desc 



Update #TopValAccy
   Set AccyTypeDescr=Convert(varchar(4),NbrThisType)+' '+AccyTypeDescr
 Where NbrThisType>1

Update #TopValAccy
   Set AccyTypeDescr=REPLACE(AccyTypeDescr,':','')

Update #TopValAccy
   Set AccyTypeDescr=REPLACE(AccyTypeDescr,',','')

Update #Accy
   set StartIndex=(select min(UniqueId) from #TopValAccy TVA where A.LandId=TVA.LandId)
From #Accy A

DECLARE @AccyCntr tinyint
SET @AccyCntr = 0
WHILE @AccyCntr < 8
BEGIN
  IF @AccyCntr = 7
  BEGIN
    UPDATE #Accy
       SET AccyTypeDescr = A.AccyTypeDescr+ ' AND OTHER ACCYS;'
           ,LastUsedId=TVA.UniqueId
      FROM #TopValAccy TVA, #Accy A
     WHERE A.LandId = TVA.LandId
       AND @AccyCntr<=A.MaxRows
       AND TVA.UniqueId=@AccyCntr+A.StartIndex
  END
  ELSE
  BEGIN
    UPDATE #Accy
       SET AccyTypeDescr = A.AccyTypeDescr+TVA.AccyTypeDescr + '; '
           ,LastUsedId=TVA.UniqueId
      FROM #TopValAccy TVA, #Accy A 
     WHERE A.LandId = TVA.LandId
       AND @AccyCntr<=A.MaxRows
       AND TVA.UniqueId=@AccyCntr+A.StartIndex
  END
  SET @AccyCntr = @AccyCntr + 1
  
END  
  
                    

 
-- Get Sale Warnings
-- Sale warnings are all upper case, but Title case gives clearer separation and avoids SHOUTING (e.g., Multi-parcel sale; Financial institution resale; )
CREATE TABLE #SaleWarnings (SaleGuid uniqueidentifier, LastWarnId int default 0, Warnings varchar(150) default '')
DECLARE @WarnCntr tinyint
SET @WarnCntr = 0


INSERT #SaleWarnings (SaleGuid)
SELECT DISTINCT SaleGuid
FROM GisMapData gmd
INNER JOIN #RealProp rp ON rp.RpGuid = gmd.RpGuid
WHERE (CmlLandSale = 'Y' OR CmlAccySale = 'Y' OR CmlLandSale = 'Y' OR ResAccySale = 'Y' )


  WHILE @WarnCntr < 5
  BEGIN
    IF @WarnCntr = 4
    BEGIN
      UPDATE #SaleWarnings
         SET Warnings = Warnings + ' + OTHER WARNINGS;'
            ,LastWarnId = sw.WarningItemId
        FROM #SaleWarnings rs INNER JOIN SaleWarning sw (NOLOCK) ON (rs.SaleGuid = sw.SaleGuid)
       WHERE sw.WarningId = 7
         AND sw.WarningItemId > rs.LastWarnId
         AND LEN(Warnings + ' + OTHER WARNINGS;') < 150
    END
    ELSE
    BEGIN
      UPDATE #SaleWarnings                                                                                                           
         SET Warnings = Warnings + ' ' + SUBSTRING(UPPER(lu.LUItemShortDesc),1,1) + SUBSTRING(lower(lu.LUItemShortDesc),2,149) + ';' --Title case
            ,LastWarnId = sw.WarningItemId
        FROM #SaleWarnings rs INNER JOIN SaleWarning sw (NOLOCK) ON (rs.SaleGuid = sw.SaleGuid)
                          INNER JOIN LUItem2 lu ON (lu.LUTypeId = sw.WarningId AND lu.LUItemId = sw.WarningItemId)
       WHERE sw.WarningId = 7
         AND sw.WarningItemId > rs.LastWarnId
         AND LEN(Warnings + ' ' + lu.LUItemShortDesc + ';') < 150
    END
    SET @WarnCntr = @WarnCntr + 1
  END


----Select the appeals data into #Appeals


CREATE TABLE #ReviewTypeA
(
ReviewType         	VARCHAR(14)
,LUItemId		INT
)

--INSERT INTO #ReviewTypeA 
--Don't want to use or whole or part of luitemshortdesc.  If using luitemshortdesc, need lutypeid = 123 & 136
INSERT INTO #ReviewTypeA VALUES ('Local', 1 )
INSERT INTO #ReviewTypeA VALUES ('State', 2 )
INSERT INTO #ReviewTypeA VALUES ('Court', 3 )
INSERT INTO #ReviewTypeA VALUES ('AR', 4 )
INSERT INTO #ReviewTypeA VALUES ('CR', 5 )
INSERT INTO #ReviewTypeA VALUES ('Destruct', 6 )



CREATE TABLE #Appeals
( 
  RealPropId 		INT
 ,RpGuid           uniqueidentifier 
 ,AssmtReviewId      	INT
 ,AppealNbr          	VARCHAR(20)
 ,BillYr		CHAR(4)
 ,RespAppr		CHAR(4)
 ,ReviewType         	VARCHAR(14)
 ,StatusAssmtReview	VARCHAR(15)
 ,AssrRecommendation 	VARCHAR(7)
 ,StatusAssessor     	VARCHAR(15)
 ,StatusBoard        	VARCHAR(18)
 ,StatusStipulation  	VARCHAR(21)
 ,HearingDate		SMALLDATETIME
 ,HearingResult      	VARCHAR(10)
 ,ValuationType		VARCHAR(12)
 ,AppealedLandVal	INT	
 ,AppealedImpsVal	INT
 ,AsrRecOrStipLandVal	INT
 ,AsrRecOrStipImpsVal	INT
 ,SettlementLandVal	INT	
 ,SettlementImpsVal	INT
 ,StipLandVal		INT
 ,StipImpsVal		INT
 ,FinalLandVal		VARCHAR(25)
 ,FinalImpsVal		VARCHAR(25)
)




/*
Add appeals info 4/25/02 DSAX.   Specs: 
Need only most recent appeal from AssmtReview (1 record per parcel).
Display final value if available, if appeal/review not yet resolved than display StatusAssessor
This appeals/review data is problematic.  See notes in context below.
*/

INSERT INTO #Appeals
SELECT AR.RealPropId
      ,r.RpGuid
      ,AR.AssmtReviewId 
      ,AR.AppealNbr
      ,AR.BillYr
      ,AR.RespAppr
      ,AR.ReviewType  --not using lu on this yet because some reviewtypes (e.g. 6) not in lu where luitemid=123
      ,(SELECT SUBSTRING(lu.luitemshortdesc,1,15)	   	--StatusAssmtReview 
          FROM luitem2 lu (nolock)
         WHERE ar.statusassmtreview = lu.luitemid 
           AND lu.lutypeid = 122)	
      ,(SELECT  SUBSTRING(lu.luitemshortdesc,1,7)  		--AssrRecommendation (truncated)
          FROM luitem2 lu (nolock)
         WHERE ar.assrrecommendation = lu.luitemid 
           AND lu.lutypeid = 80)			--LUTypeDesc = Assessor Action
      ,(SELECT SUBSTRING(lu.luitemshortdesc,1,15)	   	--StatusAssessor (truncated)
          FROM luitem2 lu (nolock)
         WHERE ar.statusassessor = lu.luitemid 
           AND lu.lutypeid = 122)			--Note that all the status's are lutypeid = 122
      ,(SELECT SUBSTRING(lu.luitemshortdesc,1,18)		--StatusBoard (truncated)
          FROM luitem2 lu (nolock)
         WHERE ar.statusboard = lu.luitemid 
           AND lu.lutypeid = 122)	
      ,(SELECT SUBSTRING(lu.luitemshortdesc,1,21)		--StatusStip (truncated)
          FROM luitem2 lu (nolock)
         WHERE ar.statusstipulation = lu.luitemid 
           AND lu.lutypeid = 122)
      ,AR.HearingDate
      ,(SELECT SUBSTRING(lu.luitemshortdesc,1,10)		--HearingResult (truncated)
	  FROM luitem2 lu (nolock)
         WHERE ar.HearingResult = lu.luitemid 
	   AND lu.lutypeid = 73)
      ,''  --ARV.ValuationType (truncated)
      ,''  --ARV.ApprLandVal AppealedLandVal
      ,''  --ARV.ApprImpsVal AppealedImpsVal
      ,''  --ARV.ApprLandVal RecommLandVal
      ,''  --ARV.ApprImpsVal RecommImpsVal
      ,''  --ARV.ApprLandVal SettlementLandVal
      ,''  --ARV.ApprImpsVal SettlementImpVal
      ,''  --ARV.ApprLandVal StipLandVal
      ,''  --ARV.ApprImpsVal StipImpsVal
      ,''  --FinalLandVal Stip or Settlement
      ,''  --FinalImpsVal Stip or Settlement
  FROM AssmtReview_V ar	INNER JOIN RealProp r ON ar.RealpropId = r.Id
                        INNER JOIN #RealProp rp ON r.RpGuid= rp.RpGuid
 WHERE AR.AssmtReviewId =( SELECT max(AR1.AssmtReviewId) 
                             FROM AssmtReview_V AR1  (nolock)
                            WHERE AR1.RealPropId=AR.RealPropId)


/*
Select Lu.LUItemShortDesc from LuItem Lu where Lu.lutypeid = 125: 
Appealed Value (1), Assessor Recommended Value (2), Board Order Value (3), Taxpayer Recommended (4)& Stipulated (5).
*/



UPDATE 	#Appeals
SET  	AppealedLandVal =  ARV.ApprLandVal
	   ,AppealedImpsVal =  ARV.ApprImpsVal 
FROM 	AssmtReviewVal_AppealedVal_V ARV, #Appeals A 
WHERE ARV.AssmtReviewID = A.AssmtReviewID

UPDATE 	#Appeals
SET  	AsrRecOrStipLandVal =  ARV.ApprLandVal
	   ,AsrRecOrStipImpsVal =  ARV.ApprImpsVal 
FROM 	AssmtReviewVal_RecommendedVal_V ARV, #Appeals A 
WHERE ARV.AssmtReviewID = A.AssmtReviewID

UPDATE 	#Appeals
SET  	SettlementLandVal =  ARV.ApprLandVal
	   ,SettlementImpsVal =  ARV.ApprImpsVal 
FROM 	AssmtReviewVal_BoardOrderVal_V ARV, #Appeals A 
WHERE ARV.AssmtReviewID = A.AssmtReviewID

UPDATE 	#Appeals
SET  	StipLandVal =  ARV.ApprLandVal
	   ,StipImpsVal =  ARV.ApprImpsVal 
FROM 	AssmtReviewVal_StipulatedVal_V ARV, #Appeals A 
WHERE ARV.AssmtReviewID = A.AssmtReviewID

/*
Cleanup and format due to problematic data (examples follow):

AssmtReviewId StatusAssmtReview StatusStipulation     StipLandVal StipImpsVal 
------------- ----------------- --------------------- ----------- ----------- 
54648         Completed         Stipulation Finalized 99000       98000		GOOD:  STIP FINALIZED WITH STIP VALS
49577         Completed         Stipulation Finalized 0           0		NOT GOOD:  STIP FINALIZED BUT NO STIP VALS

COMBOS OF STATUSUS THAT MIGHT CAUSE PROBLEMS

Count       StatusAssmtReview StatusAssessor  StatusBoard        StatusStipulation     
----------- ----------------- --------------- ------------------ --------------------- 
1488        Completed         Assigned        Withdrawn          Stipulation Finalized	GOOD: COMPLETED, WITHDRAWN, FINALIZED 	
16          Active            Assigned        Appeal Filed       Stipulation Finalized	NOT GOOD: ACTIVE,FILED,FINALIZED
16          Active            Assigned        Hearing Scheduled  Stipulation Finalized	NOT GOOD: ACTIVE,SCHEDULED,FINALIZED
8           Completed         Case Submitted  Hearing Scheduled				NOT GOOD: COMPLETED,SCHEDULED
320         Active            Decision Review Board Order Issued			GOOD: ACTIVE, ASSR STILL REVIEWING
276         Completed         Order Review Co Board Order Issued			NOT GOOD: COMPLETED ASSR STILL REVIEWING
ETC.
Also, for hearings already past, StatusBoard might not have been changed from Hearing Scheduled to Hearing Completed
*/

DELETE #Appeals  --really final but didn't go through the process
WHERE (StatusBoard = 'withdrawn' OR StatusBoard = 'void' ) and StatusStipulation = ''

DELETE #Appeals  --Delete old cases, many have incomplete, misleading statusus
WHERE BillYr <(@BillYr - 4)  

DELETE #Appeals  --Delete final old cases (really final as indicated by value decision)
WHERE BillYr < @BillYr-1 
AND (StipLandVal > 0 or SettlementLandVal > 0 or StatusBoard = 'Board Order Issued'  or StatusBoard = 'Withdrawn' )

UPDATE 	#Appeals
SET  	FinalLandVal =  AppealedLandVal
	,FinalImpsVal =  AppealedImpsVal
WHERE 	HearingResult = 'SUSTAIN'

UPDATE 	#Appeals
SET  	FinalLandVal =  'HearingCompl'
	,FinalImpsVal = 'HearingCompl'
WHERE 	StatusBoard = 'Hearing Completed'

UPDATE 	#Appeals
SET  	FinalLandVal =  'HearingSched '
	,FinalImpsVal = HearingDate
WHERE 	StatusBoard = 'Hearing Scheduled' and (StatusAssessor <> 'Assigned' or StatusAssessor <> 'Received')

UPDATE 	#Appeals
SET  	FinalLandVal =  'Asr'+ StatusAssessor + ' HearingSched'
	,FinalImpsVal = HearingDate
WHERE 	StatusBoard = 'Hearing Scheduled' and (StatusAssessor = 'Assigned' or StatusAssessor = 'Received')


UPDATE 	#Appeals
SET  	FinalLandVal =  'HearingCompl'
	,FinalImpsVal = 'HearingCompl'
WHERE 	StatusBoard = 'Hearing Scheduled' and HearingDate < (SELECT (CURRENT_TIMESTAMP))

UPDATE 	#Appeals
SET  	FinalLandVal =  StipLandVal
	,FinalImpsVal =  StipImpsVal
	,AsrRecOrStipLandVal =  StipLandVal
	,AsrRecOrStipImpsVal =  StipImpsVal	
WHERE 	StatusStipulation =  'Stipulation Finalized'

UPDATE 	#Appeals
SET  	FinalLandVal =  'StipPending'
	,FinalImpsVal =  'StipPending'
	,AsrRecOrStipLandVal =  StipLandVal
	,AsrRecOrStipImpsVal =  StipImpsVal	
WHERE 	StatusStipulation =  'Stipulation Pending'

UPDATE 	#Appeals
SET  	FinalLandVal =  SettlementLandVal
	,FinalImpsVal =  SettlementImpsVal
WHERE 	SettlementLandVal > 0 or SettlementImpsVal > 0  --Contaminated pcls can have landval=0 and impval>0


UPDATE 	#Appeals
SET 	ReviewType = R.ReviewType
FROM    #Appeals A, #ReviewTypeA R
WHERE 	A.ReviewType= R.LUItemId

DELETE #Appeals  --Delete old cases with really final with value decision
WHERE BillYr < @BillYr-5 
AND StatusAssmtReview='completed' 
and (ReviewType = 'AR' or  ReviewType = 'CR' or ReviewType = 'Destruct')

UPDATE 	#Appeals
SET  	FinalLandVal =  ReviewType + ' '+StatusAssmtReview
	,FinalImpsVal = ReviewType + ' '+StatusAssmtReview 
WHERE 	ReviewType = 'AR' or  ReviewType = 'CR' or ReviewType = 'Destruct' 


--Comment out following portion due to following error.  Fix later if needed.
--Server: Msg 245, Level 16, State 1, Line 1
--Syntax error converting the varchar value 'HearingCompl' to a column of data type int.

--UPDATE 	#Appeals
--SET  	FinalLandVal =  StatusAssessor
--	,FinalImpsVal =  StatusAssessor
--WHERE  (FinalLandVal = 0 or FinalLandVal='') 
--AND (StatusBoard <> 'Board Order Issued' or StatusBoard <> 'Withdrawn' or StatusBoard <> 'Hearing Scheduled')

--UPDATE 	#Appeals
--SET  	FinalLandVal =  FinalLandVal + " "+ StatusAssessor
--	,FinalImpsVal = FinalImpsVal + " "+ StatusAssessor
--WHERE StatusAssessor = 'Decision Review' AND (FinalLandVal >0 OR FinalImpsVal >0)


--MEDIA
--Getting a pic count is more involved using the new media tables (compared to using ImageParcel)
--Getting the most recent pic date is even more involved
--So use temp table 

--select top 10 * from Media

--select * from luitem2 where lutypeid = 301  
--select * from luitem2 where lutypeid = 309
--select * from luitem2 where lutypeid = 313


--SELECT
-- (select count(*) from Media m inner join MediaAccy ma on m.MediaId = ma.MediaId where m.ImageTypeItemId = 1 and m.ActiveFlag = 'Y' and ma.LandId = gmd.LandId)
--+(select count(*) from Media m inner join MediaLand ml on m.MediaId = ml.MediaId where m.ImageTypeItemId = 1 and m.ActiveFlag = 'Y' and ml.LandId = gmd.LandId)
--+(select count(*) from Media m inner join MediaBldg mb on m.MediaId = mb.MediaId where m.ImageTypeItemId = 1 and m.ActiveFlag = 'Y' and mb.RealPropId = gmd.RealPropId)
----+MediaMobileHome
--from GisMapData gmd
--where gmd.PIN = '2106000070'
               

--SELECT
--m.MediaDate, m2.MediaDate, m3.MediaDate
--from GisMapData gmd
--left join MediaAccy ma on ma.LandId = gmd.LandId left join Media m on ma.MediaId = m.MediaId and m.ImageTypeItemId = 1 and m.ActiveFlag = 'Y'
--left join MediaLand ml on ml.LandId = gmd.LandId left join Media m2 on ml.MediaId = m2.MediaId and m2.ImageTypeItemId = 1 and m2.ActiveFlag = 'Y'
--left join MediaBldg mb on mb.RealPropId = gmd.RealPropId left join Media m3 on mb.MediaId = m3.MediaId and m3.ImageTypeItemId = 1 and m3.ActiveFlag = 'Y'
----+MediaMobileHome
--where gmd.PIN = '2106000070'


CREATE TABLE #Media
( 
  RpGuid 		uniqueidentifier
 ,MediaId int
 ,MediaUsage varchar(10) --select * from luitem2 where lutypeid = 313
 ,MediaDate smalldatetime
)

INSERT #Media
SELECT a.RpGuid, mc.MediaId, 'Accy', am.MediaDate
FROM AccyMedia am INNER JOIN Media_cmpt mc ON am.AccyMedGuid = mc.TblRecGuid
                  INNER JOIN Accy a ON am.AccyGuid = a.AccyGuid
                  INNER JOIN #RealProp r ON a.RpGuid = r.RpGuid
WHERE am.ImageTypeItemId = 1 AND am.ActiveFlag = 'Y'                  

INSERT #Media
SELECT r.RpGuid, mc.MediaId, 'Land', lm.MediaDate
FROM LandMedia lm INNER JOIN Media_cmpt mc ON lm.LndMedGuid = mc.TblRecGuid
                  INNER JOIN #RealProp r ON lm.LndGuid = r.LndGuid
WHERE lm.ImageTypeItemId = 1 AND lm.ActiveFlag = 'Y'

INSERT #Media
SELECT cb.RpGuid, mc.MediaId, 'Bldg', cbm.MediaDate
FROM CommBldgMedia cbm INNER JOIN Media_cmpt mc ON cbm.BldgMedGuid = mc.TblRecGuid
                       INNER JOIN CommBldg cb ON cbm.BldgGuid = cb.BldgGuid
                       INNER JOIN #RealProp r ON cb.RpGuid = r.RpGuid
WHERE cbm.ImageTypeItemId = 1 AND cbm.ActiveFlag = 'Y'


INSERT #Media
SELECT mh.RpGuid, mc.MediaId, 'MH', mam.MediaDate
FROM MHAcctMedia mam INNER JOIN Media_cmpt mc ON mam.MHMedGuid = mc.TblRecGuid
                     INNER JOIN MHAcct mh ON mam.MhGuid = mh.MhGuid
                     INNER JOIN #RealProp r ON mh.RpGuid = r.RpGuid 
WHERE mam.ImageTypeItemId = 1 AND mam.ActiveFlag = 'Y'

--GET CondoValPostingDescr
Create Table #CondoUnitCounts 
(
 ComplexMajor char(6)
 ,ResUnitCount int
 ,ResPosted int
 ,ResReadyToPost int
 ,ResNoValSel int
 ,ResMfFlag0 int
 ,ResApprInTrain int
 ,ResExceedsThresh int
 ,ResHoldout int
 ,ResAcctPostManually int
 ,ResMfPostingErr int
 ,CommUnitCount int
 ,CommPosted int
 ,CommReadyToPost int
 ,CommNoValSel int
 ,CommMfFlag0 int
 ,CommApprInTrain int
 ,CommExceedsThresh int
 ,CommHoldout int
 ,CommAcctPostManually int
 ,CommMfPostingErr int 
 ,ResCondoValPostDescr varchar(100)
 ,CommCondoValPostDescr varchar(100)
 )
 
Insert #CondoUnitCounts 
 (
 ComplexMajor
 ,ResUnitCount
 ,ResPosted
 ,ResReadyToPost
 ,ResNoValSel
 ,ResMfFlag0
 ,ResApprInTrain
 ,ResExceedsThresh
 ,ResHoldout
 ,ResAcctPostManually
 ,ResMfPostingErr
 ,CommUnitCount
 ,CommPosted
 ,CommReadyToPost
 ,CommNoValSel
 ,CommMfFlag0
 ,CommApprInTrain
 ,CommExceedsThresh
 ,CommHoldout
 ,CommAcctPostManually
 ,CommMfPostingErr
 ,ResCondoValPostDescr
 ,CommCondoValPostDescr)
Select 
 Major
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,0
 ,''
 ,''   
From RealProp
Where PropType = 'K' and Minor = '0000' 

UPdate #CondoUnitCounts
SET ResUnitCount = (SELECT COUNT(*)
                     FROM RealProp r
                     WHERE r.Major = c.ComplexMajor 
                      AND r.ApplGroup = 'K' and r.Minor<>'0000')
From #CondoUnitCounts c

UPdate #CondoUnitCounts
SET CommUnitCount = (SELECT COUNT(*)
                     FROM RealProp r
                     WHERE r.Major = c.ComplexMajor 
                      AND r.ApplGroup = 'C' and r.Minor<>'0000')
From #CondoUnitCounts c

UPDATE #CondoUnitCounts
SET ResPosted = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'K'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag IN (2,5)))
                 
    ,CommPosted = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'C'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag IN (2,5)))
FROM #CondoUnitCounts c 

UPDATE #CondoUnitCounts
SET ResReadyToPost = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
            WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'K'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag = 4))
                 
    ,CommReadyToPost = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'C'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag =4))
FROM #CondoUnitCounts c                                 

UPDATE #CondoUnitCounts
SET ResMfFlag0 = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'K'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag = 0))
                 
    ,CommMfFlag0 = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'C'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag =0))
FROM #CondoUnitCounts c

UPDATE #CondoUnitCounts
SET ResMfFlag0 = ResMfFlag0 +
                    (SELECT COUNT(*)
                     FROM RealProp r
                     WHERE r.Major = c.ComplexMajor AND r.Minor <> '0000' and r.ApplGroup = 'K'
                      AND NOT EXISTS(SELECT * FROM CondoApplHist h 
                                     WHERE r.Id = h.RealPRopId AND h.RollYr = @AssmtYr + 1))
                                     
     ,CommMfFlag0  = CommMfFlag0 +
                     (SELECT COUNT(*)
                     FROM RealProp r
                     WHERE r.Major = c.ComplexMajor AND r.Minor <> '0000' and r.ApplGroup = 'C'
                      AND NOT EXISTS(SELECT * FROM CondoApplHist h 
                                     WHERE r.Id = h.RealPRopId AND h.RollYr = @AssmtYr + 1))
                                 
FROM #CondoUnitCounts c                     

UPDATE #CondoUnitCounts
SET ResApprInTrain = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'K'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0 
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag = 16))
                 
    ,CommApprInTrain = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'C'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag =16))
FROM #CondoUnitCounts c 

UPDATE #CondoUnitCounts
SET ResExceedsThresh = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'K'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag = 17))
                 
    ,CommExceedsThresh = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'C'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag =17))
FROM #CondoUnitCounts c 

UPDATE #CondoUnitCounts
SET ResHoldout = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'K'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0 
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag in (19,30,31)))
                 
    ,CommHoldout = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'C'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag in (19,30,31)))
FROM #CondoUnitCounts c 

UPDATE #CondoUnitCounts
SET ResAcctPostManually = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'K'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0 
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag = 9))
                 
    ,CommAcctPostManually = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'C'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag = 9))
FROM #CondoUnitCounts c 

UPDATE #CondoUnitCounts
SET ResMfPostingErr = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'K'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0 
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag = 6))
                 
    ,CommMfPostingErr = (SELECT COUNT(*)
                 FROM CondoApplHist h INNER JOIN RealProp r ON h.RealPropId = r.Id
                 WHERE r.Major = c.ComplexMajor and r.ApplGroup = 'C'
                   AND h.RollYr = @AssmtYr + 1 and h.SplitCode = 0
                   AND h.SelectDate = (SELECT Max(SelectDate)
                                       FROM CondoApplHist h2
                                        WHERE h2.RollYr = h.RollYr
                                         and h2.RealPRopId = h.RealPropId
                                         and h2.RevalOrMaint in ('R','M')
                                         and h.MFInterfaceFlag = 6))
FROM #CondoUnitCounts c                                                                                         
                                 
UPDATE #CondoUnitCounts
 SET ResCondoValPostDescr  = 'K/K Counts: TotUnits=' + convert(varchar(4),ResUnitCount)
                           + CASE WHEN ResPosted>0 THEN ' Posted=' + CONVERT(varchar(4),ResPosted) ELSE '' END
                           + CASE WHEN ResReadyToPost>0 THEN ' ReadyToPost=' + CONVERT(varchar(4),ResReadyToPost) ELSE '' END
                           + CASE WHEN ResMFFLag0>0 THEN ' NoValSel=' + CONVERT(varchar(4),ResMFFlag0) ELSE '' END
                           + CASE WHEN ResApprInTrain>0 THEN ' ApprInTrain=' + CONVERT(varchar(4),ResApprInTrain) ELSE '' END
                           + CASE WHEN ResExceedsThresh>0 THEN ' ExceedsThresh=' + CONVERT(varchar(4),ResExceedsThresh) ELSE '' END
                           + CASE WHEN ResHoldout>0 THEN ' Holdout=' + CONVERT(varchar(4),ResHoldout) ELSE '' END 
                           + CASE WHEN ResAcctPostManually>0 THEN ' AcctPostManually=' + CONVERT(varchar(4),ResAcctPostManually) ELSE '' END
                           + CASE WHEN ResMfPostingErr>0 THEN ' MFPostingErr=' + CONVERT(varchar(4),ResAcctPostManually) ELSE '' END  
WHERE ResUnitCount>0

UPDATE #CondoUnitCounts
 SET CommCondoValPostDescr  = 'K/C Counts: TotUnits=' + convert(varchar(4),CommUnitCount)
                           + CASE WHEN CommPosted>0 THEN ' Posted=' + CONVERT(varchar(4),CommPosted) ELSE '' END
                           + CASE WHEN CommReadyToPost>0 THEN ' ReadyToPost=' + CONVERT(varchar(4),CommReadyToPost) ELSE '' END
                           + CASE WHEN CommMFFLag0>0 THEN ' NoValSel=' + CONVERT(varchar(4),CommMFFlag0) ELSE '' END
                           + CASE WHEN CommApprInTrain>0 THEN ' ApprInTrain=' + CONVERT(varchar(4),CommApprInTrain) ELSE '' END
                           + CASE WHEN CommExceedsThresh>0 THEN ' ExceedsThresh=' + CONVERT(varchar(4),CommExceedsThresh) ELSE '' END
                           + CASE WHEN CommHoldout>0 THEN ' Holdout=' + CONVERT(varchar(4),CommHoldout) ELSE '' END 
                           + CASE WHEN CommAcctPostManually>0 THEN ' AcctPostManually=' + CONVERT(varchar(4),CommAcctPostManually) ELSE '' END
                           + CASE WHEN CommMfPostingErr>0 THEN ' MFPostingErr=' + CONVERT(varchar(4),CommAcctPostManually) ELSE '' END  
WHERE CommUnitCount>0

UPDATE #RealProp
SET TaxpayerName = rpa.TaxpayerName
FROM RealPropAcct rpa INNER JOIN #RealProp r on rpa.RpGuid = r.RpGuid
WHERE rpa.SplitCode = '0'


CREATE TABLE #MostRecentAvailableTaxes
(
 AcctNbr char(12)
,PIN char(10)
,RpGuid uniqueidentifier  
,BillYr int
,LevyCode char(4)
,SrCitizenFlag char(1)
,TaxValReason char(2)
,[Type] char(1) --R (regular) or F (senior)
,RegularRate decimal(7,5)
,SrCitizenRate decimal(7,5)
,TotalLevy decimal(7,5)
,AssessTotVal int
,AssessValAdjForSeniorCalc int
,PropTax decimal(29,2)
)


CREATE TABLE #SecondMostRecentAvailableTaxes
(
 AcctNbr char(12)
,PIN char(10)
,RpGuid uniqueidentifier  
,BillYr int
,LevyCode char(4)
,SrCitizenFlag char(1)
,TaxValReason char(2)
,Type char(1) --R (regular) or F (senior)
,RegularRate decimal(7,5)--for % voter calc
,SrCitizenRate decimal(7,5)--for % voter calc
,TotalLevy decimal(7,5)
,AssessTotVal int
,AssessValAdjForSeniorCalc int
,PropTax decimal(29,2)
)

INSERT #MostRecentAvailableTaxes
SELECT 
 tar.AcctNbr
,rp.PIN 
,rp.RpGuid   
,tar.BillYr 
,tar.LevyCode 
,tar.SrCitizenFlag 
,tar.TaxValReason 
,d.[Type] --R (regular) or F (senior)
,lcy.RegularRate 
,lcy.SrCitizenRate 
,d.TotalLevy 
,AssessTotVal  = tar.LandVal + tar.ImpsVal
,AssessValAdjForSeniorCalc =
               CASE 
                 --tar.SrCitizenFlag = 'S'  All years and values handled the same
                 WHEN tar.SrCitizenFlag IN('S','') THEN (LandVal+ImpsVal)   --if it is SrCit then will get Senior rate; if it is not will get reg rate, via join below  
                   
                 --tar.SrCitizenFlag = 'P' partial Years and values handled differently
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr <= 1998 AND (LandVal+ImpsVal) * .35 <= 30000 THEN ((LandVal+ImpsVal) - 30000) 
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr <= 1998 AND (LandVal+ImpsVal) * .35 >  30000 AND (LandVal+ImpsVal) * .35 < 50000 THEN (LandVal+ImpsVal) * .65 
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr <= 1998 AND (LandVal+ImpsVal) * .35 >= 50000 THEN ((LandVal+ImpsVal) - 50000) 

                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 1999 AND tar.BillYr <= 2004 AND (LandVal+ImpsVal) * .35 <= 40000 THEN ((LandVal+ImpsVal) - 40000)  
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 1999 AND tar.BillYr <= 2004 AND (LandVal+ImpsVal) * .35  > 40000 AND (LandVal+ImpsVal) * .35 < 60000 THEN (LandVal+ImpsVal) * .65  
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 1999 AND tar.BillYr <= 2004 AND (LandVal+ImpsVal) * .35 >= 60000 THEN ((LandVal+ImpsVal) - 60000) 

                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 2005 AND (LandVal+ImpsVal) * .35 <= 50000 THEN ((LandVal+ImpsVal) - 50000)  
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 2005 AND (LandVal+ImpsVal) * .35 >  50000 AND (LandVal+ImpsVal) * .35 < 70000 THEN (LandVal+ImpsVal) * .65  
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 2005 AND (LandVal+ImpsVal) * .35 >= 70000 THEN ((LandVal+ImpsVal) - 70000)     

                 --tar.SrCitizenFlag = 'F' full Years and values handled differently  
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr <= 1998                        AND (LandVal+ImpsVal) * .5 <= 34000 THEN ((LandVal+ImpsVal) - 34000) 
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr <= 1998                        AND (LandVal+ImpsVal) * .5 >  34000 THEN (LandVal+ImpsVal) * .5  --50% is taxable
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr >= 1999 AND tar.BillYr <= 2004 AND (LandVal+ImpsVal) * .6 <= 50000 THEN ((LandVal+ImpsVal) - 50000) 
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr >= 1999 AND tar.BillYr <= 2004 AND (LandVal+ImpsVal) * .6 >  50000 THEN (LandVal+ImpsVal) * .4  --40% is taxable  
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr >= 2005                        AND (LandVal+ImpsVal) * .6 <= 60000 THEN ((LandVal+ImpsVal) - 60000)  
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr >= 2005                        AND (LandVal+ImpsVal) * .6 >  60000 THEN (LandVal+ImpsVal) * .4  --40% is taxable   
               END
,PropTax = 0
FROM #RealProp rp
INNER JOIN RealPropAcct acc ON acc.RpGuid = rp.RpGuid
INNER JOIN TaxAcctReceivable tar ON tar.AcctNbr = acc.AcctNbr
INNER JOIN LevyCodeYr lcy ON lcy.BillYr = tar.BillYr AND lcy.LevyCode = tar.LevyCode
INNER JOIN LevyCodeDistYr lcdy ON lcdy.BillYr = tar.BillYr AND lcdy.LevyCode = tar.LevyCode
INNER JOIN District di ON di.DistrictId = lcdy.DistrictId
INNER JOIN LevyRateDistribution d ON d.BillYr = tar.BillYr AND d.Levy = tar.LevyCode 
AND d.[Type] = CASE 
                 WHEN ISNULL(tar.TaxValReason,'') = 'FS' THEN 'F'
                 ELSE 'R'
               END
WHERE tar.OmitYr = 0 AND tar.ReceivableType = 'R' 
AND tar.BillYr = @MostRecentBillYrWithTaxesAvailable







INSERT #SecondMostRecentAvailableTaxes
SELECT 
 tar.AcctNbr
,rp.PIN 
,rp.RpGuid   
,tar.BillYr 
,tar.LevyCode 
,tar.SrCitizenFlag 
,tar.TaxValReason 
,d.[Type] --R (regular) or F (senior)
,lcy.RegularRate 
,lcy.SrCitizenRate 
,d.TotalLevy 
,AssessTotVal  = tar.LandVal + tar.ImpsVal
,AssessValAdjForSeniorCalc =
               CASE 
                 --tar.SrCitizenFlag = 'S'  All years and values handled the same
                 WHEN tar.SrCitizenFlag IN('S','') THEN (LandVal+ImpsVal)   --if it is SrCit then will get Senior rate; if it is not will get reg rate, via join below  
                   
                 --tar.SrCitizenFlag = 'P' partial Years and values handled differently
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr <= 1998 AND (LandVal+ImpsVal) * .35 <= 30000 THEN ((LandVal+ImpsVal) - 30000) 
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr <= 1998 AND (LandVal+ImpsVal) * .35 >  30000 AND (LandVal+ImpsVal) * .35 < 50000 THEN (LandVal+ImpsVal) * .65 
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr <= 1998 AND (LandVal+ImpsVal) * .35 >= 50000 THEN ((LandVal+ImpsVal) - 50000) 

                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 1999 AND tar.BillYr <= 2004 AND (LandVal+ImpsVal) * .35 <= 40000 THEN ((LandVal+ImpsVal) - 40000)  
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 1999 AND tar.BillYr <= 2004 AND (LandVal+ImpsVal) * .35  > 40000 AND (LandVal+ImpsVal) * .35 < 60000 THEN (LandVal+ImpsVal) * .65  
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 1999 AND tar.BillYr <= 2004 AND (LandVal+ImpsVal) * .35 >= 60000 THEN ((LandVal+ImpsVal) - 60000) 

                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 2005 AND (LandVal+ImpsVal) * .35 <= 50000 THEN ((LandVal+ImpsVal) - 50000)  
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 2005 AND (LandVal+ImpsVal) * .35 >  50000 AND (LandVal+ImpsVal) * .35 < 70000 THEN (LandVal+ImpsVal) * .65  
                 WHEN tar.SrCitizenFlag = 'P' AND tar.BillYr >= 2005 AND (LandVal+ImpsVal) * .35 >= 70000 THEN ((LandVal+ImpsVal) - 70000)     

                 --tar.SrCitizenFlag = 'F' full Years and values handled differently  
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr <= 1998                        AND (LandVal+ImpsVal) * .5 <= 34000 THEN ((LandVal+ImpsVal) - 34000) 
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr <= 1998                        AND (LandVal+ImpsVal) * .5 >  34000 THEN (LandVal+ImpsVal) * .5  --50% is taxable
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr >= 1999 AND tar.BillYr <= 2004 AND (LandVal+ImpsVal) * .6 <= 50000 THEN ((LandVal+ImpsVal) - 50000) 
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr >= 1999 AND tar.BillYr <= 2004 AND (LandVal+ImpsVal) * .6 >  50000 THEN (LandVal+ImpsVal) * .4  --40% is taxable  
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr >= 2005                        AND (LandVal+ImpsVal) * .6 <= 60000 THEN ((LandVal+ImpsVal) - 60000)  
                 WHEN tar.SrCitizenFlag = 'F' AND tar.BillYr >= 2005                        AND (LandVal+ImpsVal) * .6 >  60000 THEN (LandVal+ImpsVal) * .4  --40% is taxable   
               END
,PropTax = 0
FROM #RealProp rp
INNER JOIN RealPropAcct acc ON acc.RpGuid = rp.RpGuid
INNER JOIN TaxAcctReceivable tar ON tar.AcctNbr = acc.AcctNbr
INNER JOIN LevyCodeYr lcy ON lcy.BillYr = tar.BillYr AND lcy.LevyCode = tar.LevyCode
INNER JOIN LevyCodeDistYr lcdy ON lcdy.BillYr = tar.BillYr AND lcdy.LevyCode = tar.LevyCode
INNER JOIN District di ON di.DistrictId = lcdy.DistrictId
INNER JOIN LevyRateDistribution d ON d.BillYr = tar.BillYr AND d.Levy = tar.LevyCode 
AND d.[Type] = CASE 
                 WHEN ISNULL(tar.TaxValReason,'') = 'FS' THEN 'F'
                 ELSE 'R'
               END
WHERE tar.OmitYr = 0 AND tar.ReceivableType = 'R' 
AND tar.BillYr = @SecondMostRecentBillYrWithTaxesAvailable



UPDATE #MostRecentAvailableTaxes
SET PropTax =  AssessValAdjForSeniorCalc * CONVERT(decimal(12,2), TotalLevy) /1000


UPDATE #SecondMostRecentAvailableTaxes
SET PropTax = AssessValAdjForSeniorCalc * CONVERT(decimal(12,2),TotalLevy) /1000


----start debug
--print 'select * from #MostRecentAvailableTaxes'
--select * from #MostRecentAvailableTaxes  

--print '#SecondMostRecentAvailableTaxes'
--select * from #SecondMostRecentAvailableTaxes 

--return(0)
----end debug

--FINAL RESULT
SELECT
  Major  
 ,Minor 
 ,rp.PropName
 ,rp.TaxPayerName 
 ,TaxStatus
 ,AddrLine  
 ,DistrictName = (select d.DistrictName 
                  from District d inner join Zoning z ON z.DistrictId = d.DistrictId 
                                  inner join Land l on l.CurrentZoning = z.ZoneId 
                  where l.LndGuid = gmd.LndGuid)			
 ,CmlApplDist = ApplDistrict
 ,GeoArea 
 ,GeoNbhd 
 --,GeoAreaNbhd 
 --,GeoAreaId   
 ,'SpecArea' = CASE WHEN SpecArea = '' THEN '0' ELSE SpecArea END
 ,'SpecNbhd' = CASE WHEN SpecNbhd = '' THEN '0' ELSE SpecNbhd END 
 --,SpecAreaNbhd  
 ,PropType 
 ,ApplGroup
 ,Bookmark = ISNULL(REVERSE(SUBSTRING(REVERSE((
                 select LUItemShortDesc + '; '  from LUItem2 lui INNER JOIN Bookmark b ON lui.LUTypeId = b.BookmarkTypeId AND lui.LUItemId = b.BookmarkTypeItemId
                                                       where b.RpGuid = gmd.RpGuid AND b.StatusItemId = 0
													   Order By b.BookMarkTypeItemId
													   For XML PATH('')
													   )),2,8000)),'')
                                              
 
 ,LandInspectedDate = isnull((select InspectedDate from Inspection insp where insp.RpGuid = gmd.RpGuid and InspectionTypeItemId = 1 and insp.AssmtYr = @AssmtYr),'') 
 
 ,ImpsInspectedDate = isnull((select InspectedDate from Inspection insp where insp.RpGuid = gmd.RpGuid and InspectionTypeItemId = 2 and insp.AssmtYr = @AssmtYr),'')

 ,BothInspectedDate = isnull((select InspectedDate from Inspection insp where insp.RpGuid = gmd.RpGuid and InspectionTypeItemId = 3 and insp.AssmtYr = @AssmtYr),'')                
                                                    
 ,Qtr = rp.QuarterSection  
 ,Sec = rp.Section 
 ,Twn = rp.Township 
 ,Rng = rp.[Range] 
 ,rp.Notes
 
 
--Estimates
 ,rp.Income	
 ,rp.RCN				
 ,rp.RCNLD		
  
--Posting and values
                     
 ,LandVal
 ,ImpsVal
 ,TotVal
 ,'TotVal/NRA' = CASE
                 WHEN CmlNetSqFtAllBldg > 0 THEN TotVal/CmlNetSqFtAllBldg
                 ELSE 0 END  --Added 6/4/12 Don G
 ,PrevLandVal
 ,PrevImpsVal
 ,PrevTotVal 
 ,PcntChgLand
 ,PcntChgImps
 ,PcntChgTotal
 ,NewConstrVal 
 ,PrevNewConstrVal
 
 ,'PostingStatus' = CASE 
                      WHEN PropType <> 'K' THEN convert(varchar(2),PostingStatus)
                      WHEN PropType =  'K' THEN ''
                    END
 ,PostingStatusDescr
 ,CondoValPostDescr = ISNULL((SELECT LTRIM(RTRIM(ResCondoValPostDescr + ' ' + CommCondoValPostDescr))
                       FROM #CondoUnitCounts c WHERE c.ComplexMajor = gmd.Major),'')
 
 --Land
 ,SqFtLot = CASE  
              WHEN FrozSqFtLot > 0 THEN FrozSqFtLot 
              ELSE SqFtLot
            END 
 ,BaseLandVal            
 ,BaseLandValSqFt
 ,BLVSqFtCalc
 ,PrevLandValSqFt 
 ,PcntBLVChg = CASE 
               WHEN BaseLandVal > 0 AND PrevLandVal > 0  AND BaseLandValTaxYr = @AssmtYr + 1 
                  THEN 100 * (( convert(decimal(20,2),BaseLandVal)/convert(decimal(20,2),PrevLandVal)-1) ) 
                  ELSE 0
               END
 ,BaseLandValTaxYr = CONVERT(CHAR(4),BaseLandValTaxYr)
 ,BaseLandValDate 
 ,CurrentZoning                                 
 ,PresentUse as PresentUse 
 ,LandProblemDescr = LandProbDescrPart1 + LandProbDescrPart2                
 ,ViewDescr
 ,WfntFootage 
 ,WfntBank 
 
 
  --Imps
 ,CmlPredominantUse
 ,CmlNbrImps
 ,CmlNetSqFtAllBldg
 ,CmlGrossSqFtAllBldg
 ,CmlBldgQual 
 ,CmlYrBuilt
 ,CmlEffYr
 ,CmlNbrUnits
 ,CmlPcntCompl
 
  --Accy
  --see TODO
 --,NbrCmlAccys
 --,AccyDescrAll
 ,a.SumAccyValue  
 ,a.AccyTypeDescr
 
 ,'PicCount' = (select count(*) from #Media m where m.RpGuid = gmd.RpGuid) 
 ,'MaxPicDate' =  (select max(MediaDate) from #Media m where m.RpGuid = gmd.RpGuid) 

 --Sales 
 ,gmd.SalePrice   
 ,gmd.VerifiedPrice 
 ,gmd.SaleDate
 ,gmd.ExciseTaxNbr
 ,gmd.NbrPclsInSale
 ,VerifAtMkt     = CASE
                    WHEN  gmd.SalePrice + gmd.VerifiedPrice > 0 THEN gmd.VerifAtMkt
                    ELSE ''
                  END   
                  
,(SELECT AssmtEntityId FROM AssmtEntity ae INNER JOIN SaleVerif sv ON ae.AeGuid = sv.VerifiedByGuid
                       WHERE sv.SaleGuid = s.SaleGuid) as VerifiedBy
,(SELECT VerifDate FROM SaleVerif sv WHERE sv.SaleGuid = s.SaleGuid)  as VerifDate
,(SELECT AssmtEntityId FROM AssmtEntity ae WHERE ae.AeGuid = s.IdentifiedByGuid) as IdentifiedBy 
,s.IdentifiedDate      
,SaleWarnings = sw.Warnings
 --,SaleWarnings = CASE 
 --                  WHEN  SalePrice + VerifiedPrice > 0 AND EXISTS (select * from realpropsalecode rpsc where rpsc.SaleId = gmd.SaleId and rpsc.LUTypeId = 7) THEN 'Y'
 --                  ELSE ''
 --                END
 ,SaleTypeRP    = CASE
                    WHEN  gmd.SalePrice + gmd.VerifiedPrice > 0 THEN SaleTypeRP      
                    ELSE ''
                  END
 ,FrozSaleSqFtLot = FrozSqFtLot
 ,FrozSaleCmlPredomUse = CASE  
                           WHEN   gmd.SalePrice + gmd.VerifiedPrice > 0 THEN gmd.FrozPredominantUse 
                           ELSE ''
                         END
 ,FrozSaleCmlNetSqFt = FrozBldgNetSqFt
 ,AVSP
 ,PrevAVSP
 ,SpSqFtLnd
 ,SpNetSqFtImps
 ,SpUnit
                    
      

--Permits
 ,PermitCountIncompl  AS IncomplNbrPrmts 
 ,PermitDateRngIncompl  AS IncomplPrmtDateRng  --
 ,PermitTypesIncompl  AS IncomplPrmtTypes                   
 ,LastPermitStatusIncompl   AS IncomplPrmtStatus   
 ,PermitValsTotIncompl  AS IncomplSumPrmtVals 
 
 
 ,PermitCountComplCurCycle AS ComplNbrPrmts
 ,PermitDateRngComplCurCycle AS ComplPrmtDateRng  
 ,PermitTypesComplCurCycle  AS ComplPrmtTypes   
 ,PermitStatusDateRngComplCurCycle AS ComplPrmtStatus  -- 
 ,PermitValsTotComplCurCycle AS ComplSumPrmtVals 





	--Appeals
 ,AppealNbr = ISNULL(ap.AppealNbr,'  ')
 ,BillYr = ISNULL(ap.BillYr,'  ')
 ,ReviewType = ISNULL(ap.ReviewType,'  ')
 ,RespAppr = ISNULL(ap.RespAppr,'  ')
 ,AppealedLand = ap.AppealedLandVal		--ISNULL(a.AppealedLandVal,0),
 ,AppealedImps = ap.AppealedImpsVal			--ISNULL(a.AppealedImpsVal,0),
 ,AsrRecOrStipLand = ap.AsrRecOrStipLandVal		--ISNULL(a.AsrRecOrStipLandVal,0),
 ,AsrRecOrStipImps = ap.AsrRecOrStipImpsVal 	--ISNULL(a.AsrRecOrStipImpsVal,0),
 ,FinalLand = ap.FinalLandVal				--ISNULL(a.FinalLandVal,0), 
 ,FinalImps = ap.FinalImpsVal				--ISNULL(a.FinalImpsVal,0), 

--Reviews
 ,ReviewTypeIncompl   
 ,ReviewTypeIncomplDetails  
 
--PersProp
 ,PP_ActiveAcct
 ,PP_M1
 ,PP_LHI
 ,PP_MH_Count
 ,PP_FH_Count

 ,(SELECT Lat FROM LatLonPin llp WHERE rp.PIN = llp.PIN) as GpsLatitude
 ,(SELECT Lon FROM LatLonPin llp WHERE rp.PIN = llp.PIN) as GpsLongitude
 

 --LEFT OFF 
-- Current Levy Code
--Current Bill Yr w/ Taxes
--Current Levy Rate
--Current Prop Tax
--Current Tax Val Reason

--Previous Levy Code
--Previous Bill Yr w/ Taxes
--Previous Levy Rate
--Previous Prop Tax
--Previous Tax Val Reason



 ,[CurrentBillYrWithTaxes] = convert(char(4),t1.BillYr)
 ,[CurrentLevyCode] = t1.LevyCode
 ,[CurrentTotalLevy] = t1.TotalLevy
 ,[CurrentPropTax] = t1.PropTax
 ,[CurrentTaxValReason] = t1.TaxValReason  --moved this to end of this group 12/3/2108 per email request

 ,[PreviousBillYrWithTaxes] = convert(char(4),t2.BillYr)
 ,[PreviousLevyCode] = t2.LevyCode
 ,[PreviousTotalLevy] = t2.TotalLevy
 ,[PreviousPropTax] = t2.PropTax
 ,[PreviousTaxValReason] = t2.TaxValReason --moved this to end of this group 12/3/2108 per email request

 ,rp.Pin
 ,rp.MapPin
 ,rp.RecId

FROM GisMapData gmd
INNER JOIN #RealProp rp ON rp.RpGuid = gmd.RpGuid
INNER JOIN #MostRecentAvailableTaxes t1 ON t1.RpGuid = gmd.RpGuid
INNER JOIN #SecondMostRecentAvailableTaxes t2 ON t2.RpGuid = gmd.RpGuid
LEFT JOIN #Accy a ON rp.RpGuid = a.RpGuid
LEFT JOIN Sale s ON gmd.SaleGuid = s.SaleGuid 
LEFT JOIN #SaleWarnings sw ON gmd.SaleGuid = sw.SaleGuid 
LEFT JOIN #Appeals ap ON ap.RpGuid = gmd.RpGuid
WHERE t1.AcctNbr = t2.AcctNbr
ORDER BY 
 rp.RecId


RETURN(@@ERROR)
END


