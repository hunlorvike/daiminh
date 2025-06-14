using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class ProfileProfile : Profile
{
    public ProfileProfile()
    {
        CreateMap<User, ProfileViewModel>();
    }
}