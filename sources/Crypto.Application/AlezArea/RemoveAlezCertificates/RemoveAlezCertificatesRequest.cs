using DustInTheWind.Crypto.Domain.CertificateModel;
using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.RemoveAlezCertificates;

public class RemoveAlezCertificatesRequest : IRequest<RemoveAlezCertificatesResponse>
{
    public CertificateType CertificateType { get; set; } = CertificateType.All;
}