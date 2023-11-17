using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class GenerateAlezRootCertificateStep : StepBase
{
    public override string Title => "Generate Alez Root Certificate";

    public GenericCertificate GeneratedCertificate { get; private set; }

    public GenerateAlezRootCertificateStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        AlezRootCertificateBuilder builder = new();
        GeneratedCertificate = builder.Build();

        Log.WriteSuccess("Certificate Generated Successfully");
    }
}