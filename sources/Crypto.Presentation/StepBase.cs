using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases;

internal abstract class StepBase
{
    protected ILog Log { get; }

    public static bool FakeIt { get; set; }

    public abstract string Title { get; }

    public string Subtitle { get; set; }

    protected StepBase(ILog log)
    {
        Log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public void Execute()
    {
        string fullTitle = Subtitle == null
            ? Title
            : $"{Title} ({Subtitle})";

        Log.WriteTitle(fullTitle);

        if (!FakeIt)
        {
            try
            {
                DoExecute();
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
        }

        Log.WriteLine();
    }

    protected abstract void DoExecute();
}