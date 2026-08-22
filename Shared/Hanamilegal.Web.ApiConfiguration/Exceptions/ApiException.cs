using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;

namespace Hanamilegal.Web.ApiConfiguration.Exceptions;

[Serializable]
public abstract class ApiException : Exception
{
    protected ApiException() { }

    protected ApiException(string? message)
        : base(message ?? GetDefaultMessage()) { }

    protected ApiException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), innerException) { }

    protected ApiException(string? message, HttpStatusCode statusCode)
        : this(message, statusCode, null) { }

    protected ApiException(
        string? message,
        HttpStatusCode statusCode,
        Exception? innerException)
        : base(message ?? GetDefaultMessage(statusCode), innerException)
    {
        StatusCode = statusCode;
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected ApiException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ArgumentNullException.ThrowIfNull(info, nameof(info));
        StatusCode = (HttpStatusCode)info.GetInt32(nameof(StatusCode));
    }

    public HttpStatusCode StatusCode { get; }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ArgumentNullException.ThrowIfNull(info, nameof(info));

        info.AddValue(nameof(StatusCode), (int)StatusCode);
        base.GetObjectData(info, context);
    }

    private static string GetDefaultMessage()
    {
        return $"Error occurred";
    }

    private static string GetDefaultMessage(HttpStatusCode statusCode)
    {
        return $"Error {statusCode} occurred";
    }
}
