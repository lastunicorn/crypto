using DustInTheWind.Crypto.Application.CertificateArea.ExportAsPem;
using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Presentation.CommonControls;
using DustInTheWind.Crypto.Presentation.Infrastructure;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.ExportAsPem;

internal class ExportAsPemView : CustomView<ExportAsPemViewModel>
{
    public override void Display(ExportAsPemViewModel viewModel)
    {
        DisplayFindCertificate(viewModel.FindCertificateResult);
        DisplayExportAsPem(viewModel.ExportAsPemResults);
    }

    private static void DisplayFindCertificate(FindCertificateResult findCertificateResult)
    {
        FindCertificateControl findCertificateControl = new(findCertificateResult);
        findCertificateControl.Display();
    }

    private static void DisplayExportAsPem(IEnumerable<ExportAsPemResult> exportAsPemResults)
    {
        foreach (ExportAsPemResult exportAsPemResult in exportAsPemResults)
            Display(exportAsPemResult);
    }

    private static void Display(ExportAsPemResult exportAsPemResult)
    {
        ExportAsPemControl exportAsPemControl = new(exportAsPemResult);
        exportAsPemControl.Display();
    }
}