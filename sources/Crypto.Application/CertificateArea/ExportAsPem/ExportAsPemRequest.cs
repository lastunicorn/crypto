using System.Security.Cryptography.X509Certificates;
using MediatR;

namespace DustInTheWind.Crypto.Application.CertificateArea.ExportAsPem;

public class ExportAsPemRequest : IRequest<ExportAsPemResponse>
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string SubjectName { get; set; }
}