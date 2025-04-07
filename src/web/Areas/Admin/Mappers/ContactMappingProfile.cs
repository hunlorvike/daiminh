using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Contact;

namespace web.Areas.Admin.Mappers;

public class ContactMappingProfile : Profile
{
    public ContactMappingProfile()
    {
        // Contact Mappings
        CreateMap<Contact, ContactListItemViewModel>();
        CreateMap<Contact, ContactViewModel>().ReverseMap();
    }
}