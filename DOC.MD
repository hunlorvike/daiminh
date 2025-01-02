# CMS và E-commerce - Tài liệu Kỹ thuật Chi Tiết

## 1. Mục tiêu ứng dụng

- Xây dựng hệ thống CMS (Hệ thống Quản lý Nội dung) và E-commerce tích hợp, sử dụng **ASP.NET Core 8 MVC**.
- Quản lý nội dung hiệu quả: bài viết, trang tĩnh, danh mục, media.
- Bán hàng trực tuyến với đầy đủ tính năng: quản lý sản phẩm, giỏ hàng, đơn hàng, đánh giá.
- Tối ưu hóa SEO để tăng khả năng hiển thị trên công cụ tìm kiếm.
- Giao diện người dùng thân thiện, responsive trên mọi thiết bị.
- Hiệu suất cao và bảo mật.

---

## 2. Kiến trúc ứng dụng

### Công nghệ chính

- **Backend**: ASP.NET Core 8 MVC
- **ORM**: Entity Framework Core 8
- **Cơ sở dữ liệu**: SQL Server
- **Frontend**: Razor Views, JavaScript/jQuery, Bootstrap 5
- **Caching**: Redis

### Containerization

- **Docker**: Container hóa ứng dụng và các dependencies.

### CI/CD

- **GitHub Actions**: Tích hợp liên tục (Continuous Integration - CI).
- **Azure DevOps**: Triển khai liên tục (Continuous Deployment - CD).

---

## 3. Công nghệ sử dụng

### Backend

- **ASP.NET Core 8 MVC**
- **Entity Framework Core 8**
- **AutoMapper**: Object Mapping
- **FluentValidation**: Validation
- **MediatR**: Mô hình CQRS

### Frontend

- **Razor Views**
- **JavaScript/jQuery**
- **Bootstrap 5/Tabler**
- **TinyMCE**: Trình soạn thảo văn bản Rich Text
- **DataTables.js**: Hiển thị và phân trang dữ liệu
- **SweetAlert2**: Tạo alert nâng cao

### Database

- **SQL Server**
- **Redis**: Hệ thống caching

### Công cụ phát triển

- **JetBrains Rider**: IDE chính
- **GitHub**: Version control
- **Docker Desktop**: Quản lý container

### Cloud Services

- **Cloudinary**: Lưu trữ và quản lý media

### Monitoring & Logging

- **Application Insights**: Giám sát hiệu năng
- **Serilog**: Structured Logging

---

## 4. Chức năng chi tiết

### **CMS Module**

#### 1. Quản lý bài viết

- CRUD (Tạo, Đọc, Cập nhật, Xóa) bài viết.
- Workflow: Soạn thảo (Draft) và xuất bản (Publish).
- Versioning: Lưu lịch sử chỉnh sửa.
- SEO Metadata: Tiêu đề, mô tả, từ khóa.
- Tích hợp trình soạn thảo TinyMCE.

#### 2. Quản lý trang tĩnh

- Tạo và chỉnh sửa trang tĩnh.
- Lựa chọn template.
- Tối ưu hóa SEO.

#### 3. Quản lý danh mục

- Hỗ trợ cấu trúc cha-con không giới hạn.
- Drag-and-drop UI để sắp xếp danh mục.

#### 4. Media Management

- Tải lên hình ảnh, video.
- Tự động tạo thumbnail.
- Tích hợp Azure Blob Storage.

#### 5. Công cụ SEO

- Tự động sinh **sitemap.xml**.
- Quản lý **Canonical URL**.
- Hỗ trợ **Schema.org markup**.
- Meta tags cho **Open Graph** và **Twitter Card**.

---

### **E-commerce Module**

#### 1. Quản lý sản phẩm

- CRUD sản phẩm.
- Quản lý biến thể sản phẩm (kích thước, màu sắc...).
- Theo dõi tồn kho (Inventory tracking).
- Import/Export sản phẩm (CSV, Excel).

#### 2. Giỏ hàng và Thanh toán

- Tích hợp nhiều cổng thanh toán (Stripe, PayPal).

#### 3. Quản lý đơn hàng

- Dashboard quản lý đơn hàng.
- Cập nhật trạng thái đơn hàng.
- Gửi email thông báo tự động.

#### 4. Đánh giá sản phẩm

- Hệ thống đánh giá và nhận xét.
- Công cụ kiểm duyệt cho Admin.

#### 5. Wishlist và So sánh sản phẩm

- Lưu sản phẩm yêu thích.
- So sánh các đặc điểm sản phẩm.

---

### **Quản trị hệ thống**

#### 1. Quản lý người dùng

- CRUD người dùng.
- Role-based Access Control (RBAC).
- Xác thực hai yếu tố (2FA).

#### 2. Dashboard & Analytics

- Tổng quan doanh số, đơn hàng, traffic.
- Tích hợp Google Analytics.

#### 3. Cấu hình hệ thống

- Quản lý cài đặt toàn cầu.
- Quản lý mẫu email.
- Tùy chỉnh giao diện.

---

## 5. Hướng dẫn phát triển

### Cài đặt môi trường

1. Cài đặt **JetBrains Rider**.
2. Cài đặt **.NET 8 SDK**.
3. Cài đặt **Docker Desktop**.
4. Cài đặt **SQL Server** (hoặc sử dụng Docker image).

### Khởi tạo project

1. Clone repository:
   ```bash
   git clone https://github.com/your-repo/cms-ecommerce.git
   ```
2. Mở solution trong **Rider**.

### Cấu hình Development

1. Cập nhật connection string trong `appsettings.Development.json`.
2. Chạy migrations:
   ```bash
   dotnet ef database update
   ```
3. Chạy ứng dụng:
   ```bash
   dotnet run
   ```

### Phát triển

1. Tạo branch mới cho mỗi tính năng/sửa lỗi.
2. Viết **unit tests** trước khi triển khai (TDD).
3. Sử dụng CQRS với MediatR cho business logic.
4. Tuân thủ nguyên tắc **Clean Architecture**.

---

## 6. Quy tắc và Quy ước

### Coding Standards

- Tuân thủ **C# Coding Conventions** của Microsoft.
- Sử dụng `camelCase` cho private fields, `PascalCase` cho public members.
- Prefix tên interface bằng `I` (e.g., `IProductService`).

### Design Patterns

- **Repository Pattern**: Data Access.
- **CQRS** với **MediatR**.
- **Factory Pattern**: Tạo object.
- **Observer Pattern**: Xử lý sự kiện.

### Best Practices

- **Dependency Injection**.
- **Asynchronous programming** với `async/await`.
- Sử dụng **strongly-typed configuration**.
- Logging cho error handling và auditing.

---

## 7. Tối ưu hóa và Bảo mật

### Tối ưu hóa Hiệu suất

- Caching với Redis.
- Lazy Loading trong Entity Framework.
- Tối ưu hóa truy vấn Database.
- Lập trình bất đồng bộ (Asynchronous).

### Bảo mật

- Bật **HTTPS** toàn ứng dụng.
- Sử dụng **ASP.NET Core Identity**.
- Bảo vệ **CSRF**.
- Kiểm tra và làm sạch input người dùng.
- Sử dụng truy vấn tham số (Parameterized Queries).
- Giới hạn tốc độ truy cập (Rate Limiting).

---

## 8. Triển khai và Bảo trì

### Docker

1. Tạo Dockerfile.
2. Sử dụng Docker Compose để phát triển.

### CI/CD Pipeline

- Triển khai sau.

---

## 9. Cấu trúc cơ sở dữ liệu

``` sql
-- *********************************************************************
-- Module: Authentication & Users
-- *********************************************************************
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserName NVARCHAR(256) NOT NULL UNIQUE,
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash VARCHAR(MAX) NOT NULL,
    Name NVARCHAR(150),
    PhoneNumber VARCHAR(20),
    Role VARCHAR(20) NOT NULL DEFAULT 'Customer', -- Admin, Customer
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

-- Users table Indexes
CREATE INDEX IX_Users_Email ON Users (Email);
CREATE INDEX IX_Users_UserName ON Users (UserName);

-- *********************************************************************
-- Module: Settings & Email
-- *********************************************************************
CREATE TABLE Settings (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Key] VARCHAR(100) NOT NULL UNIQUE,
    Value NVARCHAR(MAX) NULL,
    [Group] VARCHAR(50) NULL, -- mail, social, system, etc.
    SubGroup VARCHAR(50) NULL, -- subgroup of the group
    Type VARCHAR(50) NOT NULL DEFAULT 'string', -- data type (e.g., string, int)
    DisplayOrder INT NULL, -- display order
    Description NVARCHAR(MAX) NULL, -- description of the setting
    IsActive BIT NOT NULL DEFAULT 1 -- active status
);

CREATE TABLE EmailTemplates (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name VARCHAR(100) NOT NULL UNIQUE,
    Subject NVARCHAR(200) NOT NULL,
    Body NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(500),
    Type VARCHAR(50) NOT NULL, -- Welcome, Order, Password Reset, etc.
    Variables NVARCHAR(MAX), -- JSON array of available template variables
    IsActive BIT NOT NULL DEFAULT 1,
    UpdatedAt DATETIME2
);

-- Settings table Indexes
CREATE INDEX IX_Settings_Key ON Settings ([Key]);

-- EmailTemplates table Indexes
CREATE INDEX IX_EmailTemplates_Name ON EmailTemplates (Name);
CREATE INDEX IX_EmailTemplates_Type ON EmailTemplates (Type);

-- *********************************************************************
-- Module: Content Management (CMS)
-- *********************************************************************
CREATE TABLE Categories (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ParentId UNIQUEIDENTIFIER,
    Name NVARCHAR(100) NOT NULL,
    Slug VARCHAR(500),
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    DisplayOrder INT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,
    FOREIGN KEY (ParentId) REFERENCES Categories(Id)
);

CREATE TABLE Tags (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Slug VARCHAR(100) NOT NULL UNIQUE,
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

CREATE TABLE Posts (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(200) NOT NULL,
    Slug VARCHAR(200) NOT NULL UNIQUE,
    Summary NVARCHAR(500),
    Content NVARCHAR(MAX),
    CategoryId UNIQUEIDENTIFIER NOT NULL,
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    Status TINYINT NOT NULL DEFAULT 0, -- 0: Draft, 1: Published, 2: Scheduled
    PublishedAt DATETIME2,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

CREATE TABLE PostTags (
    PostId UNIQUEIDENTIFIER NOT NULL,
    TagId UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY (PostId, TagId),
    FOREIGN KEY (PostId) REFERENCES Posts(Id),
    FOREIGN KEY (TagId) REFERENCES Tags(Id)
);

CREATE TABLE Pages (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(200) NOT NULL,
    Slug VARCHAR(200) NOT NULL UNIQUE,
    Content NVARCHAR(MAX),
    Template VARCHAR(100),
    DisplayOrder INT NOT NULL DEFAULT 0,
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

-- Categories table Indexes
CREATE INDEX IX_Categories_Name ON Categories (Name);
CREATE INDEX IX_Categories_ParentId ON Categories (ParentId);

-- Tags table Indexes
CREATE INDEX IX_Tags_Name ON Tags (Name);
CREATE INDEX IX_Tags_Slug ON Tags (Slug);

-- Posts table Indexes
CREATE INDEX IX_Posts_Title ON Posts (Title);
CREATE INDEX IX_Posts_CategoryId ON Posts (CategoryId);
CREATE INDEX IX_Posts_Status ON Posts (Status);

-- PostTags table Indexes
CREATE INDEX IX_PostTags_PostId ON PostTags (PostId);
CREATE INDEX IX_PostTags_TagId ON PostTags (TagId);

-- Pages table Indexes
CREATE INDEX IX_Pages_Title ON Pages (Title);

-- *********************************************************************
-- Module: E-commerce
-- *********************************************************************
CREATE TABLE Manufacturers (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500),
    Logo VARCHAR(500),
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

CREATE TABLE Products (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Slug VARCHAR(200) NOT NULL UNIQUE,
    Type VARCHAR(100) NOT NULL, -- Paint, Waterproofing, Grout, Putty etc.
    ManufacturerId UNIQUEIDENTIFIER,
    CategoryId UNIQUEIDENTIFIER NOT NULL,
    SKU VARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX),
    ShortDescription NVARCHAR(500),
    TechnicalSpecs NVARCHAR(MAX), -- Rich text for technical specifications
    ApplicationGuide NVARCHAR(MAX), -- Rich text for Usage/application instructions
    Certificates NVARCHAR(MAX), -- Rich text for certification info
    Price DECIMAL(18, 2) NOT NULL,
    Discount DECIMAL(18, 2),
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,
    FOREIGN KEY (ManufacturerId) REFERENCES Manufacturers(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

CREATE TABLE ProductMedias (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProductId UNIQUEIDENTIFIER NOT NULL,
    MediaType VARCHAR(50) NOT NULL, -- Image, Video, Document
    MediaUrl VARCHAR(500) NOT NULL,
    DisplayOrder INT NOT NULL DEFAULT 0,
    IsMain BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE ProductTags (
    ProductId UNIQUEIDENTIFIER NOT NULL,
    TagId UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY (ProductId, TagId),
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (TagId) REFERENCES Tags(Id)
);

-- Manufacturers table Indexes
CREATE INDEX IX_Manufacturers_Name ON Manufacturers (Name);

-- Products table Indexes
CREATE INDEX IX_Products_Name ON Products (Name);
CREATE INDEX IX_Products_Slug ON Products (Slug);
CREATE INDEX IX_Products_CategoryId ON Products (CategoryId);
CREATE INDEX IX_Products_ManufacturerId ON Products (ManufacturerId);

-- ProductTags table Indexes
CREATE INDEX IX_ProductTags_ProductId ON ProductTags (ProductId);
CREATE INDEX IX_ProductTags_TagId ON ProductTags (TagId);

-- *********************************************************************
-- Module: SEO & Marketing
-- *********************************************************************
CREATE TABLE SeoMetadata (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EntityType VARCHAR(50) NOT NULL, -- Page, Product, Category, Tag, Post
    EntityId UNIQUEIDENTIFIER NOT NULL,
    TwitterTitle NVARCHAR(200),
    TwitterDescription NVARCHAR(500),
    TwitterImage VARCHAR(500),
    OgTitle NVARCHAR(200),
    OgDescription NVARCHAR(500),
    OgImage VARCHAR(500),
    StructuredData NVARCHAR(MAX),
    CanonicalUrl VARCHAR(500),
    Robots VARCHAR(50), -- index, follow or noindex, nofollow
    Priority DECIMAL(3, 2) DEFAULT 0.5, -- sitemap.xml (0.0 -> 0.1)
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2
);

CREATE TABLE ContactForms (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    Email VARCHAR(256),
    Phone VARCHAR(20) NOT NULL,
    Subject NVARCHAR(200),
    Message NVARCHAR(MAX),
    Status VARCHAR(20) NOT NULL DEFAULT 'New', -- New, Read, Replied
    IPAddress VARCHAR(50),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

-- SeoMetadata table Indexes
CREATE INDEX IX_SeoMetadata_EntityId ON SeoMetadata (EntityId);
CREATE INDEX IX_SeoMetadata_EntityType ON SeoMetadata (EntityType);

-- ContactForms table Indexes
CREATE INDEX IX_ContactForms_Status ON ContactForms (Status);
CREATE INDEX IX_ContactForms_Email ON ContactForms (Email);
CREATE INDEX IX_ContactForms_Phone ON ContactForms (Phone);
```

---

## 10. Hướng dẫn triển khai

- Triển khai sau

---

## 11. Bảo mật

- Sử dụng **JWT** cho API.
- Áp dụng **Web Application Firewall (WAF)**.

---

## 12. Tối ưu hiệu suất

- Minify JavaScript/CSS.
- Sử dụng CDN.
- Tối ưu hóa truy vấn và sử dụng cache.