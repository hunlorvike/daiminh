using System.ComponentModel;

namespace web.Areas.Client.Models.ContentType
{
    public class ContentTypeViewModel
    {
        public int Id { get; set; }
        [DisplayName("Loại bài viết")] public string? Name { get; set; }
        [DisplayName("Đường dẫn")] public string? Slug { get; set; }
        [DisplayName("Ngày tạo")] public DateTime? CreatedAt { get; set; }
    }
}
