-- Check if the database already exists
DECLARE @dbname nvarchar(128)
SET @dbname = 'HackCheckDB'

IF(EXISTS(SELECT name FROM master.dbo.sysdatabases WHERE ('['+ name +']' = @dbname OR name = @dbname))) 
BEGIN

DROP DATABASE HackCheckDB

DROP LOGIN HackerCheckMaster

END;

CREATE DATABASE HackCheckDB

GO
USE HackCheckDB


-- Create user account to manipulate the database with
CREATE LOGIN HackerCheckMaster WITH PASSWORD = 'HackerCheckMasterPassword'

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'HackerCheckMaster')
BEGIN
	CREATE USER [HackerCheckMaster] FOR LOGIN [HackerCheckMaster]
	EXEC sp_addrolemember N'db_owner', N'HackerCheckMaster'
END;



-- Create Users table
CREATE TABLE [dbo].[Users](
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Username] VARCHAR (16) NOT NULL,
	[Email] NVARCHAR(320) NOT NULL,
	[Password] CHAR(64) NOT NULL,
	[Salt] VARCHAR(64) NOT NULL,
	PRIMARY KEY (Id)
);

-- Create Logins table
CREATE TABLE [dbo].[Logins](
	[UserId] INT NOT NULL,
	[DateATime] VARCHAR(16) NOT NULL,
	[Location] NVARCHAR(320) NOT NULL,
	[IpAdress] CHAR(64) NOT NULL,
	CONSTRAINT PK_LoginId PRIMARY KEY (UserId, DateATime),
	FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Create CreatedGames table
CREATE TABLE [dbo].[CreatedGames](
	[CreatedGameId] INT IDENTITY (1, 1) NOT NULL,
	[UserId] INT NOT NULL,
	[Description] VARCHAR(256) NOT NULL,
	[CreationDate] DATE NOT NULL,
	[Published] BIT NOT NULL,
	[ThumbnailPath] VARCHAR (256) NOT NULL
	PRIMARY KEY (CreatedGameId),
	FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Create CreatedCollections table
CREATE TABLE [dbo].[CreatedCollections](
	[CreatedCollectionId] INT IDENTITY (1, 1) NOT NULL,
	[UserId] INT NOT NULL,
	[Description] VARCHAR(256) NOT NULL,
	[CreationDate] DATE NOT NULL,
	[Published] BIT NOT NULL,
	[ThumbnailPath] VARCHAR (256) NOT NULL
	PRIMARY KEY (CreatedCollectionId),
	FOREIGN KEY (UserId) REFERENCES Users(Id)

);

-- Create GameCollectionMatch table
CREATE TABLE [dbo].[GameCollectionMatch](
	[CreatedGameId] INT NOT NULL,
	[CreatedCollectionId] INT NOT NULL,
	CONSTRAINT PK_GameCollectionMatchId PRIMARY KEY (CreatedGameId, CreatedCollectionId),
	FOREIGN KEY (CreatedGameId) REFERENCES CreatedGames(CreatedGameId),
	FOREIGN KEY (CreatedCollectionId) REFERENCES CreatedCollections(CreatedCollectionId)


);



-- Select all for testing
SELECT * FROM [Users];
SELECT * FROM [Logins];

