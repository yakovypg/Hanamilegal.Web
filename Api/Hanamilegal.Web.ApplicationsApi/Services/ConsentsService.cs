using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.FileSystem;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApiCommon.Requests;
using Hanamilegal.Web.ApiConfiguration.Options;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;
using Hanamilegal.Web.Contracts.Consents;
using Mapster;
using Microsoft.Extensions.Options;

namespace Hanamilegal.Web.ApplicationsApi.Services;

public sealed class ConsentsService : IConsentsService
{
    private readonly IConsentsRepository _consentsRepository;
    private readonly IOptions<DocumentsOptions> _documentsOptions;

    public ConsentsService(IConsentsRepository consentsRepository, IOptions<DocumentsOptions> documentsOptions)
    {
        ArgumentNullException.ThrowIfNull(consentsRepository, nameof(consentsRepository));
        ArgumentNullException.ThrowIfNull(documentsOptions, nameof(documentsOptions));

        _consentsRepository = consentsRepository;
        _documentsOptions = documentsOptions;
    }

    public async Task<ConsentAuditDto> CreatePersonalDataConsentAuditAsync(
        ApplicationProvider applicationProvider,
        RequestContext requestContext,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(requestContext, nameof(requestContext));

        DocumentManifest privacyPolicyManifest = await LoadPrivacyPolicyManifest(applicationProvider, cancellationToken);

        var consentAudit = new ConsentAudit()
        {
            ApplicationProvider = applicationProvider,
            SessionId = requestContext.SessionId,
            IpAddress = requestContext.IpAddress?.ToString() ?? string.Empty,
            UserAgent = requestContext.UserAgent.ToString(),
            RequestPath = requestContext.RequestPath.ToString(),
            DocumentName = privacyPolicyManifest.FileName,
            DocumentHash = privacyPolicyManifest.Sha256,
            DocumentVersion = privacyPolicyManifest.Version
        };

        await _consentsRepository.AddPersonalDataConsentAuditAsync(consentAudit, cancellationToken);

        return consentAudit.Adapt<ConsentAuditDto>();
    }

    public async Task<ConsentAuditDto> AddExternalEntityIdToPersonalDataConsentAuditAsync(
        Guid consentAuditId,
        Guid externalEntityId,
        CancellationToken cancellationToken = default)
    {
        ConsentAudit consentAudit = await _consentsRepository.AddExternalEntityIdToPersonalDataConsentAuditAsync(
            consentAuditId,
            externalEntityId,
            cancellationToken);

        return consentAudit.Adapt<ConsentAuditDto>();
    }

    public async Task<ConsentAuditDto?> GetByIdAsync(Guid id)
    {
        ConsentAudit? foundConsentAudit = await _consentsRepository.FindByIdAsync(id);
        return foundConsentAudit?.Adapt<ConsentAuditDto?>();
    }

    public async Task<PaginationResult<ConsentAuditDto>> SearchAsync(ConsentAuditSearchRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        var searchFilter = dto.SearchFilter?.Adapt<ConsentAuditSearchFilter?>();
        var sortFilter = dto.SortFilter?.Adapt<ConsentAuditSortFilter?>();

        var paginationFilter = dto.PaginationFilter?.Adapt<MongoPaginationFilter<ConsentAudit>?>()
            ?? new();

        IMongoFilter<ConsentAudit>?[] rawFilters = [searchFilter, sortFilter];
        IEnumerable<IMongoFilter<ConsentAudit>> filters = rawFilters.OfType<IMongoFilter<ConsentAudit>>();

        PaginationResult<ConsentAudit> foundConsentAudits = await _consentsRepository
            .FindAsync(paginationFilter, filters);

        return foundConsentAudits.Adapt<PaginationResult<ConsentAuditDto>>();
    }

    private async Task<DocumentManifest> LoadPrivacyPolicyManifest(
        ApplicationProvider applicationProvider,
        CancellationToken cancellationToken = default)
    {
        (string documentsDirectoryPath, DocumentNameOptions documentNames) =
            GetDocumentsInfo(applicationProvider);

        string privacyPolicyManifestPath = Path.Combine(
            documentsDirectoryPath,
            documentNames.PrivacyPolicyManifestName);

        string privacyPolicyManifesJson =
            await File.ReadAllTextAsync(privacyPolicyManifestPath, cancellationToken);

        return JsonSerializer.Deserialize<DocumentManifest>(privacyPolicyManifesJson)
            ?? throw new InvalidOperationException("Failed to load privacy policy manifest");
    }

    private (string DocumentsDirectoryPath, DocumentNameOptions DocumentNames) GetDocumentsInfo(
        ApplicationProvider applicationProvider)
    {
        DocumentsOptions options = _documentsOptions.Value;

        return applicationProvider switch
        {
            ApplicationProvider.LegalSupportApp =>
                (options.LegalSupportAppDocumentsDirectoryPath, options.LegalSupportAppDocumentNames),

            ApplicationProvider.TechApp =>
                (options.TechAppDocumentsDirectoryPath, options.TechAppDocumentNames),

            _ => throw new ArgumentOutOfRangeException(
                $"Application provider {applicationProvider} is not supported")
        };
    }
}
