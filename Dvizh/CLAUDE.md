# Architecture

Dvizh has no separate Infrastructure project. `DvizhDbContext` and all entity `IEntityTypeConfiguration` implementations live in `Dvizh.Application`.

Use case implementations live under `Services/UseCases/`, not a top-level `UseCases/` folder. Interfaces are under `Interfaces/UseCases/`.

```
Dvizh.Application/
  DbContexts/
    DvizhDbContext.cs
  Configurations/        ← IEntityTypeConfiguration files
  Services/
    UseCases/            ← use case implementations
  Interfaces/
    UseCases/            ← IXxxUseCase interfaces
  Models/
  scripts/
    script.sql
```

Use cases inject `DvizhDbContext` directly — there is no store abstraction layer.

# Frontend

`Dvizh.UI` is a Next.js 16 / React 19 / TypeScript frontend.

Stack: TanStack Query v5, React Hook Form + Zod, HeadlessUI, Tailwind v4, Lucide icons.

API calls go through a custom fetch wrapper at `lib/api/client.ts`. Do not use `fetch` directly in components or hooks.
