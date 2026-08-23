using System.Threading.Tasks;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;

public interface IAccountsDbInitializer
{
    Task InitializeDatabaseAsync();
}
