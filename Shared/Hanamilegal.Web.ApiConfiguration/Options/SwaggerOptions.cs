namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed record SwaggerOptions(string ApiVersion)
{
    public const string SectionName = "Swagger";

    public SwaggerOptions()
        : this(string.Empty)
    { }
}
