write-host 'Extracting MapServer-GDAL...'
Expand-Archive -Path c:\temp\mapserver-gdal-bin.zip -DestinationPath c:\MapServer
move C:/MapServer/bin/gdal/plugins-optional/ogr_MSSQLSpatial.dll C:/MapServer/bin/gdal/plugins