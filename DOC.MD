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
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserName NVARCHAR(256) NOT NULL UNIQUE,
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash VARCHAR(MAX) NOT NULL,
    Name NVARCHAR(150),
    PhoneNumber VARCHAR(20),
    Role VARCHAR(20) NOT NULL DEFAULT 'Customer', -- Admin, Customer, Editor
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

-- Users table Indexes
CREATE INDEX IX_Users_Email ON Users (Email);
CREATE INDEX IX_Users_UserName ON Users (UserName);

-- *********************************************************************
-- Module: Settings
-- *********************************************************************
CREATE TABLE Settings (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Key VARCHAR(100) NOT NULL UNIQUE,
    Value NVARCHAR(MAX) NULL,
    Group VARCHAR(50) NULL,
    DisplayOrder INT NULL,
);

-- Settings table Indexes
CREATE INDEX IX_Settings_Key ON Settings (Key);

-- *********************************************************************
-- Module: Content Management (CMS)
-- *********************************************************************
CREATE TABLE Categories (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ParentId INT,
    Name NVARCHAR(100) NOT NULL,
    Slug VARCHAR(500),
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    TwitterTitle NVARCHAR(200),
    TwitterDescription NVARCHAR(500),
    TwitterImage VARCHAR(500),
    OgTitle NVARCHAR(200),
    OgDescription NVARCHAR(500),
    OgImage VARCHAR(500),
    StructuredData NVARCHAR(MAX),
    DisplayOrder INT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,
    FOREIGN KEY (ParentId) REFERENCES Categories(Id)
);

CREATE TABLE Tags (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Slug VARCHAR(100) NOT NULL UNIQUE,
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    TwitterTitle NVARCHAR(200),
    TwitterDescription NVARCHAR(500),
    TwitterImage VARCHAR(500),
    OgTitle NVARCHAR(200),
    OgDescription NVARCHAR(500),
    OgImage VARCHAR(500),
    StructuredData NVARCHAR(MAX),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

CREATE TABLE Posts (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Title NVARCHAR(200) NOT NULL,
    Slug VARCHAR(200) NOT NULL UNIQUE,
    Summary NVARCHAR(500),
    Content NVARCHAR(MAX),
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    TwitterTitle NVARCHAR(200),
    TwitterDescription NVARCHAR(500),
    TwitterImage VARCHAR(500),
    OgTitle NVARCHAR(200),
    OgDescription NVARCHAR(500),
    OgImage VARCHAR(500),
    StructuredData NVARCHAR(MAX),
    Status TINYINT NOT NULL DEFAULT 0, -- 0: Draft, 1: Published, 2: Scheduled
    PublishedAt DATETIME2,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

CREATE TABLE PostTags (
    PostId INT NOT NULL,
    TagId INT NOT NULL,
    PRIMARY KEY (PostId, TagId),
    FOREIGN KEY (PostId) REFERENCES Posts(Id),
    FOREIGN KEY (TagId) REFERENCES Tags(Id)
);

CREATE TABLE PostCategories (
    PostId INT NOT NULL,
    CategoryId INT NOT NULL,
    PRIMARY KEY (PostId, CategoryId),
    FOREIGN KEY (PostId) REFERENCES Posts(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

CREATE TABLE PostMedias (
    PostId INT NOT NULL,
    MediaId INT NOT NULL,
    UsageType VARCHAR(50) NOT NULL, -- Cover/Twitter/OG/Content/Gallery
    DisplayOrder INT DEFAULT 0,
    PRIMARY KEY (PostId, MediaId),
    FOREIGN KEY (PostId) REFERENCES Posts(Id),
    FOREIGN KEY (MediaId) REFERENCES Medias(Id)
);

CREATE TABLE Pages (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Title NVARCHAR(200) NOT NULL,
    Slug VARCHAR(200) NOT NULL UNIQUE,
    Content NVARCHAR(MAX),
    DisplayOrder INT NOT NULL DEFAULT 0,
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    TwitterTitle NVARCHAR(200),
    TwitterDescription NVARCHAR(500),
    TwitterImage VARCHAR(500),
    OgTitle NVARCHAR(200),
    OgDescription NVARCHAR(500),
    OgImage VARCHAR(500),
    StructuredData NVARCHAR(MAX),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

CREATE TABLE PageMedias (
    PageId INT NOT NULL,
    MediaId INT NOT NULL,
    UsageType VARCHAR(50) NOT NULL, -- Cover/Twitter/OG/Content/Gallery
    DisplayOrder INT DEFAULT 0,
    PRIMARY KEY (PageId, MediaId),
    FOREIGN KEY (PageId) REFERENCES Pages(Id),
    FOREIGN KEY (MediaId) REFERENCES Medias(Id)
);

-- Categories table Indexes
CREATE INDEX IX_Categories_Name ON Categories (Name);
CREATE INDEX IX_Categories_Slug ON Categories (Slug);
CREATE INDEX IX_Categories_ParentId ON Categories (ParentId);

-- Tags table Indexes
CREATE INDEX IX_Tags_Name ON Tags (Name);
CREATE INDEX IX_Tags_Slug ON Tags (Slug);

-- Posts table Indexes
CREATE INDEX IX_Posts_Title ON Posts (Title);
CREATE INDEX IX_Posts_CategoryId ON Posts (CategoryId);
CREATE INDEX IX_Posts_Status ON Posts (Status);
CREATE INDEX IX_PostMedias_UsageType ON PostMedias (UsageType);

-- PostTags table Indexes
CREATE INDEX IX_PostTags_PostId ON PostTags (PostId);
CREATE INDEX IX_PostTags_TagId ON PostTags (TagId);

-- PostCategories table Indexes
CREATE INDEX IX_PostCategories_PostId ON PostTags (PostId);
CREATE INDEX IX_PostCategories_CategoryId ON PostTags (CategoryId);

-- *********************************************************************
-- Module: E-commerce
-- *********************************************************************
CREATE TABLE Products (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(200) NOT NULL,
    Slug VARCHAR(200) NOT NULL UNIQUE,
    SKU VARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX),
    ShortDescription NVARCHAR(500),
    TechnicalSpecs NVARCHAR(MAX), -- Rich text for technical specifications
    Price DECIMAL(18, 2) NOT NULL,
    Discount DECIMAL(18, 2),
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    TwitterTitle NVARCHAR(200),
    TwitterDescription NVARCHAR(500),
    TwitterImage VARCHAR(500),
    OgTitle NVARCHAR(200),
    OgDescription NVARCHAR(500),
    OgImage VARCHAR(500),
    StructuredData NVARCHAR(MAX),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,
);

CREATE TABLE ProductTags (
    ProductId INT NOT NULL,
    TagId INT NOT NULL,
    PRIMARY KEY (ProductId, TagId),
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (TagId) REFERENCES Tags(Id)
);

CREATE TABLE ProductCategories (
    ProductId INT NOT NULL,
    CategoryId INT NOT NULL,
    PRIMARY KEY (ProductId, CategoryId),
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

CREATE TABLE ProductMedias (
    ProductId INT NOT NULL,
    MediaId INT NOT NULL,
    UsageType VARCHAR(50) NOT NULL, -- Main/Thumbnail/Gallery/3DView
    DisplayOrder INT DEFAULT 0,
    PRIMARY KEY (ProductId, MediaId),
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (MediaId) REFERENCES Medias(Id)
);

-- Products table Indexes
CREATE INDEX IX_Products_Name ON Products (Name);
CREATE INDEX IX_Products_Slug ON Products (Slug);

-- ProductTags table Indexes
CREATE INDEX IX_ProductTags_ProductId ON ProductTags (ProductId);
CREATE INDEX IX_ProductTags_TagId ON ProductTags (TagId);

-- ProductCategories table Indexes
CREATE INDEX IX_ProductCategories_ProductId ON ProductTags (ProductId);
CREATE INDEX IX_ProductCategories_CategoryId ON ProductTags (CategoryId);

-- *********************************************************************
-- Module: Shopping Cart & Wishlist
-- *********************************************************************
CREATE TABLE Carts (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE Wishlists (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    DeletedAt DATETIME2,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

-- *********************************************************************
-- Module: Orders (Bổ sung)
-- *********************************************************************
CREATE TABLE Orders (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    OrderDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(18, 2) NOT NULL,
    Status VARCHAR(50) NOT NULL DEFAULT 'Pending', -- Pending, Processing, Completed, Cancelled
    ShippingAddress NVARCHAR(MAX),
    BillingAddress NVARCHAR(MAX),
    PaymentMethod VARCHAR(50),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE OrderItems (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    Discount DECIMAL(18, 2),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

-- *********************************************************************
-- Module: Media Management (Áp dụng cho cả Post và Product)
-- *********************************************************************
CREATE TABLE Medias (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    MediaType VARCHAR(50) NOT NULL, -- Image/Video/Document
    MediaUrl VARCHAR(500) NOT NULL, -- URL từ cloud storage
    ThumbnailUrl VARCHAR(500), -- URL thumbnail (nếu cần)
    AltText NVARCHAR(200), -- Mô tả ảnh cho SEO
    FileSize INT, -- Kích thước file (KB)
    Dimension VARCHAR(50), -- Kích thước ảnh (e.g., "1920x1080")
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2
);

-- *********************************************************************
-- Module: Marketing
-- *********************************************************************
CREATE TABLE Contacts (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(100) NOT NULL,
    Email VARCHAR(256),
    Phone VARCHAR(20) NOT NULL,
    Message NVARCHAR(MAX),
    Status VARCHAR(20) NOT NULL DEFAULT 'New', -- New, Read, Replied
    IPAddress VARCHAR(50),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

CREATE TABLE Subscribers (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Email VARCHAR(256),
    IPAddress VARCHAR(50),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

-- Contacts table Indexes
CREATE INDEX IX_Contacts_Status ON Contacts (Status);
CREATE INDEX IX_Contacts_Email ON Contacts (Email);
CREATE INDEX IX_Contacts_Phone ON Contacts (Phone);
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