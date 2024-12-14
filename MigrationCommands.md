# Migrations

## 1. Using the CLI

### Add a migration
```bash
dotnet ef migrations add [MigrationName] --project HotelApp.WebAPI --startup-project HotelApp.WebAPI --context HotelAppDbContext --output-dir Migrations
```

### Update the database
```bash
dotnet ef database update --project HotelApp.WebAPI --startup-project HotelApp.WebAPI --context HotelAppDbContext
```

### Roll back a migration
```bash
dotnet ef database update [MigrationxName] --project HotelApp.WebAPI --startup-project HotelApp.WebAPI --context HotelAppDbContext
```

### Drop the database
```bash
dotnet ef database drop --project HotelApp.WebAPI --startup-project HotelApp.WebAPI --context HotelAppDbContext
```

### Remove a migration
```bash
dotnet ef migrations remove --project HotelApp.WebAPI --startup-project HotelApp.WebAPI --context HotelAppDbContext
```

## 2. Using the Package Manager Console
### Add a migration
```bash
Add-Migration [MigrationName] -Project HotelApp.WebAPI -StartupProject HotelApp.WebAPI -Context HotelAppDbContext -OutputDir HotelApp.WebAPI/Migrations
```

### Update the database
```bash
Update-Database -Project HotelApp.WebAPI -StartupProject HotelApp.WebAPI -Context HotelAppDbContext
```

### Roll back a migration
```bash
Update-Database [MigrationName] -Project HotelApp.WebAPI -StartupProject HotelApp.WebAPI -Context HotelAppDbContext
```

### Remove a migration
```bash
Remove-Migration -Project HotelApp.WebAPI -StartupProject HotelApp.WebAPI -Context HotelAppDbContext
```