namespace DustInTheWind.Crypto.Domain.PrivateKeyModel;

internal struct PrivateKeyLocationRoot
{
    public string DirectoryPath { get; init; }

    public PrivateKeyLocationType LocationType { get; init; }
}