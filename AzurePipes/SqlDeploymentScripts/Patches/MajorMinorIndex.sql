alter table [dynamics].[ptas_parceldetail]
alter column ptas_major nvarchar(6)

alter table [dynamics].[ptas_parceldetail]
alter column ptas_minor nvarchar(4)

alter table [dynamics].[ptas_parceldetail]
alter column ptas_name nvarchar(11)

alter table [dynamics].[ptas_parceldetail_snapshot]
alter column ptas_major nvarchar(6)

alter table [dynamics].[ptas_parceldetail_snapshot]
alter column ptas_minor nvarchar(4)

alter table [dynamics].[ptas_parceldetail_snapshot]
alter column ptas_name nvarchar(11)

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'Idx_ptas_parceldetail_ptas_major_ptas_minor')   
    DROP INDEX Idx_ptas_parceldetail_ptas_major_ptas_minor ON [dynamics].[ptas_parceldetail]
GO  
CREATE NONCLUSTERED INDEX [Idx_ptas_parceldetail_ptas_major_ptas_minor] ON 
[dynamics].[ptas_parceldetail] (ptas_major ASC, ptas_minor ASC)
GO