using web.Areas.Client.Models.Category;
using web.Areas.Client.Models.Content;
using web.Areas.Client.Models.ContentType;
using web.Areas.Client.Requests.Subscriber;

namespace web.Areas.Client.Models.Home;

public class HomeViewModel
{
    public required List<CategoryViewModel> Categories { get; set; }
    public required List<ContentViewModel> Contents { get; set; }
    public required List<ContentTypeViewModel> ContentTypes { get; set; }
    public required ContentViewModel LatestContent { get; set; }
    public SubscriberCreateRequest Subscriber { get; set; } = new SubscriberCreateRequest();
}
