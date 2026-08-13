:setvar DatabaseName "SelqiDb"

IF DB_ID(N'$(DatabaseName)') IS NULL
BEGIN
    DECLARE @sql nvarchar(max) = N'CREATE DATABASE [' + REPLACE(N'$(DatabaseName)', N']', N']]') + N']';
    EXEC sys.sp_executesql @sql;
END;
GO

PRINT N'Database $(DatabaseName) is ready.';
GO
