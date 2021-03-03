-- Check if the database already exists
DECLARE @dbname nvarchar(128)
SET @dbname = 'HackCheckDB'

IF(EXISTS(SELECT name FROM master.dbo.sysdatabases WHERE ('['+ name +']' = @dbname OR name = @dbname))) 
BEGIN

DROP DATABASE HackCheckDB

DROP LOGIN HackerCheckMaster

CREATE DATABASE HackCheckDB

END;

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
	[Id] INT IDENTITY (1, 1),
	[Username] VARCHAR(16),
	[Email] NVARCHAR(320),
	[Password] CHAR(64),
	[Salt] VARCHAR(64),
	PRIMARY KEY (Id)
);

-- Select all for testing
SELECT * FROM [Users];
