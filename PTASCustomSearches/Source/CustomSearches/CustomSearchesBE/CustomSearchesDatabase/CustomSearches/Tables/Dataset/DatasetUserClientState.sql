CREATE TABLE [cus].[DatasetUserClientState]
(
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    [DatasetId] UNIQUEIDENTIFIER NOT NULL,    	
    [DatasetClientState] NVARCHAR(MAX) NULL,

    CONSTRAINT PK_DatasetUserClientState PRIMARY KEY CLUSTERED ([UserId], [DatasetId]),

    CONSTRAINT [FK_DatasetUserClientState_ToDataset] FOREIGN KEY ([DatasetId]) REFERENCES [cus].[Dataset]([DatasetId])
)
GO