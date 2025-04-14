// --- START OF FILE CommentController.cs --- Updated ---
using AutoMapper;
using AutoMapper.QueryableExtensions; // Added
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Added
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.Comment;
using X.PagedList.EF; // Added

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class CommentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CommentController> _logger;
    // Remove IValidator

    public CommentController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<CommentController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Comment
    public async Task<IActionResult> Index(string? searchTerm = null, bool? isApproved = null, int? articleId = null, int page = 1, int pageSize = 20) // Added pagination
    {
        ViewData["Title"] = "Quản lý Bình luận - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách Bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bình luận", Url.Action(nameof(Index))) };

        int pageNumber = page;
        var query = _context.Set<Comment>()
                            .Include(c => c.Article) // For ArticleTitle
                            .Include(c => c.Replies) // For ReplyCount
                            .Where(c => c.ParentId == null) // Only show top-level comments
                            .AsNoTracking(); // Use AsNoTracking

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(c => c.AuthorName.ToLower().Contains(lowerSearchTerm)
                                  || (c.AuthorEmail != null && c.AuthorEmail.ToLower().Contains(lowerSearchTerm))
                                  || c.Content.ToLower().Contains(lowerSearchTerm));
        }
        if (isApproved.HasValue)
        {
            query = query.Where(c => c.IsApproved == isApproved.Value);
        }
        if (articleId.HasValue && articleId > 0)
        {
            query = query.Where(c => c.ArticleId == articleId.Value);
        }

        // Sorting & Pagination
        var commentsPaged = await query
            .OrderByDescending(c => c.IsApproved == false) // Unapproved first
            .ThenByDescending(c => c.CreatedAt) // Then by date
            .ProjectTo<CommentListItemViewModel>(_mapper.ConfigurationProvider) // Project efficiently
            .ToPagedListAsync(pageNumber, pageSize); // Paginate

        // Load filter dropdowns
        await LoadArticleFilterDropdownAsync(articleId); // Load articles for filter
        ViewBag.ApprovalStatusList = new List<SelectListItem> {
             new SelectListItem { Value = "true", Text = "Đã duyệt"},
             new SelectListItem { Value = "false", Text = "Chưa duyệt"}
        };

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedApprovedStatus = isApproved;
        // SelectedArticleId loaded by helper

        return View(commentsPaged); // Pass paged list
    }

    // GET: Admin/Comment/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var comment = await _context.Set<Comment>()
                                 .Include(c => c.Article) // Include article for title display
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null) { _logger.LogWarning("Edit GET: Comment ID {CommentId} not found.", id); return NotFound(); }

        var viewModel = _mapper.Map<CommentViewModel>(comment);

        ViewData["Title"] = "Chỉnh sửa Bình luận - Hệ thống quản trị";
        ViewData["PageTitle"] = "Chỉnh sửa Bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bình luận", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };

        return View(viewModel);
    }

    // POST: Admin/Comment/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CommentViewModel viewModel)
    {
        if (id != viewModel.Id) { _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id); return BadRequest(); }

        // Rely on middleware for validation
        if (ModelState.IsValid)
        {
            var comment = await _context.Set<Comment>().FindAsync(id);
            if (comment == null) { _logger.LogWarning("Edit POST: Comment ID {CommentId} not found for update.", id); TempData["error"] = "Không tìm thấy bình luận."; return RedirectToAction(nameof(Index)); }

            // Map editable fields: AuthorName, AuthorEmail, AuthorWebsite, Content, IsApproved
            _mapper.Map(viewModel, comment);
            // Set audit fields
            // comment.UpdatedBy = User.Identity?.Name;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Comment ID {CommentId} updated successfully by {User}.", id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = "Cập nhật bình luận thành công!";
                return RedirectToAction(nameof(Index)); // Redirect to list after successful edit
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Comment ID {CommentId}", id);
                ModelState.AddModelError("", "Không thể lưu thay đổi.");
            }
        }
        else
        {
            _logger.LogWarning("Comment editing failed for ID: {CommentId}. Model state is invalid.", id);
        }

        // If failed, redisplay form
        // Reload ArticleTitle if needed as it's not posted back
        var originalArticle = await _context.Articles.AsNoTracking().Select(a => new { a.Id, a.Title }).FirstOrDefaultAsync(a => a.Id == viewModel.ArticleId);
        viewModel.ArticleTitle = originalArticle?.Title;

        ViewData["Title"] = "Chỉnh sửa Bình luận - Hệ thống quản trị";
        ViewData["PageTitle"] = "Chỉnh sửa Bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bình luận", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };
        return View(viewModel);
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
                                // Include replies only if needed due to RESTRICT constraint or custom logic
                                // .Include(c => c.Replies)
                                .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null) { _logger.LogWarning("Delete POST: Comment ID {CommentId} not found.", id); return Json(new { success = false, message = "Không tìm thấy bình luận." }); }

        try
        {
            string author = comment.AuthorName;
            // If ParentId has RESTRICT and replies exist, this will fail. Handle by changing constraint to Cascade/SetNull or deleting replies first.
            _context.Remove(comment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Comment ID {CommentId} by '{Author}' deleted successfully by {User}.", id, author, User.Identity?.Name ?? "Unknown");
            return Json(new { success = true, message = $"Xóa bình luận của '{author}' thành công." });
        }
        catch (DbUpdateException ex)
        { // Catch FK errors
            _logger.LogError(ex, "Error deleting Comment ID {CommentId}. Potential constraint violation (e.g., replies exist with RESTRICT).", id);
            return Json(new { success = false, message = "Không thể xóa bình luận. Có thể do bình luận này có trả lời." });
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
        if (comment == null) { _logger.LogWarning("UpdateApprovalStatus: Comment ID {CommentId} not found.", id); return Json(new { success = false, message = "Không tìm thấy bình luận." }); }

        if (comment.IsApproved == approve)
        {
            return Json(new { success = true, message = approve ? "Bình luận đã được duyệt." : "Bình luận đã ở trạng thái chờ duyệt." }); // No change needed
        }

        comment.IsApproved = approve;
        // comment.UpdatedBy = User.Identity?.Name; // Set audit field

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Comment ID {CommentId} approval status set to {Status} by {User}.", id, approve, User.Identity?.Name ?? "Unknown");
            return Json(new { success = true, message = approve ? "Duyệt bình luận thành công." : "Bỏ duyệt bình luận thành công.", isApproved = approve }); // Return new status
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating approval status for Comment ID {CommentId} to {Status}.", id, approve);
            return Json(new { success = false, message = "Lỗi cập nhật trạng thái duyệt." });
        }
    }

    // Helper to load Article filter dropdown
    private async Task LoadArticleFilterDropdownAsync(int? selectedArticleId)
    {
        // Load only articles that actually have comments? Maybe too slow. Load recent/popular articles?
        // For simplicity, load all published articles for now. Optimize later if needed.
        ViewBag.ArticleList = await _context.Articles
                                      .Where(a => a.Status == PublishStatus.Published)
                                      .OrderBy(a => a.Title)
                                      .Select(a => new SelectListItem
                                      {
                                          Value = a.Id.ToString(),
                                          Text = a.Title.Length > 50 ? a.Title.Substring(0, 50) + "..." : a.Title, // Truncate long titles
                                          Selected = a.Id == selectedArticleId
                                      })
                                      .ToListAsync();
        ViewBag.SelectedArticleId = selectedArticleId;
    }
}
// --- END OF FILE CommentController.cs ---