use Nexus
go

exec dbo.DropSchema @schemaName = 'IceAgeBrief'
go

exec dbo.ConfigureSchema @schemaName = 'IceAgeBrief', @passwordHash = 0x0200554F6584FF31112ED5A4F46343CD988B80714781B5EE88EEEA2ACAD33FDF3B25FC487F3D94EB0D7C556DAFC2E9A64A08E9EFF0827A1727471AF02ED308077BFE9EAE4EAB
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
