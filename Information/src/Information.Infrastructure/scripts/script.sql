use Nexus
go

exec dbo.DropSchema @schemaName = 'IceAgeBrief'
go

exec dbo.ConfigureSchema @schemaName = 'IceAgeBrief', @passwordHash = 0x0200D328755971161981842E1B25611EBEA51CD1A5F1CBC183784EE0834733B5F9FE44E4A1D7FD6D9309C97F45DC6C697FF51208AF14F629A08FAB5E1CC3D937AAAF59A154C0
go

create table IceAgeBrief.TelegramUsers
(
    TelegramUserId bigint not null,
    Language int not null,
    CreatedAt datetime2 not null,
    UpdatedAt datetime2 not null,

    constraint [TelegramUsers$PK] primary key clustered (TelegramUserId)
)
go

create index [IX_TelegramUsers(Language)] on IceAgeBrief.TelegramUsers (Language)
go
