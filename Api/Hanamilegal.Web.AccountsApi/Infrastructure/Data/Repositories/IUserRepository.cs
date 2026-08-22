using System.Threading.Tasks;
using Hanamilegal.Web.Auth.Models;
using Microsoft.AspNetCore.Identity;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;

internal interface IUserRepository
{
    Task<IdentityUser> AddAsync(string email, string password, UserRole role);
    Task DeleteByEmailAsync(string email);
    Task<IdentityUser?> FindByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
}
