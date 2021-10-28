
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_FolioSearch')
	DROP PROCEDURE [cus].[SRCH_R_FolioSearch]  
GO

CREATE PROCEDURE [cus].[SRCH_R_FolioSearch]   @Folio		varchar(7)
AS BEGIN
/*
Author: Jairo Barquero
Date Created:  11/12/2020
Description:    SP that pulls all records  filtered by parameter @Folio

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

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
 Major char(6)
,Minor char(4)
,Area smallint
,SubArea smallint
,PropType char(1)
,[Address] varchar(100)
,PlatBlock char(7) Null
,PlatLot char(14) Null
,ApplGroup char(1)
,Pin char(10)
,MapPin float
,RecId int Identity(1,1)
)




INSERT #Srch
SELECT  Major 		= dpd.ptas_major 
 	   ,Minor 		= dpd.ptas_minor   
	   ,Area 		= CASE WHEN pt.PropType = 'R' THEN  COALESCE(dpd.ptas_resarea,'')
						   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN  dpd.ptas_commarea
						   ELSE dpd.ptas_commarea
					  END
	   ,SubArea		= CASE WHEN pt.PropType = 'R' THEN  COALESCE(dpd.ptas_ressubarea,'')
						   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN dpd.ptas_commsubarea
						   ELSE dpd.ptas_commsubarea
					  END
 	   ,PropType  	= pt.PropType
	   ,[Address]   = Convert(Varchar(50),Rtrim(COALESCE(dpd.ptas_addr1_streetnumber,'') 
					  + ' ' + COALESCE(dpd.ptas_nbrfraction,''))
                      + ' ' + LTRIM(COALESCE(dsm02.value,'') 
					  + ' ' + RTRIM(RTRIM(COALESCE(dpd.ptas_streetname,'') 
					  + ' ' + COALESCE(dpd.ptas_streettype,''))
				      + ' ' + COALESCE(dpd.ptas_dirsuffix,''))))
	   ,PlatBlock 	= COALESCE(dpd.ptas_platblock,'')
	   ,PlatLot     = COALESCE(dpd.ptas_platlot,'')
	   ,ApplGroup   = dpd.ptas_applgroup
 	   ,PIN   		= COALESCE(dpd.ptas_major,'') + COALESCE(dpd.ptas_minor,'')
	   ,MapPin      = CASE WHEN pt.PropType = 'K' then convert(float, dpd.ptas_major + '0000')
                       ELSE convert(float, dpd.ptas_major + dpd.ptas_minor)
					  END
  FROM dynamics.ptas_parceldetail dpd  
 INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
  LEFT JOIN [dynamics].[stringmap]	dsm02
    ON dsm02.attributevalue = dpd.ptas_addr1_directionprefix
   AND dsm02.objecttypecode = 'ptas_parceldetail'
   AND dsm02.attributename  = 'ptas_addr1_directionprefix'	
WHERE dpd.ptas_folio = @Folio
ORDER BY	   
       dpd.ptas_streetname
      ,dpd.ptas_streettype
      ,dsm02.value
      ,dpd.ptas_dirsuffix
      ,dpd.ptas_addr1_streetnumber
      ,dpd.ptas_nbrfraction
 SELECT *
   FROM #Srch 

RETURN(0)
END

