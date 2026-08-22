using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;

namespace Hanamilegal.Web.ApiConfiguration.Exceptions;

[Serializable]
public class NotFoundException : ApiException
{
    public NotFoundException()
        : base(GetDefaultMessage(), HttpStatusCode.NotFound) { }

    public NotFoundException(string? message)
        : base(message ?? GetDefaultMessage(), HttpStatusCode.NotFound) { }

    public NotFoundException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), HttpStatusCode.NotFound, innerException) { }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected NotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    private static string GetDefaultMessage()
    {
        return "Resource not found";
    }
}
