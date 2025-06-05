# 🚀 Dự án SONDAIMINH - Hệ thống Quản lý Bán Sơn & Thi công Chống thấm

## 🌟 Tổng quan

**SONDAIMINH** là một ứng dụng web ASP.NET Core được thiết kế để quản lý toàn diện các hoạt động kinh doanh liên quan đến sản phẩm sơn và dịch vụ thi công chống thấm. Hệ thống bao gồm:

*   **Trang Quản trị (Admin Area):**
    *   Quản lý Sản phẩm: Thêm, sửa, xóa sản phẩm, danh mục, thương hiệu, thuộc tính, hình ảnh.
    *   Quản lý Nội dung: Quản lý bài viết, trang tĩnh, banner, popup, FAQ.
    *   Quản lý Tương tác: Xử lý liên hệ, quản lý đánh giá khách hàng, đăng ký nhận tin.
    *   Quản lý Người dùng: Quản lý tài khoản quản trị viên, phân quyền chi tiết.
    *   Quản lý Cấu hình hệ thống.
    *   Quản lý Media: Tải lên, quản lý hình ảnh và các tệp đa phương tiện.
*   **Trang Khách hàng (Client Area):**
    *   Hiển thị sản phẩm, bài viết, thông tin công ty.
    *   Chức năng tìm kiếm, lọc sản phẩm.
    *   Gửi liên hệ, yêu cầu báo giá.
    *   (Tiềm năng) Đăng ký/Đăng nhập tài khoản khách hàng, quản lý đơn hàng, giỏ hàng.

**Điểm nổi bật:**
*   Kiến trúc phân lớp rõ ràng (Domain, Infrastructure, Shared, Web).
*   Sử dụng ASP.NET Core Identity cho xác thực và phân quyền dựa trên Claim (Permission-based).
*   Tích hợp MinIO cho lưu trữ file object storage.
*   Hỗ trợ cơ sở dữ liệu PostgreSQL (đã chuyển từ SQL Server).
*   Triển khai dễ dàng bằng Docker và Docker Compose.
*   Giao diện quản trị hiện đại, thân thiện với người dùng (dựa trên Tabler).

---

## 🛠️ Công nghệ sử dụng

### Backend
*   ASP.NET Core 8.0 (C#)
*   Entity Framework Core 8.0 (ORM)
*   PostgreSQL (Cơ sở dữ liệu)
*   ASP.NET Core Identity (Xác thực & Phân quyền)
*   AutoMapper (Ánh xạ đối tượng)
*   FluentValidation (Validate dữ liệu)
*   Serilog (Logging)
*   MinIO (Lưu trữ file)
*   Redis (Cache - tùy chọn)
*   X.PagedList (Phân trang)

### Frontend (Admin & Client)
*   Razor Views (.cshtml)
*   HTML5, CSS3, JavaScript
*   Tabler (Admin UI Template)
*   jQuery
*   Bootstrap 5
*   TomSelect (Select box nâng cao)
*   Litepicker (Date picker)
*   HugerTE (WYSIWYG Editor - có thể là TinyMCE)
*   Toastr (Thông báo)

### DevOps & Triển khai
*   Docker & Docker Compose
*   Git

---

## 🏗️ Cấu trúc Dự án

Dự án được tổ chức thành các project chính:

*   `src/domain`: Chứa các Entities (thực thể), enums, và logic nghiệp vụ cốt lõi không phụ thuộc vào công nghệ cụ thể.
*   `src/infrastructure`: Chứa `DbContext`, migrations, repositories, và các triển khai cụ thể cho cơ sở dữ liệu và các dịch vụ bên ngoài (như MinIO).
*   `src/shared`: Chứa các hằng số, enums, helpers, models dùng chung cho các project khác.
*   `src/web`: Dự án ASP.NET Core MVC chính, chứa Controllers, Views, ViewModels, Services (cho tầng application), Mappers, Validators.
    *   `Areas/Admin`: Phân hệ quản trị.
    *   `Areas/Client`: Phân hệ cho người dùng cuối.

---

## ⚙️ Setup Môi trường Development

### Yêu cầu
*   .NET SDK 8.0 hoặc mới hơn.
*   Một IDE như Visual Studio 2022, JetBrains Rider, hoặc VS Code với C# Dev Kit.
*   Docker Desktop.
*   Git.

### Các bước thiết lập

1.  **Clone dự án:**
    ```bash
    git clone https://github.com/hunlorvike/daiminh daiminh
    cd daiminh
    ```

2.  **Cấu hình file `appsettings.Development.json`:**
    Mở file `src/web/appsettings.Development.json` và cập nhật các thông tin sau:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=daiminh_pg;Username=your_postgres_user;Password=your_postgres_password_local_dev", // Thay bằng thông tin PostgreSQL của bạn
        "RedisConnection": "localhost:6380" // Hoặc port Redis bạn dùng
      },
      "Minio": {
        "Endpoint": "localhost:9000",
        "AccessKey": "minioadmin", // Nên dùng key khác cho dev
        "SecretKey": "minioadmin", // Nên dùng secret khác cho dev
        "BucketName": "daiminh-dev", // Có thể dùng bucket riêng cho dev
        "PublicBaseUrl": "http://localhost:9000/daiminh-dev",
        "UseSSL": false
      },
      // ... các cấu hình khác
    }
    ```
    *   **Lưu ý:** `your_postgres_user` và `your_postgres_password_local_dev` cần khớp với cấu hình trong `docker-compose.yml` hoặc PostgreSQL server cục bộ của bạn.

3.  **Chạy các services phụ thuộc (PostgreSQL, MinIO, Redis) bằng Docker Compose:**
    Đảm bảo bạn đang ở thư mục gốc của dự án (nơi chứa file `docker-compose.yml`).
    ```bash
    docker compose up -d postgresql minio redis
    ```
    *   Lệnh này sẽ khởi tạo PostgreSQL, MinIO và Redis dựa trên cấu hình trong `docker-compose.yml`.
    *   **Lần đầu chạy MinIO:** Truy cập MinIO Console (thường là `http://localhost:9001`) và tạo bucket có tên trùng với `Minio:BucketName` trong `appsettings.Development.json` (ví dụ: `daiminh-dev`).

4.  **Áp dụng Migrations và Seed Data:**
    *   Mở terminal trong thư mục `src/web`.
    *   Áp dụng migrations để tạo schema cho PostgreSQL:
        ```bash
        dotnet ef database update
        ```
    *   Ứng dụng được cấu hình để tự động seed data khi khởi chạy lần đầu (thông qua `ApplicationDataSeeder`).

5.  **Chạy ứng dụng web:**
    *   Bạn có thể chạy từ IDE (Visual Studio, Rider) bằng cách nhấn nút Run/Debug.
    *   Hoặc chạy từ terminal trong thư mục `src/web`:
        ```bash
        dotnet run
        ```
    *   Truy cập ứng dụng qua URL được hiển thị (thường là `http://localhost:5011` hoặc `https://localhost:7172`).
    *   **Tài khoản Admin mặc định (sau khi seed data):**
        *   Username: `superadmin` hoặc `admin`
        *   Password: (Xem trong `UserAndRoleSeeder.cs` - ví dụ: `SuperAdmin123!` hoặc `Password123!`)

---

## 🚀 Hướng dẫn Triển khai (Deployment)

### 1. Build Docker Image
Tại thư mục gốc của dự án (chứa `Dockerfile`):
```bash
docker build -t yourusername/daiminh-web:latest .
```
Thay `yourusername/daiminh-web:latest` bằng tên image và tag bạn muốn.

### 2. Đẩy Image lên Docker Registry (ví dụ: Docker Hub)
```bash
docker login
docker push yourusername/daiminh-web:latest
```

### 3. Chuẩn bị Server
*   Cài đặt Docker và Docker Compose trên server Linux (Ubuntu 24.04).
*   Đảm bảo server có đủ tài nguyên (CPU: 2 vCPU, RAM: 4 GB, HDD: 35 GB).
*   Cấu hình firewall để cho phép truy cập vào các port cần thiết (ví dụ: 80, 443).

### 4. Cấu hình `docker-compose.yml` trên Server
Sao chép file `docker-compose.yml` lên server. Chỉnh sửa các biến môi trường cho phù hợp với môi trường production, đặc biệt là:
*   `image` của service `web-app` phải là tên image bạn đã push lên registry.
*   **Connection Strings và Secrets:**
    *   **KHÔNG BAO GIỜ** hardcode credentials production trực tiếp vào `docker-compose.yml`.
    *   Sử dụng Docker Secrets, file `.env` (được `.gitignore` và chỉ tồn tại trên server), hoặc các biến môi trường được thiết lập trực tiếp trên server.
    *   Ví dụ sử dụng file `.env`:
        Tạo file `.env` cùng cấp với `docker-compose.yml` trên server:
        ```env
        DB_USER=prod_db_user
        DB_PASSWORD=SuperSecurePassword123!
        DB_NAME=daiminh_prod
        MINIO_ACCESS_KEY=prod_minio_key
        MINIO_SECRET_KEY=SuperSecureMinioSecret!
        MINIO_BUCKET_NAME=daiminh-prod
        REDIS_CONNECTION_STRING=prod_redis_host:6379
        # ... các biến khác
        ```
        Sau đó trong `docker-compose.yml`:
        ```yaml
        # ...
        services:
          web-app:
            # ...
            environment:
              - ConnectionStrings__DefaultConnection=Host=postgresql;Port=5432;Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASSWORD}
              - Minio__Endpoint=http://minio:9000 # Minio service name
              - Minio__AccessKey=${MINIO_ACCESS_KEY}
              # ...
            # ...
          postgresql:
            environment:
              POSTGRES_USER: ${DB_USER}
              POSTGRES_PASSWORD: ${DB_PASSWORD}
              POSTGRES_DB: ${DB_NAME}
            # ...
        # ...
        ```

*   **Volumes:** Đảm bảo mapping volume cho PostgreSQL, MinIO, Redis để dữ liệu được lưu trữ bền vững.
*   **Networking:** Đảm bảo tất cả services trong cùng một Docker network.
*   **Reverse Proxy (Nginx, Traefik, Caddy):**
    *   **Rất khuyến nghị** sử dụng một reverse proxy phía trước ứng dụng ASP.NET Core.
    *   Reverse proxy sẽ xử lý:
        *   SSL/TLS termination (HTTPS).
        *   Load balancing (nếu có nhiều instance).
        *   Caching tĩnh.
        *   Nén HTTP.
    *   Ứng dụng ASP.NET Core bên trong container sẽ chạy trên HTTP (port 80).
    *   Ví dụ cấu hình Nginx đơn giản (tạo file trong `/etc/nginx/sites-available/daiminh`):
        ```nginx
        server {
            listen 80;
            listen [::]:80;
            server_name yourdomain.com www.yourdomain.com; # Thay bằng domain của bạn

            # Chuyển hướng HTTP sang HTTPS
            location / {
                return 301 https://$host$request_uri;
            }
        }

        server {
            listen 443 ssl http2;
            listen [::]:443 ssl http2;
            server_name yourdomain.com www.yourdomain.com;

            ssl_certificate /etc/letsencrypt/live/yourdomain.com/fullchain.pem; # Đường dẫn certificate
            ssl_certificate_key /etc/letsencrypt/live/yourdomain.com/privkey.pem; # Đường dẫn private key
            # Thêm các cấu hình SSL khác nếu cần

            location / {
                proxy_pass http://localhost:DOCKER_HOST_PORT; # Thay DOCKER_HOST_PORT bằng port bạn map cho web-app trong docker-compose
                proxy_http_version 1.1;
                proxy_set_header Upgrade $http_upgrade;
                proxy_set_header Connection keep-alive;
                proxy_set_header Host $host;
                proxy_cache_bypass $http_upgrade;
                proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
                proxy_set_header X-Forwarded-Proto $scheme;
            }
        }
        ```
        Sau đó tạo symlink: `sudo ln -s /etc/nginx/sites-available/daiminh /etc/nginx/sites-enabled/` và restart Nginx: `sudo systemctl restart nginx`.
        Sử dụng Let's Encrypt (Certbot) để lấy SSL certificate miễn phí.

### 5. Chạy ứng dụng trên Server
Trên server, trong thư mục chứa `docker-compose.yml`:
```bash
docker compose pull # Kéo image mới nhất (nếu đã có thay đổi)
docker compose up -d
```
Lệnh này sẽ khởi động ứng dụng web cùng với PostgreSQL, MinIO và Redis.

---

## 🧱 Câu lệnh Entity Framework Core

Luôn chạy các lệnh EF Core từ thư mục `src/web` (hoặc thư mục chứa project startup) và chỉ định project chứa DbContext (`infrastructure`).

1.  **Thêm một Migration:**
    Tạo một tệp migration mới ghi lại các thay đổi trong model.
    ```bash
    dotnet ef migrations add TenMigrationCuaBan --project ../infrastructure --startup-project .
    ```
    Hoặc từ thư mục gốc:
    ```bash
    dotnet ef migrations add TenMigrationCuaBan --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    ```

2.  **Áp dụng Migration vào Database:**
    ```bash
    dotnet ef database update --project ../infrastructure --startup-project .
    ```
    Hoặc từ thư mục gốc:
    ```bash
    dotnet ef database update --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    ```

3.  **Xóa Migration Cuối cùng (nếu cần):**
    ```bash
    dotnet ef migrations remove --project ../infrastructure --startup-project .
    ```
    Hoặc từ thư_mục_gốc:
    ```bash
    dotnet ef migrations remove --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    ```

### Cài đặt EF Core Tools (nếu chưa có)
```bash
dotnet tool install --global dotnet-ef
# Hoặc cập nhật:
# dotnet tool update --global dotnet-ef
```

---