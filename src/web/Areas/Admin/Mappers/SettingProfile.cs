using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Setting;

namespace web.Areas.Admin.Mappers;

public class SettingProfile : Profile
{
    public SettingProfile()
    {
        // Entity -> ViewModel
        // AutoMapper should map the FieldType enum correctly by name/value
        CreateMap<Setting, SettingViewModel>();

        // ViewModel -> Entity (Only updatable fields: Value, IsActive)
        CreateMap<SettingViewModel, Setting>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            // Ignore all non-updatable properties
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Key, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.Ignore()) // Don't update Type
            .ForMember(dest => dest.Description, opt => opt.Ignore())
            .ForMember(dest => dest.DefaultValue, opt => opt.Ignore());
    }
}
