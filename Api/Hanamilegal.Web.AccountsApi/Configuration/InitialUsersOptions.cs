using System.Collections.Generic;

namespace Hanamilegal.Web.AccountsApi.Configuration;

public sealed record InitialUsersOptions(IEnumerable<InitialUser> Users)
{
    public const string SectionName = "InitialUsers";

    public InitialUsersOptions()
        : this([])
    { }
}
