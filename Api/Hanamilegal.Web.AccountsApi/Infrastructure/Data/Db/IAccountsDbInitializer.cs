using System.Threading.Tasks;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;

internal interface IAccountsDbInitializer
{
    Task InitializeDatabaseAsync();
}
