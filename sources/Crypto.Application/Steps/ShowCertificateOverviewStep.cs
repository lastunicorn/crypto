using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class ShowCertificateOverviewStep : StepBase
{
    public override string Title => "Show Certificate Overview";

    public GenericCertificate Certificate { get; set; }

    public ShowCertificateOverviewStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        if (Certificate == null) throw new ArgumentException(nameof(Certificate));

        X509Certificate2 certificate = Certificate.Value;

        Log.WriteValue("Simple Name", certificate.GetNameInfo(X509NameType.SimpleName, false));
        Log.WriteValue("Friendly Name", certificate.FriendlyName);

        Log.WriteHorizontalLine(1);

        ShowOverview(certificate);
    }

    private void ShowOverview(X509Certificate2 certificate)
    {
        Log.WithIndentation("Names", () =>
        {
            IDictionary<string, string> names = Enum.GetValues<X509NameType>()
                .ToDictionary(Enum.GetName, x => certificate.GetNameInfo(x, false));

            foreach ((string key, string value) in names)
                Log.WriteValue(key, value);
        });

        Log.WriteValue("Friendly Name", certificate.FriendlyName);

        Log.WriteValue("Subject", certificate.Subject);
        Log.WriteValue("SubjectName.Name", certificate.SubjectName.Name);
        Log.WriteValue("SubjectName.Oid", certificate.SubjectName.Oid.ToNiceString());

        Log.WriteValue("Has private key", certificate.HasPrivateKey);
        Log.WriteValue("Issuer", certificate.Issuer);
        Log.WriteValue("Version", certificate.Version);
        Log.WriteValue("Valid Date", certificate.NotBefore);
        Log.WriteValue("Expiry Date", certificate.NotAfter);
        Log.WriteValue("Thumbprint", certificate.Thumbprint);
        Log.WriteValue("Serial Number", certificate.SerialNumber);
        Log.WriteValue("Archived", certificate.Archived);
        Log.WriteValue("SignatureAlgorithm OID", certificate.SignatureAlgorithm.ToNiceString());
        Log.WriteValue("Raw Data Length", certificate.RawData.Length);

        Log.WriteHorizontalLine(1);

        Log.WriteValue("To String", certificate.ToString(true));
    }
}