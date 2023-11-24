using DustInTheWind.Crypto.Domain.CertificateModel;
using MediatR;

namespace DustInTheWind.Crypto.Application.WatersArea.ShowWatersCertificates;

public class ShowWatersCertificatesRequest : IRequest<ShowWatersCertificatesResponse>
{
    public CertificateType CertificateType { get; set; } = CertificateType.All;
    
    public string Filter { get; set; }
    
    public CertificateDetailsType Details { get; set; }

}