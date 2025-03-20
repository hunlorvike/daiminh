using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Requests.Subscriber;

namespace web.Areas.Admin.Profiles;

public class SubscriberProfile : Profile
{
    public SubscriberProfile()
    {
        CreateMap<Subscriber, SubscriberDeleteRequest>().ReverseMap();
    }
}