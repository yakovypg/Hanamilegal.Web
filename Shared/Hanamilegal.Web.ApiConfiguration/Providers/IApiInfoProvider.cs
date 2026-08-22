using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace Hanamilegal.Web.ApiConfiguration.Providers;

public interface IApiInfoProvider
{
    OpenApiInfo CreateApiInfo(ApiVersionDescription versionDescription);
}
