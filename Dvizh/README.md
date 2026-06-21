# Dvizh

Invite link service. Consists of an API (`Dvizh.Api`) and a web frontend (`Dvizh.UI`).

## Requirements

- .NET 10 SDK
- Node.js 20+
- SQL Server (local or remote)

## First-time setup

### 1. Database

Make sure the `Nexus` database exists (see [root README](../README.md)).

Then run the Dvizh schema script:

```
src/Dvizh.Application/scripts/script.sql
```

This creates the `DvizhLogin` SQL login, the `Dvizh` schema, and all tables. Re-running recreates everything from scratch.

### 2. API — secrets

The connection string is stored in [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) and is never committed to the repository. Ask the project lead for the password.

Initialize secrets for the project (one-time):

```
cd src/Dvizh.Api
dotnet user-secrets init
```

Set the connection string:

```
dotnet user-secrets set "SqlServer:ConnectionString" "Server=.;Database=Nexus;User Id=DvizhLogin;Password=SPECIFY_VALUE;TrustServerCertificate=true"
```

Replace `SPECIFY_VALUE` with the actual password for `DvizhLogin`.

### 3. API — run

Open `Dvizh.slnx` in Visual Studio and start the `http` profile.

The API runs on `http://localhost:5055`.

### 4. Web frontend — secrets

Create `src/Dvizh.UI/.env.local` (never committed — already in `.gitignore`):

```
NEXT_PUBLIC_API_URL=SPECIFY_VALUE
```

For local development set it to `http://localhost:5055`.

### 5. Web frontend — run

Install dependencies (once):

```
cd src/Dvizh.UI
npm install
```

Start the dev server:

```
src/Dvizh.UI/launch.cmd
```

Or manually:

```
cd src/Dvizh.UI
npm run dev
```

The frontend runs on `http://localhost:3000`.

### Production build

```
cd src/Dvizh.UI
npm run build
npm start
```

`next build` minifies JS/CSS and optimizes all assets automatically.

## Scripts

| File | Action |
|------|--------|
| `src/Dvizh.UI/launch.cmd` | Start Next.js dev server |

## Structure

```
Dvizh/
├── src/
│   ├── Dvizh.Api/              # ASP.NET Core API
│   ├── Dvizh.Application/      # Use cases, EF Core, DB scripts (no separate Infrastructure)
│   └── Dvizh.UI/               # Next.js 16 frontend
├── test/
│   └── Dvizh.Integration.Tests/
└── Dvizh.slnx                  # Visual Studio solution
```
