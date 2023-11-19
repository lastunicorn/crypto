using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.RemoveCertificate;

internal class RemoveCertificateUseCase : IRequestHandler<RemoveCertificateRequest>
{
    private const StoreLocation DefaultStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultStoreName = StoreName.My;

    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;
    private readonly IFileSystem fileSystem;

    public RemoveCertificateUseCase(ILog log, ICertificateRepository certificateRepository, IFileSystem fileSystem)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public Task Handle(RemoveCertificateRequest request, CancellationToken cancellationToken)
    {
        StoreLocation storeLocation = Enum.IsDefined(typeof(StoreLocation), request.StoreLocation)
            ? request.StoreLocation
            : DefaultStoreLocation;

        StoreName storeName = Enum.IsDefined(typeof(StoreName), request.StoreName)
            ? request.StoreName
            : DefaultStoreName;

        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = new CertificateIdentifier
            {
                Name = request.Name,
                StoreLocation = storeLocation,
                StoreName = storeName
            }
        };

        findCertificateStep.Execute();

        if (findCertificateStep.FoundCertificates is { Count: not 0 })
        {
            RemoveCertificateFromStoreStep removeCertificateFromStoreStep = new(log, certificateRepository)
            {
                Certificates = findCertificateStep.FoundCertificates
            };

            removeCertificateFromStoreStep.Execute();

            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}