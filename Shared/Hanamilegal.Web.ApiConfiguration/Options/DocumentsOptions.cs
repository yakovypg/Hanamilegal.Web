namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed class DocumentsOptions
{
    public const string SectionName = "Documents";

    public string LegalSupportAppDocumentsDirectoryPath { get; init; } = string.Empty;
    public DocumentNameOptions LegalSupportAppDocumentNames { get; init; } = new();

    public string TechAppDocumentsDirectoryPath { get; init; } = string.Empty;
    public DocumentNameOptions TechAppDocumentNames { get; init; } = new();
}
