IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_LandValue')
    DROP VIEW dynamics.vw_LandValue ;  
GO 

CREATE VIEW dynamics.vw_LandValue
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  10/11/2020
Description:    This View returns some lands values for latest records 

Modifications:
02/04/2021 - Hairo Barquero:  Change in the entire solution. The previous version was to old and with all the 
							  changes in the data and structure was necesary to modify the output for this view.
							  Still using the same table [ptas].[ptas_appraisalhistory] as the first version
							  These are the old colums():
													 pah.ptas_parcelid
													,pah.ptas_landvalue
													,pah.ptas_impvalue_base
													,pah.ptas_newconstruction
													,pah.ptas_method 
													,CAST(pah.ptas_taxyearidname as INT) as [Year]
24/02/2021 - Hairo Baruqero: Modification on column names since Artic implemented a change in their database.													
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT   pa1.ParcelGuid						AS ptas_parcelid
		,pa1.RollYear						AS [year]
		,COALESCE(pa1.landvalue,0)			AS ptas_landvalue
		,COALESCE(pa1.impsValue,0)			AS ptas_ImpVal
		,COALESCE(pa1.totalvalue,0)			AS ptas_totalvalue
		,COALESCE(pa1.newConstrValue,0)		AS ptas_newconstruction
		,COALESCE(pa1.ApprMethod,'')		AS ptas_method
		,pa1.interfaceFlag					AS ptas_interfaceFlag
		,COALESCE(pa1.appraisedDate,'')		AS SelectDate
		,COALESCE(pa1.revalOrMaint,'')		AS ptas_revalormaint
		,PostingStatusDescr =  CASE
		                            WHEN pa1.interfaceFlag  =  6 THEN 'MfPostingErr'
		                            WHEN pa1.interfaceFlag  =  7 THEN 'PostUndivInt'
		                            WHEN pa1.interfaceFlag  =  8 THEN 'PostSplitAcct'
		                            WHEN pa1.interfaceFlag  = 17 THEN 'ExceedsThresh'
		                            WHEN pa1.interfaceFlag  = 18 THEN 'ExceptionalProp'                            
		                            WHEN pa1.interfaceFlag  = 19 THEN 'Holdout'      
		                            ELSE sm.value
		                          END
FROM ptas.ptas_appraisalhistory pa1
LEFT OUTER JOIN ptas.ptas_appraisalhistory pa2 ON (pa1.ParcelGuid = pa2.ParcelGuid AND pa1.RollYear = pa2.RollYear AND 
	(pa1.appraisedDate < pa2.appraisedDate OR (pa1.appraisedDate = pa2.appraisedDate AND pa1.ParcelGuid < pa2.ParcelGuid)))
LEFT JOIN dynamics.stringmap sm ON pa1.interfaceFlag = sm.attributevalue
AND sm.objecttypecode = 'ptas_appraisalhistory'
AND sm.attributename = 'ptas_method'
WHERE pa2.ParcelGuid IS NULL 