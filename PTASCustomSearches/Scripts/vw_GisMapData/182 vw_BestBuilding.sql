IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_BestBuilding')
    DROP VIEW dynamics.vw_BestBuilding ;  
GO 

CREATE VIEW dynamics.vw_BestBuilding
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  01/13/2021
Description:    This best building list

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT pbdmax.[_ptas_parceldetailid_value],pbd1.[ptas_buildingdetailid],py02.ptas_name yearbuiltid_value,py03.ptas_name effectiveyearid_value,[ptas_buildingquality],[ptas_percentcomplete]
  FROM [dynamics].[ptas_buildingdetail] (NOLOCK) pbd1
 INNER JOIN (SELECT  pbd01.[_ptas_parceldetailid_value], pbd01.[ptas_buildingdetailid],ptas_buildingnet_sqft
			   FROM [dynamics].[ptas_buildingdetail] (NOLOCK) pbd01
			  WHERE pbd01.[ptas_buildingdetailid] = (select top 1 pbd02.[ptas_buildingdetailid] from [dynamics].[ptas_buildingdetail] (NOLOCK) pbd02 where pbd01.[_ptas_parceldetailid_value] = pbd02.[_ptas_parceldetailid_value] 
												and pbd02.ptas_buildingnet_sqft = (select max(ptas_buildingnet_sqft) from [dynamics].[ptas_buildingdetail] (NOLOCK) pbd03 where pbd03.[_ptas_parceldetailid_value] = pbd02.[_ptas_parceldetailid_value]))							
			) pbdmax
	ON pbdmax.[ptas_buildingdetailid] = pbd1.[ptas_buildingdetailid]
 INNER JOIN [dynamics].[ptas_year] py02
	ON py02.ptas_yearid = pbd1._ptas_yearbuiltid_value
 INNER JOIN [dynamics].[ptas_year] py03
	ON py03.ptas_yearid = pbd1._ptas_effectiveyearid_value