IF EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'fn_GetValueFromStringMap')
    DROP FUNCTION dynamics.fn_GetValueFromStringMap;  
GO  

CREATE FUNCTION dynamics.fn_GetValueFromStringMap(@objecttypecode nvarchar(1000), @attributename nvarchar(1000), @attributevalue bigint)  
RETURNS nvarchar(100)   
AS   
/*
Author: Jairo Barquero
Date Created:  10/27/2020
Description:    This Function return the description based on id and attribute name from an specific column in table dynamics.stringmap

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/

-- Return the string value for an specific code
BEGIN  
	DECLARE @return nvarchar(1000)

	 SELECT @return = psm.value   
	   FROM dynamics.stringmap psm
	  WHERE psm.objecttypecode = @objecttypecode
	    AND psm.attributename = @attributename
		AND psm.attributevalue = @attributevalue 
   
	RETURN substring(@return,1,1000);  
END; 
