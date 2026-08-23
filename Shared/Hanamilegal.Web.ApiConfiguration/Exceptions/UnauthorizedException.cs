using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;

namespace Hanamilegal.Web.ApiConfiguration.Exceptions;

[Serializable]
public class UnauthorizedException : ApiException
{
    public UnauthorizedException()
        : base(GetDefaultMessage(), HttpStatusCode.Unauthorized) { }

    public UnauthorizedException(string? message)
        : base(message ?? GetDefaultMessage(), HttpStatusCode.Unauthorized) { }

    public UnauthorizedException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), HttpStatusCode.Unauthorized, innerException) { }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected UnauthorizedException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    private static string GetDefaultMessage()
    {
        return "Authorization failed";
    }
}
