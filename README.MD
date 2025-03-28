# Daiminh

Daiminh là một ứng dụng web .NET 8 thể hiện mô hình kiến trúc đa tầng.

## Cấu trúc Dự án

Dự án tuân theo cách tiếp cận Clean Architecture / Đa tầng tiêu chuẩn:

```text
daiminh/
├── docs/                     # Các tệp tài liệu
│   ├── DATABASE.MD           # Thông tin lược đồ cơ sở dữ liệu
│   └── database.mermaid      # Sơ đồ Mermaid của cơ sở dữ liệu
├── src/                      # Mã nguồn
│   ├── application/          # Lớp Application: Chứa logic nghiệp vụ, dịch vụ, use case. Điều phối các thực thể miền.
│   │   ├── Services/         # Các dịch vụ ứng dụng triển khai luồng nghiệp vụ
│   │   └── application.csproj
│   ├── domain/               # Lớp Domain: Các mô hình, thực thể và quy tắc nghiệp vụ cốt lõi. Độc lập với các lớp khác.
│   │   ├── Entities/         # Các thực thể miền đại diện cho các khái niệm cốt lõi
│   │   └── domain.csproj
│   ├── infrastructure/       # Lớp Infrastructure: Xử lý các mối quan tâm kỹ thuật như truy cập dữ liệu, tích hợp dịch vụ bên ngoài.
│   │   ├── Migrations/       # Các migration cơ sở dữ liệu của Entity Framework Core
│   │   ├── Repositories/     # Các repository truy cập dữ liệu triển khai các interface được định nghĩa trong Application/Domain
│   │   ├── UnitOfWork/       # Triển khai mô hình Unit of Work
│   │   ├── ApplicationDbContext.cs # DbContext của EF Core
│   │   ├── AuditSaveChangesInterceptor.cs # Interceptor SaveChanges của EF Core cho việc kiểm toán
│   │   └── infrastructure.csproj
│   ├── shared/               # Shared Kernel: Các mối quan tâm xuyên suốt, tiện ích chung, DTO, enum được sử dụng bởi nhiều lớp.
│   │   ├── Attributes/       # Các attribute tùy chỉnh
│   │   ├── Constants/        # Các hằng số toàn ứng dụng
│   │   ├── Enums/            # Các Enumeration
│   │   ├── Extensions/       # Các phương thức mở rộng (extension methods)
│   │   ├── Interfaces/       # Các interface dùng chung
│   │   ├── Models/           # Các model dùng chung (DTO, ViewModel, v.v.)
│   │   └── shared.csproj
│   └── web/                  # Lớp Presentation: Ứng dụng Web ASP.NET Core xử lý giao diện người dùng và yêu cầu API.
│       ├── Areas/            # Các khu vực chức năng (ví dụ: Admin, Client)
│       ├── Properties/
│       ├── wwwroot/          # Các tài sản web tĩnh (CSS, JavaScript, hình ảnh)
│       ├── appsettings.Development.json
│       ├── appsettings.json
│       ├── libman.json       # Cấu hình thư viện phía client (LibMan)
│       ├── Program.cs        # Điểm vào ứng dụng và cấu hình dịch vụ
│       └── web.csproj
├── .dockerignore            # Chỉ định các tệp/thư mục cần bỏ qua trong quá trình build Docker
├── .editorconfig           # Định nghĩa kiểu và định dạng code để đảm bảo code nhất quán
├── .gitignore              # Chỉ định các tệp không được theo dõi cố ý mà Git nên bỏ qua
├── daiminh.sln             # Tệp Solution của Visual Studio
├── docker-compose.yml      # Định nghĩa các dịch vụ ứng dụng Docker đa container
├── Dockerfile              # Hướng dẫn để build image Docker của ứng dụng
└── README.MD               # Tệp này (phiên bản tiếng Việt)
```

## Điều kiện tiên quyết

Trước khi bắt đầu, hãy đảm bảo bạn đã cài đặt các công cụ sau:

-   [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Docker](https://www.docker.com/products/docker-desktop/)
-   [Docker Compose](https://docs.docker.com/compose/install/)
-   [Git](https://git-scm.com/downloads)
-   (Tùy chọn, nhưng khuyến nghị) LibMan CLI: `dotnet tool install -g Microsoft.Web.LibraryManager.Cli`
-   (Tùy chọn, cho EF Migrations) EF Core Tools: `dotnet tool install --global dotnet-ef`

---

## Cài đặt Môi trường Phát triển Cục bộ

### 1. Sao chép Repository

```bash
git clone https://github.com/hunlorvike/daiminh.git daiminh
cd daiminh
```

### 2. Chọn Phương pháp Cài đặt của Bạn

#### **Cách 1: Sử dụng Docker Compose (Khuyến nghị)**

Phương pháp này sử dụng Docker để quản lý cơ sở dữ liệu và container ứng dụng.

1.  **Khởi chạy Dịch vụ:**
    Chạy lệnh sau để build các image (nếu cần) và khởi chạy các container được định nghĩa trong `docker-compose.yml` (bao gồm cơ sở dữ liệu PostgreSQL và ứng dụng web) ở chế độ detached (chạy nền).

    ```bash
    docker compose up -d --build
    ```
    *Lưu ý: Chờ một lát để container cơ sở dữ liệu khởi tạo xong.*

2.  **Áp dụng Database Migrations:**
    Chạy lệnh sau từ máy host của bạn để áp dụng các migration của Entity Framework vào cơ sở dữ liệu đang chạy bên trong container Docker. Lệnh này nhắm vào dự án `web` để lấy cấu hình (như chuỗi kết nối) và dự án `infrastructure` nơi chứa các migration.

    ```bash
     dotnet ef database update --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    ```
    *Đảm bảo chuỗi kết nối trong `src/web/appsettings.Development.json` trỏ đúng đến dịch vụ PostgreSQL được định nghĩa trong `docker-compose.yml` (ví dụ: `Host=localhost;Port=5432...` nếu cổng 5432 được map).*

3.  **Truy cập Ứng dụng:**
    Ứng dụng bây giờ sẽ đang chạy. Truy cập nó qua trình duyệt của bạn, thường là tại `http://localhost:8080` (hoặc cổng được map trong `docker-compose.yml` cho dịch vụ `api`).

4.  **Xem Logs (Tùy chọn):**
    ```bash
    docker compose logs -f
    ```

5.  **Dừng Dịch vụ:**
    ```bash
    docker compose down
    ```

---

#### **Cách 2: Chạy Thủ công không cần Docker**

Phương pháp này yêu cầu bạn phải có một instance PostgreSQL đang chạy cục bộ.

1.  **Cấu hình Cơ sở dữ liệu:**
    -   Đảm bảo bạn đã cài đặt và đang chạy PostgreSQL cục bộ.
    -   Cập nhật mục `ConnectionStrings` trong `src/web/appsettings.Development.json` để trỏ đến instance PostgreSQL cục bộ của bạn.

2.  **Áp dụng Database Migrations:**
    Chạy lệnh sau để tạo hoặc cập nhật lược đồ cơ sở dữ liệu:
    ```bash
    dotnet ef database update --project ./src/infrastructure/infrastructure.csproj --startup-project ./src/web/web.csproj
    ```

3.  **Build và Chạy Ứng dụng:**
    Thực thi các lệnh sau để khôi phục các dependency, build và chạy ứng dụng web:

    ```bash
    # Khôi phục các gói NuGet cho solution
    dotnet restore daiminh.sln

    # Khôi phục các thư viện phía client được định nghĩa trong libman.json
    libman restore

    # Build solution
    dotnet build daiminh.sln --no-restore

    # Chạy ứng dụng web
    dotnet run --project ./src/web/web.csproj --no-build
    ```

4.  **Truy cập Ứng dụng:**
    Ứng dụng bây giờ sẽ đang chạy. Truy cập nó qua trình duyệt của bạn, thường là tại `https://localhost:7001` hoặc `http://localhost:5001` (kiểm tra output của console để biết URL chính xác).

---

## Quản lý Thư viện Phía Client (LibMan)

Dự án này sử dụng LibMan (`libman.json`) để quản lý các thư viện front-end (JavaScript, CSS). Các thư viện này được khôi phục vào thư mục `wwwroot`.

-   **Khôi phục Thư viện:** (Tải xuống/cài đặt các thư viện như được định nghĩa trong `libman.json`)
    ```bash
    libman restore
    ```
    *Thường được thực hiện một lần sau khi clone hoặc pull các thay đổi, hoặc sau khi sửa đổi `libman.json`. Lệnh `dotnet restore` cũng có thể kích hoạt hành động này nếu được cấu hình.*

-   **Dọn dẹp Thư viện:** (Xóa các thư viện do LibMan cài đặt khỏi `wwwroot`)
    ```bash
    libman clean
    ```

-   **Cập nhật Thư viện:** (Cập nhật một thư viện cụ thể lên phiên bản mới nhất được phép)
    ```bash
    libman update <tên-thư-viện>
    ```
    *(Ví dụ: `libman update jquery`)*

-   **Cài đặt Thư viện Mới:**
    Sử dụng LibMan CLI hoặc chỉnh sửa trực tiếp `libman.json`.
    ```bash
    # Ví dụ: Cài đặt Bootstrap 5.3.3 từ jsDelivr
    libman install bootstrap@5.3.3 -p jsdelivr -d wwwroot/lib/bootstrap --files css/bootstrap.min.css,js/bootstrap.bundle.min.js
    ```
    Sau khi cài đặt qua CLI hoặc chỉnh sửa `libman.json`, chạy `libman restore`.

---

## Entity Framework Core Migrations

Nếu bạn thực hiện các thay đổi đối với các thực thể miền hoặc `ApplicationDbContext`:

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

---

## EditorConfig

Dự án này bao gồm một tệp `.editorconfig` để giúp duy trì các kiểu code nhất quán. Vui lòng cấu hình IDE của bạn (Visual Studio, VS Code, Rider) để tuân thủ các cài đặt `.editorconfig`. Điều này thường bao gồm:

-   Xóa các câu lệnh `using` không cần thiết.
-   Đảm bảo namespace khớp với cấu trúc thư mục.
-   Áp dụng các quy tắc định dạng code.
-   Sắp xếp các câu lệnh `using`.
-   Sửa các cảnh báo/lỗi của analyzer được định nghĩa trong tệp.

---

Chúc bạn coding vui vẻ! 🚀
