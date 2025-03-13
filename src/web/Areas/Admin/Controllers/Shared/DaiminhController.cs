using FluentValidation;

namespace web.Areas.Admin.Controllers.Shared;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public abstract class DaiminhController(IMapper mapper, IServiceProvider serviceProvider, IConfiguration configuration) : Controller
{
    protected readonly IMapper _mapper = mapper;
    protected readonly IServiceProvider _serviceProvider = serviceProvider;
    protected readonly IConfiguration _configuration = configuration;

    protected IValidator<T> GetValidator<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<IValidator<T>>();
    }
}