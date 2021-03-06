USE [master]
GO
/****** Object:  Database [database1]    Script Date: 13-May-21 10:27:29 PM ******/
CREATE DATABASE [database1]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'database1', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\database1.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'database1_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\database1_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [database1] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [database1].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [database1] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [database1] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [database1] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [database1] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [database1] SET ARITHABORT OFF 
GO
ALTER DATABASE [database1] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [database1] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [database1] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [database1] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [database1] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [database1] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [database1] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [database1] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [database1] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [database1] SET  DISABLE_BROKER 
GO
ALTER DATABASE [database1] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [database1] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [database1] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [database1] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [database1] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [database1] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [database1] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [database1] SET RECOVERY FULL 
GO
ALTER DATABASE [database1] SET  MULTI_USER 
GO
ALTER DATABASE [database1] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [database1] SET DB_CHAINING OFF 
GO
ALTER DATABASE [database1] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [database1] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [database1] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [database1] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'database1', N'ON'
GO
ALTER DATABASE [database1] SET QUERY_STORE = OFF
GO
USE [database1]
GO
/****** Object:  Table [dbo].[AdminDetail]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AdminID] [int] NOT NULL,
	[CountryCode] [varchar](5) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[ProfilePicture] [varchar](500) NULL,
	[SecondaryEmail] [varchar](30) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_AdminDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactUs]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactUs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](50) NOT NULL,
	[EmailID] [varchar](50) NOT NULL,
	[Subjects] [varchar](100) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ContactUs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CountryCode] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Downloads]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[Seller] [int] NOT NULL,
	[Downloader] [int] NOT NULL,
	[IsSellerHasAllowedDownload] [bit] NOT NULL,
	[AttachmentPath] [varchar](max) NULL,
	[IsAttachmentDownloaded] [bit] NOT NULL,
	[AttachmentDownloadedDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[NoteTitle] [varchar](100) NOT NULL,
	[NoteCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Downloads] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteCategories]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteCategories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteCategories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteDetail]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SellerID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ActionedBy] [int] NULL,
	[AdminRemarks] [varchar](max) NULL,
	[PublishedDate] [datetime] NULL,
	[Title] [varchar](100) NOT NULL,
	[Category] [int] NOT NULL,
	[DisplayPicture] [varchar](500) NULL,
	[NoteType] [int] NULL,
	[NumberofPages] [int] NULL,
	[Description] [varchar](max) NOT NULL,
	[UniversityName] [varchar](200) NULL,
	[Country] [int] NULL,
	[Course] [varchar](100) NULL,
	[CourseCode] [varchar](100) NULL,
	[Professor] [varchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[SellingPrice] [decimal](18, 0) NULL,
	[NotesPreview] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotesAttachments]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesAttachments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[FilePath] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NotesAttachments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotesReview]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesReview](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReviewedByID] [int] NOT NULL,
	[AgainstDownloadsID] [int] NOT NULL,
	[Ratings] [decimal](18, 0) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NotesReview] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteTypes]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReferenceData]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReferenceData](
	[ID] [int] NOT NULL,
	[Value] [varchar](100) NOT NULL,
	[Datavalue] [varchar](100) NOT NULL,
	[RefCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ReferenceData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SpamReport]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpamReport](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReportedByID] [int] NOT NULL,
	[AgainstDownloadsID] [int] NOT NULL,
	[Remarks] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SpamReport] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stats]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StatsID] [int] NOT NULL,
	[UnderReviewNotes] [int] NOT NULL,
	[PublishedNotes] [int] NOT NULL,
	[DownloadedNotes] [int] NOT NULL,
	[TotalExpenses] [int] NOT NULL,
	[TotalEarnings] [int] NOT NULL,
	[BuyerRequests] [int] NOT NULL,
	[SoldNotes] [int] NOT NULL,
	[RejectedNotes] [int] NOT NULL,
 CONSTRAINT [PK_Stats] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemConfiguration]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfiguration](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmailID1] [varchar](100) NOT NULL,
	[EmailID2] [varchar](100) NULL,
	[PhoneNumber] [varchar](15) NOT NULL,
	[DefaultProfilePicture] [varchar](max) NOT NULL,
	[DefaultNotePreview] [varchar](max) NOT NULL,
	[FacebookUrl] [varchar](50) NOT NULL,
	[TwitterUrl] [varchar](50) NOT NULL,
	[LinkedInUrl] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_SystemConfiguration] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfileDetail]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfileDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [int] NULL,
	[CountryCode] [varchar](5) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[ProfilePicture] [varchar](500) NULL,
	[AddressLine1] [varchar](100) NOT NULL,
	[AddressLine2] [varchar](100) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[ZipCode] [varchar](50) NOT NULL,
	[Country] [varchar](50) NOT NULL,
	[University] [varchar](100) NULL,
	[College] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_UserProfileDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[ID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 13-May-21 10:27:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailID] [varchar](100) NOT NULL,
	[Password] [varchar](24) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[SecretCode] [uniqueidentifier] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AdminDetail] ON 

INSERT [dbo].[AdminDetail] ([ID], [AdminID], [CountryCode], [PhoneNumber], [ProfilePicture], [SecondaryEmail], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (16, 11, N'+91', N'9265719561', NULL, N'parthpatell9265@gmail.com', NULL, NULL, CAST(N'2021-05-01T19:05:03.090' AS DateTime), 11, 1)
INSERT [dbo].[AdminDetail] ([ID], [AdminID], [CountryCode], [PhoneNumber], [ProfilePicture], [SecondaryEmail], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (17, 13, N'+91', N'6354531487', NULL, N'pragati@gmail.com', CAST(N'2021-05-04T16:44:55.020' AS DateTime), 11, CAST(N'2021-05-04T18:53:17.407' AS DateTime), 13, NULL)
INSERT [dbo].[AdminDetail] ([ID], [AdminID], [CountryCode], [PhoneNumber], [ProfilePicture], [SecondaryEmail], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (19, 15, N'+61', N'44441111', N'~/Members/15/DP_05052021_100854.jpg', NULL, CAST(N'2021-05-04T17:26:36.910' AS DateTime), 11, CAST(N'2021-05-05T22:08:55.010' AS DateTime), 15, NULL)
SET IDENTITY_INSERT [dbo].[AdminDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[ContactUs] ON 

INSERT [dbo].[ContactUs] ([ID], [FullName], [EmailID], [Subjects], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Parthh Patell', N'patel.parth.9@ldce.ac.in', N'Notes', N'published notes rating.', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[ContactUs] ([ID], [FullName], [EmailID], [Subjects], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'Dhiral Patel ', N'dhiralpatel@gmail.com', N'Primary Email', N'can I change my primary email?', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[ContactUs] ([ID], [FullName], [EmailID], [Subjects], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, N'Dhiral Patel ', N'dhiralpatel@gmail.com', N'Notes', N'it''s a very useful web app for students 
', NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[ContactUs] OFF
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'India', N'+91', CAST(N'2021-04-25T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Australia', N'+61', CAST(N'2021-04-24T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'New Zealand', N'+64', CAST(N'2021-05-01T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Canada', N'+21', CAST(N'2021-05-03T22:00:15.043' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'UK', N'+21', CAST(N'2021-05-03T22:00:38.677' AS DateTime), 11, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Downloads] ON 

INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, 6, 10, 3, 1, N'~/Members/10/6/Attachements/', 1, CAST(N'2021-04-16T13:35:40.577' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'MCWC', N'IT', CAST(N'2021-04-16T13:35:40.580' AS DateTime), 3, NULL, NULL, 0)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (26, 14, 10, 3, 1, N'~/Members/10/14/Attachements/', 1, CAST(N'2021-04-21T18:56:39.890' AS DateTime), 1, CAST(23 AS Decimal(18, 0)), N'Big Data Analytics', N'IT', CAST(N'2021-04-21T18:55:44.763' AS DateTime), 3, CAST(N'2021-04-21T18:56:39.890' AS DateTime), 3, 0)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (32, 5, 10, 3, 1, N'~/Members/10/5/Attachements/', 1, CAST(N'2021-04-26T22:44:40.170' AS DateTime), 1, CAST(13 AS Decimal(18, 0)), N'Abraham Lincoln', N'History', CAST(N'2021-04-26T16:38:43.643' AS DateTime), 3, CAST(N'2021-04-26T22:44:40.170' AS DateTime), 3, 0)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (33, 34, 10, 3, 1, N'~/Members/10/34/Attachements/', 1, CAST(N'2021-05-07T18:10:47.520' AS DateTime), 1, CAST(35 AS Decimal(18, 0)), N'Data Science', N'IT', CAST(N'2021-05-07T18:06:57.350' AS DateTime), 3, CAST(N'2021-05-07T18:10:47.520' AS DateTime), 3, 0)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (34, 33, 10, 3, 0, NULL, 0, NULL, 1, CAST(16 AS Decimal(18, 0)), N'Statatics', N'Commerce', CAST(N'2021-05-07T18:07:17.360' AS DateTime), 3, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Downloads] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteCategories] ON 

INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Science', N'science category books', CAST(N'2021-05-03T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'History', N'history books', CAST(N'2021-05-03T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, N'IT', N'CS/IT books', CAST(N'2021-05-03T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, N'Mechanical Engineering', N'ME Eng. books', CAST(N'2021-05-03T00:00:00.000' AS DateTime), 11, CAST(N'2021-05-03T21:51:58.467' AS DateTime), 11, 0)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, N'Commerce', N'account book', CAST(N'2021-05-03T21:46:21.280' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (11, N'Mechanical Engineering', N'ME Eng. books', CAST(N'2021-05-03T21:53:20.167' AS DateTime), 11, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[NoteCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteDetail] ON 

INSERT [dbo].[NoteDetail] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, 10, 11, 11, N'some details are wrong', CAST(N'2021-04-26T00:00:00.000' AS DateTime), N'Machine ', 4, NULL, 1, NULL, N'mechanical', NULL, 3, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2021-04-07T22:31:20.223' AS DateTime), 10, CAST(N'2021-05-07T11:39:36.487' AS DateTime), 11, 1)
INSERT [dbo].[NoteDetail] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, 10, 11, 11, NULL, CAST(N'2021-04-01T00:00:00.000' AS DateTime), N'Science', 4, NULL, 1, NULL, N'science', NULL, 3, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2021-04-07T22:42:06.227' AS DateTime), 10, CAST(N'2021-05-05T11:57:14.157' AS DateTime), 11, 1)
INSERT [dbo].[NoteDetail] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, 10, 9, 11, N'some pages are not proper are missing.', CAST(N'2021-05-07T11:59:09.053' AS DateTime), N'Abraham Lincoln', 5, N'~/Members/10/5/image7.jpg', 5, 15, N'history book ', N'university ', 1, N'BA', N'89', N'Stephen', 1, CAST(13 AS Decimal(18, 0)), N'~/Members/10/5/170280116075_BDA_Practical8.pdf', CAST(N'2021-04-07T22:45:41.140' AS DateTime), 10, CAST(N'2021-05-07T11:59:09.053' AS DateTime), 11, 1)
INSERT [dbo].[NoteDetail] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, 10, 9, 11, N'nice book', CAST(N'2021-04-10T00:00:00.000' AS DateTime), N'MCWC', 8, N'~/Members/10/6/image3.jpg', 3, 2, N'mobile computing and wireless communication', N'LDCE', 2, N'IT', N'007', N'Morris', 0, CAST(0 AS Decimal(18, 0)), N'~/Members/10/6/170280116075_MCWC_Practical1.pdf', CAST(N'2021-04-07T22:53:12.293' AS DateTime), 10, CAST(N'2021-04-18T21:49:45.037' AS DateTime), 10, 1)
INSERT [dbo].[NoteDetail] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (14, 10, 9, 11, N'good', CAST(N'2021-04-10T00:00:00.000' AS DateTime), N'Big Data Analytics', 8, N'~/Members/10/14/bda.jpg', 2, 4, N'big data analytics', N'LDCE', 1, N'IT', N'007', N'Frank', 1, CAST(23 AS Decimal(18, 0)), N'~/Members/10/14/170280116075_BDA_Practical7.pdf', CAST(N'2021-04-17T22:05:18.913' AS DateTime), 10, CAST(N'2021-04-17T22:07:39.337' AS DateTime), 10, 1)
INSERT [dbo].[NoteDetail] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (26, 3, 8, NULL, NULL, NULL, N'Chemitry', 4, NULL, 1, 15, N'chemistry handwritten notebook ', N'school of science', 1, N'Higher Secondary', N'31', N'Morris', 1, CAST(21 AS Decimal(18, 0)), N'~/Members/3/26/170280116075_INS_Practical1.pdf', CAST(N'2021-04-26T16:44:47.473' AS DateTime), 3, CAST(N'2021-05-10T10:51:15.220' AS DateTime), 11, 1)
INSERT [dbo].[NoteDetail] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (31, 10, 9, 13, NULL, CAST(N'2021-05-07T17:59:07.967' AS DateTime), N'Machine Learning', 8, N'~/Members/10/31/image10.jpg', 2, 16, N'This Book is useful for the students who are interested in machine learning.', N'Toronto University', 4, N'Information Technology', N'007', N'Morris', 1, CAST(30 AS Decimal(18, 0)), N'~/Members/10/31/ex6.pdf', CAST(N'2021-05-07T16:54:33.057' AS DateTime), 10, CAST(N'2021-05-07T17:59:07.967' AS DateTime), 13, 1)
INSERT [dbo].[NoteDetail] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (32, 10, 10, 13, N'some pages are not visible.', NULL, N'Accountancy', 10, N'~/Members/10/32/imag12.jpg', 7, 10, N'commerce field related book ', N'School of Commerce', 3, N'higher secondary', N'102', N'S.K.Singh', 1, CAST(10 AS Decimal(18, 0)), N'~/Members/10/32/170280116075Case Study.pdf', CAST(N'2021-05-07T17:15:36.830' AS DateTime), 10, CAST(N'2021-05-07T17:58:20.840' AS DateTime), 13, 1)
INSERT [dbo].[NoteDetail] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (33, 10, 9, 13, NULL, CAST(N'2021-05-07T17:59:24.537' AS DateTime), N'Statatics', 10, N'~/Members/10/33/stats.jpg', 3, 20, N'statatics selfwrite noteboook.', N'School of Commerce', 1, N'B.Com', N'202', N'S.M Shukla', 1, CAST(16 AS Decimal(18, 0)), N'~/Members/10/33/170280116075Case Study.pdf', CAST(N'2021-05-07T17:19:25.323' AS DateTime), 10, CAST(N'2021-05-07T17:59:24.537' AS DateTime), 13, 1)
INSERT [dbo].[NoteDetail] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (34, 10, 9, 13, NULL, CAST(N'2021-05-07T17:58:45.980' AS DateTime), N'Data Science', 8, N'~/Members/10/34/datas.jpg', 7, 20, N'Data Science Printed Book.', N'Adelaide University', 2, N'Computer Science', N'101', N'John And Brendon', 1, CAST(35 AS Decimal(18, 0)), N'~/Members/10/34/ex5.pdf', CAST(N'2021-05-07T17:23:51.750' AS DateTime), 10, CAST(N'2021-05-07T17:58:45.980' AS DateTime), 13, 1)
INSERT [dbo].[NoteDetail] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (35, 10, 6, NULL, NULL, NULL, N'ML part 2', 8, NULL, 2, 25, N'machine learning part 2 ', N'Toronto University', 6, N'IT', N'007', N'Rudolph Rusell', 1, NULL, N'~/Members/10/35/ex2.pdf', CAST(N'2021-05-07T17:41:59.960' AS DateTime), 10, CAST(N'2021-05-08T14:38:28.733' AS DateTime), 10, 1)
SET IDENTITY_INSERT [dbo].[NoteDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[NotesAttachments] ON 

INSERT [dbo].[NotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, 3, N'indexjava.pdf', N'~/Members/10/3/Attachements/', CAST(N'2021-04-07T22:39:48.237' AS DateTime), 10, NULL, NULL, 1)
INSERT [dbo].[NotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (16, 4, N'indexjava.pdf', N'~/Members/10/4/Attachements/', CAST(N'2021-04-07T23:36:36.393' AS DateTime), 10, NULL, NULL, 1)
INSERT [dbo].[NotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (24, 14, N'170280116075_BDA_Practical7.pdf', N'~/Members/10/14/Attachements/', CAST(N'2021-04-17T22:06:11.560' AS DateTime), 10, NULL, NULL, 1)
INSERT [dbo].[NotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (27, 6, N'170280116075_MCWC_Practical1.pdf', N'~/Members/10/6/Attachements/', CAST(N'2021-04-18T21:49:34.047' AS DateTime), 10, NULL, NULL, 1)
INSERT [dbo].[NotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (28, 5, N'170280116075_case study.pdf', N'~/Members/10/5/Attachements/', CAST(N'2021-04-23T00:42:36.323' AS DateTime), 10, NULL, NULL, 1)
INSERT [dbo].[NotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (42, 31, N'ex6.pdf', N'~/Members/10/31/Attachements/', CAST(N'2021-05-07T16:54:33.930' AS DateTime), 10, NULL, NULL, 1)
INSERT [dbo].[NotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (43, 32, N'170280116075Case Study.pdf', N'~/Members/10/32/Attachements/', CAST(N'2021-05-07T17:15:37.940' AS DateTime), 10, NULL, NULL, 1)
INSERT [dbo].[NotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (44, 33, N'170280116075Case Study.pdf', N'~/Members/10/33/Attachements/', CAST(N'2021-05-07T17:19:25.440' AS DateTime), 10, NULL, NULL, 1)
INSERT [dbo].[NotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (45, 34, N'ex5.pdf', N'~/Members/10/34/Attachements/', CAST(N'2021-05-07T17:23:51.847' AS DateTime), 10, NULL, NULL, 1)
INSERT [dbo].[NotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (46, 35, N'ex2.pdf', N'~/Members/10/35/Attachements/', CAST(N'2021-05-07T17:42:00.140' AS DateTime), 10, NULL, NULL, 1)
INSERT [dbo].[NotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (47, 26, N'170280116075_INS_Practical1.pdf', N'~/Members/3/26/Attachements/', CAST(N'2021-05-07T18:17:37.993' AS DateTime), 3, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[NotesAttachments] OFF
GO
SET IDENTITY_INSERT [dbo].[NotesReview] ON 

INSERT [dbo].[NotesReview] ([ID], [NoteID], [ReviewedByID], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, 14, 3, 26, CAST(3 AS Decimal(18, 0)), N'good book', CAST(N'2021-04-22T19:50:33.760' AS DateTime), 3, CAST(N'2021-04-22T21:44:46.500' AS DateTime), 3, 1)
INSERT [dbo].[NotesReview] ([ID], [NoteID], [ReviewedByID], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, 5, 3, 32, CAST(4 AS Decimal(18, 0)), N'abcdef', CAST(N'2021-04-26T22:46:09.110' AS DateTime), 3, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[NotesReview] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteTypes] ON 

INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Handwriting', N'handwritten books ', CAST(N'2021-05-01T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'University Notes', N'notes', CAST(N'2021-05-02T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Self Write', N'self written', CAST(N'2021-04-25T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Story', N'story type', CAST(N'2021-04-27T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'Novel', N'novel books', CAST(N'2021-04-25T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, N'Printed', N'print of book', CAST(N'2021-05-03T21:56:31.233' AS DateTime), 11, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[NoteTypes] OFF
GO
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Male 
', N'M', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Female', N'Fe', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Unknown', N'U', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Paid', N'P', N'Selling Mode
', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Free', N'F', N'Selling Mode
', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'Draft

', N'Draft
', N'Notes Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, N'Submitted', N'Submitted', N'Notes Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, N'In Review', N'In Review', N'Notes Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, N'Published 
', N'Published', N'Notes Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, N'Rejected
', N'Rejected', N'Notes Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (11, N'Removed', N'Removed', N'Notes Status', NULL, NULL, NULL, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[SpamReport] ON 

INSERT [dbo].[SpamReport] ([ID], [NoteID], [ReportedByID], [AgainstDownloadsID], [Remarks], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, 6, 3, 1, N'spam record', CAST(N'2021-04-22T22:18:54.997' AS DateTime), 3, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[SpamReport] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemConfiguration] ON 

INSERT [dbo].[SystemConfiguration] ([ID], [EmailID1], [EmailID2], [PhoneNumber], [DefaultProfilePicture], [DefaultNotePreview], [FacebookUrl], [TwitterUrl], [LinkedInUrl], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (22, N'parthpatel9265@gmail.com', N'patel.parth.9@ldce.ac.in', N'9265719561', N'~/Content/Default/defaultuser.jpg', N'~/Content/Default/defaultbook.jpg', N'https://www.facebook.com/', N'https://www.facebook.com/', N'https://www.facebook.com/', CAST(N'2021-05-08T11:03:48.757' AS DateTime), 11, NULL, NULL)
SET IDENTITY_INSERT [dbo].[SystemConfiguration] OFF
GO
SET IDENTITY_INSERT [dbo].[UserProfileDetail] ON 

INSERT [dbo].[UserProfileDetail] ([ID], [UserID], [DOB], [Gender], [CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, 10, CAST(N'1999-12-29T00:00:00.000' AS DateTime), 1, N'+91', N'9265719561', N'~/Members/10/DP_17042021_053524.PNG', N'umiynagar', N'vijapur', N'vijapur', N'gujarat', N'382870', N'India', N'GTU', N'LDCE', CAST(N'2021-04-17T17:35:24.233' AS DateTime), 10, NULL, NULL)
INSERT [dbo].[UserProfileDetail] ([ID], [UserID], [DOB], [Gender], [CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, 3, CAST(N'1999-12-29T00:00:00.000' AS DateTime), 1, N'+91', N'6354531487', NULL, N'umiynagar', N'vijapur', N'vijapur', N'gujarat', N'382870', N'India', N'GTU', N'LDCE', CAST(N'2021-04-18T02:43:38.510' AS DateTime), 3, NULL, NULL)
SET IDENTITY_INSERT [dbo].[UserProfileDetail] OFF
GO
INSERT [dbo].[UserRole] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'SuperAdmin', N'for super admin use only', CAST(N'2021-05-01T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[UserRole] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Admin', N'for admin use only', CAST(N'2021-05-01T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
INSERT [dbo].[UserRole] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Member', N'for members use only ', CAST(N'2021-05-01T00:00:00.000' AS DateTime), 11, NULL, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [SecretCode], [IsActive]) VALUES (3, 3, N'Parth', N'Patel', N'parthpatel9265@gmail.com', N'Par@2912', 1, CAST(N'2021-04-10T00:00:00.000' AS DateTime), 3, CAST(N'2021-05-05T12:00:14.487' AS DateTime), 11, N'e8360afb-eb04-42d7-a106-fa345e466d86', 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [SecretCode], [IsActive]) VALUES (10, 3, N'Parthh', N'Patell', N'patel.parth.9@ldce.ac.in', N'Pa@2912', 1, CAST(N'2021-04-11T00:00:00.000' AS DateTime), 10, CAST(N'2021-04-03T12:24:45.110' AS DateTime), NULL, N'ca996f5e-cdef-4057-b5d0-c7db106bc4b4', 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [SecretCode], [IsActive]) VALUES (11, 1, N'Montu', N'patelllll', N'parthpatelllll9265@gmail.com', N'Mo@2912', 1, CAST(N'2021-05-01T09:56:19.350' AS DateTime), 11, NULL, NULL, N'cbafa789-277d-4285-a6fe-d66195282db4', 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [SecretCode], [IsActive]) VALUES (13, 2, N'Pragati', N'Patel', N'pragatipatel3666@gmail.com', N'Admin@123', 1, CAST(N'2021-05-04T16:44:54.200' AS DateTime), 11, CAST(N'2021-05-04T16:52:21.787' AS DateTime), 11, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [SecretCode], [IsActive]) VALUES (15, 2, N'Dhiral', N'Patel', N'dhiralpatel@gmail.com', N'Admin@123', 1, CAST(N'2021-05-04T17:26:36.703' AS DateTime), 11, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Users_EmailID]    Script Date: 13-May-21 10:27:30 PM ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UQ_Users_EmailID] UNIQUE NONCLUSTERED 
(
	[EmailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AdminDetail] ADD  CONSTRAINT [DF_AdminDetail_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ContactUs] ADD  CONSTRAINT [DF_ContactUs_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Countries] ADD  CONSTRAINT [DF_Countries_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Downloads] ADD  CONSTRAINT [DF_Downloads_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NoteCategories] ADD  CONSTRAINT [DF_NoteCategories_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NoteDetail] ADD  CONSTRAINT [DF_NoteDetail_IsPaid]  DEFAULT ((0)) FOR [IsPaid]
GO
ALTER TABLE [dbo].[NoteDetail] ADD  CONSTRAINT [DF_NoteDetail_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NotesAttachments] ADD  CONSTRAINT [DF_NotesAttachments_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NotesReview] ADD  CONSTRAINT [DF_NotesReview_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NoteTypes] ADD  CONSTRAINT [DF_NoteTypes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ReferenceData] ADD  CONSTRAINT [DF_ReferenceData_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SpamReport] ADD  CONSTRAINT [DF_SpamReport_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Stats] ADD  CONSTRAINT [DF_Stats_UnderReviewNotes]  DEFAULT ((0)) FOR [UnderReviewNotes]
GO
ALTER TABLE [dbo].[Stats] ADD  CONSTRAINT [DF_Stats_PublishedNotes]  DEFAULT ((0)) FOR [PublishedNotes]
GO
ALTER TABLE [dbo].[Stats] ADD  CONSTRAINT [DF_Stats_DownloadedNotes]  DEFAULT ((0)) FOR [DownloadedNotes]
GO
ALTER TABLE [dbo].[Stats] ADD  CONSTRAINT [DF_Stats_TotalExpenses]  DEFAULT ((0)) FOR [TotalExpenses]
GO
ALTER TABLE [dbo].[Stats] ADD  CONSTRAINT [DF_Stats_TotalEarnings]  DEFAULT ((0)) FOR [TotalEarnings]
GO
ALTER TABLE [dbo].[Stats] ADD  CONSTRAINT [DF_Stats_BuyerRequests]  DEFAULT ((0)) FOR [BuyerRequests]
GO
ALTER TABLE [dbo].[Stats] ADD  CONSTRAINT [DF_Stats_SoldNotes]  DEFAULT ((0)) FOR [SoldNotes]
GO
ALTER TABLE [dbo].[Stats] ADD  CONSTRAINT [DF_Stats_RejectedNotes]  DEFAULT ((0)) FOR [RejectedNotes]
GO
ALTER TABLE [dbo].[UserRole] ADD  CONSTRAINT [DF_UserRole_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsEmailVerified]  DEFAULT ((0)) FOR [IsEmailVerified]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AdminDetail]  WITH CHECK ADD  CONSTRAINT [FK_AdminDetail_Users] FOREIGN KEY([AdminID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[AdminDetail] CHECK CONSTRAINT [FK_AdminDetail_Users]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_NoteDetail] FOREIGN KEY([Downloader])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_NoteDetail]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users] FOREIGN KEY([Seller])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users1] FOREIGN KEY([Downloader])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users1]
GO
ALTER TABLE [dbo].[NoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_NoteDetail_Countries] FOREIGN KEY([Country])
REFERENCES [dbo].[Countries] ([ID])
GO
ALTER TABLE [dbo].[NoteDetail] CHECK CONSTRAINT [FK_NoteDetail_Countries]
GO
ALTER TABLE [dbo].[NoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_NoteDetail_NoteCategories] FOREIGN KEY([Category])
REFERENCES [dbo].[NoteCategories] ([ID])
GO
ALTER TABLE [dbo].[NoteDetail] CHECK CONSTRAINT [FK_NoteDetail_NoteCategories]
GO
ALTER TABLE [dbo].[NoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_NoteDetail_NoteTypes] FOREIGN KEY([NoteType])
REFERENCES [dbo].[NoteTypes] ([ID])
GO
ALTER TABLE [dbo].[NoteDetail] CHECK CONSTRAINT [FK_NoteDetail_NoteTypes]
GO
ALTER TABLE [dbo].[NoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_NoteDetail_ReferenceData] FOREIGN KEY([Status])
REFERENCES [dbo].[ReferenceData] ([ID])
GO
ALTER TABLE [dbo].[NoteDetail] CHECK CONSTRAINT [FK_NoteDetail_ReferenceData]
GO
ALTER TABLE [dbo].[NoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_NoteDetail_Users] FOREIGN KEY([SellerID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NoteDetail] CHECK CONSTRAINT [FK_NoteDetail_Users]
GO
ALTER TABLE [dbo].[NoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_NoteDetail_Users1] FOREIGN KEY([ActionedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NoteDetail] CHECK CONSTRAINT [FK_NoteDetail_Users1]
GO
ALTER TABLE [dbo].[NotesAttachments]  WITH CHECK ADD  CONSTRAINT [FK_NotesAttachments_NoteDetail] FOREIGN KEY([NoteID])
REFERENCES [dbo].[NoteDetail] ([ID])
GO
ALTER TABLE [dbo].[NotesAttachments] CHECK CONSTRAINT [FK_NotesAttachments_NoteDetail]
GO
ALTER TABLE [dbo].[NotesReview]  WITH CHECK ADD  CONSTRAINT [FK_NotesReview_Downloads] FOREIGN KEY([AgainstDownloadsID])
REFERENCES [dbo].[Downloads] ([ID])
GO
ALTER TABLE [dbo].[NotesReview] CHECK CONSTRAINT [FK_NotesReview_Downloads]
GO
ALTER TABLE [dbo].[NotesReview]  WITH CHECK ADD  CONSTRAINT [FK_NotesReview_NoteDetail] FOREIGN KEY([NoteID])
REFERENCES [dbo].[NoteDetail] ([ID])
GO
ALTER TABLE [dbo].[NotesReview] CHECK CONSTRAINT [FK_NotesReview_NoteDetail]
GO
ALTER TABLE [dbo].[NotesReview]  WITH CHECK ADD  CONSTRAINT [FK_NotesReview_Users] FOREIGN KEY([ReviewedByID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NotesReview] CHECK CONSTRAINT [FK_NotesReview_Users]
GO
ALTER TABLE [dbo].[SpamReport]  WITH CHECK ADD  CONSTRAINT [FK_SpamReport_Downloads] FOREIGN KEY([AgainstDownloadsID])
REFERENCES [dbo].[Downloads] ([ID])
GO
ALTER TABLE [dbo].[SpamReport] CHECK CONSTRAINT [FK_SpamReport_Downloads]
GO
ALTER TABLE [dbo].[SpamReport]  WITH CHECK ADD  CONSTRAINT [FK_SpamReport_NoteDetail] FOREIGN KEY([NoteID])
REFERENCES [dbo].[NoteDetail] ([ID])
GO
ALTER TABLE [dbo].[SpamReport] CHECK CONSTRAINT [FK_SpamReport_NoteDetail]
GO
ALTER TABLE [dbo].[SpamReport]  WITH CHECK ADD  CONSTRAINT [FK_SpamReport_Users] FOREIGN KEY([ReportedByID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SpamReport] CHECK CONSTRAINT [FK_SpamReport_Users]
GO
ALTER TABLE [dbo].[Stats]  WITH CHECK ADD  CONSTRAINT [FK_Stats_Users] FOREIGN KEY([StatsID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Stats] CHECK CONSTRAINT [FK_Stats_Users]
GO
ALTER TABLE [dbo].[UserProfileDetail]  WITH CHECK ADD  CONSTRAINT [FK_UserProfileDetail_ReferenceData] FOREIGN KEY([Gender])
REFERENCES [dbo].[ReferenceData] ([ID])
GO
ALTER TABLE [dbo].[UserProfileDetail] CHECK CONSTRAINT [FK_UserProfileDetail_ReferenceData]
GO
ALTER TABLE [dbo].[UserProfileDetail]  WITH CHECK ADD  CONSTRAINT [FK_UserProfileDetail_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserProfileDetail] CHECK CONSTRAINT [FK_UserProfileDetail_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserRole] FOREIGN KEY([RoleID])
REFERENCES [dbo].[UserRole] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserRole]
GO
USE [master]
GO
ALTER DATABASE [database1] SET  READ_WRITE 
GO
