CREATE TABLE [cus].[UserProject]
(
	[UserProjectId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [VersionNumber] INT NOT NULL DEFAULT 1,
    [ProjectName] NVARCHAR(256) DEFAULT '',
    [Comments] NVARCHAR(MAX) DEFAULT '',
    [IsLocked] BIT DEFAULT 0,
    [AssessmentYear] INT NOT NULL,
    [AssessmentDateFrom] DATETIME NOT NULL,
    [AssessmentDateTo] DATETIME NOT NULL,
    [SelectedAreas] NVARCHAR(MAX) DEFAULT '',
    [RootVersionUserProjectId] INT,
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    [ProjectTypeId] INT NOT NULL,
    [CreatedBy] UNIQUEIDENTIFIER NOT NULL, 
    [LastModifiedBy] UNIQUEIDENTIFIER NOT NULL,
    [CreatedTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastModifiedTimestamp] DATETIME NOT NULL DEFAULT GETDATE()

    CONSTRAINT [FK_UserProject_ToUserProject] FOREIGN KEY ([RootVersionUserProjectId]) REFERENCES [cus].[UserProject]([UserProjectId]),
    CONSTRAINT [FK_UserProject_ToProjectType] FOREIGN KEY ([ProjectTypeId]) REFERENCES [cus].[ProjectType]([ProjectTypeId]),

    CONSTRAINT [FK_UserProject_ToSystemUser_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
    CONSTRAINT [FK_UserProject_ToSystemUser_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [dynamics].[SystemUser]([systemuserid]),
    CONSTRAINT [FK_UserProject_ToSystemUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [dynamics].[SystemUser]([systemuserid])

)
GO

ALTER TABLE [cus].[UserProject] NOCHECK CONSTRAINT [FK_UserProject_ToSystemUser_CreatedBy]
GO

ALTER TABLE [cus].[UserProject] NOCHECK CONSTRAINT [FK_UserProject_ToSystemUser_LastModifiedBy]
GO

ALTER TABLE [cus].[UserProject] NOCHECK CONSTRAINT [FK_UserProject_ToSystemUser_UserId]
GO

CREATE INDEX [IX_UserProject_ProjectName] ON [cus].[UserProject] ([ProjectName])
GO


CREATE INDEX [IX_UserProject_VersionNumber] ON [cus].[UserProject] ([VersionNumber])
GO

CREATE INDEX [IX_UserProject_UserId] ON [cus].[UserProject] ([UserId])
GO

CREATE INDEX [IX_UserProject_RootVersionUserProjectId] ON [cus].[UserProject] ([RootVersionUserProjectId])
GO

CREATE INDEX [IX_UserProject_ProjectTypeId] ON [cus].[UserProject] ([ProjectTypeId])
GO

CREATE INDEX [IX_UserProject_CreatedTimestamp] ON [cus].[UserProject] ([CreatedTimestamp])
GO
