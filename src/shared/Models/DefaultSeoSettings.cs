using System.ComponentModel.DataAnnotations;

namespace shared.Models;

public class DefaultSeoSettings
{
    [StringLength(100)]
    public string MetaTitleSuffix { get; set; } = " | Sơn Đại Minh";
    [StringLength(300)]
    public string MetaDescription { get; set; } = "Sơn Đại Minh - Chuyên cung cấp các loại sơn và vật liệu chống thấm chính hãng. Tư vấn thi công chuyên nghiệp.";
    [StringLength(200)]
    public string MetaKeywords { get; set; } = "sơn, chống thấm, sơn nhà, vật liệu xây dựng, sơn dulux, sơn jotun, sơn kova, sơn nippon";
    public string? OgImage { get; set; } = "/img/placeholder.svg";
}
