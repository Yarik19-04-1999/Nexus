# Dvizh

Сервис инвайт-ссылок. Состоит из API (`Dvizh.Api`) и веб-интерфейса (`Dvizh.UI`).

## Требования

- .NET 10 SDK
- Node.js 20+
- SQL Server (локальный или удалённый)

## Первый запуск

### 1. База данных

Убедитесь, что база `Nexus` создана (см. [README в корне](../README.md)).

Затем выполните скрипт схемы Dvizh:

```
src/Dvizh.Application/scripts/script.sql
```

Скрипт создаёт логин `DvizhLogin`, схему `Dvizh` и все таблицы. При повторном запуске схема пересоздаётся с нуля.

### 2. API (Dvizh.Api)

Откройте `Dvizh.slnx` в Visual Studio и запустите профиль `http`.

Конфигурация окружения — `src/Dvizh.Api/appsettings.Development.json`:

```json
{
  "SqlServer": {
    "ConnectionString": "..."
  },
  "AllowedOrigins": [ "http://localhost:3000" ]
}
```

API запускается на `http://localhost:5055`.

### 3. Веб-интерфейс (Dvizh.UI)

Установите зависимости (один раз):

```
cd src/Dvizh.UI
npm install
```

Настройте `src/Dvizh.UI/.env.local`:

```
NEXT_PUBLIC_API_URL=http://localhost:5055
```

Запустить веб-интерфейс:

```
src/Dvizh.UI/launch.cmd
```

Или вручную:

```
cd src/Dvizh.UI
npm run dev
```

UI доступен на `http://localhost:3000`.

## Батники

| Файл | Действие |
|------|----------|
| `src/Dvizh.UI/launch.cmd` | Запустить Next.js в dev-режиме |

## Структура

```
Dvizh/
├── src/
│   ├── Dvizh.Api/              # ASP.NET Core API
│   ├── Dvizh.Application/      # Use cases, EF Core, скрипты БД
│   └── Dvizh.UI/               # Next.js фронтенд
└── Dvizh.slnx                  # Visual Studio solution
```
