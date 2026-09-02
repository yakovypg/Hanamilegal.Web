using System.Collections.Generic;

namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed class CorsPolicyOptions
{
    public IReadOnlyList<string> AllowedOrigins { get; init; } = [];
}
