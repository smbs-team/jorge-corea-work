IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_LandValueDesignations')
    DROP VIEW dynamics.vw_LandValueDesignations;  
GO 

CREATE VIEW dynamics.vw_LandValueDesignations
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  10/11/2020
Description:    This View returns the List of Designations as a Pivot table, adding all Designations in 1 row per LandID

Modifications:
24/02/2021 -Hairo Baruqero: Change the value for the main columns to present a Y / '' value instead of 0 / 1
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT _ptas_landid_value
	  ,CASE WHEN COALESCE([591500000],0) = 1 THEN 'Y' ELSE '' END Desig_Adjacent_golf
	  ,CASE WHEN COALESCE([591500001],0) = 1 THEN 'Y' ELSE '' END Desig_Adjacent_greenbelt
	  ,CASE WHEN COALESCE([591500002],0) = 1 THEN 'Y' ELSE '' END Desig_Deed_restrictions
      ,CASE WHEN COALESCE([591500003],0) = 1 THEN 'Y' ELSE '' END Desig_Easements
      ,CASE WHEN COALESCE([591500004],0) = 1 THEN 'Y' ELSE '' END Desig_DNR_lease
      ,CASE WHEN COALESCE([591500005],0) = 1 THEN 'Y' ELSE '' END Desig_Native_growth
      ,CASE WHEN COALESCE([591500006],0) = 1 THEN 'Y' ELSE '' END Desig_Other
	  --PORCENTAGE
	  ,COALESCE(AdjacGolfValPct,0)		    AS AdjacGolfValPct
	  ,COALESCE(AdjacGreenbeltValPct,0)		AS AdjacGreenbeltValPct
	  ,COALESCE(DeedRestrictValPct,0)		AS DeedRestrictValPct
	  ,COALESCE(EasementsValPct,0)			AS EasementsValPct
	  ,COALESCE(DNRLeaseValPct,0)			AS DNRLeaseValPct
	  ,COALESCE(NativeGrowthValPct,0)		AS NativeGrowthValPct
	  ,COALESCE(OtherDesigValPct,0)			AS OtherDesigValPct
	  --DOLLARS
	  ,COALESCE(AdjacGolfDollars,0)			AS AdjacGolfDollars
	  ,COALESCE(AdjacGreenbeltValDollars,0) AS AdjacGreenbeltValDollars
FROM
(
	SELECT plvc._ptas_landid_value,--,plvc._ptas_viewtypeid_value
	--plvc.ptas_description,1 as indicator
	CAST(plvc.ptas_designationtype as Varchar(20)) as ValueType,1 as indicator
	FROM [dynamics].[ptas_landvaluecalculation] plvc
	WHERE plvc.ptas_characteristictype = 668020005 --Designatioons
	UNION ALL
	--Get PORCETAGE Values
	SELECT plvc._ptas_landid_value,	
    CASE WHEN plvc.ptas_designationtype = 591500000 THEN 'AdjacGolfValPct'
		 WHEN plvc.ptas_designationtype = 591500001 THEN 'AdjacGreenbeltValPct'
	     WHEN plvc.ptas_designationtype = 591500002 THEN 'DeedRestrictValPct'
	     WHEN plvc.ptas_designationtype = 591500003 THEN 'EasementsValPct'
	     WHEN plvc.ptas_designationtype = 591500004 THEN 'DNRLeaseValPct'
	     WHEN plvc.ptas_designationtype = 591500005 THEN 'NativeGrowthValPct'
	     WHEN plvc.ptas_designationtype = 591500006 THEN 'OtherDesigValPct'
		 ELSE 'N/A'
	 END ValueType,
	     plvc.ptas_percentadjustment as indicator
	FROM [dynamics].[ptas_landvaluecalculation] plvc
	WHERE plvc.ptas_characteristictype = 668020005 --Designatioons
	UNION ALL
	--Get DOLLARS Values
	SELECT plvc._ptas_landid_value,
	CASE WHEN plvc.ptas_designationtype = 591500000 THEN 'AdjacGolfDollars'
		 WHEN plvc.ptas_designationtype = 591500001 THEN 'AdjacGreenbeltValDollars'
		 ELSE 'N/A'
	END ValueType,
		plvc.ptas_dollaradjustment as indicator
	FROM [dynamics].[ptas_landvaluecalculation] plvc
	WHERE plvc.ptas_characteristictype = 668020005    --Designatioons
	AND ptas_designationtype in (591500000,591500001) --Adjacent to golf fairway and Adjacent to green belt


) P
PIVOT  
(  
SUM(indicator)
FOR ValueType IN  
(  [591500000]
  ,[591500001]
  ,[591500002]
  ,[591500003]
  ,[591500004]
  ,[591500005]
  ,[591500006]
  ,AdjacGolfValPct
  ,AdjacGreenbeltValPct
  ,DeedRestrictValPct
  ,EasementsValPct
  ,DNRLeaseValPct
  ,NativeGrowthValPct
  ,OtherDesigValPct

  ,AdjacGolfDollars
  ,AdjacGreenbeltValDollars
  )
) AS pvt  