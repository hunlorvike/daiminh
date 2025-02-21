using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.Controllers.Shared;

public abstract class DaiminhController(IServiceProvider serviceProvider, IConfiguration configuration)
    : Controller
{
    protected readonly IConfiguration Configuration = configuration;

    protected IValidator<T> GetValidator<T>() where T : class
    {
        return serviceProvider.GetRequiredService<IValidator<T>>();
    }
}