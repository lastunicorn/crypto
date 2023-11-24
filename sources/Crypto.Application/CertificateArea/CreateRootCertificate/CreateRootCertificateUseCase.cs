using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Results;
using DustInTheWind.Crypto.Application.Steps;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.Ports.UserAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.CreateRootCertificate;

internal class CreateRootCertificateUseCase : IRequestHandler<CreateRootCertificateRequest, CreateRootCertificateResponse>
{
    private const StoreLocation DefaultStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultStoreName = StoreName.My;

    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;
    private readonly IUserInterface userInterface;

    private CreateRootCertificateResponse response;

    public CreateRootCertificateUseCase(ILog log, ICertificateRepository certificateRepository, IUserInterface userInterface)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
        this.userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
    }

    public Task<CreateRootCertificateResponse> Handle(CreateRootCertificateRequest request, CancellationToken cancellationToken)
    {
        response = new CreateRootCertificateResponse();

        GenericCertificate certificate = GenerateRootCert(request);
        InstallCertificate(certificate);

        return Task.FromResult(response);
    }

    private GenericCertificate GenerateRootCert(CreateRootCertificateRequest request)
    {
        StoreLocation storeLocation = Enum.IsDefined(typeof(StoreLocation), request.StoreLocation)
            ? request.StoreLocation
            : DefaultStoreLocation;

        StoreName storeName = Enum.IsDefined(typeof(StoreName), request.StoreName)
            ? request.StoreName
            : DefaultStoreName;

        RootCertificateBuilder builder = new()
        {
            SubjectName = $"CN={request.CertificateName},O=Dust in the Wind,OU=Informatics,L=Romania,C=US",
            FriendlyName = request.FriendlyName,
            PublicKeyLength = 4096,
            PrivateKeyLength = 4096,
            Validity = TimeSpan.FromDays(36500),
            PrivateKeyPersistence = PersistenceType.MachineLevel,
            AllowPrivateKeyExport = false,
            IsPrivateKeyEphemeral = false,
            StoreName = storeName,
            StoreLocation = storeLocation
        };

        response.CertificateGenerationResult = new CertificateGenerationResult
        {
            SubjectName = builder.SubjectName,
            FriendlyName = builder.FriendlyName,
            Validity = builder.Validity,
            PublicKeyLength = builder.PublicKeyLength,
            PrivateKeyLength = builder.PrivateKeyLength,
            PrivateKeyPersistence = builder.PrivateKeyPersistence,
            AllowExport = builder.AllowPrivateKeyExport,
            StoreLocation = builder.StoreLocation,
            StoreName = builder.StoreName
        };

        try
        {
            GenericCertificate certificate = builder.Build();
            return certificate;
        }
        catch (Exception ex)
        {
            response.CertificateGenerationResult.Error = ex;
            return null;
        }
    }

    private void InstallCertificate(GenericCertificate certificate)
    {
        InstallCertificateResult result = new()
        {
            StoreLocation = certificate.StoreLocation,
            StoreName = certificate.StoreName,
            CertificateName = certificate.GetName()
        };

        try
        {
            certificateRepository.Add(certificate);
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        response.InstallCertificateResult = result;
    }
}