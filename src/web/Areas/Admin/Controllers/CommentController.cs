using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Comment;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize] // Add specific roles for moderation if needed
public class CommentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CommentViewModel> _validator;
    private readonly ILogger<CommentController> _logger;

    public CommentController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<CommentViewModel> validator,
         ILogger<CommentController> logger)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    // GET: Admin/Comment
    public async Task<IActionResult> Index(string? searchTerm = null, bool? isApproved = null, int? articleId = null)
    {
        ViewData["PageTitle"] = "Quản lý Bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bình luận", "") };

        var query = _context.Set<Comment>()
                            .Include(c => c.Article) // For ArticleTitle
                            .Include(c => c.Replies) // For ReplyCount
                            .Where(c => c.ParentId == null) // Only show top-level comments
                            .AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.AuthorName.Contains(searchTerm) || c.AuthorEmail.Contains(searchTerm) || c.Content.Contains(searchTerm));
        }
        if (isApproved.HasValue)
        {
            query = query.Where(c => c.IsApproved == isApproved.Value);
        }
        if (articleId.HasValue)
        {
            query = query.Where(c => c.ArticleId == articleId.Value);
        }

        var comments = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
        var viewModels = _mapper.Map<List<CommentListItemViewModel>>(comments);

        // TODO: Load filter dropdowns if needed (e.g., Articles list)

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedApprovedStatus = isApproved;
        ViewBag.SelectedArticleId = articleId;

        return View(viewModels);
    }

    // GET: Admin/Comment/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var comment = await _context.Set<Comment>()
                                 .Include(c => c.Article) // Include article for title display
                                 .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null) return NotFound();

        var viewModel = _mapper.Map<CommentViewModel>(comment);

        ViewData["PageTitle"] = "Chỉnh sửa Bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bình luận", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };

        return View(viewModel);
    }

    // POST: Admin/Comment/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CommentViewModel viewModel)
    {
        if (id != viewModel.Id) return BadRequest();

        var validationResult = await _validator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            ViewData["PageTitle"] = "Chỉnh sửa Bình luận";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bình luận", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };
            return View(viewModel); // Return with errors
        }

        var comment = await _context.Set<Comment>().FindAsync(id);
        if (comment == null) return NotFound();

        _mapper.Map(viewModel, comment); // Map editable fields

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật bình luận thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Comment ID {CommentId}", id);
            ModelState.AddModelError("", "Không thể lưu thay đổi.");
            ViewData["PageTitle"] = "Chỉnh sửa Bình luận";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bình luận", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };
            return View(viewModel);
        }
    }

    // POST: Admin/Comment/Approve/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        return await UpdateApprovalStatus(id, true);
    }

    // POST: Admin/Comment/Unapprove/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unapprove(int id)
    {
        return await UpdateApprovalStatus(id, false);
    }


    // POST: Admin/Comment/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _context.Set<Comment>()
                                 .Include(c => c.Replies) // Need to handle replies if Restrict
                                 .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return Json(new { success = false, message = "Không tìm thấy bình luận." });
        }

        // If using Restrict on ParentId, need to handle replies manually or change constraint
        // For now, assume Cascade on ArticleId handles replies in DB if Article is deleted
        // If deleting comment directly, and ParentId has Restrict, need logic here.
        // Let's assume replies should be deleted with parent comment for simplicity (change OnDelete behavior if needed)
        if (comment.Replies != null && comment.Replies.Any())
        {
            _context.Comments.RemoveRange(comment.Replies); // Remove replies first if needed
        }

        _context.Remove(comment);

        try
        {
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Xóa bình luận thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Comment ID {CommentId}", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa bình luận." });
        }
    }

    // --- Helper: Update Approval ---
    private async Task<IActionResult> UpdateApprovalStatus(int id, bool approve)
    {
        var comment = await _context.Set<Comment>().FindAsync(id);
        if (comment == null)
        {
            return Json(new { success = false, message = "Không tìm thấy bình luận." });
        }

        comment.IsApproved = approve;
        try
        {
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = approve ? "Duyệt bình luận thành công." : "Bỏ duyệt bình luận thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating approval status for Comment ID {CommentId}", id);
            return Json(new { success = false, message = "Lỗi cập nhật trạng thái duyệt." });
        }
    }
}