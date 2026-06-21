use Nexus
go

exec dbo.DropSchema @schemaName = 'Lore'
go

exec dbo.ConfigureSchema @schemaName = 'Lore', @passwordHash = 0x0200554F6584FF31112ED5A4F46343CD988B80714781B5EE88EEEA2ACAD33FDF3B25FC487F3D94EB0D7C556DAFC2E9A64A08E9EFF0827A1727471AF02ED308077BFE9EAE4EAB
go

create table Lore.Universes
(
    CreatedAt datetime2 not null default sysutcdatetime(),
    UpdatedAt datetime2 not null default sysutcdatetime(),
    Id int identity(1,1) not null,

    Name nvarchar(128) not null,
    Description nvarchar(max) null,
    IsHidden bit not null default 0,
    ListNo int not null default 0,

    constraint [Universes$PK] primary key clustered (Id)
)
go

create unique index [UQ_Movies(Title, ReleaseYear)] on Lore.Movies (Title, ReleaseYear)
go
pdatedAt datetime2 not null default sysutcdatetime(),
    Id int identity(1,1) not null,
    UniverseId int null,

    Title nvarchar(128) not null,
    ReleaseYear int not null,
    DurationMinutes int not null,
    ReviewText nvarchar(max) null,
    Score decimal(3,1) null,
    ViewCount int not null default 1,
    RewatchStatus int not null default 0,
    ListNo int not null default 0,

    constraint [Movies$PK] primary key clustered (Id),
    constraint [Movies(UniverseId)->Universes(Id)] foreign key (UniverseId)
        references Lore.Universes (Id) on delete set null,
    constraint [CK_Movies_ViewCount] check (ViewCount >= 0)
)
go
