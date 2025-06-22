using AutoMapper;
using application.constracts.Dtos.Brand;
using domain.Entities;

namespace application.ObjectMapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
        * Alternatively, you can split your mapping configurations
        * into multiple profile classes for a better organization. */

        CreateMap<Brand, BrandDto>();
    }
}
