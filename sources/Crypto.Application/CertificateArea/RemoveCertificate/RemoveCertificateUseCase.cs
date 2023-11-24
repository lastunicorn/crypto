using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.RemoveCertificate;

internal class RemoveCertificateUseCase : IRequestHandler<RemoveCertificateRequest, RemoveCertificateResponse>
{
    private const StoreLocation DefaultStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultStoreName = StoreName.My;

    private readonly ICertificateRepository certificateRepository;
    private readonly IFileSystem fileSystem;

    private RemoveCertificateResponse response;

    public RemoveCertificateUseCase(ICertificateRepository certificateRepository, IFileSystem fileSystem)
    {
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public Task<RemoveCertificateResponse> Handle(RemoveCertificateRequest request, CancellationToken cancellationToken)
    {
        response = new RemoveCertificateResponse();

        List<GenericCertificate> certificates = RetrieveCertificates(request);

        if (certificates is { Count: not 0 })
        {
            foreach (GenericCertificate certificate in certificates)
                RemoveCertificate(certificate);
        }

        return Task.FromResult(response);
    }

    private List<GenericCertificate> RetrieveCertificates(RemoveCertificateRequest request)
    {
        StoreLocation storeLocation = Enum.IsDefined(typeof(StoreLocation), request.StoreLocation)
            ? request.StoreLocation
            : DefaultStoreLocation;

        StoreName storeName = Enum.IsDefined(typeof(StoreName), request.StoreName)
            ? request.StoreName
            : DefaultStoreName;

        CertificateIdentifier certificateIdentifier = new()
        {
            Name = request.Name,
            StoreLocation = storeLocation,
            StoreName = storeName
        };

        List<GenericCertificate> foundCertificates = certificateRepository.Get(certificateIdentifier)
            .ToList();
        
        response.StoreLocation = storeLocation;
        response.StoreName = storeName;
        response.CertificateName = request.Name;
        response.CertificateCount = (int)foundCertificates?.Count;

        return foundCertificates;
    }

    private void RemoveCertificate(GenericCertificate certificate)
    {
        CertificateRemoval certificateRemoval = new(certificate, certificateRepository);
        CertificateRemovalResult result = certificateRemoval.Execute();

        response.CertificateRemovalResults.Add(result);
    }
}