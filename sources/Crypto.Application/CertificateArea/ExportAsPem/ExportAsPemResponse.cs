using DustInTheWind.Crypto.Application.Sections;

namespace DustInTheWind.Crypto.Application.CertificateArea.ExportAsPem;

public class ExportAsPemResponse
{
    public FindCertificateResult FindCertificateResult { get; set; }

    public List<ExportAsPemResult> SaveCertificateAsPemSections { get; } = new();
}