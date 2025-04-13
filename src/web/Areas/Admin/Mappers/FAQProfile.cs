using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.FAQ;

namespace web.Areas.Admin.Mappers;

public class FAQProfile : Profile
{
    public FAQProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<FAQ, FAQListItemViewModel>()
                  .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src =>
                      src.FAQCategories != null && src.FAQCategories.Any()
                      ? string.Join(", ", src.FAQCategories
                          .Where(fc => fc.Category != null)
                          .Select(fc => fc.Category!.Name)
                          .OrderBy(name => name))
                      : "N/A"
                  ));

        // Entity -> ViewModel (For Edit GET)
        CreateMap<FAQ, FAQViewModel>()
             .ForMember(dest => dest.SelectedCategoryIds, opt => opt.MapFrom(src =>
                 src.FAQCategories != null ? src.FAQCategories.Select(fc => fc.CategoryId).ToList() : new List<int>()
             ))
             .ForMember(dest => dest.CategoryList, opt => opt.Ignore()); // Ignore dropdown list

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<FAQViewModel, FAQ>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FAQCategories, opt => opt.Ignore()) // Handled manually in controller
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}
