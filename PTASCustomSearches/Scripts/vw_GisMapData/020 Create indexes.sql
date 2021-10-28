IF EXISTS (SELECT * FROM sys.indexes WHERE name='Idx_ptas_name' AND object_id = OBJECT_ID('dynamics.ptas_year'))
	DROP INDEX Idx_ptas_name ON [dynamics].[ptas_year];
GO	

CREATE  NONCLUSTERED INDEX Idx_ptas_name ON [dynamics].[ptas_year]
(
[ptas_name] asc
)
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ApplGroup' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ApplGroup ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_ApplGroup ON [dynamics].[ptas_parceldetail]
(
[ptas_proptype], [ptas_applgroup] asc 
)
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_snapshottype' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_snapshottype ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_snapshottype ON [dynamics].[ptas_parceldetail]
(
[ptas_snapshottype]
)
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_state_status_code' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_state_status_code ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_state_status_code ON [dynamics].[ptas_parceldetail]
(
[statecode],[statuscode]
)
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ActiveParcels' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ActiveParcels  ON [dynamics].[ptas_parceldetail];
GO
CREATE NONCLUSTERED INDEX IX_ActiveParcels
ON [dynamics].[ptas_parceldetail] ([ptas_parceldetailid])
INCLUDE ([ptas_splitcode],[statecode],[statuscode],[ptas_snapshottype])
GO


IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_namesonaccount' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_namesonaccount  ON [dynamics].[ptas_parceldetail];
GO
CREATE NONCLUSTERED INDEX IX_ptas_namesonaccount
ON [dynamics].[ptas_parceldetail] ([ptas_namesonaccount])
INCLUDE ([ptas_parceldetailid])
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_propertyname' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_propertyname  ON [dynamics].[ptas_parceldetail];
GO
CREATE NONCLUSTERED INDEX IX_ptas_propertyname
ON [dynamics].[ptas_parceldetail] ([ptas_propertyname])
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_receivabletype' AND object_id = OBJECT_ID('ptas.ptas_taxrollhistory'))
	DROP INDEX IX_ptas_receivabletype  ON [ptas].[ptas_taxrollhistory];
GO
CREATE NONCLUSTERED INDEX IX_ptas_receivabletype
ON [ptas].[ptas_taxrollhistory] ([ptas_receivabletype])
INCLUDE ([ptas_omityearidname],[ptas_parcelid],[ptas_taxableimpvalue],[ptas_taxablelandvalue],[ptas_taxaccountid],[ptas_taxaccountidname],[ptas_taxyearidname],[ptas_TaxStat])
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_characteristictype06' AND object_id = OBJECT_ID('dynamics.ptas_landvaluecalculation'))
	DROP INDEX IX_ptas_characteristictype06  ON [dynamics].[ptas_landvaluecalculation];
GO
CREATE NONCLUSTERED INDEX IX_ptas_characteristictype06
ON [dynamics].[ptas_landvaluecalculation] ([ptas_characteristictype])
INCLUDE ([ptas_quality],[ptas_viewtype],[_ptas_landid_value])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_MultipleColumns01' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_MultipleColumns01  ON [dynamics].[ptas_parceldetail];
GO
CREATE NONCLUSTERED INDEX IX_MultipleColumns01
ON [dynamics].[ptas_parceldetail] ([ptas_applgroup],[ptas_proptype],[ptas_resarea])
INCLUDE ([ptas_folio],[ptas_major],[ptas_minor],[ptas_platblock],[ptas_platlot],[ptas_ressubarea],[_ptas_landid_value],[_ptas_neighborhoodid_value],[_ptas_qstrid_value])
GO

----*************************************

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_parceldetailid_value' AND object_id = OBJECT_ID('dynamics.ptas_accessorydetail'))
	DROP INDEX IX_ptas_parceldetailid_value ON [dynamics].[ptas_accessorydetail] ;
GO
CREATE NONCLUSTERED INDEX IX_ptas_parceldetailid_value ON [dynamics].[ptas_accessorydetail] 
(
[_ptas_parceldetailid_value]
)
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_resaccessorytype' AND object_id = OBJECT_ID('dynamics.ptas_accessorydetail'))
	DROP INDEX IX_ptas_resaccessorytype ON [dynamics].[ptas_accessorydetail];
GO
CREATE NONCLUSTERED INDEX IX_ptas_resaccessorytype ON [dynamics].[ptas_accessorydetail] 
(
[ptas_resaccessorytype]
)
INCLUDE ([_ptas_parceldetailid_value])
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_parcelid' AND object_id = OBJECT_ID('ptas.ptas_inspectionhistory'))
	DROP INDEX IX_ptas_parcelid ON [ptas].[ptas_inspectionhistory];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_parcelid ON [ptas].[ptas_inspectionhistory]
(
[ptas_parcelid] asc
)
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_inspectiondate' AND object_id = OBJECT_ID('ptas.ptas_inspectionhistory'))	
	DROP INDEX IX_ptas_inspectiondate ON [ptas].[ptas_inspectionhistory];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_inspectiondate ON [ptas].[ptas_inspectionhistory]
(
[ptas_inspectiondate] asc
)
INCLUDE ([ptas_parcelid])
GO
--required in PTAStrain
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_parceldetailid' AND object_id = OBJECT_ID('dynamics.ptas_sales_parceldetail_parcelsinsale'))
	DROP INDEX IX_ptas_parceldetailid ON [dynamics].[ptas_sales_parceldetail_parcelsinsale];
GO
CREATE NONCLUSTERED INDEX IX_ptas_parceldetailid ON [dynamics].[ptas_sales_parceldetail_parcelsinsale] 
(
[ptas_parceldetailid]
)

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_RpGuid' AND object_id = OBJECT_ID('rp.ParcelAssignment'))
	DROP INDEX IX_RpGuid  ON [rp].[ParcelAssignment];
GO
CREATE NONCLUSTERED INDEX IX_RpGuid ON [rp].[ParcelAssignment]
(
[RpGuid]
)
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_parcelid_value' AND object_id = OBJECT_ID('dynamics.ptas_permit'))
	DROP INDEX IX_ptas_parcelid_value ON [dynamics].[ptas_permit] ;
GO
CREATE NONCLUSTERED INDEX IX_ptas_parcelid_value
ON [dynamics].[ptas_permit] ([_ptas_parcelid_value])
INCLUDE ([ptas_permitvalue])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_LastBillYr' AND object_id = OBJECT_ID('dynamics.HomeImpApp'))
	DROP INDEX IX_LastBillYr ON [dynamics].[HomeImpApp] ;
GO
CREATE NONCLUSTERED INDEX IX_LastBillYr
ON [rp].[HomeImpApp] ([LastBillYr])
INCLUDE ([RpGuid],[HomeImpVal])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_parcelid_ptas_receivabletype' AND object_id = OBJECT_ID('dynamics.ptas_taxrollhistory'))
	DROP INDEX IX_ptas_parcelid_ptas_receivabletype ON [dynamics].[ptas_taxrollhistory] ;
GO
CREATE NONCLUSTERED INDEX IX_ptas_parcelid_ptas_receivabletype
ON [ptas].[ptas_taxrollhistory] ([ptas_parcelid],[ptas_receivabletype])
INCLUDE ([ptas_omityearidname],[ptas_taxableimpvalue],[ptas_taxablelandvalue],[ptas_taxaccountid],[ptas_taxaccountidname],[ptas_taxyearidname],[ptas_TaxStat])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_saleswarningcodeid' AND object_id = OBJECT_ID('dynamics.ptas_sales_ptas_saleswarningcode'))
	DROP INDEX IX_ptas_saleswarningcodeid ON [dynamics].[ptas_sales_ptas_saleswarningcode] ;
GO
CREATE NONCLUSTERED INDEX IX_ptas_saleswarningcodeid
ON [dynamics].[ptas_sales_ptas_saleswarningcode] ([ptas_saleswarningcodeid])
INCLUDE ([ptas_salesid])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_EventDate' AND object_id = OBJECT_ID('dynamics.ptas_changehistory'))
	DROP INDEX IX_EventDate ON [dynamics].[ptas_changehistory] ;
GO
CREATE NONCLUSTERED INDEX IX_EventDate
ON [ptas].[ptas_changehistory] ([EventDate])
INCLUDE ([parcelGuid])
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_resarea' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_resarea ON [dynamics].[ptas_parceldetail] ;
GO
CREATE NONCLUSTERED INDEX IX_ptas_resarea
ON [dynamics].[ptas_parceldetail] ([ptas_resarea])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_mobilehometype_ptas_parcelid_value' AND object_id = OBJECT_ID('dynamics.ptas_condounit'))
	DROP INDEX IX_ptas_mobilehometype_ptas_parcelid_value ON [dynamics].[ptas_condounit] ;
GO
CREATE NONCLUSTERED INDEX IX_ptas_mobilehometype_ptas_parcelid_value
ON [dynamics].[ptas_condounit] ([ptas_mobilehometype],[_ptas_parcelid_value])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_parcelid_value_ptas_mobilehometype' AND object_id = OBJECT_ID('dynamics.ptas_condounit'))
	DROP INDEX IX_ptas_parcelid_value_ptas_mobilehometype ON [dynamics].[ptas_condounit] ;
GO
CREATE NONCLUSTERED INDEX IX_ptas_parcelid_value_ptas_mobilehometype
ON [dynamics].[ptas_condounit] ([_ptas_parcelid_value],[ptas_mobilehometype])
INCLUDE ([ptas_buildingnumber],[ptas_legacyunitid],[ptas_length],[ptas_mobilehomeclass],[ptas_mobilehomecondition],[ptas_percentnetcondition],[ptas_roomadditionalsqft],[ptas_size],[ptas_tipoutarea],[ptas_totalliving],[ptas_width],[_ptas_yearbuildid_value])
GO

/*
INDEXES 030
*/


/*
INDEXES FOR [dynamics].[ptas_parceldetail]
*/
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_landid_value' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_landid_value ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_landid_value ON [dynamics].[ptas_parceldetail]
(
[_ptas_landid_value] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_neighborhoodid_value' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_neighborhoodid_value ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_neighborhoodid_value ON [dynamics].[ptas_parceldetail]
(
[_ptas_neighborhoodid_value] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_subareaid_value' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_subareaid_value ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_subareaid_value ON [dynamics].[ptas_parceldetail]
(
[_ptas_subareaid_value] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_areaid_value' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_areaid_value ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_areaid_value ON [dynamics].[ptas_parceldetail]
(
[_ptas_areaid_value] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_geoareaid_value' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_geoareaid_value ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_geoareaid_value ON [dynamics].[ptas_parceldetail]
(
[_ptas_geoareaid_value] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_geonbhdid_value' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_geonbhdid_value ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_geonbhdid_value ON [dynamics].[ptas_parceldetail]
(
[_ptas_geonbhdid_value] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_specialtyareaid_value' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_specialtyareaid_value ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_specialtyareaid_value ON [dynamics].[ptas_parceldetail]
(
[_ptas_specialtyareaid_value] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_specialtynbhdid_value' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_specialtynbhdid_value ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_specialtynbhdid_value ON [dynamics].[ptas_parceldetail]
(
[_ptas_specialtynbhdid_value] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_splitaccount1id_value' AND object_id = OBJECT_ID('dynamics.ptas_parceldetail'))
	DROP INDEX IX_ptas_splitaccount1id_value ON [dynamics].[ptas_parceldetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_splitaccount1id_value ON [dynamics].[ptas_parceldetail]
(
[_ptas_splitaccount1id_value] asc
)
GO

/*
INDEXES FOR [dynamics].[ptas_buildingdetail]
*/
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_parceldetailid_value' AND object_id = OBJECT_ID('dynamics.ptas_buildingdetail'))
	DROP INDEX IX_ptas_parceldetailid_value ON [dynamics].[ptas_buildingdetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_parceldetailid_value ON [dynamics].[ptas_buildingdetail]
(
[_ptas_parceldetailid_value] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_effectiveyearid_value' AND object_id = OBJECT_ID('dynamics.ptas_buildingdetail'))
	DROP INDEX IX_ptas_effectiveyearid_value ON [dynamics].[ptas_buildingdetail];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_effectiveyearid_value ON [dynamics].[ptas_buildingdetail]
(
[_ptas_effectiveyearid_value] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_numberofbuildings' AND object_id = OBJECT_ID('dynamics.ptas_buildingdetail'))
	DROP INDEX IX_ptas_numberofbuildings ON [dynamics].[ptas_buildingdetail];
GO
CREATE NONCLUSTERED INDEX IX_ptas_numberofbuildings ON [dynamics].[ptas_buildingdetail] 
(
[ptas_numberofbuildings]
)
INCLUDE ([_ptas_parceldetailid_value])
GO
/*
INDEXES FOR [dynamics].[ptas_buildingdetail_commercialuse]
*/
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_buildingdetailid_value' AND object_id = OBJECT_ID('dynamics.ptas_buildingdetail_commercialuse'))
	DROP  INDEX IX_ptas_buildingdetailid_value ON [dynamics].[ptas_buildingdetail_commercialuse];
GO	
CREATE  NONCLUSTERED INDEX IX_ptas_buildingdetailid_value ON [dynamics].[ptas_buildingdetail_commercialuse]
(
[_ptas_buildingdetailid_value] asc
)
GO
/*
INDEXES FOR [ptas].[ptas_appraisalhistory]
*/
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_taxyearidname' AND object_id = OBJECT_ID('ptas.ptas_appraisalhistory'))
	DROP INDEX IX_ptas_taxyearidname ON [ptas].[ptas_appraisalhistory];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_taxyearidname ON [ptas].[ptas_appraisalhistory]
(
[ptas_taxyearidname] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_parcelid' AND object_id = OBJECT_ID('ptas.ptas_appraisalhistory'))
	DROP INDEX IX_ptas_parcelid ON [ptas].[ptas_appraisalhistory];
GO	
CREATE  NONCLUSTERED INDEX IX_ptas_parcelid ON [ptas].[ptas_appraisalhistory]
(
[ptas_parcelid] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_appraiserdate' AND object_id = OBJECT_ID('ptas.ptas_appraisalhistory'))
	DROP INDEX IX_ptas_appraiserdate ON [ptas].[ptas_appraisalhistory];
GO	
CREATE  NONCLUSTERED INDEX IX_ptas_appraiserdate ON [ptas].[ptas_appraisalhistory]
(
[ptas_appraiserdate] asc
)
GO

/*
INDEXES FOR [dynamics].[ptas_parceleconomicunit]
*/
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_parcelid_value' AND object_id = OBJECT_ID('dynamics.ptas_parceleconomicunit'))
	DROP INDEX IX_ptas_parcelid_value ON [dynamics].[ptas_parceleconomicunit];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_parcelid_value ON [dynamics].[ptas_parceleconomicunit]
(
[_ptas_parcelid_value] asc
)
/*
INDEXES FOR [dynamics].[ptas_landvaluecalculation]
*/
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_landid_value' AND object_id = OBJECT_ID('dynamics.ptas_landvaluecalculation'))
	DROP INDEX IX_ptas_landid_value ON [dynamics].[ptas_landvaluecalculation];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_landid_value ON [dynamics].[ptas_landvaluecalculation]
(
[_ptas_landid_value] asc
)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_characteristictype' AND object_id = OBJECT_ID('dynamics.ptas_landvaluecalculation'))
	DROP INDEX IX_ptas_characteristictype ON [dynamics].[ptas_landvaluecalculation];
GO
CREATE NONCLUSTERED INDEX IX_ptas_characteristictype ON [dynamics].[ptas_landvaluecalculation] 
(
[ptas_characteristictype]
)
INCLUDE ([ptas_designationtype],[ptas_percentadjustment],[_ptas_landid_value])
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_LandDesignations01' AND object_id = OBJECT_ID('dynamics.ptas_landvaluecalculation'))
	DROP INDEX IX_LandDesignations01 ON [dynamics].[ptas_landvaluecalculation];
GO
CREATE NONCLUSTERED INDEX IX_LandDesignations01 ON [dynamics].[ptas_landvaluecalculation] 
(
[ptas_characteristictype],[ptas_designationtype]
)
INCLUDE ([ptas_dollaradjustment],[_ptas_landid_value])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_characteristictype01' AND object_id = OBJECT_ID('dynamics.ptas_landvaluecalculation'))
	DROP INDEX IX_ptas_characteristictype01 ON [dynamics].[ptas_landvaluecalculation];
GO
CREATE NONCLUSTERED INDEX IX_ptas_characteristictype01 ON [dynamics].[ptas_landvaluecalculation] 
([ptas_characteristictype])
INCLUDE ([ptas_transfertype],[_ptas_landid_value])
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_characteristictype02' AND object_id = OBJECT_ID('dynamics.ptas_landvaluecalculation'))
	DROP INDEX IX_ptas_characteristictype02 ON [dynamics].[ptas_landvaluecalculation];
GO
CREATE NONCLUSTERED INDEX IX_ptas_characteristictype02 ON [dynamics].[ptas_landvaluecalculation] 
([ptas_characteristictype],[ptas_environmentalrestriction])
INCLUDE ([ptas_percentadjustment],[_ptas_landid_value])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_characteristictype03' AND object_id = OBJECT_ID('dynamics.ptas_landvaluecalculation'))
	DROP INDEX IX_ptas_characteristictype03 ON [dynamics].[ptas_landvaluecalculation];
GO
CREATE NONCLUSTERED INDEX IX_ptas_characteristictype03 ON [dynamics].[ptas_landvaluecalculation] 
([ptas_characteristictype],[ptas_nuisancetype])
INCLUDE ([ptas_dollaradjustment],[_ptas_landid_value])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_characteristictype04' AND object_id = OBJECT_ID('dynamics.ptas_landvaluecalculation'))
	DROP INDEX IX_ptas_characteristictype04 ON [dynamics].[ptas_landvaluecalculation];
GO
CREATE NONCLUSTERED INDEX IX_ptas_characteristictype04 ON [dynamics].[ptas_landvaluecalculation] 
([ptas_characteristictype])
INCLUDE ([ptas_noiselevel],[ptas_nuisancetype],[_ptas_landid_value])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_characteristictype05' AND object_id = OBJECT_ID('dynamics.ptas_landvaluecalculation'))
	DROP INDEX IX_ptas_characteristictype05 ON [dynamics].[ptas_landvaluecalculation];
GO
CREATE NONCLUSTERED INDEX IX_ptas_characteristictype05 ON [dynamics].[ptas_landvaluecalculation] 
([ptas_characteristictype],[ptas_nuisancetype])
INCLUDE ([ptas_percentadjustment],[_ptas_landid_value])
GO

/*
INDEXES FOR [dynamics].[ptas_permit]
*/

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_condounitid_value' AND object_id = OBJECT_ID('dynamics.ptas_permit'))
	DROP INDEX IX_ptas_condounitid_value ON [dynamics].[ptas_permit];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_condounitid_value ON [dynamics].[ptas_permit]
(
[_ptas_condounitid_value] asc
)


/**************************************************************************************
**************************************************************************************
									INDEXES FOR SEARCHES
**************************************************************************************
***************************************************************************************/

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_parcelid_value' AND object_id = OBJECT_ID('dynamics.ptas_camanotes'))
	DROP INDEX IX_ptas_parcelid_value ON [dynamics].[ptas_camanotes];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_parcelid_value ON [dynamics].[ptas_camanotes]
(
[_ptas_parcelid_value] asc
)
GO


IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_saleid_value' AND object_id = OBJECT_ID('dynamics.ptas_salesnote'))
	DROP INDEX IX_ptas_saleid_value ON [dynamics].[ptas_salesnote];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_saleid_value ON [dynamics].[ptas_salesnote]
(
[_ptas_saleid_value] asc
)
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_salesid' AND object_id = OBJECT_ID('dynamics.ptas_sales_ptas_saleswarningcode'))
	DROP INDEX IX_ptas_salesid ON [dynamics].[ptas_sales_ptas_saleswarningcode];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_salesid ON [dynamics].[ptas_sales_ptas_saleswarningcode]
(
[ptas_salesid] asc
)
GO


IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_parcelid' AND object_id = OBJECT_ID('ptas.ptas_taxrollhistory'))
	DROP INDEX IX_ptas_parcelid ON [ptas].[ptas_taxrollhistory];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_parcelid ON [ptas].[ptas_taxrollhistory]
(
[ptas_parcelid] asc
)

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_taxyearidname' AND object_id = OBJECT_ID('ptas.ptas_taxrollhistory'))
	DROP INDEX IX_ptas_taxyearidname ON [ptas].[ptas_taxrollhistory];
GO
CREATE  NONCLUSTERED INDEX IX_ptas_taxyearidname ON [ptas].[ptas_taxrollhistory]
(
[ptas_taxyearidname] asc
)



/**************************************************************************************
**************************************************************************************
									INDEXES M
**************************************************************************************
***************************************************************************************/
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='IX_ptas_sales_parceldetail_parcelsinsale_ptas_parceldetailid')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_ptas_sales_parceldetail_parcelsinsale_ptas_parceldetailid]
	ON [dynamics].[ptas_sales_parceldetail_parcelsinsale] ([ptas_parceldetailid])
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='IX_ptas_condounit_ptas_complexid_value')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_ptas_condounit_ptas_complexid_value]
	ON [dynamics].[ptas_condounit] ([_ptas_complexid_value])
	INCLUDE ([ptas_addr1_directionprefix],[ptas_addr1_directionsuffix],[ptas_addr1_streetnumber],[ptas_addr1_streetnumberfraction],[ptas_minornumber],[_ptas_addr1_streetnameid_value],[_ptas_addr1_streettypeid_value],[_ptas_parcelid_value])
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='IX_ptas_condocomplex_ptas_parcelid_value')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_ptas_condocomplex_ptas_parcelid_value]
	ON [dynamics].[ptas_condocomplex] ([_ptas_parcelid_value])
	INCLUDE ([ptas_legacyrpcomplexid],[ptas_name])
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='IX_ptas_condocomplex_ptas_parcelid_value')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_ptas_permit_ptas_parcelid_value]
	ON [dynamics].[ptas_permit] ([_ptas_parcelid_value])
	INCLUDE ([ptas_issueddate],[ptas_name],[ptas_permittype],[ptas_permitvalue])
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='IX_ptas_parceleconomicunit_ptas_economicunitid_value')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_ptas_parceleconomicunit_ptas_economicunitid_value]
	ON [dynamics].[ptas_parceleconomicunit] ([_ptas_economicunitid_value])
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='IX_ptas_changehistory_entity_attrib_DispName')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_ptas_changehistory_entity_attrib_DispName]
	ON [ptas].[ptas_changehistory] ([entityDispName],[attribDispName],[EventDate],[displayValueNew])
	INCLUDE ([parcelGuid])
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='IX_ptas_changehistory_entity_attrib_DispName')
BEGIN
	CREATE INDEX IX_ptas_camanotes_parcelId_minorparcelId_IdentityDisplay 
	ON dynamics.ptas_camanotes (_ptas_parcelid_value, _ptas_minorparcelid_value   ) 
	INCLUDE (ptas_attachedentitydisplayname,ptas_notetext)
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='Ix_ptas_parcelid_Taxyear_revalormaint_impvalue')
BEGIN
	CREATE NONCLUSTERED INDEX [Ix_ptas_parcelid_Taxyear_revalormaint_impvalue]  
	ON [ptas].[ptas_appraisalhistory](ptas_parcelid,ptas_taxyearidname,ptas_revalormaint,ptas_impvalue) 
	INCLUDE (ptas_landvalue,ptas_totalvalue,ptas_method,createdon,ptas_newconstruction,ptas_apprname,ptas_interfaceflag,ptas_valuationreason)
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='IX_ptas_ptas_bookmark_ptas_bookmarktag_ptas_bookmarkid')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_ptas_ptas_bookmark_ptas_bookmarktag_ptas_bookmarkid] ON [dynamics].[ptas_ptas_bookmark_ptas_bookmarktag] ([ptas_bookmarkid])
	INCLUDE ([ptas_bookmarktagid])
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='ix_ptas_condounit_ptas_parcelid_value')
BEGIN
	CREATE NONCLUSTERED INDEX [ix_ptas_condounit_ptas_parcelid_value] ON [dynamics].[ptas_condounit] ([_ptas_parcelid_value])
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='IX_ptas_parceldetail_applgroup_statecode_statuscode_snapshottype')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_ptas_parceldetail_applgroup_statecode_statuscode_snapshottype]
ON [dynamics].[ptas_parceldetail] ([ptas_applgroup],[statecode],[statuscode],[ptas_snapshottype])
INCLUDE ([ptas_major],[ptas_minor],[ptas_proptype],[ptas_resarea],[ptas_ressubarea],[ptas_splitcode],[_ptas_propertytypeid_value])
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='IX_appraisalhistory_realpropid_taxyearidname')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_appraisalhistory_realpropid_taxyearidname]
	ON [ptas].[ptas_appraisalhistory] ([ptas_realpropid],[ptas_taxyearidname])
	INCLUDE ([ptas_parcelid])
END
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes Where name ='IX_ReviewValSel_RollYr_ReleasedBy')
BEGIN
CREATE NONCLUSTERED INDEX [IX_ReviewValSel_RollYr_ReleasedBy]
ON [rp].[ReviewValSel] ([RollYr],[ReleasedBy])
INCLUDE ([LandId],[RealPropId],[SelectReviewed],[SelectReviewDate],[ReviewedBy],[ReviewDate],[ReleaseDate],[MsgId])
END
GO
--****************************************************************

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_inspectiondate_ptas_inspectiontype' AND object_id = OBJECT_ID('dynamics.ptas_inspectionhistory'))
	DROP INDEX IX_ptas_inspectiondate_ptas_inspectiontype ON [dynamics].[ptas_inspectionhistory];
GO
CREATE NONCLUSTERED INDEX IX_ptas_inspectiondate_ptas_inspectiontype
ON [ptas].[ptas_inspectionhistory] ([ptas_inspectiondate],[ptas_inspectiontype])
INCLUDE ([ptas_name],[ptas_parcelid])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_bookmarktagid' AND object_id = OBJECT_ID('dynamics.ptas_ptas_bookmark_ptas_bookmarktag'))
	DROP INDEX IX_ptas_bookmarktagid ON [dynamics].[ptas_ptas_bookmark_ptas_bookmarktag];
GO
CREATE NONCLUSTERED INDEX IX_ptas_bookmarktagid
ON [dynamics].[ptas_ptas_bookmark_ptas_bookmarktag] ([ptas_bookmarktagid])
INCLUDE ([ptas_bookmarkid])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_inspectiontype_ptas_year' AND object_id = OBJECT_ID('dynamics.ptas_inspectionyear'))
	DROP INDEX IX_ptas_inspectiontype_ptas_year ON [dynamics].[ptas_inspectionyear];
GO
CREATE NONCLUSTERED INDEX IX_ptas_inspectiontype_ptas_year
ON [dynamics].[ptas_inspectionyear] ([ptas_inspectiontype],[ptas_year])
INCLUDE ([_ptas_area_value])
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_imagetype_statecode_statuscode' AND object_id = OBJECT_ID('dynamics.ptas_mediarepository'))
	DROP INDEX IX_ptas_imagetype_statecode_statuscode ON [dynamics].[ptas_mediarepository];
GO
CREATE NONCLUSTERED INDEX IX_ptas_imagetype_statecode_statuscode
ON [dynamics].[ptas_mediarepository] ([ptas_imagetype],[statecode],[statuscode])
INCLUDE ([createdon],[ptas_fileextension])
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_AssmtYr_AssignmentTypeItemId' AND object_id = OBJECT_ID('rp.ParcelAssignment'))
	DROP INDEX IX_AssmtYr_AssignmentTypeItemId ON [rp].[ParcelAssignment];
GO
CREATE NONCLUSTERED INDEX IX_AssmtYr_AssignmentTypeItemId
ON [rp].[ParcelAssignment] ([AssmtYr],[AssignmentTypeItemId])
INCLUDE ([RpGuid],[AssignedToGuid])
GO

--****************************


IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_inspectiondate_ptas_inspectiontype' AND object_id = OBJECT_ID('dynamics.ptas_inspectionhistory'))
	DROP INDEX IX_ptas_inspectiondate_ptas_inspectiontype ON [dynamics].[ptas_inspectionhistory];
GO
CREATE NONCLUSTERED INDEX IX_ptas_inspectiondate_ptas_inspectiontype
ON [ptas].[ptas_inspectionhistory] ([ptas_inspectiondate],[ptas_inspectiontype])
INCLUDE ([ptas_name],[ptas_parcelid])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_bookmarktagid' AND object_id = OBJECT_ID('dynamics.ptas_ptas_bookmark_ptas_bookmarktag'))
	DROP INDEX IX_ptas_bookmarktagid ON [dynamics].[ptas_ptas_bookmark_ptas_bookmarktag];
GO
CREATE NONCLUSTERED INDEX IX_ptas_bookmarktagid
ON [dynamics].[ptas_ptas_bookmark_ptas_bookmarktag] ([ptas_bookmarktagid])
INCLUDE ([ptas_bookmarkid])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_inspectiontype_ptas_year' AND object_id = OBJECT_ID('dynamics.ptas_inspectionyear'))
	DROP INDEX IX_ptas_inspectiontype_ptas_year ON [dynamics].[ptas_inspectionyear];
GO
CREATE NONCLUSTERED INDEX IX_ptas_inspectiontype_ptas_year
ON [dynamics].[ptas_inspectionyear] ([ptas_inspectiontype],[ptas_year])
INCLUDE ([_ptas_area_value])
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_imagetype_statecode_statuscode' AND object_id = OBJECT_ID('dynamics.ptas_mediarepository'))
	DROP INDEX IX_ptas_imagetype_statecode_statuscode ON [dynamics].[ptas_mediarepository];
GO
CREATE NONCLUSTERED INDEX IX_ptas_imagetype_statecode_statuscode
ON [dynamics].[ptas_mediarepository] ([ptas_imagetype],[statecode],[statuscode])
INCLUDE ([createdon],[ptas_fileextension])
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_AssmtYr_AssignmentTypeItemId' AND object_id = OBJECT_ID('rp.ParcelAssignment'))
	DROP INDEX IX_AssmtYr_AssignmentTypeItemId ON [rp].[ParcelAssignment];
GO
CREATE NONCLUSTERED INDEX IX_AssmtYr_AssignmentTypeItemId
ON [rp].[ParcelAssignment] ([AssmtYr],[AssignmentTypeItemId])
INCLUDE ([RpGuid],[AssignedToGuid])
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_accessorydetailid' AND object_id = OBJECT_ID('dynamics.ptas_accessorydetail_ptas_mediarepository'))
	DROP INDEX IX_ptas_accessorydetailid ON [dynamics].[ptas_accessorydetail_ptas_mediarepository];
GO
CREATE NONCLUSTERED INDEX IX_ptas_accessorydetailid
ON [dynamics].[ptas_accessorydetail_ptas_mediarepository] ([ptas_accessorydetailid])
INCLUDE ([ptas_mediarepositoryid])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_landid' AND object_id = OBJECT_ID('dynamics.ptas_land_ptas_mediarepository'))
	DROP INDEX IX_ptas_landid ON [dynamics].[ptas_land_ptas_mediarepository];
GO
CREATE NONCLUSTERED INDEX IX_ptas_landid
ON [dynamics].[ptas_land_ptas_mediarepository] ([ptas_landid])
INCLUDE ([ptas_mediarepositoryid])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_buildingdetailid' AND object_id = OBJECT_ID('dynamics.ptas_buildingdetail_ptas_mediarepository'))
	DROP INDEX IX_ptas_buildingdetailid ON [dynamics].[ptas_buildingdetail_ptas_mediarepository];
GO
CREATE NONCLUSTERED INDEX IX_ptas_buildingdetailid
ON [dynamics].[ptas_buildingdetail_ptas_mediarepository] ([ptas_buildingdetailid])
INCLUDE ([ptas_mediarepositoryid])
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_ptas_mediarepositoryid' AND object_id = OBJECT_ID('dynamics.ptas_buildingdetail_ptas_mediarepository'))
	DROP INDEX IX_ptas_mediarepositoryid ON [dynamics].[ptas_buildingdetail_ptas_mediarepository];
GO
CREATE NONCLUSTERED INDEX IX_ptas_mediarepositoryid
ON [dynamics].[ptas_buildingdetail_ptas_mediarepository] ([ptas_mediarepositoryid])
INCLUDE ([ptas_buildingdetailid])
GO