using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class VerifyCertificateParentStep : StepBase
{
    public override string Title => "Verify Certificate Parent";

    public X509Certificate2 ParentCertificate { get; set; }

    public X509Certificate2 Certificate { get; set; }

    public VerifyCertificateParentStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        IsCertificateFromCurrentParent();
    }

    private bool IsCertificateFromCurrentParent()
    {
        X509SubjectKeyIdentifierExtension subjectKeyIdentifier = GetSki();
        X509Extension aki = GetAki();

        if (aki == null)
            return false;

        string parentSki = subjectKeyIdentifier?.Format(false) ?? string.Empty;
        string certAki = aki.Format(false) ?? string.Empty;

        bool areCertificatesRelated = certAki.Contains(parentSki);
        
        if (areCertificatesRelated)
            Console.WriteLine("Certificate is from current parent");

        return areCertificatesRelated;
    }

    private X509SubjectKeyIdentifierExtension GetSki()
    {
        X509Extension x509Extension = ParentCertificate.Extensions
            .FirstOrDefault(x => x.Oid?.FriendlyName == "Subject Key Identifier");

        return x509Extension as X509SubjectKeyIdentifierExtension;
    }

    private X509Extension GetAki()
    {
        return Certificate.Extensions
            .FirstOrDefault(x => x.Oid?.Value == OidValues.AuthorityKeyIdentifier);
    }
}