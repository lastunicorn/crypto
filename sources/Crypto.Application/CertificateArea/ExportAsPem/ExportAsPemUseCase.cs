using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Results;
using DustInTheWind.Crypto.Domain;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.ExportAsPem;

internal class ExportAsPemUseCase : IRequestHandler<ExportAsPemRequest, ExportAsPemResponse>
{
    private const StoreLocation DefaultStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultStoreName = StoreName.My;

    private readonly IFileSystem fileSystem;
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;

    private ExportAsPemResponse response;

    public ExportAsPemUseCase(IFileSystem fileSystem, ILog log, ICertificateRepository certificateRepository)
    {
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task<ExportAsPemResponse> Handle(ExportAsPemRequest request, CancellationToken cancellationToken)
    {
        response = new ExportAsPemResponse();

        IEnumerable<GenericCertificate> certificates = RetrieveCertificates(request);
        ExportCertificates(certificates);

        return Task.FromResult(response);
    }

    private IEnumerable<GenericCertificate> RetrieveCertificates(ExportAsPemRequest request)
    {
        StoreLocation storeLocation = Enum.IsDefined(typeof(StoreLocation), request.StoreLocation)
            ? request.StoreLocation
            : DefaultStoreLocation;

        StoreName storeName = Enum.IsDefined(typeof(StoreName), request.StoreName)
            ? request.StoreName
            : DefaultStoreName;

        CertificateIdentifier certificateIdentifier = new()
        {
            Name = request.SubjectName,
            StoreLocation = storeLocation,
            StoreName = storeName
        };

        List<GenericCertificate> foundCertificates = certificateRepository.Get(certificateIdentifier)
            .ToList();

        response.FindCertificateResult = new FindCertificateResult
        {
            StoreLocation = storeLocation,
            StoreName = storeName,
            CertificateName = request.SubjectName,
            CertificateCount = (int)foundCertificates?.Count
        };

        return foundCertificates;
    }

    private void ExportCertificates(IEnumerable<GenericCertificate> certificates)
    {
        int index = 0;

        foreach (GenericCertificate genericCertificate in certificates)
        {
            ExportCertificate(genericCertificate, index);
            index++;
        }
    }

    private void ExportCertificate(GenericCertificate certificate, int index)
    {
        string fileName = $"certificate-{index:00}.pem";

        ExportAsPemResult result = new()
        {
            Thumbprint = certificate.Thumbprint,
            FileName = fileName
        };

        try
        {
            PemDocument pemDocument = certificate.ExportAsPem();

            fileSystem.SaveFile(fileName, pemDocument);
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        response.ExportAsPemResults.Add(result);
    }
}