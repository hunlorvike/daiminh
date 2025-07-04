using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class NewsletterProfile : Profile
{
    public NewsletterProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Newsletter, NewsletterListItemViewModel>();

        // Entity -> ViewModel (For Edit GET)
        CreateMap<Newsletter, NewsletterViewModel>();

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<NewsletterViewModel, Newsletter>()
            .ForMember(dest => dest.IpAddress, opt => opt.Ignore())
            .ForMember(dest => dest.UserAgent, opt => opt.Ignore())
            .ForMember(dest => dest.ConfirmedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UnsubscribedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}