using System.ComponentModel;

namespace web.Areas.Admin.Models.Tag;

public class TagViewModel
{
    public int Id { get; set; }
    [DisplayName("Tên thẻ")] public string? Name { get; set; }
    [DisplayName("Đường dẫn")] public string? Slug { get; set; }
    [DisplayName("Ngày tạo")] public DateTime? CreatedAt { get; set; }
}