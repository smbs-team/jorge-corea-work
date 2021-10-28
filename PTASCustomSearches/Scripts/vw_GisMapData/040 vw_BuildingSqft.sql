IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_BuildingSqft')
    DROP VIEW dynamics.vw_BuildingSqft ;  
GO 

CREATE VIEW dynamics.vw_BuildingSqft
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  01/14/2021
Description:    List the building Sqft data

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT _ptas_parceldetailid_value,
	   SUM(ptas_buildingnet_sqft) all_buildingnet_sqft,
	   SUM(ptas_buildinggross_sqft) all_buildinggross_sqft
  FROM [dynamics].[ptas_buildingdetail] (NOLOCK)
 GROUP BY _ptas_parceldetailid_value