CREATE TABLE [cus].[UserProject_Dataset]
(
	[UserProjectId] INT NOT NULL, 
    [DatasetId] UNIQUEIDENTIFIER NOT NULL,
	[OwnsDataset] BIT DEFAULT 0 NOT NULL,
	[DatasetRole] NVARCHAR(256) NOT NULL DEFAULT 'Population'

	PRIMARY KEY ([UserProjectId], [DatasetId]),
	CONSTRAINT [FK_UserProject_Dataset_ToUserProject] FOREIGN KEY ([UserProjectId]) REFERENCES [cus].[UserProject]([UserProjectId]),
	CONSTRAINT [FK_UserProject_Dataset_ToDataset] FOREIGN KEY ([DatasetId]) REFERENCES [cus].[Dataset]([DatasetId]),
)

GO