/*
Modifies the columns and create indexes
*/
ALTER TABLE [dynamics].[ptas_parceldetail] ALTER COLUMN [ptas_propertyname] NVARCHAR (500) NULL 
GO
ALTER TABLE [dynamics].[ptas_parceldetail] ALTER COLUMN [ptas_namesonaccount] NVARCHAR (500) NULL 
GO
ALTER TABLE [dynamics].[ptas_parceldetail] ALTER COLUMN [ptas_applgroup] NVARCHAR (1) NULL 
GO
ALTER TABLE [dynamics].[ptas_year] ALTER COLUMN [ptas_name] NVARCHAR ( 4 ) NULL 
GO



