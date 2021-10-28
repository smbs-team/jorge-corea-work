EXEC SP_DropColumn_With_Constraints  '[cus].[UserProject]', 'SplitModelProperty'
ALTER TABLE [cus].UserProject ADD SplitModelProperty NVARCHAR(256) NULL
GO -- 1. Add SplitModelProperty field to [UserProject]

EXEC SP_DropColumn_With_Constraints  '[cus].[UserProject]', 'SplitModelValue'
ALTER TABLE [cus].UserProject ADD SplitModelValue NVARCHAR(MAX) NULL
GO -- 2. Add SplitModelValue field to [UserProject]

EXEC SP_DropColumn_With_Constraints  '[cus].[UserProject]', 'ModelArea'
ALTER TABLE [cus].UserProject ADD ModelArea INT NOT NULL DEFAULT 0
GO -- 3. Add ModelArea field to [UserProject]


DECLARE @Metadata NVARCHAR(MAX)
DELETE FROM [dbo].[MetadataStoreItem] WHERE [ItemName] = 'SplitModelValues'

SET @Metadata  = 
	'[ ' +
		'{ ' +
			'"SplitModelProperty": "PresentUse", ' +
			'"SplitModelValues": ["Single Family Home", "Townhouse", "Waterfront"] ' +
		'}, ' +
		'{ ' +
			'"SplitModelProperty": "SubArea", ' +
			'"SplitModelValues": "[]" ' +
		'} ' +
	'] ' 

INSERT [dbo].[MetadataStoreItem] ([Version], [StoreType], [ItemName], [Value]) VALUES (1, N'GlobalConstant', N'SplitModelValues', @Metadata)
GO --  4. Add SplitModelValues to [dbo].[MetadataStoreItem]

DROP TABLE IF EXISTS [dbo].[UserJobNotification]

CREATE TABLE [dbo].[UserJobNotification]
(
	[JobNotificationId] INT NOT NULL IDENTITY,    	
    [UserId] UNIQUEIDENTIFIER NOT NULL,
	[JobId] INT NULL,
	[JobType] NVARCHAR(256) NOT NULL,
	[JobNotificationText] NVARCHAR(MAX) NOT NULL,
	[JobNotificationType] NVARCHAR(16) NOT NULL,
	[JobNotificationPayload] NVARCHAR(MAX) NULL,
	[ErrorMessage] NVARCHAR(MAX) NULL,
	[Dismissed] BIT NOT NULL DEFAULT 0,
	[CreatedTimestamp] DateTime NOT NULL DEFAULT (getdate()),

	CONSTRAINT [PK_UserJobNotification] PRIMARY KEY ([JobNotificationId]),
	CONSTRAINT [FK_UserJobNotification_ToJob_JobId] FOREIGN KEY ([JobId]) REFERENCES [dbo].[WorkerJobQueue]([JobId])
)

ALTER TABLE [dbo].[UserJobNotification]  WITH NOCHECK ADD  CONSTRAINT [FK_UserJobNotification_ToSystemUser_UserId] FOREIGN KEY([UserId])
REFERENCES [dynamics].[systemuser] ([systemuserid])

DROP INDEX IF EXISTS [IX_UserJobNotification_Composite] ON [dbo].[UserJobNotification]
CREATE INDEX [IX_UserJobNotification_Composite] ON [dbo].[UserJobNotification] ([UserId], [Dismissed], [CreatedTimestamp])

DROP INDEX IF EXISTS [IX_UserJobNotification_JobId] ON [dbo].[UserJobNotification]
CREATE INDEX [IX_UserJobNotification_JobId] ON [dbo].[UserJobNotification] ([JobId])

GO -- 5. Create [UserNotification] table