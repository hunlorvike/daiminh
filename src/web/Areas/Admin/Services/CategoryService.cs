using AutoMapper;
using AutoRegister;
using infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ApplicationDbContext context, IMapper mapper, ILogger<CategoryService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<CategoryListItemViewModel>> GetPagedCategoriesAsync(CategoryFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<domain.Entities.Category> query = _context.Set<domain.Entities.Category>()
                                         .Include(c => c.Parent)
                                         .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(lowerSearchTerm) ||
                                     c.Description != null && c.Description.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(c => c.Type == filter.Type.Value);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == filter.IsActive.Value);
        }

        var allCategoriesMatchingFilter = await query
           .Select(c => new
           {
               Category = c,
               ItemCount = c.Type == CategoryType.Product ? c.Products.Count :
                            c.Type == CategoryType.Article ? c.Articles.Count :
                            c.Type == CategoryType.FAQ ? c.FAQs.Count : 0,
               HasChildren = c.Children!.Any()
           })
            .OrderBy(c => c.Category.Type)
            .ThenBy(c => c.Category.ParentId == null ? 0 : 1)
            .ThenBy(c => c.Category.ParentId)
            .ThenBy(c => c.Category.OrderIndex)
            .ThenBy(c => c.Category.Name)
           .ToListAsync();


        var categoryVMs = allCategoriesMatchingFilter.Select(c =>
        {
            var vm = _mapper.Map<CategoryListItemViewModel>(c.Category);
            vm.ItemCount = c.ItemCount;
            vm.HasChildren = c.HasChildren;
            return vm;
        }).ToList();

        CalculateHierarchyLevels(categoryVMs);

        IPagedList<CategoryListItemViewModel> categoriesPaged = categoryVMs.ToPagedList(pageNumber, pageSize);

        return categoriesPaged;
    }

    public async Task<CategoryViewModel?> GetCategoryByIdAsync(int id)
    {
        var categoryData = await _context.Set<domain.Entities.Category>()
            .Where(c => c.Id == id)
            .Select(c => new
            {
                Category = c,
                ItemCount = c.Type == CategoryType.Product ? c.Products.Count :
                            c.Type == CategoryType.Article ? c.Articles.Count :
                            c.Type == CategoryType.FAQ ? c.FAQs.Count : 0,
                HasChildren = c.Children!.Any()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (categoryData == null) return null;

        CategoryViewModel viewModel = _mapper.Map<CategoryViewModel>(categoryData.Category);
        viewModel.ItemCount = categoryData.ItemCount;
        viewModel.HasChildren = categoryData.HasChildren;

        return viewModel;
    }

    public async Task<OperationResult<int>> CreateCategoryAsync(CategoryViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!, viewModel.Type))
        {
            return OperationResult<int>.FailureResult(message: "Slug này đã tồn tại cho loại danh mục này.", errors: new List<string> { "Slug này đã tồn tại cho loại danh mục này." });
        }

        var category = _mapper.Map<domain.Entities.Category>(viewModel);
        _context.Add(category);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created Category: ID={Id}, Name={Name}, Type={Type}", category.Id, category.Name, category.Type);
            return OperationResult<int>.SuccessResult(category.Id, $"Thêm danh mục '{category.Name}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo danh mục: {Name}", viewModel.Name);
            if (ex.InnerException?.Message?.Contains("UQ_Category_Slug_Type", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult<int>.FailureResult(message: "Slug này đã tồn tại cho loại danh mục này.", errors: new List<string> { "Slug này đã tồn tại cho loại danh mục này." });
            }
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi lưu danh mục.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi lưu danh mục." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo danh mục.");
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi lưu danh mục.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi lưu danh mục." });
        }
    }

    public async Task<OperationResult> UpdateCategoryAsync(CategoryViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!, viewModel.Type, viewModel.Id))
        {
            return OperationResult.FailureResult(message: "Slug đã được sử dụng.", errors: new List<string> { "Slug đã được sử dụng." });
        }

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == viewModel.Id);
        if (category == null)
        {
            _logger.LogWarning("Category not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy danh mục để cập nhật.");
        }

        _mapper.Map(viewModel, category);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated Category: ID={Id}, Name={Name}, Type={Type}", category.Id, category.Name, category.Type);
            return OperationResult.SuccessResult($"Cập nhật danh mục '{category.Name}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật danh mục: ID = {Id}, Name = {Name}", viewModel.Id, viewModel.Name);
            if (ex.InnerException?.Message?.Contains("UQ_Category_Slug_Type", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult(message: "Slug đã được sử dụng.", errors: new List<string> { "Slug đã được sử dụng." });
            }
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi cập nhật danh mục.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi cập nhật danh mục." });
        }
    }

    public async Task<OperationResult> DeleteCategoryAsync(int id)
    {
        var checkResult = await CheckCategoryRelationsAsync(id);

        if (checkResult.name == null)
        {
            _logger.LogWarning("Category not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy danh mục.");
        }

        if (checkResult.hasChildren)
        {
            _logger.LogWarning("Cannot delete Category {Name} (ID: {Id}) due to children.", checkResult.name, id);
            return OperationResult.FailureResult($"Không thể xóa danh mục '{checkResult.name}' vì nó chứa danh mục con.");
        }

        if (checkResult.itemCount > 0)
        {
            string itemTypeName = checkResult.type.GetDisplayName().ToLowerInvariant();
            _logger.LogWarning("Cannot delete Category {Name} (ID: {Id}) due to {ItemCount} related items.", checkResult.name, id, checkResult.itemCount);
            return OperationResult.FailureResult($"Không thể xóa danh mục '{checkResult.name}' vì đang được sử dụng bởi {checkResult.itemCount} {itemTypeName}.");
        }

        var categoryToDelete = new domain.Entities.Category { Id = id };
        _context.Entry(categoryToDelete).State = EntityState.Deleted;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Category: ID={Id}, Name={Name}", id, checkResult.name);
            return OperationResult.SuccessResult($"Xóa danh mục '{checkResult.name}' thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa danh mục: ID {Id}, Name: {Name}", id, checkResult.name);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa danh mục.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa danh mục." });
        }
    }

    public async Task<List<SelectListItem>> GetParentCategorySelectListAsync(CategoryType categoryType, int? selectedValue = null, int? excludeCategoryId = null)
    {
        var query = _context.Set<domain.Entities.Category>()
                     .Where(c => c.Type == categoryType)
                     .AsNoTracking();

        if (excludeCategoryId.HasValue && excludeCategoryId.Value > 0)
        {
            var allCategoriesOfType = await query.ToListAsync();
            var idsToExclude = new HashSet<int>();

            var categoryToExclude = allCategoriesOfType.FirstOrDefault(c => c.Id == excludeCategoryId.Value);

            if (categoryToExclude != null)
            {
                idsToExclude.Add(categoryToExclude.Id);
                Queue<int> queue = new Queue<int>();
                queue.Enqueue(categoryToExclude.Id);

                while (queue.Count > 0)
                {
                    var currentId = queue.Dequeue();
                    var children = allCategoriesOfType.Where(c => c.ParentId == currentId).ToList();
                    foreach (var child in children)
                    {
                        if (!idsToExclude.Contains(child.Id))
                        {
                            idsToExclude.Add(child.Id);
                            queue.Enqueue(child.Id);
                        }
                    }
                }
            }

            query = query.Where(c => !idsToExclude.Contains(c.Id));
        }

        query = query.Where(c => c.IsActive);


        var categories = await query
            .OrderBy(c => c.OrderIndex)
            .ThenBy(c => c.Name)
            .Select(c => new { c.Id, c.Name })
            .ToListAsync();

        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Chọn danh mục cha (để trống nếu là gốc) --", Selected = !selectedValue.HasValue }
        };

        items.AddRange(categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name,
            Selected = selectedValue.HasValue && c.Id == selectedValue.Value
        }));

        return items;
    }

    public async Task<bool> IsSlugUniqueAsync(string slug, CategoryType type, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(slug)) return false;

        var lowerSlug = slug.Trim().ToLower();
        var query = _context.Set<domain.Entities.Category>()
                            .Where(c => c.Slug.ToLower() == lowerSlug && c.Type == type);

        if (ignoreId.HasValue && ignoreId.Value > 0)
        {
            query = query.Where(c => c.Id != ignoreId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<(bool hasChildren, int itemCount, CategoryType type, string name)> CheckCategoryRelationsAsync(int categoryId)
    {
        var categoryData = await _context.Set<domain.Entities.Category>()
          .Where(c => c.Id == categoryId)
          .Select(c => new
          {
              Category = c,
              HasChildren = c.Children!.Any(),
              ItemCount = c.Type == CategoryType.Product ? c.Products.Count :
                          c.Type == CategoryType.Article ? c.Articles.Count :
                          c.Type == CategoryType.FAQ ? c.FAQs.Count : 0,
          })
          .AsNoTracking()
          .FirstOrDefaultAsync();

        if (categoryData == null) return (false, 0, CategoryType.Product, null!);

        return (categoryData.HasChildren, categoryData.ItemCount, categoryData.Category.Type, categoryData.Category.Name);
    }


    private void CalculateHierarchyLevels(List<CategoryListItemViewModel> categories)
    {
        var categoryDict = categories.ToDictionary(c => c.Id);
        foreach (var category in categories)
        {
            int level = 0;
            int? currentParentId = category.ParentId;
            HashSet<int> visitedParents = new HashSet<int>();
            while (currentParentId.HasValue && categoryDict.ContainsKey(currentParentId.Value))
            {
                if (visitedParents.Contains(currentParentId.Value)) break;
                visitedParents.Add(currentParentId.Value);

                level++;
                currentParentId = categoryDict[currentParentId.Value].ParentId;
                if (level > 100) break;
            }
            category.Level = level;
        }
    }
}