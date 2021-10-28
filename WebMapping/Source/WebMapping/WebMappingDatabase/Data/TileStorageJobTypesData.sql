GO
INSERT [gis].[TileStorageJobType] ([JobTypeId], [SourceLocation], [JobFormat], [TargetLocation],[MaxTimeInSeconds]) VALUES (0, N'gis.CONTOUR005_LINE(SHAPE)', 0, N'GPKG\Contour005_Line.gpkg', 86400)
GO
INSERT [gis].[TileStorageJobType] ([JobTypeId], [SourceLocation], [JobFormat], [TargetLocation], [MaxTimeInSeconds]) VALUES (1, N'gis.PARCEL_GEOM_AREA(Shape)', 0, N'GPKG\Parcel.gpkg', 86400)
GO
INSERT [gis].[TileStorageJobType] ([JobTypeId], [SourceLocation], [JobFormat], [TargetLocation], [MaxTimeInSeconds]) VALUES (2, N'WAKING058411.ecw', 2, N'GPKG\WAKING058411.ecw', 600)
GO
INSERT [gis].[TileStorageJobType] ([JobTypeId], [SourceLocation], [JobFormat], [TargetLocation], [MaxTimeInSeconds]) VALUES (3, N'gis.PARCEL_GEOM_AREA(Shape)', 3, N'gis.PARCEL', 86400)
GO
