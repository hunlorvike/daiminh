using web.Areas.Client.Models.Category;
using web.Areas.Client.Models.Content;
using web.Areas.Client.Models.ContentType;
using web.Areas.Client.Requests.Subscriber;

namespace web.Areas.Client.Models.Home
{
    public class HomeViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
        public List<ContentViewModel> Contents { get; set; }
        public List<ContentTypeViewModel> ContentTypes { get; set; }
        public ContentViewModel LatestContent { get; set; }
        public SubscriberCreateRequest Subscriber { get; set; } = new SubscriberCreateRequest();
    }
}
