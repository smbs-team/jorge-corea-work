	/*
	XXCREATE PROCEDURE [cus].[SRCH_R_CommPhysInspPclPlan]
	--10/21/2016 simplify with just the following 3
	   @Area varchar(3)
	  ,@Nbhd varchar(3) 
	  ,@AssignedAppr  varchar(50) -- Hairo comment: This must be "fullname" column from dynamics.systemuser table
	  ,@IncludeSpecLandY int  --1=y 2=n
				
	AS
	BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	/*  
	Author: Hairo Barquero  
	Date Created:  02/08/2021  
	Description:    
  
	Modifications:  
	mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]  
	*/ 

	*/

	--BEGIN ONLY FOR TEST PORPUSES  -- ONLY FOR TEST PORPUSES  -- ONLY FOR TEST PORPUSES  -- 
	DECLARE 
	   @Area varchar(3)
	  ,@Nbhd varchar(3) 
	  ,@AssignedAppr  varchar(50)  
	  ,@IncludeSpecLandY int  --1=y 2=n

	SELECT
	   @Area = '17'
	  ,@Nbhd = NULL
	  ,@AssignedAppr  = NULL -- Hairo comment: This must be "fullname" column from dynamics.systemuser table
	  ,@IncludeSpecLandY =1  --1=y 2=n



	--END ONLY FOR TEST PORPUSES  -- ONLY FOR TEST PORPUSES  -- ONLY FOR TEST PORPUSES  -- 


	DECLARE @Error	Int

	DECLARE @RPAssmtYr int
	SELECT @RPAssmtYr = 2020
	--SELECT @RPAssmtYr = RPAssmtYr FROM AssmtYr Hairo comment: this need to be anabled when the table for Assessment year is in place


	--Because the Y/N search params use the lookup, the SP call sends 1's and 0's
	--For non-Y/N params, RP2 searches returns null.  Less awkward to work with non-null, so reset


	IF @AssignedAppr = 0 SELECT @AssignedAppr = '' --The call actually returns 0
	IF @Area IS NULL SELECT @Area = ''
	IF @Nbhd IS NULL SELECT @Nbhd = ''

	DECLARE @GeoArea varchar(3) 
		   ,@GeoNbhd varchar(3) 
		   ,@SpecArea varchar(3) 
		   ,@SpecNbhd varchar(3)
       
	SELECT  @GeoArea = ''
		   ,@GeoNbhd = ''
		   ,@SpecArea = ''
		   ,@SpecNbhd = ''

	IF @Area <> '' AND CONVERT(int,@Area) < 100
	BEGIN
	  SELECT @GeoArea = @Area
	  SELECT @GeoNbhd = @Nbhd  
	END


	IF @Area <> '' AND CONVERT(int,@Area) >= 100
	BEGIN
	  SELECT @SpecArea = @Area
	  SELECT @SpecNbhd = @Nbhd  
	END

	--print 'debug'
	--select @Area, @Nbhd, @GeoArea, @GeoNbhd, @SpecArea, @SpecNbhd

	CREATE TABLE #PropType
	(
	ptas_propertyTypeId UNIQUEIDENTIFIER PRIMARY KEY ,
	PropType NVARCHAR(1)
	)
	INSERT INTO #PropType
	SELECT  ptas_propertyTypeId       
			,SUBSTRING(pt.ptas_name,1,1) 
	FROM dynamics.ptas_propertytype AS pt


	CREATE TABLE #AllCmlParcels
	(
	 RecId int Identity(1,1) 
	,Major char(6)
	,Minor char(4) 
	,PIN char(10)
	,MapPin float
	,ParcelId uniqueidentifier
	,LandId uniqueidentifier
	,GeoDistrict varchar(20) default 0  --ApplDistrict
	,SpecDistrict varchar(20) default 0  --SpecApplDistrict
	,GeoArea varchar(3) default 0
	,GeoNbhd varchar(3) default 0
	,SpecArea varchar(3) default 0
	,SpecNbhd varchar(3) default 0
	,PropType char(1)
	,ApplGroup char(1)
	,CondoUnit char(1)  default ''--C,K,blank
	,TaxStatus varchar(20) default ''
	,VacImpAccy varchar(4) default ''
	,PropName varchar(80) default ''
	,LandResponsiblity  varchar(20)
	,ImpsResponsiblity  varchar(20)
	,AssignedLandAppr varchar(100) default ''  
	,AssignedImpsAppr varchar(100) default '' 
	,Target_PI_Land_AssmtYr int default 0
	,Target_PI_Imps_AssmtYr int default 0
	,Target_PI_Both_AssmtYr int default 0
	,Last_Land_Inspect_AssmtYr int default 0
	,Last_Imps_Inspect_AssmtYr int default 0
	,Land_Insp_Date date default '1/1/1900'
	,Imps_Insp_Date date default '1/1/1900'
	,Land_Insp_By Varchar(100) default ''--char(4) default ''
	,Imps_Insp_By Varchar(100) default ''--char(4) default ''
	,Target_PI_Land_UpdatedBy varchar(40) default ''  
	,Target_PI_Imps_UpdatedBy varchar(40) default '' 
	,Target_PI_Both_UpdatedBy varchar(40) default '' 
	,Target_PI_Land_UpdateDate date default '1/1/1900'  
	,Target_PI_Imps_UpdateDate date default '1/1/1900' 
	,Target_PI_Both_UpdateDate date default '1/1/1900' 
	)

	CREATE UNIQUE NONCLUSTERED INDEX IX_ParcelId
	ON #AllCmlParcels (ParcelId)



	INSERT #AllCmlParcels
	(
	 Major 
	,Minor 
	,PIN 
	,MapPin  
	,ParcelId
	,LandId 
	,GeoDistrict
	--,SpecDistrict
	,GeoArea
	,GeoNbhd 
	,SpecArea 
	,SpecNbhd 
	,PropType 
	,ApplGroup 
	,CondoUnit 
	,TaxStatus
	--,VacImpAccy		
	,PropName
	,LandResponsiblity
	,ImpsResponsiblity
	)

	SELECT DISTINCT
	 gmd.Major 
	,gmd.Minor 
	,gmd.PIN 
	,MapPin = CONVERT(float,PIN)
	,gmd.ParcelID 
	,gmd.LndGuid 
	,gmd.ApplDistrict
	--,gmd.SpecApplDistrict Hairo comment: requires to be calculated in View GismapData
	,gmd.GeoArea
	,gmd.GeoNbhd 
	,gmd.SpecArea 
	,gmd.SpecNbhd 
	,gmd.PropType 
	,gmd.ApplGroup 
	,''
	,gmd.TaxStatus 
	--,VacImpAccy  Hairo comment: requires to be calculated in View GismapData
	,gmd.PropName
	,LandResponsiblity = 'GeoAppr'
	,ImpsResponsiblity = CASE WHEN gmd.SpecArea <> '0' THEN 'SpecAppr' ELSE 'GeoAppr' END 
	FROM dynamics.vw_GISMapData gmd
	WHERE gmd.PropType <> 'R'
	AND  gmd.ApplGroup NOT IN('T','A')
	--I don't think this actually works to keep out the GeoAreas with 0 counts:
	--AND ISNULL((select count(*) from GisMapData gmd2 where gmd2.GeoArea = gmd.GeoArea and gmd2.GeoNbhd = gmd.GeoNbhd),0)> 0
	AND ((@GeoArea = CAST(GeoArea AS INT) AND @GeoArea <>'')  OR @GeoArea = '') 
	AND ((@GeoNbhd = CAST(GeoNbhd AS INT) AND @GeoNbhd <>'')  OR @GeoNbhd = '') 
	AND ((@SpecArea = CAST(SpecArea AS INT) AND @SpecArea <>'')  OR @SpecArea = '') 
	AND ((@SpecNbhd = CAST(SpecNbhd AS INT) AND @SpecNbhd <>'')  OR @SpecNbhd = '') 

	--This is a problem.  The maint value select was for a unit, but it ended up as a complex value in RealPropApplHist: 

		--PIN = '0294200000'
		--RevalOrMaint RollYr LandVal     ImpsVal     TotalVal    NewConstrVal SelectMethod SelectReason SelectAppr SelectDate              MFInterfaceFlag MFInterfaceDate         Post Code
		-------------- ------ ----------- ----------- ----------- ------------ ------------ ------------ ---------- ----------------------- --------------- ----------------------- ---------
		--M            2016   156500      486500      643000      0            0            0                       1900-01-01 00:00:00     10              1900-01-01 00:00:00     10
		--R            2016   5935000     19728000    25663000    0            0            0                       1900-01-01 00:00:00     10              1900-01-01 00:00:00     10

	--For now, see if avoiding RevalOrMaint = 'M' for complexes works better.  Did this in GisMapData  8/31/2016

	--PART 2.  Same story with 3026049137, which is an apartment, not a condo. Turns out to be TaxStatus = Xmpt
	  --M	2016	Reconciled	10	 379000	1000	 380000	
	  --R	2016	Reconciled	10	1306100	1000	1307100	
 
	------debug
	--select top 100 * from #AllCmlParcels
	--return(0)

	----CONDO UNITS      CONDO UNITS      CONDO UNITS      CONDO UNITS 
	----select top 1 * from CondoUnit
	----select top 10 * from CondoApplHist --Need RealPropId
	----There are 165 condo unit parcels that have RpGuid in CommSpecAreaPcl.  I dont understand why, but for now will inherit spec/geo from parent.
	----select count(*)  from CommSpecAreaPcl s inner join RealProp rp on rp.RpGuid = s.RpGuid where rp.minor <> '0000' and rp.PropType = 'K' --165
	INSERT #AllCmlParcels
	(
	 Major 
	,Minor 
	,PIN 
	,MapPin  
	,ParcelId
	,LandId 
	,GeoDistrict 
	,SpecDistrict 
	,GeoArea
	,GeoNbhd 
	,SpecArea 
	,SpecNbhd 
	,PropType 
	,ApplGroup 
	,CondoUnit 
	,VacImpAccy 
	,LandResponsiblity
	,ImpsResponsiblity
	)

	SELECT DISTINCT   
	  pd.ptas_major
	,cu.ptas_minornumber-- .ptas_minor
	,PIN	= COALESCE(pd.ptas_major,'') + COALESCE(pd.ptas_minor,'')
	,ac.MapPin --complex
	,cu.ptas_condounitid--,pd.ptas_parceldetailid
	,ac.LandId
	,ac.GeoDistrict
	,ac.SpecDistrict 
	,ac.GeoArea
	,ac.GeoNbhd 
	,ac.SpecArea 
	,ac.SpecNbhd 
	,pt.PropType
	,pd.ptas_applgroup
	,pd.ptas_applgroup--CondoUni
	,ac.VacImpAccy
	,LandResponsiblity = '' --Already covered via complex.  Populate here to eliminate NULL
	,ImpsResponsiblity = CASE  --700 res or cml condo 720 MH condo, 730 floating condos 
						   WHEN ac.SpecArea = '700' AND pd.ptas_applgroup = 'K' THEN 'SpecAppr' --THEN 'ResCondoAppr'  
						   WHEN ac.SpecArea = '700' AND pd.ptas_applgroup = 'C' THEN 'GeoAppr'  
						   WHEN ac.SpecArea = '720' AND pd.ptas_applgroup = 'H' THEN 'SpecAppr' --THEN 'ResCondoAppr'  
						   WHEN ac.SpecArea = '720' AND pd.ptas_applgroup = 'C' THEN 'GeoAppr'   
						   WHEN ac.SpecArea = '730' AND pd.ptas_applgroup = 'F' THEN 'SpecAppr' --THEN 'FloatingHomeAppr'  
						   WHEN ac.SpecArea = '0'   AND pd.ptas_applgroup = 'C' THEN 'GeoAppr'                         
						   WHEN ac.SpecArea <> '0'  AND ac.SpecArea <> '700' AND  pd.ptas_applgroup = 'C' THEN 'SpecAppr'                                           
						   ELSE 'UNK' 
						 END
	FROM dynamics.ptas_parceldetail(NOLOCK) pd
	INNER JOIN #PropType pt ON pd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
	INNER JOIN dynamics.ptas_condounit(NOLOCK) cu ON cu._ptas_parcelid_value =  pd.ptas_parceldetailid
	INNER JOIN #AllCmlParcels ac ON ac.ParcelId = cu._ptas_complexid_value
	WHERE ac.PropType = 'K'
	--AND  pd.ptas_applgroup <> 'T'--AND  pd.ptas_applgroup <> 'A'
	AND  pd.ptas_applgroup = 'C'
	

	--LEFT OFF.  HOW TO HANDLE LAND?
	IF @Area <> '' AND CONVERT(int,@Area) < 100  AND @IncludeSpecLandY IN (2,0)
	BEGIN
	  DELETE #AllCmlParcels WHERE ImpsResponsiblity <> 'GeoAppr'
	END


	IF @Area <> '' AND CONVERT(int,@Area) >= 100
	BEGIN
	  DELETE #AllCmlParcels WHERE ImpsResponsiblity <> 'SpecAppr' 
	END




	--GisMapData already provided TaxStatus for non-condos.  Get for condos
	CREATE TABLE #MixedTaxStatus (ParcelId uniqueidentifier, TaxStatus varchar(20))

INSERT #MixedTaxStatus 
SELECT  ac.ParcelId
		,'Part Xmpt' 
FROM #AllCmlParcels ac
INNER JOIN dynamics.ptas_condounit(NOLOCK) cu ON ac.ParcelId = cu.ptas_condounitid
INNER JOIN dynamics.ptas_taxaccount(NOLOCK) pt ON cu.ptas_condounitid= pt._ptas_condounitid_value
INNER JOIN ptas.ptas_taxrollhistory(NOLOCK) trh ON  pt.ptas_taxaccountid = trh.ptas_taxaccountid 
WHERE trh.ptas_receivabletype = 'R'
AND trh.ptas_omityearidname = 0
AND trh.ptas_taxyearidname = @RPAssmtYr + 1
--AND trh.ptas_taxyearidname = 2020 + 1
AND EXISTS (select * from dynamics.ptas_condounit cu2 
							INNER JOIN dynamics.ptas_taxaccount(NOLOCK) pt2 ON cu2.ptas_condounitid= pt2._ptas_condounitid_value
							INNER JOIN ptas.ptas_taxrollhistory(NOLOCK) trh2 ON  pt2.ptas_taxaccountid = trh2.ptas_taxaccountid
            where cu2.ptas_condounitid = ac.ParcelId and trh2.ptas_taxaccountid <> trh.ptas_taxaccountid and trh2.ptas_TaxStat <> trh.ptas_TaxStat
            and trh2.ptas_TaxStat IN  ('X','0'))	
AND trh.ptas_TaxStat  = 'T' 
AND ac.PropType = 'K'
ORDER BY  ac.ParcelId

CREATE TABLE #XmptTaxStatus (ParcelId uniqueidentifier)

INSERT INTO #XmptTaxStatus 
SELECT ac.ParcelId
FROM #AllCmlParcels ac
INNER JOIN dynamics.ptas_condounit(NOLOCK) cu ON ac.ParcelId = cu.ptas_condounitid
INNER JOIN dynamics.ptas_taxaccount(NOLOCK) pt ON cu.ptas_condounitid= pt._ptas_condounitid_value
INNER JOIN ptas.ptas_taxrollhistory(NOLOCK) trh ON  pt.ptas_taxaccountid = trh.ptas_taxaccountid 
WHERE trh.ptas_receivabletype = 'R'
AND trh.ptas_omityearidname = 0
AND trh.ptas_taxyearidname = @RPAssmtYr + 1
--AND trh.ptas_taxyearidname = 2020 + 1
AND NOT EXISTS (select * from #MixedTaxStatus m where m.ParcelId = ac.ParcelId)
AND trh.ptas_TaxStat IN ('X','0')
AND ac.PropType = 'K'
AND ac.ParcelId = cu.ptas_condounitid


UPDATE #AllCmlParcels
SET TaxStatus = 'Xmpt' 
FROM #AllCmlParcels ac 
INNER JOIN #XmptTaxStatus xts ON ac.ParcelId = xts.ParcelId


/*
--Hairo comment: Seems like ParcelGrp data was moved to [dynamics].[ptas_economicunit] but the count of records 
				is not the same and record PgName = 'Government Owned' is not there, I already ask Regis to double 
				check if there is a problem with the data.

UPDATE #AllCmlParcels
SET TaxStatus = 'Govt Xmpt' 
FROM #AllCmlParcels ac
INNER JOIN ParcelGrpItem i ON ac.RpGuid = i.RpGuid
INNER JOIN ParcelGrp g ON i.PgGuid = g.PgGuid
WHERE g.PgName = 'Government Owned'  
AND ac.PropType = 'K'  

*/

--Get Inspection history
UPDATE #AllCmlParcels
SET Last_Land_Inspect_AssmtYr = SUBSTRING(ih.ptas_Name,1,4)
   ,Land_Insp_Date = ih.ptas_inspectiondate
   ,Land_Insp_By = su.fullname
FROM #AllCmlParcels ac
INNER JOIN ptas.ptas_inspectionhistory(NOLOCK) ih ON ac.ParcelId = ih.ptas_parcelid
INNER JOIN dynamics.systemuser su ON ih.ptas_inspectedbyid = su.systemuserid
WHERE ih.ptas_inspectiontype IN ('Land','Both')
AND ih.ptas_inspectiondate = (SELECT MAX(ih2.ptas_inspectiondate) FROM ptas.ptas_inspectionhistory(NOLOCK) ih2
							  WHERE ih2.ptas_parcelid = ih.ptas_parcelid AND ih2.ptas_inspectiontype IN ('Land','Both')
							  )

UPDATE #AllCmlParcels
SET Last_Imps_Inspect_AssmtYr = SUBSTRING(ih.ptas_Name,1,4)
   ,Imps_Insp_Date = ih.ptas_inspectiondate
   ,Imps_Insp_By = su.fullname
FROM #AllCmlParcels ac
INNER JOIN ptas.ptas_inspectionhistory(NOLOCK) ih ON ac.ParcelId = ih.ptas_parcelid
INNER JOIN dynamics.systemuser su ON ih.ptas_inspectedbyid = su.systemuserid
WHERE ih.ptas_inspectiontype IN ('Imps','Both')
AND ih.ptas_inspectiondate = (SELECT MAX(ih2.ptas_inspectiondate) FROM ptas.ptas_inspectionhistory(NOLOCK) ih2
							  WHERE ih2.ptas_parcelid = ih.ptas_parcelid AND ih2.ptas_inspectiontype IN ('Imps','Both')
							  )

/* 
Hairo Comment: In order to continue with these UPDATES we need table InspectionLog

 --InspectionLog table is new as of 9/15/2016 and will offer a straightforward way to capture past inspections     
--select * from InspectionLog
--In most cases, land inspection will have been overridden with later imps inspection.  If so, get prior land inspection from here
UPDATE #AllCmlParcels
SET Last_Land_Inspect_AssmtYr = i.AssmtYr
   ,Land_Insp_Date = i.InspectedDate
   ,Land_Insp_By = (select ae.AssmtEntityId from AssmtEntity ae where i.InspectedByGuid = ae.AeGuid) 
  FROM InspectionLog i
  INNER JOIN #AllCmlParcels ac ON ac.RpGuid  = i.RpGuid
  WHERE i.InspectionTypeItemId IN (1,3)--land or both  
  AND i.InspectedDate  = (select max(InspectedDate) from Inspection i2 where i2.RpGuid  = i.RpGuid and i2.InspectionTypeItemId IN (1,3))
  AND Last_Land_Inspect_AssmtYr = 0

--Not sure if there will be many cases of prior imps inspection.  
UPDATE #AllCmlParcels
SET Last_Imps_Inspect_AssmtYr = i.AssmtYr
   ,Imps_Insp_Date = i.InspectedDate
   ,Imps_Insp_By = (select ae.AssmtEntityId from AssmtEntity ae where i.InspectedByGuid = ae.AeGuid) 
  FROM InspectionLog i
  INNER JOIN #AllCmlParcels ac ON ac.RpGuid  = i.RpGuid
  WHERE i.InspectionTypeItemId IN (1,3)--Imps or both  
  AND i.InspectedDate  = (select max(InspectedDate) from Inspection i2 where i2.RpGuid  = i.RpGuid and i2.InspectionTypeItemId IN (1,3))
  AND Last_Imps_Inspect_AssmtYr = 0
  
*/
 
UPDATE #AllCmlParcels
SET AssignedLandAppr = su.fullname
FROM #AllCmlParcels ac
INNER JOIN dynamics.ptas_geoarea pg 
			INNER JOIN [dynamics].[ptas_geoneighborhood] gn ON pg.ptas_geoareaid = gn._ptas_geoareaid_value
ON pg.ptas_areanumber =  ac.GeoArea AND gn.ptas_nbhdnumber = convert(int,ac.GeoNbhd)
INNER JOIN dynamics.systemuser su ON pg._ptas_appraiserid_value = su.systemuserid
WHERE pg._ptas_appraiserid_value IS NOT NULL


UPDATE #AllCmlParcels
SET AssignedImpsAppr = su.fullname
FROM #AllCmlParcels ac
INNER JOIN dynamics.ptas_geoarea pg 
			INNER JOIN [dynamics].[ptas_geoneighborhood] gn ON pg.ptas_geoareaid = gn._ptas_geoareaid_value
ON pg.ptas_areanumber =  ac.GeoArea AND gn.ptas_nbhdnumber = convert(int,ac.GeoNbhd)
INNER JOIN dynamics.systemuser su ON pg._ptas_appraiserid_value = su.systemuserid
WHERE pg._ptas_appraiserid_value IS NOT NULL
AND (ac.SpecArea = '0' OR (ac.SpecArea = '700' AND ac.ApplGroup = 'C'))



UPDATE #AllCmlParcels
SET AssignedImpsAppr = su.fullname
FROM #AllCmlParcels ac
INNER JOIN dynamics.ptas_geoarea pg 
			INNER JOIN [dynamics].[ptas_geoneighborhood] gn ON pg.ptas_geoareaid = gn._ptas_geoareaid_value
ON pg.ptas_areanumber =  ac.GeoArea AND gn.ptas_nbhdnumber = convert(int,ac.GeoNbhd)
INNER JOIN dynamics.systemuser su ON pg._ptas_appraiserid_value = su.systemuserid
WHERE pg._ptas_appraiserid_value IS NOT NULL
AND ac.SpecArea <> '0'


--Delete to end up with only @AssignedAppr. TODO, not efficient would be better to cover on initial insert.
IF @AssignedAppr <> '' 
BEGIN

  DELETE #AllCmlParcels WHERE AssignedLandAppr <> @AssignedAppr AND AssignedImpsAppr <> @AssignedAppr AND @IncludeSpecLandY = 1 --1=y 
  DELETE #AllCmlParcels WHERE AssignedImpsAppr <> @AssignedAppr AND @IncludeSpecLandY IN (0,2) --2=y   

END



/*
Hairo comment: These UPDATES are not possible to do, the table PhysInspPclPlan in RP is empty and there only 2 records
			   in Luitem2, for that reason this codfe is disabled. 
 
UPDATE #AllCmlParcels
SET
 Target_PI_Land_AssmtYr = ISNULL((select lui.LuItemId from LuItem2 lui where convert(int,lui.LUItemShortDesc) = p.AssmtYr and lui.LuTypeId = 406),0)
,Target_PI_Land_UpdatedBy = ISNULL((select AssmtEntityId from AssmtEntity ae where ae.AeGuid = p.UpdatedByGuid),'')
,Target_PI_Land_UpdateDate = ISNULL(p.UpdateDate,'1/1/1900')
FROM #AllCmlParcels ac
INNER JOIN PhysInspPclPlan p ON p.RpGuid = ac.RpGuid
WHERE p.InspectionTypeItemId = 1


UPDATE #AllCmlParcels
SET
 Target_PI_Imps_AssmtYr = ISNULL((select lui.LuItemId from LuItem2 lui where convert(int,lui.LUItemShortDesc) = p.AssmtYr and lui.LuTypeId = 406),0)
,Target_PI_Imps_UpdatedBy = ISNULL((select AssmtEntityId from AssmtEntity ae where ae.AeGuid = p.UpdatedByGuid),'')
,Target_PI_Imps_UpdateDate = ISNULL(p.UpdateDate,'1/1/1900')
FROM #AllCmlParcels ac
INNER JOIN PhysInspPclPlan p ON p.RpGuid = ac.RpGuid
WHERE p.InspectionTypeItemId = 2

UPDATE #AllCmlParcels
SET
 Target_PI_Both_AssmtYr = ISNULL((select lui.LuItemId from LuItem2 lui where convert(int,lui.LUItemShortDesc) = p.AssmtYr and lui.LuTypeId = 406),0)
,Target_PI_Both_UpdatedBy = ISNULL((select AssmtEntityId from AssmtEntity ae where ae.AeGuid = p.UpdatedByGuid),'')
,Target_PI_Both_UpdateDate = ISNULL(p.UpdateDate,'1/1/1900')
FROM #AllCmlParcels ac
INNER JOIN PhysInspPclPlan p ON p.RpGuid = ac.RpGuid
WHERE p.InspectionTypeItemId = 3

*/


  
SELECT
 RecId
,Major
,Minor
,PIN
,MapPin
,GeoDistrict
--,SpecDistrict --Hairo comment: this came from GisMapData View, still not calculated
,GeoArea
,GeoNbhd
,SpecArea
,SpecNbhd
,PropType
,ApplGroup
,CondoUnit
,TaxStatus
--,VacImpAccy	--Hairo comment: this came from GisMapData View, still not calculated
,PropName
,LandResponsiblity
,ImpsResponsiblity
,AssignedLandAppr
,AssignedImpsAppr
,Target_PI_Land_AssmtYr
,Target_PI_Imps_AssmtYr
,Target_PI_Both_AssmtYr
,Last_Land_Inspect_AssmtYr
,Last_Imps_Inspect_AssmtYr
,Land_Insp_Date
,Imps_Insp_Date
,Land_Insp_By
,Imps_Insp_By
,Target_PI_Land_UpdatedBy
,Target_PI_Imps_UpdatedBy
,Target_PI_Both_UpdatedBy
,Target_PI_Land_UpdateDate
,Target_PI_Imps_UpdateDate
,Target_PI_Both_UpdateDate

FROM #AllCmlParcels
ORDER BY
 Major 
,Minor




RETURN(0)

ErrorHandler:
RETURN (@Error)


END



