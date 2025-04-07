using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Tag;

public class TagViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên thẻ không được để trống")]
    [Display(Name = "Tên thẻ", Prompt = "Nhập tên thẻ (tag)")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Slug không được để trống")]
    [Display(Name = "Slug", Prompt = "Nhập slug URL (không dấu, chữ thường, gạch ngang)")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả cho thẻ (không bắt buộc)")]
    public string? Description { get; set; }

    [Display(Name = "Loại thẻ")]
    public TagType Type { get; set; } = TagType.Product;
}
