using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.InternalApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
[AllowAnonymous]
public class ErrorModel : PageModel
{
    private readonly ILogger<ErrorModel> _logger;

    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        IExceptionHandlerPathFeature? exceptionFeature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionFeature is not null)
        {
            _logger.LogError(
                exceptionFeature.Error,
                "An error occuren on {Path}",
                exceptionFeature.Path);
        }
    }
}
