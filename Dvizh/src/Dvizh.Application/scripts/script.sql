use Nexus
go

exec dbo.DropSchema @schemaName = 'Dvizh'
go

exec dbo.ConfigureSchema @schemaName = 'Dvizh', @passwordHash = 0x0200554F6584FF31112ED5A4F46343CD988B80714781B5EE88EEEA2ACAD33FDF3B25FC487F3D94EB0D7C556DAFC2E9A64A08E9EFF0827A1727471AF02ED308077BFE9EAE4EAB
go

create table Dvizh.Invites
(
    CreatedAt datetime2 not null default sysutcdatetime(),
    UpdatedAt datetime2 not null default sysutcdatetime(),
    Id int identity(1,1) not null,

    Code nvarchar(16) not null,
    Message nvarchar(200) not null,
    Description nvarchar(200) null,
    ExpiresAt datetime2 null,
    Answer int not null default 0,
    Language int not null default 0,

    constraint [Invites$PK] primary key clustered (Id),
    constraint [UQ_Invites(Code)] unique (Code)
)
go

create table Dvizh.InviteEvents
(
    CreatedAt datetime2 not null default sysutcdatetime(),
    Id bigint identity(1,1) not null,

    InviteId int not null,
    EventType int not null,

    constraint [InviteEvents$PK] primary key nonclustered (Id),
    constraint [InviteEvents(InviteId)->Invites(Id)] foreign key (InviteId)
        references Dvizh.Invites (Id) on delete cascade
)
go

create clustered index [IX_InviteEvents(InviteId, CreatedAt)] on Dvizh.InviteEvents (InviteId, CreatedAt)
go
