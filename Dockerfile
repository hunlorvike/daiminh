# Sử dụng .NET SDK 8.0 làm base image để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Cài đặt LibMan
RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli

# Thêm dotnet tools vào PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

# Sao chép toàn bộ solution và các project files
COPY *.sln ./
COPY src/domain/*.csproj ./src/domain/
COPY src/infrastructure/*.csproj ./src/infrastructure/
COPY src/shared/*.csproj ./src/shared/
COPY src/web/*.csproj ./src/web/
COPY tests/domain.tests/*.csproj ./tests/domain.tests/
COPY tests/infrastructure.tests/*.csproj ./tests/infrastructure.tests/
COPY tests/web.tests/*.csproj ./tests/web.tests/

# Restore packages
RUN dotnet restore

# Copy toàn bộ source code
COPY . .

# Chạy LibMan restore để tải các thư viện client-side
WORKDIR /app/src/web
RUN libman clean
RUN libman restore

# Quay lại thư mục app
WORKDIR /app

# Build và publish
RUN dotnet build -c Release -o /app/build
RUN dotnet publish src/web/web.csproj -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy các file đã publish
COPY --from=build /app/publish .

# Thiết lập biến môi trường
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Expose port
EXPOSE 80

# Thiết lập healthcheck
HEALTHCHECK --interval=30s --timeout=3s \
    CMD curl -f http://localhost/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "web.dll"]