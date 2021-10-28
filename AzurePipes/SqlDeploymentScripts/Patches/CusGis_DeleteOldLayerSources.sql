delete gis.MapRenderer from gis.MapRenderer mr inner join gis.layersource ls on mr.layersourceid = ls.layersourceid where ls.OgrLayerData is null
delete from gis.layersource where OgrLayerData is null
GO -- 1. Delete old layer sources