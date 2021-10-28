CREATE TABLE [cus].[InteractiveChart]
(
	[InteractiveChartId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DatasetId] UNIQUEIDENTIFIER NOT NULL,
    [ChartType] NVARCHAR(256) NOT NULL,
    [ChartTitle] NVARCHAR(256) NOT NULL,
    [CreatedBy] UNIQUEIDENTIFIER NOT NULL, 
    [LastModifiedBy] UNIQUEIDENTIFIER NOT NULL,
    [CreatedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastModifiedTimestamp] DATETIME NOT NULL DEFAULT GETDATE()

    CONSTRAINT [FK_InteractiveChart_ToDataset] FOREIGN KEY ([DatasetId]) REFERENCES [cus].[Dataset]([DatasetId]),
    CONSTRAINT [FK_InteractiveChart_ToChartType] FOREIGN KEY ([ChartType]) REFERENCES [cus].[ChartType]([ChartType]),

    CONSTRAINT [FK_InteractiveChat_ToSystemUser_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
    CONSTRAINT [FK_InteractiveChat_ToSystemUser_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid])
)
GO

ALTER TABLE [cus].[InteractiveChat] NOCHECK CONSTRAINT [FK_InteractiveChat_ToSystemUser_CreatedBy]
GO

ALTER TABLE [cus].[InteractiveChat] NOCHECK CONSTRAINT [FK_InteractiveChat_ToSystemUser_LastModifiedBy]
GO


CREATE INDEX [IX_InteractiveChart_ChartTitle] ON [cus].[InteractiveChart] ([ChartTitle])
GO

CREATE INDEX [IX_InteractiveChart_DatasetId] ON [cus].[InteractiveChart] ([DatasetId])
GO