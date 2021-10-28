IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_LandValueView')
    DROP VIEW dynamics.vw_LandValueView ;  
GO 

CREATE VIEW dynamics.vw_LandValueView
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  10/11/2020
Description:    This View returns a List of Land Views adding all views in a single Row per each LandId

Modifications:
01/06/2021 - Hairo Barquero : Modification to use ptas_viewtype instead ptas_description.
22/02/2021 - Hairo Barquero : adding the description for each view column, ending with  "_value"
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT _ptas_landid_value
		,COALESCE([591500000],0) AS VI_Bellevue
		,CASE 
			WHEN [591500000] = 1 THEN 'Fair'
			WHEN [591500000] = 2 THEN 'Average'
			WHEN [591500000] = 3 THEN 'Good'
			WHEN [591500000] = 4 THEN 'Excellent'
			ELSE ''
		END VI_Bellevue_value
		,COALESCE([591500001],0) AS VI_Cascades
		,CASE 
			WHEN [591500001] = 1 THEN 'Fair'
			WHEN [591500001] = 2 THEN 'Average'
			WHEN [591500001] = 3 THEN 'Good'
			WHEN [591500001] = 4 THEN 'Excellent'
			ELSE ''
		END VI_Cascades_value
		,COALESCE([591500002],0) AS VI_Lake_Sammamish
		,CASE 
			WHEN [591500002] = 1 THEN 'Fair'
			WHEN [591500002] = 2 THEN 'Average'
			WHEN [591500002] = 3 THEN 'Good'
			WHEN [591500002] = 4 THEN 'Excellent'
			ELSE ''
		END VI_Lake_Sammamish_value
		,COALESCE([591500003],0) AS VI_Lake_Washington
		,CASE 
			WHEN [591500003] = 1 THEN 'Fair'
			WHEN [591500003] = 2 THEN 'Average'
			WHEN [591500003] = 3 THEN 'Good'
			WHEN [591500003] = 4 THEN 'Excellent'
			ELSE ''
		END VI_Lake_Washington_value
		,COALESCE([591500004],0) AS VI_Lake_river_or_creek
		,CASE 
			WHEN [591500004] = 1 THEN 'Fair'
			WHEN [591500004] = 2 THEN 'Average'
			WHEN [591500004] = 3 THEN 'Good'
			WHEN [591500004] = 4 THEN 'Excellent'
			ELSE ''
		END VI_Lake_river_or_creek_value
		,COALESCE([591500005],0) AS VI_Olympics
		,CASE 
			WHEN [591500005] = 1 THEN 'Fair'
			WHEN [591500005] = 2 THEN 'Average'
			WHEN [591500005] = 3 THEN 'Good'
			WHEN [591500005] = 4 THEN 'Excellent'
			ELSE ''
		END VI_Olympics_value		
		,COALESCE([591500006],0) AS VI_Puget_Sound
		,CASE 
			WHEN [591500006] = 1 THEN 'Fair'
			WHEN [591500006] = 2 THEN 'Average'
			WHEN [591500006] = 3 THEN 'Good'
			WHEN [591500006] = 4 THEN 'Excellent'
			ELSE ''
		END VI_Puget_Sound_value
		,COALESCE([591500007],0) AS VI_Rainier
		,CASE 
			WHEN [591500007] = 1 THEN 'Fair'
			WHEN [591500007] = 2 THEN 'Average'
			WHEN [591500007] = 3 THEN 'Good'
			WHEN [591500007] = 4 THEN 'Excellent'
			ELSE ''
		END VI_Rainier_value
		,COALESCE([591500008],0) AS VI_Seattle
		,CASE 
			WHEN [591500008] = 1 THEN 'Fair'
			WHEN [591500008] = 2 THEN 'Average'
			WHEN [591500008] = 3 THEN 'Good'
			WHEN [591500008] = 4 THEN 'Excellent'
			ELSE ''
		END VI_Seattle_value
		,COALESCE([591500009],0) AS VI_Territorial
		,CASE 
			WHEN [591500009] = 1 THEN 'Fair'
			WHEN [591500009] = 2 THEN 'Average'
			WHEN [591500009] = 3 THEN 'Good'
			WHEN [591500009] = 4 THEN 'Excellent'
			ELSE ''
		END VI_Territorial_value
		,COALESCE([591500010],0) AS VI_Other_view
		,CASE 
			WHEN [591500010] = 1 THEN 'Fair'
			WHEN [591500010] = 2 THEN 'Average'
			WHEN [591500010] = 3 THEN 'Good'
			WHEN [591500010] = 4 THEN 'Excellent'
			ELSE ''
		END VI_Other_view_value				
FROM
(
	SELECT  plvc._ptas_landid_value
	       ,plvc.ptas_viewtype
		   ,ptas_quality as indicator
	  FROM [dynamics].[ptas_landvaluecalculation] plvc
	 WHERE plvc.ptas_characteristictype = 668020003 --View
) P
PIVOT  
(  
SUM(indicator)
FOR ptas_viewtype IN  
(  
	 [591500000]--	Bellevue
	,[591500001]--	Cascades
	,[591500002]--	Lake Sammamish
	,[591500003]--	Lake Washington
	,[591500004]--	Lake, river or creek
	,[591500005]--	Olympics
	,[591500006]--	Puget Sound
	,[591500007]--	Rainier
	,[591500008]--	Seattle
	,[591500009]--	Territorial
	,[591500010]--	Other view
  )
) AS pvt  