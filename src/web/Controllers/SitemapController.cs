//using domain.Entities;
//using infrastructure;
//using System.Xml.Linq;
//using Microsoft.AspNetCore.Mvc;
//using web.Areas.Admin.Services;
//using System.Text.Json;

//namespace web.Controllers;

//[Route("sitemap.xml")]
//public class SitemapController : Controller
//{
//    private readonly ISeoService _seoService;
//    private readonly ApplicationDbContext _context;

//    public SitemapController(ISeoService seoService, ApplicationDbContext context)
//    {
//        _seoService = seoService;
//        _context = context;
//    }

//    [HttpGet]
//    public async Task<IActionResult> Index()
//    {
//        var sitemapSettings = await _seoService.GetSeoSettingValueAsync(SeoSettings.SitemapSettings);
//        var settings = !string.IsNullOrEmpty(sitemapSettings)
//            ? JsonSerializer.Deserialize<SitemapSettingsModel>(sitemapSettings)
//            : new SitemapSettingsModel();

//        var sitemap = new XDocument(
//            new XDeclaration("1.0", "utf-8", null),
//            new XElement(XName.Get("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9"))
//        );

//        // Thêm trang chủ
//        sitemap.Root.Add(
//            new XElement("url",
//                new XElement("loc", Url.Action("Index", "Home", null, Request.Scheme)),
//                new XElement("lastmod", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sszzz")),
//                new XElement("changefreq", "daily"),
//                new XElement("priority", "1.0")
//            )
//        );

//        // Thêm các trang sản phẩm
//        if (settings.Include.Contains("products"))
//        {
//            var products = await _context.Products.Where(p => p.IsActive).ToListAsync();
//            foreach (var product in products)
//            {
//                sitemap.Root.Add(
//                    new XElement("url",
//                        new XElement("loc", Url.Action("Details", "Products", new { id = product.Id, slug = product.Slug }, Request.Scheme)),
//                        new XElement("lastmod", product.UpdatedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz") ?? product.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz")),
//                        new XElement("changefreq", settings.Changefreq.TryGetValue("products", out var freq) ? freq : "weekly"),
//                        new XElement("priority", settings.Priority.TryGetValue("products", out var priority) ? priority.ToString() : "0.8")
//                    )
//                );
//            }
//        }

//        // Thêm các trang danh mục
//        if (settings.Include.Contains("categories"))
//        {
//            var categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();
//            foreach (var category in categories)
//            {
//                sitemap.Root.Add(
//                    new XElement("url",
//                        new XElement("loc", Url.Action("Index", "Categories", new { id = category.Id, slug = category.Slug }, Request.Scheme)),
//                        new XElement("lastmod", category.UpdatedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz") ?? category.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz")),
//                        new XElement("changefreq", settings.Changefreq.TryGetValue("categories", out var freq) ? freq : "monthly"),
//                        new XElement("priority", settings.Priority.TryGetValue("categories", out var priority) ? priority.ToString() : "0.7")
//                    )
//                );
//            }
//        }

//        // Thêm các trang bài viết
//        if (settings.Include.Contains("articles"))
//        {
//            var articles = await _context.Articles.Where(a => a.IsActive).ToListAsync();
//            foreach (var article in articles)
//            {
//                sitemap.Root.Add(
//                    new XElement("url",
//                        new XElement("loc", Url.Action("Details", "Articles", new { id = article.Id, slug = article.Slug }, Request.Scheme)),
//                        new XElement("lastmod", article.UpdatedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz") ?? article.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz")),
//                        new XElement("changefreq", settings.Changefreq.TryGetValue("articles", out var freq) ? freq : "daily"),
//                        new XElement("priority", settings.Priority.TryGetValue("articles", out var priority) ? priority.ToString() : "0.6")
//                    )
//                );
//            }
//        }

//        return Content(sitemap.ToString(), "application/xml");
//    }
//}