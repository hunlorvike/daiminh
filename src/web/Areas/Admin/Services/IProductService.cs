using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services;

public interface IProductService
{
    /// <summary>
    /// Gets a paged list of products based on filter criteria.
    /// Includes minimal related data needed for list items (Category, Brand, primary Image, counts).
    /// </summary>
    Task<IPagedList<ProductListItemViewModel>> GetPagedProductsAsync(ProductFilterViewModel filter, int pageNumber, int pageSize);

    /// <summary>
    /// Gets a single product for editing by its ID.
    /// Includes all related data required by the ProductViewModel (Images, Attributes, Tags, Articles).
    /// Also populates necessary SelectLists for the ViewModel.
    /// </summary>
    Task<ProductViewModel?> GetProductByIdAsync(int id);

    /// <summary>
    /// Gets a new ProductViewModel with default values and SelectLists populated for the Create view.
    /// </summary>
    Task<ProductViewModel> GetProductViewModelForCreateAsync();


    /// <summary>
    /// Creates a new product, including relationships (Attributes, Tags, Articles, Images).
    /// </summary>
    Task<OperationResult<int>> CreateProductAsync(ProductViewModel viewModel, string authorId, string authorName); // Include author info

    /// <summary>
    /// Updates an existing product, including relationships (Attributes, Tags, Articles, Images).
    /// </summary>
    Task<OperationResult> UpdateProductAsync(ProductViewModel viewModel);

    /// <summary>
    /// Deletes a product.
    /// </summary>
    Task<OperationResult> DeleteProductAsync(int id);

    /// <summary>
    /// Checks if a slug is unique for a product, optionally ignoring a specific product ID.
    /// </summary>
    Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null);

    /// <summary>
    /// Gets a select list of products.
    /// </summary>
    Task<List<SelectListItem>> GetProductSelectListAsync(int? selectedValue = null);
}
