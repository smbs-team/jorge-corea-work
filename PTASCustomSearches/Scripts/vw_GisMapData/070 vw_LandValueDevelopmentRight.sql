IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_LandValueDevelopmentRight')
    DROP VIEW dynamics.vw_LandValueDevelopmentRight ;  
GO 

CREATE VIEW dynamics.vw_LandValueDevelopmentRight 
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  10/11/2020
Description:    This View returns a List of Development Right adding each value in a single rows per each LandId

Modifications:
24/02/2021 - Hairo Baruqero: Change the values for  Dev_Sold and Dev_Purchased into "Y/N"
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT _ptas_landid_value
	  ,CASE WHEN COALESCE([668020000],0) = 1 THEN 'Y' ELSE 'N' END Dev_Sold
	  ,CASE WHEN COALESCE([668020001],0) = 1 THEN 'Y' ELSE 'N' END Dev_Purchased
	  ,COALESCE(DevRightsValPct,0)	AS DevRightsValPct

FROM
(
	SELECT plvc._ptas_landid_value,
	CAST(plvc.ptas_transfertype as Varchar(20)) ValueType,1 as indicator
	FROM [dynamics].[ptas_landvaluecalculation] plvc
	WHERE plvc.ptas_characteristictype = 591500000 --Development Right
	UNION ALL
	--Get the PORCENTAGE
	SELECT plvc._ptas_landid_value,
	CASE WHEN ptas_characteristictype = 591500000 THEN 'DevRightsValPct'
	END ValueType,
	plvc.ptas_percentadjustment as indicator
	FROM [dynamics].[ptas_landvaluecalculation] plvc
	WHERE plvc.ptas_characteristictype = 591500000 --Development Right

) P
PIVOT  
(  
SUM(indicator)
FOR ValueType IN  
(  [668020000]
  ,[668020001]
  ,DevRightsValPct
  )
) AS pvt   