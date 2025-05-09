using AutoMapper;
using domain.Entities;
using web.Areas.Client.ViewModels.FAQ;

namespace web.Areas.Client.Mappers;

public class FAQProfile : Profile
{
    public FAQProfile()
    {
        CreateMap<FAQ, FAQItemViewModel>();

        CreateMap<Category, FAQCategoryViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Faqs, opt => opt.MapFrom(src =>
                src.FAQs != null ? src.FAQs.Where(f => f.IsActive).OrderBy(f => f.OrderIndex).ToList() : new List<FAQ>()));
    }
}