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

Primary key constraint naming: `[TableName$PK]`.

```sql
constraint [Invites$PK] primary key clustered (Id)
```

Foreign key constraint naming: `[ChildTable(FKCol)->ParentTable(PKCol)]`.

```sql
constraint [InviteEvents(InviteId)->Invites(Id)] foreign key (InviteId) references Dvizh.Invites(Id)
```

# Controllers

Use `[FromServices]` to inject use case dependencies directly on action method parameters, not via constructor injection.

```csharp
public async Task<IActionResult> GetById(
    int id,
    [FromServices] IGetInviteByIdUseCase useCase,
    CancellationToken cancellationToken)
```

Decorate controllers with `[ApiController]` and `[NexusRoute]`. `[NexusRoute]` produces `api/v{version}/[controller]`.

# Mapperly

All mappers use Riok.Mapperly. No manual static mapping code.

All mapper methods are named `Map` regardless of direction or purpose.

Request→input mappers (Api layer) and input→entity mappers (Application layer) use `RequiredMappingStrategy.Source`.

Response mappers (model→response, Api layer) use `RequiredMappingStrategy.Target`.

```csharp
// Request → Input or Input → Entity
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class CreateUniverseRequestMapper
{
    public static partial CreateUniverseInput Map(CreateUniverseRequest request);
}

// Model → Response
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetUniverseByIdResponseMapper
{
    public static partial GetUniverseByIdResponse Map(Universe universe);
}
```

When the route contributes a field not present in the request body (e.g. `id` on a PUT), use a non-partial wrapper that calls a private partial method and fills in the missing field:

```csharp
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class UpdateMovieRequestMapper
{
    public static UpdateMovieInput Map(int id, UpdateMovieRequest request)
    {
        var input = MapFromRequest(request);
        return input with { Id = id };
    }

    [MapperIgnoreTarget(nameof(UpdateMovieInput.Id))]
    private static partial UpdateMovieInput MapFromRequest(UpdateMovieRequest request);
}
```

For paged-result response mappers, use a non-partial wrapper for the `PagedResult<T>` overload and a `partial` method for the item:

```csharp
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetMoviesResponseMapper
{
    public static GetMoviesResponse Map(PagedResult<Movie> result)
        => new(result.Items.Select(Map).ToList(), result.TotalCount, result.Page, result.PageSize, result.TotalPages);

    public static partial GetMovieItemResponse Map(Movie movie);
}
```

Always add `ExcludeAssets="runtime"` to the Riok.Mapperly package reference — it is a source generator and must not be included in the output.

```xml
<PackageReference Include="Riok.Mapperly" ExcludeAssets="runtime" />
```

# Error codes

Error code string values use PascalCase: `"NotFound"`, `"AlreadyAnswered"`. Never SCREAMING_SNAKE_CASE.

# Packages

To add a new NuGet package:
1. Add `<PackageVersion Include="PackageName" Version="x.y.z" />` to the root `Directory.Packages.props`.
2. Add `<PackageReference Include="PackageName" />` (no `Version` attribute) to the relevant `.csproj`.

Never add `Version` attributes to `<PackageReference>` elements in `.csproj` files.

# Naming

Input DTOs live in a folder named `Inputs` (plural): `Models/Inputs/XxxInput.cs`.

Enum types live in `Models/Enums/`: `Models/Enums/RewatchStatus.cs` (namespace `*.Models.Enums`).

Non-entity result types (e.g. search results) live in `Models/Results/`: `Models/Results/SearchMovieResult.cs` (namespace `*.Models.Results`).
