using web.Areas.Client.Models.Category;
using web.Areas.Client.Requests.Subscriber;

namespace web.Areas.Client.Models.Home
{
    public class HomeViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
        public SubscriberCreateRequest Subscriber { get; set; } = new SubscriberCreateRequest();
    }
}
