IF OBJECT_ID('dbo.SqlServerPatchManifest', 'U') IS NULL 
BEGIN
	CREATE TABLE [dbo].[SqlServerPatchManifest]
	(
		[PatchId] NVARCHAR(36) NOT NULL PRIMARY KEY
	)
END

GO