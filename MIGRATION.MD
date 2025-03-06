## Database Migrations

### Option 1: Using .NET CLI

1. Install the EF Core CLI tools globally (if not already installed):

```bash
dotnet tool install --global dotnet-ef
```

2. Create a new migration:

```bash
# Create migration 
dotnet ef migrations add [MigrationName] --project ./infrastructure/infrastructure.csproj --startup-project ./web/web.csproj -o Data/Migrations
```

3. Apply migrations:

```bash
# Apply pending migrations
dotnet ef database update --project ./infrastructure/infrastructure.csproj --startup-project ./web/web.csproj
```

4. Remove migrations:

```bash
# Remove migrations
dotnet ef migrations remove --project ./infrastructure/infrastructure.csproj  --startup-project ./web/web.csproj
```