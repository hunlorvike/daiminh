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
├── libman.json             # LibMan configuration file (for client-side libraries)
└── README.MD               # This file
```

## Prerequisites

Before you begin, ensure you have the following installed:

-   [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Docker](https://www.docker.com/products/docker-desktop/)
-   [Docker Compose](https://docs.docker.com/compose/install/)
-   (Optional, but recommended) LibMan CLI:  `dotnet tool install -g Microsoft.Web.LibraryManager.Cli`

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

3. **Clean, Restore, Build, and Run**

   ```bash
   dotnet clean daiminh.sln
   dotnet restore daiminh.sln
   dotnet build daiminh.sln
   libman clean  # Clean client-side libraries
   libman restore # Restore client-side libraries
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

3. **Clean, Restore, Build, and Run**

   ```bash
    dotnet clean daiminh.sln       # Clean the solution
    dotnet restore daiminh.sln    # Restore NuGet packages
    dotnet build daiminh.sln       # Build the solution
    libman clean                  # Clean client-side libraries
    libman restore                # Restore client-side libraries (using libman.json)
    dotnet run --project ./src/web/web.csproj
   ```

---

##  Managing Client-Side Libraries (LibMan)

This project uses LibMan to manage client-side libraries (JavaScript, CSS, etc.).  The `libman.json` file at the root of the project defines these libraries.

- **Restore Libraries:**
  ```bash
  libman restore
  ```
  This command downloads and installs the libraries specified in `libman.json` into the `wwwroot` folder.

- **Clean Libraries:**
  ```bash
  libman clean
  ```
  This command removes all library files managed by LibMan from the `wwwroot` folder.  It's a good practice to run this before restoring to ensure you have a clean slate.

- **Update a Specific Library:**
  ```bash
  libman update <library-name>
  ```
  This command updates a specific library to the latest version (or the version specified in `libman.json`).

- **Install a New Library:**
    -  You can use the LibMan CLI:
        ```bash
        libman install <library-name>@<version> -p <provider> -d <destination>
        ```
        (e.g., `libman install jquery@3.7.1 -p unpkg -d wwwroot/lib/jquery`)
    -  Or, you can directly edit the `libman.json` file and then run `libman restore`.  Visual Studio also provides a GUI for managing LibMan.

**Important:** Always run `libman restore` after cloning the repository, pulling changes, or modifying `libman.json`.  This ensures your `wwwroot` folder contains the correct library files.

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
