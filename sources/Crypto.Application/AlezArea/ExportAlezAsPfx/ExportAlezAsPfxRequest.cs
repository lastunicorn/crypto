using DustInTheWind.Crypto.Domain.CertificateModel;
using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.ExportAlezAsPfx;

public class ExportAlezAsPfxRequest : IRequest
{
    public CertificateType CertificateType { get; set; } = CertificateType.All;
}