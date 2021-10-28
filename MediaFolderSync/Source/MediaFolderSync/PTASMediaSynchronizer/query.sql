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
