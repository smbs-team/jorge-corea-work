IF EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'fn_ReturnMetaData')
    DROP FUNCTION dynamics.fn_ReturnMetaData;  
GO  

CREATE FUNCTION dynamics.fn_ReturnMetaData (@itemName AS NVARCHAR(100), @storeType AS NVARCHAR(100))
RETURNS NVARCHAR(500) AS 
BEGIN
	DECLARE @returnItem nvarchar(500) 

    SELECT @returnItem = COALESCE(Value,'') FROM [dbo].[MetadataStoreItem] mds
	WHERE mds.ItemName =  @itemName
	AND mds.StoreType = @storeType
	AND mds.Version = (SELECT MAX(mds2.Version) 
						 FROM MetadataStoreItem mds2 
						WHERE mds2.ItemName = mds.ItemName)

    RETURN @returnItem
END

