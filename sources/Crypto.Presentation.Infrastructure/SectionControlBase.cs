namespace DustInTheWind.Crypto.Presentation.Infrastructure;

public abstract class SectionControlBase : ControlBase
{
    public string Title { get; set; }
    
    public string Subtitle { get; set; }

    public void Display()
    {
        string fullTitle = Subtitle == null
            ? Title
            : $"{Title} ({Subtitle})";

        WriteTitle(fullTitle);

        try
        {
            DoDisplay();
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }
    }

    protected abstract void DoDisplay();
}