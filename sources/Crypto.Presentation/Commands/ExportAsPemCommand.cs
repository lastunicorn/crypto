using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.PresentationAndUseCases.Steps;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

[NamedCommand("export-pem", Description = "Exports a specific certificate from the store as a pem file.")]
internal class ExportAsPemCommand : ICommand
{
    private readonly IFileSystem fileSystem;
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; } = StoreName.My;

    [NamedParameter("subject-name", ShortName = 's')]
    public string SubjectName { get; set; }

    public ExportAsPemCommand(IFileSystem fileSystem, ILog log, ICertificateRepository certificateRepository)
    {
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Execute()
    {
        IEnumerable<GenericCertificate> certificates = RetrieveCertificates();

        int index = 0;

        foreach (GenericCertificate genericCertificate in certificates)
        {
            ExportCertificate(genericCertificate, index);
            index++;
        }

        return Task.CompletedTask;
    }

    private IEnumerable<GenericCertificate> RetrieveCertificates()
    {
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = new CertificateIdentifier
            {
                Name = SubjectName,
                StoreLocation = StoreLocation,
                StoreName = StoreName
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