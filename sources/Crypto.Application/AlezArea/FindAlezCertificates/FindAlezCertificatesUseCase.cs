using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.FindAlezCertificates;

internal class FindAlezCertificatesUseCase : IRequestHandler<FindAlezCertificatesRequest>
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public FindAlezCertificatesUseCase(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Handle(FindAlezCertificatesRequest request, CancellationToken cancellationToken)
    {
        AlezCertificateIdentifiers alezCertificateIdentifiers = new();

        foreach (CertificateIdentifier alezCertificateIdentifier in alezCertificateIdentifiers)
        {
            FindCertificateStep findCertificateStep = new(log, certificateRepository)
            {
                CertificateIdentifier = alezCertificateIdentifier
            };
            findCertificateStep.Execute();
        }

        return Task.CompletedTask;
    }
}