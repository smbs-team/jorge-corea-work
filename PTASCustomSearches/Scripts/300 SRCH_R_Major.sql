
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_Major')
	DROP PROCEDURE [cus].[SRCH_R_Major]  
GO

CREATE PROCEDURE [cus].[SRCH_R_Major]   @Major char(6)
AS BEGIN
/*
Author: Jairo Barquero
Date Created:  11/12/2020
Description:    SP that pulls all records  filtered by parameter @Mayor

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

/* RealPropId, Major, Minor, QSTR returned as a recordset
** for Search; SPS returns parts of Account Nbr for Major Minor. */

/* Condos handled in separate search for now */
 
DECLARE @Error		Int
DECLARE @RowCount	Int

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

 
CREATE TABLE #Srch (
 RecId int Identity(1,1)
,ParcelId uniqueidentifier
,[Status] varchar(10)
,EventDt varchar(1)
,Major char(6)
,Minor char(4)
,PropType char(1)
,Area smallint
,SubArea smallint
,QuarterSection char(2)
,Section tinyint
,Township tinyint
,[Range] tinyint
,PropName nvarchar(max)
,Pin char(10)
,MapPin float)

/* SPS  range is 970000 through 972999 */
IF @Major >= '970000' and @Major <= '972999'
  INSERT #Srch
  SELECT 	
	NULL --ParcelId
   ,'SPS' Status
   ,'' EventDt
	,Substring(a.ptas_accountnumber,1,6) Major
	,Substring(a.ptas_accountnumber,7,4) Minor
	,' ' PT
   ,' ' Area
   ,' ' Sub
	,' ' Q
	,' ' S
	,' ' T
	,' ' R
	,' ' PropName
    ,substring(a.ptas_accountnumber,1,10)
    ,convert(float, substring(a.ptas_accountnumber,1,10))
  FROM [dynamics].[ptas_taxaccount] a
  WHERE a.ptas_accountnumber Like @Major + '%'
 
ELSE 
   INSERT #Srch
   SELECT
	ParcelId  = dpd.ptas_parceldetailid--ParcelId
    ,Status   = 'Active'
    ,EventDt  = ''
	,Major 	  = dpd.ptas_major
	,Minor 	  = dpd.ptas_minor 
	,PropType = pt.PropType

    ,Area 		= CASE WHEN pt.PropType = 'R' THEN  COALESCE(dpd.ptas_resarea,'')
					   WHEN pt.PropType = 'T' THEN  ( ISNULL((dpd.ptas_resarea), (dpd.ptas_commarea))) 
					   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN  dpd.ptas_commarea
					   ELSE dpd.ptas_commarea
				  END
    ,SubArea	= CASE WHEN pt.PropType = 'R' THEN  COALESCE(dpd.ptas_ressubarea,'')
					   WHEN pt.PropType = 'T' THEN  ( ISNULL((dpd.ptas_ressubarea), (dpd.ptas_commsubarea))) 
					   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN dpd.ptas_commsubarea
					   ELSE dpd.ptas_commsubarea
				  END	
    ,QuarterSection = COALESCE(dsm.value,'')
    ,Section		= pqstr.ptas_section	
    ,Township		= pqstr.ptas_township
    ,Range			= pqstr.ptas_range		
	,PropName = COALESCE(dpd.ptas_propertyname,'') 
    ,PIN 	  = COALESCE(dpd.ptas_major,'') + COALESCE(dpd.ptas_minor,'')
    ,MapPin   = CASE WHEN pt.PropType = 'K' THEN convert(float, dpd.ptas_major + '0000')
                     ELSE convert(float, dpd.ptas_major + dpd.ptas_minor)
				END
   FROM dynamics.ptas_parceldetail dpd
 INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
  LEFT JOIN [dynamics].[ptas_qstr] pqstr
    ON pqstr.[ptas_qstrid] = dpd._ptas_qstrid_value
  
  LEFT JOIN [dynamics].[stringmap]	dsm
    ON dsm.attributevalue = pqstr.ptas_quartersection 
   AND dsm.objecttypecode = 'ptas_qstr'
   AND dsm.attributename  = 'ptas_quartersection'
   
   WHERE dpd.ptas_major  = @Major
     AND (dpd.statecode  = 0 AND dpd.statuscode = 1) --This get the ACTIVE parcels. When "statecode  = 1 AND dpd.statuscode = 2" the parcel is Killed
     AND COALESCE(dpd.ptas_snapshottype,'') = '' 
	 AND COALESCE(dpd.ptas_splitcode,0) = 0

SELECT 
	   Major
	  ,Minor
      ,[Status]
      ,EventDt
	  ,PropType 'PT'
      ,Area
      ,SubArea
	  ,QuarterSection 'Q'
	  ,Section 'S'
	  ,Township 'T'
	  ,Range 'R'
	  ,PropName
      ,Pin
      ,MapPin 
      ,RecId
  FROM #Srch
 ORDER BY Minor

SELECT @Error		= @@ERROR
      ,@RowCount	= @@ROWCOUNT
IF @Error		<> 0  RETURN (@Error)
--IF @@RowCount = 0 RETURN 100002

RETURN(0)
 
END

GO


