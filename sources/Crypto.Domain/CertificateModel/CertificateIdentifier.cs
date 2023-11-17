using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public struct CertificateIdentifier
{
    public string Name { get; init; }

    public StoreLocation StoreLocation { get; init; }

    public StoreName StoreName { get; init; }
}