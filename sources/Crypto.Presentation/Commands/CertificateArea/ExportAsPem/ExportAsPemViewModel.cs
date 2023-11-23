using DustInTheWind.Crypto.Application.Sections;

namespace DustInTheWind.Crypto.Presentation.Commands.CertificateArea.ExportAsPem;

internal class ExportAsPemViewModel
{
    public FindCertificateResult FindCertificateResult { get; set; }

    public List<ExportAsPemResult> ExportAsPemSections { get; set; }
}