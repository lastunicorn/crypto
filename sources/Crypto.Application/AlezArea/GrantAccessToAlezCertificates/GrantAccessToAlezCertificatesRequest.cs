using DustInTheWind.Crypto.Domain.CertificateModel;
using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.GrantAccessToAlezCertificates;

public class GrantAccessToAlezCertificatesRequest : IRequest<GrantAccessToAlezCertificatesResponse>
{
    public CertificateType CertificateType { get; set; } = CertificateType.All;
    
    public string Filter { get; set; }

}