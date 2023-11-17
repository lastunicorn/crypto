using System.Text;

namespace DustInTheWind.Crypto.Domain;

public class PemDocument
{
    private const string HeaderType = "CERTIFICATE";

    public byte[] CertificateBytes { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        AddHeader(stringBuilder);
        AddContent(stringBuilder);
        AddFooter(stringBuilder);

        return stringBuilder.ToString();
    }

    private static void AddHeader(StringBuilder stringBuilder)
    {
        const string headerText = $"-----BEGIN {HeaderType}-----";
        stringBuilder.AppendLine(headerText);
    }

    private void AddContent(StringBuilder stringBuilder)
    {
        string base64Content = Convert.ToBase64String(CertificateBytes);

        for (int i = 0; i < base64Content.Length; i += 64)
        {
            int lineLength = Math.Min(64, base64Content.Length - i);
            string line = base64Content.Substring(i, lineLength);
            stringBuilder.AppendLine(line);
        }
    }

    private static void AddFooter(StringBuilder stringBuilder)
    {
        const string footerText = $"-----END {HeaderType}-----";
        stringBuilder.AppendLine(footerText);
    }
}