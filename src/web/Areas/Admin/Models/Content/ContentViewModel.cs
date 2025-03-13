using System.ComponentModel;

namespace web.Areas.Admin.Models.Content;

public class ContentViewModel
{
    public int Id { get; set; }
    [DisplayName("Loại nội dung")]
    public string? ContentTypeName { get; set; }

    [DisplayName("Tác giả")]
    public string? AuthorName { get; set; }

    [DisplayName("Tiêu đề")]
    public string? Title { get; set; }

    [DisplayName("Đường dẫn")]
    public string? Slug { get; set; }
    [DisplayName("Trạng thái")]
    public string? Status { get; set; }

    [DisplayName("Meta Title")]
    public string? MetaTitle { get; set; }

    [DisplayName("Meta Description")]
    public string? MetaDescription { get; set; }
    [DisplayName("Canonical Url")]
    public string? CanonicalUrl { get; set; }
    [DisplayName("Og Title")]
    public string? OgTitle { get; set; }
    [DisplayName("Og Description")]
    public string? OgDescription { get; set; }

    [DisplayName("Og Image")]
    public string? OgImage { get; set; }

    [DisplayName("Structured Data")]
    public string? StructuredData { get; set; }

    [DisplayName("Ngày tạo")]
    public DateTime? CreatedAt { get; set; }

    [DisplayName("Ngày cập nhật")]
    public DateTime? UpdatedAt { get; set; }
}