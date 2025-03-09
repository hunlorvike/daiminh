using AutoMapper;
using domain.Entities;
using web.Areas.Client.Requests.Contact;

namespace web.Areas.Client.Profiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<ContactCreateRequest, Contact>();
    }
}