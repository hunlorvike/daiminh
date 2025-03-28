using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Comment;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
public class CommentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CommentViewModel> _validator;

    public CommentController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<CommentViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/Comment
    public async Task<IActionResult> Index(CommentFilterViewModel filter)
    {
        ViewData["PageTitle"] = "Quản lý bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Bài viết", "/Admin/Article"),
            ("Bình luận", "")
        };

        var query = _context.Set<Comment>()
            .Include(c => c.Article)
            .Include(c => c.Replies)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(c => c.Content.Contains(filter.SearchTerm) ||
                                    c.AuthorName.Contains(filter.SearchTerm) ||
                                    (c.AuthorEmail != null && c.AuthorEmail.Contains(filter.SearchTerm)));
        }

        if (filter.IsApproved.HasValue)
        {
            query = query.Where(c => c.IsApproved == filter.IsApproved.Value);
        }

        if (filter.ArticleId.HasValue)
        {
            query = query.Where(c => c.ArticleId == filter.ArticleId.Value);
        }

        if (filter.HasParent.HasValue)
        {
            query = query.Where(c => filter.HasParent.Value ? c.ParentId.HasValue : !c.ParentId.HasValue);
        }

        var comments = await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        var viewModels = _mapper.Map<List<CommentListItemViewModel>>(comments);

        // Add reply count to each view model
        foreach (var viewModel in viewModels)
        {
            var comment = comments.First(c => c.Id == viewModel.Id);
            viewModel.ReplyCount = comment.Replies?.Count ?? 0;
        }

        // Get articles for dropdown
        ViewBag.Articles = await _context.Set<Article>()
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Title,
                Selected = filter.ArticleId.HasValue && a.Id == filter.ArticleId.Value
            })
            .ToListAsync();

        ViewBag.Filter = filter;

        return View(viewModels);
    }

    // GET: Admin/Comment/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var comment = await _context.Set<Comment>()
            .Include(c => c.Article)
            .Include(c => c.Parent)
            .Include(c => c.Replies)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chi tiết bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Bài viết", "/Admin/Article"),
            ("Bình luận", "/Admin/Comment"),
            ("Chi tiết", "")
        };

        var viewModel = _mapper.Map<CommentViewModel>(comment);
        viewModel.ArticleTitle = comment.Article?.Title ?? "Không tìm thấy bài viết";
        viewModel.ParentAuthorName = comment.Parent?.AuthorName;

        return View(viewModel);
    }

    // GET: Admin/Comment/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var comment = await _context.Set<Comment>()
            .Include(c => c.Article)
            .Include(c => c.Parent)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chỉnh sửa bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Bài viết", "/Admin/Article"),
            ("Bình luận", "/Admin/Comment"),
            ("Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<CommentViewModel>(comment);
        viewModel.ArticleTitle = comment.Article?.Title ?? "Không tìm thấy bài viết";
        viewModel.ParentAuthorName = comment.Parent?.AuthorName;

        return View(viewModel);
    }

    // POST: Admin/Comment/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CommentViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            // Reload article title for display
            var article = await _context.Set<Article>().FindAsync(viewModel.ArticleId);
            viewModel.ArticleTitle = article?.Title ?? "Không tìm thấy bài viết";

            if (viewModel.ParentId.HasValue)
            {
                var parentComment = await _context.Set<Comment>().FindAsync(viewModel.ParentId.Value);
                viewModel.ParentAuthorName = parentComment?.AuthorName;
            }

            return View(viewModel);
        }

        try
        {
            var comment = await _context.Set<Comment>().FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, comment);

            _context.Update(comment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật bình luận thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CommentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/Comment/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _context.Set<Comment>()
            .Include(c => c.Replies)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return Json(new { success = false, message = "Không tìm thấy bình luận" });
        }

        // Check if there are replies
        if (comment.Replies != null && comment.Replies.Any())
        {
            return Json(new { success = false, message = $"Không thể xóa bình luận này vì có {comment.Replies.Count} phản hồi. Vui lòng xóa các phản hồi trước." });
        }

        _context.Set<Comment>().Remove(comment);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa bình luận thành công" });
    }

    // POST: Admin/Comment/ToggleApproval/5
    [HttpPost]
    public async Task<IActionResult> ToggleApproval(int id)
    {
        var comment = await _context.Set<Comment>().FindAsync(id);

        if (comment == null)
        {
            return Json(new { success = false, message = "Không tìm thấy bình luận" });
        }

        comment.IsApproved = !comment.IsApproved;
        await _context.SaveChangesAsync();

        return Json(new { success = true, approved = comment.IsApproved });
    }

    // GET: Admin/Comment/Reply/5
    public async Task<IActionResult> Reply(int id)
    {
        var parentComment = await _context.Set<Comment>()
            .Include(c => c.Article)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (parentComment == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Trả lời bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Bài viết", "/Admin/Article"),
            ("Bình luận", "/Admin/Comment"),
            ("Trả lời", "")
        };

        var viewModel = new CommentViewModel
        {
            ParentId = parentComment.Id,
            ArticleId = parentComment.ArticleId,
            ArticleTitle = parentComment.Article?.Title ?? "Không tìm thấy bài viết",
            ParentAuthorName = parentComment.AuthorName,
            IsApproved = true,
            AuthorName = "Admin", // Default value for admin reply
            AuthorEmail = "admin@example.com" // Default value for admin reply
        };

        return View(viewModel);
    }

    // POST: Admin/Comment/Reply
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reply(CommentViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            // Reload article title and parent author name for display
            var article = await _context.Set<Article>().FindAsync(viewModel.ArticleId);
            viewModel.ArticleTitle = article?.Title ?? "Không tìm thấy bài viết";

            if (viewModel.ParentId.HasValue)
            {
                var parentComment = await _context.Set<Comment>().FindAsync(viewModel.ParentId.Value);
                viewModel.ParentAuthorName = parentComment?.AuthorName;
            }

            return View(viewModel);
        }

        var comment = _mapper.Map<Comment>(viewModel);
        comment.Id = 0; // Ensure new entity

        _context.Add(comment);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Trả lời bình luận thành công";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Comment/GetPendingCount
    [HttpGet]
    public async Task<IActionResult> GetPendingCount()
    {
        var count = await _context.Set<Comment>()
            .Where(c => !c.IsApproved)
            .CountAsync();

        return Json(count);
    }

    private async Task<bool> CommentExists(int id)
    {
        return await _context.Set<Comment>().AnyAsync(e => e.Id == id);
    }
}