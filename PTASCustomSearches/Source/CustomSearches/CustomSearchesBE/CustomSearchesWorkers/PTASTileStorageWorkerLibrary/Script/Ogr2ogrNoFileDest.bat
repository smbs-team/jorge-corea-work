REM argDestFormat=%1
REM argDest=%2
REM argSource=%3
REM sourceTransform=%4
REM targetTransform=%5
REM newLayerName=%6
REM outputFileName=%7

SET "PATH=%SDK_ROOT%bin;%SDK_ROOT%bin\gdal\python\osgeo;%SDK_ROOT%bin\proj6\apps;%SDK_ROOT%bin\gdal\apps;%SDK_ROOT%bin\ms\apps;%SDK_ROOT%bin\gdal\csharp;%SDK_ROOT%bin\ms\csharp;%SDK_ROOT%bin\curl;%PATH%"
SET "GDAL_DATA=%SDK_ROOT%bin\gdal-data"
SET "GDAL_DRIVER_PATH=%SDK_ROOT%bin\gdal\plugins"
SET "PYTHONPATH=%SDK_ROOT%bin\gdal\python;%SDK_ROOT%bin\ms\python"
SET "PROJ_LIB=%SDK_ROOT%bin\proj6\SHARE"

ogr2ogr.exe -f %1 %2 %3 -progress -s_srs %4 -t_srs %5 -skipfailures -progress -lco OVERWRITE=YES -lco SCHEMA=GIS -nln %6 -overwrite --config MSSQLSPATIAL_USE_BCP YES --config MSSQLSPATIAL_BCP_SIZE 10000 > %7 2>&1
