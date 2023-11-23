using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.Ports.UserAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class ShowCertificateOverviewStep : StepBase
{
    private readonly IUserInterface userInterface;

    public override string Title => "Show Certificate Overview";

    public GenericCertificate Certificate { get; set; }

    public ShowCertificateOverviewStep(ILog log, IUserInterface userInterface)
        : base(log)
    {
        this.userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
    }

    protected override void DoExecute()
    {
        if (Certificate == null) throw new ArgumentException(nameof(Certificate));

        userInterface.DisplayOverview(Certificate);
    }
}