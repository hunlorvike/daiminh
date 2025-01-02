using core.Entities.Ecommerce;
using core.Entities.Shared;

namespace core.Entities.CMS;

public class Category : ActivatableEntity
{
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public int DisplayOrder { get; set; }

    public Category? Parent { get; set; }
    public ICollection<Category> Children { get; set; } = new List<Category>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}