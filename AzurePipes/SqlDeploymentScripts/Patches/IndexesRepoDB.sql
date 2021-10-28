-- Reduce columns size
ALTER TABLE [dynamics].[ptas_parceldetail]
ALTER COLUMN [ptas_name] nvarchar(100) null
GO


-- Find an existing index named IX_PTAS_ParcelDetail_PTAS_Name and delete it if found.   
IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_ptas_parceldetail_ptas_name')   
    DROP INDEX IX_ptas_parceldetail_ptas_name ON  [dynamics].[ptas_parceldetail]
GO  
-- Create a nonclustered index
CREATE NONCLUSTERED INDEX IX_ptas_parceldetail_ptas_name   
    ON [dynamics].[ptas_parceldetail] ([ptas_name])
GO 

CREATE NONCLUSTERED INDEX [Idx_ptas_accessorydetail_modifiedon] ON [dynamics].[ptas_accessorydetail]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_accessorydetail_ptas_mediarepository_modifiedon] ON [dynamics].[ptas_accessorydetail_ptas_mediarepository]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_apartmentregion_modifiedon] ON [dynamics].[ptas_apartmentregion]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_apartmentsupergroup_modifiedon] ON [dynamics].[ptas_apartmentsupergroup]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_area_modifiedon] ON [dynamics].[ptas_area]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_bookmark_modifiedon] ON [dynamics].[ptas_bookmark]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_bookmarktag_modifiedon] ON [dynamics].[ptas_bookmarktag]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_buildingdetail_modifiedon] ON [dynamics].[ptas_buildingdetail]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_buildingdetail_commercialuse_modifiedon] ON [dynamics].[ptas_buildingdetail_commercialuse]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_buildingdetail_ptas_mediarepository_modifiedon] ON [dynamics].[ptas_buildingdetail_ptas_mediarepository]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_buildingsectionfeature_modifiedon] ON [dynamics].[ptas_buildingsectionfeature]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_buildingsectionuse_modifiedon] ON [dynamics].[ptas_buildingsectionuse]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_camanotes_modifiedon] ON [dynamics].[ptas_camanotes]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_city_modifiedon] ON [dynamics].[ptas_city]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_city_county_modifiedon] ON [dynamics].[ptas_city_county]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_city_stateorprovince_modifiedon] ON [dynamics].[ptas_city_stateorprovince]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_city_zipcode_modifiedon] ON [dynamics].[ptas_city_zipcode]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_condocomplex_modifiedon] ON [dynamics].[ptas_condocomplex]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_condocomplex_ptas_mediarepository_modifiedon] ON [dynamics].[ptas_condocomplex_ptas_mediarepository]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_condounit_modifiedon] ON [dynamics].[ptas_condounit]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_condounit_ptas_mediarepository_modifiedon] ON [dynamics].[ptas_condounit_ptas_mediarepository]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_country_modifiedon] ON [dynamics].[ptas_country]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_county_modifiedon] ON [dynamics].[ptas_county]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_county_stateorprovince_modifiedon] ON [dynamics].[ptas_county_stateorprovince]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_county_zipcode_modifiedon] ON [dynamics].[ptas_county_zipcode]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_district_modifiedon] ON [dynamics].[ptas_district]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_economicunit_modifiedon] ON [dynamics].[ptas_economicunit]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_economicunit_accessorydetail_modifiedon] ON [dynamics].[ptas_economicunit_accessorydetail]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_fileattachmentmetadata_modifiedon] ON [dynamics].[ptas_fileattachmentmetadata]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_geoarea_modifiedon] ON [dynamics].[ptas_geoarea]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_geoneighborhood_modifiedon] ON [dynamics].[ptas_geoneighborhood]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_housingprogram_modifiedon] ON [dynamics].[ptas_housingprogram]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_inspectionhistory_modifiedon] ON [dynamics].[ptas_inspectionhistory]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_jurisdiction_modifiedon] ON [dynamics].[ptas_jurisdiction]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_land_modifiedon] ON [dynamics].[ptas_land]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_land_ptas_mediarepository_modifiedon] ON [dynamics].[ptas_land_ptas_mediarepository]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_landuse_modifiedon] ON [dynamics].[ptas_landuse]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_landvaluebreakdown_modifiedon] ON [dynamics].[ptas_landvaluebreakdown]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_landvaluecalculation_modifiedon] ON [dynamics].[ptas_landvaluecalculation]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_levycode_modifiedon] ON [dynamics].[ptas_levycode]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_lowincomehousingprogram_modifiedon] ON [dynamics].[ptas_lowincomehousingprogram]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_masspayer_modifiedon] ON [dynamics].[ptas_masspayer]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_mediarepository_modifiedon] ON [dynamics].[ptas_mediarepository]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_naicscode_modifiedon] ON [dynamics].[ptas_naicscode]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_neighborhood_modifiedon] ON [dynamics].[ptas_neighborhood]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_parceldetail_modifiedon] ON [dynamics].[ptas_parceldetail]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_parceldetail_ptas_mediarepository_modifiedon] ON [dynamics].[ptas_parceldetail_ptas_mediarepository]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_parceleconomicunit_modifiedon] ON [dynamics].[ptas_parceleconomicunit]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_parkingdistrict_modifiedon] ON [dynamics].[ptas_parkingdistrict]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_permit_modifiedon] ON [dynamics].[ptas_permit]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_permitinspectionhistory_modifiedon] ON [dynamics].[ptas_permitinspectionhistory]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_permitwebsiteconfig_modifiedon] ON [dynamics].[ptas_permitwebsiteconfig]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_projectdock_modifiedon] ON [dynamics].[ptas_projectdock]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_propertytype_modifiedon] ON [dynamics].[ptas_propertytype]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_ptas_bookmark_ptas_bookmarktag_modifiedon] ON [dynamics].[ptas_ptas_bookmark_ptas_bookmarktag]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_ptas_camanotes_ptas_fileattachmentmetad_modifiedon] ON [dynamics].[ptas_ptas_camanotes_ptas_fileattachmentmetad]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_ptas_sales_ptas_fileattachmentmetadata_modifiedon] ON [dynamics].[ptas_ptas_sales_ptas_fileattachmentmetadata]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_ptasconfiguration_modifiedon] ON [dynamics].[ptas_ptasconfiguration]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_ptassetting_modifiedon] ON [dynamics].[ptas_ptassetting]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_qstr_modifiedon] ON [dynamics].[ptas_qstr]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_recentparcel_modifiedon] ON [dynamics].[ptas_recentparcel]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_responsibility_modifiedon] ON [dynamics].[ptas_responsibility]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_salepriceadjustment_modifiedon] ON [dynamics].[ptas_salepriceadjustment]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_sales_modifiedon] ON [dynamics].[ptas_sales]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_sales_parceldetail_parcelsinsale_modifiedon] ON [dynamics].[ptas_sales_parceldetail_parcelsinsale]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_sales_ptas_saleswarningcode_modifiedon] ON [dynamics].[ptas_sales_ptas_saleswarningcode]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_salesnote_modifiedon] ON [dynamics].[ptas_salesnote]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_saleswarningcode_modifiedon] ON [dynamics].[ptas_saleswarningcode]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_sectionusesqft_modifiedon] ON [dynamics].[ptas_sectionusesqft]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_sketch_modifiedon] ON [dynamics].[ptas_sketch]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_specialtyarea_modifiedon] ON [dynamics].[ptas_specialtyarea]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_specialtyneighborhood_modifiedon] ON [dynamics].[ptas_specialtyneighborhood]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_stateorprovince_modifiedon] ON [dynamics].[ptas_stateorprovince]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_streetname_modifiedon] ON [dynamics].[ptas_streetname]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_streettype_modifiedon] ON [dynamics].[ptas_streettype]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_subarea_modifiedon] ON [dynamics].[ptas_subarea]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_submarket_modifiedon] ON [dynamics].[ptas_submarket]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_supergroup_modifiedon] ON [dynamics].[ptas_supergroup]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_taxaccount_modifiedon] ON [dynamics].[ptas_taxaccount]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_unitbreakdown_modifiedon] ON [dynamics].[ptas_unitbreakdown]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_unitbreakdowntype_modifiedon] ON [dynamics].[ptas_unitbreakdowntype]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_visitedsketch_modifiedon] ON [dynamics].[ptas_visitedsketch]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_year_modifiedon] ON [dynamics].[ptas_year]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_zipcode_modifiedon] ON [dynamics].[ptas_zipcode]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_zipcode_stateorprovince_modifiedon] ON [dynamics].[ptas_zipcode_stateorprovince]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_zoning_modifiedon] ON [dynamics].[ptas_zoning]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_role_modifiedon] ON [dynamics].[role]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_stringmap_modifiedon] ON [dynamics].[stringmap]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_systemuser_modifiedon] ON [dynamics].[systemuser]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_systemuserroles_modifiedon] ON [dynamics].[systemuserroles]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_team_modifiedon] ON [dynamics].[team]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_teammembership_modifiedon] ON [dynamics].[teammembership]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_teamprofiles_modifiedon] ON [dynamics].[teamprofiles]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_teamroles_modifiedon] ON [dynamics].[teamroles]
(
	[modifiedon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

/*Michael Shreiber's indexes*/
/****** Object:  Index [IX_ptas_parceldetailid_value]    Script Date: 8/20/2021 3:05:05 PM ******/
CREATE NONCLUSTERED INDEX [idx_StateCodeParceldetailId_value] ON [dynamics].[ptas_buildingdetail]
(
	 [_ptas_parceldetailid_value] ASC, statecode
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


/****** Object:  Index [IX_ptas_parceldetailid_value]    Script Date: 8/20/2021 3:12:16 PM ******/
CREATE NONCLUSTERED INDEX [idx_StateCodeParceldetailid_value] ON [dynamics].[ptas_accessorydetail]
(
	 [_ptas_parceldetailid_value] ASC, statecode
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


/****** Object:  Index [IX_ptas_parcelid_value]    Script Date: 8/20/2021 3:13:43 PM ******/
CREATE NONCLUSTERED INDEX [idx_StatecodeStatuscodeParcelid_value] ON [dynamics].[ptas_permit]
(
	[_ptas_parcelid_value] ASC, Statecode,Statuscode
)
INCLUDE([ptas_permitvalue]) WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


/****** Object:  Index [ix_ptas_condounit_ptas_parcelid_value]    Script Date: 8/20/2021 3:19:15 PM ******/
CREATE NONCLUSTERED INDEX [idx_Parcelid_value] ON [dynamics].[ptas_condounit]
(
	[_ptas_parcelid_value] ASC, statecode, ptas_unittype, ptas_condo, _ptas_complexid_value
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


/****** Object:  Index [IX_ptas_condocomplex_ptas_parcelid_value]    Script Date: 8/20/2021 3:21:56 PM ******/
CREATE NONCLUSTERED INDEX [idx_StatecodeParcelid_value] ON [dynamics].[ptas_condocomplex]
(
	[_ptas_parcelid_value] ASC, statecode
)
 WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_statecode_statuscode_ptas_snapshottype_ptas_minor]    Script Date: 8/20/2021 3:30:49 PM ******/
CREATE NONCLUSTERED INDEX [idx_MasterParcelSnapshottypeStatecode] ON [dynamics].[ptas_parceldetail]
(
	_ptas_masterparcelid_value,
	[ptas_snapshottype] ASC,
	[statecode] ASC
)
WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


/****** Object:  Index [IX_ptas_parcelid_value]    Script Date: 8/20/2021 3:34:48 PM ******/
CREATE NONCLUSTERED INDEX [idx_Parcelid_valueStatecode] ON [dynamics].[ptas_camanotes]
(
	[_ptas_parcelid_value] ASC,
	Statecode
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


CREATE INDEX idx_PriParcelId
ON [dynamics].[ptas_sales]
(
	_ptas_primaryparcelid_value
)
GO

CREATE INDEX idx_MstrParcelId
ON [dynamics].[ptas_parceldetail_snapshot]
(
	_ptas_masterparcelid_value
)
GO