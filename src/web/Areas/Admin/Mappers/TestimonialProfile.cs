using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Testimonial;

namespace web.Areas.Admin.Mappers;

public class TestimonialMappingProfile : Profile
{
    public TestimonialMappingProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Testimonial, TestimonialListItemViewModel>();

        // Entity -> ViewModel (For Edit GET)
        CreateMap<Testimonial, TestimonialViewModel>();

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<TestimonialViewModel, Testimonial>();
    }
}
