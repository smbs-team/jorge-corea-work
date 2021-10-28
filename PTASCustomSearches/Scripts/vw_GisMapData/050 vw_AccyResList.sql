IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_AccyResList')
    DROP VIEW dynamics.vw_AccyResList;  
GO 

CREATE VIEW dynamics.vw_AccyResList
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  10/11/2020
Description:    This Function returns an Accessory residential list

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/

SELECT _ptas_parceldetailid_value, 
[1] AS AccyDetGar, [2] AS AccyCarport, [3] AS AccyConcretePvmt, [6] AS AccyConcPool, [7] AS AccyPlasPool, [8] AS AccyMissImp
,[100] AS AccyAreaDetGar
,[101] AS AccyBarnSteel
,[102] AS AccyBarnWood
,[103] AS AccyBoatCanopy
,[104] AS AccyBoatLift
,[105] AS AccyDock
,[106] AS AccyMoorage
,[107] AS AccySeaplaneLift
,[108] AS AccySportCourt
,[109] AS AccyStables
FROM   
(   SELECT a._ptas_parceldetailid_value,1 ResAccy, COALESCE(dsm.attributevalue,0) AccyType
	 FROM [dynamics].[ptas_accessorydetail] a
    INNER JOIN [dynamics].[stringmap]	dsm
       ON dsm.attributevalue = a.ptas_resaccessorytype
      AND dsm.objecttypecode = 'ptas_accessorydetail'
      AND dsm.attributename  = 'ptas_resaccessorytype'
	) p  
PIVOT  
(  
COUNT(ResAccy)
FOR AccyType IN  
( [1] , [2] , [3] , [6] , [7] ,
  [8] , [9] , [10] , 
  [4] , [5] , [0], 
  [100],[101],[102],[103],[104],[105],[106],[107],[108],[109]
  )
) AS pvt  
