IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_ResAreaPcl')
    DROP VIEW dynamics.vw_ResAreaPcl ;  
GO 

CREATE VIEW dynamics.vw_ResAreaPcl
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  01/26/2021
Description:    List Residential Parcels

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT pd.ptas_parceldetailid, pd.ptas_resarea, ptas_ressubarea,COALESCE(dpn.ptas_name,0) AS Neighborhood
  FROM [dynamics].[ptas_parceldetail] pd
  LEFT JOIN dynamics.ptas_neighborhood (NOLOCK) dpn
    ON dpn.ptas_neighborhoodid = pd._ptas_neighborhoodid_value  
 WHERE pd.ptas_resarea    is not null
   AND pd.ptas_ressubarea is not null
   AND COALESCE(pd.ptas_splitcode,0) = 0
   AND (pd.statecode  = 0 AND pd.statuscode = 1)
   AND COALESCE(pd.ptas_snapshottype,'') = '' 