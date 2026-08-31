using System;
using Hanamilegal.Web.ApiConfiguration.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class HeadersExtensions
{
    public static IServiceCollection SetupForwardedHeaders(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        return services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto;

            options.KnownIPNetworks.Clear();

            IOptions<DockerOptions> dockerOptions = configuration
                .GetRequiredOptions<DockerOptions>(DockerOptions.SectionName);

            string dockerNetworkString = dockerOptions.Value.Network.Subnet;

            if (System.Net.IPNetwork.TryParse(dockerNetworkString, out System.Net.IPNetwork network))
                options.KnownIPNetworks.Add(network);
        });
    }

    public static IApplicationBuilder SetupForwardedHeaders(this IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder, nameof(appBuilder));
        return appBuilder.UseForwardedHeaders();
    }
}
