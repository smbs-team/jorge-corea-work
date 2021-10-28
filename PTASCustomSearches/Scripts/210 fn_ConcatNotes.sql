create function dynamics.fn_ConcatNotes(
	@Id uniqueidentifier,					-- UniqueIdentifier that represents the parcel or the sale
	@DesiredNumberOfMostRecentNotes int,	-- Number of notes to concatenate
	@NotesType int,							-- 0 = Parcel Notes, 1 = Sales Notes, 2 = Sale Warning Notes
	@maxString int							-- Maximum size of the string returned
)
returns nvarchar(max)
as
begin
declare @val varchar(max)
declare @lastLabel varchar(100)

if (@NotesType = 0) begin
	select @val = coalesce(@val + ', ' + ptas_notetext, ptas_notetext),
	       @lastLabel = '... AND OTHER NOTES.'
	  from (select top (@DesiredNumberOfMostRecentNotes)
			       ptas_notetext
  		      from dynamics.ptas_camanotes
			 where _ptas_parcelid_value = @Id
		     order by modifiedon desc) as st
end

if (@NotesType = 1) begin
	select @val = coalesce(@val + ', ' + ptas_notetext, ptas_notetext),
	       @lastLabel = '... AND OTHER NOTES.'
	  from (select top (@DesiredNumberOfMostRecentNotes)
			       ptas_notetext
  		      from dynamics.ptas_salesnote
			 where _ptas_saleid_value = @Id
		     order by modifiedon desc) as st
end

if (@NotesType = 2) begin
	select @val = coalesce(@val + '; ' + ptas_name, ptas_name),
	       @lastLabel = '... OTHER WARNINGS;'
	  from (select top (@DesiredNumberOfMostRecentNotes)
			       substring(upper(swc.ptas_name),1,1) + substring(lower(swc.ptas_name),2,149) ptas_name
  		      from dynamics.ptas_saleswarningcode swc
			       inner join dynamics.ptas_sales_ptas_saleswarningcode sswc
				      on swc.ptas_saleswarningcodeid = sswc.ptas_saleswarningcodeid
					 and sswc.ptas_salesid = @Id) as st
end

if (len(@val) > (@maxString - len(@lastLabel))) begin
	set @val = substring(@val, 1, @maxString - len(@lastLabel)) + @lastLabel
end

return @val;
end