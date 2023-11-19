using System.Security.Cryptography.X509Certificates;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.RemoveCertificate;

public class RemoveCertificateRequest : IRequest
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string Name { get; set; }
}