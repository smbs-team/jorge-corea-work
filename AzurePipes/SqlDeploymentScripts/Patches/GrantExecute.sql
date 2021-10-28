if exists (select 1 from [sys].[database_principals] where name = 'PTASdevAZ-DBReaders') begin
	GRANT EXECUTE TO [PTASdevAZ-DBReaders]
end

if exists (select 1 from [sys].[database_principals] where name = 'PTAStestAZ-DBReaders') begin
	GRANT EXECUTE TO [PTAStestAZ-DBReaders]
end

if exists (select 1 from [sys].[database_principals] where name = 'PTASuatAZ-DBReaders') begin
	GRANT EXECUTE TO [PTASuatAZ-DBReaders]
end


if exists (select 1 from [sys].[database_principals] where name = 'PTASstagingAZ-DBReaders') begin
	GRANT EXECUTE TO [PTASstagingAZ-DBReaders]
end

if exists (select 1 from [sys].[database_principals] where name = 'PTASprodmaintAZ-DBReaders') begin
	GRANT EXECUTE TO [PTASprodmaintAZ-DBReaders]
end

if exists (select 1 from [sys].[database_principals] where name = 'PTASproductionAZ-DBReaders') begin
	GRANT EXECUTE TO [PTASproductionAZ-DBReaders]
end