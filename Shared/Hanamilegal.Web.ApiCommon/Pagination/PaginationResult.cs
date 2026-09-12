using System;
using System.Collections.Generic;

namespace Hanamilegal.Web.ApiCommon.Pagination;

public class PaginationResult<T>
{
    public long PageNumber { get; init; }
    public long PageSize { get; init; }
    public long TotalItemsCount { get; init; }
    public IReadOnlyList<T> Items { get; init; } = [];

    public long TotalPages => (int)Math.Ceiling((double)TotalItemsCount / PageSize);
}
