using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Hanamilegal.Web.ApiCommon.Requests;

public record RequestContext(
    string SessionId,
    IPAddress? IpAddress,
    StringValues UserAgent,
    PathString RequestPath)
{
    public static RequestContext Create(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext, nameof(httpContext));

        return new RequestContext(
            SessionId: httpContext.Session.Id,
            IpAddress: httpContext.Connection.RemoteIpAddress,
            UserAgent: httpContext.Request.Headers.UserAgent,
            RequestPath: httpContext.Request.Path);
    }
}
