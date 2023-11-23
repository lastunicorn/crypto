using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.FindCertificate;

internal class FindCertificateUseCase : IRequestHandler<FindCertificateRequest, FindCertificateResponse>
{
    private const StoreLocation DefaultStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultStoreName = StoreName.My;

    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    public FindCertificateUseCase(ILog log, ICertificateRepository certificateRepository)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task<FindCertificateResponse> Handle(FindCertificateRequest request, CancellationToken cancellationToken)
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

        FindCertificateResponse  response = new()
        {
            FindCertificateResult = new FindCertificateResult
            {
                StoreLocation = storeLocation,
                StoreName = storeName,
                CertificateName = request.Name,
                CertificateCount = (int)foundCertificates?.Count
            }
        };

        return Task.FromResult(response);
    }
}