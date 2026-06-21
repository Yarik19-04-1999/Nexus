# Lore

Universe and lore catalog service with a Next.js admin UI.

| Component | URL |
|---|---|
| API | http://localhost:5045 |
| UI | http://localhost:3001 |
| Scalar (API docs) | http://localhost:5045/scalar |

## Prerequisites

- .NET 10 SDK
- Node.js (LTS)
- SQL Server with the `Nexus` database set up (see [root README](../README.md))

## First-time setup

### 1. Database

Run the schema script against your SQL Server instance:

```
Lore/src/Lore.Infrastructure/scripts/script.sql
```

### 2. API secrets

```
cd src/Lore.Api
dotnet user-secrets set "SqlServer:ConnectionString" "Server=localhost;Database=Nexus;Trusted_Connection=True;TrustServerCertificate=True"
```

### 3. UI dependencies

```
cd src/Lore.UI
npm install
```

Set the API base URL in `src/Lore.UI/.env.local`:

```
NEXT_PUBLIC_API_URL=http://localhost:5045
```

## Running

Start the API (from `src/Lore.Api`):

```
dotnet run
```

Or open `Lore.slnx` in Visual Studio and start the `http` profile.

Start the UI (from `src/Lore.UI`):

```
npm run dev
```

The UI runs on port 3001 (configured in `package.json`).

## Structure

```
Lore/
├── src/
│   ├── Lore.Api/             # ASP.NET Core API
│   ├── Lore.Application/     # Use cases, models, ILoreStore interface
│   ├── Lore.Infrastructure/  # LoreDbContext, LoreStore, SQL scripts
│   └── Lore.UI/              # Next.js frontend
└── test/
    └── Lore.Integration.Tests/
```
