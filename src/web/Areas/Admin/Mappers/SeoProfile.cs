using AutoMapper;
using domain.Entities.Shared;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Mappers;

public class SeoProfile : Profile
{
    public SeoProfile()
    {
        CreateMap<SeoEntity<int>, SeoViewModel>().ReverseMap();
    }
}
