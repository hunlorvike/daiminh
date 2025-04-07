using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Testimonial;

namespace web.Areas.Admin.Mappers;

public class TestimonialMappingProfile : Profile
{
    public TestimonialMappingProfile()
    {
        // Testimonial Mappings
        CreateMap<Testimonial, TestimonialListItemViewModel>();

        // Entity -> ViewModel (For Edit GET)
        CreateMap<Testimonial, TestimonialViewModel>();

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<TestimonialViewModel, Testimonial>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id and Nav properties
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}