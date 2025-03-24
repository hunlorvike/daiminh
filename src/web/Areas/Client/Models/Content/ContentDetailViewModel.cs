namespace web.Areas.Client.Models.Content;

public class ContentDetailViewModel
{
    public domain.Entities.Content Content { get; set; } = null!;
    public List<domain.Entities.Content> RelatedContents { get; set; } = new();
}