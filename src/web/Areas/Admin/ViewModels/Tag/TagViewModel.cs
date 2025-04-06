using System.ComponentModel.DataAnnotations;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Tag;

public class TagViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên thẻ không được để trống")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Slug không được để trống")]
    public string Slug { get; set; }

    public string? Description { get; set; }

    public TagType Type { get; set; } = TagType.Product;
}