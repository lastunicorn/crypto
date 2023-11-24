using DustInTheWind.Crypto.Application.Sections;

namespace DustInTheWind.Crypto.Application.CertificateArea.ExportAsPfx;

public class ExportAsPfxResult : ResultBase
{
    public override string Title => "Save Certificate in PFX format";

    public string Thumbprint { get; set; }

    public string FileName { get; set; }

    public byte[] CertificateBytes { get; set; }

    public Exception Error { get; set; }
}