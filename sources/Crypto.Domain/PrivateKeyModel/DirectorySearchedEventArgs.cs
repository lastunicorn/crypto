namespace DustInTheWind.Crypto.Domain.PrivateKeyModel;

internal class DirectorySearchedEventArgs : EventArgs
{
    public string DirectoryPath { get; init; }

    public string FileName { get; init; }

    public bool Found { get; init; }

    public Exception Exception { get; init; }
}