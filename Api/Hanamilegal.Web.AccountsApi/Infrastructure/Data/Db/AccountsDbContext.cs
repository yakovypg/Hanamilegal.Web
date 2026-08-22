using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;

internal class AccountsDbContext : IdentityDbContext
{
    internal AccountsDbContext(DbContextOptions<AccountsDbContext> options)
        : base(options ?? throw new ArgumentNullException(nameof(options)))
    {
    }
}
