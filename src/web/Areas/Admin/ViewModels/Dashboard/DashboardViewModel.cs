using shared.Enums;

namespace web.Areas.Admin.ViewModels.Dashboard;

public class DashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalArticles { get; set; }
    public int TotalProjects { get; set; }
    public int TotalContacts { get; set; }
    public int NewContacts { get; set; }

    public List<RecentArticleViewModel> RecentArticles { get; set; } = new List<RecentArticleViewModel>();
    public List<RecentContactViewModel> RecentContacts { get; set; } = new List<RecentContactViewModel>();
}

public class RecentArticleViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? AuthorName { get; set; }
    public DateTime CreatedAt { get; set; }
    public PublishStatus Status { get; set; }
}

public class RecentContactViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public ContactStatus Status { get; set; }
}