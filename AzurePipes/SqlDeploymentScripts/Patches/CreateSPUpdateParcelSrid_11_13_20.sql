create procedure gis.UpdateParcelSrid
	@key int
as
begin
	update [gis].[PARCEL_GEOM_AREA]
	   set Shape.STSrid = 2926
	 where [OBJECTID] = @key
end