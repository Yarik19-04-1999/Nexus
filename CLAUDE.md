# SQL Server

Always pass stored procedure parameters by name, never by position.

```sql
exec dbo.ConfigureSchema @schemaName = 'Lore', @passwordHash = 0x0200...
```
