using System.Collections.Generic;

namespace Hanamilegal.Web.AccountsApi.Configuration;

internal sealed record InitialUsersOptions(List<InitialUser> Users)
{
    public const string SectionName = "InitialUsers";

    public InitialUsersOptions()
        : this([])
    { }
}
