REM argDestFormat=%1
REM argDest=%2
REM targetFolder=%3
REM argSource=%4
REM sourceTransform=%5
REM targetTransform=%6

SET "PATH=%SDK_ROOT%bin;%SDK_ROOT%bin\gdal\python\osgeo;%SDK_ROOT%bin\proj6\apps;%SDK_ROOT%bin\gdal\apps;%SDK_ROOT%bin\ms\apps;%SDK_ROOT%bin\gdal\csharp;%SDK_ROOT%bin\ms\csharp;%SDK_ROOT%bin\curl;%PATH%"
SET "GDAL_DATA=%SDK_ROOT%bin\gdal-data"
SET "GDAL_DRIVER_PATH=%SDK_ROOT%bin\gdal\plugins"
SET "PYTHONPATH=%SDK_ROOT%bin\gdal\python;%SDK_ROOT%bin\ms\python"
SET "PROJ_LIB=%SDK_ROOT%bin\proj6\SHARE"

REM Creates target folder if it doesn't exists
if not exist %3 mkdir %3
cd \MapServer

ogr2ogr.exe -f %1 %2 %4 -progress -s_srs %5 -t_srs %6
