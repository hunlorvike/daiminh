using application.constracts.Dtos.Shared;

namespace application.constracts.Dtos.Brand;

public abstract class BrandDtoBase : BaseDto<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? Website { get; set; }
    public bool IsActive { get; set; }
}
