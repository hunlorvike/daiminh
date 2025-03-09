# Daiminh

A .NET 8 web application with a multi-layer architecture.

## Project Structure

```text
daiminh/
├── docs/                     # Documentation files
│   ├── DATABASE.MD           # Database schema information
│   └── database.mermaid      # Mermaid diagram of the database
├── src/                      # Source code
│   ├── application/          # Application layer (business logic, services)
│   │   ├── Services/         # Application services
│   │   │   ├── AuthService.cs
│   │   │   ├── CategoryService.cs
│   │   │   ├── ... (other services)
│   │   │   └── UserService.cs
│   │   └── application.csproj
│   ├── domain/               # Domain layer (entities, core business rules)
│   │   ├── Entities/         # Domain entities
│   │   │   ├── Shared/       # Shared entities
│   │   │   ├── Category.cs
│   │   │   ├── ... (other entities)
│   │   │   └── User.cs
│   ├── infrastructure/       # Infrastructure layer (data access, external services)
│   │   ├── Migrations/       # Database migrations
│   │   ├── Repositories/     # Data repositories
│   │   ├── UnitOfWork/       # Unit of Work implementation
│   │   ├── ApplicationDbContext.cs
│   │   ├── AuditSaveChangesInterceptor.cs
│   │   └── infrastructure.csproj
│   ├── shared/               # Shared components, utilities, and models
│   │   ├── Attributes/       # Custom attributes
│   │   ├── Constants/        # Constants
│   │   ├── Enums/            # Enumerations
│   │   ├── Extensions/       # Extension methods
│   │   ├── Interfaces/       # Shared interfaces
│   │   ├── Models/           # Shared models (e.g., DTOs, ViewModels)
│   │   └── shared.csproj
│   └── web/                  # Presentation layer (ASP.NET Core Web App)
│       ├── Areas/            # Areas (e.g., Admin, Client)
│       │   ├── Admin/        # Admin area
│       │   │   ├── Controllers/
│       │   │   ├── Models/
│       │   │   ├── Profiles/      # AutoMapper Profiles
│       │   │   ├── Requests/      # Request Models
│       │   │   └── Views/
│       │   └── Client/       # Client area
│       │       ├── Controllers/
│       │       ├── Models/
│       │       ├── Profiles/      # AutoMapper Profiles
│       │       ├── Requests/      # Request Models
│       │       └── Views/
│       ├── Properties/
│       ├── wwwroot/          # Static files (CSS, JavaScript, images)
│       ├── appsettings.Development.json
│       ├── appsettings.json
│       ├── Program.cs
│       └── web.csproj
├── .dockerignore            # Files/folders to ignore in Docker builds
├── .editorconfig           # Editor configuration
├── .gitignore              # Files/folders to ignore in Git
├── daiminh.sln             # Solution file
├── docker-compose.yml      # Docker Compose configuration
├── Dockerfile              # Dockerfile for building the application image
└── README.MD               # This file
```

## Prerequisites

Before you begin, ensure you have the following installed:

-   [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Docker](https://www.docker.com/products/docker-desktop/)
-   [Docker Compose](https://docs.docker.com/compose/install/)

---

## Local Development Setup

### 1. Clone the Repository

```bash
git clone https://github.com/hunlorvike/daiminh.git daiminh
cd daiminh
```

### 2. Setup and Run the Project

#### **Option 1: Using Docker Compose (Recommended)**

1.  **Start the Services (database, application, etc.)**  
    Run the following command to start all necessary containers:

    ```bash
    docker compose up -d
    ```

2.  **Apply Database Migrations**  
    Run the following command to update the database:
    
    ```bash
     dotnet ef database update --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    ```
3. **Build and Run**  
   Build and Run application:
  ```bash
   dotnet clean daiminh.sln
   dotnet restore daiminh.sln
   dotnet build daiminh.sln
   dotnet run --project ./src/web/web.csproj
   ```

---

#### **Option 2: Running Manually without Docker**

If you don't want to use Docker, you can run the project manually:

1.  **Configure the Database**  
    -   Ensure you have PostgreSQL installed.  
    -   Update the connection string in `src/web/appsettings.json`.

2.  **Apply Database Migrations**  
    ```bash
    dotnet ef database update --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    ```

3.  **Run the Application**  
    ```bash
    dotnet run --project ./src/web/web.csproj
    ```

---

## Notes

-   If you encounter errors related to Entity Framework, ensure you have the `dotnet-ef` tool installed globally:

    ```bash
    dotnet tool install --global dotnet-ef
    ```

-   If you make changes to the migrations, run the following commands:

    ```bash
    dotnet ef migrations add <MigrationName> --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    dotnet ef database update --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    ```

Happy coding! 🚀
