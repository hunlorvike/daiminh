using AutoMapper;
using core.Entities;
using web.Areas.Admin.Models.Contact;
using web.Areas.Admin.Requests.Contact;

namespace web.Areas.Admin.Profiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<Contact, ContactViewModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<Contact, ContactUpdateRequest>().ForMember(cr => cr.ContactStatus, opt => opt.MapFrom(usr => usr.Status)).ReverseMap();
    }
}