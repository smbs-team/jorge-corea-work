IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_ResBuildings')
    DROP VIEW dynamics.vw_ResBuildings ;  
GO 

CREATE VIEW dynamics.vw_ResBuildings
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  12/01/2020
Description:    This View returns the quantity of residential buildings per Parcel

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT COUNT_BIG(*) BldngCount, _ptas_parceldetailid_value 
		FROM [dynamics].[ptas_buildingdetail] A
		INNER JOIN [dynamics].[ptas_propertytype] dpt
		ON A._ptas_propertytypeid_value = dpt.ptas_propertytypeid
		AND ptas_description = 'Residential'
		GROUP BY _ptas_parceldetailid_value 

GO
CREATE UNIQUE CLUSTERED INDEX Idx_ptas_parceldetailid_value ON [dynamics].[vw_ResBuildings]
(
[_ptas_parceldetailid_value] 

)