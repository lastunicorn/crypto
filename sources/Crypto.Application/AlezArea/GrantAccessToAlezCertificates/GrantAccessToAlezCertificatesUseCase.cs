using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using System.Security.Cryptography.X509Certificates;
using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.GrantAccessToAlezCertificates;

internal class GrantAccessToAlezCertificatesUseCase : IRequestHandler<GrantAccessToAlezCertificatesRequest, GrantAccessToAlezCertificatesResponse>
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public GrantAccessToAlezCertificatesUseCase(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task<GrantAccessToAlezCertificatesResponse> Handle(GrantAccessToAlezCertificatesRequest request, CancellationToken cancellationToken)
    {
        AlezCertificateIdentifiers alezCertificateIdentifiers = new();

        IEnumerable<CertificateIdentifier> certificateIdentifiers = request.CertificateType switch
        {
            CertificateType.Root => alezCertificateIdentifiers.Where(x => x.StoreName == StoreName.Root),
            CertificateType.Intermediate => alezCertificateIdentifiers.Where(x => x.StoreName == StoreName.CertificateAuthority),
            CertificateType.Normal => alezCertificateIdentifiers.Where(x => x.StoreName == StoreName.My),
            _ => alezCertificateIdentifiers
        };

        if (request.Filter != null)
            certificateIdentifiers = certificateIdentifiers.Where(x => x.Name.Contains(request.Filter));

        foreach (CertificateIdentifier watersCertificateIdentifier in certificateIdentifiers)
            GrantAccessTo(watersCertificateIdentifier);

        GrantAccessToAlezCertificatesResponse response = new();

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