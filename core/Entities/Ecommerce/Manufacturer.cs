using core.Entities.Shared;

namespace core.Entities.Ecommerce;

public class Manufacturer : ActivatableEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Logo { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}