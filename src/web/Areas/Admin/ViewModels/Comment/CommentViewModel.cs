using System.ComponentModel.DataAnnotations;
namespace web.Areas.Admin.ViewModels.Comment;
public class CommentViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tên tác giả")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(100)]
    public string AuthorName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "{0} không hợp lệ")]
    [MaxLength(100)]
    public string? AuthorEmail { get; set; }

    [Display(Name = "Website")]
    [MaxLength(255)]
    [Url(ErrorMessage = "{0} không hợp lệ")]
    public string? AuthorWebsite { get; set; }

    [Display(Name = "Nội dung bình luận")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Đã duyệt")]
    public bool IsApproved { get; set; }

    // Display Only Fields
    [Display(Name = "Ngày gửi")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Bài viết")]
    public string? ArticleTitle { get; set; }
    public int ArticleId { get; set; }
}