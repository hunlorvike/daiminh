using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class ArticleService : IArticleService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ArticleService> _logger;

    public ArticleService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ArticleService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<ArticleListItemViewModel>> GetPagedArticlesAsync(ArticleFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<Article> query = _context.Set<Article>()
                                         .Include(a => a.Category)
                                         .Include(a => a.ArticleTags)
                                         .AsNoTracking();


        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(a => a.Title.ToLower().Contains(lowerSearchTerm) ||
                                     a.Summary != null && a.Summary.ToLower().Contains(lowerSearchTerm) ||
                                     a.Content != null && a.Content.ToLower().Contains(lowerSearchTerm) ||
                                     a.AuthorName != null && a.AuthorName.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.CategoryId.HasValue && filter.CategoryId.Value > 0)
        {
            query = query.Where(a => a.CategoryId == filter.CategoryId.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(a => a.Status == filter.Status.Value);
        }

        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(a => a.IsFeatured == filter.IsFeatured.Value);
        }

        query = query.OrderByDescending(a => a.PublishedAt)
                     .ThenByDescending(a => a.CreatedAt);

        IPagedList<ArticleListItemViewModel> articlesPaged = await query.ProjectTo<ArticleListItemViewModel>(_mapper.ConfigurationProvider)
                                   .ToPagedListAsync(pageNumber, pageSize);

        return articlesPaged;
    }

    public async Task<ArticleViewModel?> GetArticleByIdAsync(int id)
    {
        Article? article = await _context.Set<Article>()
                                     .Include(a => a.Category)
                                     .Include(a => a.ArticleTags)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null) return null;

        return _mapper.Map<ArticleViewModel>(article);
    }

    public async Task<OperationResult<int>> CreateArticleAsync(ArticleViewModel viewModel, string? authorId, string? authorName)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!))
        {
            return OperationResult<int>.FailureResult(message: "Slug bài viết này đã tồn tại.", errors: new List<string> { "Slug bài viết này đã tồn tại." });
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var article = _mapper.Map<Article>(viewModel);

            article.AuthorId = authorId;
            article.AuthorName = authorName ?? "Admin";

            if (article.Status == PublishStatus.Published && article.PublishedAt == null)
            {
                article.PublishedAt = DateTime.Now;
            }

            UpdateArticleRelationships(article, viewModel.SelectedTagIds);

            _context.Add(article);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Created Article: ID={Id}, Title={Title}, Slug={Slug}", article.Id, article.Title, article.Slug);
            return OperationResult<int>.SuccessResult(article.Id, $"Thêm bài viết '{article.Title}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Lỗi DB khi tạo bài viết: {Title}", viewModel.Title);
            if (ex.InnerException?.Message?.Contains("UQ_Article_Slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult<int>.FailureResult(message: "Slug bài viết này đã tồn tại.", errors: new List<string> { "Slug bài viết này đã tồn tại." });
            }
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi lưu bài viết.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi lưu bài viết." });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Lỗi không xác định khi tạo bài viết.");
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi lưu bài viết.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi lưu bài viết." });
        }
    }

    public async Task<OperationResult> UpdateArticleAsync(ArticleViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!, viewModel.Id))
        {
            return OperationResult.FailureResult(message: "Slug bài viết này đã tồn tại.", errors: new List<string> { "Slug bài viết này đã tồn tại." });
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var article = await _context.Set<Article>()
                .Include(a => a.ArticleTags)
                .FirstOrDefaultAsync(a => a.Id == viewModel.Id);

            if (article == null)
            {
                _logger.LogWarning("Article not found for update. ID: {Id}", viewModel.Id);
                return OperationResult.FailureResult("Không tìm thấy bài viết để cập nhật.");
            }

            var oldStatus = article.Status;

            _mapper.Map(viewModel, article);

            if (oldStatus != PublishStatus.Published && article.Status == PublishStatus.Published && article.PublishedAt == null)
            {
                article.PublishedAt = DateTime.Now;
            }
            UpdateArticleRelationships(article, viewModel.SelectedTagIds);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Updated Article: ID={Id}, Title={Title}, Slug={Slug}", article.Id, article.Title, article.Slug);
            return OperationResult.SuccessResult($"Cập nhật bài viết '{article.Title}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Lỗi DB khi cập nhật bài viết ID: {Id}", viewModel.Id);
            if (ex.InnerException?.Message?.Contains("UQ_Article_Slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult(message: "Slug bài viết này đã tồn tại.", errors: new List<string> { "Slug bài viết này đã tồn tại." });
            }
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi cập nhật bài viết.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi cập nhật bài viết." });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Lỗi hệ thống khi cập nhật bài viết ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi không mong muốn.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn." });
        }
    }


    public async Task<OperationResult> DeleteArticleAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        string articleTitle = string.Empty;
        try
        {
            var article = await _context.Set<Article>()
                .Include(a => a.ArticleTags)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                _logger.LogWarning("Article not found for delete. ID: {Id}", id);
                return OperationResult.FailureResult("Không tìm thấy bài viết.");
            }

            articleTitle = article.Title;

            if (article.ArticleTags != null && article.ArticleTags.Any())
            {
                _context.RemoveRange(article.ArticleTags);
            }

            _context.Remove(article);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Deleted Article: ID={Id}, Title={Title}", id, articleTitle);
            return OperationResult.SuccessResult($"Xóa bài viết '{articleTitle}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Lỗi DB khi xóa bài viết ID {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult($"Không thể xóa bài viết '{articleTitle}' vì đang được sử dụng.", errors: new List<string> { $"Không thể xóa bài viết '{articleTitle}' vì đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa bài viết.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa bài viết." });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Lỗi không xác định khi xóa bài viết ID {Id}", id);
            return OperationResult.FailureResult($"Không thể xóa bài viết.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa bài viết." });
        }
    }

    public async Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(slug)) return false;

        var lowerSlug = slug.Trim().ToLower();
        var query = _context.Set<Article>()
                            .Where(a => a.Slug.ToLower() == lowerSlug);

        if (ignoreId.HasValue && ignoreId.Value > 0)
        {
            query = query.Where(a => a.Id != ignoreId.Value);
        }

        return await query.AnyAsync();
    }
    private void UpdateArticleRelationships(Article article, List<int>? selectedTagIds)
    {
        var existingTagIds = article.ArticleTags?.Select(at => at.TagId).ToList() ?? new List<int>();
        var tagIdsToAdd = selectedTagIds?.Except(existingTagIds).ToList() ?? new List<int>();
        var tagIdsToRemove = existingTagIds.Except(selectedTagIds ?? new List<int>()).ToList();

        foreach (var tagId in tagIdsToRemove)
        {
            var articleTag = article.ArticleTags!.First(at => at.TagId == tagId);
            _context.Remove(articleTag);
        }

        foreach (var tagId in tagIdsToAdd)
        {
            article.ArticleTags ??= new List<ArticleTag>();
            article.ArticleTags.Add(new ArticleTag { ArticleId = article.Id, TagId = tagId });
        }
    }

    public async Task<List<SelectListItem>> GetArticleSelectListAsync(List<int>? selectedValues = null)
    {
        var articles = await _context.Set<Article>()
            .Where(a => a.Status == PublishStatus.Published)
            .OrderBy(a => a.Id)
            .AsNoTracking()
            .Select(a => new { a.Id, a.Title })
            .ToListAsync();

        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Chọn bài viết --", Selected = selectedValues == null || !selectedValues.Any() }
        };

        items.AddRange(articles.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Title,
            Selected = selectedValues != null && selectedValues.Contains(t.Id)
        }));

        return items;
    }
}