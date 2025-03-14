using AutoMapper;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Constants;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Gallery;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class GalleryController(
    ApplicationDbContext context,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(mapper, serviceProvider, configuration);

public partial class GalleryController
{
    public async Task<IActionResult> Index(int? folderId = null)
    {
        GalleryViewModel viewModel = new()
        {
            CurrentFolder = folderId.HasValue
                    ? await context.Set<Folder>().FindAsync(folderId.Value)
                    : null,
            Folders = await context.Set<Folder>()
                    .Where(f => f.ParentId == folderId)
                    .OrderBy(f => f.Name)
                    .ToListAsync(),
            MediaFiles = await context.Set<MediaFile>()
                    .Where(m => m.FolderId == folderId)
            .OrderBy(m => m.Name)
                    .ToListAsync(),
            Breadcrumbs = await GetBreadcrumbsAsync(folderId)
        };
        return View(viewModel);
    }
}

public partial class GalleryController
{
    private async Task<List<Folder>> GetBreadcrumbsAsync(int? folderId)
    {
        var breadcrumbs = new List<Folder>();
        if (folderId == null)
            return breadcrumbs;

        var folder = await context.Set<Folder>().FindAsync(folderId);
        if (folder != null)
        {
            breadcrumbs.Add(folder);
            var parentId = folder.ParentId;

            while (parentId != null)
            {
                var parent = await context.Set<Folder>().FindAsync(parentId);
                if (parent != null)
                {
                    breadcrumbs.Insert(0, parent);
                    parentId = parent.ParentId;
                }
                else
                {
                    break;
                }
            }
        }

        return breadcrumbs;
    }
}


