IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_LandValueWaterFront')
    DROP VIEW dynamics.vw_LandValueWaterFront ;  
GO 

CREATE VIEW dynamics.vw_LandValueWaterFront
--WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  10/11/2020
Description:    This View returns a List on Waterfront characteristics of the Land adding all of them in a single row per LandId

Modifications:
28/01/2021 - Hairo Barquero : For WF_Id there used to be a CASE using ptas_description now I´m using ptas_waterfrontlocation and
							  Column WF_ptas_description was replaced for WF_Location instead using the corresponding function to get the value.
22/02/2021 - Hairo Barquero : Added new columns for description, column names endig in "_value"							  
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT plvc._ptas_landid_value
    ,COALESCE(plvc.ptas_waterfrontlocation,0) AS WF_Id
	,COALESCE(sm.value,'') AS WF_Location
	,COALESCE(ptas_waterfrontbank,0) AS WF_ptas_waterfrontbank
	,COALESCE(sm3.value,'') AS WF_ptas_waterfrontbank_value
	,COALESCE(ptas_linearfootage,0) AS WF_ptas_linearfootage
	,COALESCE(ptas_depthfactor,0) AS WF_ptas_depthfactor
	,COALESCE(ptas_tidelandorshoreland,0) AS WF_ptas_tidelandorshoreland
	,COALESCE(sm2.value,'') AS WF_ptas_tidelandorshoreland_value
	,COALESCE(ptas_restrictedaccess,0) AS WF_ptas_restrictedaccess
	,COALESCE(sm1.value,'') AS WF_ptas_restrictedaccess_value
	,CASE WHEN COALESCE(ptas_accessrights,0) = 1 THEN 'Y' ELSE '' END WF_ptas_accessrights
	,CASE WHEN COALESCE(ptas_proximityinfluence,0) = 1 THEN 'Y' ELSE '' END WF_ptas_proximityinfluence
	,CASE WHEN COALESCE(ptas_poorquality,0) = 1 THEN 'Y' ELSE '' END WF_ptas_poorquality
	FROM [dynamics].[ptas_landvaluecalculation] plvc
	LEFT JOIN [dynamics].[stringmap] sm ON plvc.ptas_restrictedaccess = sm.attributevalue AND sm.attributename = 'ptas_waterfrontlocation' AND sm.objecttypecode = 'ptas_landvaluecalculation'
	LEFT JOIN [dynamics].[stringmap] sm1 ON plvc.ptas_restrictedaccess = sm1.attributevalue AND sm1.attributename = 'ptas_restrictedaccess' AND sm1.objecttypecode = 'ptas_landvaluecalculation'
	LEFT JOIN [dynamics].[stringmap] sm2 ON plvc.ptas_restrictedaccess = sm2.attributevalue AND sm2.attributename = 'ptas_tidelandorshoreland' AND sm2.objecttypecode = 'ptas_landvaluecalculation'
	LEFT JOIN [dynamics].[stringmap] sm3 ON plvc.ptas_restrictedaccess = sm3.attributevalue AND sm3.attributename = 'ptas_waterfrontbank' AND sm3.objecttypecode = 'ptas_landvaluecalculation'
	WHERE plvc.ptas_characteristictype = 668020002 --WaterFront
GO
--CREATE UNIQUE CLUSTERED INDEX Idx_ptas_landid_value ON [dynamics].[vw_LandValueWaterFront]
--(
--[_ptas_landid_value] 
--)
	