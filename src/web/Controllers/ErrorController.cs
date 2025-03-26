using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using shared.Models;

namespace web.Controllers;

public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;
    private readonly IWebHostEnvironment _env;

    public ErrorController(
        ILogger<ErrorController> logger,
        IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    [Route("Error")]
    public IActionResult Index()
    {
        var errorViewModel = _GetErrorViewModel();
        return View(errorViewModel);
    }

    [Route("Error/NotFound")]
    public new IActionResult NotFound()
    {
        var errorViewModel = _GetErrorViewModel();
        return View(errorViewModel);
    }

    [Route("Error/Unauthorized")]
    public new IActionResult Unauthorized()
    {
        var errorViewModel = _GetErrorViewModel();
        return View(errorViewModel);
    }

    [Route("Error/Forbidden")]
    public IActionResult Forbidden()
    {
        var errorViewModel = _GetErrorViewModel();
        return View(errorViewModel);
    }

    private ErrorViewModel _GetErrorViewModel()
    {
        if (HttpContext.Items.TryGetValue("ErrorViewModel", out var errorModel) &&
            errorModel is ErrorViewModel model)
        {
            return model;
        }

        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        if (exception != null)
        {
            _logger.LogError(exception, "An error occurred");
        }

        return new ErrorViewModel
        {
            RequestId = HttpContext.TraceIdentifier,
            StatusCode = HttpContext.Response.StatusCode,
            Message = _env.IsDevelopment() && exception != null
                ? exception.Message
                : "An error occurred while processing your request.",
            Exception = _env.IsDevelopment() ? exception : null
        };
    }
}
