namespace DustInTheWind.Crypto.Domain.PrivateKeyModel;

internal class CryptoDirectory
{
    private readonly string rootPath;

    public event EventHandler<DirectorySearchingEventArgs> DirectorySearching;

    public event EventHandler<DirectorySearchedEventArgs> DirectorySearched;

    public CryptoDirectory(string rootPath)
    {
        this.rootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
    }

    public string FindFirst(string fileName)
    {
        string filePath = SearchSubDirectory(rootPath, fileName);

        if (filePath != null)
            return filePath;

        string[] subDirectoryPaths = Directory.GetDirectories(rootPath);

        if (subDirectoryPaths.Length <= 0)
            return null;

        return subDirectoryPaths
            .Select(x => SearchSubDirectory(x, fileName))
            .FirstOrDefault(x => x != null);
    }

    private string SearchSubDirectory(string subDirectoryPath, string fileName)
    {
        OnDirectorySearching(new DirectorySearchingEventArgs
        {
            DirectoryPath = subDirectoryPath,
            FileName = fileName
        });

        try
        {
            string[] filePaths = Directory.GetFiles(subDirectoryPath, fileName);

            if (filePaths.Length > 0)
            {
                OnDirectorySearched(new DirectorySearchedEventArgs
                {
                    DirectoryPath = subDirectoryPath,
                    FileName = fileName,
                    Found = true
                });

                return filePaths[0];
            }

            OnDirectorySearched(new DirectorySearchedEventArgs
            {
                DirectoryPath = subDirectoryPath,
                FileName = fileName,
                Found = false
            });
        }
        catch (Exception ex)
        {
            OnDirectorySearched(new DirectorySearchedEventArgs
            {
                DirectoryPath = subDirectoryPath,
                FileName = fileName,
                Found = false,
                Exception = ex
            });
        }

        return null;
    }

    protected virtual void OnDirectorySearching(DirectorySearchingEventArgs e)
    {
        DirectorySearching?.Invoke(this, e);
    }

    protected virtual void OnDirectorySearched(DirectorySearchedEventArgs e)
    {
        DirectorySearched?.Invoke(this, e);
    }
}