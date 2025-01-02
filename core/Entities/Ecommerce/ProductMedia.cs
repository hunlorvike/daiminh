using core.Common.Enums;
using core.Entities.Shared;

namespace core.Entities.Ecommerce;

public class ProductMedia : BaseEntity
{
    public Guid ProductId { get; set; }
    public MediaType MediaType { get; set; }
    public string MediaUrl { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public bool IsMain { get; set; }

    public Product Product { get; set; } = null!;
}