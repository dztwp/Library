USE [master]
GO
/****** Object:  Database [LibraryDB]    Script Date: 10/24/2021 11:56:24 AM ******/
CREATE DATABASE [LibraryDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LibraryDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\LibraryDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LibraryDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\LibraryDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [LibraryDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LibraryDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LibraryDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LibraryDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LibraryDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LibraryDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LibraryDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [LibraryDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LibraryDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LibraryDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LibraryDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LibraryDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LibraryDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LibraryDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LibraryDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LibraryDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LibraryDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LibraryDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LibraryDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LibraryDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LibraryDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LibraryDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LibraryDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LibraryDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LibraryDB] SET RECOVERY FULL 
GO
ALTER DATABASE [LibraryDB] SET  MULTI_USER 
GO
ALTER DATABASE [LibraryDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LibraryDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LibraryDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LibraryDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LibraryDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LibraryDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LibraryDB', N'ON'
GO
ALTER DATABASE [LibraryDB] SET QUERY_STORE = OFF
GO
USE [LibraryDB]
GO
/****** Object:  User [User123]    Script Date: 10/24/2021 11:56:25 AM ******/
CREATE USER [User123] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [reader]    Script Date: 10/24/2021 11:56:25 AM ******/
CREATE USER [reader] FOR LOGIN [reader] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [library1]    Script Date: 10/24/2021 11:56:25 AM ******/
CREATE USER [library1] FOR LOGIN [library1] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [admin]    Script Date: 10/24/2021 11:56:25 AM ******/
CREATE USER [admin] FOR LOGIN [admin] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  DatabaseRole [readerRole]    Script Date: 10/24/2021 11:56:25 AM ******/
CREATE ROLE [readerRole]
GO
/****** Object:  DatabaseRole [librarianRole]    Script Date: 10/24/2021 11:56:25 AM ******/
CREATE ROLE [librarianRole]
GO
/****** Object:  DatabaseRole [adminRole]    Script Date: 10/24/2021 11:56:25 AM ******/
CREATE ROLE [adminRole]
GO
ALTER ROLE [readerRole] ADD MEMBER [reader]
GO
ALTER ROLE [librarianRole] ADD MEMBER [library1]
GO
ALTER ROLE [adminRole] ADD MEMBER [admin]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Author] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookAuthors]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookAuthors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL,
 CONSTRAINT [IX_BookAuthors] UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookData]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookData](
	[Id] [int] NOT NULL,
	[CityName] [nvarchar](50) NOT NULL,
	[PublishingHouse] [nvarchar](300) NOT NULL,
	[NumberOfPages] [int] NOT NULL,
	[ISBN] [nvarchar](20) NOT NULL,
 CONSTRAINT [IX_BookData] UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Issue]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Issue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NumberOfIssue] [int] NOT NULL,
	[ReleaseDay] [int] NOT NULL,
	[NumberOfPages] [int] NOT NULL,
	[NewspaperId] [int] NULL,
 CONSTRAINT [PK_Issue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateOfOperation] [datetime2](7) NOT NULL,
	[ObjectId] [int] NOT NULL,
	[Description] [nvarchar](2000) NOT NULL,
	[NameOfUser] [nvarchar](50) NOT NULL,
	[ObjectType] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NewspaperData]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewspaperData](
	[Id] [int] NOT NULL,
	[CityName] [nvarchar](50) NOT NULL,
	[PublishingHouse] [nvarchar](300) NOT NULL,
	[ISSN] [nvarchar](20) NULL,
 CONSTRAINT [IX_NewspaperData] UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaperObj]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaperObj](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](300) NOT NULL,
	[Note] [nvarchar](2000) NOT NULL,
	[YearOfPublishing] [int] NOT NULL,
	[MarkedAsDelete] [bit] NOT NULL,
	[TypeOfObj] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_PaperObj] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatentAuthors]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatentAuthors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatentId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL,
 CONSTRAINT [IX_PatentAuthors] UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatentData]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatentData](
	[Id] [int] NOT NULL,
	[Country] [nvarchar](200) NOT NULL,
	[RegistrationNumber] [nvarchar](10) NOT NULL,
	[ApplicationDate] [int] NOT NULL,
	[NumberOfPages] [int] NOT NULL,
 CONSTRAINT [IX_PatentData] UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PaperObj] ADD  CONSTRAINT [DF_PaperObj_MarkedAsDelete]  DEFAULT ((0)) FOR [MarkedAsDelete]
GO
ALTER TABLE [dbo].[BookAuthors]  WITH CHECK ADD  CONSTRAINT [FK_BookAuthor_Author] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([Id])
GO
ALTER TABLE [dbo].[BookAuthors] CHECK CONSTRAINT [FK_BookAuthor_Author]
GO
ALTER TABLE [dbo].[BookAuthors]  WITH CHECK ADD  CONSTRAINT [FK_BookAuthors_BookData] FOREIGN KEY([BookId])
REFERENCES [dbo].[BookData] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BookAuthors] CHECK CONSTRAINT [FK_BookAuthors_BookData]
GO
ALTER TABLE [dbo].[BookData]  WITH CHECK ADD  CONSTRAINT [FK_BookData_PaperObj] FOREIGN KEY([Id])
REFERENCES [dbo].[PaperObj] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BookData] CHECK CONSTRAINT [FK_BookData_PaperObj]
GO
ALTER TABLE [dbo].[Issue]  WITH CHECK ADD  CONSTRAINT [FK_Issue_NewspaperData] FOREIGN KEY([NewspaperId])
REFERENCES [dbo].[NewspaperData] ([Id])
GO
ALTER TABLE [dbo].[Issue] CHECK CONSTRAINT [FK_Issue_NewspaperData]
GO
ALTER TABLE [dbo].[NewspaperData]  WITH CHECK ADD  CONSTRAINT [FK_NewspaperData_PaperObj] FOREIGN KEY([Id])
REFERENCES [dbo].[PaperObj] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewspaperData] CHECK CONSTRAINT [FK_NewspaperData_PaperObj]
GO
ALTER TABLE [dbo].[PatentAuthors]  WITH CHECK ADD  CONSTRAINT [FK_PatentAuthor_Author] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([Id])
GO
ALTER TABLE [dbo].[PatentAuthors] CHECK CONSTRAINT [FK_PatentAuthor_Author]
GO
ALTER TABLE [dbo].[PatentAuthors]  WITH CHECK ADD  CONSTRAINT [FK_PatentAuthors_PatentData] FOREIGN KEY([PatentId])
REFERENCES [dbo].[PatentData] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatentAuthors] CHECK CONSTRAINT [FK_PatentAuthors_PatentData]
GO
ALTER TABLE [dbo].[PatentData]  WITH CHECK ADD  CONSTRAINT [FK_PatentData_PaperObj] FOREIGN KEY([Id])
REFERENCES [dbo].[PaperObj] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatentData] CHECK CONSTRAINT [FK_PatentData_PaperObj]
GO
/****** Object:  StoredProcedure [dbo].[Authors_Adding]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Authors_Adding]
	@id INT OUTPUT,
	@firstName nvarchar(300),
	@lastName nvarchar(300)
AS
BEGIN
    INSERT INTO Authors (FirstName,LastName)
	VALUES (@firstName, @lastName)

	SET @id=SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[Authors_Deleting]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Authors_Deleting]
	@id int
AS
BEGIN
	SET NOCOUNT ON;

    DELETE FROM Authors 
	WHERE Id = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[Authors_GetAuthorById]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Authors_GetAuthorById]
	@id int
AS
BEGIN

    SELECT Id, FirstName, LastName 
	FROM Authors WHERE Id=@id;
END
GO
/****** Object:  StoredProcedure [dbo].[Authors_GetIdByName]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	
CREATE PROCEDURE [dbo].[Authors_GetIdByName]
	-- Add the parameters for the stored procedure here
	@firstName nvarchar(300),
	@lastName nvarchar(300)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id FROM Authors
	WHERE FirstName=@firstName 
	AND LastName=@lastName;
END
GO
/****** Object:  StoredProcedure [dbo].[Authors_IsPersonExistInDB]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Authors_IsPersonExistInDB]
	@firstName nvarchar(300),
	@lastName nvarchar(300)
AS
BEGIN
    -- Insert statements for procedure here
	SELECT COUNT(*) FROM Authors
	WHERE FirstName=@firstName 
	AND LastName = @lastName;
END
GO
/****** Object:  StoredProcedure [dbo].[Book_Adding]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_Adding]
	@id INT OUTPUT,
	@name nvarchar(300),
	@note nvarchar(2000),
	@yearOfPubl int,
	@cityName nvarchar(50),
	@publishingHouse nvarchar(300),
	@numberOfPages int,
	@isbn nvarchar(20)

AS
BEGIN
	BEGIN TRAN
		INSERT INTO PaperObj ([Name],Note,YearOfPublishing, TypeOfObj)
		VALUES (@name,@note,@yearOfPubl, 'Book');

		SET @id = SCOPE_IDENTITY()

		INSERT INTO BookData (Id, CityName, PublishingHouse, NumberOfPages, ISBN)
		VALUES (@id, @cityName, @publishingHouse, @numberOfPages, @isbn);

		IF NOT EXISTS (SELECT Id FROM PaperObj WHERE Id=@id) 
		OR NOT EXISTS (SELECT Id FROM BookData WHERE Id=@id)
			Rollback
		ELSE			
			BEGIN
				COMMIT
				DECLARE @description nvarchar(300) 
				SET @description= CONCAT('Added new Book with params: Id =', @id ,
					' Name = ', @name,
					' Note = ', @note, 
					' Year = ', @yearOfPubl,
					' CityName = ', @cityName,
					' PublishingHouse = ', @publishingHouse,
					' NumberOfPages = ',@numberOfPages,
					' ISBN =',@isbn);
				EXEC Inner_WriteLog 
					@typeOfObject = 'Book',
					@objId = @id,
					@descriptionOfAction = @description
			END
END
GO
/****** Object:  StoredProcedure [dbo].[Book_AddingAuthorToBook]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_AddingAuthorToBook]
	@id INT OUTPUT,
	@bookId int,
	@authorId int
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM PaperObj
	WHERE Id=@bookId AND MarkedAsDelete = 1)
	BEGIN
	INSERT INTO BookAuthors(BookId, AuthorId)
	VALUES (@bookId, @authorId);
	END
	SET @id=SCOPE_IDENTITY();
	DECLARE @description nvarchar(300);
			SET @description= CONCAT('Added author with Id =', @authorId ,
					' to book with Id=  ', @bookId);
				EXEC Inner_WriteLog 
					@typeOfObject = 'AuthorBooks',
					@objId = @id,
					@descriptionOfAction = @description
END
GO
/****** Object:  StoredProcedure [dbo].[Book_GetAllBooks]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_GetAllBooks]

AS
BEGIN
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, NumberOfPages, ISBN
	FROM PaperObj INNER JOIN BookData 
	ON PaperObj.Id = BookData.Id
	WHERE MarkedAsDelete = 0
END
GO
/****** Object:  StoredProcedure [dbo].[Book_GetBookByAuthor]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_GetBookByAuthor]
	@firstName nvarchar(300),
	@lastName nvarchar(300)

AS
BEGIN
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, NumberOfPages, ISBN
	FROM PaperObj INNER JOIN BookData 
	ON PaperObj.Id = BookData.Id
	WHERE (PaperObj.Id = (SELECT BookId FROM BookAuthors
		WHERE AuthorId = (SELECT Id FROM Authors 
			WHERE FirstName=@firstName AND LastName=@lastName)) AND MarkedAsDelete = 0)
END
GO
/****** Object:  StoredProcedure [dbo].[Book_GetBookById]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_GetBookById]
	@id int
AS
BEGIN

	SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, NumberOfPages, ISBN
	FROM PaperObj INNER JOIN BookData 
	ON PaperObj.Id = BookData.Id
	WHERE PaperObj.Id = @id AND MarkedAsDelete = 0
END
GO
/****** Object:  StoredProcedure [dbo].[Book_GetBooksAuthors]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_GetBooksAuthors]
	@bookId int
AS
BEGIN

	SELECT Id, FirstName, LastName
	FROM Authors 	
	WHERE Id = (
	SELECT AuthorId 
	FROM BookAuthors
	WHERE BookId=@bookId)
END
GO
/****** Object:  StoredProcedure [dbo].[Book_GetBooksStartWithCharacterSet]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_GetBooksStartWithCharacterSet]
	@inputString nvarchar(300)
AS
BEGIN

    SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, NumberOfPages, ISBN
	FROM PaperObj INNER JOIN BookData 
	ON PaperObj.Id = BookData.Id
	WHERE Name LIKE	@inputString + '%' AND MarkedAsDelete = 0
END
GO
/****** Object:  StoredProcedure [dbo].[Book_GetOrderedBooks]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_GetOrderedBooks]

AS
BEGIN
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, NumberOfPages, ISBN
	FROM PaperObj INNER JOIN BookData 
	ON PaperObj.Id = BookData.Id
	WHERE MarkedAsDelete =0
	ORDER BY YearOfPublishing 
END
GO
/****** Object:  StoredProcedure [dbo].[Book_GetOrderedBooksByDesc]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_GetOrderedBooksByDesc]

AS
BEGIN
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, NumberOfPages, ISBN
	FROM PaperObj INNER JOIN BookData 
	ON PaperObj.Id = BookData.Id
	WHERE MarkedAsDelete =0
	ORDER BY YearOfPublishing DESC
END
GO
/****** Object:  StoredProcedure [dbo].[Book_GetPatentAuthors]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_GetPatentAuthors]
	@patentId int
AS
BEGIN

	SELECT Authors.Id, FirstName, LastName
	FROM Authors 
	INNER JOIN PatentAuthors
	ON Authors.Id = PatentAuthors.Id
	WHERE PatentAuthors.PatentId = @patentId;
END
GO
/****** Object:  StoredProcedure [dbo].[Book_GetPatentsAuthors]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Book_GetPatentsAuthors]
	@patentId int
AS
BEGIN

	SELECT Authors.Id, FirstName, LastName
	FROM Authors 
	INNER JOIN PatentAuthors
	ON Authors.Id = PatentAuthors.Id
	WHERE PatentAuthors.PatentId = @patentId;
END
GO
/****** Object:  StoredProcedure [dbo].[Book_IsBookAlreadyExist]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_IsBookAlreadyExist]
	@name nvarchar(300),
	@yearOfPubl int,
	@firstName nvarchar(300),
	@lastName nvarchar(300)

AS
BEGIN
    -- Insert statements for procedure here
	SELECT COUNT(*) FROM PaperObj
	INNER JOIN BookAuthors
	ON PaperObj.Id = BookAuthors.BookId
	INNER JOIN Authors
	ON BookAuthors.AuthorId = Authors.Id
	WHERE [Name]=@name 
	AND YearOfPublishing=@yearOfPubl 
	AND FirstName = @firstName
	AND LastName = @lastName
END
GO
/****** Object:  StoredProcedure [dbo].[Book_IsISSNExist]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_IsISSNExist]
	@ISBN nvarchar(50)
AS
BEGIN
    -- Insert statements for procedure here
	SELECT COUNT(*) FROM BookData
	WHERE ISBN=@ISBN
END
GO
/****** Object:  StoredProcedure [dbo].[Inner_MarkAsDelete]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Inner_MarkAsDelete]
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE PaperObj
	SET MarkedAsDelete = 1
	WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[Inner_WriteLog]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Inner_WriteLog]
	@typeOfObject NVARCHAR(300),
	@objId int,
	@descriptionOfAction NVARCHAR(300)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO Log (ObjectType, ObjectId, [Description], NameOfUser, DateOfOperation)
	VALUES (@typeOfObject, @objId, @descriptionOfAction, CURRENT_USER, SYSDATETIME());
END
GO
/****** Object:  StoredProcedure [dbo].[Issue_Adding]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Issue_Adding]
	@id INT OUTPUT,
	@numberOfIssue int,
	@releaseDay int,
	@numberOfPages int

AS
BEGIN

	SET NOCOUNT ON;

    INSERT INTO Issue(NumberOfIssue, ReleaseDay, NumberOfPages)
	VALUES (@numberOfIssue, @releaseDay, @numberOfPages);
	
	SET @id = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[Issue_Deleting]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Issue_Deleting]
	@id int

AS
BEGIN

    DELETE FROM Issue
	WHERE Id=@id;
END
GO
/****** Object:  StoredProcedure [dbo].[Issue_GetIssueById]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Issue_GetIssueById]
	@id int

AS
BEGIN

    SELECT Id, NumberOfIssue, ReleaseDay, NumberOfPages, NewspaperId
	FROM Issue WHERE Id=@id;
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_Adding]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_Adding]
	@id INT OUTPUT,
	@name nvarchar(300),
	@note nvarchar(2000),
	@yearOfPubl int,
	@cityName nvarchar(50),
	@publishingHouse nvarchar(300),
	@ISSN nvarchar(20)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRAN
		INSERT INTO PaperObj (Name,Note,YearOfPublishing, TypeOfObj)
		VALUES (@name,@note,@yearOfPubl, 'Newspaper');

		SET @id = SCOPE_IDENTITY()

		INSERT INTO NewspaperData(Id, CityName, PublishingHouse, ISSN)
		VALUES (@id, @cityName, @publishingHouse, @issn);

		IF NOT EXISTS (SELECT Id FROM PaperObj WHERE Id=@id) OR NOT EXISTS (SELECT Id FROM NewspaperData WHERE Id=@id)
			Rollback
		ELSE
			COMMIT
			DECLARE @description nvarchar(300);
			SET @description= CONCAT('Added new Newspaper with params: Id =', @id ,
					' Name = ', @name,
					' Note = ', @note, 
					' Year = ', @yearOfPubl,
					' CityName = ', @cityName,
					' PublishingHouse = ', @publishingHouse,
					' ISSN =',@ISSN);
				EXEC Inner_WriteLog 
					@typeOfObject = 'Newspaper',
					@objId = @id,
					@descriptionOfAction = @description
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_AddingIssueToNewspaper]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_AddingIssueToNewspaper]
	@newspaperId int,
	@issueId int
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (SELECT Id FROM NewspaperData WHERE Id=@newspaperId) 
	AND EXISTS (SELECT Id FROM Issue WHERE Id=@issueId)
	AND NOT EXISTS (SELECT * FROM PaperObj
					WHERE Id=@newspaperId AND MarkedAsDelete = 1)
	BEGIN
	UPDATE Issue
	SET NewspaperId = @newspaperId
	WHERE Id = @issueId
	END

			DECLARE @description nvarchar(300);
			SET @description= CONCAT('Added issue with Id =', @issueId ,
					' to newspaper with Id=  ', @newspaperId);
				EXEC Inner_WriteLog 
					@typeOfObject = 'Issue',
					@objId = @issueId,
					@descriptionOfAction = @description
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_GetAllNewspapers]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_GetAllNewspapers]

AS
BEGIN   
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, ISSN 
	FROM PaperObj INNER JOIN NewspaperData 
	ON PaperObj.Id = NewspaperData.Id
	WHERE MarkedAsDelete = 0
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_GetIssuesOfNewspaper]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Newspaper_GetIssuesOfNewspaper]
	@newspaperId int
AS
BEGIN

	SET NOCOUNT ON;

	SELECT Id, NumberOfIssue, ReleaseDay, NumberOfPages 
	FROM Issue 
	WHERE NewspaperId=@newspaperId;
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_GetNewspaperById]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_GetNewspaperById]
	@newspaperId int
AS
BEGIN  
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, ISSN 
	FROM PaperObj INNER JOIN NewspaperData 
	ON PaperObj.Id = NewspaperData.Id
	WHERE PaperObj.Id = @newspaperId AND MarkedAsDelete =0
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_GetNewspapersByPublishingHouse]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_GetNewspapersByPublishingHouse]
	@publishingHouse nvarchar(300)
AS
BEGIN   
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, ISSN 
	FROM PaperObj INNER JOIN NewspaperData 
	ON PaperObj.Id = NewspaperData.Id
	WHERE PublishingHouse LIKE @publishingHouse AND MarkedAsDelete =0
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_GetNewspapersStartsWithsCharSet]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_GetNewspapersStartsWithsCharSet]
	@charSet nvarchar(300)
AS
BEGIN   
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, ISSN 
	FROM PaperObj INNER JOIN NewspaperData 
	ON PaperObj.Id = NewspaperData.Id
	WHERE Name LIKE @charSet + '%' AND MarkedAsDelete =0
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_GetOrderedByDescNewspapers]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_GetOrderedByDescNewspapers]

AS
BEGIN   
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, ISSN 
	FROM PaperObj INNER JOIN NewspaperData 
	ON PaperObj.Id = NewspaperData.Id
	WHERE MarkedAsDelete = 0
	ORDER BY YearOfPublishing DESC 
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_GetOrderedNewspapers]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_GetOrderedNewspapers]

AS
BEGIN   
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, CityName, PublishingHouse, ISSN 
	FROM PaperObj INNER JOIN NewspaperData 
	ON PaperObj.Id = NewspaperData.Id
	WHERE  MarkedAsDelete =0
	ORDER BY YearOfPublishing 
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_IsISSNAndNameEqualsDataInDB]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_IsISSNAndNameEqualsDataInDB]
	@issn nvarchar(20),
	@name nvarchar(300)
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*) FROM PaperObj
	INNER JOIN NewspaperData
	ON PaperObj.Id = NewspaperData.Id
	WHERE ISSN= @issn 
	AND Name=@name;
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_IsISSNExistInStorage]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_IsISSNExistInStorage]
	@issn nvarchar(20)
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*) FROM NewspaperData
	WHERE ISSN= @issn;
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_IsNewspaperUnique]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_IsNewspaperUnique]
	@name nvarchar(300),
	@publishingHouse nvarchar(300),
	@yearOfPublishing int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*) FROM PaperObj
	INNER JOIN NewspaperData
	ON PaperObj.Id = NewspaperData.Id
	WHERE [Name] = @name 
	AND PublishingHouse = @publishingHouse
	AND YearOfPublishing = @yearOfPublishing;
END
GO
/****** Object:  StoredProcedure [dbo].[Obj_Deleting]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Obj_Deleting]
	@id int
AS
BEGIN
	IF (IS_ROLEMEMBER('Librarian')=1)
	BEGIN
		EXEC dbo.Inner_MarkAsDelete @id=@id
	END
	ELSE
	BEGIN
    DELETE FROM PaperObj WHERE Id=@id;
	END

			DECLARE @description nvarchar(300);
			SET @description= CONCAT('Delete object with Id', @id)
				EXEC Inner_WriteLog 
					@typeOfObject = 'PaperObj',
					@objId = @id,
					@descriptionOfAction = @description
END
GO
/****** Object:  StoredProcedure [dbo].[Obj_GetAllPapers]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Obj_GetAllPapers]	
AS
BEGIN

	SELECT PaperObj.Id, [Name], Note, YearOfPublishing, TypeOfObj, BookData.CityName AS BookCityName, BookData.PublishingHouse AS BookPublishingHouse, BookData.NumberOfPages AS BookNumberOfPages, ISBN, NewspaperData.CityName AS NewspaperCityName, NewspaperData.PublishingHouse AS NewspaperPublishingHouse, ISSN, Country, RegistrationNumber, ApplicationDate, PatentData.NumberOfPages AS PatentNumberOfPages
	FROM PaperObj 
	FULL JOIN BookData 
	ON PaperObj.Id = BookData.Id
	FULL JOIN NewspaperData
	ON PaperObj.Id = NewspaperData.Id
	FULL JOIN PatentData
	ON PaperObj.Id = PatentData.Id
	WHERE MarkedAsDelete = 0

END
GO
/****** Object:  StoredProcedure [dbo].[Obj_GetAllPapersByName]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Obj_GetAllPapersByName]
	@name nvarchar(300)
AS
BEGIN

    -- Insert statements for procedure here
	SELECT PaperObj.Id, [Name], Note, YearOfPublishing, TypeOfObj, BookData.CityName AS BookCityName, BookData.PublishingHouse AS BookPublishingHouse, BookData.NumberOfPages AS BookNumberOfPages, ISBN, NewspaperData.CityName AS NewspaperCityName, NewspaperData.PublishingHouse AS NewspaperPublishingHouse, ISSN, Country, RegistrationNumber, ApplicationDate, PatentData.NumberOfPages AS PatentNumberOfPages
	FROM PaperObj 
	FULL JOIN BookData 
	ON PaperObj.Id = BookData.Id
	FULL JOIN NewspaperData
	ON PaperObj.Id = NewspaperData.Id
	FULL JOIN PatentData
	ON PaperObj.Id = PatentData.Id
	WHERE MarkedAsDelete = 0 AND [Name] LIKE @name;

END
GO
/****** Object:  StoredProcedure [dbo].[Obj_GetAllPapersGroupedByDate]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Obj_GetAllPapersGroupedByDate]
	@year int
AS
BEGIN

    -- Insert statements for procedure here
	SELECT PaperObj.Id, [Name], Note, YearOfPublishing, TypeOfObj, BookData.CityName AS BookCityName, BookData.PublishingHouse AS BookPublishingHouse, BookData.NumberOfPages AS BookNumberOfPages, ISBN, NewspaperData.CityName AS NewspaperCityName, NewspaperData.PublishingHouse AS NewspaperPublishingHouse, ISSN, Country, RegistrationNumber, ApplicationDate, PatentData.NumberOfPages AS PatentNumberOfPages
	FROM PaperObj 
	FULL JOIN BookData 
	ON PaperObj.Id = BookData.Id
	FULL JOIN NewspaperData
	ON PaperObj.Id = NewspaperData.Id
	FULL JOIN PatentData
	ON PaperObj.Id = PatentData.Id
	WHERE MarkedAsDelete = 0 AND YearOfPublishing= @year;

END
GO
/****** Object:  StoredProcedure [dbo].[Patent_Adding]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Patent_Adding]
	@id INT OUTPUT,
	@name nvarchar(300),
	@note nvarchar(2000),
	@yearOfPubl int,
	@country nvarchar(200),
	@registrationNumber nvarchar(20),
	@applicationDate int,
	@numberOfPages int
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRAN
		INSERT INTO PaperObj ([Name],Note,YearOfPublishing, TypeOfObj)
		VALUES (@name,@note,@yearOfPubl, 'Patent');

		SET @id = SCOPE_IDENTITY()

		INSERT INTO PatentData(Id, Country, RegistrationNumber, ApplicationDate, NumberOfPages)
		VALUES (@id, @country, @registrationNumber, @applicationDate, @numberOfPages);

		IF NOT EXISTS (SELECT Id FROM PaperObj WHERE Id=@id) OR NOT EXISTS (SELECT Id FROM PatentData WHERE Id=@id)
			Rollback
		ELSE
			COMMIT
			DECLARE @description nvarchar(300);
			SET @description= CONCAT('Added new Newspaper with params: Id =', @id ,
					' Name = ', @name,
					' Note = ', @note, 
					' Year = ', @yearOfPubl,
					' Country = ', @country,
					' RegistrationNumber = ', @registrationNumber,
					' ApplicationDate =',@applicationDate,
					' NumberOfPages =',@numberOfPages);
				EXEC Inner_WriteLog 
					@typeOfObject = 'Patent',
					@objId = @id,
					@descriptionOfAction = @description

END
GO
/****** Object:  StoredProcedure [dbo].[Patent_AddingAuthorToPatent]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Patent_AddingAuthorToPatent]
	@id INT OUTPUT,
	@patentId int,
	@authorId int
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM PaperObj
	WHERE Id=@patentId AND MarkedAsDelete = 1)
	BEGIN
	INSERT INTO BookAuthors(BookId, AuthorId)
	VALUES (@patentId, @authorId);
	END
	
	SET @id = SCOPE_IDENTITY()

	DECLARE @description nvarchar(300);
			SET @description= CONCAT('Added author with Id =', @authorId ,
					' to patent with Id=  ', @patentId);
				EXEC Inner_WriteLog 
					@typeOfObject = 'AuthorBooks',
					@objId = @id,
					@descriptionOfAction = @description
END
GO
/****** Object:  StoredProcedure [dbo].[Patent_GetAllPatents]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Patent_GetAllPatents]

AS
BEGIN
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, Country, RegistrationNumber, ApplicationDate, NumberOfPages
	FROM PaperObj INNER JOIN PatentData 
	ON PaperObj.Id = PatentData.Id
	WHERE MarkedAsDelete = 0
END
GO
/****** Object:  StoredProcedure [dbo].[Patent_GetOrderedByDescPatents]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Patent_GetOrderedByDescPatents]
	@patentId int

AS
BEGIN
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, Country, RegistrationNumber, ApplicationDate, NumberOfPages
	FROM PaperObj INNER JOIN PatentData 
	ON PaperObj.Id = PatentData.Id
	WHERE  MarkedAsDelete =0
	ORDER BY YearOfPublishing DESC
	
END
GO
/****** Object:  StoredProcedure [dbo].[Patent_GetOrderedPatents]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Patent_GetOrderedPatents]
	@patentId int

AS
BEGIN
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, Country, RegistrationNumber, ApplicationDate, NumberOfPages
	FROM PaperObj INNER JOIN PatentData 
	ON PaperObj.Id = PatentData.Id
	WHERE MarkedAsDelete =0
	ORDER BY YearOfPublishing 
	
END
GO
/****** Object:  StoredProcedure [dbo].[Patent_GetPatentById]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Patent_GetPatentById]
	@patentId int

AS
BEGIN
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, Country, RegistrationNumber, ApplicationDate, NumberOfPages
	FROM PaperObj INNER JOIN PatentData 
	ON PaperObj.Id = PatentData.Id
	WHERE PaperObj.Id = @patentId AND MarkedAsDelete =0
	
END
GO
/****** Object:  StoredProcedure [dbo].[Patent_GetPatentsByAuthor]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Patent_GetPatentsByAuthor]
	@firstName nvarchar(300),
	@lastName nvarchar(300)

AS
BEGIN
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, Country, RegistrationNumber,ApplicationDate, NumberOfPages
	FROM PaperObj INNER JOIN PatentData 
	ON PaperObj.Id = PatentData.Id
	WHERE (PaperObj.Id = (SELECT PatentId FROM PatentAuthors
		WHERE AuthorId = (SELECT Id FROM Authors 
			WHERE FirstName=@firstName AND LastName=@lastName)) AND MarkedAsDelete =0)
END
GO
/****** Object:  StoredProcedure [dbo].[Patent_GetPatentsStartsWithsCharSet]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Patent_GetPatentsStartsWithsCharSet]
	@charSet nvarchar(300)
AS
BEGIN   
	SELECT PaperObj.Id, Name, Note, YearOfPublishing, Country, RegistrationNumber, ApplicationDate, NumberOfPages 
	FROM PaperObj INNER JOIN PatentData 
	ON PaperObj.Id = PatentData.Id
	WHERE Name LIKE @charSet + '%' AND MarkedAsDelete =0
END
GO
/****** Object:  StoredProcedure [dbo].[Patent_IsPatentUnique]    Script Date: 10/24/2021 11:56:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Patent_IsPatentUnique]
	@registrationNumber nvarchar(20),
	@country nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*) FROM PatentData 
	WHERE RegistrationNumber = @registrationNumber 
	AND Country=@country;
END
GO
USE [master]
GO
ALTER DATABASE [LibraryDB] SET  READ_WRITE 
GO
