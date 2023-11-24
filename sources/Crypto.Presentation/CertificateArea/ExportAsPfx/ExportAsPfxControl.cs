using DustInTheWind.Crypto.Application.CertificateArea.ExportAsPfx;
using DustInTheWind.Crypto.Presentation.Infrastructure;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.ExportAsPfx;

public class ExportAsPfxControl : SectionControlBase
{
    public string Title { get; set; }

    public string Thumbprint { get; set; }

    public string FileName { get; set; }

    public byte[] CertificateBytes { get; set; }

    public Exception Error { get; set; }

    public ExportAsPfxControl(ExportAsPfxResult result)
    {
        Title = result.Title;
        Thumbprint = result.Thumbprint;
        FileName = result.FileName;
        CertificateBytes = result.CertificateBytes;
        Error = result.Error;
    }

    protected override void DoDisplay()
    {
        WriteValue("Certificate Thumbprint", Thumbprint);
        WriteValue("File", FileName);
        WriteLine();

        WriteValue("Certificate", CertificateBytes);

        if (Error == null)
            WriteSuccess("Certificate exported successfully.");
        else
            WriteError(Error);
    }
}