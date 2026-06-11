use Nexus
go

exec dbo.DropSchema @schemaName = 'Dvizh'
go

exec dbo.ConfigureSchema @schemaName = 'Dvizh', @passwordHash = 0x0200
go

create table Dvizh.Invites
(
    CreatedAt datetime2 not null default sysutcdatetime(),
    UpdatedAt datetime2 not null default sysutcdatetime(),
    Id int identity(1,1) not null,

    ShortCode nvarchar(16) not null,
    Message nvarchar(200) not null,
    Description nvarchar(200) null,
    ExpiresAt datetime2 null,
    Answer int not null default 0,

    constraint [Invites$PK] primary key clustered (Id),
    constraint [UQ_Invites(ShortCode)] unique (ShortCode)
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
