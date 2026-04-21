using DustInTheWind.Crypto.Application.Results;

namespace DustInTheWind.Crypto.Application.CertificateArea.ExportAsPem;

public class ExportAsPemResult : ResultBase
{
    public override string Title => "Save Certificate in PEM format";

    public string Thumbprint { get; set; }

    public string FileName { get; set; }

    public Exception Error { get; set; }
}