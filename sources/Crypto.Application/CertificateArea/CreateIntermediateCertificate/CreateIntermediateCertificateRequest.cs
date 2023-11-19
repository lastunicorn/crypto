using System.Security.Cryptography.X509Certificates;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.CreateIntermediateCertificate;

public class CreateIntermediateCertificateRequest : IRequest
{
    public StoreLocation ParentStoreLocation { get; set; }

    public StoreName ParentStoreName { get; set; }

    public string ParentName { get; set; }

    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string CertificateName { get; set; }
}