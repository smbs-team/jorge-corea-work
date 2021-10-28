IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_PropNameSearch')
	DROP PROCEDURE [cus].[SRCH_R_PropNameSearch]  
GO

CREATE PROCEDURE [cus].[SRCH_R_PropNameSearch] @PropName	varchar(100)
AS BEGIN
/*
Author: Jairo Barquero
Date Created:  11/12/2020
Description:    Search by Prop Name

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/

SET DEADLOCK_PRIORITY HIGH
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
 PropName nvarchar(Max)
,Major char(6)
,Minor char(4)
,PropType varchar(30) --Hairo comment changed from "char(1)" to varchar(30) due "Mobile Home" and "Floating Home", when finish this search I need to know if I´m not including these in the resultset.
,Area smallint
,SubArea smallint
,Pin char(10)
,MapPin float
,RecId int Identity(1,1))


-- PropName Search
-- Commercial Buildings or Condos
INSERT #Srch
SELECT  PropName = dpd.ptas_propertyname
	   ,Major    = dpd.ptas_major
	   ,Minor    = dpd.ptas_minor
	   ,PropType = pt.PropType
       ,Area     =  CASE WHEN pt.PropType = 'R' THEN  COALESCE(dpd.ptas_resarea,'')
	               	   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN  dpd.ptas_commarea
	               	   ELSE dpd.ptas_commarea
	                END
       ,SubArea  =  CASE WHEN pt.PropType = 'R' THEN  COALESCE(dpd.ptas_ressubarea,'')
	               	   WHEN pt.PropType = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN dpd.ptas_commsubarea
	               	   ELSE dpd.ptas_commsubarea
	                END
       ,Pin      = COALESCE(dpd.ptas_major,'') + COALESCE(dpd.ptas_minor,'')
       ,MapPin   = CASE WHEN pt.PropType = 'K' then convert(float, dpd.ptas_major + '0000')
                        ELSE convert(float, dpd.ptas_major + dpd.ptas_minor)
                   END			 
 FROM [dynamics].[ptas_parceldetail] dpd
INNER JOIN #PropType pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertyTypeId
WHERE dpd.ptas_propertyname like '%' + @PropName + '%'

SELECT Major
      ,Minor      
      ,PropName
      ,PropType 
      ,Area
      ,SubArea
      ,Pin
      ,MapPin
  FROM #Srch
 ORDER BY Major, Minor


RETURN(@@ERROR)
END



GO


