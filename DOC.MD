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
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserName NVARCHAR(256) NOT NULL UNIQUE,
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Name NVARCHAR(150),
    PhoneNumber NVARCHAR(20),
    Role NVARCHAR(20) NOT NULL DEFAULT 'Customer', -- Admin, Customer
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

CREATE TABLE UserAddresses (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    AddressType NVARCHAR(20) NOT NULL, -- Billing, Shipping
    Name NVARCHAR(200) NOT NULL,
    AddressLine1 NVARCHAR(200) NOT NULL,
    AddressLine2 NVARCHAR(200),
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100),
    PostalCode NVARCHAR(20) NOT NULL,
    Country NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    IsDefault BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE Categories (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ParentId UNIQUEIDENTIFIER,
    Name NVARCHAR(200) NOT NULL,
    Path NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500),
    Type NVARCHAR(50) NOT NULL, -- Product, Post, etc.
    Slug NVARCHAR(500),
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    DisplayOrder INT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,
    FOREIGN KEY (ParentId) REFERENCES Categories(Id),
);

CREATE TABLE Tags (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Slug NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    Type NVARCHAR(50) NOT NULL, -- Product, Post, etc.
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,
);

CREATE TABLE Posts (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(200) NOT NULL,
    Slug NVARCHAR(200) NOT NULL UNIQUE,
    Summary NVARCHAR(500),
    Content NVARCHAR(MAX),
    CategoryId UNIQUEIDENTIFIER NOT NULL,
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Draft', -- Draft, Published
    PublishedAt DATETIME2,
    IsActive BIT NOT NULL DEFAULT 1
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
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
    Slug NVARCHAR(200) NOT NULL UNIQUE,
    Content NVARCHAR(MAX),
    Template NVARCHAR(100),
    DisplayOrder INT NOT NULL DEFAULT 0,
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(200),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

CREATE TABLE Media (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FileName NVARCHAR(255) NOT NULL,
    FileType NVARCHAR(50) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    FileSize BIGINT NOT NULL,
    ThumbPath NVARCHAR(500),
    Alt NVARCHAR(200),
    Title NVARCHAR(200),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Manufacturers (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500),
    Logo NVARCHAR(500),
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
    Slug NVARCHAR(200) NOT NULL UNIQUE,
    Type NVARCHAR(100) NOT NULL, -- Paint, Waterproofing, Grout, Putty etc.
    ManufacturerId UNIQUEIDENTIFIER,
    CategoryId UNIQUEIDENTIFIER NOT NULL,
    SKU NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX),
    ShortDescription NVARCHAR(500),
    TechnicalSpecs NVARCHAR(MAX), -- Rich text for technical specifications
    ApplicationGuide NVARCHAR(MAX), -- Rich text for Usage/application instructions
    Unit NVARCHAR(20) NOT NULL, -- kg, liter, bucket, etc.
    PackagingSizes NVARCHAR(MAX), -- JSON array of available sizes
    Certificates NVARCHAR(MAX), -- Rich text for certification info
    Price DECIMAL(18, 2) NOT NULL,
    Discount DECIMAL(18, 2),
    Currency NVARCHAR(10) NOT NULL,
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

CREATE TABLE ProductImages (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProductId UNIQUEIDENTIFIER NOT NULL,
    MediaId UNIQUEIDENTIFIER NOT NULL,
    DisplayOrder INT NOT NULL DEFAULT 0,
    IsMain BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (MediaId) REFERENCES Media(Id)
);

CREATE TABLE ProductTags (
    ProductId UNIQUEIDENTIFIER NOT NULL,
    TagId UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY (ProductId, TagId),
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (TagId) REFERENCES Tags(Id)
);

CREATE TABLE ProductAttributes (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    DataType NVARCHAR(50) NOT NULL, -- Text, Number, Boolean, etc.
    IsFilterable BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2
);

CREATE TABLE ProductAttributeValues (
    ProductId UNIQUEIDENTIFIER NOT NULL,
    AttributeId UNIQUEIDENTIFIER NOT NULL,
    Value NVARCHAR(MAX) NOT NULL,
    PRIMARY KEY (ProductId, AttributeId),
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (AttributeId) REFERENCES ProductAttributes(Id)
);

CREATE TABLE Orders (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER,
    OrderNumber NVARCHAR(100) NOT NULL UNIQUE,
    OrderStatus NVARCHAR(50) NOT NULL,
    PaymentStatus NVARCHAR(50) NOT NULL,
    PaymentMethod NVARCHAR(100),
    ShippingMethod NVARCHAR(100),
    SubTotal DECIMAL(18,2) NOT NULL,
    ShippingCost DECIMAL(18,2) NOT NULL,
    Tax DECIMAL(18,2) NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    BillingName NVARCHAR(200) NOT NULL,
    BillingEmail NVARCHAR(256) NOT NULL,
    BillingPhone NVARCHAR(20),
    BillingAddress NVARCHAR(500) NOT NULL,
    ShippingName NVARCHAR(200) NOT NULL,
    ShippingEmail NVARCHAR(256) NOT NULL,
    ShippingPhone NVARCHAR(20),
    ShippingAddress NVARCHAR(500) NOT NULL,
    Notes NVARCHAR(1000),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE OrderItems (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    OrderId UNIQUEIDENTIFIER NOT NULL,
    ProductId UNIQUEIDENTIFIER NOT NULL,
    ProductName NVARCHAR(200) NOT NULL,
    SKU NVARCHAR(100),
    UnitPrice DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
);

CREATE TABLE Reviews (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProductId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER,
    Rating INT NOT NULL,
    Title NVARCHAR(200),
    Comment NVARCHAR(1000),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending', -- Pending, Approved, Rejected
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE Wishlists (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    ProductId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE Settings (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Key] NVARCHAR(100) NOT NULL UNIQUE,
    Value NVARCHAR(MAX),
    Description NVARCHAR(500),
    Type NVARCHAR(50) NOT NULL, -- System, Site, Mail, Payment, Shipping
    IsActive BIT NOT NULL DEFAULT 1,
    UpdatedAt DATETIME2
);

CREATE TABLE EmailTemplates (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Subject NVARCHAR(200) NOT NULL,
    Body NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(500),
    Type NVARCHAR(50) NOT NULL, -- Welcome, Order, Password Reset, etc.
    Variables NVARCHAR(MAX), -- JSON array of available template variables
    IsActive BIT NOT NULL DEFAULT 1,
    UpdatedAt DATETIME2
);

CREATE TABLE SeoMetadata (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EntityType NVARCHAR(50) NOT NULL, -- Page, Product, Category, Tag, Post
    EntityId UNIQUEIDENTIFIER NOT NULL,
    TwitterTitle NVARCHAR(200),             
    TwitterDescription NVARCHAR(500),     
    TwitterImage NVARCHAR(500),            
    OgTitle NVARCHAR(200),
    OgDescription NVARCHAR(500),
    OgImage NVARCHAR(500),
    StructuredData NVARCHAR(MAX),          
    CanonicalUrl NVARCHAR(500),
    Robots NVARCHAR(50), -- index, follow or noindex, nofollow
    Priority DECIMAL(3, 2) DEFAULT 0.5, -- sitemap.xml (0.0 -> 0.1)
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
);

CREATE TABLE ContactForms (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(256) NOT NULL,
    Phone NVARCHAR(20),
    Subject NVARCHAR(200),
    Message NVARCHAR(MAX),
    Status NVARCHAR(20) NOT NULL DEFAULT 'New', -- New, Read, Replied
    IPAddress NVARCHAR(50),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2
);

CREATE TABLE Notifications (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Message NVARCHAR(500) NOT NULL,
    Type NVARCHAR(50) NOT NULL, -- Info, Success, Warning, Error
    EntityType NVARCHAR(50), -- Order, Product, User, etc.
    EntityId UNIQUEIDENTIFIER,
    IsRead BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    ReadAt DATETIME2,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Create Indexes
CREATE INDEX IX_Categories_Slug_Type ON Categories(Slug, Type);
CREATE INDEX IX_Categories_Type ON Categories(Type);
CREATE INDEX IX_Categories_ParentId ON Categories(ParentId);
CREATE INDEX IX_Categories_Path ON Categories(Path);

CREATE INDEX IX_Posts_CategoryId ON Posts(CategoryId);
CREATE INDEX IX_Posts_Slug ON Posts(Slug);
CREATE INDEX IX_Posts_Status ON Posts(Status);

CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
CREATE INDEX IX_Products_ProductTypeId ON Products(ProductTypeId);
CREATE INDEX IX_Products_ManufacturerId ON Products(ManufacturerId);
CREATE INDEX IX_Products_Slug ON Products(Slug);

CREATE INDEX IX_Orders_UserId ON Orders(UserId);
CREATE INDEX IX_Orders_OrderNumber ON Orders(OrderNumber);
CREATE INDEX IX_Orders_OrderStatus ON Orders(OrderStatus);
CREATE INDEX IX_Orders_CreatedAt ON Orders(CreatedAt);

CREATE INDEX IX_SeoMetadata_EntityType_EntityId ON SeoMetadata(EntityType, EntityId);
CREATE INDEX IX_Notifications_UserId_IsRead ON Notifications(UserId, IsRead);

-- Add Full-Text Search
CREATE FULLTEXT CATALOG ft_catalog AS DEFAULT;

CREATE FULLTEXT INDEX ON Products(Name, Description, ShortDescription) 
KEY INDEX PK_Products;

CREATE FULLTEXT INDEX ON Posts(Title, Content, Summary) 
KEY INDEX PK_Posts;

CREATE FULLTEXT INDEX ON Categories(Name, Description) 
KEY INDEX PK_Categories;
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