# Daiminh

A .NET 8 web application with a multi-layer architecture.

## Project Structure

```
daiminh/
├── core/                 # Core business logic and domain models
├── infrastructure/       # Data access and external services
├── web/                 # MVC web application
└── tests/               # Test projects
```

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Local Development Setup

### 1. Clone the Repository

```bash
git clone https://github.com/hunlorvike/daiminh.git
cd daiminh
```

### 2. Development Environment

#### Option 1: Using Docker Compose (Recommended)

1. Start the application and its dependencies:

```bash
docker compose up -d
```

The application will be available at `http://localhost:8080`

#### Option 2: Local Development

1. Install dependencies:

```bash
dotnet restore
```

2. Start PostgreSQL and Redis using Docker:

```bash
docker compose up db redis -d
```

3. Update the connection strings in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "Postgres": "Host=localhost;Port=5432;Database=daiminh;Username=user;Password=password"
  },
  "Redis": {
    "Connection": "localhost:6379"
  }
}
```

4. Run the application:

```bash
cd web
dotnet run
```

## Environment Variables

The application uses the following environment variables:

- `ASPNETCORE_ENVIRONMENT`: Application environment (Development/Production)
- `ConnectionStrings__Postgres`: PostgreSQL connection string
- `Redis__Connection`: Redis connection string

## Docker Services

- **Web Application**: ASP.NET Core MVC application
    - Port: 8080
    - URL: http://localhost:8080

- **PostgreSQL Database**:
    - Port: 5432
    - Database: daiminh
    - Username: user
    - Password: password

- **Redis Cache**:
    - Port: 6379

## Development Workflow

1. Make changes to the code
2. Build and test locally:

```bash
dotnet build
dotnet test
```

3. Run with Docker Compose:

```bash
docker compose up --build
```

## Useful Commands

### Docker Compose

- Start all services:

```bash
docker compose up -d
```

- Stop all services:

```bash
docker compose down
```

- View logs:

```bash
docker compose logs -f
```

### Database

- Access PostgreSQL:

```bash
docker compose exec db psql -U user -d daiminh
```

### Application

- Clean solution:

```bash
dotnet clean
```

- Build solution:

```bash
dotnet build
```

- Run tests:

```bash
dotnet test
```

## Troubleshooting

1. If the application can't connect to PostgreSQL:
    - Ensure PostgreSQL container is running: `docker compose ps`
    - Check logs: `docker compose logs db`
    - Verify connection string settings

2. If Redis connection fails:
    - Check Redis container status: `docker compose ps`
    - Verify Redis logs: `docker compose logs redis`

3. If the web application fails to start:
    - Check application logs: `docker compose logs web`
    - Verify environment variables in docker-compose.yml

## Contributing

1. Create a new branch
2. Make your changes
3. Submit a pull request

