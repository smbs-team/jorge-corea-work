IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_LandValueEnvironmentalRestriction')
    DROP VIEW dynamics.vw_LandValueEnvironmentalRestriction ;  
GO 

CREATE VIEW dynamics.vw_LandValueEnvironmentalRestriction 
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  10/11/2020
Description:    This View returns a List of Environmental Restrictions adding all characteristics in a single Row per LandId

Modifications:
23/02/2021 - Hairo Barquero: Replace  "1 as indicator" by "plvc.ptas_percentaffected  as indicator" in order to calculated to values properly,
							 then, added a CAS logic to calculate the value correctly. 
mm/dd/yyyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT _ptas_landid_value
	   --,COALESCE(,0)		 
	   ,CASE
            WHEN [591500000] > 0 THEN convert(varchar(3),[591500000])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500000],0)*.01)))
            WHEN [591500000] = 0 THEN 'Y'
			ELSE ''
        END ER_100_year_flood_plain
	   ,CASE
            WHEN [591500001] > 0 THEN convert(varchar(3),[591500001])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500001],0)*.01)))
            WHEN [591500001] = 0 THEN 'Y'
			ELSE ''
        END ER_Channel_migration_areas_moderate
	   ,CASE
            WHEN [591500002] > 0 THEN convert(varchar(3),[591500002])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500002],0)*.01)))
            WHEN [591500002] = 0 THEN 'Y'
			ELSE ''
        END  ER_Channel_migration_areas_severe
	   ,CASE
            WHEN [591500003] > 0 THEN convert(varchar(3),[591500003])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500003],0)*.01)))
            WHEN [591500003] = 0 THEN 'Y'
			ELSE ''
        END ER_Coal_mine_hazard
	   ,CASE
            WHEN [591500004] > 0 THEN convert(varchar(3),[591500004])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500004],0)*.01)))
            WHEN [591500004] = 0 THEN 'Y'
			ELSE ''
        END ER_Contamination
	   ,CASE
            WHEN [591500005] > 0 THEN convert(varchar(3),[591500005])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500005],0)*.01)))
            WHEN [591500005] = 0 THEN 'Y'
			ELSE ''
        END  ER_Critical_area
	   ,CASE
            WHEN [591500006] > 0 THEN convert(varchar(3),[591500006])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500006],0)*.01)))
            WHEN [591500006] = 0 THEN 'Y'
			ELSE ''
        END  ER_Critical_drainage
	   ,CASE
            WHEN [591500007] > 0 THEN convert(varchar(3),[591500007])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500007],0)*.01)))
            WHEN [591500007] = 0 THEN 'Y'
			ELSE ''
        END ER_Erosion_hazard
	   ,CASE
            WHEN [591500008] > 0 THEN convert(varchar(3),[591500008])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500008],0)*.01)))
            WHEN [591500008] = 0 THEN 'Y'
			ELSE ''
        END ER_Floodway
	   ,CASE
            WHEN [591500009] > 0 THEN convert(varchar(3),[591500009])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500009],0)*.01)))
            WHEN [591500009] = 0 THEN 'Y'
			ELSE ''
        END ER_Landfill_buffer
	   ,CASE
            WHEN [591500010] > 0 THEN convert(varchar(3),[591500010])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500010],0)*.01)))
            WHEN [591500010] = 0 THEN 'Y'
			ELSE ''
        END ER_Landslide_hazard
	   ,CASE
            WHEN [591500011] > 0 THEN convert(varchar(3),[591500011])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500011],0)*.01)))
            WHEN [591500011] = 0 THEN 'Y'
			ELSE ''
        END ER_Seismic_hazard
	   ,CASE
            WHEN [591500012] > 0 THEN convert(varchar(3),[591500012])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500012],0)*.01)))
            WHEN [591500012] = 0 THEN 'Y'
			ELSE ''
        END ER_Sensitive_area_tract
	   ,CASE
            WHEN [591500013] > 0 THEN convert(varchar(3),[591500013])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500013],0)*.01)))
            WHEN [591500013] = 0 THEN 'Y'
			ELSE ''
        END ER_Species_of_concern
	   ,CASE
            WHEN [591500014] > 0 THEN convert(varchar(3),[591500014])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500014],0)*.01)))
            WHEN [591500014] = 0 THEN 'Y'
			ELSE ''
        END ER_Steep_slope_hazard
	   ,CASE
            WHEN [591500015] > 0 THEN convert(varchar(3),[591500015])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500015],0)*.01)))
            WHEN [591500015] = 0 THEN 'Y'
			ELSE ''
        END ER_Stream
	   ,CASE
            WHEN [591500016] > 0 THEN convert(varchar(3),[591500016])+'%'+'/'+convert(varchar(11),convert(int,COALESCE(pl.ptas_sqfttotal,0) * (COALESCE([591500016],0)*.01)))
            WHEN [591500016] = 0 THEN 'Y'
			ELSE ''
        END ER_Wetland
	   ,COALESCE(HundredYrValPct,0)	 AS HundredYrValPct	
	   ,COALESCE(CoalValPct,0)		 AS	CoalValPct		
	   ,COALESCE(ContamValPct,0)	 AS	ContamValPct	
	   ,COALESCE(DrainageValPct,0) 	 AS	DrainageValPct 	
	   ,COALESCE(ErosionValPct,0)	 AS	ErosionValPct	
	   ,COALESCE(LandfillValPct,0) 	 AS	LandfillValPct 	
	   ,COALESCE(LandslideValPct,0)	 AS	LandslideValPct	
	   ,COALESCE(SeismicValPct,0)	 AS	SeismicValPct	
	   ,COALESCE(SensitiveValPct,0)	 AS	SensitiveValPct	
	   ,COALESCE(SpeciesValPct,0)	 AS	SpeciesValPct	
	   ,COALESCE(SteepSlopeValPct,0) AS	SteepSlopeValPct
	   ,COALESCE(StreamValPct,0)	 AS	StreamValPct	
	   ,COALESCE(WetlandValPct,0)	 AS WetlandValPct		
	   ,pl.ptas_sqfttotal
FROM
(
	SELECT plvc._ptas_landid_value,
	CAST(plvc.ptas_environmentalrestriction as Varchar(20)) ValueType,plvc.ptas_percentaffected as indicator
	FROM [dynamics].[ptas_landvaluecalculation] plvc
	WHERE plvc.ptas_characteristictype = 668020004 --Environmental Restriction
	UNION ALL
	--Get PORCETAGE Values
	SELECT plvc._ptas_landid_value,
	CASE WHEN plvc.ptas_environmentalrestriction = 591500000 THEN 'HundredYrValPct'  --100-year flood plain
		 WHEN plvc.ptas_environmentalrestriction = 591500003 THEN 'CoalValPct'		 --Coal mine hazard
		 WHEN plvc.ptas_environmentalrestriction = 591500004 THEN 'ContamValPct'	 --Contamination
		 WHEN plvc.ptas_environmentalrestriction = 591500006 THEN 'DrainageValPct'   --Critical drainage
		 WHEN plvc.ptas_environmentalrestriction = 591500007 THEN 'ErosionValPct'	 --Erosion hazard
		 WHEN plvc.ptas_environmentalrestriction = 591500009 THEN 'LandfillValPct'   --Landfill buffer
		 WHEN plvc.ptas_environmentalrestriction = 591500010 THEN 'LandslideValPct'  --Landslide hazard
		 WHEN plvc.ptas_environmentalrestriction = 591500011 THEN 'SeismicValPct'	 --Seismic hazard
		 WHEN plvc.ptas_environmentalrestriction = 591500012 THEN 'SensitiveValPct'  --Sensitive area tract
		 WHEN plvc.ptas_environmentalrestriction = 591500013 THEN 'SpeciesValPct'	 --Species of concern
		 WHEN plvc.ptas_environmentalrestriction = 591500014 THEN 'SteepSlopeValPct' --Steep slope hazard
		 WHEN plvc.ptas_environmentalrestriction = 591500015 THEN 'StreamValPct'	 --Stream
		 WHEN plvc.ptas_environmentalrestriction = 591500016 THEN 'WetlandValPct'	 --Wetland 
		 ELSE 'N/A'
	END ValueType,
	plvc.ptas_percentadjustment as indicator
	FROM [dynamics].[ptas_landvaluecalculation] plvc
	WHERE plvc.ptas_characteristictype = 668020004 --Environmental Restriction
	  AND plvc.ptas_environmentalrestriction in (591500000,591500003,591500004,591500006,591500007,591500009,591500010,
												 591500011,591500012,591500013,591500014,591500015,591500016)
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
	,[591500012]
	,[591500013]
	,[591500014]
	,[591500015]
	,[591500016]
	,HundredYrValPct
	,CoalValPct		
	,ContamValPct	
	,DrainageValPct 
	,ErosionValPct	
	,LandfillValPct 
	,LandslideValPct
	,SeismicValPct	
	,SensitiveValPct
	,SpeciesValPct	
	,SteepSlopeValPct
	,StreamValPct	
	,WetlandValPct	
  )
) AS pvt  
INNER JOIN dynamics.ptas_land pl ON pl.ptas_landid = pvt._ptas_landid_value