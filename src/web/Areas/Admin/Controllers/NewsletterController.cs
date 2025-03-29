using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Newsletter;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class NewsletterController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<NewsletterViewModel> _validator;

    public NewsletterController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<NewsletterViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/Newsletter
    public async Task<IActionResult> Index(bool? isActive = null, string? searchTerm = null)
    {
        ViewData["PageTitle"] = "Quản lý đăng ký nhận tin";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đăng ký nhận tin", "")
        };

        IQueryable<Newsletter> query = _context.Set<Newsletter>().AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(n => n.IsActive == isActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(n =>
                n.Email.Contains(searchTerm) ||
                (n.Name != null && n.Name.Contains(searchTerm)));
        }

        var newsletters = await query
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        var viewModels = _mapper.Map<List<NewsletterListItemViewModel>>(newsletters);

        ViewBag.IsActiveFilter = isActive;
        ViewBag.SearchTerm = searchTerm;
        ViewBag.ActiveCount = await _context.Set<Newsletter>().CountAsync(n => n.IsActive);
        ViewBag.InactiveCount = await _context.Set<Newsletter>().CountAsync(n => !n.IsActive);

        return View(viewModels);
    }

    // GET: Admin/Newsletter/Details/5
    public async Task<IActionResult> Details(int id)
    {
        ViewData["PageTitle"] = "Chi tiết đăng ký nhận tin";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đăng ký nhận tin", "/Admin/Newsletter"),
            ("Chi tiết", "")
        };

        var newsletter = await _context.Set<Newsletter>().FindAsync(id);

        if (newsletter == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<NewsletterViewModel>(newsletter);

        return View(viewModel);
    }

    // GET: Admin/Newsletter/Create
    public IActionResult Create()
    {
        ViewData["PageTitle"] = "Thêm đăng ký nhận tin";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đăng ký nhận tin", "/Admin/Newsletter"),
            ("Thêm mới", "")
        };

        return View(new NewsletterViewModel());
    }

    // POST: Admin/Newsletter/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsletterViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            ViewData["PageTitle"] = "Thêm đăng ký nhận tin";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
            {
                ("Đăng ký nhận tin", "/Admin/Newsletter"),
                ("Thêm mới", "")
            };

            return View(viewModel);
        }

        // Kiểm tra email đã tồn tại chưa
        if (await _context.Set<Newsletter>().AnyAsync(n => n.Email == viewModel.Email))
        {
            ModelState.AddModelError("Email", "Email này đã đăng ký nhận tin");

            ViewData["PageTitle"] = "Thêm đăng ký nhận tin";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
            {
                ("Đăng ký nhận tin", "/Admin/Newsletter"),
                ("Thêm mới", "")
            };

            return View(viewModel);
        }

        var newsletter = _mapper.Map<Newsletter>(viewModel);
        newsletter.CreatedAt = DateTime.UtcNow;
        newsletter.ConfirmedAt = DateTime.UtcNow; // Đăng ký từ admin mặc định đã xác nhận

        _context.Set<Newsletter>().Add(newsletter);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm đăng ký nhận tin thành công";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Newsletter/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["PageTitle"] = "Sửa đăng ký nhận tin";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Đăng ký nhận tin", "/Admin/Newsletter"),
            ("Sửa", "")
        };

        var newsletter = await _context.Set<Newsletter>().FindAsync(id);

        if (newsletter == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<NewsletterViewModel>(newsletter);

        return View(viewModel);
    }

    // POST: Admin/Newsletter/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NewsletterViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            ViewData["PageTitle"] = "Sửa đăng ký nhận tin";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
            {
                ("Đăng ký nhận tin", "/Admin/Newsletter"),
                ("Sửa", "")
            };

            return View(viewModel);
        }

        try
        {
            var newsletter = await _context.Set<Newsletter>().FindAsync(id);

            if (newsletter == null)
            {
                return NotFound();
            }

            // Kiểm tra email đã tồn tại chưa (nếu thay đổi email)
            if (newsletter.Email != viewModel.Email &&
                await _context.Set<Newsletter>().AnyAsync(n => n.Email == viewModel.Email))
            {
                ModelState.AddModelError("Email", "Email này đã đăng ký nhận tin");

                ViewData["PageTitle"] = "Sửa đăng ký nhận tin";
                ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
                {
                    ("Đăng ký nhận tin", "/Admin/Newsletter"),
                    ("Sửa", "")
                };

                return View(viewModel);
            }

            newsletter.Email = viewModel.Email;
            newsletter.Name = viewModel.Name;
            newsletter.IsActive = viewModel.IsActive;

            // Cập nhật trạng thái đăng ký/hủy đăng ký
            if (newsletter.IsActive && !viewModel.IsActive)
            {
                newsletter.UnsubscribedAt = DateTime.UtcNow;
            }
            else if (!newsletter.IsActive && viewModel.IsActive)
            {
                newsletter.UnsubscribedAt = null;
                if (newsletter.ConfirmedAt == null)
                {
                    newsletter.ConfirmedAt = DateTime.UtcNow;
                }
            }

            newsletter.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật đăng ký nhận tin thành công";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await NewsletterExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    // POST: Admin/Newsletter/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var newsletter = await _context.Set<Newsletter>().FindAsync(id);

        if (newsletter == null)
        {
            return Json(new { success = false, message = "Không tìm thấy đăng ký nhận tin" });
        }

        _context.Set<Newsletter>().Remove(newsletter);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa đăng ký nhận tin thành công" });
    }

    // POST: Admin/Newsletter/ToggleStatus/5
    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var newsletter = await _context.Set<Newsletter>().FindAsync(id);

        if (newsletter == null)
        {
            return Json(new { success = false, message = "Không tìm thấy đăng ký nhận tin" });
        }

        newsletter.IsActive = !newsletter.IsActive;

        if (newsletter.IsActive)
        {
            newsletter.UnsubscribedAt = null;
            if (newsletter.ConfirmedAt == null)
            {
                newsletter.ConfirmedAt = DateTime.UtcNow;
            }
        }
        else
        {
            newsletter.UnsubscribedAt = DateTime.UtcNow;
        }

        newsletter.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            active = newsletter.IsActive,
            message = newsletter.IsActive
                ? "Kích hoạt đăng ký nhận tin thành công"
                : "Hủy đăng ký nhận tin thành công"
        });
    }

    private async Task<bool> NewsletterExists(int id)
    {
        return await _context.Set<Newsletter>().AnyAsync(e => e.Id == id);
    }
}
