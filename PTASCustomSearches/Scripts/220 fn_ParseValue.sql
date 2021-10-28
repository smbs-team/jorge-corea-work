IF EXISTS (SELECT * FROM sys.objects WHERE type = 'TF' AND name = 'fn_ParseValue')
    DROP FUNCTION dynamics.fn_ParseValue;  
GO  

CREATE FUNCTION dynamics.fn_ParseValue( @Value as varchar(1000), @Delimiter char(1))  
--RETURNS TABLE
RETURNS @TableList TABLE (ReturnedColumn VARCHAR(256))
--WITH SCHEMABINDING
AS
BEGIN
/*
Author: Jairo Barquero
Date Created:  11/18/2020
Description:    This Function split a string into rows using a delimiter

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/

DECLARE	@ReturnedColumn VARCHAR(256),
		@pos INT


--DECLARE @PINList TABLE ([PinNumber] VARCHAR)

	WHILE CHARINDEX(@Delimiter, @Value) > 0
		 BEGIN
		  SELECT @pos  = CHARINDEX(@Delimiter, @Value)  
		  SELECT @ReturnedColumn = cast(SUBSTRING(@Value, 1, @pos-1) as VARCHAR(256))

		  INSERT INTO @TableList 
		  SELECT @ReturnedColumn

		  SELECT @Value = SUBSTRING(@Value, @pos+1, LEN(@Value)-@pos)
		 END;

	INSERT INTO @TableList
	SELECT @Value;
	
	RETURN

END	
