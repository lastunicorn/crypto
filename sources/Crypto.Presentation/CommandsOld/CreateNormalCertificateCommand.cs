using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.CommandsOld;

[NamedCommand("create-normal")]
internal class CreateNormalCertificateCommand : IConsoleCommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    [NamedParameter("parent-store-location", IsOptional = true)]
    public StoreLocation ParentStoreLocation { get; set; } = StoreLocation.CurrentUser;

    [NamedParameter("parent-store-name", IsOptional = true)]
    public StoreName ParentStoreName { get; set; } = StoreName.Root;

    [NamedParameter("parent-name", ShortName = 'p')]
    public string ParentName { get; set; }

    [NamedParameter("store-location", ShortName = 'L', IsOptional = true)]
    public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

    [NamedParameter("store-name", ShortName = 'S', IsOptional = true)]
    public StoreName StoreName { get; set; } = StoreName.CertificateAuthority;

    [NamedParameter("name", ShortName = 'n')]
    public string CertificateName { get; set; }

    public CreateNormalCertificateCommand(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Execute()
    {
        GenericCertificate parentCertificate = FindParentCertificate();
        GenericCertificate certificate = GenerateNormalCertificate(parentCertificate);

        ShowCertificate(certificate);
        InstallCertificate(certificate);

        return Task.CompletedTask;
    }

    private GenericCertificate FindParentCertificate()
    {
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = new CertificateIdentifier
            {
                Name = ParentName,
                StoreLocation = ParentStoreLocation,
                StoreName = ParentStoreName
            }
        };

        findCertificateStep.Execute();

        return findCertificateStep.FoundCertificates.FirstOrDefault();
    }

    private GenericCertificate GenerateNormalCertificate(GenericCertificate parentCertificate)
    {
        GenerateNormalCertificateStep generateNormalCertificateStep = new(log)
        {
            ParentCertificate = parentCertificate,
            SubjectName = $"CN={CertificateName},O=Dust in the Wind,OU=Informatics,L=Brașov,C=RO",
            KeyLength = 4096,
            StoreLocation = StoreLocation,
            StoreName = StoreName,
            PrivateKeyPersistence = PersistenceType.UserLevel
        };

        generateNormalCertificateStep.Execute();

        return generateNormalCertificateStep.GeneratedCertificate;
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