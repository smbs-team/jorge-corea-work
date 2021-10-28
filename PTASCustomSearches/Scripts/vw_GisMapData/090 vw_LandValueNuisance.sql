IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_LandValueNuisance')
    DROP VIEW dynamics.vw_LandValueNuisance ;  
GO 

CREATE VIEW dynamics.vw_LandValueNuisance 
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  10/11/2020
Description:    This View returns a List of all Land Nuisances adding all in a single Row per LandId

Modifications:
22/02/2021: Hairo Barquero: Added column NU_Traffic_noise_level
22/02/2021: Hairo Barquero: Disable 4 Nuisances NU_Parking, NU_Road_access, NU_Sewer_system and NU_Water_system, since I´m 
							getting the values from ptas_land table. Also modify the calculation with CASEs
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT _ptas_landid_value
	   ,COALESCE([591500000],0)						 AS NU_Airport_noise
	   ,CASE WHEN [591500002] = 1 THEN 'Y' ELSE ''  END NU_Power_lines
	   ,CASE WHEN [591500003] = 1 THEN 'Y' ELSE 'N' END NU_Restrictive_size_or_shape
	   ,CASE WHEN [591500006] = 1 THEN 'Y' ELSE ''  END NU_Topography
	   ,COALESCE([591500007],0)						 AS NU_Traffic_noise
	   ,CASE 
		WHEN [591500007] = 1 THEN 'Moderate'
		WHEN [591500007] = 2 THEN 'High'
		WHEN [591500007] = 3 THEN 'Extreme'
		ELSE ''
	    END											   NU_Traffic_noise_level	   
	   ,CASE WHEN [591500008] = 1 THEN 'Y' ELSE '' END NU_Water_problem
	   ,CASE WHEN [591500010] = 1 THEN 'Y' ELSE '' END NU_Other_nuisance
	   ,CASE WHEN [591500011] = 1 THEN 'Y' ELSE '' END NU_Other_Problems
	   ,COALESCE([AirportValPct],0)					AS AirportValPct
	   ,COALESCE([OtherNuisValPct],0)				AS OtherNuisValPct
	   ,COALESCE([OtherProblemsValPct],0)			AS OtherProblemsValPct
	   ,COALESCE([PowerLinesValPct],0)				AS PowerLinesValPct
	   ,COALESCE([RoadAccessValPct],0)				AS RoadAccessValPct
	   ,COALESCE([TopoValPct],0)					AS TopoValPct
	   ,COALESCE([WaterProblemsValPct],0)			AS WaterProblemsValPct
	   ,COALESCE([TrafficValPct],0)					AS TrafficValPct
	   ,COALESCE([AirportValDollars],0)				AS AirportValDollars
	   ,COALESCE([TrafficValDollars],0)				AS TrafficValDollars
	   --,COALESCE([591500001],0)			  AS NU_Parking
	   --,COALESCE([591500009],0)			  AS NU_Water_system
	   --,COALESCE([591500004],0)			  AS NU_Road_access
	   --,COALESCE([591500005],0)			  AS NU_Sewer_system	   
FROM
(
	--Identify all Nuisance
	SELECT plvc._ptas_landid_value,
		   CAST(plvc.ptas_nuisancetype as Varchar(20)) as ValueType,
	CASE
		WHEN plvc.ptas_noiselevel = 591500000 THEN 1 --Moderate
		WHEN plvc.ptas_noiselevel = 591500001 THEN 2 --High
		WHEN plvc.ptas_noiselevel = 591500002 THEN 3 --Extreme
		ELSE 1 -- 1 for the all the rest of Nuisances
	END indicator
	FROM [dynamics].[ptas_landvaluecalculation] plvc
	WHERE plvc.ptas_characteristictype = 668020006 --Nuisance
	UNION ALL
	--Get PORCETAGE Values
	SELECT plvc._ptas_landid_value,
		CASE WHEN plvc.ptas_nuisancetype = 591500000 THEN 'AirportValPct'  
			 WHEN plvc.ptas_nuisancetype = 591500002 THEN 'PowerLinesValPct'  
			 WHEN plvc.ptas_nuisancetype = 591500004 THEN 'RoadAccessValPct'  
			 WHEN plvc.ptas_nuisancetype = 591500006 THEN 'TopoValPct'  
			 WHEN plvc.ptas_nuisancetype = 591500007 THEN 'TrafficValPct'  
			 WHEN plvc.ptas_nuisancetype = 591500008 THEN 'WaterProblemsValPct'  
			 WHEN plvc.ptas_nuisancetype = 591500010 THEN 'OtherNuisValPct'  
			 WHEN plvc.ptas_nuisancetype = 591500011 THEN 'OtherProblemsValPct'  
			 ELSE 'N/A'			 
		END ValueType,
			COALESCE(plvc.ptas_percentadjustment,0) AS indicator
	FROM [dynamics].[ptas_landvaluecalculation] plvc
	WHERE plvc.ptas_characteristictype = 668020006 --Nuisance
	  AND plvc.ptas_nuisancetype in (591500000,591500002,591500004,591500006,591500007,591500008,591500010,591500011)
	UNION ALL
	--Get DOLLARS Values
	SELECT plvc._ptas_landid_value,
		CASE WHEN plvc.ptas_nuisancetype = 591500000 THEN 'AirportValDollars' 
			 WHEN plvc.ptas_nuisancetype = 591500007 THEN 'TrafficValDollars' 
			 ELSE 'N/A'
		END ValueType,
			COALESCE(plvc.ptas_dollaradjustment,0) AS indicator
	FROM [dynamics].[ptas_landvaluecalculation] plvc
	WHERE plvc.ptas_characteristictype = 668020006 --Nuisance
	  AND plvc.ptas_nuisancetype in (591500000,591500007)

) P
PIVOT  
(  
SUM(indicator)
FOR ValueType IN  
(  
 [591500000] 
,[591500001] 
,[591500002]
,[591500003]
,[591500004]
,[591500005]
,[591500006]
,[591500007]
,[591500008]
,[591500009]
,[591500010]
,[591500011]
,AirportValPct
,OtherNuisValPct
,OtherProblemsValPct
,PowerLinesValPct
,RoadAccessValPct
,TopoValPct
,WaterProblemsValPct
,TrafficValPct
,AirportValDollars
,TrafficValDollars
  )
) AS pvt  