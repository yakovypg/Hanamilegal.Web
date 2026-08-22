using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;

internal sealed class AccountsDbContext : IdentityDbContext
{
    public AccountsDbContext(DbContextOptions<AccountsDbContext> options)
        : base(options ?? throw new ArgumentNullException(nameof(options)))
    {
    }
}
