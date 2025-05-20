using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleListItemViewModel>();
        CreateMap<Role, RoleViewModel>().ReverseMap();
    }
}
