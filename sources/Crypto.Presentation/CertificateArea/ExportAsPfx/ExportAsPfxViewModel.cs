using DustInTheWind.Crypto.Application.CertificateArea.ExportAsPfx;
using DustInTheWind.Crypto.Application.Sections;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.ExportAsPfx;

internal class ExportAsPfxViewModel
{
    public FindCertificateResult FindCertificateResult { get; set; }

    public List<ExportAsPfxResult> ExportAsPfxResults { get; set; }
}