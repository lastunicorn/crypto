using DustInTheWind.Crypto.Application.Results;

namespace DustInTheWind.Crypto.Application.CertificateArea.ExportAsPem;

public class ExportAsPemResponse
{
    public FindCertificateResult FindCertificateResult { get; set; }

    public List<ExportAsPemResult> ExportAsPemResults { get; } = new();
}