IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_Inspections')
    DROP VIEW dynamics.vw_Inspections;  
GO 

CREATE VIEW dynamics.vw_Inspections
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  12/01/2021
Description:    This View returns theInspection type by ParcelId

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT ptas_parcelid
	  ,ptas_inspectionreason
	  ,[Land] AS Land
	  ,[Imps] AS Imps
	  ,[Both] AS Both
	  ,Inspection = CASE
						WHEN ([Land] <> 0 AND [Imps] <> 0)  OR [Both] <> 0 Then 'Both'
						WHEN [Land] <> 0 AND [Both] = 0 Then 'Land'
						WHEN [Imps] <> 0 AND [Both] = 0 Then 'Imps'
						ELSE ''
					END
FROM
(
	select dpi.ptas_parcelid,dpi.ptas_inspectionreason,dpi.ptas_inspectiontype,1 as indicator
	from [ptas].[ptas_inspectionhistory] dpi
	where YEAR(ptas_inspectiondate) = (SELECT 2020)
) P
PIVOT  
(  
COUNT(indicator)
FOR ptas_inspectiontype IN  
(  [Land]
  ,[Imps]
  ,[Both]
  )
) AS pvt  
