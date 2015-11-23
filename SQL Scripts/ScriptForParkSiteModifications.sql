alter table rental.Quote
add ExpiryDate datetime null

alter table rental.Quote
add IsParkSitePolicy bit null default 0

CREATE TABLE dbo.ParkSites
(
Id int IDENTITY(1,1) primary key,
ParkId int foreign key references dbo.Park(Id),
SiteNumber int null,
TenantFirstName varchar(50) not null,
TenantLastName varchar(50) not null,
PhysicalAddress1 varchar(150) null,
PhysicalAddress2 varchar(150) null,
PhysicalCity varchar(50) null,
PhysicalStateId int foreign key references dbo.States(Id) null,
PhysicalZip int null,
QuoteId int foreign key references rental.Quote(Id) null,
IsActive bit,
CreatedDate datetime
)

INSERT INTO ParkSites VALUES(35,35,'Mark','Dunlop','12th Street','','OK City',23,22332, 10)
select top 10 * from dbo.Park
update rental.Quote set IsParkSitePolicy = 1 where Id = 10
select top 1 * from rental.Quote

alter table dbo.ParkSites
add IsActive bit null, CreatedDate datetime null 


update dbo.ParkSites set IsActive = 1 , CreatedDate = GETDATE()