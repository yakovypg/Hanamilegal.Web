using System;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.Contracts.Applications;
using Mapster;

namespace Hanamilegal.Web.ApplicationsApi.Mapping;

public static class MapsterConfig
{
    public static void Register(TypeAdapterConfig config)
    {
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        config.NewConfig<CreateApplicationRequestDto, Application>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAtUtc);
    }
}
