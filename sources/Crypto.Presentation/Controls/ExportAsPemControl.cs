using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Presentation.Infrastructure;

namespace DustInTheWind.Crypto.Presentation.Controls;

public class ExportAsPemControl : SectionControlBase
{
    public string Title { get; set; }

    public string Thumbprint { get; set; }

    public string FileName { get; set; }

    public Exception Error { get; set; }

    public ExportAsPemControl(ExportAsPemResult result)
    {
        Title = result.Title;
        Thumbprint = result.Thumbprint;
        FileName = result.FileName;
        Error = result.Error;
    }

    protected override void DoDisplay()
    {
        WriteValue("Certificate Thumbprint", Thumbprint);
        WriteValue("File", FileName);
        WriteLine();

        if (Error == null)
            WriteSuccess("Certificate exported successfully.");
        else
            WriteError(Error);
    }
}