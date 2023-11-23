using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class VerifyCertificateParentStep : StepBase
{
    public override string Title => "Verify Certificate Parent";

    public GenericCertificate ParentCertificate { get; set; }

    public GenericCertificate Certificate { get; set; }

    public VerifyCertificateParentStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        bool areCertificatesRelated = Certificate.IsChildOf(ParentCertificate);

        if (areCertificatesRelated)
            Console.WriteLine("Certificate is from current parent");
    }
}