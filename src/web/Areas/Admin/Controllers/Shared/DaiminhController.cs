using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.Controllers.Shared;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

public abstract class DaiminhController : Controller
{
    protected readonly IMapper _mapper;
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IConfiguration _configuration;

    protected DaiminhController(IMapper mapper, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _mapper = mapper;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected IValidator<T> GetValidator<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<IValidator<T>>();
    }
}