namespace web.Areas.Admin.ViewModels.Comment;

public class CommentFilterViewModel
{
    public string? SearchTerm { get; set; }
    public bool? IsApproved { get; set; }
    public int? ArticleId { get; set; }
    public bool? HasParent { get; set; }
}