using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public class AlezCertificateIdentifiers : IEnumerable<CertificateIdentifier>
{
    public static CertificateIdentifier Root { get; } = new()
    {
        Name = "Alez Root CA",
        StoreLocation = StoreLocation.CurrentUser,
        StoreName = StoreName.Root
    };

    public static CertificateIdentifier Intermediate { get; } = new()
    {
        Name = "Alez Intermediate CA",
        StoreLocation = StoreLocation.CurrentUser,
        StoreName = StoreName.CertificateAuthority
    };

    public static CertificateIdentifier Normal { get; } = new()
    {
        Name = "Alez Normal Certificate",
        StoreLocation = StoreLocation.CurrentUser,
        StoreName = StoreName.My
    };

    public IEnumerable<CertificateIdentifier> Enumerate(CertificateType certificateType)
    {
        if (certificateType is CertificateType.All or CertificateType.Normal)
            yield return Normal;

        if (certificateType is CertificateType.All or CertificateType.Intermediate)
            yield return Intermediate;

        if (certificateType is CertificateType.All or CertificateType.Root)
            yield return Root;
    }

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