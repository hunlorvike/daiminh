using System.ComponentModel;

namespace web.Areas.Admin.Models.Tag
{
    public class TagViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        // CreatedAt should be a formatted date string
        public string CreatedAt { get; set; } = string.Empty;

        // Optional: If you want to display the UpdatedAt field, you can add it similarly
        public string UpdatedAt { get; set; } = string.Empty;

        // Optional: If you want to display the DeletedAt field for soft-deleted tags
        public string? DeletedAt { get; set; } = null;
    }
}