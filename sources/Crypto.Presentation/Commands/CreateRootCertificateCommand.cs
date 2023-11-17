using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.PresentationAndUseCases.Steps;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

/// <summary>
/// Call Example: crypto create-root -n "Dummy Root CA" -f "Dummy Root Certificate Authority"
/// Call Example: crypto create-root -n "Dummy Root CA" -f "Dummy Root Certificate Authority" -L LocalMachine -S Root
/// </summary>
[NamedCommand("create-root", Description = "Creates a new root certificate and installs it in the store.")]
internal class CreateRootCertificateCommand : ICommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; } = StoreName.Root;

    [NamedParameter("name", ShortName = 'n')]
    public string CertificateName { get; set; }

    [NamedParameter("friendly-name", ShortName = 'f', IsOptional = true)]
    public string FriendlyName { get; set; }

    public CreateRootCertificateCommand(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Execute()
    {
        GenericCertificate certificate = GenerateRootCert();
        ShowCertificate(certificate);
        InstallCertificate(certificate);

        return Task.CompletedTask;
    }

    private GenericCertificate GenerateRootCert()
    {
        GenerateRootCertStep generateRootCertStep = new(log)
        {
            SubjectName = $"CN={CertificateName},O=Dust in the Wind,OU=Informatics,L=Romania,C=US",
            FriendlyName = FriendlyName,
            KeyLength = 4096,
            ValidityYears = 100,
            PrivateKeyPersistence = PersistenceType.MachineLevel,
            StoreLocation = StoreLocation,
            StoreName = StoreName
        };

        generateRootCertStep.Execute();

        return generateRootCertStep.GeneratedCertificate;
    }

    private void ShowCertificate(GenericCertificate certificate)
    {
        ShowCertificateOverviewStep showCertificateOverviewStep = new(log)
        {
            Certificate = certificate
        };

        showCertificateOverviewStep.Execute();

        ShowCertificateKeysStep certificateKeysStep = new(log)
        {
            Certificate = certificate
        };

        certificateKeysStep.Execute();

        ShowCertificateExtensionsStep showCertificateExtensionsStep = new(log)
        {
            Certificate = certificate
        };

        showCertificateExtensionsStep.Execute();
    }

    private void InstallCertificate(GenericCertificate certificate)
    {
        InstallCertificateStep installCertificateStep = new(log, certificateRepository)
        {
            Certificate = certificate
        };

        installCertificateStep.Execute();
    }
}