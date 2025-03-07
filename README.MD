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

Trước khi bắt đầu, hãy đảm bảo bạn đã cài đặt:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/)
- [Docker Compose](https://docs.docker.com/compose/install/)

---

## Local Development Setup

### 1. Clone the Repository

```bash
git clone https://github.com/hunlorvike/daiminh.git daiminh
cd daiminh
```

### 2. Setup và Chạy Dự Án

#### **Option 1: Sử dụng Docker Compose (Recommended)**

1. **Khởi động các dịch vụ (database, ứng dụng, etc.)**  
   Chạy lệnh sau để khởi động tất cả container cần thiết:

   ```bash
   docker compose up -d
   ```

2. **Apply Database Migrations**  
   Chạy lệnh sau để cập nhật database:

   ```bash
   dotnet ef database update --project ./infrastructure/infrastructure.csproj --startup-project ./web/web.csproj
   ```

3. **Build và Chạy Ứng Dụng**  
   Chạy các lệnh sau để build và chạy ứng dụng:

   ```bash
   dotnet clean daiminh.sln
   dotnet restore daiminh.sln
   dotnet build daiminh.sln
   dotnet run --project ./web/web.csproj
   ```
---

#### **Option 2: Chạy thủ công không dùng Docker**

Nếu bạn không muốn sử dụng Docker, có thể chạy thủ công như sau:

1. **Cấu hình database**  
   - Đảm bảo bạn đã cài đặt PostgreSQL.  
   - Cập nhật connection string trong `appsettings.json`.

2. **Apply Database Migrations**  
   ```bash
   dotnet ef database update --project ./infrastructure/infrastructure.csproj --startup-project ./web/web.csproj
   ```

3. **Chạy ứng dụng**  
   ```bash
   dotnet run --project ./web/web.csproj
   ```

---

## Lưu ý

- Nếu gặp lỗi liên quan đến Entity Framework, hãy đảm bảo bạn đã cài đặt công cụ `dotnet-ef` bằng lệnh:

  ```bash
  dotnet tool install --global dotnet-ef
  ```

- Nếu có thay đổi trong migrations, chạy lại:

  ```bash
  dotnet ef migrations add <MigrationName> --project ./infrastructure/infrastructure.csproj --startup-project ./web/web.csproj
  dotnet ef database update --project ./infrastructure/infrastructure.csproj --startup-project ./web/web.csproj
  ```

Chúc bạn coding vui vẻ! 🚀