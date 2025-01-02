using core.Entities.Ecommerce;
using core.Entities.Shared;

namespace core.Entities.CMS;

public class Tag : ActivatableEntity
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}