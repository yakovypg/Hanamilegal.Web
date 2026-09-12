using System;

namespace Hanamilegal.Web.InternalApp.ViewModels;

public sealed class PaginationViewModel
{
    public long CurrentPage { get; init; }
    public long TotalPages { get; init; }
    public required Func<long, string> GetPageUrl { get; init; }
}
