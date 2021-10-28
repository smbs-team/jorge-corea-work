CREATE NONCLUSTERED INDEX [Idx_ptas_accessorydetail_key_ptas_accessorydetailid] ON [dbo].[Ptas_accessorydetail_Key]
(
	[ptas_accessorydetailid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_accessorydetail_ptas_mediarepository_Key_ptas_accessorydetail_ptas_mediarepositoryid] ON [dbo].[Ptas_accessorydetail_ptas_mediarepository_Key]
(
	[ptas_accessorydetail_ptas_mediarepositoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_apartmentregion_key_ptas_apartmentregionid] ON [dbo].[Ptas_apartmentregion_Key]
(
	[ptas_apartmentregionid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_apartmentsupergroup_key_ptas_apartmentsupergroupid] ON [dbo].[Ptas_apartmentsupergroup_Key]
(
	[ptas_apartmentsupergroupid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_appraisalhistory_Key_appraisalHistoryGuid] ON [dbo].[Ptas_appraisalhistory_Key]
(
	[appraisalHistoryGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_area_key_ptas_areaid] ON [dbo].[Ptas_area_Key]
(
	[ptas_areaid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_bookmark_key_ptas_bookmarkid] ON [dbo].[Ptas_bookmark_Key]
(
	[ptas_bookmarkid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_bookmarktag_key_ptas_bookmarktagid] ON [dbo].[Ptas_bookmarktag_Key]
(
	[ptas_bookmarktagid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_buildingdetail_commercialuse_key_ptas_buildingdetail_commercialuseid] ON [dbo].[Ptas_buildingdetail_commercialuse_Key]
(
	[ptas_buildingdetail_commercialuseid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_buildingdetail_key_ptas_buildingdetailid] ON [dbo].[Ptas_buildingdetail_Key]
(
	[ptas_buildingdetailid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_buildingdetail_ptas_mediarepository_Key_ptas_buildingdetail_ptas_mediarepositoryid] ON [dbo].[Ptas_buildingdetail_ptas_mediarepository_Key]
(
	[ptas_buildingdetail_ptas_mediarepositoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_buildingsectionfeature_key_ptas_buildingsectionfeatureid] ON [dbo].[Ptas_buildingsectionfeature_Key]
(
	[ptas_buildingsectionfeatureid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_buildingsectionuse_key_ptas_buildingsectionuseid] ON [dbo].[Ptas_buildingsectionuse_Key]
(
	[ptas_buildingsectionuseid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_camanotes_key_ptas_camanotesid] ON [dbo].[Ptas_camanotes_Key]
(
	[ptas_camanotesid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_camanotes_ptas_mediarepository_Key_ptas_camanotes_ptas_mediarepositoryid] ON [dbo].[Ptas_camanotes_ptas_mediarepository_Key]
(
	[ptas_camanotes_ptas_mediarepositoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_city_key_ptas_cityid] ON [dbo].[Ptas_city_Key]
(
	[ptas_cityid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_condocomplex_key_ptas_condocomplexid] ON [dbo].[Ptas_condocomplex_Key]
(
	[ptas_condocomplexid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_condocomplex_ptas_mediarepository_Key_ptas_condocomplex_ptas_mediarepositoryid] ON [dbo].[Ptas_condocomplex_ptas_mediarepository_Key]
(
	[ptas_condocomplex_ptas_mediarepositoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_condounit_key_ptas_condounitid] ON [dbo].[Ptas_condounit_Key]
(
	[ptas_condounitid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_condounit_ptas_mediarepository_Key_ptas_condounit_ptas_mediarepositoryid] ON [dbo].[Ptas_condounit_ptas_mediarepository_Key]
(
	[ptas_condounit_ptas_mediarepositoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_country_key_ptas_countryid] ON [dbo].[Ptas_country_Key]
(
	[ptas_countryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_district_key_ptas_districtid] ON [dbo].[Ptas_district_Key]
(
	[ptas_districtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_economicunit_key_ptas_economicunitid] ON [dbo].[Ptas_economicunit_Key]
(
	[ptas_economicunitid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_estimatehistory_Key_estimateHistoryGuid] ON [dbo].[Ptas_estimatehistory_Key]
(
	[estimateHistoryGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_geoarea_Key_ptas_geoareaid] ON [dbo].[Ptas_geoarea_Key]
(
	[ptas_geoareaid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_housingprogram_key_ptas_housingprogramid] ON [dbo].[Ptas_housingprogram_Key]
(
	[ptas_housingprogramid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_inspectionhistory_key_ptas_inspectionhistoryid] ON [dbo].[Ptas_inspectionhistory_Key]
(
	[ptas_inspectionhistoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_jurisdiction_Key_ptas_jurisdictionid] ON [dbo].[Ptas_jurisdiction_Key]
(
	[ptas_jurisdictionid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_land_key_ptas_landid] ON [dbo].[Ptas_land_Key]
(
	[ptas_landid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_land_ptas_mediarepository_Key_ptas_land_ptas_mediarepositoryid] ON [dbo].[Ptas_land_ptas_mediarepository_Key]
(
	[ptas_land_ptas_mediarepositoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_landuse_key_ptas_landuseid] ON [dbo].[Ptas_landuse_Key]
(
	[ptas_landuseid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_landvaluecalculation_key_ptas_landvaluecalculationid] ON [dbo].[Ptas_landvaluecalculation_Key]
(
	[ptas_landvaluecalculationid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_levycode_key_ptas_levycodeid] ON [dbo].[Ptas_levycode_Key]
(
	[ptas_levycodeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_lowincomehousingprogram_key_ptas_lowincomehousingprogramid] ON [dbo].[Ptas_lowincomehousingprogram_Key]
(
	[ptas_lowincomehousingprogramid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_mediarepository_Key_ptas_mediarepositoryid] ON [dbo].[Ptas_mediarepository_Key]
(
	[ptas_mediarepositoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_neighborhood_key_ptas_neighborhoodid] ON [dbo].[Ptas_neighborhood_Key]
(
	[ptas_neighborhoodid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_parceldetail_key_ptas_parceldetailid] ON [dbo].[Ptas_parceldetail_Key]
(
	[ptas_parceldetailid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_parceldetail_ptas_mediarepository_Key_ptas_parceldetail_ptas_mediarepositoryid] ON [dbo].[Ptas_parceldetail_ptas_mediarepository_Key]
(
	[ptas_parceldetail_ptas_mediarepositoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_parceleconomicunit_key_ptas_parceleconomicunitid] ON [dbo].[Ptas_parceleconomicunit_Key]
(
	[ptas_parceleconomicunitid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_parkingdistrict_key_ptas_parkingdistrictid] ON [dbo].[Ptas_parkingdistrict_Key]
(
	[ptas_parkingdistrictid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_permit_key_ptas_permitid] ON [dbo].[Ptas_permit_Key]
(
	[ptas_permitid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_projectdock_key_ptas_projectdockid] ON [dbo].[Ptas_projectdock_Key]
(
	[ptas_projectdockid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_propertytype_key_ptas_propertytypeid] ON [dbo].[Ptas_propertytype_Key]
(
	[ptas_propertytypeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_ptas_bookmark_ptas_bookmarktag_Key_ptas_ptas_bookmark_ptas_bookmarktagid] ON [dbo].[Ptas_ptas_bookmark_ptas_bookmarktag_Key]
(
	[ptas_ptas_bookmark_ptas_bookmarktagid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_ptas_permit_ptas_mediarepository_Key_ptas_ptas_permit_ptas_mediarepositoryid] ON [dbo].[Ptas_ptas_permit_ptas_mediarepository_Key]
(
	[ptas_ptas_permit_ptas_mediarepositoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_sales_ptas_mediarepository_Key_ptas_ptas_sales_ptas_mediarepositoryid] ON [dbo].[Ptas_sales_ptas_mediarepository_Key]
(
	[ptas_sales_ptas_mediarepositoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_qstr_key_ptas_qstrid] ON [dbo].[Ptas_qstr_Key]
(
	[ptas_qstrid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_responsibility_key_ptas_responsibilityid] ON [dbo].[Ptas_responsibility_Key]
(
	[ptas_responsibilityid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_sales_key_ptas_salesid] ON [dbo].[Ptas_sales_Key]
(
	[ptas_salesid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_sales_parceldetail_parcelsinsale_Key_ptas_sales_parceldetail_parcelsinsaleid] ON [dbo].[Ptas_sales_parceldetail_parcelsinsale_Key]
(
	[ptas_sales_parceldetail_parcelsinsaleid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_sales_ptas_saleswarningcode_key_ptas_sales_ptas_saleswarningcodeid] ON [dbo].[Ptas_sales_ptas_saleswarningcode_Key]
(
	[ptas_sales_ptas_saleswarningcodeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_saleswarningcode_key_ptas_saleswarningcodeid] ON [dbo].[Ptas_saleswarningcode_Key]
(
	[ptas_saleswarningcodeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_sectionusesqft_key_ptas_sectionusesqftid] ON [dbo].[Ptas_sectionusesqft_Key]
(
	[ptas_sectionusesqftid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_sketch_key_ptas_sketchId] ON [dbo].[Ptas_sketch_Key]
(
	[ptas_sketchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_specialtyarea_key_ptas_specialtyareaid] ON [dbo].[Ptas_specialtyarea_Key]
(
	[ptas_specialtyareaid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_specialtyneighborhood_key_ptas_specialtyneighborhoodid] ON [dbo].[Ptas_specialtyneighborhood_Key]
(
	[ptas_specialtyneighborhoodid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_stateorprovince_key_ptas_stateorprovinceid] ON [dbo].[Ptas_stateorprovince_Key]
(
	[ptas_stateorprovinceid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_streetname_key_ptas_streetnameid] ON [dbo].[Ptas_streetname_Key]
(
	[ptas_streetnameid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_streettype_key_ptas_streettypeid] ON [dbo].[Ptas_streettype_Key]
(
	[ptas_streettypeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_subarea_key_ptas_subareaid] ON [dbo].[Ptas_subarea_Key]
(
	[ptas_subareaid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_taxaccount_key_ptas_taxaccountid] ON [dbo].[Ptas_taxaccount_Key]
(
	[ptas_taxaccountid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_Ptas_taxrollhistory_Key_taxRollHistoryGuid ] ON [dbo].[Ptas_taxrollhistory_Key]
(
	[taxRollHistoryGuid ] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_unitbreakdown_key_ptas_unitbreakdownid] ON [dbo].[Ptas_unitbreakdown_Key]
(
	[ptas_unitbreakdownid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_unitbreakdowntype_key_ptas_unitbreakdowntypeid] ON [dbo].[Ptas_unitbreakdowntype_Key]
(
	[ptas_unitbreakdowntypeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_visitedsketch_key_ptas_visitedsketchId] ON [dbo].[Ptas_visitedsketch_Key]
(
	[ptas_visitedsketchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_year_key_ptas_yearid] ON [dbo].[Ptas_year_Key]
(
	[ptas_yearid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_zipcode_key_ptas_zipcodeid] ON [dbo].[Ptas_zipcode_Key]
(
	[ptas_zipcodeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_ptas_zoning_key_ptas_zoningid] ON [dbo].[Ptas_zoning_Key]
(
	[ptas_zoningid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_role_key_roleid] ON [dbo].[Role_Key]
(
	[roleid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_stringmap_key_stringmapid] ON [dbo].[Stringmap_Key]
(
	[stringmapid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_systemuser_key_systemuserid] ON [dbo].[Systemuser_Key]
(
	[systemuserid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_systemuserroles_key_systemuserroleid] ON [dbo].[Systemuserroles_Key]
(
	[systemuserroleid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_team_key_teamid] ON [dbo].[Team_Key]
(
	[teamid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_teammembership_key_teammembershipid] ON [dbo].[Teammembership_Key]
(
	[teammembershipid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_User_filter_key_user_filter_id] ON [dbo].[User_filter_Key]
(
	[user_filter_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_land_Data_area_mb_changesetId_mb] ON [dbo].Ptas_land_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_economicunit_Data_area_mb_changesetId_mb] ON [dbo].Ptas_economicunit_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_housingprogram_Data_area_mb_changesetId_mb] ON [dbo].Ptas_housingprogram_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_apartmentregion_Data_area_mb_changesetId_mb] ON [dbo].Ptas_apartmentregion_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_propertytype_Data_area_mb_changesetId_mb] ON [dbo].Ptas_propertytype_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_responsibility_Data_area_mb_changesetId_mb] ON [dbo].Ptas_responsibility_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_area_Data_area_mb_changesetId_mb] ON [dbo].Ptas_area_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_subarea_Data_area_mb_changesetId_mb] ON [dbo].Ptas_subarea_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_neighborhood_Data_area_mb_changesetId_mb] ON [dbo].Ptas_neighborhood_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_qstr_Data_area_mb_changesetId_mb] ON [dbo].Ptas_qstr_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_zoning_Data_area_mb_changesetId_mb] ON [dbo].Ptas_zoning_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_levycode_Data_area_mb_changesetId_mb] ON [dbo].Ptas_levycode_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_streetname_Data_area_mb_changesetId_mb] ON [dbo].Ptas_streetname_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_streettype_Data_area_mb_changesetId_mb] ON [dbo].Ptas_streettype_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_city_Data_area_mb_changesetId_mb] ON [dbo].Ptas_city_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_stateorprovince_Data_area_mb_changesetId_mb] ON [dbo].Ptas_stateorprovince_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_zipcode_Data_area_mb_changesetId_mb] ON [dbo].Ptas_zipcode_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_country_Data_area_mb_changesetId_mb] ON [dbo].Ptas_country_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_district_Data_area_mb_changesetId_mb] ON [dbo].Ptas_district_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_year_Data_area_mb_changesetId_mb] ON [dbo].Ptas_year_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_parkingdistrict_Data_area_mb_changesetId_mb] ON [dbo].Ptas_parkingdistrict_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_landuse_Data_area_mb_changesetId_mb] ON [dbo].Ptas_landuse_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_unitbreakdowntype_Data_area_mb_changesetId_mb] ON [dbo].Ptas_unitbreakdowntype_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_buildingsectionuse_Data_area_mb_changesetId_mb] ON [dbo].Ptas_buildingsectionuse_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Systemuser_Data_area_mb_changesetId_mb] ON [dbo].Systemuser_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Team_Data_area_mb_changesetId_mb] ON [dbo].Team_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Role_Data_area_mb_changesetId_mb] ON [dbo].Role_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_User_filter_Data_area_mb_changesetId_mb] ON [dbo].User_filter_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_saleswarningcode_Data_area_mb_changesetId_mb] ON [dbo].Ptas_saleswarningcode_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_specialtyarea_Data_area_mb_changesetId_mb] ON [dbo].Ptas_specialtyarea_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_bookmarktag_Data_area_mb_changesetId_mb] ON [dbo].Ptas_bookmarktag_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Stringmap_Data_area_mb_changesetId_mb] ON [dbo].Stringmap_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_jurisdiction_Data_area_mb_changesetId_mb] ON [dbo].Ptas_jurisdiction_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_geoarea_Data_area_mb_changesetId_mb] ON [dbo].Ptas_geoarea_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_ptassetting_Data_area_mb_changesetId_mb] ON [dbo].Ptas_ptassetting_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluecalculation_Data_area_mb_changesetId_mb] ON [dbo].Ptas_landvaluecalculation_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_taxaccount_Data_area_mb_changesetId_mb] ON [dbo].Ptas_taxaccount_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_apartmentsupergroup_Data_area_mb_changesetId_mb] ON [dbo].Ptas_apartmentsupergroup_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_sketch_Data_area_mb_changesetId_mb] ON [dbo].Ptas_sketch_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Systemuserroles_Data_area_mb_changesetId_mb] ON [dbo].Systemuserroles_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Teammembership_Data_area_mb_changesetId_mb] ON [dbo].Teammembership_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data_area_mb_changesetId_mb] ON [dbo].Ptas_sales_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_specialtyneighborhood_Data_area_mb_changesetId_mb] ON [dbo].Ptas_specialtyneighborhood_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_geoneighborhood_Data_area_mb_changesetId_mb] ON [dbo].Ptas_geoneighborhood_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_parceleconomicunit_Data_area_mb_changesetId_mb] ON [dbo].Ptas_parceleconomicunit_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data_area_mb_changesetId_mb] ON [dbo].Ptas_buildingdetail_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_accessorydetail_Data_area_mb_changesetId_mb] ON [dbo].Ptas_accessorydetail_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data_area_mb_changesetId_mb] ON [dbo].Ptas_condocomplex_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_visitedsketch_Data_area_mb_changesetId_mb] ON [dbo].Ptas_visitedsketch_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_inspectionhistory_Data_area_mb_changesetId_mb] ON [dbo].Ptas_inspectionhistory_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_camanotes_Data_area_mb_changesetId_mb] ON [dbo].Ptas_camanotes_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_sales_ptas_saleswarningcode_Data_area_mb_changesetId_mb] ON [dbo].Ptas_sales_ptas_saleswarningcode_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_bookmark_Data_area_mb_changesetId_mb] ON [dbo].Ptas_bookmark_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_mediarepository_Data_area_mb_changesetId_mb] ON [dbo].Ptas_mediarepository_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_land_ptas_mediarepository_Data_area_mb_changesetId_mb] ON [dbo].Ptas_land_ptas_mediarepository_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_ptas_mediarepository_Data_area_mb_changesetId_mb] ON [dbo].Ptas_parceldetail_ptas_mediarepository_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_ptas_bookmark_ptas_bookmarktag_Data_area_mb_changesetId_mb] ON [dbo].Ptas_ptas_bookmark_ptas_bookmarktag_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_sales_parceldetail_parcelsinsale_Data_area_mb_changesetId_mb] ON [dbo].Ptas_sales_parceldetail_parcelsinsale_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_taxrollhistory_Data_area_mb_changesetId_mb] ON [dbo].Ptas_taxrollhistory_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_appraisalhistory_Data_area_mb_changesetId_mb] ON [dbo].Ptas_appraisalhistory_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_estimatehistory_Data_area_mb_changesetId_mb] ON [dbo].Ptas_estimatehistory_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_ptas_permit_ptas_mediarepository_Data_area_mb_changesetId_mb] ON [dbo].Ptas_ptas_permit_ptas_mediarepository_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_sales_ptas_mediarepository_Data_area_mb_changesetId_mb] ON [dbo].Ptas_sales_ptas_mediarepository_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_changehistory_Data_area_mb_changesetId_mb] ON [dbo].Ptas_changehistory_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluebreakdown_Data_area_mb_changesetId_mb] ON [dbo].Ptas_landvaluebreakdown_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_unitbreakdown_Data_area_mb_changesetId_mb] ON [dbo].Ptas_unitbreakdown_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_lowincomehousingprogram_Data_area_mb_changesetId_mb] ON [dbo].Ptas_lowincomehousingprogram_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_projectdock_Data_area_mb_changesetId_mb] ON [dbo].Ptas_projectdock_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_sectionusesqft_Data_area_mb_changesetId_mb] ON [dbo].Ptas_sectionusesqft_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_buildingsectionfeature_Data_area_mb_changesetId_mb] ON [dbo].Ptas_buildingsectionfeature_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_accessorydetail_ptas_mediarepository_Data_area_mb_changesetId_mb] ON [dbo].Ptas_accessorydetail_ptas_mediarepository_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_ptas_mediarepository_Data_area_mb_changesetId_mb] ON [dbo].Ptas_buildingdetail_ptas_mediarepository_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_ptas_mediarepository_Data_area_mb_changesetId_mb] ON [dbo].Ptas_condocomplex_ptas_mediarepository_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_camanotes_ptas_mediarepository_Data_area_mb_changesetId_mb] ON [dbo].Ptas_camanotes_ptas_mediarepository_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data_area_mb_changesetId_mb] ON [dbo].Ptas_condounit_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_commercialuse_Data_area_mb_changesetId_mb] ON [dbo].Ptas_buildingdetail_commercialuse_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_permit_Data_area_mb_changesetId_mb] ON [dbo].Ptas_permit_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_ptas_mediarepository_Data_area_mb_changesetId_mb] ON [dbo].Ptas_condounit_ptas_mediarepository_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO
CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data_area_mb_changesetId_mb] ON [dbo].Ptas_parceldetail_Data ([area_mb], [changesetId_mb] ASC)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY] 
 GO

 -------------------------------- PARCEL --------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_areaid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_areaid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_addr1_cityid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_addr1_cityid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_addr1_countryid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_addr1_countryid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_geoareaid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_geoareaid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_geonbhdid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_geonbhdid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_landid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_landid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_levycodeid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_levycodeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_neighborhoodid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_neighborhoodid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_propertytypeid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_propertytypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_qstrid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_qstrid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_responsibilityid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_responsibilityid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_saleid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_saleid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_specialtyareaid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_specialtyareaid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_specialtynbhdid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_specialtynbhdid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_addr1_stateid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_addr1_stateid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_addr1_streetnameid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_addr1_streetnameid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_addr1_streettypeid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_addr1_streettypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_subareaid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_subareaid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_splitaccount2id_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_splitaccount2id_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_splitaccount1id_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_splitaccount1id_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data__ptas_addr1_zipcodeid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_addr1_zipcodeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data___ptas_jurisdiction_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_jurisdiction_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceldetail_Data___ptas_districtid_value] ON [dbo].[Ptas_parceldetail_Data]
(
	[_ptas_districtid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


------------------------- Ptas_Land ----------------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_land_Data___ptas_masterlandid_value] ON [dbo].[Ptas_land_Data]
(
	[_ptas_masterlandid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_land_Data___transactioncurrencyid_value] ON [dbo].[Ptas_land_Data]
(
	[_transactioncurrencyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

------------------------- Ptas_landvaluecalculation ----------------------------------------


CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluecalculation_Data___ptas_landid_value] ON [dbo].[Ptas_landvaluecalculation_Data]
(
	[_ptas_landid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluecalculation_Data____ptas_masterlandcharacteristicid_value] ON [dbo].[Ptas_landvaluecalculation_Data]
(
	_ptas_masterlandcharacteristicid_value ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluecalculation_Data____ptas_zoningtypeid_value] ON [dbo].[Ptas_landvaluecalculation_Data]
(
	_ptas_zoningtypeid_value ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluecalculation_Data____ptas_designationtypeid_value] ON [dbo].[Ptas_landvaluecalculation_Data]
(
	[_ptas_designationtypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluecalculation_Data____ptas_envrestypeid_value] ON [dbo].[Ptas_landvaluecalculation_Data]
(
	[_ptas_envrestypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluecalculation_Data____ptas_nuisancetypeid_value] ON [dbo].[Ptas_landvaluecalculation_Data]
(
	[_ptas_nuisancetypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluecalculation_Data____ptas_viewtypeid_value] ON [dbo].[Ptas_landvaluecalculation_Data]
(
	[_ptas_viewtypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluecalculation_Data____transactioncurrencyid_value] ON [dbo].[Ptas_landvaluecalculation_Data]
(
	[_transactioncurrencyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

------------------------ Ptas_taxaccount ----------------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_taxaccount_Data____ptas_parcelid_value] ON [dbo].[Ptas_taxaccount_Data]
(
	[_ptas_parcelid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_taxaccount_Data____ptas_condounitid_value] ON [dbo].[Ptas_taxaccount_Data]
(
	[_ptas_condounitid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_taxaccount_Data____ptas_personalpropertyid_value] ON [dbo].[Ptas_taxaccount_Data]
(
	[_ptas_personalpropertyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_taxaccount_Data____ptas_addr1_cityid_value] ON [dbo].[Ptas_taxaccount_Data]
(
	[_ptas_addr1_cityid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_taxaccount_Data____ptas_addr1_stateid_value] ON [dbo].[Ptas_taxaccount_Data]
(
	[_ptas_addr1_stateid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_taxaccount_Data____ptas_addr1_zipcodeid_value] ON [dbo].[Ptas_taxaccount_Data]
(
	[_ptas_addr1_zipcodeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_taxaccount_Data____ptas_addr1_countryid_value] ON [dbo].[Ptas_taxaccount_Data]
(
	[_ptas_addr1_countryid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_taxaccount_Data____ptas_mastertaxaccountid_value] ON [dbo].[Ptas_taxaccount_Data]
(
	[_ptas_mastertaxaccountid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_taxaccount_Data____ptas_levycodeid_value] ON [dbo].[Ptas_taxaccount_Data]
(
	[_ptas_levycodeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_taxaccount_Data____ptas_masspayerid_value] ON [dbo].[Ptas_taxaccount_Data]
(
	[_ptas_masspayerid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

---------------------------- Ptas_parceleconomicunit -----------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_parceleconomicunit_Data____ptas_parcelid_value] ON [dbo].[Ptas_parceleconomicunit_Data]
(
	[_ptas_parcelid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_parceleconomicunit_Data____ptas_economicunitid_value] ON [dbo].[Ptas_parceleconomicunit_Data]
(
	[_ptas_economicunitid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


---------------------------- Ptas_buildingdetail -----------------------


CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_parceldetailid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_parceldetailid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_propertytypeid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_propertytypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_yearbuiltid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_yearbuiltid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_addr1_streetnameid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_addr1_streetnameid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_addr1_streettypeid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_addr1_streettypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_addr1_cityid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_addr1_cityid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_addr1_stateid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_addr1_stateid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_addr1_zipcodeid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_addr1_zipcodeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_addr1_countryid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_addr1_countryid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_buildingsectionuseid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_buildingsectionuseid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_effectiveyearid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_effectiveyearid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_sketchid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_sketchid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_masterbuildingid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_masterbuildingid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____transactioncurrencyid_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_transactioncurrencyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_Data____ptas_taxaccount_value] ON [dbo].[Ptas_buildingdetail_Data]
(
	[_ptas_taxaccount_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


------------------------------------- Ptas_buildingdetail_commercialuse --------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_commercialuse_Data____ptas_buildingdetailid_value] ON [dbo].[Ptas_buildingdetail_commercialuse_Data]
(
	[_ptas_buildingdetailid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_commercialuse_Data____ptas_buildingsectionuseid_value] ON [dbo].[Ptas_buildingdetail_commercialuse_Data]
(
	[_ptas_buildingsectionuseid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_commercialuse_Data____ptas_unitid_value] ON [dbo].[Ptas_buildingdetail_commercialuse_Data]
(
	[_ptas_unitid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_commercialuse_Data____ptas_mastersectionuseid_value] ON [dbo].[Ptas_buildingdetail_commercialuse_Data]
(
	[_ptas_mastersectionuseid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_commercialuse_Data____ptas_projectid_value] ON [dbo].[Ptas_buildingdetail_commercialuse_Data]
(
	[_ptas_projectid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_commercialuse_Data____ptas_specialtyareaid_value] ON [dbo].[Ptas_buildingdetail_commercialuse_Data]
(
	[_ptas_specialtyareaid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingdetail_commercialuse_Data____ptas_specialtynbhdid_value] ON [dbo].[Ptas_buildingdetail_commercialuse_Data]
(
	[_ptas_specialtynbhdid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


----------------------------------- Ptas_accessorydetail ---------------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_accessorydetail_Data___ptas_parceldetailid_value] ON [dbo].[Ptas_accessorydetail_Data]
(
	[_ptas_parceldetailid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_accessorydetail_Data____ptas_propertytypeid_value] ON [dbo].[Ptas_accessorydetail_Data]
(
	[_ptas_propertytypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_accessorydetail_Data____ptas_buildingdetailid_value] ON [dbo].[Ptas_accessorydetail_Data]
(
	[_ptas_buildingdetailid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_accessorydetail_Data____ptas_res_accessorytypeid_value] ON [dbo].[Ptas_accessorydetail_Data]
(
	[_ptas_res_accessorytypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_accessorydetail_Data____ptas_com_accessorytypeid_value] ON [dbo].[Ptas_accessorydetail_Data]
(
	[_ptas_com_accessorytypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_accessorydetail_Data____ptas_sketchid_value] ON [dbo].[Ptas_accessorydetail_Data]
(
	[_ptas_sketchid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Ptas_accessorydetail_Data____ptas_masteraccessoryid_value] ON [dbo].[Ptas_accessorydetail_Data]
(
	[_ptas_masteraccessoryid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_accessorydetail_Data___transactioncurrencyid_value] ON [dbo].[Ptas_accessorydetail_Data]
(
	[_transactioncurrencyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

----------------------------------- Ptas_unitbreakdown -----------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_unitbreakdown_Data____ptas_condocomplexid_value] ON [dbo].[Ptas_unitbreakdown_Data]
(
	[_ptas_condocomplexid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_unitbreakdown_Data____ptas_unitbreakdowntypeid_value] ON [dbo].[Ptas_unitbreakdown_Data]
(
	[_ptas_unitbreakdowntypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_unitbreakdown_Data____ptas_buildingid_value] ON [dbo].[Ptas_unitbreakdown_Data]
(
	[_ptas_buildingid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_unitbreakdown_Data____ptas_masterunitbreakdownid_value] ON [dbo].[Ptas_unitbreakdown_Data]
(
	[_ptas_masterunitbreakdownid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


----------------------------- Ptas_lowincomehousingprogram ------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_lowincomehousingprogram_Data___ptas_condocomplexid_value] ON [dbo].[Ptas_lowincomehousingprogram_Data]
(
	[_ptas_condocomplexid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_lowincomehousingprogram_Data____ptas_housingprogramid_value] ON [dbo].[Ptas_lowincomehousingprogram_Data]
(
	[_ptas_housingprogramid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_lowincomehousingprogram_Data____ptas_masterlowincomehousingid_value] ON [dbo].[Ptas_lowincomehousingprogram_Data]
(
	[_ptas_masterlowincomehousingid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


------------------------------- Ptas_apartmentsupergroup -----------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_apartmentsupergroup_Data____ptas_apartmentregionid_value] ON [dbo].[Ptas_apartmentsupergroup_Data]
(
	[_ptas_apartmentregionid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


----------------------------- Ptas_projectdock ---------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_projectdock_Data___ptas_condocomplexid_value] ON [dbo].[Ptas_projectdock_Data]
(
	[_ptas_condocomplexid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_projectdock_Data___ptas_masterdockid_value] ON [dbo].[Ptas_projectdock_Data]
(
	[_ptas_masterdockid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_projectdock_Data___organizationid_value] ON [dbo].[Ptas_projectdock_Data]
(
	[_organizationid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

--------------------------- Ptas_propertytype ---------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_propertytype_Data___organizationid_value] ON [dbo].[Ptas_propertytype_Data]
(
	[_organizationid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

------------------------- Ptas_neighborhood ----------------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_neighborhood_Data___ptas_areaid_value] ON [dbo].[Ptas_neighborhood_Data]
(
	[_ptas_areaid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


-------------------------- Ptas_zoning ---------------------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_zoning_Data___ptas_taxdistrictid_value] ON [dbo].[Ptas_zoning_Data]
(
	[_ptas_taxdistrictid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


-------------------------- Ptas_condocomplex ---------------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_parcelid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_parcelid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ownerid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ownerid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_addr1_cityid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_addr1_cityid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_addr1_countryid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_addr1_countryid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_addr1_stateid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_addr1_stateid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_addr1_zipcode_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_addr1_zipcode_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_associatedparcel2id_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_associatedparcel2id_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_associatedparcel3id_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_associatedparcel3id_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_associatedparcelid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_associatedparcelid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_contaminationproject_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_contaminationproject_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_majorcondocomplexid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_majorcondocomplexid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_masterprojectid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_masterprojectid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_parkingdistrictid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_parkingdistrictid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___transactioncurrencyid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_transactioncurrencyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_accessoryid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_accessoryid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condocomplex_Data___ptas_economicunitid_value] ON [dbo].[Ptas_condocomplex_Data]
(
	[_ptas_economicunitid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

------------------------------ Ptas_sectionusesqft -------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_sectionusesqft_Data___ptas_projectid_value] ON [dbo].[Ptas_sectionusesqft_Data]
(
	[_ptas_projectid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sectionusesqft_Data___ptas_sectionuse_value] ON [dbo].[Ptas_sectionusesqft_Data]
(
	[_ptas_sectionuse_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sectionusesqft_Data___ptas_mastersectionusesqftid_value] ON [dbo].[Ptas_sectionusesqft_Data]
(
	[_ptas_mastersectionusesqftid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

---------------------------- Ptas_condounit ------------------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_sketchid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_sketchid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_parcelid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_parcelid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_addr1_cityid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_addr1_cityid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_addr1_countryid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_addr1_countryid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_addr1_stateid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_addr1_stateid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_addr1_streetnameid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_addr1_streetnameid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_addr1_streettypeid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_addr1_streettypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_addr1_zipcodeid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_addr1_zipcodeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_buildingid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_buildingid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_complexid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_complexid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_masterunitid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_masterunitid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_propertytypeid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_propertytypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_responsibilityid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_responsibilityid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_selectbyid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_selectbyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_specialtyareaid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_specialtyareaid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_specialtynbhdid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_specialtynbhdid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_taxaccountid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_taxaccountid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_unitinspectedbyid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_unitinspectedbyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_yearbuildid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_yearbuildid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___transactioncurrencyid_value] ON [dbo].[Ptas_condounit_Data]
(
	[_transactioncurrencyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_condounit_Data___ptas_dock_value] ON [dbo].[Ptas_condounit_Data]
(
	[_ptas_dock_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


-------------------------------- Ptas_buildingsectionfeature --------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingsectionfeature_Data___ptas_buildingsectionuseid_value] ON [dbo].[Ptas_buildingsectionfeature_Data]
(
	[_ptas_buildingsectionuseid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_buildingsectionfeature_Data___ptas_masterfeatureid_value] ON [dbo].[Ptas_buildingsectionfeature_Data]
(
	[_ptas_masterfeatureid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

--------------------------------- Ptas_sketch ------------------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_sketch_Data___ptas_drawauthorid_value] ON [dbo].[Ptas_sketch_Data]
(
	[_ptas_drawauthorid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sketch_Data___ptas_lockedbyid_value] ON [dbo].[Ptas_sketch_Data]
(
	[_ptas_lockedbyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sketch_Data___ptas_templateid_value] ON [dbo].[Ptas_sketch_Data]
(
	[_ptas_templateid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sketch_Data___ptas_parcelid_value] ON [dbo].[Ptas_sketch_Data]
(
	[_ptas_parcelid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sketch_Data___ptas_buildingid_value] ON [dbo].[Ptas_sketch_Data]
(
	[_ptas_buildingid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sketch_Data___ptas_unitid_value] ON [dbo].[Ptas_sketch_Data]
(
	[_ptas_unitid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sketch_Data___ptas_accessoryid_value] ON [dbo].[Ptas_sketch_Data]
(
	[_ptas_accessoryid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


------------------------------ Ptas_visitedsketch -------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_visitedsketch_Data___ptas_sketchid_value] ON [dbo].[Ptas_visitedsketch_Data]
(
	[_ptas_sketchid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_visitedsketch_Data___ptas_visitedbyid_value] ON [dbo].[Ptas_visitedsketch_Data]
(
	[_ptas_visitedbyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

--------------------------- Ptas_permit -------------------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_permit_Data___ptas_parcelid_value] ON [dbo].[Ptas_permit_Data]
(
	[_ptas_parcelid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX__Data___ptas_currentjurisdiction_value] ON [dbo].[Ptas_permit_Data]
(
	[_ptas_currentjurisdiction_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_permit_Data___ptas_reviewedbyid_value] ON [dbo].[Ptas_permit_Data]
(
	[_ptas_reviewedbyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_permit_Data___ptas_condounitid_value] ON [dbo].[Ptas_permit_Data]
(
	[_ptas_condounitid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

------------------------------- Ptas_sales --------------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_primaryparcelid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_primaryparcelid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_nonrepresentativesale1id_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_nonrepresentativesale1id_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_nonrepresentativesale2id_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_nonrepresentativesale2id_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_primarybuildingid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_primarybuildingid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_addr1_cityid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_addr1_cityid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_addr1_countryid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_addr1_countryid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_addr1_stateid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_addr1_stateid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_addr1_streetnameid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_addr1_streetnameid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_addr1_streettypeid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_addr1_streettypeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_addr1_zipcodeid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_addr1_zipcodeid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_taxaccountid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_taxaccountid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_unitid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_unitid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___transactioncurrencyid_value] ON [dbo].[Ptas_sales_Data]
(
	[_transactioncurrencyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_verifiedbyid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_verifiedbyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_sales_Data___ptas_identifiedbyid_value] ON [dbo].[Ptas_sales_Data]
(
	[_ptas_identifiedbyid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

------------------------ Ptas_camanotes --------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_camanotes_Data___ptas_parcelid_value] ON [dbo].[Ptas_camanotes_Data]
(
	[_ptas_parcelid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_camanotes_Data___ptas_propertyreviewid_value] ON [dbo].[Ptas_camanotes_Data]
(
	[_ptas_propertyreviewid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

------------------------ Ptas_specialtyneighborhood -----------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_specialtyneighborhood_Data___ptas_specialtyareaid_value] ON [dbo].[Ptas_specialtyneighborhood_Data]
(
	[_ptas_specialtyareaid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

----------------------- Ptas_bookmark -----------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_bookmark_Data___ptas_parceldetailid_value] ON [dbo].[Ptas_bookmark_Data]
(
	[_ptas_parceldetailid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_bookmark_Data___ownerid_value] ON [dbo].[Ptas_bookmark_Data]
(
	[_ownerid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

----------------------- Ptas_mediarepository ---------------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_mediarepository_Data___ptas_saleid_value] ON [dbo].[Ptas_mediarepository_Data]
(
	[_ptas_saleid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_mediarepository_Data___ptas_yearid_value] ON [dbo].[Ptas_mediarepository_Data]
(
	[_ptas_yearid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

---------------------- Ptas_geoneighborhood -----------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_geoneighborhood_Data___ptas_geoareaid_value] ON [dbo].[Ptas_geoneighborhood_Data]
(
	[_ptas_geoareaid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

----------------------- Ptas_landvaluebreakdown --------------------------

CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluebreakdown_Data___ptas_landid_value] ON [dbo].[Ptas_landvaluebreakdown_Data]
(
	[_ptas_landid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluebreakdown_Data___ptas_masterlandvaluebreakdownid_value] ON [dbo].[Ptas_landvaluebreakdown_Data]
(
	[_ptas_masterlandvaluebreakdownid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Ptas_landvaluebreakdown_Data___ptas_parceldetailid_value] ON [dbo].[Ptas_landvaluebreakdown_Data]
(
	[_ptas_parceldetailid_value] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO
