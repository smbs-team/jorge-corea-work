
CREATE function [dynamics].[fn_ConcatSalesNotes](
	@Id uniqueidentifier,					-- UniqueIdentifier that represents the parcel or the sale
	@DesiredNumberOfMostRecentNotes int,	-- Number of notes to concatenate
	@WarningIds NVARCHAR(100),				-- List separated by quotes
	@maxString int							-- Maximum size of the string returned
)
returns nvarchar(max)
as
begin
declare @val		varchar(max)
declare @lastLabel	varchar(100)

	select @val = coalesce(@val + '; ' + ptas_name, ptas_name),
	       @lastLabel = '... OTHER WARNINGS;'
	  from (select top (@DesiredNumberOfMostRecentNotes)
			       substring(upper(swc.ptas_name),1,1) + substring(lower(swc.ptas_name),2,149) ptas_name
  		      from dynamics.ptas_saleswarningcode swc
			       inner join dynamics.ptas_sales_ptas_saleswarningcode sswc
				      on swc.ptas_saleswarningcodeid = sswc.ptas_saleswarningcodeid
					 and swc.ptas_id IN(select value 
										from STRING_SPLIT(@WarningIds,',')
										)  /*WarningId in Old system*/
					 and sswc.ptas_salesid = @Id) as st


if (len(@val) > (@maxString - len(@lastLabel))) begin
	set @val = substring(@val, 1, @maxString - len(@lastLabel)) + @lastLabel
end

return @val;
end