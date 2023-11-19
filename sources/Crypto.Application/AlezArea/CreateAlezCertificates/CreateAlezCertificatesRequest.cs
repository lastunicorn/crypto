using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.CreateAlezCertificates;

public class CreateAlezCertificatesRequest : IRequest
{
    public CertificateType CertificateType { get; set; } = CertificateType.All;
}