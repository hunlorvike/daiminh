using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Contact;

namespace web.Areas.Admin.Mappers;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Contact, ContactListItemViewModel>();

        // Entity -> DetailViewModel
        CreateMap<Contact, ContactDetailViewModel>()
            .ForMember(dest => dest.StatusList, opt => opt.Ignore()); // Ignore dropdown

        // DetailViewModel -> Entity (Update Status and Notes only)
        CreateMap<ContactDetailViewModel, Contact>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.AdminNotes, opt => opt.MapFrom(src => src.AdminNotes))
            // Ignore all other properties that shouldn't be changed from this VM
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FullName, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.Phone, opt => opt.Ignore())
            .ForMember(dest => dest.Subject, opt => opt.Ignore())
            .ForMember(dest => dest.Message, opt => opt.Ignore())
            .ForMember(dest => dest.CompanyName, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectDetails, opt => opt.Ignore())
            .ForMember(dest => dest.IpAddress, opt => opt.Ignore())
            .ForMember(dest => dest.UserAgent, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}