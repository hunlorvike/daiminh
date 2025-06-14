using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Client.Services.Interfaces;
using web.Areas.Client.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Client.Services;

[Register(ServiceLifetime.Scoped)]
public class ArticleClientService : IArticleClientService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ArticleClientService> _logger;

    public ArticleClientService(ApplicationDbContext context, IMapper mapper, ILogger<ArticleClientService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<ArticleCardViewModel>> GetArticlesAsync(int pageNumber, int pageSize)
    {
        var query = _context.Articles
            .AsNoTracking()
            .Where(a => a.Status == PublishStatus.Published && a.PublishedAt <= DateTime.Now)
            .OrderByDescending(a => a.PublishedAt);

        return await query
            .ProjectTo<ArticleCardViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);
    }

    public async Task<ArticleDetailViewModel?> GetArticleBySlugAsync(string slug)
    {
        var article = await _context.Articles
            .AsNoTracking()
            .Include(a => a.Category)
            .Include(a => a.ArticleTags)
                .ThenInclude(at => at.Tag)
            .FirstOrDefaultAsync(a => a.Slug == slug && a.Status == PublishStatus.Published && a.PublishedAt <= DateTime.Now);

        if (article == null)
        {
            _logger.LogWarning("Không tìm thấy bài viết với slug: {Slug} hoặc bài viết chưa được xuất bản.", slug);
            return null;
        }

        // Tăng lượt xem
        article.ViewCount++;
        _context.Articles.Update(article);
        _context.Entry(article).Property(x => x.ViewCount).IsModified = true;
        await _context.SaveChangesAsync();

        var viewModel = _mapper.Map<ArticleDetailViewModel>(article);

        // Lấy các bài viết liên quan (cùng danh mục, trừ bài hiện tại)
        if (article.CategoryId.HasValue)
        {
            viewModel.RelatedArticles = await _context.Articles
                .AsNoTracking()
                .Where(a => a.CategoryId == article.CategoryId && a.Id != article.Id && a.Status == PublishStatus.Published)
                .OrderByDescending(a => a.PublishedAt)
                .Take(3)
                .ProjectTo<ArticleCardViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        return viewModel;
    }
}