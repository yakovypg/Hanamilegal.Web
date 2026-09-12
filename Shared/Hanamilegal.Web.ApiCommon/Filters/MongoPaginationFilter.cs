using System;
using MongoDB.Driver;

namespace Hanamilegal.Web.ApiCommon.Filters;

public class MongoPaginationFilter<T> : IMongoFilter<T>
{
    private int _pageNumber;
    private int _pageSize;

    public MongoPaginationFilter()
        : this(1, 1)
    {
    }

    public MongoPaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int PageNumber
    {
        get => _pageNumber;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, nameof(value));
            _pageNumber = value;
        }
    }

    public int PageSize
    {
        get => _pageSize;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, nameof(value));
            _pageSize = value;
        }
    }

    public FilterDefinition<T> Apply(FilterDefinition<T> filter, FindOptions<T, T> options)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        int skip = checked((PageNumber - 1) * PageSize);

        options.Skip = skip;
        options.Limit = PageSize;

        return filter;
    }
}
