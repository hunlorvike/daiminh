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
CREATE DATABASE cms_daiminh;
\c cms_daiminh;

CREATE TABLE users (
    id BIGSERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    role_id INT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    FOREIGN KEY (role_id) REFERENCES roles(id) ON DELETE SET NULL
);

CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    permissions JSONB NOT NULL DEFAULT '[]',
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE 
);

CREATE TABLE categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    parent_category_id INT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    FOREIGN KEY (parent_category_id) REFERENCES categories(id) ON DELETE SET NULL
);

CREATE TABLE tags (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    slug VARCHAR(50) NOT NULL UNIQUE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE 
);

CREATE TABLE content_types (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    slug VARCHAR(50) NOT NULL UNIQUE,
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE 
);

CREATE TABLE content_field_definitions (
    id SERIAL PRIMARY KEY,
    content_type_id INT,
    field_name VARCHAR(50) NOT NULL,
    field_type VARCHAR(30) NOT NULL,
    is_required BOOLEAN NOT NULL DEFAULT FALSE,
    field_options JSONB,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    FOREIGN KEY (content_type_id) REFERENCES content_types(id) ON DELETE CASCADE
);

CREATE TABLE content_field_values (
    id BIGSERIAL PRIMARY KEY,
    content_item_id BIGINT,
    field_id INT,
    value TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    FOREIGN KEY (content_item_id) REFERENCES contents(id) ON DELETE CASCADE,
    FOREIGN KEY (field_id) REFERENCES content_field_definitions(id) ON DELETE CASCADE
);

CREATE TABLE contents (
    id BIGSERIAL PRIMARY KEY,
    content_type_id INT,
    author_id BIGINT,
    title VARCHAR(255) NOT NULL,
    slug VARCHAR(255) NOT NULL UNIQUE,
    status VARCHAR(20) NOT NULL,
    meta_title VARCHAR(100),
    meta_description VARCHAR(160),
    canonical_url VARCHAR(255),
    og_title VARCHAR(100),
    og_description VARCHAR(200),
    og_image VARCHAR(255),
    structured_data JSONB,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    FOREIGN KEY (content_type_id) REFERENCES content_types(id) ON DELETE CASCADE,
    FOREIGN KEY (author_id) REFERENCES users(id) ON DELETE SET NULL
);

CREATE TABLE content_categories (
    content_item_id BIGINT,
    category_id INT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    PRIMARY KEY (content_item_id, category_id),
    FOREIGN KEY (content_item_id) REFERENCES contents(id) ON DELETE CASCADE,
    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE CASCADE
);

CREATE TABLE content_tags (
    content_item_id BIGINT,
    tag_id INT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    PRIMARY KEY (content_item_id, tag_id),
    FOREIGN KEY (content_item_id) REFERENCES contents(id) ON DELETE CASCADE,
    FOREIGN KEY (tag_id) REFERENCES tags(id) ON DELETE CASCADE
);

CREATE TABLE content_comments (
    id BIGSERIAL PRIMARY KEY,
    content_item_id BIGINT NOT NULL,
    user_id BIGINT,
    parent_comment_id BIGINT,
    content TEXT NOT NULL,
    status VARCHAR(20) NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    FOREIGN KEY (content_item_id) REFERENCES contents(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE SET NULL,
    FOREIGN KEY (parent_comment_id) REFERENCES content_comments(id) ON DELETE CASCADE
);

CREATE TABLE product_types (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    slug VARCHAR(50) NOT NULL UNIQUE,
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE 
);

CREATE TABLE product_field_definitions (
    id SERIAL PRIMARY KEY,
    product_type_id INT NOT NULL,
    field_name VARCHAR(50) NOT NULL,
    field_type VARCHAR(30) NOT NULL,
    is_required BOOLEAN NOT NULL DEFAULT FALSE,
    field_options JSONB,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    FOREIGN KEY (product_type_id) REFERENCES product_types(id) ON DELETE CASCADE
);

CREATE TABLE product_field_values (
    id BIGSERIAL PRIMARY KEY,
    product_id BIGINT,
    field_id INT,
    value TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE,
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE,
    FOREIGN KEY (field_id) REFERENCES product_field_definitions(id) ON DELETE CASCADE
);

CREATE TABLE products (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    slug VARCHAR(255) NOT NULL UNIQUE,
    description TEXT,
    base_price DECIMAL(10, 2) NOT NULL,
    sku VARCHAR(50) UNIQUE,
    stock_quantity INT NOT NULL DEFAULT 0,
    meta_title VARCHAR(100),
    meta_description VARCHAR(160),
    canonical_url VARCHAR(255),
    og_title VARCHAR(100),
    og_description VARCHAR(200),
    og_image VARCHAR(255),
    structured_data JSONB,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE 
);

CREATE TABLE product_images (
    id BIGSERIAL PRIMARY KEY,
    product_id BIGINT NOT NULL,
    image_url VARCHAR(255) NOT NULL,
    alt_text VARCHAR(255),
    is_primary BOOLEAN NOT NULL DEFAULT FALSE,
    display_order SMALLINT NOT NULL DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE
);

CREATE TABLE product_categories (
    product_id BIGINT,
    category_id INT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    PRIMARY KEY (product_id, category_id),
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE,
    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE CASCADE
);

CREATE TABLE product_tags (
    product_id BIGINT,
    tag_id INT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    PRIMARY KEY (product_id, tag_id),
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE,
    FOREIGN KEY (tag_id) REFERENCES tags(id) ON DELETE CASCADE
);

CREATE TABLE product_comments (
    id BIGSERIAL PRIMARY KEY,
    product_id BIGINT NOT NULL,
    user_id BIGINT,
    parent_comment_id BIGINT,
    content TEXT NOT NULL,
    status VARCHAR(20) NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE SET NULL,
    FOREIGN KEY (parent_comment_id) REFERENCES product_comments(id) ON DELETE CASCADE
);

CREATE TABLE product_reviews (
    id BIGSERIAL PRIMARY KEY,
    product_id BIGINT NOT NULL,
    user_id BIGINT NOT NULL,
    rating SMALLINT NOT NULL CHECK (rating BETWEEN 1 AND 5),
    comment TEXT,
    status VARCHAR(20) NOT NULL DEFAULT 'pending',
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE, 
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

CREATE TABLE settings (
    id SERIAL PRIMARY KEY,
    setting_key VARCHAR(50) NOT NULL UNIQUE,
    setting_value TEXT NOT NULL,
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE 
);

CREATE TABLE contacts (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL,
    phone VARCHAR(20),
    message TEXT,
    status VARCHAR(20) NOT NULL DEFAULT 'pending',
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE 
);

CREATE TABLE subscriber (
    id BIGSERIAL PRIMARY KEY,
    email VARCHAR(255) NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'pending',
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE 
);

-- Indexes cho bảng users
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_username ON users(username);

-- Indexes cho bảng roles
CREATE INDEX idx_roles_name ON roles(name);

-- Indexes cho bảng categories
CREATE INDEX idx_categories_slug ON categories(slug);
CREATE INDEX idx_categories_parent_category_id ON categories(parent_category_id);

-- Indexes cho bảng tags
CREATE INDEX idx_tags_slug ON tags(slug);

-- Indexes cho bảng content_types
CREATE INDEX idx_content_types_slug ON content_types(slug);

-- Indexes cho bảng content_field_definitions
CREATE INDEX idx_content_field_definitions_content_type_id ON content_field_definitions(content_type_id);

-- Indexes cho bảng contents
CREATE INDEX idx_contents_slug ON contents(slug);
CREATE INDEX idx_contents_content_type_id ON contents(content_type_id);
CREATE INDEX idx_contents_author_id ON contents(author_id);

-- Indexes cho bảng content_field_values
CREATE INDEX idx_content_field_values_content_item_id ON content_field_values(content_item_id);
CREATE INDEX idx_content_field_values_field_id ON content_field_values(field_id);

-- Indexes cho bảng content_categories
CREATE INDEX idx_content_categories_content_item_id ON content_categories(content_item_id);
CREATE INDEX idx_content_categories_category_id ON content_categories(category_id);

-- Indexes cho bảng content_tags
CREATE INDEX idx_content_tags_content_item_id ON content_tags(content_item_id);
CREATE INDEX idx_content_tags_tag_id ON content_tags(tag_id);

-- Indexes cho bảng content_comments
CREATE INDEX idx_content_comments_content_item_id ON content_comments(content_item_id);
CREATE INDEX idx_content_comments_user_id ON content_comments(user_id);
CREATE INDEX idx_content_comments_parent_comment_id ON content_comments(parent_comment_id);

-- Indexes cho bảng products
CREATE INDEX idx_products_slug ON products(slug);
CREATE INDEX idx_products_product_type_id ON products(product_type_id);

-- Indexes cho bảng product_types
CREATE INDEX idx_product_types_slug ON product_types(slug);

-- Indexes cho bảng product_field_definitions
CREATE INDEX idx_product_field_definitions_product_type_id ON product_field_definitions(product_type_id);

-- Indexes cho bảng product_field_values
CREATE INDEX idx_product_field_values_product_id ON product_field_values(product_id);
CREATE INDEX idx_product_field_values_field_id ON product_field_values(field_id);

-- Indexes cho bảng product_images
CREATE INDEX idx_product_images_product_id ON product_images(product_id);

-- Indexes cho bảng product_categories
CREATE INDEX idx_product_categories_product_id ON product_categories(product_id);
CREATE INDEX idx_product_categories_category_id ON product_categories(category_id);

-- Indexes cho bảng product_tags
CREATE INDEX idx_product_tags_product_id ON product_tags(product_id);
CREATE INDEX idx_product_tags_tag_id ON product_tags(tag_id);

-- Indexes cho bảng product_comments
CREATE INDEX idx_product_comments_product_id ON product_comments(product_id);
CREATE INDEX idx_product_comments_user_id ON product_comments(user_id);
CREATE INDEX idx_product_comments_parent_comment_id ON product_comments(parent_comment_id);

-- Indexes cho bảng product_reviews
CREATE INDEX idx_product_reviews_product_id ON product_reviews(product_id);
CREATE INDEX idx_product_reviews_user_id ON product_reviews(user_id);

-- Indexes cho bảng settings
CREATE INDEX idx_settings_setting_key ON settings(setting_key);

-- Indexes cho bảng contacts
CREATE INDEX idx_contacts_email ON contacts(email);

-- Indexes cho bảng subscriber
CREATE INDEX idx_subscriber_email ON subscriber(email);```

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