namespace DustInTheWind.Crypto.Application.Results;

public abstract class ResultBase
{
    public abstract string Title { get; }

    public string Subtitle { get; set; }
}