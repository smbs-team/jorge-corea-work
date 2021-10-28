IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_InspectionHistory')
    DROP VIEW dynamics.vw_InspectionHistory ;  
GO 

CREATE VIEW dynamics.vw_InspectionHistory
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  12/09/2020
Description:    This View returns the Inspection history records

Modifications:
01/12/2021 - Hairo Barquero: Adding column AssmtYr, since we don´t have the assesment year we get it from pts_name by now.

mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
select dpi.ptas_parcelid , dpi.ptas_inspectiondate, dpi.ptas_inspectiontype, CAST(SUBSTRING(ptas_name,1,4) AS INT) AssmtYr
 FROM [ptas].[ptas_inspectionhistory] dpi
INNER JOIN 	(
			SELECT ptas_parcelid ,MAX(ptas_inspectiondate) AS Max_ptas_inspectiondate
		  	  FROM [ptas].[ptas_inspectionhistory]
			 --WHERE YEAR(ptas_inspectiondate) =2020
			 GROUP BY ptas_parcelid 
			 ) dpi1
  ON dpi.ptas_parcelid = dpi1.ptas_parcelid
 AND dpi.ptas_inspectiondate = dpi1.Max_ptas_inspectiondate

GO
