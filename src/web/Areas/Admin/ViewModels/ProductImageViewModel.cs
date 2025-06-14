using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.ViewModels;

public class ProductImageViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "URL Hình ảnh", Prompt = "URL đầy đủ của hình ảnh")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.ImageUrl)]
    public string ImageUrl { get; set; } = string.Empty;

    [Display(Name = "Thứ tự hiển thị")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm.")]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Ảnh chính")]
    public bool IsMain { get; set; } = false;
}