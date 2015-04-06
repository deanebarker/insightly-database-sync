USE [Insightly]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 08/07/2014 17:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] NOT NULL,
	[Name] [varchar](max) NULL,
	[ContactId] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 08/07/2014 17:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[ObjectId] [nvarchar](max) NOT NULL,
	[Tag] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 08/07/2014 17:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Projects](
	[Id] [varchar](max) NOT NULL,
	[OrgId] [varchar](50) NULL,
	[Name] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Organizations]    Script Date: 08/07/2014 17:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Organizations](
	[Id] [varchar](max) NOT NULL,
	[ParentId] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[CityState] [varchar](max) NULL,
	[Url] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Opportunities]    Script Date: 08/07/2014 17:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Opportunities](
	[Id] [int] NOT NULL,
	[Name] [varchar](max) NULL,
	[State] [varchar](max) NULL,
	[Details] [varchar](max) NULL,
	[Created] [datetime] NULL,
	[Visibility] [varchar](max) NULL,
	[OwnerId] [int] NULL,
 CONSTRAINT [PK_Opportunities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Objects]    Script Date: 08/07/2014 17:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Objects](
	[Id] [nvarchar](max) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Reported] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 08/07/2014 17:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Contacts](
	[Id] [varchar](max) NOT NULL,
	[OrgId] [varchar](50) NULL,
	[Name] [varchar](max) NULL,
	[Email] [varchar](max) NULL,
	[Title] [varchar](max) NULL,
	[WorkPhone] [varchar](max) NULL,
	[MobilePhone] [varchar](max) NULL,
	[LinkedInUrl] [varchar](max) NULL,
	[Background] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 08/07/2014 17:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] NOT NULL,
	[Name] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_Objects_Created]    Script Date: 08/07/2014 17:46:21 ******/
ALTER TABLE [dbo].[Objects] ADD  CONSTRAINT [DF_Objects_Created]  DEFAULT (getdate()) FOR [Created]
GO
