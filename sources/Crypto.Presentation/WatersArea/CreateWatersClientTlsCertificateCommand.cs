using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.Ports.UserAccess;

namespace DustInTheWind.Crypto.Presentation.CommandsOld;

[NamedCommand("create-waters-tls", Enabled = false)]
internal class CreateWatersClientTlsCertificateCommand : IConsoleCommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;
    private readonly IUserInterface userInterface;

    public CreateWatersClientTlsCertificateCommand(ILog log, ICertificateRepository certificateRepository, IUserInterface userInterface)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
        this.userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
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
                Name = "waters_connect Root CA",
                StoreLocation = StoreLocation.LocalMachine,
                StoreName = StoreName.Root
            }
        };

        findCertificateStep.Execute();

        return findCertificateStep.FoundCertificates[0];
    }

    private GenericCertificate GenerateNormalCertificate(GenericCertificate parentCertificate)
    {
        GenerateNormalCertificateStep generateNormalCertificateStep = new(log)
        {
            ParentCertificate = parentCertificate,
            SubjectName = "CN=waters_connect Client TLS,O=Waters Corporation,OU=Informatics,L=Milford,C=US",
            FriendlyName = "waters_connect Client TLS",
            KeyLength = 4096,
            AllowExport = false,
            KeyUsage = new OidCollection
            {
                new(OidValues.ClientAuthentication)
            },
            StoreLocation = StoreLocation.LocalMachine,
            StoreName = StoreName.My
        };

        generateNormalCertificateStep.Execute();

        return generateNormalCertificateStep.GeneratedCertificate;
    }

    private void ShowCertificate(GenericCertificate certificate)
    {
        ShowCertificateOverviewStep showCertificateOverviewStep = new(log, userInterface)
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