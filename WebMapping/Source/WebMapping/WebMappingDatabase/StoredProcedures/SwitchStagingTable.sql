CREATE PROCEDURE [dbo].[SwitchStagingTable]
	@TableName VARCHAR(MAX)
AS
DECLARE @StagingTable VARCHAR(MAX) = CONCAT(@TableName, '_staging');
DECLARE @OldTable VARCHAR(MAX) = CONCAT(@TableName, '_old');

EXEC ('delete from ' + @oldTable + '; alter table ' + @TableName + ' switch to ' + @OldTable + '; alter table ' + @StagingTable + ' switch to ' + @TableName + ';');

RETURN 0