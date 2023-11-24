using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.Ports.UserAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.CreateAlezCertificates;

internal class CreateAlezCertificatesUseCase : IRequestHandler<CreateAlezCertificatesRequest>
{
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;
    private readonly IUserInterface userInterface;

    private GenericCertificate rootCertificate;
    private GenericCertificate intermediateCertificate;
    private GenericCertificate normalCertificate;

    public CreateAlezCertificatesUseCase(ILog log, ICertificateRepository certificateRepository, IUserInterface userInterface)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
        this.userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
    }

    public Task Handle(CreateAlezCertificatesRequest request, CancellationToken cancellationToken)
    {
        if (request.CertificateType is CertificateType.All or CertificateType.Root)
        {
            GenerateRootCert();
            ShowRootCertificate();
            InstallRootCertificate();
        }

        if (request.CertificateType is CertificateType.All or CertificateType.Intermediate)
        {
            if (rootCertificate == null)
                FindRootCertificate();

            GenerateIntermediateCertificate();
            ShowIntermediateCertificate();
            InstallIntermediateCertificate();
        }

        if (request.CertificateType is CertificateType.All or CertificateType.Normal)
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
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = AlezCertificateIdentifiers.Root
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
        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = AlezCertificateIdentifiers.Intermediate
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
        ShowCertificateOverviewStep showCertificateOverviewStep = new(log, userInterface)
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