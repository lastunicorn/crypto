using System.Security.Cryptography.X509Certificates;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.CreateRootCertificate;

public class CreateRootCertificateRequest : IRequest
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string CertificateName { get; set; }

    public string FriendlyName { get; set; }
}