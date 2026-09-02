using System;
using Hanamilegal.Web.AccountsApi.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;

public sealed class AccountsDbContext : IdentityDbContext
{
    public AccountsDbContext(DbContextOptions<AccountsDbContext> options)
        : base(options ?? throw new ArgumentNullException(nameof(options)))
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
