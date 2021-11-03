USE master;
GO

IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'aspnet')
	BEGIN
		USE aspnet;
		SELECT * FROM dbo.AspNetUsers ORDER BY 1 DESC;
		SELECT * FROM dbo.AspNetUserLogins ORDER BY 1 DESC;
		SELECT * FROM dbo.AspNetRoles ORDER BY 1 DESC;
		SELECT * FROM dbo.AspNetUserRoles ORDER BY 1 DESC;
		SELECT * FROM dbo.AspNetUserClaims ORDER BY 1 DESC;
		IF  EXISTS (SELECT name FROM sys.tables WHERE name = N'__MigrationHistory')
			SELECT * FROM dbo.__MigrationHistory;
		USE master;
	END
GO

/*
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'aspnet')
	BEGIN
		USE master;
		ALTER DATABASE aspnet
			SET SINGLE_USER
			WITH ROLLBACK IMMEDIATE;
		DROP DATABASE aspnet;
	END
GO
*/
