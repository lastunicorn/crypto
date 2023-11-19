using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.ExportAsPem;

internal class ExportAsPemUseCase : IRequestHandler<ExportAsPemRequest>
{
    private const StoreLocation DefaultStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultStoreName = StoreName.My;

    private readonly IFileSystem fileSystem;
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public ExportAsPemUseCase(IFileSystem fileSystem, ILog log, ICertificateRepository certificateRepository)
    {
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Handle(ExportAsPemRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<GenericCertificate> certificates = RetrieveCertificates(request);

        int index = 0;

        foreach (GenericCertificate genericCertificate in certificates)
        {
            ExportCertificate(genericCertificate, index);
            index++;
        }

        return Task.CompletedTask;
    }

    private IEnumerable<GenericCertificate> RetrieveCertificates(ExportAsPemRequest request)
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
                Name = request.SubjectName,
                StoreLocation = storeLocation,
                StoreName = storeName
            }
        };

        findCertificateStep.Execute();

        return findCertificateStep.FoundCertificates;
    }

    private void ExportCertificate(GenericCertificate certificate, int index)
    {
        SaveCertificateAsPemStep saveCertificateAsPemStep = new(log, fileSystem)
        {
            Certificate = certificate,
            FileName = $"certificate-{index:00}.pem"
        };
        saveCertificateAsPemStep.Execute();
    }
}