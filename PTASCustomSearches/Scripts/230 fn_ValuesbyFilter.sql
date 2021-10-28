IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_ValuesbyFilter')
    DROP PROCEDURE cus.sp_ValuesbyFilter;  
GO  

CREATE PROCEDURE cus.sp_ValuesbyFilter( @Value as varchar(1000), @Delimiter char(1),@TableName varchar(100), @FilteredColum varchar(100), @ReturnedColumns varchar(1000))  
 
AS
BEGIN
/*
Author: Jairo Barquero
Date Created:  11/18/2020
Description:    This Function return a list of records(any column) using different strings for the filter, 
				could be used for different tables, columns and filters, currently is executed only from SRCH_R_TaxpayerNameSearch
				for example, the user send these values:
														@Value = 'WILSON JOHN L', 
														@Delimiter = ' ',
														@TableName= 'ptas_parceldetail',
														@FilteredColum='ptas_namesonaccount',
														@ReturnColumn= 'ptas_parceldetailid,ptas_namesonaccount'
				Then the function creates this SqlCommand:
													  SELECT ptas_parceldetailid,ptas_namesonaccount                            
														FROM dynamics.ptas_parceldetail                             
													   WHERE SUBSTRING(ptas_namesonaccount,11,1) >= '0'            
														 AND (    TRANSLATE(ptas_namesonaccount,'&+-','   ') like '%WILSON %' 
														      AND TRANSLATE(ptas_namesonaccount,'&+-','   ') LIKE '%JOHN %' 
															  AND TRANSLATE(ptas_namesonaccount,'&+-','   ') LIKE '%L %'
															  )																									 
Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/

DECLARE	@ReturnedColumn VARCHAR(256),
		@pos INT,
		@sqlCommand 	NVARCHAR(MAX)

select @sqlCommand = ''
      ,@FilteredColum = @FilteredColum + ' '


	WHILE CHARINDEX(@Delimiter, @Value) > 0
		 BEGIN
		  SELECT @pos  = CHARINDEX(@Delimiter, @Value)  

		  IF LEN(TRIM(@sqlCommand)) = 0
				SELECT @sqlCommand = ' TRANSLATE('+@FilteredColum+'+'' '',''&+-'',''   '') like ''%'+ cast(SUBSTRING(@Value, 1, @pos-1) as VARCHAR(256)) + ' %'''
		  ELSE
		    SELECT @sqlCommand = @sqlCommand + ' AND TRANSLATE('+@FilteredColum+'+'' '',''&+-'',''   '') LIKE ''%'+ cast(SUBSTRING(@Value, 1, @pos-1) as VARCHAR(256)) + ' %'''

			SELECT @Value = SUBSTRING(@Value, @pos+1, LEN(@Value)-@pos)
		 END;
	IF LEN(@sqlCommand) > 0 	
		SELECT @sqlCommand = @sqlCommand +' AND TRANSLATE('+@FilteredColum+'+'' '',''&+-'',''   '') LIKE ''%'+ @Value + ' %'''
	ELSE
		SELECT @sqlCommand = @sqlCommand +' TRANSLATE('+@FilteredColum+'+'' '',''&+-'',''   '') LIKE ''%'+ @Value + ' %'''
	

	SELECT @sqlCommand ='INSERT INTO #SearchValues '
	                   +'SELECT '+ @ReturnedColumns 
					   +' FROM dynamics.'+ @TableName 
					   +' WHERE SUBSTRING(ptas_acctnbr,11,1) >= ''0'' AND ('
	                   + @sqlCommand +')'
	

	--INSERT INTO #SearchValues
	EXEC (@sqlCommand)
	RETURN 

END	
