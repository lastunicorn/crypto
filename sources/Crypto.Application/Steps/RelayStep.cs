using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

internal class RelayStep : StepBase
{
    public override string Title { get; }

    public Action Action { get; set; }

    public RelayStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        Action?.Invoke();
    }
}