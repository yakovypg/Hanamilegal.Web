namespace Hanamilegal.Web.ApiConfiguration.Options;

internal sealed record SwaggerOptions(string ApiVersion)
{
    public const string SectionName = "Swagger";

    public SwaggerOptions()
        : this(string.Empty)
    { }
}
