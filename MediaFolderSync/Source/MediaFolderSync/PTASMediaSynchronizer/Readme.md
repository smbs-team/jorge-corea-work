# Media Synchronizer
## Important values App.Config

## What it does

After running the AZCopy script to move all the media:

```cmd
cd "C:\Program Files (x86)\Microsoft SDKs\Azure\AzCopy\" 

azcopy.exe /source:\\asr-nas-dr\media$\Prod /dest:https://ptasdevstorage.file.core.windows.net/media /destkey:[key] /s /xo 

pause 
```

Use the following SQL to fetch changes from the database.

```sql
(SELECT  AccyMedGuid as Id, FileExtension, UpdateDate
FROM            AccyMedia
where UpdateDate>@LastCheckDate)

union 

(SELECT  BpMedGuid as Id, FileExtension, UpdateDate
FROM            BldgPermitMedia
where UpdateDate>@LastCheckDate)

union

(SELECT  BldgMedGuid as Id, FileExtension, UpdateDate
FROM            CommBldgMedia
where UpdateDate>@LastCheckDate)

union

(SELECT  FloatMedGuid as Id, FileExtension, UpdateDate
FROM            FloatMedia
where UpdateDate>@LastCheckDate)

union

(SELECT  HinMedGuid as Id, FileExtension, UpdateDate
FROM            HINoteMedia
where UpdateDate>@LastCheckDate)

union

(SELECT  LndMedGuid as Id, FileExtension, UpdateDate
FROM            LandMedia
where UpdateDate>@LastCheckDate)

union

(SELECT  MhMedGuid as Id, FileExtension, UpdateDate
FROM            MhAcctMedia
where UpdateDate>@LastCheckDate)

union

(SELECT  BldgMedGuid as Id, FileExtension, UpdateDate
FROM            ResBldgMedia
where UpdateDate>@LastCheckDate)

union

(SELECT  RnMedGuid as Id, FileExtension, UpdateDate
FROM            ReviewNoteMedia
where UpdateDate>@LastCheckDate)

union

(SELECT  RpnMedGuid as Id, FileExtension, UpdateDate
FROM            RpNoteMedia
where UpdateDate>@LastCheckDate)

union

(SELECT  MediaGuid as Id, FileExtension, UpdateDate as UpdateDate
FROM            RPSMedia
where UpdateDate>@LastCheckDate)

union

(SELECT  SnMedGuid as Id, FileExtension, UpdateDate as UpdateDate
FROM            SaleNoteMedia
where UpdateDate>@LastCheckDate)

```

And then saves them to the azure storage.