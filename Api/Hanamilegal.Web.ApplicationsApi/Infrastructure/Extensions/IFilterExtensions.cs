using System;
using System.Collections.Generic;
using System.Linq;
using Hanamilegal.Web.ApiCommon.Filters;
using MongoDB.Driver;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Extensions;

internal static class IFilterExtensions
{
    internal static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IEnumerable<IFilter<T>>? filters)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return query.ApplyFilters(filters?.ToArray() ?? []);
    }

    internal static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, params IFilter<T>[] filters)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        ArgumentNullException.ThrowIfNull(filters, nameof(filters));

        foreach (IFilter<T> filter in filters ?? [])
        {
            query = filter.Apply(query);
        }

        return query;
    }

    internal static (FilterDefinition<T> Filter, FindOptions<T, T> Options) CombineFilters<T>(
        this IEnumerable<IMongoFilter<T>>? filters)
    {
        FilterDefinition<T> filter = Builders<T>.Filter.Empty;
        FindOptions<T, T> options = new();

        if (filters is not null)
        {
            foreach (IMongoFilter<T> currentFilter in filters)
            {
                filter = currentFilter.Apply(filter, options);
            }
        }

        return (filter, options);
    }
}
