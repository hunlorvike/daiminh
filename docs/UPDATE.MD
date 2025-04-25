**1. Trang chủ (Homepage):**

*   **Banner/Slider:**
    *   *Hiện trạng:* Chưa có entity cụ thể (`Banner`, `Slide`). Có thể quản lý các URL ảnh/link đơn giản qua `Setting`, nhưng nếu cần quản lý phức tạp (tiêu đề, mô tả, nút bấm, thứ tự cho mỗi slide) thì **còn thiếu**.
    *   *Đánh giá:* **Cần bổ sung entity `Banner/Slide` nếu yêu cầu quản lý slider động và phức tạp.** Nếu chỉ là ảnh tĩnh, có thể dùng `Setting`.
*   **Giới thiệu ngắn:**
    *   *Hiện trạng:* Nội dung này thường tĩnh hoặc có thể lấy từ một `Setting` có `Key = "HomePageIntro"`.
    *   *Đánh giá:* **Phù hợp** (qua `Setting` hoặc nội dung tĩnh).
*   **Sản phẩm nổi bật:**
    *   *Hiện trạng:* Lấy các `Product` có `IsFeatured = true`. Có thể lấy thêm thông tin `Category`, `Brand`, và ảnh chính (`ProductImage` có `IsMain = true` hoặc ảnh đầu tiên). Cần lấy giá từ `ProductVariation` (có thể là biến thể `IsDefault = true`).
    *   *Đánh giá:* **Phù hợp.** Cần logic xử lý để lấy giá và ảnh đại diện chính xác.
*   **Dịch vụ nổi bật:**
    *   *Hiện trạng:* Lấy các `Article` có `IsFeatured = true` và thuộc `Category` có `Type = Article` và `Slug = "dich-vu"` (hoặc tương tự). Cần lấy `Title`, `Summary`, `ThumbnailImage`.
    *   *Đánh giá:* **Phù hợp.**
*   **Tại sao chọn chúng tôi?:**
    *   *Hiện trạng:* Nội dung tĩnh hoặc quản lý qua các `Setting` riêng lẻ (vd: `ReasonToChoose1`, `ReasonToChoose2`...).
    *   *Đánh giá:* **Phù hợp.**
*   **Dự án/Công trình tiêu biểu (Mini gallery):**
    *   *Hiện trạng:* Lấy các `Article` có `IsFeatured = true` và thuộc `Category` có `Type = Article` và `Slug = "du-an"` (hoặc tương tự). Cần lấy `Title`, `FeaturedImage`.
    *   *Đánh giá:* **Phù hợp.**
*   **Phản hồi khách hàng (Testimonials):**
    *   *Hiện trạng:* Lấy các `Testimonial` có `IsActive = true`, sắp xếp theo `OrderIndex`. Hiển thị `ClientName`, `Content`, `Rating`, `ClientAvatar`.
    *   *Đánh giá:* **Phù hợp.** *Lưu ý:* `ClientAvatar` là URL, nên tích hợp với `MediaFile`.
*   **Tin tức/Bài viết mới nhất (Tư vấn & Kiến thức):**
    *   *Hiện trạng:* Lấy các `Article` mới nhất (`PublishedAt` hoặc `CreatedAt`) có `Status = Published` và thuộc `Category` có `Type = Article` và `Slug` là "kien-thuc", "tu-van", "tin-tuc"... Hiển thị `Title`, `Summary`, `ThumbnailImage`, `PublishedAt`.
    *   *Đánh giá:* **Phù hợp.**
*   **Thông tin liên hệ nhanh:**
    *   *Hiện trạng:* Lấy các giá trị từ `Setting` (ví dụ: `ContactPhone`, `ContactAddress`).
    *   *Đánh giá:* **Phù hợp.**

**2. Trang Giới thiệu:**

*   *Hiện trạng:* Thường là nội dung tĩnh. Nếu muốn quản lý động, có thể tạo một `Article` đặc biệt thuộc `Category` "Giới thiệu".
*   *Đánh giá:* **Phù hợp** (Nội dung tĩnh hoặc dùng `Article`).

**3. Trang Sản phẩm:**

*   **Danh sách sản phẩm (Theo Category):**
    *   *Hiện trạng:* Lọc `Product` theo `CategoryId` (với `Category.Type = Product`). Hiển thị `Name`, `Slug`, ảnh chính (`ProductImage` có `IsMain=true`), giá (cần lấy từ `ProductVariation` mặc định).
    *   *Đánh giá:* **Phù hợp.**
*   **Bộ lọc & Sắp xếp:**
    *   *Hiện trạng:* Lọc theo `BrandId`. Lọc theo `CategoryId`. Lọc theo `AttributeValue` (cần truy vấn qua `ProductVariation` -> `ProductVariationAttributeValue` -> `AttributeValue`). Sắp xếp theo `Name`, `CreatedAt`, giá (cần join với `ProductVariation`).
    *   *Đánh giá:* **Phù hợp**, nhưng logic query lọc theo Attribute sẽ phức tạp.
*   **Trang chi tiết sản phẩm:**
    *   *Hiện trạng:* Hiển thị thông tin từ `Product` (`Name`, `Description`, `Specifications`, `Usage`...). Hiển thị `Brand.Name`. Hiển thị `Category.Name`. Hiển thị danh sách `ProductImage`. Hiển thị các `Attribute` áp dụng (qua `ProductAttribute`) và các `AttributeValue` tương ứng để người dùng chọn. Hiển thị giá/kho/SKU/ảnh của `ProductVariation` tương ứng với lựa chọn thuộc tính. Hiển thị danh sách `ProductReview` có `Status = Approved`. Hiển thị các `Tag` (qua `ProductTag`). Hiển thị các `Article` liên quan (qua `ArticleProduct`).
    *   *Đánh giá:* **Phù hợp.** Cấu trúc entity hỗ trợ đầy đủ các thông tin cần thiết.
*   **Đánh giá sản phẩm:**
    *   *Hiện trạng:* Hiển thị danh sách `ProductReview` đã duyệt. Form để người dùng (đã đăng nhập - cần lấy `UserId`) nhập `Rating`, `Title`, `Content` và lưu vào `ProductReview` với `Status = Pending`.
    *   *Đánh giá:* **Phù hợp.**

**4. Trang Dịch vụ / Dự án / Tư vấn & Kiến thức (Module Article):**

*   **Danh sách bài viết (Theo Category):**
    *   *Hiện trạng:* Lọc `Article` theo `CategoryId` (với `Category.Type = Article`). Hiển thị `Title`, `Summary`, `ThumbnailImage`, `PublishedAt`, `AuthorName`.
    *   *Đánh giá:* **Phù hợp.**
*   **Trang chi tiết bài viết (Dịch vụ/Dự án/Kiến thức):**
    *   *Hiện trạng:* Hiển thị `Title`, `Content`, `FeaturedImage`, `PublishedAt`, `AuthorName`, `AuthorAvatar`, `ViewCount`, `Category.Name`. Hiển thị danh sách `Tag` (qua `ArticleTag`). Hiển thị danh sách `Product` liên quan (qua `ArticleProduct`).
    *   *Đánh giá:* **Phù hợp.** *Lưu ý:* `AuthorAvatar`, `FeaturedImage`, `ThumbnailImage` là URL, nên tích hợp `MediaFile`. **Vẫn thiếu Gallery ảnh cho bài viết (đặc biệt quan trọng cho Dự án).**
*   **Trang FAQ:**
    *   *Hiện trạng:* Lấy danh sách `FAQ` có `IsActive = true`, có thể nhóm theo `CategoryId` (với `Category.Type = FAQ`).
    *   *Đánh giá:* **Phù hợp.**

**5. Trang Liên hệ:**

*   **Thông tin liên hệ:**
    *   *Hiện trạng:* Lấy từ các `Setting` như `CompanyName`, `ContactAddress`, `ContactPhone`, `ContactEmail`, `ContactMapEmbed`.
    *   *Đánh giá:* **Phù hợp.**
*   **Form liên hệ:**
    *   *Hiện trạng:* Dữ liệu người dùng nhập (FullName, Email, Phone, Subject, Message) sẽ được lưu vào entity `Contact`.
    *   *Đánh giá:* **Phù hợp.**

**6. Các Chức năng/Module khác:**

*   **Quản lý Người dùng (`User`):**
    *   *Hiện trạng:* Lưu thông tin cơ bản (Username, Email, Hash MK, FullName). Có seed user admin.
    *   *Đánh giá:* **Phù hợp** cho nhu cầu cơ bản. Nếu cần phân quyền chi tiết (Editor, Admin...) thì nên dùng ASP.NET Core Identity.
*   **Quản lý Media (`MediaFile`):**
    *   *Hiện trạng:* Có entity `MediaFile` để lưu thông tin file.
    *   *Đánh giá:* **Cần tích hợp sâu hơn.** Như đã đề cập, các trường URL ảnh trong các entity khác (`ProductImage`, `Article`, `Brand`, `Testimonial`, `ProductVariation`) nên được thay thế bằng `MediaFileId` (FK).
*   **Quản lý Cài đặt (`Setting`):**
    *   *Hiện trạng:* Thiết kế tốt, linh hoạt với Key, Category, Type, Value. Có seed dữ liệu cơ bản.
    *   *Đánh giá:* **Phù hợp.**
*   **Quản lý Tags (`Tag`, `ProductTag`, `ArticleTag`):**
    *   *Hiện trạng:* Phân loại Tag theo Type (Product, Article). Các bảng nối được thiết kế đúng.
    *   *Đánh giá:* **Phù hợp.**
*   **Quản lý Newsletter (`Newsletter`):**
    *   *Hiện trạng:* Lưu email đăng ký nhận tin.
    *   *Đánh giá:* **Phù hợp.**

