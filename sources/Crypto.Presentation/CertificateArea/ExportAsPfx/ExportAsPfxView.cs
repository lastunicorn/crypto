using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.CertificateArea.ExportAsPfx;
using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Presentation.Controls;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.ExportAsPfx;

internal class ExportAsPfxView : ViewBase<ExportAsPfxViewModel>
{
    public override void Display(ExportAsPfxViewModel viewModel)
    {
        DisplayFindCertificate(viewModel.FindCertificateResult);
        DisplayExportAsPem(viewModel.ExportAsPfxResults);
    }

    private static void DisplayFindCertificate(FindCertificateResult findCertificateResult)
    {
        FindCertificateControl findCertificateControl = new(findCertificateResult);
        findCertificateControl.Display();
    }

    private static void DisplayExportAsPem(IEnumerable<ExportAsPfxResult> exportAsPfxResults)
    {
        foreach (ExportAsPfxResult exportAsPfxResult in exportAsPfxResults)
            Display(exportAsPfxResult);
    }

    private static void Display(ExportAsPfxResult exportAsPfxResult)
    {
        ExportAsPfxControl exportAsPfxControl = new(exportAsPfxResult);
        exportAsPfxControl.Display();
    }
}