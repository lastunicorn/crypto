using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.PresentationAndUseCases.Steps;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Commands;

/// <summary>
/// Call Example: crypto alez-create
/// Call Example: crypto alez-create -t root
/// Call Example: crypto alez-create -t intermediate
/// Call Example: crypto alez-create -t normal
/// </summary>
[NamedCommand("alez-create", Description = "Creates the three \"alez\" certificates: one root, one intermediate and one normal.")]
internal class CreateAlezCertificatesCommand : ICommand
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;
    private GenericCertificate rootCertificate;
    private GenericCertificate intermediateCertificate;
    private GenericCertificate normalCertificate;

    [NamedParameter("type", ShortName = 't', IsOptional = true)]
    public CertificateType CertificateType { get; set; } = CertificateType.All;

    public CreateAlezCertificatesCommand(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Execute()
    {
        if (CertificateType is CertificateType.All or CertificateType.Root)
        {
            GenerateRootCert();
            ShowRootCertificate();
            InstallRootCertificate();
        }

        if (CertificateType is CertificateType.All or CertificateType.Intermediate)
        {
            if (rootCertificate == null)
                FindRootCertificate();

            GenerateIntermediateCertificate();
            ShowIntermediateCertificate();
            InstallIntermediateCertificate();
        }

        if (CertificateType is CertificateType.All or CertificateType.Normal)
        {
            if (intermediateCertificate == null)
                FindIntermediateCertificate();

            GenerateNormalCertificate();
            ShowNormalCertificate();
            InstallCertificate();
        }

        return Task.CompletedTask;
    }

    #region Root

    private void GenerateRootCert()
    {
        GenerateAlezRootCertificateStep generateAlezRootCertificateStep = new(log);
        generateAlezRootCertificateStep.Execute();
        rootCertificate = generateAlezRootCertificateStep.GeneratedCertificate;
    }

    private void FindRootCertificate()
    {
        AlezCertificateIdentifiers alezCertificateIdentifiers = new();

        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = alezCertificateIdentifiers.Root
        };
        findCertificateStep.Execute();
        rootCertificate = findCertificateStep.FoundCertificates.FirstOrDefault();
    }

    private void ShowRootCertificate()
    {
        ShowCertificate(rootCertificate, "Alez Root");
    }

    private void InstallRootCertificate()
    {
        InstallCertificateStep installCertificateStep = new(log, certificateRepository)
        {
            Certificate = rootCertificate
        };

        installCertificateStep.Execute();
    }

    #endregion

    #region Intermediate

    private void GenerateIntermediateCertificate()
    {
        GenerateAlezIntermediateCertStep generateAlezIntermediateCertStep = new(log)
        {
            ParentCertificate = rootCertificate
        };
        generateAlezIntermediateCertStep.Execute();

        intermediateCertificate = generateAlezIntermediateCertStep.GeneratedCertificate;
    }

    private void FindIntermediateCertificate()
    {
        AlezCertificateIdentifiers alezCertificateIdentifiers = new();

        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = alezCertificateIdentifiers.Intermediate
        };
        findCertificateStep.Execute();
        intermediateCertificate = findCertificateStep.FoundCertificates.FirstOrDefault();
    }

    private void ShowIntermediateCertificate()
    {
        ShowCertificate(intermediateCertificate, "Alez Intermediate");
    }

    private void InstallIntermediateCertificate()
    {
        InstallCertificateStep installCertificateStep = new(log, certificateRepository)
        {
            Certificate = intermediateCertificate
        };

        installCertificateStep.Execute();
    }

    #endregion

    #region Normal

    private void GenerateNormalCertificate()
    {
        GenerateAlezNormalCertificateStep generateNormalCertificateStep = new(log)
        {
            ParentCertificate = intermediateCertificate
        };

        generateNormalCertificateStep.Execute();

        normalCertificate = generateNormalCertificateStep.GeneratedCertificate;
    }

    private void ShowNormalCertificate()
    {
        ShowCertificate(normalCertificate, "Alez Normal");
    }

    private void ShowCertificate(GenericCertificate certificate, string details)
    {
        ShowCertificateOverviewStep showCertificateOverviewStep = new(log)
        {
            Subtitle = details,
            Certificate = certificate
        };

        showCertificateOverviewStep.Execute();

        ShowCertificateKeysStep certificateKeysStep = new(log)
        {
            Subtitle = details,
            Certificate = certificate
        };

        certificateKeysStep.Execute();

        ShowCertificateExtensionsStep showCertificateExtensionsStep = new(log)
        {
            Subtitle = details,
            Certificate = certificate
        };

        showCertificateExtensionsStep.Execute();
    }

    private void InstallCertificate()
    {
        InstallCertificateStep installCertificateStep = new(log, certificateRepository)
        {
            Certificate = normalCertificate
        };

        installCertificateStep.Execute();
    }

    #endregion
}