using AutoMapper;
using AutoRegister;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class AdminSearchService : IAdminSearchService
{
    private readonly ApplicationDbContext _context;
    private readonly IUrlHelper _urlHelper;

    public AdminSearchService(ApplicationDbContext context, IUrlHelper urlHelper)
    {
        _context = context;
        _urlHelper = urlHelper;
    }

    public async Task<List<AdminSearchResultItemViewModel>> SearchAsync(string query)
    {
        var results = new List<AdminSearchResultItemViewModel>();
        var lowerQuery = query.ToLower();

        // Search Products
        var products = await _context.Products
            .Where(p => p.Name.ToLower().Contains(lowerQuery) || (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(lowerQuery)))
            .Take(10)
            .Select(p => new AdminSearchResultItemViewModel
            {
                Id = p.Id,
                Title = p.Name,
                Description = p.ShortDescription ?? $"Thương hiệu: {p.Brand!.Name}",
                Url = _urlHelper.Action("Edit", "Product", new { area = "Admin", id = p.Id }) ?? "",
                Type = "Sản phẩm",
                Icon = "ti-package"
            }).ToListAsync();
        results.AddRange(products);

        // Search Articles
        var articles = await _context.Articles
            .Where(a => a.Title.ToLower().Contains(lowerQuery) || (a.Summary != null && a.Summary.ToLower().Contains(lowerQuery)))
            .Take(10)
            .Select(a => new AdminSearchResultItemViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Summary ?? $"Tác giả: {a.AuthorName}",
                Url = _urlHelper.Action("Edit", "Article", new { area = "Admin", id = a.Id }) ?? "",
                Type = "Bài viết",
                Icon = "ti-file-text"
            }).ToListAsync();
        results.AddRange(articles);

        // Search Users
        var users = await _context.Users
            .Where(u => (u.FullName != null && u.FullName.ToLower().Contains(lowerQuery)) || (u.Email != null && u.Email.ToLower().Contains(lowerQuery)))
            .Take(10)
            .Select(u => new AdminSearchResultItemViewModel
            {
                Id = u.Id,
                Title = u.FullName ?? u.Email!,
                Description = $"Email: {u.Email}",
                Url = _urlHelper.Action("Edit", "User", new { area = "Admin", id = u.Id }) ?? "",
                Type = "Người dùng",
                Icon = "ti-user"
            }).ToListAsync();
        results.AddRange(users);

        return results;
    }
}