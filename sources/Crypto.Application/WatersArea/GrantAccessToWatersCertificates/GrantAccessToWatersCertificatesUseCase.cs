using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.WatersArea.GrantAccessToWatersCertificates;

internal class GrantAccessToWatersCertificatesUseCase : IRequestHandler<GrantAccessToWatersCertificatesRequest, GrantAccessToWatersCertificatesResponse>
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public GrantAccessToWatersCertificatesUseCase(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task<GrantAccessToWatersCertificatesResponse> Handle(GrantAccessToWatersCertificatesRequest request, CancellationToken cancellationToken)
    {
        WatersCertificateIdentifiers watersCertificateIdentifiers = new();

        IEnumerable<CertificateIdentifier> certificateIdentifiers = request.CertificateType switch
        {
            CertificateType.Root => watersCertificateIdentifiers.Where(x => x.StoreName == StoreName.Root),
            CertificateType.Intermediate => watersCertificateIdentifiers.Where(x => x.StoreName == StoreName.CertificateAuthority),
            CertificateType.Normal => watersCertificateIdentifiers.Where(x => x.StoreName == StoreName.My),
            _ => watersCertificateIdentifiers
        };

        if (request.Filter != null)
            certificateIdentifiers = certificateIdentifiers.Where(x => x.Name.Contains(request.Filter));

        foreach (CertificateIdentifier watersCertificateIdentifier in certificateIdentifiers)
            GrantAccessTo(watersCertificateIdentifier);

        GrantAccessToWatersCertificatesResponse response = new();

        return Task.FromResult(response);
    }

    private void GrantAccessTo(CertificateIdentifier certificateIdentifier)
    {
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = certificateIdentifier
        };
        findCertificateStep.Execute();

        GrantReadAccessToCertificateStep grantReadAccessToCertificateStep = new(log)
        {
            Certificate = findCertificateStep.FoundCertificates?.FirstOrDefault()
        };

        grantReadAccessToCertificateStep.Execute();
    }
}