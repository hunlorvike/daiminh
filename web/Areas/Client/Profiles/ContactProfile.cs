using AutoMapper;
using core.Entities;
using web.Areas.Client.Requests.Contact;

namespace web.Areas.Client.Profiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<ContactCreateRequest, Contact>();
    }
}