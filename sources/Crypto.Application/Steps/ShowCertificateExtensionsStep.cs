using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class ShowCertificateExtensionsStep : StepBase
{
    public override string Title => "Show Certificate Extensions";

    public GenericCertificate Certificate { get; set; }

    public ShowCertificateExtensionsStep(ILog log)
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

        DisplayExtensions(certificate);
    }

    private void DisplayExtensions(X509Certificate2 certificate)
    {
        for (int i = 0; i < certificate.Extensions.Count; i++)
        {
            if (i > 0)
                Log.WriteLine();

            X509Extension extension = certificate.Extensions[i];
            DisplayExtension($"Extension {i}", extension);
        }
    }

    private void DisplayExtension(string title, X509Extension extension)
    {
        Log.WithIndentation(title, () =>
        {
            Log.WriteValue("OID", extension.Oid.ToNiceString());
            Log.WriteValue("Is Critical", extension.Critical);
            Log.WriteValue("RawData", extension.RawData);
            Log.WriteValue("Formatted data", extension.Format(true));

            switch (extension)
            {
                case X509BasicConstraintsExtension basicConstraintsExtension:
                    DisplayExtensionSpecificValues(basicConstraintsExtension);
                    break;

                case X509SubjectKeyIdentifierExtension subjectKeyIdentifierExtension:
                    DisplayExtensionSpecificValues(subjectKeyIdentifierExtension);
                    break;

                case X509EnhancedKeyUsageExtension enhancedKeyUsageExtension:
                    DisplayExtensionSpecificValues(enhancedKeyUsageExtension);
                    break;

                case X509KeyUsageExtension keyUsageExtension:
                    DisplayExtensionSpecificValues(keyUsageExtension);
                    break;
            }
        });
    }

    private void DisplayExtensionSpecificValues(X509BasicConstraintsExtension basicConstraintsExtension)
    {
        Log.WriteValue("CertificateAuthority", basicConstraintsExtension.CertificateAuthority);
        Log.WriteValue("HasPathLengthConstraint", basicConstraintsExtension.HasPathLengthConstraint);
        Log.WriteValue("PathLengthConstraint", basicConstraintsExtension.PathLengthConstraint);
    }

    private void DisplayExtensionSpecificValues(X509SubjectKeyIdentifierExtension subjectKeyIdentifierExtension)
    {
        Log.WriteValue("SubjectKeyIdentifier", subjectKeyIdentifierExtension.SubjectKeyIdentifier);
    }

    private void DisplayExtensionSpecificValues(X509EnhancedKeyUsageExtension enhancedKeyUsageExtension)
    {
        Log.WithIndentation("Usages", () =>
        {
            foreach (Oid oid in enhancedKeyUsageExtension.EnhancedKeyUsages)
                Log.WriteValue("OID", oid.ToNiceString());
        });
    }

    private void DisplayExtensionSpecificValues(X509KeyUsageExtension keyUsageExtension)
    {
        Log.WriteValue("Usages", keyUsageExtension.KeyUsages);
    }
}