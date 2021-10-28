CREATE PROCEDURE [dbo].[PopNextStorageJob]
AS
	set nocount on;

	DECLARE @JobId int = -1;

	with cte as (
		select top(1) * 
		from gis.TileStorageJobQueue as tsj with (rowlock, readpast)
		where ((StartedTimestamp is NULL) or 
			(DATEDIFF(second, StartedTimestamp, GETDATE()) > (select MaxTimeInSeconds from gis.TileStorageJobType where JobTypeId = tsj.JobTypeId)))
		order by CreatedTimestamp ASC)
	update cte SET 
		StartedTimestamp = GETDATE(),
		@JobId = JobId;

RETURN @JobId
