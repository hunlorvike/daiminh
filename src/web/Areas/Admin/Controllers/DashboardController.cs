using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Controllers;


[Area("Admin")]
//[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel
        {
            TotalProducts = await _context.Products.CountAsync(),
            TotalArticles = await _context.Articles.CountAsync(),
            TotalProjects = await _context.Projects.CountAsync(),
            TotalContacts = await _context.Contacts.CountAsync(),

            NewContacts = await _context.Contacts
                .Where(c => c.Status == shared.Enums.ContactStatus.New)
                .CountAsync(),

            RecentArticles = await _context.Articles
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .Select(a => new RecentArticleViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Slug = a.Slug,
                    AuthorName = a.AuthorName,
                    CreatedAt = a.CreatedAt,
                    Status = a.Status
                })
                .ToListAsync(),

            RecentContacts = await _context.Contacts
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .Select(c => new RecentContactViewModel
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    Subject = c.Subject,
                    CreatedAt = c.CreatedAt,
                    Status = c.Status
                })
                .ToListAsync()
        };

        return View(viewModel);
    }
}