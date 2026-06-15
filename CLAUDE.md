# SQL Server

Always pass stored procedure parameters by name, never by position.

```sql
exec dbo.ConfigureSchema @schemaName = 'Lore', @passwordHash = 0x0200...
```

Do not align columns with multiple spaces to create a visual table. Use a single space between tokens.

Index naming convention: `IX_` prefix for regular indexes, `UQ_` prefix for unique indexes. No spaces in index names. Include columns go after a `+` sign: `[IX_TableName(col1 + includeCol1)]`.

```sql
create index [IX_InviteEvents(InviteId, CreatedAt)] on Dvizh.InviteEvents (InviteId, CreatedAt)
constraint [UQ_Invites(ShortCode)] unique (ShortCode)
```

# C#

Do not write explicit numeric values in enums unless they are required (e.g. skipping values, non-default start, flags).

Always wrap statement bodies in curly braces, even for single-line statements.

Keep interfaces minimal. Any functionality that can be derived from existing interface members should be an extension method, not part of the interface itself. For example, if an interface has `GetById`, then `GetByIdRequired` (throws if null) belongs as an extension method.

Do not add the `Async` suffix to method names. The `await` keyword and the `Task` return type already make it clear that a method is asynchronous.

Never call `Result.Failure()` directly in use cases or services. The failure path must go through typed factory methods in a `*ResultConstants` class. The pattern is:

1. Error code — add to `CommonErrorCodes` if broadly applicable, otherwise to the project's `*ErrorCodes` (e.g. `DvizhErrorCodes`).
2. Error message — add to the project's `*ErrorMessages` (e.g. `DvizhErrorMessages`). Messages must be informative enough to understand what went wrong and on what data, not generic placeholders.
3. Factory method — add to the project's `*ResultConstants` (e.g. `DvizhResultConstants`) which calls `Failure` internally using the constants above.

Use cases call only the factory methods, never `Failure` directly.

# Configuration

Secrets and sensitive values in `appsettings.json` or environment variables use `SPECIFY_VALUE` as placeholder — never commit real values.

# EF Core

Schema is set via `HasDefaultSchema` in the `DbContext`. Never specify schema in `IEntityTypeConfiguration` — omit schema from `ToTable(...)` calls entirely.

```csharp
// DbContext
modelBuilder.HasDefaultSchema("MySchema");

// IEntityTypeConfiguration
builder.ToTable("Users"); // no schema argument
```

# SQL Server (additions)

All enum columns use `int` type.

SQL migration scripts belong in the service's Infrastructure project under a `scripts/` folder (e.g. `Information.Infrastructure/scripts/script.sql`).
