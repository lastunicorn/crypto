using DustInTheWind.Crypto.Application.CertificateArea.ExportAsPem;
using DustInTheWind.Crypto.Application.Sections;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.ExportAsPem;

internal class ExportAsPemViewModel
{
    public FindCertificateResult FindCertificateResult { get; set; }

    public List<ExportAsPemResult> ExportAsPemResults { get; set; }
}