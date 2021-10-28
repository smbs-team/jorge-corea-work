CREATE PROCEDURE [cus].[SP_GetDatasetView]
(
    @DatasetId NVARCHAR(36)
)
AS
BEGIN
    SET NOCOUNT ON

	DECLARE @TableName NVARCHAR(4000)
	DECLARE @ViewName NVARCHAR(4000)
	SELECT @TableName = GeneratedTableName FROM [cus].[Dataset] WHERE DatasetId = @DatasetId
	
	SET @ViewName = @TableName
	IF @ViewName IS NOT NULL 	   
	BEGIN
		SET @ViewName = (Concat(@TableName, '_PostProcess_View'))
		IF NOT EXISTS(select * FROM sys.views where name = @ViewName)
		BEGIN
			SET @ViewName = Concat(@TableName, '_View')
			IF NOT EXISTS(select * FROM sys.views where name = @ViewName)
			BEGIN 
				SET @ViewName = @TableName
			END
		END		
	END		

	SELECT @ViewName as ResolvedViewName
END
GO


