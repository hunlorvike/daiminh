using AutoMapper;
using core.Entities;
using web.Areas.Admin.Models.Subscriber;
using web.Areas.Admin.Requests.Subscriber;

namespace web.Areas.Admin.Profiles;

public class SubscriberProfile : Profile
{
    public SubscriberProfile()
    {
        CreateMap<Subscriber, SubscriberViewModel>();
        CreateMap<Subscriber, SubscriberDeleteRequest>().ReverseMap();
    }
}

