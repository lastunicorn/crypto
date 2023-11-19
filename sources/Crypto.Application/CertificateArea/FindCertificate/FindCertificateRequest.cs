using System.Security.Cryptography.X509Certificates;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.FindCertificate;

public class FindCertificateRequest : IRequest
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string Name { get; set; }
}