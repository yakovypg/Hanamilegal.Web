using System;
using Hanamilegal.Web.ApiConfiguration.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class ConfigurationExtensions
{
    public static string GetRequiredConnectionString(this IConfiguration configuration, string name)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        return configuration.GetConnectionString(name)
            ?? throw new ConfigurationException($"Connection string to {name} not specified");
    }

    public static T GetRequiredObject<T>(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        return configuration.GetSection(key).Get<T>()
            ?? throw new ConfigurationException($"{key} not specified");
    }

    public static IOptions<T> GetRequiredOptions<T>(this IConfiguration configuration, string key)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        T optionsSource = configuration.GetRequiredObject<T>(key);

        return Microsoft.Extensions.Options.Options.Create(optionsSource);
    }
}
