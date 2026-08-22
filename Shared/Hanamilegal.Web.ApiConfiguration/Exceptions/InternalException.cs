using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;

namespace Hanamilegal.Web.ApiConfiguration.Exceptions;

[Serializable]
public class InternalException : ApiException
{
    public InternalException()
        : base(GetDefaultMessage(), HttpStatusCode.InternalServerError) { }

    public InternalException(string? message)
        : base(message ?? GetDefaultMessage(), HttpStatusCode.InternalServerError) { }

    public InternalException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), HttpStatusCode.InternalServerError, innerException) { }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected InternalException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    private static string GetDefaultMessage()
    {
        return "Internal server error occured";
    }
}
