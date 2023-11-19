using System.Security.Cryptography.X509Certificates;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.ExportAsPfx;

public class ExportAsPfxRequest : IRequest
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string SubjectName { get; set; }
}