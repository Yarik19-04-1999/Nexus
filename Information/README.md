# Information

Aggregation service that provides USD exchange rates, Epic Games Store free giveaways, and weather data. Intended for use by a Telegram bot and other consumers.

## Requirements

- .NET 10 SDK

## First-time setup

### 1. API secrets

External providers require API keys. Ask the project lead for the values.

Initialize secrets (once):

```
cd src/Information.Api
dotnet user-secrets init
```

Set the required secrets:

```
dotnet user-secrets set "Weather:ApiKey" "SPECIFY_VALUE"
```

> Exchange rates and Epic Games giveaways use public endpoints and do not require API keys.

### 2. Run

Open `Information.slnx` in Visual Studio and start the `http` profile.

The API runs on `http://localhost:5065`.

## Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/v1/information/exchange-rates` | USD buy/sell rates from multiple banks |
| GET | `/api/v1/information/epic-giveaways` | Currently free games on Epic Games Store |
| GET | `/api/v1/information/weather?city={city}` | Current weather for the given city |

## Structure

```
Information/
├── src/
│   ├── Information.Api/              # ASP.NET Core API
│   ├── Information.Application/      # Use cases, models, provider interfaces
│   └── Information.Infrastructure/   # Provider implementations (HTTP clients)
└── Information.slnx                  # Visual Studio solution
```
