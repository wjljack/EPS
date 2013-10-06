use [hr]
go
ALTER TABLE [dbo].[Hr_EP_Online_History]
ADD [employeetype] int NULL 
GO
update [dbo].[Hr_EP_Online_History] set [employeetype]=0
go
alter table [dbo].[Hr_EP_Online_History]
alter column [employeetype] int not NULL 

