## Database Migrations

### Option 1: Using Visual Studio Package Manager Console

1. Set the `infrastructure` project as the default project in Package Manager Console
2. Run the following commands:

```powershell
# Create a new migration
Add-Migration [MigrationName] -Project infrastructure

# Apply migration to database
Update-Database -Project infrastructure
```

### Option 2: Using .NET CLI

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

### Option 3: Using Docker (For Development)

1. Create a migration:

```bash
docker compose exec web dotnet ef migrations add [MigrationName] --project ../infrastructure/infrastructure.csproj
```

2. Apply migrations:

```bash
docker compose exec web dotnet ef database update --project ../infrastructure/infrastructure.csproj
```

### Common Migration Commands

```bash
# Remove the last migration (if not applied to database)
dotnet ef migrations remove --startup-project ../web/web.csproj

# List all migrations
dotnet ef migrations list --startup-project ../web/web.csproj

# Generate SQL script
dotnet ef migrations script --startup-project ../web/web.csproj

# Drop the database
dotnet ef database drop --startup-project ../web/web.csproj

# Revert to a specific migration
dotnet ef database update [MigrationName] --startup-project ../web/web.csproj
```

### Best Practices

1. Always create meaningful migration names (e.g., `AddUserTable`, `UpdateProductSchema`)
2. Review migration files before applying them
3. Test migrations on a development database first
4. Keep migrations small and focused
5. Include both Up() and Down() methods for reversibility
6. Use data migrations for seed data when needed

### Troubleshooting Migrations

1. If migration fails:
    - Check the database connection string
    - Ensure database user has sufficient permissions
    - Review migration SQL script before applying
    - Check for pending model changes

2. If migrations conflict:
    - Remove last migration if not applied
    - Update to latest successful migration
    - Create new migration with all pending changes
