using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Requests.Slider;

namespace web.Areas.Admin.Profiles;

public class SliderProfile : Profile
{
    public SliderProfile()
    {
        CreateMap<SliderCreateRequest, Slider>();
        CreateMap<Slider, SliderUpdateRequest>().ReverseMap();
        CreateMap<Slider, SliderDeleteRequest>().ReverseMap();
    }
}