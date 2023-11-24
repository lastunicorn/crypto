using DustInTheWind.Crypto.Domain.CertificateModel;
using MediatR;

namespace DustInTheWind.Crypto.Application.WatersArea.GrantAccessToWatersCertificates;

public class GrantAccessToWatersCertificatesRequest : IRequest<GrantAccessToWatersCertificatesResponse>
{
    public CertificateType CertificateType { get; set; }

    public string Filter { get; set; }

}