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

        // Entity -> ViewModel (For Details GET)
        CreateMap<Contact, ContactViewModel>()
            .ForMember(dest => dest.StatusOptions, opt => opt.Ignore()); // Ignore SelectList

        // ViewModel -> Entity
        // Chỉ map các trường Admin có thể sửa
        CreateMap<ContactViewModel, Contact>()
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
             .ForMember(dest => dest.AdminNotes, opt => opt.MapFrom(src => src.AdminNotes))
             // Ignore tất cả các trường khác để tránh ghi đè dữ liệu gốc
             .ForMember(dest => dest.FullName, opt => opt.Ignore())
             .ForMember(dest => dest.Email, opt => opt.Ignore())
             .ForMember(dest => dest.Phone, opt => opt.Ignore())
             .ForMember(dest => dest.Subject, opt => opt.Ignore())
             .ForMember(dest => dest.Message, opt => opt.Ignore())
             .ForMember(dest => dest.IpAddress, opt => opt.Ignore())
             .ForMember(dest => dest.UserAgent, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
