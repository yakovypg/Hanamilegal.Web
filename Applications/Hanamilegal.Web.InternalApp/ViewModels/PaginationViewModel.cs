using System;

namespace Hanamilegal.Web.InternalApp.ViewModels;

public sealed class PaginationViewModel
{
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public required Func<int, string> GetPageUrl { get; init; }
}
