USE [master]
GO
/****** Object:  Database [database1]    Script Date: 12-Mar-21 9:40:57 PM ******/
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
/****** Object:  Table [dbo].[AdminDetail]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminDetail](
	[ID] [int] NOT NULL,
	[AdminID] [int] NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[ProfilePicture] [varchar](500) NULL,
	[SecondaryEmail] [varchar](30) NOT NULL,
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
/****** Object:  Table [dbo].[ContactUs]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactUs](
	[ID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[Countries]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[ID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[Downloads]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[ID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[NoteCategories]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteCategories](
	[ID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[NoteDetail]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteDetail](
	[ID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[NotesAttachments]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesAttachments](
	[ID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[NotesReview]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesReview](
	[ID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[NoteTypes]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteTypes](
	[ID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[ReferenceData]    Script Date: 12-Mar-21 9:40:58 PM ******/
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
/****** Object:  Table [dbo].[SpamReport]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpamReport](
	[ID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[Stats]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stats](
	[ID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[SystemConfiguration]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfiguration](
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
	[ModifiedBy] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfileDetail]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfileDetail](
	[ID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[UserRole]    Script Date: 12-Mar-21 9:40:58 PM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 12-Mar-21 9:40:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] NOT NULL,
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
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_AdminDetail_SecondaryEmail]    Script Date: 12-Mar-21 9:40:58 PM ******/
ALTER TABLE [dbo].[AdminDetail] ADD  CONSTRAINT [UQ_AdminDetail_SecondaryEmail] UNIQUE NONCLUSTERED 
(
	[SecondaryEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Users_EmailID]    Script Date: 12-Mar-21 9:40:58 PM ******/
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
