EXEC SP_DropColumn_With_Constraints '[cus].[ProjectType]', 'EffectiveLotSizeColumnName'
ALTER TABLE [cus].[ProjectType] ADD [EffectiveLotSizeColumnName] NVARCHAR(256) NOT NULL DEFAULT 'SqFtLot'
GO -- 1. Add [EffectiveLotSizeColumnName] field to [ProjectType]

EXEC SP_DropColumn_With_Constraints '[cus].[ProjectType]', 'DryLotSizeColumnName'
ALTER TABLE [cus].[ProjectType] ADD [DryLotSizeColumnName] NVARCHAR(256) NOT NULL DEFAULT 'SqFtLotDry'
GO -- 2. Add [DryLotSizeColumnName] field to [ProjectType]


EXEC SP_DropColumn_With_Constraints '[cus].[ProjectType]', 'WaterFrontLotSizeColumnName'
ALTER TABLE [cus].[ProjectType] ADD [WaterFrontLotSizeColumnName] NVARCHAR(256) NOT NULL DEFAULT 'WftFoot'
GO -- 3. Add [WaterFrontLotSizeColumnName] field to [ProjectType]
