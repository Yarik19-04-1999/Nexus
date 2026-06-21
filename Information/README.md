# Information

Aggregation service that provides USD exchange rates, Epic Games Store free giveaways, and weather data. Intended for use by a Telegram bot and other consumers.

| Component | URL |
|---|---|
| API | http://localhost:5065 |
| Scalar (API docs) | http://localhost:5065/scalar |

## Prerequisites

- .NET 10 SDK
- SQL Server with the `Nexus` database set up (see [root README](../README.md))

## First-time setup

### 1. Database

Run the schema script against your SQL Server instance:

```
Information/src/Information.Infrastructure/scripts/script.sql
```

### 2. API secrets

```
cd src/Information.Api
dotnet user-secrets set "SqlServer:ConnectionString" "Server=localhost;Database=Nexus;Trusted_Connection=True;TrustServerCertificate=True"
dotnet user-secrets set "Weather:ApiKey" "SPECIFY_VALUE"
```

`Weather:ApiKey` — ask the project lead for the value. Exchange rates and Epic Games giveaways use public endpoints and do not require API keys.

## Running

Start the API (from `src/Information.Api`):

```
dotnet run
```

Or open `Information.slnx` in Visual Studio and start the `http` profile.

## Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/v1/information/exchange-rates` | USD buy/sell rates from multiple banks |
| GET | `/api/v1/information/epic-giveaways` | Currently free games on Epic Games Store |
| GET | `/api/v1/information/weather?city={city}` | Current weather for the given city |

## Structure

```
Information/
├── src/
│   ├── Information.Api/              # ASP.NET Core API
│   ├── Information.Application/      # Use cases, models, provider interfaces
│   └── Information.Infrastructure/   # Provider implementations (HTTP clients), SQL scripts
└── test/
    ├── Information.Api.Tests/
    └── Information.Integration.Tests/
```
