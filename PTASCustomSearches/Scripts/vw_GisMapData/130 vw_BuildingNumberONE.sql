IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_BuildingNumberONE')
    DROP VIEW dynamics.vw_BuildingNumberONE ;  
GO 

CREATE VIEW dynamics.vw_BuildingNumberONE 
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  12/09/2020
Description:    This record where Building number is equeal to 1, but in some cases there are 2 records with the same value, that is why 
				I create this view. that could be a errorn in the data.

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT bd._ptas_parceldetailid_value
,bd.ptas_buildingdetailid
,bd.ptas_buildingnumber
,bd.ptas_numberofstories
,bd.ptas_buildingquality
,bd.ptas_numberofbuildings
,bd.ptas_units
,bd._ptas_yearbuiltid_value	
,bd._ptas_yearrenovatedid_value
,bd.ptas_buildinggrade
,bd.ptas_gradevariance
,bd.ptas_res_buildingcondition
,bd.ptas_totalliving_sqft
,bd.ptas_numberofstoriesdecimal
,bd.ptas_1stflr_sqft
,bd.ptas_halfflr_sqft
,bd.ptas_2ndflr_sqft
,bd.ptas_upperflr_sqft
,bd.ptas_unfinished_full_sqft	
,bd.ptas_unfinished_half_sqft
,bd.ptas_totalbsmt_sqft
,bd.ptas_finbsmt_sqft
,bd.ptas_res_basementgrade
,bd.ptas_basementgarage_sqft
,bd.ptas_attachedgarage_sqft
,bd.ptas_daylightbasement 
,bd.ptas_bedroomnbr
,bd.ptas_12baths
,bd.ptas_34baths
,bd.ptas_fullbathnbr
,bd.ptas_single_fireplace
,bd.ptas_multi_fireplace
,bd.ptas_fr_std_fireplace
,bd.ptas_addl_fireplace
,bd.ptas_percentcomplete	
,bd.ptas_percentnetcondition
,bd.ptas_buildingobsolescence
,bd.ptas_openporch_sqft
,bd.ptas_enclosedporch_sqft
,bd.ptas_deck_sqft
,bd.ptas_residentialheatingsystem
,bd.ptas_res_heatsource
,bd.ptas_percentbrickorstone
,bd.ptas_additionalcost
,bd.ptas_viewutilizationrating
  FROM [dynamics].[ptas_buildingdetail] bd
 INNER JOIN 	(
			SELECT _ptas_parceldetailid_value ,MAX(createdon) AS Max_Createdon
		  	  FROM [dynamics].[ptas_buildingdetail] bd1
			 WHERE ptas_buildingnumber = 1
			 GROUP BY _ptas_parceldetailid_value 
			 ) bd2
    ON bd._ptas_parceldetailid_value = bd2._ptas_parceldetailid_value
   AND bd.createdon = bd2.Max_Createdon
 WHERE ptas_buildingnumber = 1

GO
