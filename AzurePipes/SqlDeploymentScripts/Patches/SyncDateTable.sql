/****** Object:  Table [dbo].[entityDateSync_mb] ******/
CREATE TABLE [dbo].[entityDateSync_mb](
	[entityName] [nvarchar](1000) NOT NULL,
	[lastDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[entityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

/****** Object:  Trigger [Ptas_mediarepository_Data_QueueBlob] ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Ptas_mediarepository_Data_QueueBlob]'))
DROP TRIGGER [dbo].[Ptas_mediarepository_Data_QueueBlob]
GO