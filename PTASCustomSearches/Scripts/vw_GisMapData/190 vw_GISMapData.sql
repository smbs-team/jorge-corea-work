
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_GISMapData')
    DROP VIEW dynamics.vw_GISMapData;  
GO  
CREATE VIEW dynamics.vw_GISMapData

AS
/*
Author: Jairo Barquero
Date Created:  10/11/2020
Description:    This View returns all GISMapData columns required.

Modifications:
IMPORTANT NOTE: When you find (SELECT 2020) the idea is change that and instead use the table that is going to have the Assesment year,
				currently the value is hardcoded, the idea is that this value came from an specific table like in RP [dbo].[AssmtYr]

11/25/2020 - Hairo Barquero : Changed the calculation from SelectMethod, since the value is now located in the table [ptas].[ptas_appraisalhistory]
							  previously only the code came and we found the description using the function fn_GetValueFromStringMap, not anymore.						  
11/30/2020 - Hairo Barquero : Added calculation for ApplDistrict
12/07/2020 - Hairo Barquero : Added Columns:
											CurrentUseDesignation
											HistoricSite		  
											NbrBldgSites	
12/08/2020 - Hairo Barquero : Added column OtherProblems	
12/22/2020 - Hairo Barquero : Added COALESCE for YrBuiltRes, YrRenovated, PcntComplRes,Obsolescence,PcntNetCondition,Condition 		 
12/30/2020 - Hairo Barquero : Added 3 new columns: SalesCountUnverified, SalesCountVerifiedThisCycle and SalesCountVerifiedAtMkt	
01/05/2021 - Hairo Barquero : Label for PcntBaseLandValImpacted
01/05/2021 - Hairo Barquero : Modification for ViewDescr, added a TRIM and CASE to Get the "Non-View" values
01/12/2021 - Hairo Barquero : Change [dynamics].[ptas_inspectionhistory] for [ptas].[ptas_inspectionhistory]
01/12/2021 - Hairo Barquero : change YEAR(ptas_inspectiondate) for dpi.AssmtYr, there was a change in the view [dynamics].[vw_InspectionHistory]
01/14/2021 - Hairo Barquero : Added the calculation for columns AssignedLand, AssignedImps and AssignedBoth
01/14/2021 - Hairo Barquero : Change the calculation for CmlNbrImps, replace the queries for a view(dynamics.vw_BuildingCount)
01/14/2021 - Hairo Barquero : Replace the queries in alias bldngsum and added view dynamics.vw_BuildingSqft
01/18/2021 - Hairo Barquero : Change the calculation for ApplDistrict
01/22/2021 - Hairo Barquero : Added filter to bring ONLY ACTIVE records 
																	   AND COALESCE(dpd.ptas_splitcode,0) = 0
																	   AND (dpd.statecode  = 0 AND dpd.statuscode = 1) --This get the ACTIVE parcels. When "statecode  = 1 AND dpd.statuscode = 2" the parcel is Killed
																	   AND COALESCE(dpd.ptas_snapshottype,'') = ''
01/25/2021 - Hairo Barquero : Added calculation for PropType using the correct JOIN with dynamics.ptas_propertytype table
28/01/2021 - Hairo Barquero : Change WF_ptas_description for WF_Location, there was a change in view vw_LandValueWaterFront
04/02/2020 - Hairo Baruqero : Modification for ALL columns related to LandValues, this is becasue the view vw_LandValue was modified,
							  also I´m adding 2 more columns PostingStatus and PostingStatusDescr
22/02/2021 - Hairo Barquero : Adding new columns, description for land characteristics: TrafficNoiseLevel, View columns ending with "_value"
							  Waterfront columns ending with "_value"
23/02/2021 - Hairo Barquero : Added columns LandProbDescrPart1 and 2, also modification for Nuisance and Land vies, since the values in Views changed.							  
25/02/2021 - Hairo Barquero : Added function "fn_ReturnMetaData" to get Assesment year from a Global  Constant
26/02/2021 - Hairo Barquero : Change the calculation for AddrLine INSTEAD ptas_address USE  ptas_addr1_compositeaddress
mm/dd/yyyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT 
 	    ParcelID = dpd.ptas_parceldetailid
 	   ,LndGuid= dpl.ptas_landid 
 	   ,Legacyrp_Landid = dpl.ptas_legacyrplandid
 	   ,Major = dpd.ptas_major 
 	   ,Minor = dpd.ptas_minor   
 	   ,PIN   = COALESCE(dpd.ptas_major,'') + COALESCE(dpd.ptas_minor,'')
 	   ,PropName  = COALESCE(SUBSTRING(dpd.ptas_propertyname,1,80),'') 
 	   ,PropType  = SUBSTRING(pt.ptas_name,1,1) 
 	   ,ApplGroup = dpd.ptas_applgroup 
 	   ,ApplGroupNumeric =(SELECT psm.displayorder FROM dynamics.stringmap psm WHERE psm.value = dpd.ptas_applgroup AND psm.attributename = 'ptas_appraisalgroup')--NOT FOUND,ApplGroupNumeric = (select lui.LuItemId from LUItem2 lui where lui.MainframeCode = rp.ApplGroup AND lui.LuTypeId = 44)	   
 	   ,Folio = dpd.ptas_folio 
 	   ,LevyCode = dpd.ptas_levyCode 
 	   ,BaseLandVal 	 = dpl.ptas_baselandValue
 	   ,BaseLandValTaxYr = dpl.ptas_taxyear
 	   ,BaseLandValSqFt  = dpl.ptas_dollarsPerSquareFoot 
 	   --,BaseLandValUnit  = Hairo Comment This value is ptas_landvaluecalculation.ptas_valuemethodcalculation(linked to stringmap table) but in ptas_landvaluecalculation there are several records for 1 land, I need to know what would be the unique record to obtain one specific value
 	   ,BaseLandValDate  = dpl.ptas_valueDate 
 	   --,NewBaseLandValUnit= Hairo Comment, I need to find BaseLandValUnit to calculate this column
	   ,NewBaseLandValDate  =  CASE WHEN dpl.ptas_taxyear = (dynamics.fn_ReturnMetaData('AssessmentYear','GlobalConstant'))+1 THEN  dpl.ptas_valueDate 		ELSE '19000101' END   
 	   ,NewBaseLandVal 		=  CASE WHEN dpl.ptas_taxyear = (dynamics.fn_ReturnMetaData('AssessmentYear','GlobalConstant'))+1 THEN  dpl.ptas_baselandValue 	ELSE 0 END      
 	   ,NewBaseLandValTaxYr =  CASE WHEN dpl.ptas_taxyear = (dynamics.fn_ReturnMetaData('AssessmentYear','GlobalConstant'))+1 THEN  dpl.ptas_taxyear 			ELSE 0 END     
 	   ,NewBaseLandValSqFt  =  CASE WHEN dpl.ptas_taxyear = (dynamics.fn_ReturnMetaData('AssessmentYear','GlobalConstant'))+1 THEN  dpl.ptas_dollarsPerSquareFoot ELSE 0 END      	 	   
 	   ,PresentUse		    = [dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_presentuse',	 dpl.ptas_presentuse		) 
 	   ,HBUAsIfVacant	    = [dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_hbuifvacant',  dpl.ptas_hbuifvacant 	) 
 	   ,HBUAsImproved	    = [dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_hbuifimproved',dpl.ptas_hbuifimproved 	)  
 	   ,WaterSystem		    = [dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_watersystem',  dpl.ptas_watersystem 	) 
 	   ,SewerSystem		    = [dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_sewersystem',  dpl.ptas_sewersystem 	) 
 	   ,StreetSurface	    = [dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_streetSurface',dpl.ptas_streetSurface 	) 
 	   ,RoadAccess		    = [dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_roadaccess',	 dpl.ptas_roadaccess 	)      
	   ,InadequateParking = CASE 
									WHEN dpl.ptas_parking = 1 THEN 'Y' 
									WHEN dpl.ptas_parking = 2 THEN 'N' 
									ELSE '' 
								  END 	   
		
 	   ,Unbuildable = [dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_unbuildable',	dpl.ptas_unbuildable 	) 
 	   ,PcntBaseLandValImpacted = dpl.ptas_percentbaseLandValue
 	   ,PcntUnusable = COALESCE(dpl.ptas_percentUnusable,0) 
 	   ,UpdateDate = GetDate() 
 	   --,Source = 'NEW_GisMapData'
--**NEW COLUMNS **********************************************************************************************************************************************************
		,ResArea 	= CASE WHEN LEN(dpd.ptas_resarea) = 1 THEN '00'+CAST(dpd.ptas_resarea AS VARCHAR(3))
						   WHEN LEN(dpd.ptas_resarea) = 2 THEN '0'+CAST(dpd.ptas_resarea AS VARCHAR(3))
						   WHEN LEN(dpd.ptas_resarea) = 3 THEN CAST(dpd.ptas_resarea AS VARCHAR(3))
						   ELSE ''
					  END
		,ResSubArea = CASE WHEN LEN(dpd.ptas_ressubarea) = 1 THEN '00'+CAST(dpd.ptas_ressubarea AS VARCHAR(3))
						   WHEN LEN(dpd.ptas_ressubarea) = 2 THEN '0'+CAST(dpd.ptas_ressubarea AS VARCHAR(3))
						   WHEN LEN(dpd.ptas_ressubarea) = 3 THEN CAST(dpd.ptas_ressubarea AS VARCHAR(3))
						   ELSE ''
					  END
		,ResNbhd 	= COALESCE(dpn.ptas_name,'')
		,ResAreaSub = CASE 
						WHEN COALESCE(dpd.ptas_resarea,'') <> '' THEN (CASE WHEN LEN(dpd.ptas_resarea) = 1 THEN '00'+CAST(dpd.ptas_resarea AS VARCHAR(3))
																	        WHEN LEN(dpd.ptas_resarea) = 2 THEN '0'+CAST(dpd.ptas_resarea AS VARCHAR(3))
																	        WHEN LEN(dpd.ptas_resarea) = 3 THEN CAST(dpd.ptas_resarea AS VARCHAR(3))
																	        ELSE ''
																		END
																		+ '-'+
																	   CASE WHEN LEN(dpd.ptas_ressubarea) = 1 THEN '00'+CAST(dpd.ptas_ressubarea AS VARCHAR(3))
																			WHEN LEN(dpd.ptas_ressubarea) = 2 THEN '0'+CAST(dpd.ptas_ressubarea AS VARCHAR(3))
																			WHEN LEN(dpd.ptas_ressubarea) = 3 THEN CAST(dpd.ptas_ressubarea AS VARCHAR(3))
																			ELSE ''
																		END																		
																	   )
						ELSE '' 
					 END
		,SqFtLot = dpl.ptas_sqftTotal 	
		,BLVSqFtCalc = CASE 
					 	 WHEN dpl.ptas_baselandValue > 0 AND dpl.ptas_sqftTotal > 0 THEN CONVERT(DECIMAL(20,2), convert(real,dpl.ptas_baselandValue)/convert(real,dpl.ptas_sqftTotal))
					 	 ELSE 0
					   END   	
		,NewBLVSqFtCalc  = CASE 
							WHEN dpl.ptas_baselandValue > 0 AND dpl.ptas_sqftTotal > 0 AND dpl.ptas_taxyear = (dynamics.fn_ReturnMetaData('AssessmentYear','GlobalConstant'))+1 THEN CONVERT(DECIMAL(20,2), convert(real,dpl.ptas_baselandValue)/convert(real,dpl.ptas_sqftTotal))
							ELSE 0
						   END  
		
		,SubmergedLand = CASE 
						  WHEN dpl.ptas_submergedsqftru > 0 AND dpl.ptas_sqfttotal > 0 THEN 
						  CONVERT(VARCHAR(6),CONVERT(DECIMAL(20,0), 100*convert(real,dpl.ptas_submergedsqftru)/convert(real,dpl.ptas_sqfttotal)))	+'%'+'/'+ CONVERT(VARCHAR(11),dpl.ptas_submergedsqftru)--get the sqft affected
						  ELSE ''
						END

		--,AssignedLand 	  = 'NOT FOUND' 
		--,AssignedImps 	  = 'NOT FOUND' 
		--,AssignedBoth 	  = 'NOT FOUND' 
		,Inspection 	  = COALESCE(dpi.ptas_inspectiontype,'') /*Hairo Comment Need to double check this calculation*/
		,InspectAssmtYear = COALESCE(dpi2.InspectYear,'') /*Hairo Comment Need to double check this calculation*/		
		--,LastInspectAppr  = 'NOT FOUND' 
		,GeoArea  	  	  = COALESCE(pga.ptas_areanumber,'0')			/*Hairo Comment Need to double check this calculation*/
		,GeoNbhd  	  	  = COALESCE(substring(pgn.ptas_name,1,3),'0') --VALIDATE THIS, THERE IS NO CODE COLUMN IN IN TABLE dynamics].[ptas_geoneighborhood] /*Hairo Comment Need to double check this calculation*/
		,SpecArea 	  	  = COALESCE(psa.ptas_areanumber,'0')			/*Hairo Comment Need to double check this calculation*/
		,SpecNbhd 	  	  = COALESCE(substring(psn.ptas_name,1,3),'0')	/*Hairo Comment Need to double check this calculation*/
		,GeoAreaNbhd  	  = CASE WHEN COALESCE(pga.ptas_areanumber,'0') <> 0 THEN COALESCE(pga.ptas_areanumber,'0')+'-'+COALESCE(substring(pgn.ptas_name,1,3),'0') ELSE '' END
		,SpecAreaNbhd     = CASE  WHEN COALESCE(psa.ptas_areanumber,'0') <> 0 THEN COALESCE(psa.ptas_areanumber,'0')+'-'+COALESCE(substring(psn.ptas_name,1,3),'0') ELSE '' END
		,AddrLine 	  	  = COALESCE(dpd.ptas_addr1_compositeaddress,'')--COALESCE(dpd.ptas_address,'')
		,StreetNbr    	  = COALESCE(dpd.ptas_addr1_streetnumber,'')
		--,ApplDistrict     = [dynamics].[fn_GetValueFromStringMap]('ptas_parceldetail','ptas_residentialdistrict', dpd.ptas_residentialdistrict	)  
		,ApplDistrict	  = CASE 
								WHEN SUBSTRING(pt.ptas_name,1,1) = 'R' THEN 'Res'+ [dynamics].[fn_GetValueFromStringMap]('ptas_parceldetail','ptas_residentialdistrict', dpd.ptas_residentialdistrict	)  
								WHEN COALESCE(dpd.ptas_commercialdistrict,'') <> '' AND COALESCE(dpd.ptas_commarea,'') <> '' THEN 'Cml'+ [dynamics].[fn_GetValueFromStringMap]('ptas_parceldetail','ptas_commercialdistrict', dpd.ptas_commercialdistrict	)  
								WHEN dpd.ptas_applgroup <> 'A' AND COALESCE(dpd.ptas_commercialdistrict,'') <> '' THEN  'CmlAssignNeeded'
								WHEN SUBSTRING(pt.ptas_name,1,1) = 'T' THEN 'Timber(DOR)'
								WHEN COALESCE(ptas_ressubarea,0) = 0
								     AND dpd.ptas_resarea >0
								     AND SUBSTRING(pt.ptas_name,1,1) ='R'
								     THEN 'AssignNeeded'
								ELSE 'AssignNeeded'
							END
		,ApplDistrictCode  = CASE 
								WHEN SUBSTRING(pt.ptas_name,1,1) = 'R' THEN dpd.ptas_residentialdistrict  
								WHEN COALESCE(dpd.ptas_commercialdistrict,'') <> '' AND COALESCE(dpd.ptas_commarea,'') <> '' THEN dpd.ptas_commercialdistrict   
								WHEN dpd.ptas_applgroup <> 'A' AND COALESCE(dpd.ptas_commercialdistrict,'') <> '' THEN  ''
								WHEN SUBSTRING(pt.ptas_name,1,1) = 'T' THEN ''
								WHEN COALESCE(ptas_ressubarea,0) = 0
								     AND dpd.ptas_resarea >0
								     AND SUBSTRING(pt.ptas_name,1,1) ='R'
								     THEN ''
								ELSE ''
							END
		--,Team 		    = 'NOT FOUND' 
		--,SpecApplDistrict = 'NOT FOUND'  
		,TaxStatus 	      = [dynamics].[fn_GetValueFromStringMap]('ptas_parceldetail','ptas_taxstatus',dpd.ptas_taxstatus)  --Hairo comment Not sure about this calculation
		,TaxpayerName     = dpd.ptas_namesonaccount
		,CurrentZoning 	  = COALESCE(dpl.ptas_zoning,'')
		,DistrictName  	  = dpd.ptas_district
		--Begin DESIGNATIONS
		,AdjacentGolfFairway = COALESCE(vw_LvDes.Desig_Adjacent_golf,'')
		,AdjacentGreenbelt   = COALESCE(vw_LvDes.Desig_Adjacent_greenbelt,'')
		,DeedRestrictions    = COALESCE(vw_LvDes.Desig_Deed_restrictions,'')
		,DNRLease            = COALESCE(vw_LvDes.Desig_Easements,'')
		,Easements 		     = COALESCE(vw_LvDes.Desig_DNR_lease,'')
		,NativeGrowthProtEsmt= COALESCE(vw_LvDes.Desig_Native_growth,'')
		,OtherDesignation    = COALESCE(vw_LvDes.Desig_Other,'')
		--Begin DEVELOPMENT RIGHT
		,DevelopmentRightsPurchased = COALESCE(vw_LvDR.Dev_Purchased,'') 
		,DevelopmentRightsSold		= COALESCE(vw_LvDR.Dev_Sold,'') 
		--Begin ENVIROMENTAL RESTRICTIONS
		,CoalMineHazard		  = COALESCE(vw_LvER.ER_Coal_mine_hazard ,'')
		,CriticalDrainage     = COALESCE(vw_LvER.ER_Critical_drainage ,'')
		,ErosionHazard        = COALESCE(vw_LvER.ER_Erosion_hazard ,'')
		,LandfillBuffer       = COALESCE(vw_LvER.ER_Landfill_buffer ,'')
		,HundredYrFloodPlain  = COALESCE(vw_LvER.ER_100_year_flood_plain ,'')
		,SeismicHazard        = COALESCE(vw_LvER.ER_Seismic_hazard ,'')
		,LandslideHazard      = COALESCE(vw_LvER.ER_Landslide_hazard ,'')
		,SteepSlopeHazard     = COALESCE(vw_LvER.ER_Steep_slope_hazard ,'')
		,Stream               = COALESCE(vw_LvER.ER_Stream ,'')
		,Wetland              = COALESCE(vw_LvER.ER_Wetland ,'')
		,SpeciesOfConcern     = COALESCE(vw_LvER.ER_Species_of_concern ,'')
		,Contamination        = COALESCE(vw_LvER.ER_Contamination ,'')
		,SensitiveAreaTract   = COALESCE(vw_LvER.ER_Sensitive_area_tract ,'')
		,ChannelModerate      = COALESCE(vw_LvER.ER_Channel_migration_areas_moderate ,'')
		,ChannelSevere        = COALESCE(vw_LvER.ER_Channel_migration_areas_severe ,'')
		,CriticalArea         = COALESCE(vw_LvER.ER_Critical_area ,'')
		,Floodway             = COALESCE(vw_LvER.ER_Floodway ,'')
		--Begin NUISANCE     
		--,InadequateParking 
		--,SewerSystem		 This 4 Nuisance are calculated above
		--,RoadAccess        
		--,WaterSystem		 
		,AirportNoise         = COALESCE(vw_LvNU.NU_Airport_noise,0)
		,TrafficNoise         = COALESCE(vw_LvNU.NU_Traffic_noise,0)
		,TrafficNoiseLevel	  = COALESCE(vw_LvNU.NU_Traffic_noise_level,'')
		,CurrentUseDesignation= [dynamics].[fn_GetValueFromStringMap]('ptas_parceldetail','ptas_currentuse',dpd.ptas_currentuse) 							
		,HistoricSite		  = [dynamics].[fn_GetValueFromStringMap]('ptas_parceldetail','ptas_historicsite',dpd.ptas_historicsite ) 
		,NbrBldgSites		  = COALESCE(dpl.ptas_totalsitesperzoning,0)
		,Topography			  = COALESCE(vw_LvNU.NU_Topography,'')
		,PowerLines           = COALESCE(vw_LvNU.NU_Power_lines,'')
		,RestrictiveSzShape   = COALESCE(vw_LvNU.NU_Restrictive_size_or_shape,'')
		,WaterProblems        = COALESCE(vw_LvNU.NU_Water_problem,'')
		,OtherNuisances       = COALESCE(vw_LvNU.NU_Other_nuisance,'')
		,OtherProblems        = COALESCE(vw_LvNU.NU_Other_Problems,'')		
		
		--Begin VIEWS        
		,ViewBellevue		  		= COALESCE(vw_LvVW.VI_Bellevue,0)
		,ViewBellevue_value	  		= COALESCE(vw_LvVW.VI_Bellevue_value,'')		
		,ViewCascades         		= COALESCE(vw_LvVW.VI_Cascades,0)
		,ViewCascades_value   		= COALESCE(vw_LvVW.VI_Cascades_value,'')
		,ViewLake_Sammamish   		= COALESCE(vw_LvVW.VI_Lake_Sammamish,0)
		,ViewLake_Sammamish_value   = COALESCE(vw_LvVW.VI_Lake_Sammamish_value,'')		
		,ViewLake_Washington  		= COALESCE(vw_LvVW.VI_Lake_Washington,0)
		,ViewLake_Washington_value  = COALESCE(vw_LvVW.VI_Lake_Washington_value,'')		
		,ViewLakeRiverCreek   		= COALESCE(vw_LvVW.VI_Lake_river_or_creek,0)
		,ViewLakeRiverCreek_value   = COALESCE(vw_LvVW.VI_Lake_river_or_creek_value,'')		
		,ViewOlympics         		= COALESCE(vw_LvVW.VI_Olympics,0)
		,ViewOlympics_value         = COALESCE(vw_LvVW.VI_Olympics_value,'')	
		,ViewPugetSound       		= COALESCE(vw_LvVW.VI_Puget_Sound,0)
		,ViewPugetSound_value       = COALESCE(vw_LvVW.VI_Puget_Sound_value,'')
		,ViewRainier          		= COALESCE(vw_LvVW.VI_Rainier,0)
		,ViewRainier_value          = COALESCE(vw_LvVW.VI_Rainier_value,'')
		,ViewSeattle          		= COALESCE(vw_LvVW.VI_Seattle,0)
		,ViewSeattle_value          = COALESCE(vw_LvVW.VI_Seattle_value,'')
		,ViewTerritorial      		= COALESCE(vw_LvVW.VI_Territorial,0)
		,ViewTerritorial_value      = COALESCE(vw_LvVW.VI_Territorial_value,'')
		,ViewOtherView		  		= COALESCE(vw_LvVW.VI_Other_view,0)
		,ViewOtherView_value		= COALESCE(vw_LvVW.VI_Other_view_value,'')
		
		,NonWaterViews 		  =	COALESCE(vw_LvVW.VI_Bellevue,0) + COALESCE(vw_LvVW.VI_Cascades,0) + COALESCE(vw_LvVW.VI_Olympics,0) + COALESCE(vw_LvVW.VI_Rainier,0) + COALESCE(vw_LvVW.VI_Seattle,0) + COALESCE(vw_LvVW.VI_Territorial,0) + COALESCE(vw_LvVW.VI_Other_view,0)
		,WaterViews 		  = COALESCE(vw_LvVW.VI_Lake_Sammamish,0) + COALESCE(vw_LvVW.VI_Lake_Washington,0) + COALESCE(vw_LvVW.VI_Lake_river_or_creek,0) + COALESCE(vw_LvVW.VI_Puget_Sound,0)
		,TotalViews 		  = COALESCE(vw_LvVW.VI_Bellevue,0) + COALESCE(vw_LvVW.VI_Cascades,0) + COALESCE(vw_LvVW.VI_Olympics,0) + COALESCE(vw_LvVW.VI_Rainier,0) + COALESCE(vw_LvVW.VI_Seattle,0) + COALESCE(vw_LvVW.VI_Territorial,0) + COALESCE(vw_LvVW.VI_Other_view,0)
								+ COALESCE(vw_LvVW.VI_Lake_Sammamish,0) + COALESCE(vw_LvVW.VI_Lake_Washington,0) + COALESCE(vw_LvVW.VI_Lake_river_or_creek,0) + COALESCE(vw_LvVW.VI_Puget_Sound,0)
		,ViewDescr 			  = TRIM(CASE WHEN (
									    	 CASE WHEN COALESCE(vw_LvVW.VI_Bellevue,0) > 0 THEN COALESCE('Belle='+CAST(COALESCE(vw_LvVW.VI_Bellevue,0) AS CHAR(1)),'') ELSE '' END
									       + CASE WHEN COALESCE(vw_LvVW.VI_Cascades,0) > 0 THEN COALESCE(' Casc='+CAST(COALESCE(vw_LvVW.VI_Cascades,0) AS CHAR(1)),'') ELSE '' END
									       + CASE WHEN COALESCE(vw_LvVW.VI_Lake_Sammamish,0) > 0 THEN COALESCE(' LkSam='+CAST(COALESCE(vw_LvVW.VI_Lake_Sammamish,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Lake_Washington,0) > 0 THEN COALESCE(' LkWa='+CAST(COALESCE(vw_LvVW.VI_Lake_Washington,0) AS CHAR(1)),'') ELSE '' END										
									       + CASE WHEN COALESCE(vw_LvVW.VI_Lake_river_or_creek,0) > 0 THEN COALESCE(' LkRvCk='+CAST(COALESCE(vw_LvVW.VI_Lake_river_or_creek,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Olympics,0) > 0 THEN COALESCE(' Oly='+CAST(COALESCE(vw_LvVW.VI_Olympics,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Puget_Sound,0) > 0 THEN COALESCE(' Pug='+CAST(COALESCE(vw_LvVW.VI_Puget_Sound,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Rainier,0) > 0 THEN COALESCE(' Rain='+CAST(COALESCE(vw_LvVW.VI_Rainier,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Seattle,0) > 0 THEN COALESCE(' Seat='+CAST(COALESCE(vw_LvVW.VI_Seattle,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Territorial,0) > 0 THEN COALESCE(' Terr='+CAST(COALESCE(vw_LvVW.VI_Territorial,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Other_view,0) > 0 THEN COALESCE(' OtherVw='+CAST(COALESCE(vw_LvVW.VI_Other_view,0) AS CHAR(1)),'') ELSE '' END								
									      ) = '' AND SUBSTRING(pt.ptas_name,1,1) = 'R' THEN 'Non-View'
										  
									ELSE 									    	 
									         CASE WHEN COALESCE(vw_LvVW.VI_Bellevue,0) > 0 THEN COALESCE('Belle='+CAST(COALESCE(vw_LvVW.VI_Bellevue,0) AS CHAR(1)),'') ELSE '' END
									       + CASE WHEN COALESCE(vw_LvVW.VI_Cascades,0) > 0 THEN COALESCE(' Casc='+CAST(COALESCE(vw_LvVW.VI_Cascades,0) AS CHAR(1)),'') ELSE '' END
									       + CASE WHEN COALESCE(vw_LvVW.VI_Lake_Sammamish,0) > 0 THEN COALESCE(' LkSam='+CAST(COALESCE(vw_LvVW.VI_Lake_Sammamish,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Lake_Washington,0) > 0 THEN COALESCE(' LkWa='+CAST(COALESCE(vw_LvVW.VI_Lake_Washington,0) AS CHAR(1)),'') ELSE '' END										
									       + CASE WHEN COALESCE(vw_LvVW.VI_Lake_river_or_creek,0) > 0 THEN COALESCE(' LkRvCk='+CAST(COALESCE(vw_LvVW.VI_Lake_river_or_creek,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Olympics,0) > 0 THEN COALESCE(' Oly='+CAST(COALESCE(vw_LvVW.VI_Olympics,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Puget_Sound,0) > 0 THEN COALESCE(' Pug='+CAST(COALESCE(vw_LvVW.VI_Puget_Sound,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Rainier,0) > 0 THEN COALESCE(' Rain='+CAST(COALESCE(vw_LvVW.VI_Rainier,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Seattle,0) > 0 THEN COALESCE(' Seat='+CAST(COALESCE(vw_LvVW.VI_Seattle,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Territorial,0) > 0 THEN COALESCE(' Terr='+CAST(COALESCE(vw_LvVW.VI_Territorial,0) AS CHAR(1)),'') ELSE '' END								
									       + CASE WHEN COALESCE(vw_LvVW.VI_Other_view,0) > 0 THEN COALESCE(' OtherVw='+CAST(COALESCE(vw_LvVW.VI_Other_view,0) AS CHAR(1)),'') ELSE '' END								

								END)
									   
		,WfntLocation 			=  COALESCE(vw_LvWF.WF_Location,'')
		,WfntFootage  			=  COALESCE(vw_LvWF.WF_ptas_linearfootage,0)						
		,WfntPoorQuality 		=  COALESCE(vw_LvWF.WF_ptas_poorquality,'')							
		,WfntBank 				=  COALESCE(vw_LvWF.WF_ptas_waterfrontbank,0)		
		,WfntBank_value			=  COALESCE(vw_LvWF.WF_ptas_waterfrontbank_value,'')				
		,TidelandShoreland 		=  COALESCE(vw_LvWF.WF_ptas_tidelandorshoreland,0)							
		,TidelandShoreland_value=  COALESCE(vw_LvWF.WF_ptas_tidelandorshoreland_value,'')									
		,WfntRestrictedAccess   =  COALESCE(vw_LvWF.WF_ptas_restrictedaccess,0)							
		,WfntRestrictedAccess_value =  COALESCE(vw_LvWF.WF_ptas_restrictedaccess_value,'')									
		,LotDepthFactor			=  COALESCE(vw_LvWF.WF_ptas_depthfactor,0)							
		,WfntAccessRights 		=  COALESCE(vw_LvWF.WF_ptas_accessrights,'')							
		,WfntProximityInfluence =  COALESCE(vw_LvWF.WF_ptas_proximityinfluence,'')		
		,WfntLabel   			= convert(varchar(8),vw_LvWF.WF_ptas_linearfootage) + ' wff'		
		,EconomicUnit			= [dynamics].[fn_GetValueFromStringMap]('ptas_parceleconomicunit','ptas_type',ppeu.ptas_type)--Hairo Comment: currently treated as a unique records in the table for each parcel, in case of several records for parcel, the join must change
        ,EconomicUnitName		= ppeu.ptas_name --Hairo Comment: currently treated as a unique records in the table for each parcel, in case of several records for parcel, the join must change
        --,EconomicUnitParcelList = 'NOT FOUND'
		,BldgGuid = pbd.ptas_buildingdetailid
		,ResNbrImps = COALESCE(BldngQnt.BldngCount,0) --(SELECT COUNT(1) FROM [dynamics].[ptas_buildingdetail] A WHERE A._ptas_parceldetailid_value = dpd.ptas_parceldetailid)
		,NbrLivingUnits = COALESCE(pbd.ptas_units,0)
		,Stories 	   = COALESCE(pbd.ptas_numberofstories,0)
		,BldgGrade 	   = COALESCE(pbd.ptas_buildinggrade,0)
		,BldgGrDescr   = [dynamics].[fn_GetValueFromStringMap]('ptas_buildingdetail','ptas_buildinggrade',pbd.ptas_buildinggrade)
		,SqFtTotLiving = COALESCE(pbd.ptas_totalliving_sqft,0)
		,UnfinArea     = COALESCE(pbd.ptas_unfinished_full_sqft,0) + COALESCE(pbd.ptas_unfinished_half_sqft,0)
		,YrBuiltRes	   = COALESCE(py01.ptas_name,0)
		,YrRenovated   = COALESCE(py02.ptas_name,0)
		,YrBltRen 	   = CASE
							WHEN isnull(CAST(py02.ptas_name AS INT),0) > 0 THEN COALESCE(py01.ptas_name,'')+'/'+ COALESCE(py02.ptas_name,'')
							WHEN isnull(CAST(py01.ptas_name AS INT),0) > 0 THEN py01.ptas_name
							ELSE ''
						 END
		,PcntComplRes	  = COALESCE(pbd.ptas_percentcomplete,0)
		,Obsolescence	  = COALESCE(pbd.ptas_buildingobsolescence,0)
		,PcntNetCondition = COALESCE(pbd.ptas_percentnetcondition,0)
		,Condition 		  = COALESCE(pbd.ptas_res_buildingcondition,0)
		,CondDescr		  = [dynamics].[fn_GetValueFromStringMap]('ptas_buildingdetail','ptas_res_buildingcondition',pbd.ptas_res_buildingcondition)
		,ViewUtilization  = [dynamics].[fn_GetValueFromStringMap]('ptas_buildingdetail','ptas_viewutilizationrating',pbd.ptas_viewutilizationrating)
		,CmlBldgQual       = [dynamics].[fn_GetValueFromStringMap]('ptas_buildingdetail','ptas_buildingquality',BestBldg.ptas_buildingquality)
		,CmlYrBuilt        = COALESCE(BestBldg.yearbuiltid_value,0)
		,CmlEffYr          = COALESCE(BestBldg.effectiveyearid_value,0)
		--,CmlPredominantUse = 'NOT FOUND'
		,CmlPcntCompl      = COALESCE(BestBldg.ptas_percentcomplete,0)
		,CmlNetSqFtAllBldg   = COALESCE(bldngsum.all_buildingnet_sqft,0)
		,CmlGrossSqFtAllBldg = COALESCE(bldngsum.all_buildinggross_sqft,0)		
		,CmlNbrImps			 = COALESCE(pbdcount.sum_ptas_numberofbuildings,0)
		,LandVal 			 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh.ptas_landvalue,0) 
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh.ptas_ImpVal,0) > 0 THEN COALESCE(apprh.ptas_landvalue,0)
									ELSE 0
							   END
		,ImpsVal 			 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh.ptas_ImpVal,0) 
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh.ptas_ImpVal,0) > 0 THEN COALESCE(apprh.ptas_ImpVal,0)
									ELSE 0
							   END								
		,TotVal 			 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh.ptas_totalvalue,0) 
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh.ptas_ImpVal,0) > 0 THEN COALESCE(apprh.ptas_totalvalue,0)
									ELSE 0
							   END
		,NewConstrVal 		 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh.ptas_newconstruction,0) 
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh.ptas_ImpVal,0) > 0 THEN COALESCE(apprh.ptas_newconstruction,0)
									ELSE 0
							   END							   
		,SelectMethod		 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh.ptas_method,'')
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh.ptas_ImpVal,0) > 0 THEN COALESCE(apprh.ptas_method,'')
									ELSE ''
							   END										
		,PostingStatus		 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh.ptas_interfaceflag,'')
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh.ptas_ImpVal,0) > 0 THEN COALESCE(apprh.ptas_interfaceflag,'')
									ELSE ''
							   END														
		,PostingStatusDescr	 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh.PostingStatusDescr,'')
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh.ptas_ImpVal,0) > 0 THEN COALESCE(apprh.PostingStatusDescr,'')
									ELSE ''
							   END														
								
		,PrevLandVal 		 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh02.ptas_landvalue,0) 
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh02.ptas_ImpVal,0) > 0 AND apprh02.ptas_revalormaint = 'R' THEN COALESCE(apprh02.ptas_landvalue,0)
									ELSE 0
							   END
		,PrevImpsVal 		 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh02.ptas_ImpVal,0) 
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh02.ptas_ImpVal,0) > 0 AND apprh02.ptas_revalormaint = 'R' THEN COALESCE(apprh02.ptas_ImpVal,0)
									ELSE 0
							   END								
		,PrevTotVal 		 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh02.ptas_totalvalue,0) 
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh02.ptas_ImpVal,0) > 0 AND apprh02.ptas_revalormaint = 'R' THEN COALESCE(apprh02.ptas_totalvalue,0)
									ELSE 0
							   END
		,PrevNewConstrVal 	 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh02.ptas_newconstruction,0) 
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh02.ptas_ImpVal,0) > 0 AND apprh02.ptas_revalormaint = 'R' THEN COALESCE(apprh02.ptas_newconstruction,0)
									ELSE 0
							   END							   
		,PrevSelectMethod	 = CASE WHEN SUBSTRING(pt.ptas_name,1,1) <> 'K' THEN COALESCE(apprh02.ptas_method,'')
									WHEN SUBSTRING(pt.ptas_name,1,1) = 'K' AND COALESCE(apprh02.ptas_ImpVal,0) > 0 AND apprh02.ptas_revalormaint = 'R' THEN COALESCE(apprh02.ptas_method,'')
									ELSE ''
							   END										
		,LandValSqFt		 = CASE WHEN COALESCE(apprh.ptas_landvalue,0)   > 0 AND COALESCE(dpl.ptas_sqftTotal,0) > 0 THEN convert(decimal(20,2),apprh.ptas_landvalue)/convert(decimal(20,2),dpl.ptas_sqftTotal) ELSE 0 END 
		,PrevLandValSqFt     = CASE WHEN COALESCE(apprh02.ptas_landvalue,0) > 0 AND COALESCE(dpl.ptas_sqftTotal,0) > 0 THEN convert(decimal(20,2),apprh02.ptas_landvalue)/convert(decimal(20,2),dpl.ptas_sqftTotal) ELSE 0 END 
		,PcntChgLand         = CASE WHEN COALESCE(apprh.ptas_landvalue,0)   > 0 AND COALESCE(apprh02.ptas_landvalue,0) > 0 THEN 100 * (( convert(decimal(20,2),apprh.ptas_landvalue)/convert(decimal(20,2),apprh02.ptas_landvalue)-1) ) ELSE 0 END 
		,PcntChgImps         = CASE
								 WHEN apprh02.ptas_ImpVal > 0 AND COALESCE(apprh.ptas_ImpVal,0) > 0 THEN 100 * (( convert(decimal(20,2),apprh.ptas_ImpVal)/convert(decimal(20,2),apprh02.ptas_ImpVal)-1) )
								 WHEN apprh02.ptas_ImpVal = 0 AND COALESCE(apprh.ptas_ImpVal,0) > 0 THEN 100 
								 WHEN apprh02.ptas_ImpVal > 0 AND COALESCE(apprh.ptas_ImpVal,0) = 0 THEN  0                  
								 ELSE 0
							   END	  
		,PcntChgTotal        = CASE WHEN (COALESCE(apprh.ptas_landvalue,0) + COALESCE(apprh.ptas_ImpVal,0) ) > 0 AND (COALESCE(apprh02.ptas_landvalue,0) + COALESCE(apprh02.ptas_ImpVal,0)) > 0 THEN 100 * (( convert(decimal(20,2),COALESCE(apprh.ptas_landvalue,0) + COALESCE(apprh.ptas_ImpVal,0))/convert(decimal(20,2),COALESCE(apprh02.ptas_landvalue,0) + COALESCE(apprh02.ptas_ImpVal,0))-1) ) ELSE 0 END 
		,BLVPcntChg          = CASE WHEN COALESCE(dpl.ptas_baselandValue,0) > 0 AND COALESCE(apprh02.ptas_landvalue,0) > 0 AND dpl.ptas_taxyear = (dynamics.fn_ReturnMetaData('AssessmentYear','GlobalConstant')) + 1 THEN 100 * (( convert(decimal(20,2),COALESCE(dpl.ptas_baselandValue,0))/convert(decimal(20,2),COALESCE(apprh02.ptas_landvalue,0))-1) ) ELSE 0 END
		,AccyPRKDETGAR 		 = COALESCE(vwArl.AccyDetGar,0)
		,AccyPRKCARPORT 	 = COALESCE(vwArl.AccyCarport,0)
		,AccyPVCONCRETE 	 = COALESCE(vwArl.AccyConcretePvmt,0)
		,AccyPOOL 			 = COALESCE(vwArl.AccyConcPool,0) + COALESCE(vwArl.AccyPlasPool,0)
		,AccyMISCIMP 		 = COALESCE(vwArl.AccyMissImp,0)
		--,AccyMHPersProp	     = vwArl.AccyMHPersProp --Hairo Comment this is disabled becasue the values are not available in RealProperty, it could be a new type of Res Accy in PTAS, can be enabled if necesary
		--,AccyMHRealProp	     = vwArl.AccyMHRealProp --Hairo Comment this is disabled becasue the values are not available in RealProperty, it could be a new type of Res Accy in PTAS, can be enabled if necesary
		,NbrResAccys 		 = COALESCE(vwArl.AccyDetGar,0) +COALESCE(vwArl.AccyCarport,0) + COALESCE(vwArl.AccyConcretePvmt,0) + COALESCE(vwArl.AccyConcPool,0) + COALESCE(vwArl.AccyPlasPool,0) + COALESCE(vwArl.AccyMissImp,0)
		,SalesCountUnverified 		 = COALESCE(uvs.SalesCountUnverified,0)
		,SalesCountVerifiedThisCycle = COALESCE(uvs.SalesCountVerifiedThisCycle,0)
		,SalesCountVerifiedAtMkt	 = COALESCE(uvs.SalesCountVerifiedAtMkt,0)
		,AssignedLand = CASE WHEN ats.AssignmentTypeItemId = 1 AND  SUBSTRING(pt.ptas_name,1,1) = 'R' THEN ats.ptas_legacyid ELSE '' END 
		,AssignedImps = CASE WHEN ats.AssignmentTypeItemId = 2 AND  SUBSTRING(pt.ptas_name,1,1) = 'R' THEN ats.ptas_legacyid ELSE '' END 
		,AssignedBoth = CASE WHEN ats.AssignmentTypeItemId = 3 AND  SUBSTRING(pt.ptas_name,1,1) = 'R' THEN ats.ptas_legacyid ELSE '' END 

		,LandProbDescrPart1 =  case when COALESCE(vw_LvNU.NU_Airport_noise,0) > 0 then ' AirprtNois='+CONVERT(VARCHAR(5),COALESCE(vw_LvNU.NU_Airport_noise,0)) else '' end
		+ case when COALESCE(vw_LvER.ER_Coal_mine_hazard ,'') <> '' then ' CoalMine=' + COALESCE(vw_LvER.ER_Coal_mine_hazard ,'') else '' end 
		+ case when COALESCE(vw_LvER.ER_Contamination ,'') <> '' then  ' Contam=' +   COALESCE(vw_LvER.ER_Contamination ,'') else '' end    
		+ case when COALESCE(vw_LvER.ER_Critical_drainage ,'') <> '' then  ' CritDrain=' + COALESCE(vw_LvER.ER_Critical_drainage ,'') else '' end   
		+ case when COALESCE(vw_LvDes.Desig_Deed_restrictions,'')  = 'Y' then  ' DeedRestr=' + COALESCE(vw_LvDes.Desig_Deed_restrictions,'') else '' end   
		+ case when COALESCE(vw_LvDR.Dev_Purchased,'')  = 'Y' then  ' DevRights=' + +CONVERT(VARCHAR(5),COALESCE(vw_LvDR.Dev_Purchased,'')) else '' end   
		+ case when COALESCE(vw_LvDes.Desig_Easements,'')  = 'Y' then  ' DNRLease=' + COALESCE(vw_LvDes.Desig_Easements,'') else '' end   
		+ case when COALESCE(vw_LvDes.Desig_DNR_lease,'')  = 'Y' then  ' Easement=' + COALESCE(vw_LvDes.Desig_DNR_lease,'') else '' end      
		+ case when COALESCE(vw_LvER.ER_Erosion_hazard ,'') <> '' then  ' Erosion=' + COALESCE(vw_LvER.ER_Erosion_hazard ,'') else '' end   
		+ case when COALESCE(vw_LvER.ER_100_year_flood_plain ,'') <> '' then  ' FldPlain=' + COALESCE(vw_LvER.ER_100_year_flood_plain ,'') else '' end    
		+ case when dpl.ptas_parking = 1 then  ' InadPrkg= Y'  else '' end      
		+ case when COALESCE(vw_LvER.ER_Landfill_buffer ,'') <> '' then  ' Lndfill=' +  COALESCE(vw_LvER.ER_Landfill_buffer ,'') else '' end     
		+ case when COALESCE(vw_LvER.ER_Landslide_hazard ,'') <> '' then  ' Lndslide=' + COALESCE(vw_LvER.ER_Landslide_hazard ,'') else '' end      
		+ case when COALESCE(vw_LvDes.Desig_Native_growth,'') = 'Y' then  ' NatvGrowthEsmt=' + COALESCE(vw_LvDes.Desig_Native_growth,'') else '' end      
		+ case when COALESCE(vw_LvDes.Desig_Other,'') = 'Y' then  ' OthrDesig=' +  COALESCE(vw_LvDes.Desig_Other,'') else '' end     
		+ case when COALESCE(vw_LvNU.NU_Other_nuisance,'') = 'Y' then  ' OthrNuis=' + COALESCE(vw_LvNU.NU_Other_nuisance,'') else '' end     
		+ case when COALESCE(vw_LvNU.NU_Other_Problems,'') = 'Y' then  ' OthrProb=' +  COALESCE(vw_LvNU.NU_Other_Problems,'') else '' end    
		+ case when dpl.ptas_percentbaseLandValue < 100 then ' PcntBLV='+ CONVERT(VARCHAR(5),dpl.ptas_percentbaseLandValue) + '%'  else '' end 
		+ case when COALESCE(dpl.ptas_percentUnusable,0)  > 0 then ' PcntUnus='+ CONVERT(VARCHAR(5),COALESCE(dpl.ptas_percentUnusable,0)) + '%'  else '' end   
		+ case when COALESCE(vw_LvNU.NU_Power_lines,'') = 'Y' then  ' PwerLine=' +  COALESCE(vw_LvNU.NU_Power_lines,'') else '' end   
		+ case when COALESCE(vw_LvNU.NU_Restrictive_size_or_shape,'') = 'Y' then  ' RestrSzShape=' +  COALESCE(vw_LvNU.NU_Restrictive_size_or_shape,'') else '' end   
		+ case when UPPER([dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_roadaccess',	 dpl.ptas_roadaccess 	)) IN (NULL,'LEGAL/UNDEVELOPED','RESTRICTED','WALK IN') then  ' RestrAccess=' + [dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_roadaccess',	 dpl.ptas_roadaccess 	) else '' end    
		+ case when COALESCE(vw_LvER.ER_Seismic_hazard ,'') <> '' then  ' Seismic=' + COALESCE(vw_LvER.ER_Seismic_hazard ,'') else '' end    
		+ case when COALESCE(vw_LvER.ER_Sensitive_area_tract ,'') <> '' then  ' Sensitive=' + COALESCE(vw_LvER.ER_Sensitive_area_tract ,'') else '' end     
		+ case when UPPER([dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_sewersystem',  dpl.ptas_sewersystem 	))  LIKE '%RESTRICTED%' then ' RestrSewer=Y' else '' end  
		+ case when COALESCE(vw_LvER.ER_Species_of_concern ,'') <> '' then  ' Species=' + COALESCE(vw_LvER.ER_Species_of_concern ,'') else '' end     
		+ case when COALESCE(vw_LvER.ER_Steep_slope_hazard ,'') <> '' then  ' SteepSlope=' + COALESCE(vw_LvER.ER_Steep_slope_hazard ,'') else '' end     
		+ case when COALESCE(vw_LvER.ER_Stream ,'') <> '' then  ' Stream=' + COALESCE(vw_LvER.ER_Stream ,'') else '' end     
		+ case when (CASE 
							  WHEN dpl.ptas_submergedsqftru > 0 AND dpl.ptas_sqfttotal > 0 THEN 
							  CONVERT(VARCHAR(6),CONVERT(DECIMAL(20,0), 100*convert(real,dpl.ptas_submergedsqftru)/convert(real,dpl.ptas_sqfttotal)))	+'%'+'/'+ CONVERT(VARCHAR(11),dpl.ptas_submergedsqftru)--get the sqft affected
							  ELSE ''
							END) <> '' then  ' SubmergedLand=' + (CASE 
							  WHEN dpl.ptas_submergedsqftru > 0 AND dpl.ptas_sqfttotal > 0 THEN 
							  CONVERT(VARCHAR(6),CONVERT(DECIMAL(20,0), 100*convert(real,dpl.ptas_submergedsqftru)/convert(real,dpl.ptas_sqfttotal)))	+'%'+'/'+ CONVERT(VARCHAR(11),dpl.ptas_submergedsqftru)--get the sqft affected
							  ELSE ''
							END) else '' end     
		+ case when COALESCE(vw_LvNU.NU_Topography,'') = 'Y' then  ' Topo=' + COALESCE(vw_LvNU.NU_Topography,'') else '' end   
		+ case when COALESCE(vw_LvNU.NU_Traffic_noise,0) <> 0 then  ' Traff=' + CONVERT(VARCHAR(5),COALESCE(vw_LvNU.NU_Traffic_noise,0)) else '' end   
		--+ case when TransportationConcurrency = 'Y' then  ' TransprtConcur=' + TransportationConcurrency else '' end     
		+ case when COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_unbuildable',	dpl.ptas_unbuildable 	),'') = 'Yes' then  ' Unbuild=' + COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_unbuildable',	dpl.ptas_unbuildable 	),'') else '' end    
		+ case when COALESCE(vw_LvNU.NU_Water_problem,'') = 'Y' then  ' WtrProb=' + COALESCE(vw_LvNU.NU_Water_problem,'') else '' end    
		+ case when UPPER(COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_land','ptas_watersystem',  dpl.ptas_watersystem 	),'')) LIKE '%RESTRICTED%' then ' RestrWtrSys=Y' else '' end     
		+ case when COALESCE(vw_LvER.ER_Wetland ,'') <> '' then  ' WetLnd=' + COALESCE(vw_LvER.ER_Wetland ,'') else '' end  
		+ case when COALESCE(vw_LvWF.WF_ptas_poorquality,'') = 'Y' then  ' WfntPoor=' + COALESCE(vw_LvWF.WF_ptas_poorquality,'') else '' end     
		,LandProbDescrPart2 = ''
	/*
	Hairo comment: code to add SpecApplDistrict, I´m waiting for a clarification on Area-sub areas in order to continue with this
	,SpecApplDistrict = CASE 
                            	WHEN dpd.ptas_specialtyarea > 0  THEN 'Cml' + [dynamics].[fn_GetValueFromStringMap]('ptas_parceldetail','ptas_commercialdistrict', dpd.ptas_commercialdistrict	)  
                           	    ELSE ''
                            END
							
  LEFT JOIN [dynamics].[ptas_specialtyarea] pg
			INNER JOIN [dynamics].[ptas_specialtyneighborhood] gn ON pg.ptas_specialtyareaid = gn._ptas_specialtyareaid_value
   ON pg.ptas_specialtyareaid = dpd.ptas_specialtyarea
   */
--**END NEW COLUMNS **************************************************************************************************************************************************************		
  FROM dynamics.ptas_parceldetail (NOLOCK) dpd 
 INNER JOIN dynamics.ptas_land (NOLOCK) dpl
    ON dpd._ptas_landid_value = dpl.ptas_landid
 INNER JOIN dynamics.ptas_propertytype AS pt ON dpd._ptas_propertytypeid_value = pt.ptas_propertytypeid
  LEFT JOIN dynamics.ptas_neighborhood (NOLOCK) dpn
    ON dpn.ptas_neighborhoodid = dpd._ptas_neighborhoodid_value

  /*
  Hairo comment: this view [dynamics].[vw_InspectionHistory], requires further validation, there is a probelm, for reveral
				 parcels come 2 records with different ptas_inspectiontype for the same year, and I´m not sure what records
				 should be presented in GisMapData, by now I´m presenting the most recent record, that´s is why I create the view.
  */	
  LEFT JOIN [dynamics].[vw_InspectionHistory] (NOLOCK) dpi
	ON dpi.ptas_parcelid = dpd.ptas_parceldetailid
   AND dpi.AssmtYr = (dynamics.fn_ReturnMetaData('AssessmentYear','GlobalConstant'))

  LEFT JOIN [dynamics].[vw_InspectionYear] (NOLOCK) dpi2
	ON dpi2.ptas_parcelid = dpd.ptas_parceldetailid
  
  --ResArea
  LEFT JOIN dynamics.ptas_area pa ON dpd._ptas_areaid_value = pa.ptas_areaid


  LEFT JOIN [dynamics].[ptas_geoarea] (NOLOCK) pga
    ON pga.ptas_geoareaid = dpd._ptas_geoareaid_value
  LEFT JOIN [dynamics].[ptas_geoneighborhood] (NOLOCK) pgn
    ON pgn.ptas_geoneighborhoodid = dpd._ptas_geonbhdid_value
  LEFT JOIN [dynamics].[ptas_specialtyarea] (NOLOCK) psa
    ON psa.ptas_specialtyareaid = dpd._ptas_specialtyareaid_value
  LEFT JOIN [dynamics].[ptas_specialtyneighborhood] (NOLOCK) psn
    ON psn.ptas_specialtyneighborhoodid = dpd._ptas_specialtynbhdid_value
     
  LEFT JOIN [dynamics].[vw_LandValueDesignations] (NOLOCK) vw_LvDes
	ON vw_LvDes._ptas_landid_value = dpd._ptas_landid_value

  LEFT JOIN [dynamics].[vw_LandValueDevelopmentRight] (NOLOCK) vw_LvDR
	ON vw_LvDR._ptas_landid_value = dpd._ptas_landid_value	

  LEFT JOIN [dynamics].[vw_LandValueEnvironmentalRestriction] (NOLOCK) vw_LvER
	ON vw_LvER._ptas_landid_value = dpd._ptas_landid_value
	
  LEFT JOIN [dynamics].[vw_LandValueNuisance] (NOLOCK) vw_LvNU
	ON vw_LvNU._ptas_landid_value = dpd._ptas_landid_value

  LEFT JOIN [dynamics].[vw_LandValueView] (NOLOCK) vw_LvVW
	ON vw_LvVW._ptas_landid_value = dpd._ptas_landid_value
		
  LEFT JOIN [dynamics].[vw_LandValueWaterFront] (NOLOCK) vw_LvWF
	ON vw_LvWF._ptas_landid_value = dpd._ptas_landid_value
	
  LEFT JOIN [dynamics].[ptas_parceleconomicunit] (NOLOCK) ppeu
	ON ppeu._ptas_parcelid_value = dpd.ptas_parceldetailid
  /* 
  LEFT JOIN [dynamics].[ptas_buildingdetail] pbd
	ON pbd._ptas_parceldetailid_value = dpd.ptas_parceldetailid
   AND ptas_buildingnumber = 1
   Hairo comment: This previous LEFT JOIN is replaced with the next one using the view dynamics.vw_BuildingNumberONE,
				  I create the view to avoid duplication, but it is necesary to check if the data was dirty and it will
				  cleaned at the end. In some cases there is more than one records with building number = it seems like an error in the data.
  */ 
  LEFT JOIN [dynamics].[vw_BuildingNumberONE] (NOLOCK) pbd
	ON pbd._ptas_parceldetailid_value = dpd.ptas_parceldetailid
 
  
  LEFT JOIN [dynamics].[ptas_year] (NOLOCK) py01
    ON py01.ptas_yearid = pbd._ptas_yearbuiltid_value
  LEFT JOIN [dynamics].[ptas_year] (NOLOCK) py02
    ON py02.ptas_yearid = pbd._ptas_yearrenovatedid_value	

  LEFT JOIN dynamics.vw_BestBuilding (NOLOCK) BestBldg
	ON BestBldg.[_ptas_parceldetailid_value] = dpd.ptas_parceldetailid  
	
  LEFT JOIN [dynamics].[vw_BuildingSqft] bldngsum
	ON bldngsum._ptas_parceldetailid_value = dpd.ptas_parceldetailid
	
  LEFT JOIN [dynamics].[vw_BuildingCount] pbdcount
    ON pbdcount._ptas_parceldetailid_value = dpd.ptas_parceldetailid

  LEFT JOIN dynamics.vw_LandValue (NOLOCK) apprh
	ON dpd.ptas_parceldetailid = apprh.ptas_parcelid
  AND  apprh.ptas_interfaceflag <> 15
   AND apprh.Year = (dynamics.fn_ReturnMetaData('AssessmentYear','GlobalConstant')) +1

  LEFT JOIN dynamics.vw_LandValue (NOLOCK) apprh02
 	ON dpd.ptas_parceldetailid = apprh02.ptas_parcelid
   AND apprh02.ptas_interfaceflag <> 15
   AND apprh02.Year = (dynamics.fn_ReturnMetaData('AssessmentYear','GlobalConstant'))
   
  LEFT JOIN [dynamics].[vw_AccyResList] (NOLOCK) vwArl
	ON vwArl._ptas_parceldetailid_value = dpd.ptas_parceldetailid
	
 LEFT JOIN [dynamics].[vw_ResBuildings] (NOLOCK) BldngQnt
	ON BldngQnt._ptas_parceldetailid_value = dpd.ptas_parceldetailid

--************
 LEFT JOIN dynamics.vw_SalesVerifiedYN (NOLOCK) uvs
	ON uvs.ptas_parceldetailid = dpd.ptas_parceldetailid
	
 LEFT JOIN dynamics.vw_AssignedTypes (NOLOCK) ats 
   ON ats.ParcelId = dpd.ptas_parceldetailid
  AND ats.AssmtYr = dynamics.fn_ReturnMetaData('AssessmentYear','GlobalConstant') 




WHERE dpd._ptas_landid_value IS NOT NULL
   AND (SUBSTRING(pt.ptas_name,1,1) <> 'K'
        OR (SUBSTRING(pt.ptas_name,1,1) = 'K' AND dpd.ptas_Minor = '0000') )
   AND COALESCE(dpd.ptas_splitcode,0) = 0
   AND (dpd.statecode  = 0 AND dpd.statuscode = 1) --This get the ACTIVE parcels. When "statecode  = 1 AND dpd.statuscode = 2" the parcel is Killed
   AND COALESCE(dpd.ptas_snapshottype,'') = '' 
 
 
/*
WHERE dpd._ptas_landid_value IS NOT NULL
   AND (dpd.ptas_proptype NOT IN ('K','Floating Home','Mobile Home')
        OR (dpd.ptas_proptype = 'K' AND dpd.ptas_Minor = '0000') )
   AND COALESCE(dpd.ptas_splitcode,0) = 0
   AND (dpd.statecode  = 0 AND dpd.statuscode = 1) --This get the ACTIVE parcels. When "statecode  = 1 AND dpd.statuscode = 2" the parcel is Killed
   AND COALESCE(dpd.ptas_snapshottype,'') = '' 
  
*/
