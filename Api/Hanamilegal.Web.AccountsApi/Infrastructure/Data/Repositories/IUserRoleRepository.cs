using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;

public interface IUserRoleRepository
{
    Task<IdentityRole> AddAsync(string roleName);
    Task DeleteByNameAsync(string roleName);
    Task<IdentityRole?> FindByNameAsync(string roleName);
    Task<bool> ExistsByNameAsync(string roleName);
}
