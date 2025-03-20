using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Product;

public class ProductDeleteRequest
{
    [Required]
    public int Id { get; set; }
}