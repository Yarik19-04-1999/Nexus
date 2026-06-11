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
