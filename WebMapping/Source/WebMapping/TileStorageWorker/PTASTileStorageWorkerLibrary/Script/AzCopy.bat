REM sourceFile=%1
REM targetFileName=%2
REM targetFolder=%3

REM Creates target folder if it doesn't exists
if not exist %3 mkdir %3
del %3\%2.old

azcopy\azcopy.exe copy %1 %3\%2.tmp --overwrite=true
ren %3\%2 %2.old
ren %3\%2.tmp %2