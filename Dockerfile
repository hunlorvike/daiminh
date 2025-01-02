# Stage 1: Build .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY *.sln .
COPY web/*.csproj ./web/
COPY core/*.csproj ./core/
COPY infrastructure/*.csproj ./infrastructure/
COPY tests/*.csproj ./tests/

# Restore as distinct layers
RUN dotnet restore "web/web.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR /src/web
RUN dotnet publish "web.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Expose port
EXPOSE 80

# Start application
ENTRYPOINT ["dotnet", "web.dll"]