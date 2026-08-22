using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi;

namespace Hanamilegal.Web.ApiConfiguration.Providers;

public interface IApiInfoProvider
{
    OpenApiInfo CreateApiInfo(ApiVersionDescription versionDescription);
}
