using System;
using Microsoft.Extensions.Configuration;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddAppSettingsJson(
        this IConfigurationBuilder configurationBuilder,
        string basePath,
        string environmentName)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder, nameof(configurationBuilder));
        ArgumentException.ThrowIfNullOrWhiteSpace(basePath, nameof(basePath));
        ArgumentException.ThrowIfNullOrWhiteSpace(environmentName, nameof(environmentName));

        const string coreSettingsFileName = "appsettings.json";
        string additionalSettingsFileName = $"appsettings.{environmentName}.json";

        return configurationBuilder
            .SetBasePath(basePath)
            .AddJsonFile(coreSettingsFileName, optional: true, reloadOnChange: true)
            .AddJsonFile(additionalSettingsFileName, optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}
