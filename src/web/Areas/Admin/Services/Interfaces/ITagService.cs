using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface ITagService
{
    Task<IPagedList<TagListItemViewModel>> GetPagedTagsAsync(TagFilterViewModel filter, int pageNumber, int pageSize);

    Task<TagViewModel?> GetTagByIdAsync(int id);

    Task<OperationResult<int>> CreateTagAsync(TagViewModel viewModel);

    Task<OperationResult> UpdateTagAsync(TagViewModel viewModel);

    Task<OperationResult> DeleteTagAsync(int id);

    Task<bool> IsNameUniqueAsync(string name, TagType? type, int? ignoreId = null);

    Task<(int productCount, int articleCount, TagType type, string name)> CheckTagRelationsAsync(int tagId);

    Task<List<SelectListItem>> GetTagSelectListAsync(TagType type, List<int>? selectedValues = null);
}