using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class ClaimDefinitionProfile : Profile
{
    public ClaimDefinitionProfile()
    {
        // Ánh xạ từ Entity sang ListItemViewModel (cho trang Index)
        CreateMap<ClaimDefinition, ClaimDefinitionListItemViewModel>();

        // Ánh xạ từ Entity sang ViewModel (cho form Edit/GET)
        CreateMap<ClaimDefinition, ClaimDefinitionViewModel>();

        // Ánh xạ từ ViewModel sang Entity (cho form Create/Edit/POST)
        CreateMap<ClaimDefinitionViewModel, ClaimDefinition>();
    }
}
