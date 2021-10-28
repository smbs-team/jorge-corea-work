
IF (NOT EXISTS(SELECT * FROM [cus].[DatasetPostProcessState] ))
BEGIN
    INSERT [cus].[DatasetPostProcessState] ([DatasetPostProcessState]) VALUES (N'NeedsPostProcessUpdate')
    INSERT [cus].[DatasetPostProcessState] ([DatasetPostProcessState]) VALUES (N'NotProcessed')
    INSERT [cus].[DatasetPostProcessState] ([DatasetPostProcessState]) VALUES (N'Processed')
    INSERT [cus].[DatasetPostProcessState] ([DatasetPostProcessState]) VALUES (N'Processing')
END

IF (NOT EXISTS(SELECT * FROM [cus].[DatasetState] ))
BEGIN
    INSERT [cus].[DatasetState] ([DatasetState]) VALUES (N'NotProcessed')
    INSERT [cus].[DatasetState] ([DatasetState]) VALUES (N'GeneratingDataset')
    INSERT [cus].[DatasetState] ([DatasetState]) VALUES (N'GeneratingIndexes')
    INSERT [cus].[DatasetState] ([DatasetState]) VALUES (N'Processed')
    INSERT [cus].[DatasetState] ([DatasetState]) VALUES (N'ExecutingPostProcess')
END

GO --0. Add dataset post-process states

UPDATE [gis].[LayerSource] SET EmbeddedDataFields = '[{"FieldName": "MAJOR", "FieldType": "String"},{"FieldName": "MINOR", "FieldType": "String"},{"FieldName": "PIN", "FieldType": "String"}]' WHERE LayerSourceName = 'Parcel'
UPDATE [gis].[LayerSource] SET EmbeddedDataFields = '[{"FieldName": "ELEVATION", "FieldType": "Double"},{"FieldName": "FIPS_CODE", "FieldType": "String"},{"FieldName": "IDX_20FT", "FieldType": "Double"},{"FieldName": "IDX_40FT", "FieldType": "Double"},{"FieldName": "IDX_50FT", "FieldType": "Double"},{"FieldName": "IDX_100FT", "FieldType": "Double"}]' WHERE LayerSourceName = 'Parcel'

GO --1. Updating embedded data fields in JSON

DROP TABLE IF EXISTS [dbo].[UserDataStoreItem] 

CREATE TABLE [dbo].[UserDataStoreItem]
(
	[UserDataStoreItemId] INT NOT NULL IDENTITY,     
    [UserId] UNIQUEIDENTIFIER NOT NULL,
	[StoreType] VARCHAR(36) NOT NULL,
	[OwnerType] NVARCHAR(256) NOT NULL DEFAULT 'NoOwnerType',
	[OwnerObjectId] VARCHAR(36) NOT NULL DEFAULT '',	
	[ItemName] NVARCHAR(64) NOT NULL DEFAULT '',
	[Value] NVARCHAR(MAX) NULL,

	PRIMARY KEY ([UserId], [StoreType], [OwnerType], [OwnerObjectId], [ItemName]),
	CONSTRAINT [FK_UserDataStoreItem_ToSystemUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
	CONSTRAINT [FK_UserDataStoreItem_ToOwnerType_OwnerType] FOREIGN KEY ([OwnerType]) REFERENCES [cus].[OwnerType]([OwnerType]),
)

CREATE INDEX [IX_UserDataStoreItem_Secondary] ON [dbo].[UserDataStoreItem] ([UserId], [StoreType], [OwnerType], [OwnerObjectId])

GO --2. Recreate [UserDataStoreItem] to fix primary key.

DROP TABLE IF EXISTS [dbo].[MetadataStoreItem] 

CREATE TABLE [dbo].[MetadataStoreItem]
(
	[MetadataStoreItemId] INT NOT NULL IDENTITY,     
	[Version] INT NOT NULL DEFAULT -1,
	[StoreType] VARCHAR(36) NOT NULL,
	[ItemName] NVARCHAR(64) NOT NULL DEFAULT '',
	[Value] NVARCHAR(MAX) NULL,

	PRIMARY KEY ([StoreType], [ItemName], [Version])
)

GO --3. Create [UserDataStoreItem] to fix primary key.


IF NOT EXISTS(SELECT * FROM [cus].[ExpressionRole] WHERE [ExpressionRole] = N'DynamicEvaluator')
BEGIN
    INSERT [cus].[ExpressionRole]([ExpressionRole]) VALUES (N'DynamicEvaluator')
END

GO --4. Add new expression role for dynamic evaluator.

EXEC SP_DropColumn_With_Constraints '[cus].[Dataset]', 'IsDataLocked'
ALTER TABLE cus.[Dataset] ADD [IsDataLocked] BIT NULL
GO --5. Add IsLocked table to dataset

DECLARE @PkRenameStatement NVARCHAR(MAX)

CREATE TABLE #SqlStatements (
    SqlStatement NVARCHAR(MAX)
)

;WITH PKNames
AS (
SELECT name AS IndexName
,OBJECT_NAME(object_id) AS TableName
,OBJECT_SCHEMA_NAME(object_id) as SchemaName
,(SELECT '' + c.name
FROM sys.index_columns ic
INNER JOIN sys.columns c
ON ic.object_id = c.object_id
AND ic.index_column_id = c.column_id
WHERE ic.object_id = i.object_id
AND ic.index_id = i.index_id
AND ic.is_included_column = 0
ORDER BY ic.key_ordinal
FOR XML PATH('')) AS Columns
FROM sys.indexes i
WHERE i.is_primary_key = 1 and ((OBJECT_SCHEMA_NAME(object_id) = 'cus' or OBJECT_SCHEMA_NAME(object_id) = 'gis') or (OBJECT_NAME(object_id) like 'UserDataStoreItem') or (OBJECT_NAME(object_id) like 'MetadataStoreItem'))
)
Insert Into #SqlStatements
SELECT 'EXEC sp_rename ''' + QUOTENAME(SchemaName) + '.' + QUOTENAME(IndexName) + ''', ''PK_' + TableName + '_' + Columns + ''''
FROM PKNames

PRINT 'Renaming PKs'
-- Iterate over all customers
WHILE (1 = 1) 
BEGIN  

  -- Get next customerId
  SELECT TOP 1 @PkRenameStatement = SqlStatement
  FROM #SqlStatements

  -- Exit loop if no more customers
  IF @@ROWCOUNT = 0 BREAK;

  -- call your sproc
  EXEC sp_executesql @PkRenameStatement;
  PRINT @PkRenameStatement
  delete top(1) from  #SqlStatements  

END 
GO -- 6. PK Name fix for scaffolding
