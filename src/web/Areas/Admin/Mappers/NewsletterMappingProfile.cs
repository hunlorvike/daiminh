using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Newsletter;

namespace web.Areas.Admin.Mappers;

public class NewsletterMappingProfile : Profile
{
    public NewsletterMappingProfile()
    {
        // Newsletter Mappings
        CreateMap<Newsletter, NewsletterListItemViewModel>();
        CreateMap<Newsletter, NewsletterViewModel>().ReverseMap();
    }
}