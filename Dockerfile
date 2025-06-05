# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first to leverage Docker layer caching
COPY ["daiminh.sln", "."]
COPY ["src/domain/domain.csproj", "src/domain/"]
COPY ["src/infrastructure/infrastructure.csproj", "src/infrastructure/"]
COPY ["src/shared/shared.csproj", "src/shared/"]
COPY ["src/web/web.csproj", "src/web/"]
# Nếu có các project test hoặc project khác, cũng copy file .csproj của chúng vào đây
# Ví dụ: COPY ["tests/MyProject.Tests/MyProject.Tests.csproj", "tests/MyProject.Tests/"]

# Restore dependencies
RUN dotnet restore "daiminh.sln"

# Copy the rest of the source code
COPY . .

# Build and publish the web project in Release configuration
WORKDIR "/src/src/web"
ENV DOCKER_BUILD=true
RUN dotnet build "web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for the runtime
# IMPORTANT: Ensure ASPNETCORE_ENVIRONMENT is set to Production for security and performance
ENV ASPNETCORE_ENVIRONMENT=Production
# ASP.NET Core will listen on port 80 inside the container
ENV ASPNETCORE_URLS=http://+:80
# (Optional) Configure other environment variables if needed, e.g., connection strings
# ENV ConnectionStrings__DefaultConnection="your_production_connection_string"
# ENV Minio__Endpoint="your_minio_endpoint"
# ... other ENV variables

# Expose port 80 for the Kestrel web server
EXPOSE 80

# Entry point to run the application
ENTRYPOINT ["dotnet", "web.dll"]