IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'SmartClient')
EXEC sp_addlogin 'SmartClient','IMI1234'
GO

CREATE USER [SmartClient] FOR LOGIN [SmartClient] WITH DEFAULT_SCHEMA=[dbo]
GO 

EXEC sp_addrolemember 'db_datawriter','SmartClient'
EXEC sp_addrolemember 'db_datareader','SmartClient'
GO
