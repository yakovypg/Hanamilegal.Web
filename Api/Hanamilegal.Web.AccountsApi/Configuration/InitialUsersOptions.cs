using System.Collections.Generic;

namespace Hanamilegal.Web.AccountsApi.Configuration;

public sealed class InitialUsersOptions
{
    public const string SectionName = "InitialUsers";
    public IEnumerable<InitialUser> Users { get; init; } = [];
}
