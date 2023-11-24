using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.WatersArea.FindWatersCertificates;

internal class FindWatersCertificatesUseCase : IRequestHandler<FindWatersCertificatesRequest, FindWatersCertificatesResponse>
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public FindWatersCertificatesUseCase(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task<FindWatersCertificatesResponse> Handle(FindWatersCertificatesRequest request, CancellationToken cancellationToken)
    {
        WatersCertificateIdentifiers certificateIdentifiers = new();

        foreach (CertificateIdentifier certificateIdentifier in certificateIdentifiers)
        {
            FindCertificateStep findCertificateStep = new(log, certificateRepository)
            {
                CertificateIdentifier = certificateIdentifier
            };
            findCertificateStep.Execute();
        }

        FindWatersCertificatesResponse response = new();

        return Task.FromResult(response);
    }
}