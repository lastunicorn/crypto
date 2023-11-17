using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class GenerateAlezNormalCertificateStep : StepBase
{
    public override string Title => "Generate Alez Normal Certificate";

    public GenericCertificate ParentCertificate { get; set; }

    public GenericCertificate GeneratedCertificate { get; private set; }

    public GenerateAlezNormalCertificateStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        AlezNormalCertificateBuilder builder = new()
        {
            ParentCertificate = ParentCertificate
        };
        GeneratedCertificate = builder.Build();

        Log.WriteSuccess("Certificate Generated Successfully");
    }
}