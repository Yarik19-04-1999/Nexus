# Architecture

Lore follows clean architecture: `Lore.Api` / `Lore.Application` / `Lore.Infrastructure`.

Use cases in `Lore.Application` depend on `ILoreStore` (defined in `Interfaces/Stores/`), not on `DbContext` directly. `LoreStore` (in `Lore.Infrastructure`) implements `ILoreStore`.

```
Lore.Application/
  Interfaces/
    Stores/
      ILoreStore.cs      ← single store interface for all DB operations
    UseCases/
  Models/
    Inputs/
    Mappers/
  UseCases/
  scripts/               ← does not exist here; scripts live in Lore.Infrastructure

Lore.Infrastructure/
  DbContexts/
    LoreDbContext.cs
  Configurations/
  Stores/
    LoreStore.cs         ← ILoreStore implementation
  scripts/
    script.sql
```

When adding a new entity, add its CRUD methods to `ILoreStore` and implement them in `LoreStore`. Do not inject `LoreDbContext` into use cases.

# Frontend

`Lore.UI` is a Next.js / React / TypeScript frontend for managing Lore entities (universes, etc.).
