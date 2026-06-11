use master
go

if exists(select 1 from sys.databases where name = 'Nexus')
begin
    alter database Nexus set single_user with rollback immediate
    drop database Nexus
end
go

create database Nexus collate Latin1_General_CI_AS
go

alter database Nexus set ansi_null_default on
go

alter database Nexus set ansi_nulls on
go

alter database Nexus set ansi_padding on
go

alter database Nexus set ansi_warnings on
go

alter database Nexus set arithabort on
go

alter database Nexus set concat_null_yields_null on
go

alter database Nexus set quoted_identifier on
go

alter database Nexus set numeric_roundabort off
go

alter database Nexus set trustworthy off
go

alter database Nexus set db_chaining off
go

alter database Nexus set allow_snapshot_isolation on
go

alter database Nexus set read_committed_snapshot on
go

alter database Nexus set page_verify checksum
go

alter database Nexus set recovery simple
go

alter database Nexus set auto_close off
go

alter database Nexus set auto_shrink off
go

alter database Nexus set target_recovery_time = 60 seconds
go

alter database Nexus set accelerated_database_recovery = on
go

alter database Nexus set auto_create_statistics on
go

alter database Nexus set auto_update_statistics on
go

alter database Nexus set auto_update_statistics_async on
go

alter database Nexus set date_correlation_optimization off
go

alter database Nexus set parameterization simple
go

alter database Nexus set cursor_default local
go

alter database Nexus set cursor_close_on_commit off
go

alter database Nexus set recursive_triggers off
go

alter database Nexus set disable_broker
go

alter database Nexus set honor_broker_priority off
go

alter database Nexus set query_store = on
(
    operation_mode = read_write,
    cleanup_policy = (stale_query_threshold_days = 30),
    max_storage_size_mb = 100,
    query_capture_mode = auto
)
go

use Nexus
go

create or alter procedure dbo.DropSchema
    @schemaName sysname
as
begin
    set nocount on

    declare @sql    nvarchar(max)
    declare @login  sysname = @schemaName + 'Login'
    declare @user   sysname = @schemaName + 'User'

    set @sql = ''
    select @sql += 'alter table ' + quotename(@schemaName) + '.' + quotename(t.name)
                 + ' drop constraint ' + quotename(fk.name) + ';'
    from sys.foreign_keys fk
    join sys.tables t on fk.parent_object_id = t.object_id
    join sys.schemas s on t.schema_id = s.schema_id
    where s.name = @schemaName
    if len(@sql) > 0 exec sp_executesql @sql

    set @sql = ''
    select @sql += 'drop table ' + quotename(@schemaName) + '.' + quotename(t.name) + ';'
    from sys.tables t
    join sys.schemas s on t.schema_id = s.schema_id
    where s.name = @schemaName
    if len(@sql) > 0 exec sp_executesql @sql

    if exists (select 1 from sys.schemas where name = @schemaName)
    begin
        set @sql = 'drop schema ' + quotename(@schemaName)
        exec sp_executesql @sql
    end

    if exists (select 1 from sys.database_principals where name = @user)
    begin
        set @sql = 'drop user ' + quotename(@user)
        exec sp_executesql @sql
    end

    if exists (select 1 from sys.server_principals where name = @login)
    begin
        set @sql = 'drop login ' + quotename(@login)
        exec sp_executesql @sql
    end
end
go

create or alter procedure dbo.ConfigureSchema
    @schemaName    sysname,
    @passwordHash  varbinary(256)
as
begin
    set nocount on

    declare @sql    nvarchar(max)
    declare @login  sysname = @schemaName + 'Login'
    declare @user   sysname = @schemaName + 'User'

    if not exists (select 1 from sys.server_principals where name = @login)
    begin
        set @sql = 'create login ' + quotename(@login)
                 + ' with password = ' + convert(nvarchar(512), @passwordHash, 1) + ' hashed, check_policy = off'
        exec sp_executesql @sql
    end

    if not exists (select 1 from sys.database_principals where name = @user)
    begin
        set @sql = 'create user ' + quotename(@user) + ' for login ' + quotename(@login)
        exec sp_executesql @sql
    end

    if not exists (select 1 from sys.schemas where name = @schemaName)
    begin
        set @sql = 'create schema ' + quotename(@schemaName) + ' authorization dbo'
        exec sp_executesql @sql
    end

    set @sql = 'grant insert, update, delete, select on schema::' + quotename(@schemaName) + ' to ' + quotename(@user)
    exec sp_executesql @sql
end
go
