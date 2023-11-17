using DustInTheWind.Crypto.Domain;
using DustInTheWind.Crypto.Ports.FileAccess;

namespace DustInTheWind.Crypto.Adapters.FileAccess;

public class FileSystem : IFileSystem
{
    public void SaveFile(string filePath, PemDocument pemDocument)
    {
        string content = pemDocument.ToString();
        File.WriteAllText(filePath, content);
    }

    public void SaveFile(string filePath, PfxDocument pfxDocument)
    {
        byte[] bytes = pfxDocument.CertificateBytes;
        File.WriteAllBytes(filePath, bytes);
    }

    public bool DeleteFile(string filePath)
    {
        if (!File.Exists(filePath))
            return false;

        File.Delete(filePath);
        return true;
    }
}