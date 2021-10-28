IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_SalesVerifiedYN')
    DROP VIEW dynamics.vw_SalesVerifiedYN;  
GO 

CREATE VIEW dynamics.vw_SalesVerifiedYN
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  12/30/2020
Description:    This View returns the List verified and unverified sales

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT ptas_parceldetailid
	  ,COALESCE(SalesCountUnverified,0) 		AS SalesCountUnverified
	  ,COALESCE(SalesCountVerifiedThisCycle,0) 	AS SalesCountVerifiedThisCycle
	  ,COALESCE(SalesCountVerifiedAtMkt,0) 		AS SalesCountVerifiedAtMkt	  
FROM
(
	--Get PORCETAGE Values
	SELECT dpd1.ptas_parceldetailid, 'SalesCountUnverified' Description, count(1) SaleCount
	  FROM [dynamics].[ptas_parceldetail] dpd1
	 INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
		ON spdps.ptas_parceldetailid = dpd1.ptas_parceldetailid
	 INNER JOIN [dynamics].[ptas_sales] dps
		ON dps.ptas_salesid = spdps.ptas_salesid
	   AND COALESCE(dps.ptas_atmarket,'') = ''
	   AND dps.ptas_saleprice >  0
	   AND dps.ptas_saledate  >=  '1/1/2017'	--Hairo comment, this date where hardcoded in the Original GismapData Code(ASSR_InitPop_GisMapData)
	   AND dps.ptas_saledate  <=  '12/31/2020'  --Hairo comment, this date where hardcoded in the Original GismapData Code(ASSR_InitPop_GisMapData)
	 GROUP BY dpd1.ptas_parceldetailid 
	UNION ALL
	SELECT dpd1.ptas_parceldetailid, 'SalesCountVerifiedThisCycle' Description, count(1) SaleCount
	  FROM [dynamics].[ptas_parceldetail] dpd1
	 INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
		ON spdps.ptas_parceldetailid = dpd1.ptas_parceldetailid
	 INNER JOIN [dynamics].[ptas_sales] dps
		ON dps.ptas_salesid = spdps.ptas_salesid
	   AND COALESCE(dps.ptas_atmarket,'') = ''
	   AND dps.ptas_saleprice >  0
	   AND dps.ptas_saledate  		>=  '1/1/2017' 	 --Hairo comment, this date where hardcoded in the Original GismapData Code(ASSR_InitPop_GisMapData)
	   AND dps.ptas_saledate  		<=  '12/31/2020' --Hairo comment, this date where hardcoded in the Original GismapData Code(ASSR_InitPop_GisMapData)
	   AND dps.ptas_verifiedbydate  > '9/23/2019'    --Hairo comment, this date where hardcoded in the Original GismapData Code(ASSR_InitPop_GisMapData)
	 GROUP BY dpd1.ptas_parceldetailid 
	 UNION ALL 
	 	SELECT dpd1.ptas_parceldetailid, 'SalesCountVerifiedAtMkt' Description, count(1) SaleCount
	  FROM [dynamics].[ptas_parceldetail] dpd1
	 INNER JOIN [dynamics].[ptas_sales_parceldetail_parcelsinsale] spdps
		ON spdps.ptas_parceldetailid = dpd1.ptas_parceldetailid
	 INNER JOIN [dynamics].[ptas_sales] dps
		ON dps.ptas_salesid = spdps.ptas_salesid
	   AND COALESCE(dps.ptas_atmarket,'') = 'Y'
	   AND dps.ptas_saleprice >  0
	   AND dps.ptas_saledate  		>=  '1/1/2017' 	 --Hairo comment, this date where hardcoded in the Original GismapData Code(ASSR_InitPop_GisMapData)
	   AND dps.ptas_saledate  		<=  '12/31/2020' --Hairo comment, this date where hardcoded in the Original GismapData Code(ASSR_InitPop_GisMapData)
	 GROUP BY dpd1.ptas_parceldetailid 

) P
PIVOT  
(  
SUM(SaleCount)
FOR Description IN  
(  SalesCountUnverified
  ,SalesCountVerifiedThisCycle
  ,SalesCountVerifiedAtMkt
  )
) AS pvt  