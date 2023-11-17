using DustInTheWind.Crypto.Domain;

namespace DustInTheWind.Crypto.Ports.FileAccess;

public interface IFileSystem
{
    void SaveFile(string filePath, PemDocument pemDocument);

    void SaveFile(string filePath, PfxDocument pfxDocument);

    bool DeleteFile(string filePath);
}