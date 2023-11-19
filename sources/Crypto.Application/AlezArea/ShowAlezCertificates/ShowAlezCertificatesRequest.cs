using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.ShowAlezCertificates;

public class ShowAlezCertificatesRequest : IRequest
{
    public CertificateType CertificateType { get; set; } = CertificateType.All;

    public string Subject { get; set; }

    public CertificateDetailsType Details { get; set; }
}