CREATE TABLE Species(
	[ID] int primary key not null, 
	[Name] nvarchar(50) not null
);


CREATE TABLE Animal(
	[ID] int primary key not null identity(1,1), 
	[SpeciesID] int not null foreign key references Species(ID), 
	[Name] nvarchar(50) not null, 
	[Updated] datetime
);