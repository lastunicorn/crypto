using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Presentation.Controls;
using DustInTheWind.Crypto.Presentation.Infrastructure;

namespace DustInTheWind.Crypto.Presentation.Commands.CertificateArea.ExportAsPem;

internal class ExportAsPemView : CustomView<ExportAsPemViewModel>
{
    public override void Display(ExportAsPemViewModel viewModel)
    {
        DisplayFindCertificate(viewModel.FindCertificateResult);
        DisplayExportAsPem(viewModel.ExportAsPemSections);
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