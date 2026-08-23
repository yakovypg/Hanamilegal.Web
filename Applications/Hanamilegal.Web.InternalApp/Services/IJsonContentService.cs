using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hanamilegal.Web.InternalApp.Services;

public interface IJsonContentService
{
    Task<T> ReadAsync<T>(HttpContent httpContent, CancellationToken cancellationToken = default);
}
