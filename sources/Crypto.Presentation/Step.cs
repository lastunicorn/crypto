using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases;

internal class Step
{
    private readonly ILog log;
    private string title;
    private string subtitle;

    public static bool FakeIt { get; set; }

    private Step(ILog log)
    {
        this.log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public static Step Create(ILog log)
    {
        return new Step(log);
    }

    public Step WithTitle(string title, string subtitle = null)
    {
        this.title = title;
        this.subtitle = subtitle;

        return this;
    }

    public void Execute(Action<ILog> action)
    {
        string fullTitle = subtitle == null
            ? title
            : $"{title} ({subtitle})";

        log.WriteTitle(fullTitle);

        if (!FakeIt)
        {
            try
            {
                action?.Invoke(log);
            }
            catch (Exception ex)
            {
                log.WriteError(ex);
            }
        }

        log.WriteLine();
    }
}