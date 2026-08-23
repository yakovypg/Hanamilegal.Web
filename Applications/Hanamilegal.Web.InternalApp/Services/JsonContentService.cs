using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hanamilegal.Web.InternalApp.Services;

public class JsonContentService : IJsonContentService
{
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonContentService(IOptions<JsonOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        _jsonOptions = options.Value.JsonSerializerOptions;
    }

    public async Task<T> ReadAsync<T>(
        HttpContent httpContent,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpContent, nameof(httpContent));

        return await httpContent.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken)
            ?? throw new InvalidDataException("Response content has invalid data");
    }
}
