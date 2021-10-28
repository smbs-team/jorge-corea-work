ALTER TABLE [dynamics].[ptas_accessorydetail_ptas_mediarepository] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_accessorydetail_ptas_mediarepository
   ON  [dynamics].[ptas_accessorydetail_ptas_mediarepository]
   AFTER INSERT, UPDATE
AS
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_accessorydetail_ptas_mediarepository] modfied
	JOIN INSERTED i on modfied.ptas_accessorydetail_ptas_mediarepositoryid = i.ptas_accessorydetail_ptas_mediarepositoryid
END
GO

ALTER TABLE [dynamics].[ptas_buildingdetail_ptas_mediarepository] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_buildingdetail_ptas_mediarepository
   ON  [dynamics].[ptas_buildingdetail_ptas_mediarepository]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_buildingdetail_ptas_mediarepository] modfied
	JOIN INSERTED i on modfied.ptas_buildingdetail_ptas_mediarepositoryid = i.ptas_buildingdetail_ptas_mediarepositoryid
END
GO

ALTER TABLE [dynamics].[ptas_city_county] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_city_county
   ON  [dynamics].[ptas_city_county]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_city_county] modfied
	JOIN INSERTED i on modfied.ptas_city_countyid = i.ptas_city_countyid
END
GO

ALTER TABLE [dynamics].[ptas_city_stateorprovince] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_city_stateorprovince
   ON  [dynamics].[ptas_city_stateorprovince]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_city_stateorprovince] modfied
	JOIN INSERTED i on modfied.ptas_city_stateorprovinceid = i.ptas_city_stateorprovinceid
END
GO

ALTER TABLE [dynamics].[ptas_city_zipcode] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_city_zipcode
   ON [dynamics].[ptas_city_zipcode]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_city_zipcode] modfied
	JOIN INSERTED i on modfied.ptas_city_zipcodeid = i.ptas_city_zipcodeid
END
GO

ALTER TABLE [dynamics].[ptas_condocomplex_ptas_mediarepository] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_condocomplex_ptas_mediarepository
   ON [dynamics].[ptas_condocomplex_ptas_mediarepository]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_condocomplex_ptas_mediarepository] modfied
	JOIN INSERTED i on modfied.ptas_condocomplex_ptas_mediarepositoryid = i.ptas_condocomplex_ptas_mediarepositoryid
END
GO

ALTER TABLE [dynamics].[ptas_condounit_ptas_mediarepository] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_condounit_ptas_mediarepository
   ON [dynamics].[ptas_condounit_ptas_mediarepository]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_condounit_ptas_mediarepository] modfied
	JOIN INSERTED i on modfied.ptas_condounit_ptas_mediarepositoryid = i.ptas_condounit_ptas_mediarepositoryid
END
GO

ALTER TABLE [dynamics].[ptas_county_stateorprovince] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_county_stateorprovince
   ON [dynamics].[ptas_county_stateorprovince]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_county_stateorprovince] modfied
	JOIN INSERTED i on modfied.ptas_county_stateorprovinceid = i.ptas_county_stateorprovinceid
END
GO

ALTER TABLE [dynamics].[ptas_county_zipcode] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_county_zipcode
   ON [dynamics].[ptas_county_zipcode]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_county_zipcode] modfied
	JOIN INSERTED i on modfied.ptas_county_zipcodeid = i.ptas_county_zipcodeid
END
GO

ALTER TABLE [dynamics].[ptas_economicunit_accessorydetail] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_economicunit_accessorydetail
   ON [dynamics].[ptas_economicunit_accessorydetail]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_economicunit_accessorydetail] modfied
	JOIN INSERTED i on modfied.ptas_economicunit_accessorydetailid = i.ptas_economicunit_accessorydetailid
END
GO

ALTER TABLE [dynamics].[ptas_land_ptas_mediarepository] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_land_ptas_mediarepository
   ON [dynamics].[ptas_land_ptas_mediarepository]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_land_ptas_mediarepository] modfied
	JOIN INSERTED i on modfied.ptas_land_ptas_mediarepositoryid = i.ptas_land_ptas_mediarepositoryid
END
GO

ALTER TABLE [dynamics].[ptas_parceldetail_ptas_mediarepository] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_parceldetail_ptas_mediarepository
   ON [dynamics].[ptas_parceldetail_ptas_mediarepository]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_parceldetail_ptas_mediarepository] modfied
	JOIN INSERTED i on modfied.ptas_parceldetail_ptas_mediarepositoryid = i.ptas_parceldetail_ptas_mediarepositoryid
END
GO

ALTER TABLE [dynamics].[ptas_ptas_bookmark_ptas_bookmarktag] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_ptas_bookmark_ptas_bookmarktag
   ON [dynamics].[ptas_ptas_bookmark_ptas_bookmarktag]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_ptas_bookmark_ptas_bookmarktag] modfied
	JOIN INSERTED i on modfied.ptas_ptas_bookmark_ptas_bookmarktagid = i.ptas_ptas_bookmark_ptas_bookmarktagid
END
GO

ALTER TABLE [dynamics].[ptas_ptas_camanotes_ptas_fileattachmentmetad] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_ptas_camanotes_ptas_fileattachmentmetad
   ON [dynamics].[ptas_ptas_camanotes_ptas_fileattachmentmetad]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_ptas_camanotes_ptas_fileattachmentmetad] modfied
	JOIN INSERTED i on modfied.ptas_ptas_camanotes_ptas_fileattachmentmetadid = i.ptas_ptas_camanotes_ptas_fileattachmentmetadid
END
GO

ALTER TABLE [dynamics].[ptas_camanotes_ptas_mediarepository] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_camanotes_ptas_mediarepository
   ON [dynamics].[ptas_camanotes_ptas_mediarepository]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_camanotes_ptas_mediarepository] modfied
	JOIN INSERTED i on modfied.ptas_camanotes_ptas_mediarepositoryid = i.ptas_camanotes_ptas_mediarepositoryid
END
GO

ALTER TABLE [dynamics].[ptas_ptas_sales_ptas_fileattachmentmetadata] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_ptas_sales_ptas_fileattachmentmetadata
   ON [dynamics].[ptas_ptas_sales_ptas_fileattachmentmetadata]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_ptas_sales_ptas_fileattachmentmetadata]  modfied
	JOIN INSERTED i on modfied.ptas_ptas_sales_ptas_fileattachmentmetadataid = i.ptas_ptas_sales_ptas_fileattachmentmetadataid
END
GO

ALTER TABLE [dynamics].[ptas_sales_parceldetail_parcelsinsale] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_sales_parceldetail_parcelsinsale
   ON [dynamics].[ptas_sales_parceldetail_parcelsinsale]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_sales_parceldetail_parcelsinsale] modfied
	JOIN INSERTED i on modfied.ptas_sales_parceldetail_parcelsinsaleid = i.ptas_sales_parceldetail_parcelsinsaleid
END
GO

ALTER TABLE [dynamics].[ptas_sales_ptas_saleswarningcode] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_sales_ptas_saleswarningcode
   ON [dynamics].[ptas_sales_ptas_saleswarningcode]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_sales_ptas_saleswarningcode] modfied
	JOIN INSERTED i on modfied.ptas_sales_ptas_saleswarningcodeid = i.ptas_sales_ptas_saleswarningcodeid
END
GO

ALTER TABLE [dynamics].[ptas_zipcode_stateorprovince] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_zipcode_stateorprovince
   ON [dynamics].[ptas_zipcode_stateorprovince]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_zipcode_stateorprovince] modfied
	JOIN INSERTED i on modfied.ptas_zipcode_stateorprovinceid = i.ptas_zipcode_stateorprovinceid
END
GO

ALTER TABLE [dynamics].[stringmap] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_stringmap
   ON [dynamics].[stringmap]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[stringmap] modfied
	JOIN INSERTED i on modfied.stringmapid = i.stringmapid
END
GO

ALTER TABLE [dynamics].[systemuserroles] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_systemuserroles
   ON [dynamics].[systemuserroles]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[systemuserroles] modfied
	JOIN INSERTED i on modfied.systemuserroleid = i.systemuserroleid
END
GO

ALTER TABLE [dynamics].[teammembership] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_teammembership
   ON [dynamics].[teammembership]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[teammembership] modfied
	JOIN INSERTED i on modfied.teammembershipid = i.teammembershipid
END
GO

ALTER TABLE [dynamics].[teamprofiles] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_teamprofiles
   ON [dynamics].[teamprofiles]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[teamprofiles] modfied
	JOIN INSERTED i on modfied.teamprofileid = i.teamprofileid
END
GO

ALTER TABLE [dynamics].[teamroles] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_teamroles
   ON [dynamics].[teamroles] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[teamroles]  modfied
	JOIN INSERTED i on modfied.teamroleid = i.teamroleid
END
GO

ALTER TABLE [dynamics].[ptas_ptas_permit_ptas_mediarepository] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_ptas_permit_ptas_mediarepository
   ON [dynamics].[ptas_ptas_permit_ptas_mediarepository] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_ptas_permit_ptas_mediarepository]  modfied
	JOIN INSERTED i on modfied.ptas_ptas_permit_ptas_mediarepositoryid = i.ptas_ptas_permit_ptas_mediarepositoryid
END
GO

ALTER TABLE [dynamics].[ptas_sales_ptas_mediarepository] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_sales_ptas_mediarepository
   ON [dynamics].[ptas_sales_ptas_mediarepository] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_sales_ptas_mediarepository]  modfied
	JOIN INSERTED i on modfied.ptas_sales_ptas_mediarepositoryid = i.ptas_sales_ptas_mediarepositoryid
END
GO

ALTER TABLE [dynamics].[ptas_ptas_task_ptas_fileattachmentmetadata] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_ptas_task_ptas_fileattachmentmetadata
   ON [dynamics].[ptas_ptas_task_ptas_fileattachmentmetadata] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_ptas_task_ptas_fileattachmentmetadata]  modfied
	JOIN INSERTED i on modfied.ptas_ptas_task_ptas_fileattachmentmetadataid = i.ptas_ptas_task_ptas_fileattachmentmetadataid
END
GO

ALTER TABLE [dynamics].[ptas_ptas_task_ptas_parceldetail] ADD modifiedon datetime NOT NULL DEFAULT GETUTCDATE()
GO

CREATE TRIGGER tr_ptas_ptas_task_ptas_parceldetail
   ON [dynamics].[ptas_ptas_task_ptas_parceldetail] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	UPDATE modfied
	SET modifiedon = GETUTCDATE()
	FROM [dynamics].[ptas_ptas_task_ptas_parceldetail]  modfied
	JOIN INSERTED i on modfied.ptas_ptas_task_ptas_parceldetailid = i.ptas_ptas_task_ptas_parceldetailid
END
GO