IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_BuildingCount')
    DROP VIEW dynamics.vw_BuildingCount ;  
GO 

CREATE VIEW dynamics.vw_BuildingCount
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  01/13/2021
Description:    List building count 

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT SUM(nmrbldg.sum_ptas_numberofbuildings) sum_ptas_numberofbuildings, nmrbldg._ptas_parceldetailid_value
FROM  (
			SELECT COUNT(A.ptas_numberofbuildings) sum_ptas_numberofbuildings ,A._ptas_parceldetailid_value 
			  FROM [dynamics].[ptas_buildingdetail] (NOLOCK) A
			 INNER JOIN [dynamics].[ptas_buildingdetail_commercialuse] (NOLOCK) B
				ON A.ptas_buildingdetailid = B._ptas_buildingdetailid_value
			 WHERE A.ptas_numberofbuildings <= 1
		     GROUP BY A._ptas_parceldetailid_value
			UNION ALL
			SELECT SUM(A.ptas_numberofbuildings) sum_ptas_numberofbuildings ,A._ptas_parceldetailid_value 
			  FROM [dynamics].[ptas_buildingdetail] (NOLOCK) A
			 INNER JOIN [dynamics].[ptas_buildingdetail_commercialuse] (NOLOCK) B
				ON A.ptas_buildingdetailid = B._ptas_buildingdetailid_value
			 WHERE A.ptas_numberofbuildings > 1
		     GROUP BY A._ptas_parceldetailid_value
) nmrbldg
group by nmrbldg._ptas_parceldetailid_value