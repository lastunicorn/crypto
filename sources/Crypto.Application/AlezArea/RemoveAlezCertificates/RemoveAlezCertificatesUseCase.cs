using DustInTheWind.Crypto.Application.Results;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.RemoveAlezCertificates;

internal class RemoveAlezCertificatesUseCase : IRequestHandler<RemoveAlezCertificatesRequest, RemoveAlezCertificatesResponse>
{
    private readonly ICertificateRepository certificateRepository;

    public RemoveAlezCertificatesUseCase(ICertificateRepository certificateRepository)
    {
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task<RemoveAlezCertificatesResponse> Handle(RemoveAlezCertificatesRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<CertificateIdentifier> certificateIdentifiers = GenerateIdentifiers(request.CertificateType);
        RemoveAlezCertificatesResponse response = SearchAndRemove(certificateIdentifiers);

        return Task.FromResult(response);
    }

    private static IEnumerable<CertificateIdentifier> GenerateIdentifiers(CertificateType certificateType)
    {
        AlezCertificateIdentifiers alezCertificateIdentifiers = new();
        return alezCertificateIdentifiers.Enumerate(certificateType);
    }

    private RemoveAlezCertificatesResponse SearchAndRemove(IEnumerable<CertificateIdentifier> certificateIdentifiers)
    {
        RemoveAlezCertificatesResponse response = new();

        foreach (CertificateIdentifier certificateIdentifier in certificateIdentifiers)
        {
            CertificateSearchAndRemoval certificateSearchAndRemoval = new(certificateIdentifier, certificateRepository);
            CertificateSearchAndRemovalResult result = certificateSearchAndRemoval.Execute();

            response.Results.Add(result);
        }

        return response;
    }
}