EXEC SP_DropColumn_With_Constraints  '[cus].[RScriptModel]', 'RScriptDisplayName'
ALTER TABLE [cus].[RScriptModel] ADD [RScriptDisplayName] NVARCHAR(256) NULL

GO -- 1. Add [RScriptDisplayName] field to [RScriptModel] table

EXEC SP_DropColumn_With_Constraints  '[cus].[CustomSearchParameter]', 'DisplayOrder'
ALTER TABLE [cus].[CustomSearchParameter] ADD [DisplayOrder] INT NOT NULL DEFAULT 0

GO -- 2. Add [DisplayOrder] field to [CustomSearchParameter] table


EXEC SP_DropColumn_With_Constraints  '[cus].[CustomSearchParameter]', 'DisplayName'
ALTER TABLE [cus].[CustomSearchParameter] ADD [DisplayName] NVARCHAR(256) NULL

GO -- 3. Add [DisplayName] field to [CustomSearchParameter] table