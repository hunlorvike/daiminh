using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Setting;

namespace web.Areas.Admin.Mappers;

public class SettingMappingProfile : Profile
{
    public SettingMappingProfile()
    {
        // Setting Mappings (Generic)
        CreateMap<Setting, SettingViewModel>();

        // Map from ViewModel back to Entity (ONLY updateable fields)
        CreateMap<SettingViewModel, Setting>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            // Ignore all other properties during the update mapping
            .ForMember(dest => dest.Key, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.Ignore())
            .ForMember(dest => dest.DefaultValue, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
