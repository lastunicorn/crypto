using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.ExportAlezAsPem;

internal class ExportAlezAsPemUseCase : IRequestHandler<ExportAlezAsPemRequest>
{
    private readonly IFileSystem fileSystem;
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public ExportAlezAsPemUseCase(IFileSystem fileSystem, ILog log, ICertificateRepository certificateRepository)
    {
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Handle(ExportAlezAsPemRequest request, CancellationToken cancellationToken)
    {
        if (request.CertificateType is CertificateType.Root or CertificateType.All)
        {
            GenericCertificate alezRootCertificate = RetrieveRootCertificate();
            ExportCertificate(alezRootCertificate, "alez-root-certificate.pem");
        }

        if (request.CertificateType is CertificateType.Intermediate or CertificateType.All)
        {
            GenericCertificate alezIntermediateCertificate = RetrieveIntermediateCertificate();
            ExportCertificate(alezIntermediateCertificate, "alez-intermediate-certificate.pem");
        }

        if (request.CertificateType is CertificateType.Normal or CertificateType.All)
        {
            GenericCertificate alezNormalCertificate = RetrieveNormalCertificate();
            ExportCertificate(alezNormalCertificate, "alez-normal-certificate.pem");
        }

        return Task.CompletedTask;
    }

    private GenericCertificate RetrieveRootCertificate()
    {
        AlezCertificateIdentifiers alezCertificateIdentifiers = new();

        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = alezCertificateIdentifiers.Root
        };
        findCertificateStep.Execute();
        return findCertificateStep.FoundCertificates.FirstOrDefault();
    }

    private GenericCertificate RetrieveIntermediateCertificate()
    {
        AlezCertificateIdentifiers alezCertificateIdentifiers = new();

        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = alezCertificateIdentifiers.Intermediate
        };
        findCertificateStep.Execute();
        return findCertificateStep.FoundCertificates.FirstOrDefault();
    }

    private GenericCertificate RetrieveNormalCertificate()
    {
        AlezCertificateIdentifiers alezCertificateIdentifiers = new();

        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = alezCertificateIdentifiers.Normal
        };
        findCertificateStep.Execute();
        return findCertificateStep.FoundCertificates.FirstOrDefault();
    }

    private void ExportCertificate(GenericCertificate certificate, string filePath)
    {
        SaveCertificateAsPemStep saveCertificateAsPemStep = new(log, fileSystem)
        {
            Certificate = certificate,
            FileName = filePath
        };
        saveCertificateAsPemStep.Execute();
    }
}