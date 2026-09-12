using MongoDB.Driver;

namespace Hanamilegal.Web.ApiCommon.Filters;

public interface IMongoFilter<T>
{
    FilterDefinition<T> Apply(FilterDefinition<T> filter, FindOptions<T, T> options);
}
