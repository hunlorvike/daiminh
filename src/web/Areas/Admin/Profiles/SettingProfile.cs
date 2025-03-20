using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Requests.Setting;


namespace web.Areas.Admin.Profiles;

public class SettingProfile : Profile
{
    public SettingProfile()
    {
        CreateMap<SettingCreateRequest, Setting>();
        CreateMap<Setting, SettingUpdateRequest>().ReverseMap();
        CreateMap<Setting, SettingDeleteRequest>().ReverseMap();
    }
}