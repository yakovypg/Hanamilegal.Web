using System.Linq;

namespace Hanamilegal.Web.ApiCommon.Filters;

public interface IFilter<T>
{
    IQueryable<T> Apply(IQueryable<T> query);
}
