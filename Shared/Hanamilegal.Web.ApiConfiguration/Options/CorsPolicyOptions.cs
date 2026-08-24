using System.Collections.Generic;

namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed record CorsPolicyOptions(IReadOnlyList<string> AllowedOrigins)
{
    public CorsPolicyOptions()
        : this([])
    { }
}
