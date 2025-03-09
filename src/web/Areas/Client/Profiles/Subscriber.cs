using AutoMapper;
using domain.Entities;
using web.Areas.Client.Requests.Subscriber;

namespace web.Areas.Client.Profiles;

public class SubscriberProfile : Profile
{
    public SubscriberProfile()
    {
        CreateMap<SubscriberCreateRequest, Subscriber>();
    }
}