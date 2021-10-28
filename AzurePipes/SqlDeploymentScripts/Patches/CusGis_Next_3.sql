BEGIN TRY  
DELETE FROM [cus].[DataType] WHERE DataType NOT IN ('Boolean','Byte[]','Char[]','DateTime','DateTimeOffset','Decimal','Double','Guid','Int16','Int32','Int64','Single','String','TimeSpan')
END TRY
BEGIN CATCH  
     PRINT 'Could not delete old Data Types' 
END CATCH  
GO --1. Delete old Data Types if possible

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'Boolean')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'Boolean')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'Byte[]')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'Byte[]')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'Char[]')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'Char[]')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'DateTime')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'DateTime')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'DateTimeOffset')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'DateTimeOffset')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'Decimal')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'Decimal')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'Double')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'Double')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'Guid')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'Guid')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'Int16')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'Int16')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'Int32')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'Int32')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'Int64')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'Int64')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'Single')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'Single')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'String')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'String')

IF NOT EXISTS (SELECT [DataType] FROM [cus].[DataType] WHERE [DataType] = 'TimeSpan')
	INSERT [cus].[DataType] ([DataType]) VALUES (N'TimeSpan')
GO --1. Re-Add new Data Types