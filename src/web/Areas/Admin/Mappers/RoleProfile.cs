using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        // Entity -> ListItemViewModel (cho trang Index)
        CreateMap<Role, RoleListItemViewModel>();

        // Entity -> ViewModel (cho form Edit/GET)
        CreateMap<Role, RoleViewModel>()
            .ForMember(dest => dest.SelectedClaimDefinitionIds, opt => opt.Ignore())
            .ForMember(dest => dest.AvailableClaimDefinitions, opt => opt.Ignore());

        // ViewModel -> Entity (cho form Create/Edit/POST)
        CreateMap<RoleViewModel, Role>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.Name.ToUpperInvariant()));
    }
}