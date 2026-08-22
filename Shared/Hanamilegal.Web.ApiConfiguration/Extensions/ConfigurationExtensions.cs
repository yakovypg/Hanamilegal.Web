using System;
using Hanamilegal.Web.ApiConfiguration.Exceptions;
using Microsoft.Extensions.Configuration;

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

    public static string GetString(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        return configuration[key]
            ?? throw new ConfigurationException($"{key} not specified");
    }

    public static int GetInt32(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        string valueString = configuration.GetString(key);

        return int.TryParse(valueString, out int value)
            ? value
            : throw new ConfigurationException($"{key} not recognized");
    }

    public static double GetDouble(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        string valueString = configuration.GetString(key);

        return double.TryParse(valueString, out double value)
            ? value
            : throw new ConfigurationException($"{key} not recognized");
    }

    public static T GetEnum<T>(this IConfiguration configuration, string key)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        string valueString = configuration.GetString(key);

        return Enum.TryParse(valueString, out T value)
            ? value
            : throw new ConfigurationException($"{key} not recognized");
    }
}
