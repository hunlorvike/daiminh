using AutoMapper;
using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Admin.Controllers;

public partial class ContentFieldDefinitionController(
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class ContentFieldDefinitionController
{
    
}