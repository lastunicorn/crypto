using DustInTheWind.Crypto.Domain.PrivateKeyModel;

namespace DustInTheWind.Crypto.Application.Sections;

public class PrivateKeyFileInfo
{
    public string FilePath { get; set; }

    public string DirectoryPath { get; set; }

    public PrivateKeyLocationType LocationType { get; set; }

    public bool IsSuccessfullyRemoved { get; set; }

    public Exception RemoveError { get; set; }
}