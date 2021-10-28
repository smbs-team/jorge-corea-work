DROP TABLE IF EXISTS [cus].[DatasetPostProcess_SecondaryDataset]

CREATE TABLE [cus].[DatasetPostProcess_SecondaryDataset]
(
	[DatasetPostProcessId] INT NOT NULL, 
	[SecondaryDatasetId] UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT PK_DatasetPostProcess_SecondaryDataset PRIMARY KEY ([DatasetPostProcessId], [SecondaryDatasetId]),

	CONSTRAINT [FK_DatasetPostProcess_SecondaryDataset_ToPostProcess] FOREIGN KEY ([DatasetPostProcessId]) REFERENCES [cus].[DatasetPostProcess]([DatasetPostProcessId]),
	CONSTRAINT [FK_DatasetPostProcess_SecondaryDataset_ToDataset] FOREIGN KEY ([SecondaryDatasetId]) REFERENCES [cus].[Dataset]([DatasetId]),
)

GO --  1. Create [DatasetPostProcess_SecondaryDataset] table.

EXEC SP_DropColumn_With_Constraints  '[cus].[CustomSearchParameter]', 'ParameterExtensions'
ALTER TABLE [cus].[CustomSearchParameter] ADD ParameterExtensions NVARCHAR(4000) NULL
GO -- 2. Add ParameterExtensions field to CustomSearchParameter


ALTER TABLE [cus].[DatasetPostProcess]  DROP CONSTRAINT IF EXISTS [FK_DatasetPostProcess_ToPrimaryPostProcess] 
EXEC SP_DropColumn_With_Constraints  '[cus].[DatasetPostProcess]', 'PrimaryDatasetPostProcessId'
ALTER TABLE [cus].[DatasetPostProcess] ADD PrimaryDatasetPostProcessId INT NULL
ALTER TABLE [cus].[DatasetPostProcess]  WITH NOCHECK ADD CONSTRAINT [FK_DatasetPostProcess_ToPrimaryPostProcess] 
	FOREIGN KEY([PrimaryDatasetPostProcessId])
	REFERENCES [cus].[DatasetPostProcess] ([DatasetPostProcessId])

GO -- 3. Add PrimaryDatasetPostProcessId field to [DatasetPostProcess]