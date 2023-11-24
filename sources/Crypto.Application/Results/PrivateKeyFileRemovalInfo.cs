using DustInTheWind.Crypto.Domain.PrivateKeyModel;

namespace DustInTheWind.Crypto.Application.Results;

public class PrivateKeyFileRemovalInfo
{
    public string FilePath { get; set; }

    public string DirectoryPath { get; set; }

    public PrivateKeyLocationType LocationType { get; set; }

    public bool IsSuccessfullyRemoved { get; set; }

    public Exception RemoveError { get; set; }
}