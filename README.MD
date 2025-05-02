# 🚀 Dự án SONDAIMINH

## 🧩 Tổng quan

Hệ thống quản lý bán sơn và thi công chống thấm, gồm:
- Quản lý sản phẩm, thuộc tính, biến thể
- Tích hợp MinIO để lưu trữ media (ảnh/video)
- Kết nối SQL Server qua Docker
- Hệ thống được container hóa hoàn toàn bằng Docker

---

## ⚙️ Cấu trúc Docker Compose

```bash
docker-compose.db.yml         # Chạy SQL Server
docker-compose.minio.yml      # Chạy MinIO object storage
docker-compose.web.yml        # Chạy ASP.NET Web Application
```

---

## 🛠️ Setup để chạy dự án

### 1. Clone dự án và cấu hình file

```bash
git clone https://github.com/hunlorvike/daiminh daiminh
cd daiminh
```

### 2. Tạo file cấu hình môi trường

- appsettings.json
- appsettings.Development.json
- appsettings.Production.json

> Đã có sẵn các file mẫu.

### 3. Chạy MinIO và SQL Server

```bash
docker compose -f docker-compose.db.yml up -d
docker compose -f docker-compose.minio.yml up -d
```

### 4. Build image và đẩy lên Docker Hub

```bash
docker build -t hungcutedethuongg/daiminh-web:latest .
docker push hungcutedethuongg/daiminh-web:latest
```

### 5. Trên server, pull và chạy web

```bash
docker compose -f docker-compose.web.yml pull
docker compose -f docker-compose.web.yml up -d
```

---

## 🧱 Câu lệnh Entity Framework Core

1.  **Thêm một Migration:**
    Tạo một tệp migration mới ghi lại các thay đổi.
    ```bash
    dotnet ef migrations add <TênMigrationCủaBạn> --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    ```

2.  **Áp dụng Migration:**
    Áp dụng các migration đang chờ xử lý vào cơ sở dữ liệu.
    ```bash
    dotnet ef database update --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    ```

3.  **Xóa Migration Cuối cùng (nếu cần):**
    ```bash
    dotnet ef migrations remove --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    ```

### Cấu hình EF nếu chưa có

Cài tool:
```bash
dotnet tool install --global dotnet-ef
```

Thêm `ItemGroup` trong `.csproj` nếu cần:

```xml
<ItemGroup>
  <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="8.0.0" />
</ItemGroup>
```

---

## 🧠 Ghi chú

- Môi trường production sử dụng domain: `cdn.yourdomain.com` proxy về `minio`
- ASP.NET Core sử dụng `ASPNETCORE_ENVIRONMENT=Production` trong docker-compose