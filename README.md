# Nexus

Monorepo containing three backend services and two frontend apps.

| Service | API port | UI port | Description |
|---|---|---|---|
| [Dvizh](Dvizh/README.md) | 5055 | 3000 | Invite management |
| [Lore](Lore/README.md) | 5045 | 3001 | Lore / universe catalog |
| [Information](Information/README.md) | 5065 | — | Exchange rates, Epic giveaways, weather |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (LTS)
- SQL Server (local instance or Docker)

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
| Dvizh | `Dvizh/src/Dvizh.Application/scripts/script.sql` |
| Lore | `Lore/src/Lore.Infrastructure/scripts/script.sql` |
| Information | `Information/src/Information.Infrastructure/scripts/script.sql` |

Each script creates a login, user, schema, and tables. Re-running is idempotent — `DropSchema` tears everything down first.
