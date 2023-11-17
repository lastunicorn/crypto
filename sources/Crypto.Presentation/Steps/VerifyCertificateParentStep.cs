using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class VerifyCertificateParentStep : StepBase
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
        X509SubjectKeyIdentifierExtension subjectKeyIdentifier = ParentCertificate.Extensions.OfType<X509Extension>()
            .Where(x => x.Oid.FriendlyName == "Subject Key Identifier")
            .FirstOrDefault() as X509SubjectKeyIdentifierExtension;

        X509Extension aki = Certificate.Extensions
            .OfType<X509Extension>()
            .FirstOrDefault(x => x.Oid.Value == OidValues.AuthorityKeyIdentifier);

        if (aki == null)
        {
            return false;
        }

        string parentSki = subjectKeyIdentifier?.Format(false) ?? "";
        string certAki = aki.Format(false) ?? "";
        if (certAki.Contains(parentSki))
            Console.WriteLine("Certificate is from current parent");

        return true;
    }
}