
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_Parcels')
	DROP PROCEDURE [cus].[SRCH_R_Parcels]  
GO
CREATE PROCEDURE [cus].[SRCH_R_Parcels]  @Parcels as nvarchar(1000)

AS BEGIN
/*
Author: Jairo Barquero
Date Created:  10/23/2020
Description:    This SP get a list of Parcels by Status

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/

SET DEADLOCK_PRIORITY HIGH
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

 
DECLARE @Error		Int
DECLARE @RowCount	Int

DECLARE @NumSPS int
       ,@Cntr int
       ,@SPSPin char(10)
       ,@NumParcels int
       ,@Major char(6)
       ,@Minor char(4)

--IF OBJECT_ID('Tempdb..#PropType') IS NOT NULL
--	DROP TABLE Tempdb..#PropType;

CREATE TABLE #PropType
(
ptas_propertyTypeId UNIQUEIDENTIFIER PRIMARY KEY ,
PropType NVARCHAR(1)
)
INSERT INTO #PropType
SELECT  ptas_propertyTypeId       
		,SUBSTRING(pt.ptas_name,1,1) 
FROM dynamics.ptas_propertytype AS pt


CREATE TABLE #Input
(Parcel char(10))

INSERT INTO #Input
Select * from dynamics.fn_ParseValue(REPLACE(@Parcels,'-',''),',')

CREATE TABLE #Parcels (
  RecId int Identity(1,1)
 ,ParcelId uniqueidentifier
 ,AcctNbr char(12)
 ,Major char(6)
 ,Minor char(4)
 ,Status char(1) -- A=Active, K=Killed, S=SPS
 ,EventDate smalldatetime
 ,Pin char(10)
 ,MapPin float)


INSERT #Parcels (Major, Minor,Pin,MapPin)
SELECT SUBSTRING(Parcel,1,6),SUBSTRING(Parcel,7,4),Parcel,convert(float, Parcel)
  FROM #Input


UPDATE #Parcels
   SET ParcelId = r.ptas_parceldetailid
       ,Status = 'A'
  FROM #Parcels p INNER JOIN dynamics.ptas_ParcelDetail r (nolock) ON (p.Major = r.ptas_Major and p.Minor = r.ptas_Minor)	  
    AND (r.statecode  = 0 AND r.statuscode = 1) --This get the ACTIVE parcels. When "statecode  = 1 AND dpd.statuscode = 2" the parcel is Killed
    AND COALESCE(r.ptas_snapshottype,'') = '' 
	AND COALESCE(r.ptas_splitcode,0) = 0
   
/* SPS  range is 970000 through 972999 */
UPDATE #Parcels
   SET Status = 'S'
  FROM #Parcels p INNER JOIN [dynamics].[ptas_taxaccount] rpa ON rpa.ptas_accountnumber like p.Pin + '%'
 WHERE p.Major >= '970000'
   AND p.Major <= '972999'


-- Hairo comment, for now I commented this becasue I'm not sure where the killed parcels are
UPDATE #Parcels
   SET ParcelId = dpd.ptas_parceldetailid
       ,EventDate = dpd.modifiedon
       ,Status = 'K'
  FROM #Parcels p 
  INNER JOIN dynamics.ptas_ParcelDetail dpd ON p.Major = dpd.ptas_major and p.Minor = dpd.ptas_minor
  INNER JOIN (SELECT MAX(k.createdon)  MaxCreate
					,MAX(k.modifiedon) MaxModified
					,k.ptas_major
					,k.ptas_minor 
			   FROM [dynamics].[ptas_parceldetail] K
			  WHERE (k.statecode	= 1 and k.statuscode = 2)
			  AND COALESCE(k.ptas_splitcode,0) = 0
			  GROUP BY k.ptas_major, k.ptas_minor
			  ) kp
		ON dpd.createdon = kp.MaxCreate AND dpd.modifiedon = kp.MaxModified 
		

SELECT Status 	= 'Active'
      ,EventDt 	= ''
      ,Major	= dpd.ptas_Major
      ,Minor 	= dpd.ptas_Minor
      ,PropertyType = pt.PropType 
      ,Area	 		= CASE WHEN pt.PropType = 'R' THEN  COALESCE(dpd.ptas_resarea,'')
						   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN  dpd.ptas_commarea
						   ELSE dpd.ptas_resarea
					  END
      ,SubArea 	   	= CASE WHEN pt.PropType = 'R' THEN  COALESCE(dpd.ptas_ressubarea,'')
						   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN dpd.ptas_commsubarea
						   ELSE dpd.ptas_commsubarea
					  END				  
      ,CmlSpecArea 	= CASE WHEN pt.PropType = 'R' THEN 0
						   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN dpd.ptas_specialtyarea
					  ELSE dpd.ptas_specialtyarea
       END  
      ,CmlSpecNbhd 	= CASE WHEN pt.PropType = 'R' THEN 0
						   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000'  THEN dpd.ptas_specialtyneighborhood
					  ELSE ptas_specialtyneighborhood
       END  
      ,PresentUse 	= [dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_presentuse',	 dpl.ptas_presentuse		) 
      ,SitusAddr 	= COALESCE(dpd.ptas_address,'')--ISNULL((Select AddrLine FROM SitusAddr_V2 sa WHERE sa.RpGuid = dpd.RpGuid),'') 'SitusAddr'        
	  ,QuarterSection = COALESCE(dsm.value,'')
	  ,Section		  = pqstr.ptas_section	
	  ,Township		  = pqstr.ptas_township
	  ,Range		  = pqstr.ptas_range	
      ,PropName = COALESCE(SUBSTRING(dpd.ptas_propertyname,1,80),'') 
      ,SqFtLot 	= dpl.ptas_sqftTotal
      ,NewNote 	= ''
      ,p.RecId
      ,p.Pin
      ,MapPin	= CASE WHEN pt.PropType = 'K' then convert(float, dpd.ptas_major + '0000')
                       ELSE convert(float, dpd.ptas_major + dpd.ptas_minor)
				  END
  FROM dynamics.ptas_parceldetail dpd (nolock) 
  INNER JOIN #Parcels p 
    ON dpd.ptas_parceldetailid = p.ParcelId
 INNER JOIN dynamics.ptas_land dpl
    ON dpd._ptas_landid_value = dpl.ptas_landid  	
 INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
  LEFT JOIN [dynamics].[ptas_qstr] pqstr
    ON pqstr.[ptas_qstrid] = dpd._ptas_qstrid_value
  LEFT JOIN [dynamics].[stringmap]	dsm
    ON dsm.attributevalue = pqstr.ptas_quartersection 
   AND dsm.objecttypecode = 'ptas_qstr'
   AND dsm.attributename  = 'ptas_quartersection'	
                           
 WHERE p.Status = 'A'

UNION

SELECT Status 	   = 'SPS' 
      ,EventDt 	   = ''
      ,Major       = p.Major
      ,Minor	   = p.Minor
      ,PT          = '' 
      ,Area        = '' 
      ,Sub         = '' 
      ,CmlSpecArea = '' 
      ,CmlSpecNbhd = '' 
      ,PresentUse  = '' 
      ,SitusAddr   = '' 
      ,Q           = '' 
      ,S           = '' 
      ,T           = '' 
      ,R           = '' 
      ,PropName    = '' 
      ,SqFtLot     = 0 
      ,NewNote     = '' 
      ,p.RecId
      ,p.Pin
      ,p.MapPin 
  FROM #Parcels p
 WHERE p.Status = 'S'

UNION

SELECT Status  = 'Killed'
      ,EventDt = p.EventDate 
      ,Major   = p.Major
      ,Minor   = p.Minor
      ,PT           = '' 
      ,Area         = '' 
      ,Sub          = '' 
      ,CmlSpecArea  = '' 
      ,CmlSpecNbhd  = '' 
      ,PresentUse   = '' 
      ,SitusAddr    = '' 
      ,Q            = '' 
      ,S            = '' 
      ,T            = '' 
      ,R            = '' 
      ,PropName     = '' 
      ,SqFtLot      = 0  
      ,NewNote      = '' 
      ,p.RecId
      ,p.Pin
      ,p.MapPin 
  FROM #Parcels p
 WHERE p.Status = 'K'
 ORDER BY Major, Minor


SELECT @Error		= @@ERROR
      ,@RowCount	= @@ROWCOUNT
IF @Error		<> 0  RETURN (@Error)

RETURN(0)
 
END
 



GO


