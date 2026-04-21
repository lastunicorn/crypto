using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Results;
using DustInTheWind.Crypto.Domain;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.ExportAsPfx;

internal class ExportAsPfxUseCase : IRequestHandler<ExportAsPfxRequest, ExportAsPfxResponse>
{
    private const StoreLocation DefaultStoreLocation = StoreLocation.CurrentUser;
    private const StoreName DefaultStoreName = StoreName.My;

    private readonly IFileSystem fileSystem;
    private readonly ILog log;
    private readonly ICertificateRepository certificateRepository;
    private ExportAsPfxResponse response;

    public ExportAsPfxUseCase(IFileSystem fileSystem, ILog log, ICertificateRepository certificateRepository)
    {
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public Task<ExportAsPfxResponse> Handle(ExportAsPfxRequest request, CancellationToken cancellationToken)
    {
        response = new ExportAsPfxResponse();

        IEnumerable<GenericCertificate> certificates = RetrieveCertificates(request);
        ExportCertificates(certificates, request.Password);

        return Task.FromResult(response);
    }

    private IEnumerable<GenericCertificate> RetrieveCertificates(ExportAsPfxRequest request)
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

    private void ExportCertificates(IEnumerable<GenericCertificate> certificates, string password)
    {
        int index = 0;

        foreach (GenericCertificate genericCertificate in certificates)
        {
            ExportCertificate(genericCertificate, password, index);
            index++;
        }
    }

    private void ExportCertificate(GenericCertificate certificate, string password, int index)
    {
        string fileName = $"certificate-{index:00}.pfx";

        ExportAsPfxResult result = new()
        {
            Thumbprint = certificate.Thumbprint,
            FileName = fileName
        };

        try
        {
            PfxDocument pfxDocument = certificate.ExportAsPfx(password);
            result.CertificateBytes = pfxDocument.CertificateBytes;

            fileSystem.SaveFile(fileName, pfxDocument);
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        response.ExportAsPfxResults.Add(result);
    }
}