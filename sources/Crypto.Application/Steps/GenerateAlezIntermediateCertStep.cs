using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

internal class GenerateAlezIntermediateCertStep : StepBase
{
    public override string Title => "Generate Alez Intermediate Certificate";

    public GenericCertificate ParentCertificate { get; set; }

    public GenericCertificate GeneratedCertificate { get; private set; }

    public GenerateAlezIntermediateCertStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        AlezIntermediateCertificateBuilder builder = new()
        {
            ParentCertificate = ParentCertificate
        };
        GeneratedCertificate = builder.Build();
        
        Log.WriteSuccess("Certificate Generated Successfully");
    }
}