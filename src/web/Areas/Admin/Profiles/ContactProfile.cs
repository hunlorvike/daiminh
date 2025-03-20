using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Requests.Contact;

namespace web.Areas.Admin.Profiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<Contact, ContactUpdateRequest>()
            .ForMember(dest => dest.ContactStatus, opt => opt.MapFrom(usr => usr.Status)).ReverseMap();
    }
}