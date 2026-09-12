using System;
using System.Collections.Generic;

namespace Hanamilegal.Web.ApiCommon.Pagination;

public class PaginationResult<T>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalItemsCount { get; init; }
    public IReadOnlyList<T> Items { get; init; } = [];

    public int TotalPages => (int)Math.Ceiling((double)TotalItemsCount / PageSize);
}
