using DustInTheWind.Crypto.Application.Sections;

namespace DustInTheWind.Crypto.Application.CertificateArea.ExportAsPfx;

public class ExportAsPfxResponse
{
    public FindCertificateResult FindCertificateResult { get; set; }

    public List<ExportAsPfxResult> ExportAsPfxResults { get; } = new();
}