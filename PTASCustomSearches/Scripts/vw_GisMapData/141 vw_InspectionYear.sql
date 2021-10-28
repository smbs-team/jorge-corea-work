IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_InspectionYear')
    DROP VIEW dynamics.vw_InspectionYear ;  
GO 

CREATE VIEW dynamics.vw_InspectionYear
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  01/12/2021
Description:    This View returns the Inspection year records

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT MAX(CAST(SUBSTRING(ptas_name,1,4) AS INT)) InspectYear , pih.ptas_parcelid 
			   FROM [ptas].[ptas_inspectionhistory] pih
			  GROUP BY pih.ptas_parcelid
GO

