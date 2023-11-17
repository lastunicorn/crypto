using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.PresentationAndUseCases.Steps;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

[NamedCommand("create-intermediate", Description = "Creates an intermediate certificate and installs it in the store.")]
internal class CreateIntermediateCertificateCommand : ICommand
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

    public CreateIntermediateCertificateCommand(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Execute()
    {
        GenericCertificate parentCertificate = FindParentCertificate();
        GenericCertificate intermediateCertificate = GenerateIntermediateCertificate(parentCertificate);

        ShowCertificateDetails(intermediateCertificate);
        InstallCertificate(intermediateCertificate);

        //VerifyCertificateIsInstalled(certificate);
        //VerifyCertificateParent(certificate, parentCertificate);

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

    private GenericCertificate GenerateIntermediateCertificate(GenericCertificate certificate)
    {
        GenerateIntermediateCertStep generateIntermediateCertStep = new(log)
        {
            ParentCertificate = certificate,
            SubjectName = $"CN={CertificateName},O=Dust in the Wind,OU=Informatics,L=Brașov,C=RO",
            KeyLength = 4096,
            StoreLocation = StoreLocation,
            StoreName = StoreName,
            PrivateKeyPersistence = PersistenceType.UserLevel
        };

        generateIntermediateCertStep.Execute();

        return generateIntermediateCertStep.GeneratedCertificate;
    }

    private void ShowCertificateDetails(GenericCertificate certificate)
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

    private void VerifyCertificateIsInstalled(X509Certificate2 certificate)
    {
        VerifyCertificateIsInstalledStep verifyCertificateIsInstalledStep = new(log)
        {
            Certificate = certificate,
            StoreLocation = StoreLocation.CurrentUser,
            StoreName = StoreName.CertificateAuthority
        };

        verifyCertificateIsInstalledStep.Execute();
    }

    private void VerifyCertificateParent(X509Certificate2 certificate, X509Certificate2 parentCertificate)
    {
        VerifyCertificateParentStep verifyParentStep = new(log)
        {
            Certificate = certificate,
            ParentCertificate = parentCertificate
        };

        verifyParentStep.Execute();
    }
}