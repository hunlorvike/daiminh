using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class SettingProfile : Profile
{
    public SettingProfile()
    {
        // Entity -> ViewModel (For Display/Edit GET)
        CreateMap<Setting, SettingViewModel>();
    }
}