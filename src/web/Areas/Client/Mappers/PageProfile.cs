using AutoMapper;
using domain.Entities;
using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Mappers;

public class PageProfile : Profile
{
    public PageProfile()
    {
        CreateMap<Page, PageDetailViewModel>();
    }
}
