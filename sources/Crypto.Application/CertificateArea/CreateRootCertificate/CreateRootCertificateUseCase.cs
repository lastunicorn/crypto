using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.CreateRootCertificate;

internal class CreateRootCertificateUseCase : IRequestHandler<CreateRootCertificateRequest>
{
    private const StoreLocation DefaultStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultStoreName = StoreName.My;

    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public CreateRootCertificateUseCase(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Handle(CreateRootCertificateRequest request, CancellationToken cancellationToken)
    {
        GenericCertificate certificate = GenerateRootCert(request);
        ShowCertificate(certificate);
        InstallCertificate(certificate);

        return Task.CompletedTask;
    }

    private GenericCertificate GenerateRootCert(CreateRootCertificateRequest request)
    {
        StoreLocation storeLocation = Enum.IsDefined(typeof(StoreLocation), request.StoreLocation)
            ? request.StoreLocation
            : DefaultStoreLocation;

        StoreName storeName = Enum.IsDefined(typeof(StoreName), request.StoreName)
            ? request.StoreName
            : DefaultStoreName;

        GenerateRootCertStep generateRootCertStep = new(log)
        {
            SubjectName = $"CN={request.CertificateName},O=Dust in the Wind,OU=Informatics,L=Romania,C=US",
            FriendlyName = request.FriendlyName,
            KeyLength = 4096,
            ValidityYears = 100,
            PrivateKeyPersistence = PersistenceType.MachineLevel,
            StoreLocation = storeLocation,
            StoreName = storeName
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