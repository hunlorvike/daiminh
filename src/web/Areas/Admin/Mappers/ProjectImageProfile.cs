using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Project;
namespace web.Areas.Admin.Mappers;
public class ProjectImageProfile : Profile
{
    public ProjectImageProfile()
    {
        CreateMap<ProjectImage, ProjectImageViewModel>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Project, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectId, opt => opt.Ignore());
    }
}