using System.Security.Cryptography.X509Certificates;
using MediatR;

namespace DustInTheWind.Crypto.Application.ShowCertificate;

public class ShowCertificateRequest : IRequest
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string SubjectName { get; set; }

    public CertificateDetailsType Details { get; set; }
}