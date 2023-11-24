using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.ExportAlezAsPfx;

internal class ExportAlezAsPfxUseCase : IRequestHandler<ExportAlezAsPfxRequest>
{
    private readonly IFileSystem fileSystem;
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;
    
    public ExportAlezAsPfxUseCase(IFileSystem fileSystem, ILog log, ICertificateRepository certificateRepository)
    {
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Handle(ExportAlezAsPfxRequest request, CancellationToken cancellationToken)
    {
        if (request.CertificateType is CertificateType.Root or CertificateType.All)
        {
            GenericCertificate alezRootCertificate = RetrieveRootCertificate();
            ExportCertificate(alezRootCertificate, "alez-root-certificate.pfx");
        }

        if (request.CertificateType is CertificateType.Intermediate or CertificateType.All)
        {
            GenericCertificate alezIntermediateCertificate = RetrieveIntermediateCertificate();
            ExportCertificate(alezIntermediateCertificate, "alez-intermediate-certificate.pfx");
        }

        if (request.CertificateType is CertificateType.Normal or CertificateType.All)
        {
            GenericCertificate alezNormalCertificate = RetrieveNormalCertificate();
            ExportCertificate(alezNormalCertificate, "alez-normal-certificate.pfx");
        }

        return Task.CompletedTask;
    }

    private GenericCertificate RetrieveRootCertificate()
    {
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = AlezCertificateIdentifiers.Root
        };
        findCertificateStep.Execute();
        return findCertificateStep.FoundCertificates.FirstOrDefault();
    }

    private GenericCertificate RetrieveIntermediateCertificate()
    {
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = AlezCertificateIdentifiers.Intermediate
        };
        findCertificateStep.Execute();
        return findCertificateStep.FoundCertificates.FirstOrDefault();
    }

    private GenericCertificate RetrieveNormalCertificate()
    {
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = AlezCertificateIdentifiers.Normal
        };
        findCertificateStep.Execute();
        return findCertificateStep.FoundCertificates.FirstOrDefault();
    }

    private void ExportCertificate(GenericCertificate certificate, string filePath)
    {
        SaveCertificateAsPfxStep saveCertificateAsPemStep = new(log, fileSystem)
        {
            Certificate = certificate,
            FileName = filePath
        };
        saveCertificateAsPemStep.Execute();
    }
}