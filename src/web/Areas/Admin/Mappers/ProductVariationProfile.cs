using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class ProductVariationProfile : Profile
{
    public ProductVariationProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<ProductVariation, ProductVariationListItemViewModel>()
            .ForMember(dest => dest.AttributeValueCombination, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                if (src.ProductVariationAttributeValues == null || !src.ProductVariationAttributeValues.Any())
                {
                    dest.AttributeValueCombination = "Không có thuộc tính";
                    return;
                }

                var groups = src.ProductVariationAttributeValues
                                .GroupBy(pvav => pvav.AttributeValue.Attribute?.Name ?? "Unknown Attribute")
                                .OrderBy(g => g.Key);

                dest.AttributeValueCombination = string.Join(", ", groups.Select(group =>
                {
                    var values = group.Select(pvav => pvav.AttributeValue.Value).OrderBy(value => value);
                    return $"{group.Key}: {string.Join(" / ", values)}";
                }));
            });


        // Entity -> ViewModel (GET Edit) 
        CreateMap<ProductVariation, ProductVariationViewModel>()
             .ForMember(dest => dest.AttributeValueOptions, opt => opt.Ignore())
             .ForMember(dest => dest.SelectedAttributeValueIds, opt => opt.MapFrom(src =>
                src.ProductVariationAttributeValues != null ?
                src.ProductVariationAttributeValues.Select(pvav => pvav.AttributeValueId).ToList() :
                new List<int>()
            ));

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<ProductVariationViewModel, ProductVariation>()
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.ProductVariationAttributeValues, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}