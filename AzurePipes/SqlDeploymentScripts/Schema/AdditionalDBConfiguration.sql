/****** Object:  Table [dbo].[TableTransfer]    Script Date: 6/9/2020 1:05:53 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TableTransfer]') AND type in (N'U'))
DROP TABLE [dbo].[TableTransfer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TableTransfer](
	[tableId] [nvarchar](200) NOT NULL,
	[LastDateProcessed] [datetime] NOT NULL,
	[LastGuidProcessed] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_tableTransfer] PRIMARY KEY CLUSTERED 
(
	[tableId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[TransferErrors]    Script Date: 6/9/2020 1:07:22 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransferErrors]') AND type in (N'U'))
DROP TABLE [dbo].[TransferErrors]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransferErrors](
	[Id] [uniqueidentifier] NOT NULL,
	[EntityName] [nvarchar](1000) NOT NULL,
	[ExecutedQuery] [ntext] NULL,
	[ErrorMessage] [ntext] NULL,
	[ErrorType] [nvarchar](1000) NULL,
	[ErrorDate] [datetime] NULL,
	[Processed] [bit] NULL,
 CONSTRAINT [PK_TransferErrors] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[TransferErrors] ADD  CONSTRAINT [DF_TransferErrors_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[TransferErrors] ADD  CONSTRAINT [DF_TransferErrors_ErrorDate]  DEFAULT (getdate()) FOR [ErrorDate]
GO
ALTER TABLE [dbo].[TransferErrors] ADD  CONSTRAINT [DF_TransferErrors_Processed]  DEFAULT ((0)) FOR [Processed]
GO

/****** Object:  StoredProcedure [dbo].[UpdateLastProcessedInfo]    Script Date: 6/9/2020 1:08:22 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateLastProcessedInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateLastProcessedInfo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateLastProcessedInfo]
	@EntityName varchar(1000), @LastProcessedDate DateTime, @LastProcessedGuid uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

UPDATE       tableTransfer
SET                LastDateProcessed = @LastProcessedDate,
                   LastGuidProcessed = @LastProcessedGuid
WHERE        (tableId = @EntityName)
	if @@ROWCOUNT=0
		INSERT INTO tableTransfer
								 (LastGuidProcessed, LastDateProcessed, tableId)
		VALUES        (@LastProcessedGuid,@LastProcessedDate,@EntityName)
END
GO

/****** Object:  Index [Idx_displayorder]    Script Date: 9/1/2020 9:07:56 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dynamics].[stringmap]') AND name = N'Idx_displayorder')
DROP INDEX [Idx_displayorder] ON [dynamics].[stringmap]
GO
/****** Object:  Index [Idx_attributename_objecttypecode]    Script Date: 9/1/2020 9:07:56 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dynamics].[stringmap]') AND name = N'Idx_attributename_objecttypecode')
DROP INDEX [Idx_attributename_objecttypecode] ON [dynamics].[stringmap]
GO
/****** Object:  Table [dynamics].[stringmap]    Script Date: 9/1/2020 9:07:56 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dynamics].[stringmap]') AND type in (N'U'))
DROP TABLE [dynamics].[stringmap] 
GO
/****** Object:  Table [dynamics].[stringmap]    Script Date: 9/1/2020 9:07:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dynamics].[stringmap]') AND type in (N'U'))
BEGIN
CREATE TABLE [dynamics].[stringmap](
	[stringmapid] [uniqueidentifier] NOT NULL,
	[attributename] [nvarchar](1000) NULL,
	[attributevalue] [bigint] NULL,
	[displayorder] [int] NULL,
	[langid] [int] NULL,
	[objecttypecode] [nvarchar](1000) NULL,
	[value] [nvarchar](4000) NULL,
	[versionnumber] [bigint] NULL,
	[organizationid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_stringmap] PRIMARY KEY CLUSTERED 
(
	[stringmapid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Index [Idx_attributename_objecttypecode]    Script Date: 9/1/2020 9:07:57 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dynamics].[stringmap]') AND name = N'Idx_attributename_objecttypecode')
CREATE NONCLUSTERED INDEX [Idx_attributename_objecttypecode] ON [dynamics].[stringmap]
(
	[attributename] ASC,
	[objecttypecode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Idx_displayorder]    Script Date: 9/1/2020 9:07:57 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dynamics].[stringmap]') AND name = N'Idx_displayorder')
CREATE NONCLUSTERED INDEX [Idx_displayorder] ON [dynamics].[stringmap]
(
	[displayorder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******ADDITIONAL TABLES******/
/****** Object:  Table [per].[User_Filters]    Script Date: 8/25/2020 10:47:14 AM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[per].[User_Filters]') AND type in (N'U'))
DROP TABLE [per].[User_Filters]
GO
/****** Object:  Schema [per]    Script Date: 8/25/2020 1:30:52 AM ******/
IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'per')
DROP SCHEMA [per]
GO
/****** Object:  Schema [per]    Script Date: 8/25/2020 1:30:54 AM ******/
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'per')
EXEC sys.sp_executesql N'CREATE SCHEMA [per] AUTHORIZATION [dbo]'
GO
/****** Object:  Table [per].[User_Filters]    Script Date: 8/25/2020 10:47:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[per].[User_Filters]') AND type in (N'U'))
BEGIN
CREATE TABLE [per].[User_Filters](
	[user_filter_id] [UNIQUEIDENTIFIER] NOT NULL,
	[filter_name] [nvarchar](400) NOT NULL,
	[content_json] [nvarchar](max) NOT NULL,
	[user] [nvarchar](400) NOT NULL,
	[modifiedOn] [datetime] NOT NULL,
	[scope] [int] NULL,
 CONSTRAINT [PK_User_Filters] PRIMARY KEY CLUSTERED 
(
	[user_filter_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO

/****** Object:  Table [gis].[MobileLayerRenderer]    Script Date: 7/23/2021 1:35:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [gis].[MobileLayerRenderer](
	[MobileLayerRendererId] [uniqueidentifier] NOT NULL,
	[layersourceID] [int] NULL,
	[rendererRules] [nvarchar](max) NULL,
	[query] [nvarchar](max) NULL,
	[name] [nvarchar](255) NULL,
	[createdTimestamp] [datetime] NULL,
	[lastModifiedTimestamp] [datetime] NULL,
	[isSelected] [bit] NULL,
	[role] [nvarchar](100) NULL,
	[categories] [nvarchar](4000) NULL,
PRIMARY KEY CLUSTERED 
(
	[MobileLayerRendererId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [gis].[MobileLayerRenderer]  WITH CHECK ADD  CONSTRAINT [FK_MobileLayerRenderer_LayerSource] FOREIGN KEY([layersourceID])
REFERENCES [gis].[LayerSource] ([LayerSourceId])
GO

ALTER TABLE [gis].[MobileLayerRenderer] CHECK CONSTRAINT [FK_MobileLayerRenderer_LayerSource]
GO

insert into [gis].[MobileLayerRenderer](
    [MobileLayerRendererId],
	[layersourceID],
	[rendererRules],
	[query],
	[name],
	[createdTimestamp],
	[lastModifiedTimestamp],
	[isSelected],
	[role],
	[categories]
) values (
newid(),
1,
'{rules:{name:"Building Grade",type:"unique",colors:[{value:1,description:"Cabin",fillColor:"rgba(0,0,255,0.4)",outlineColor:"#000000"},{value:2,description:"Substandard",fillColor:"rgba(0,0,255,0.4)",outlineColor:"#000000"},{value:3,description:"Poor",fillColor:"rgba(0,0,255,0.4)",outlineColor:"#000000"},{value:4,description:"Low",fillColor:"rgba(0,115,255,0.4)",outlineColor:"#000000"},{value:5,description:"Fair",fillColor:"rgba(0,115,255,0.4)",outlineColor:"#000000"},{value:6,description:"Low average",fillColor:"rgba(0,229,255,0.4)",outlineColor:"#000000"},{value:7,description:"Average",fillColor:"rgba(19,236,187,0.4)",outlineColor:"#000000"},{value:8,description:"Good",fillColor:"rgba(19,236,187,0.4)",outlineColor:"#000000"},{value:9,description:"Better",fillColor:"rgba(189,250,5,0.4)",outlineColor:"#000000"},{value:10,description:"Very good",fillColor:"rgba(255,255,128,0.4)",outlineColor:"#000000"},{value:11,description:"Exelent",fillColor:"rgba(250,194,5,0.4)",outlineColor:"#000000"},{value:12,description:"Luxury",fillColor:"rgba(255,111,0,0.4)",outlineColor:"#000000"},{value:13,description:"Mansion",fillColor:"rgba(255,0,0,0.4)",outlineColor:"#000000"},{value:20,description:"Exceptional",fillColor:"rgba(255,0,255,0.4)",outlineColor:"#000000"}]},layer:{id:"building-grade",type:"fill",source:"parcelSource","source-layer":"parcel",paint:{"fill-color":["case",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],0],"rgba(255,255,255,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],1],"rgba(0,0,255,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],2],"rgba(0,0,255,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],3],"rgba(0,0,255,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],4],"rgba(0,0,255,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],5],"rgba(0,115,255,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],6],"rgba(0,229,255,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],7],"rgba(19,236,187,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],8],"rgba(0,255,55,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],9],"rgba(189,250,5,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],10],"rgba(255,255,128,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],11],"rgba(250,194,5,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],12],"rgba(255,111,0,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],13],"rgba(255,0,0,0.4)",["==",["coalesce",["feature-state","ptas_buildinggrade"],0],20],"rgba(255,0,255,0.4)","rgba(0,0,0,0.00000)"],"fill-outline-color":"#000000"}}}',
'select p.ptas_major, p.ptas_minor, b.ptas_buildinggrade
from ptas_buildingdetail as b
inner join ptas_parceldetail as p on b._ptas_parceldetailid_value = p.ptas_parceldetailid',
'Building grade',
getutcdate(),
getutcdate(),
0,
'Residential',
'Residential'
);


/***** Object:  Table [dbo].[DynamicsDeltaSyncState]    Script Date: 9/23/2021 4:56:14 PM *****/
CREATE TABLE [dbo].[DynamicsDeltaSyncState](
	[EntityName] [nvarchar](200) NOT NULL,
	[SyncUrl] [nvarchar](max) NULL,
	[DateAdded] [datetime] NULL,
 CONSTRAINT [PK_DynamicsDeltaSyncState] PRIMARY KEY CLUSTERED 
(
	[EntityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[DynamicsDeltaSyncState] ADD  CONSTRAINT [DF_DynamicsDeltaSyncState_DateAdded]  DEFAULT (getdate()) FOR [DateAdded]
GO