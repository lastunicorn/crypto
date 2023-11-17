namespace DustInTheWind.Crypto.Domain.PrivateKeyModel;

internal class DirectorySearchingEventArgs : EventArgs
{
    public string DirectoryPath { get; init; }

    public string FileName { get; init; }
}