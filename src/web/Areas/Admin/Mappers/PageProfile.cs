using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Page;

namespace web.Areas.Admin.Mappers;

public class PageProfile : Profile
{
    public PageProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Page, PageListItemViewModel>();

        // Entity -> ViewModel (GET Edit)
        CreateMap<Page, PageViewModel>();

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<PageViewModel, Page>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}