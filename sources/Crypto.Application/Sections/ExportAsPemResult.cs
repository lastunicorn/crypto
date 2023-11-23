namespace DustInTheWind.Crypto.Application.Sections;

public class ExportAsPemResult : ResultBase
{
    public override string Title => "Save Certificate in PEM format";

    public string Thumbprint { get; set; }

    public string FileName { get; set; }

    public Exception Error { get; set; }
}