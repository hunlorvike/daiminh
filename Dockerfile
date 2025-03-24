# Sử dụng .NET SDK 8.0 làm base image để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Thiết lập thư mục làm việc trong container
WORKDIR /app

# Copy toàn bộ solution và project files
COPY ["daiminh.sln", "./"]
COPY ["src/domain/domain.csproj", "src/domain/"]
COPY ["src/infrastructure/infrastructure.csproj", "src/infrastructure/"]
COPY ["src/shared/shared.csproj", "src/shared/"]
COPY ["src/web/web.csproj", "src/web/"]

# Restore các dependencies
RUN dotnet restore

# Copy toàn bộ source code
COPY . .

# Build ứng dụng
RUN dotnet build -c Release -o /app/build

# Publish ứng dụng
RUN dotnet publish -c Release -o /app/publish

# Sử dụng .NET Runtime 8.0 làm base image cho production
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Thiết lập thư mục làm việc
WORKDIR /app

# Copy các file đã publish từ build stage
COPY --from=build /app/publish .

# Expose port 80 cho ứng dụng
EXPOSE 80

# Thiết lập entry point cho container
ENTRYPOINT ["dotnet", "web.dll"]
