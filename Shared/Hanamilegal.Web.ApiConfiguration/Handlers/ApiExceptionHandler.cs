using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiConfiguration.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.ApiConfiguration.Handlers;

public class ApiExceptionHandler
{
    private readonly ILogger<ApiExceptionHandler> _logger;

    public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _logger = logger;
    }

    public async Task HandleExceptionAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        IExceptionHandlerPathFeature? exceptionFeature = context.Features
            .Get<IExceptionHandlerPathFeature>();

        Exception? exception = exceptionFeature?.Error;

        if (exception is not null)
        {
            _logger.LogWarning("API error occured: {Message}", exception.Message);
            _logger.LogError(exception, "{@Exception}", exception);
        }

        (HttpStatusCode errorStatusCode, string errorMessage) = exception switch
        {
            ApiException apiException => (apiException.StatusCode, apiException.Message),
            _ => (HttpStatusCode.InternalServerError, nameof(HttpStatusCode.InternalServerError))
        };

        await WriteErrorToResponseAsync(errorStatusCode, errorMessage, context);
    }

    private static JsonSerializerOptions CreateJsonSerializerOptions()
    {
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        return jsonSerializerOptions;
    }

    private static async Task WriteErrorToResponseAsync(
        HttpStatusCode errorStatusCode,
        string? errorMessage,
        HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        var errorData = new
        {
            StatusCode = errorStatusCode,
            Message = errorMessage
        };

        JsonSerializerOptions jsonSerializerOptions = CreateJsonSerializerOptions();
        string response = JsonSerializer.Serialize(errorData, jsonSerializerOptions);

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(response);
    }
}
