IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_CommAreaPcl')
    DROP VIEW dynamics.vw_CommAreaPcl ;  
GO 

CREATE VIEW dynamics.vw_CommAreaPcl
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  01/26/2021
Description:    List Commercial Area Parcels

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT pd.ptas_parceldetailid, pd.ptas_commarea, pd.ptas_commsubarea,0 AS Neighborhood
  FROM [dynamics].[ptas_parceldetail] pd
  LEFT JOIN dynamics.ptas_neighborhood (NOLOCK) dpn
    ON dpn.ptas_neighborhoodid = pd._ptas_neighborhoodid_value  
 WHERE pd.ptas_commarea    is not null
   AND pd.ptas_commsubarea is not null
   AND COALESCE(pd.ptas_splitcode,0) = 0
   AND (pd.statecode  = 0 AND pd.statuscode = 1)
   AND COALESCE(pd.ptas_snapshottype,'') = '' 