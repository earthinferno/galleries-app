-- run in application database
drop user appuser
go

--CREATE USER [appuser] FOR LOGIN [appuser];
create user appuser for login appuser
go

exec sp_addrolemember 'db_datareader', 'appuser'
exec sp_addrolemember 'db_datawriter', 'appuser'
exec sp_addrolemember 'db_ddladmin', 'appuser'
