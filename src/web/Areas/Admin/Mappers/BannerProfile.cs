using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Banner;

namespace web.Areas.Admin.Mappers;

public class BannerProfile : Profile
{
    public BannerProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Banner, BannerListItemViewModel>();

        // Entity -> ViewModel (GET Edit)
        CreateMap<Banner, BannerViewModel>();

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<BannerViewModel, Banner>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}