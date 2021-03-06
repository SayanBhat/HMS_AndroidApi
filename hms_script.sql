USE [master]
GO
/****** Object:  Database [HMS_DB]    Script Date: 1/7/2018 8:29:15 PM ******/
CREATE DATABASE [HMS_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HMS_DB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\HMS_DB.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HMS_DB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\HMS_DB_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [HMS_DB] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HMS_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HMS_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HMS_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HMS_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HMS_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HMS_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [HMS_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HMS_DB] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [HMS_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HMS_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HMS_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HMS_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HMS_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HMS_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HMS_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HMS_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HMS_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HMS_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HMS_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HMS_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HMS_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HMS_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HMS_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HMS_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HMS_DB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HMS_DB] SET  MULTI_USER 
GO
ALTER DATABASE [HMS_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HMS_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HMS_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HMS_DB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [HMS_DB]
GO
/****** Object:  StoredProcedure [dbo].[Result_get]    Script Date: 1/7/2018 8:29:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Result_get] 
	@pUserId int,
	@pCategoryName varchar(30)
	AS
BEGIN


	Select * From Result where CategoryId=(Select CategoryId From Category where CategoryName=@pCategoryName)AND UserID=@pUserId
    

END

GO
/****** Object:  StoredProcedure [dbo].[Result_Save]    Script Date: 1/7/2018 8:29:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Result_Save] 
	
			@pUserId  int 
           ,@pCategoryId int  
           ,@pResult varchar(30) 
           ,@pDate varchar(30) 
	
AS
BEGIN


INSERT INTO [dbo].[Result]
           ([UserId]
           ,[CategoryId]
           ,[Result]
           ,[Date])
     VALUES
           (@pUserId
           ,@pCategoryId
           ,@pResult
           ,@pDate)

End

--EXEC [dbo].[Result_save] '1','1','34','12/12/2017'

GO
/****** Object:  StoredProcedure [dbo].[User_Authenticate]    Script Date: 1/7/2018 8:29:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[User_Authenticate]	
		@pEmail nchar(50) 

AS
BEGIN

	SET NOCOUNT ON;

	SELECT * FROM [User] WHERE Email = @pEmail
	
END

--EXEC [dbo].[User_Authenticate] 'Test 1E'
GO
/****** Object:  StoredProcedure [dbo].[User_Save]    Script Date: 1/7/2018 8:29:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[User_Save]
		@pUserId int Output,
		@pName varchar(30) ,
		@pEmail nchar(50), 
		@pPassword nchar(50)=NULL

AS
BEGIN

	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM [User] WHERE UserId = @pUserId)
	BEGIN

    INSERT INTO [dbo].[User]
           ([Name]
           ,[Email]
           ,[Password]
		   )
     VALUES
           (
		@pName ,
		@pEmail , 
		@pPassword)

		   SET @pUserId = SCOPE_IDENTITY()
	END
	ELSE 
	BEGIN
	  UPDATE [dbo].[User] SET

			[Name] = @pName,
		  [Email] = @pEmail

		   WHERE UserId = @pUserId
	END
END
GO
/****** Object:  Table [dbo].[Category]    Script Date: 1/7/2018 8:29:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](30) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Result]    Script Date: 1/7/2018 8:29:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Result](
	[ResultId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Result] [varchar](30) NOT NULL,
	[Date] [varchar](30) NOT NULL,
 CONSTRAINT [PK_Result] PRIMARY KEY CLUSTERED 
(
	[ResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 1/7/2018 8:29:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Email] [nchar](50) NOT NULL,
	[Password] [nchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (1, N'BMI')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (2, N'Diabetes')
SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[Result] ON 

INSERT [dbo].[Result] ([ResultId], [UserId], [CategoryId], [Result], [Date]) VALUES (1, 1, 1, N'22', N'28/12/2017')
INSERT [dbo].[Result] ([ResultId], [UserId], [CategoryId], [Result], [Date]) VALUES (4, 2, 1, N'33', N'28/12/2017')
INSERT [dbo].[Result] ([ResultId], [UserId], [CategoryId], [Result], [Date]) VALUES (5, 1, 1, N'34', N'31/1/2018')
SET IDENTITY_INSERT [dbo].[Result] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserId], [Name], [Email], [Password]) VALUES (1, N'avi', N'avi@avi.com                                       ', N'avi                                               ')
INSERT [dbo].[User] ([UserId], [Name], [Email], [Password]) VALUES (2, N'sayan', N'sayan@sayan.com                                   ', N'sayan                                             ')
SET IDENTITY_INSERT [dbo].[User] OFF
USE [master]
GO
ALTER DATABASE [HMS_DB] SET  READ_WRITE 
GO
