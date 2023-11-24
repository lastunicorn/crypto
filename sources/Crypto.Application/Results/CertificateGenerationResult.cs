using DustInTheWind.Crypto.Domain.CertificateModel;
using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Application.Results;

public class CertificateGenerationResult
{
    public string SubjectName { get; set; }

    public string FriendlyName { get; set; }

    public TimeSpan Validity { get; set; }

    public int PublicKeyLength { get; set; }
    
    public int PrivateKeyLength { get; set; }

    public PersistenceType PrivateKeyPersistence { get; set; }

    public bool AllowExport { get; set; }

    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public Exception Error { get; set; }
}