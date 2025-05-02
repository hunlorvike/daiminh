using AutoMapper;
using domain.Entities;
using infrastructure;
using web.Areas.Admin.ViewModels.ProductVariation;

namespace web.Areas.Admin.Mappers;

public class ProductVariationProfile : Profile
{
    public ProductVariationProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<ProductVariation, ProductVariationListItemViewModel>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : "N/A"))
            .ForMember(dest => dest.AttributeCombination, opt => opt.MapFrom<ProductVariationAttributeCombinationResolver>());

        // Entity -> ViewModel (GET Edit/Create)
        CreateMap<ProductVariation, ProductVariationViewModel>()
             .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : "N/A"))
             .ForMember(dest => dest.SelectedAttributeValueIds, opt => opt.MapFrom(src => src.ProductVariationAttributeValues != null ? src.ProductVariationAttributeValues.Select(pvav => pvav.AttributeValueId).ToList() : new List<int>()))
             .ForMember(dest => dest.AttributeValueOptionsByAttribute, opt => opt.Ignore())
             .ForMember(dest => dest.ParentProductAttributes, opt => opt.Ignore());

        // ViewModel -> Entity (POST Create/Edit)
        CreateMap<ProductVariationViewModel, ProductVariation>()
             .ForMember(dest => dest.Product, opt => opt.Ignore())
             .ForMember(dest => dest.ProductVariationAttributeValues, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}

public class ProductVariationAttributeCombinationResolver : IValueResolver<ProductVariation, ProductVariationListItemViewModel, string>
{
    private readonly ApplicationDbContext _context;

    public ProductVariationAttributeCombinationResolver(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Resolve(ProductVariation source, ProductVariationListItemViewModel destination, string destMember, ResolutionContext context)
    {
        // Need to load related AttributeValues and their Attributes
        // This is inefficient in a list projection. Ideally, this should be pre-loaded or handled differently.
        // For demonstration, we'll load it here, but be aware of N+1 query issues if not careful with includes in the main query.
        // The main query in ProductVariationController's Index *does* include ProductVariationAttributeValues.
        // We need to include AttributeValue and Attribute from there too for this to work efficiently via ProjectTo.

        if (source.ProductVariationAttributeValues == null || !source.ProductVariationAttributeValues.Any())
        {
            return "Không có thuộc tính";
        }

        // Assumes ProductVariationAttributeValues are included and linked to AttributeValues
        // To get Attribute Names, AttributeValues need to be included with their Attribute navigations
        // Let's adjust the Controller query to include ProductVariationAttributeValues -> AttributeValue -> Attribute
        var attributeValues = source.ProductVariationAttributeValues
                                    .Where(pvav => pvav.AttributeValue != null) // Ensure AttributeValue is loaded
                                    .Select(pvav => pvav.AttributeValue!)
                                    .Where(av => av.Attribute != null) // Ensure Attribute is loaded
                                    .ToList();


        if (!attributeValues.Any())
        {
            return "Không có thuộc tính";
        }

        // Format: AttributeName1: Value1 / AttributeName2: Value2
        // Or simpler: Value1 / Value2 / Value3 ... (if attribute names are less important in list view)
        // Let's do Value1 / Value2
        return string.Join(" / ", attributeValues.Select(av => av.Value));
    }
}