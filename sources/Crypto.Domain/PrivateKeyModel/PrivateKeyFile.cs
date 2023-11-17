using System.Security.Cryptography;

namespace DustInTheWind.Crypto.Domain.PrivateKeyModel;

public class PrivateKeyFile
{
    private static readonly string ApplicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static readonly string CommonApplicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
    private static readonly string WindowsPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

    private readonly RSA privateKey;

    public string DirectoryPath { get; private set; }

    public string FullPath { get; private set; }

    public PrivateKeyLocationType LocationType { get; private set; }

    public bool Exists { get; private set; }

    public PrivateKeyFile(RSA privateKey)
    {
        this.privateKey = privateKey ?? throw new ArgumentNullException(nameof(privateKey));

        FindKeyLocation();
    }

    private void FindKeyLocation()
    {
        string uniqueName = privateKey.GetUniqueName();

        IEnumerable<PrivateKeyLocationRoot> locationRoots = EnumerateCngLocations()
            .Concat(EnumerateCspLocations());

        foreach (PrivateKeyLocationRoot privateKeyLocationRoot in locationRoots)
        {
            string filePath = SearchInDirectory(uniqueName, privateKeyLocationRoot.DirectoryPath);

            if (filePath == null)
                continue;

            FullPath = filePath;
            DirectoryPath = privateKeyLocationRoot.DirectoryPath;
            LocationType = privateKeyLocationRoot.LocationType;
            Exists = true;

            break;
        }
    }

    /// <summary>
    /// https://learn.microsoft.com/en-us/windows/win32/seccng/key-storage-and-retrieval
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<PrivateKeyLocationRoot> EnumerateCngLocations()
    {
        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(ApplicationDataPath, "Microsoft", "Crypto", "Keys"),
            LocationType = PrivateKeyLocationType.User
        };
        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(CommonApplicationDataPath, "Application Data", "Microsoft", "Crypto", "SystemKeys"),
            LocationType = PrivateKeyLocationType.LocalSystem
        };
        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(WindowsPath, "ServiceProfiles", "LocalService"),
            LocationType = PrivateKeyLocationType.LocalService
        };
        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(WindowsPath, "ServiceProfiles", "NetworkService"),
            LocationType = PrivateKeyLocationType.NetworkService
        };
        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(CommonApplicationDataPath, "Application Data", "Microsoft", "Crypto", "Keys"),
            LocationType = PrivateKeyLocationType.Shared
        };
    }

    private static IEnumerable<PrivateKeyLocationRoot> EnumerateCspLocations()
    {
        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(ApplicationDataPath, "Microsoft", "Crypto", "RSA"),
            LocationType = PrivateKeyLocationType.User
        };
        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(ApplicationDataPath, "Microsoft", "Crypto", "DSS"),
            LocationType = PrivateKeyLocationType.User
        };

        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(CommonApplicationDataPath, "Application Data", "Microsoft", "Crypto", "RSA", "S-1-5-18"),
            LocationType = PrivateKeyLocationType.LocalSystem
        };
        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(CommonApplicationDataPath, "Application Data", "Microsoft", "Crypto", "DSS", "S-1-5-18"),
            LocationType = PrivateKeyLocationType.LocalSystem
        };

        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(CommonApplicationDataPath, "Application Data", "Microsoft", "Crypto", "RSA", "S-1-5-19"),
            LocationType = PrivateKeyLocationType.LocalService
        };
        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(CommonApplicationDataPath, "Application Data", "Microsoft", "Crypto", "DSS", "S-1-5-19"),
            LocationType = PrivateKeyLocationType.LocalService
        };

        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(CommonApplicationDataPath, "Application Data", "Microsoft", "Crypto", "RSA", "S-1-5-20"),
            LocationType = PrivateKeyLocationType.NetworkService
        };
        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(CommonApplicationDataPath, "Application Data", "Microsoft", "Crypto", "DSS", "S-1-5-20"),
            LocationType = PrivateKeyLocationType.NetworkService
        };

        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(CommonApplicationDataPath, "Application Data", "Microsoft", "Crypto", "RSA", "MachineKeys"),
            LocationType = PrivateKeyLocationType.Shared
        };
        yield return new PrivateKeyLocationRoot
        {
            DirectoryPath = Path.Combine(CommonApplicationDataPath, "Application Data", "Microsoft", "Crypto", "DSS", "MachineKeys"),
            LocationType = PrivateKeyLocationType.Shared
        };
    }

    private static string SearchInDirectory(string fileName, string directoryPath)
    {
        try
        {
            CryptoDirectory cryptoDirectory = new(directoryPath);
            return cryptoDirectory.FindFirst(fileName);
        }
        catch
        {
            return null;
        }
    }

    public bool Delete()
    {
        if (!File.Exists(FullPath))
            return false;

        File.Delete(FullPath);
        return true;
    }
}