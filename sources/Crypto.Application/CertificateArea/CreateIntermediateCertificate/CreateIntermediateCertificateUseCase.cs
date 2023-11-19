using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.CreateIntermediateCertificate;

internal class CreateIntermediateCertificateUseCase : IRequestHandler<CreateIntermediateCertificateRequest>
{
    private const StoreLocation DefaultParentStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultParentStoreName = StoreName.Root;
    private const StoreLocation DefaultStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultStoreName = StoreName.CertificateAuthority;

    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public CreateIntermediateCertificateUseCase(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task Handle(CreateIntermediateCertificateRequest request, CancellationToken cancellationToken)
    {
        GenericCertificate parentCertificate = FindParentCertificate(request);
        GenericCertificate intermediateCertificate = GenerateIntermediateCertificate(parentCertificate, request);

        ShowCertificateDetails(intermediateCertificate);
        InstallCertificate(intermediateCertificate);

        //VerifyCertificateIsInstalled(certificate);
        //VerifyCertificateParent(certificate, parentCertificate);

        return Task.CompletedTask;
    }

    private GenericCertificate FindParentCertificate(CreateIntermediateCertificateRequest request)
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

    private GenericCertificate GenerateIntermediateCertificate(GenericCertificate certificate, CreateIntermediateCertificateRequest request)
    {
        StoreLocation storeLocation = Enum.IsDefined(typeof(StoreLocation), request.StoreLocation)
            ? request.StoreLocation
            : DefaultStoreLocation;

        StoreName storeName = Enum.IsDefined(typeof(StoreName), request.StoreName)
            ? request.StoreName
            : DefaultStoreName;

        GenerateIntermediateCertStep generateIntermediateCertStep = new(log)
        {
            ParentCertificate = certificate,
            SubjectName = $"CN={request.CertificateName},O=Dust in the Wind,OU=Informatics,L=Brașov,C=RO",
            KeyLength = 4096,
            StoreLocation = storeLocation,
            StoreName = storeName,
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