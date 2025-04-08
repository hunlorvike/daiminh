using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Newsletter;

namespace web.Areas.Admin.Mappers;

public class NewsletterProfile : Profile
{
    public NewsletterProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Newsletter, NewsletterListItemViewModel>();
    }
}