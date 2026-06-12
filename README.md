# Nexus

Monorepo. Each service lives in its own folder (`Dvizh`, `Lore`, …).

## Database

Nexus uses SQL Server. The database is created once; each service then creates its own schema on top of it.

### Create the database

Connect to SQL Server and run:

```
Shared/scripts/create_database.sql
```

This creates the `Nexus` database with all required settings and registers the `dbo.ConfigureSchema` / `dbo.DropSchema` stored procedures used by all services.

### Add a service schema

After the database is created, run the schema script for each service you want to set up:

| Service | Script |
|---------|--------|
| Dvizh   | `Dvizh/src/Dvizh.Application/scripts/script.sql` |

Each script creates a login, user, schema, and tables. Re-running is idempotent — `DropSchema` tears everything down first.

## Services

| Service | Description |
|---------|-------------|
| [Dvizh](Dvizh/README.md) | Invite link system |
| [Information](Information/README.md) | Exchange rates, Epic Games giveaways, weather |
