using DustInTheWind.Crypto.Domain.CertificateModel;
using MediatR;

namespace DustInTheWind.Crypto.Application.AlezArea.ExportAlezAsPem;

public class ExportAlezAsPemRequest : IRequest
{
    public CertificateType CertificateType { get; set; } = CertificateType.All;
}