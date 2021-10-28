
CREATE FUNCTION [cus].[FN_ValidateRole]
(
	@UserRoles NVARCHAR(MAX),
	@RequiredRole NVARCHAR(512)
)
RETURNS BIT
AS
BEGIN
    -- Declare the return variable here
    DECLARE @RoleCount int = 0;

	SELECT @RoleCount = count(value) 
		FROM STRING_SPLIT(@UserRoles, ',')  
		WHERE LOWER(TRIM(value)) = LOWER(TRIM(@RequiredRole));

	RETURN (IIF(@RoleCount > 0, 1, 0))
END
GO


GO -- 1. Create [FN_ValidateRole] user function.


IF (NOT EXISTS(SELECT * FROM [dbo].[MetadataStoreItem] WHERE [ItemName] = 'MinLandValue'))
BEGIN
	INSERT [dbo].[MetadataStoreItem] ([Version], [StoreType], [ItemName], [Value]) VALUES (1, N'GlobalConstant', N'MinLandValue', N'1000')
END
GO --  2. Add Imported to [dbo].[MetadataStoreItem]


EXEC SP_DropColumn_With_Constraints  '[cus].[CustomSearchDefinition]', 'Version'
ALTER TABLE [cus].[CustomSearchDefinition] ADD [Version] INT NOT NULL DEFAULT 0
GO -- 3. Add Version field to [CustomSearchDefinition]

DROP TABLE IF EXISTS [cus].[ProjectVersionType]
CREATE TABLE [cus].[ProjectVersionType](
	[ProjectVersionType] [nvarchar](256) NOT NULL,

	CONSTRAINT PK_ProjectVersionType PRIMARY KEY ([ProjectVersionType]))
GO -- 4. Create Table

IF (NOT EXISTS(SELECT * FROM [cus].[ProjectVersionType] WHERE [ProjectVersionType] = 'Draft'))
BEGIN
	INSERT [cus].[ProjectVersionType] ([ProjectVersionType]) VALUES ('Draft')
END

IF (NOT EXISTS(SELECT * FROM [cus].[ProjectVersionType] WHERE [ProjectVersionType] = 'Adjustments'))
BEGIN
	INSERT [cus].[ProjectVersionType] ([ProjectVersionType]) VALUES ('Adjustments')
END

IF (NOT EXISTS(SELECT * FROM [cus].[ProjectVersionType] WHERE [ProjectVersionType] = 'WhatIf'))
BEGIN
	INSERT [cus].[ProjectVersionType] ([ProjectVersionType]) VALUES ('WhatIf')
END

IF (NOT EXISTS(SELECT * FROM [cus].[ProjectVersionType] WHERE [ProjectVersionType] = 'Frozen'))
BEGIN
	INSERT [cus].[ProjectVersionType] ([ProjectVersionType]) VALUES ('Frozen')
END
GO -- 5. Add Version Types

EXEC SP_DropColumn_With_Constraints  '[cus].[UserProject]', 'IsFrozen'
ALTER TABLE [cus].UserProject ADD IsFrozen BIT NOT NULL DEFAULT 0
GO -- 6. Add IsFrozen field to [UserProject]

EXEC SP_DropColumn_With_Constraints  '[cus].[UserProject]', 'VersionType'
ALTER TABLE [cus].UserProject ADD VersionType NVARCHAR(256) NOT NULL DEFAULT 'Draft'

ALTER TABLE [cus].UserProject  WITH NOCHECK ADD CONSTRAINT [FK_UserProject_ProjectVersionType] 
	FOREIGN KEY([VersionType])
	REFERENCES [cus].[ProjectVersionType] ([ProjectVersionType])
GO -- 7. Add VersionType field to [UserProject]


