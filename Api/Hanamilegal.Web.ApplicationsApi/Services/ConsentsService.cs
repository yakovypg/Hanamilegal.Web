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
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;
using Hanamilegal.Web.Contracts.Consents;
using Mapster;
using Microsoft.Extensions.Options;

namespace Hanamilegal.Web.ApplicationsApi.Services;

public sealed class ConsentsService : IConsentsService
{
    private readonly IConsentsRepository _consentsRepository;
    private readonly IOptions<PathOptions> _pathOptions;
    private readonly IOptions<FileNameOptions> _fileNameOptions;

    public ConsentsService(
        IConsentsRepository consentsRepository,
        IOptions<PathOptions> pathOptions,
        IOptions<FileNameOptions> fileNameOptions)
    {
        ArgumentNullException.ThrowIfNull(consentsRepository, nameof(consentsRepository));
        ArgumentNullException.ThrowIfNull(pathOptions, nameof(pathOptions));
        ArgumentNullException.ThrowIfNull(fileNameOptions, nameof(fileNameOptions));

        _consentsRepository = consentsRepository;
        _pathOptions = pathOptions;
        _fileNameOptions = fileNameOptions;
    }

    public async Task<ConsentAuditDto> CreatePersonalDataConsentAuditAsync(
        RequestContext requestContext,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(requestContext, nameof(requestContext));

        DocumentManifest privacyPolicyManifest = await LoadPrivacyPolicyManifest(cancellationToken);

        var consentAudit = new ConsentAudit()
        {
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

    public async Task<PaginationResult<ConsentAuditDto>> SearchAllAsync(ConsentAuditSearchRequestDto dto)
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
        CancellationToken cancellationToken = default)
    {
        string privacyPolicyManifestName = _fileNameOptions.Value.PrivacyPolicyManifestName;
        string documentsDirPath = _pathOptions.Value.Documents;

        string privacyPolicyManifesPath =
            Path.Combine(documentsDirPath, privacyPolicyManifestName);

        string privacyPolicyManifesJson =
            await File.ReadAllTextAsync(privacyPolicyManifesPath, cancellationToken);

        return JsonSerializer.Deserialize<DocumentManifest>(privacyPolicyManifesJson)
            ?? throw new InvalidOperationException("Failed to load privacy policy manifest");
    }
}
