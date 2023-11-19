using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.CreateNormalCertificate;

internal class CreateNormalCertificateUseCase : IRequestHandler<CreateNormalCertificateRequest>
{
    private const StoreLocation DefaultParentStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultParentStoreName = StoreName.Root;
    private const StoreLocation DefaultStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultStoreName = StoreName.CertificateAuthority;

    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public CreateNormalCertificateUseCase(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Handle(CreateNormalCertificateRequest request, CancellationToken cancellationToken)
    {
        GenericCertificate parentCertificate = FindParentCertificate(request);
        GenericCertificate certificate = GenerateNormalCertificate(parentCertificate, request);

        ShowCertificate(certificate);
        InstallCertificate(certificate);

        return Task.CompletedTask;
    }

    private GenericCertificate FindParentCertificate(CreateNormalCertificateRequest request)
    {
        StoreLocation parentStoreLocation = Enum.IsDefined(typeof(StoreLocation), request.ParentStoreLocation)
            ? request.ParentStoreLocation
            : DefaultParentStoreLocation;

        StoreName parentStoreName = Enum.IsDefined(typeof(StoreName), request.ParentStoreName)
            ? request.ParentStoreName
            : DefaultParentStoreName;

        FindCertificateStep findCertificateStep = new(log, certificateRepository)
        {
            CertificateIdentifier = new CertificateIdentifier
            {
                Name = request.ParentName,
                StoreLocation = parentStoreLocation,
                StoreName = parentStoreName
            }
        };

        findCertificateStep.Execute();

        return findCertificateStep.FoundCertificates.FirstOrDefault();
    }

    private GenericCertificate GenerateNormalCertificate(GenericCertificate parentCertificate, CreateNormalCertificateRequest request)
    {
        StoreLocation storeLocation = Enum.IsDefined(typeof(StoreLocation), request.StoreLocation)
            ? request.StoreLocation
            : DefaultStoreLocation;

        StoreName storeName = Enum.IsDefined(typeof(StoreName), request.StoreName)
            ? request.StoreName
            : DefaultStoreName;

        GenerateNormalCertificateStep generateNormalCertificateStep = new(log)
        {
            ParentCertificate = parentCertificate,
            SubjectName = $"CN={request.CertificateName},O=Dust in the Wind,OU=Informatics,L=Brașov,C=RO",
            KeyLength = 4096,
            StoreLocation = storeLocation,
            StoreName = storeName,
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