CREATE TABLE [cus].[ChartTemplate]
(
	[ChartTemplateId] INT NOT NULL PRIMARY KEY IDENTITY,     
    [ChartType] NVARCHAR(256) NOT NULL,
    [ChartTitle] NVARCHAR(256) NOT NULL,
    [CreatedBy] UNIQUEIDENTIFIER NOT NULL, 
    [LastModifiedBy] UNIQUEIDENTIFIER NOT NULL,
    [CreatedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastModifiedTimestamp] DATETIME NOT NULL DEFAULT GETDATE()

    CONSTRAINT [FK_ChartTemplate_ToChartType] FOREIGN KEY ([ChartType]) REFERENCES [cus].[ChartType]([ChartType]),

    CONSTRAINT [FK_ChartTemplate_ToSystemUser_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
    CONSTRAINT [FK_ChartTemplate_ToSystemUser_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid])
)
GO

ALTER TABLE [cus].[ChartTemplate] NOCHECK CONSTRAINT [FK_ChartTemplate_ToSystemUser_CreatedBy]
GO

ALTER TABLE [cus].[ChartTemplate] NOCHECK CONSTRAINT [FK_ChartTemplate_ToSystemUser_LastModifiedBy]
GO


CREATE INDEX [IX_ChartTemplate_ChartTitle] ON [cus].[ChartTemplate] ([ChartTitle])
GO

GO