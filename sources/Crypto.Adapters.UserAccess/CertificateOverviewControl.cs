using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Presentation.Infrastructure;

namespace DustInTheWind.Crypto.Adapters.UserAccess;

internal class CertificateOverviewControl : ControlBase
{
    public GenericCertificate Certificate { get; set; }
    
    public void Display()
    {
        if (Certificate == null) throw new ArgumentException(nameof(Certificate));

        X509Certificate2 certificate = Certificate.Value;

        WriteValue("Simple Name", certificate.GetNameInfo(X509NameType.SimpleName, false));
        WriteValue("Friendly Name", certificate.FriendlyName);

        WriteHorizontalLine(1);

        ShowOverview(certificate);
    }

    private void ShowOverview(X509Certificate2 certificate)
    {
        WithIndentation("Names", () =>
        {
            IDictionary<string, string> names = Enum.GetValues<X509NameType>()
                .ToDictionary(Enum.GetName, x => certificate.GetNameInfo(x, false));

            foreach ((string key, string value) in names)
                WriteValue(key, value);
        });

        WriteValue("Friendly Name", certificate.FriendlyName);

        WriteValue("Subject", certificate.Subject);
        WriteValue("SubjectName.Name", certificate.SubjectName.Name);
        WriteValue("SubjectName.Oid", certificate.SubjectName.Oid.ToNiceString());

        WriteValue("Has private key", certificate.HasPrivateKey);
        WriteValue("Issuer", certificate.Issuer);
        WriteValue("Version", certificate.Version);
        WriteValue("Valid Date", certificate.NotBefore);
        WriteValue("Expiry Date", certificate.NotAfter);
        WriteValue("Thumbprint", certificate.Thumbprint);
        WriteValue("Serial Number", certificate.SerialNumber);
        WriteValue("Archived", certificate.Archived);
        WriteValue("SignatureAlgorithm OID", certificate.SignatureAlgorithm.ToNiceString());
        WriteValue("Raw Data Length", certificate.RawData.Length);

        WriteHorizontalLine(1);

        WriteValue("To String", certificate.ToString(true));
    }
}