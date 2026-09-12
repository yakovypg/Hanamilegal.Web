using System;
using System.Linq;

namespace Hanamilegal.Web.ApiCommon.Filters;

public class PaginationFilter<T> : IFilter<T>
{
    private int _pageNumber;
    private int _pageSize;

    public PaginationFilter()
        : this(1, 1)
    {
    }

    public PaginationFilter(int pageNumber, int pageSize)
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

    public IQueryable<T> Apply(IQueryable<T> query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        int skip = checked((PageNumber - 1) * PageSize);

        return query
            .Skip(skip)
            .Take(PageSize);
    }
}
