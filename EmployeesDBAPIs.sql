create database EmployeesDBAPIs 
go
use EmployeesDBAPIs
go
Create table Department(
	ID int identity(1,1) primary key,
	 varchar(500),
)
go
insert into Department values
('Support'),
('It')

go
Create table Employee(
	ID int identity(1,1) primary key,
	EmployeeName varchar(500),
	DateOfJoining datetime,
	PhotoFileName varchar(500),
	Department varchar(500)
)
go
insert into Employee values
('Sam', '2020-01-01', 'acx.png', 'It')

update dbo.Employee set
EmployeeName = '" + + @"',
DateOfJoining = '" + + @"',
PhotoFileName = '" + + @"',
Department = '" + + @"',
where ID = '" + + @"',


alter table dbo.Employee alter column DateOfJoining datetime 

select * from Employee