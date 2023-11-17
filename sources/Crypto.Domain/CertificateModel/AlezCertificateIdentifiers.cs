using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public class AlezCertificateIdentifiers : IEnumerable<CertificateIdentifier>
{
    public CertificateIdentifier Root { get; } = new()
    {
        Name = "Alez Root CA",
        StoreLocation = StoreLocation.CurrentUser,
        StoreName = StoreName.Root
    };

    public CertificateIdentifier Intermediate { get; } = new()
    {
        Name = "Alez Intermediate CA",
        StoreLocation = StoreLocation.CurrentUser,
        StoreName = StoreName.CertificateAuthority
    };

    public CertificateIdentifier Normal { get; } = new()
    {
        Name = "Alez Normal Certificate",
        StoreLocation = StoreLocation.CurrentUser,
        StoreName = StoreName.My
    };

    public IEnumerator<CertificateIdentifier> GetEnumerator()
    {
        yield return Root;
        yield return Intermediate;
        yield return Normal;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}