using DustInTheWind.Crypto.Application.CertificateArea.ExportAsPfx;
using DustInTheWind.Crypto.Application.Results;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.ExportAsPfx;

internal class ExportAsPfxViewModel
{
    public FindCertificateResult FindCertificateResult { get; set; }

    public List<ExportAsPfxResult> ExportAsPfxResults { get; set; }
}