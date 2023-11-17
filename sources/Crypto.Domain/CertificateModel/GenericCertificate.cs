using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public class GenericCertificate
{
    public string SubjectName => Value.SubjectName.Name;

    public string FriendlyName => Value.FriendlyName;

    public int PublicKeyLength => Value.GetRSAPublicKey()?.KeySize ?? 0;

    public int PrivateKeyLength => Value.GetRSAPrivateKey()?.KeySize ?? 0;


    public StoreLocation StoreLocation { get; }

    public StoreName StoreName { get; }

    public string Thumbprint => Value.Thumbprint;

    public DateTime StartDate => Value.NotBefore;

    public DateTime EndDate => Value.NotAfter;

    public X509Certificate2 Value { get; protected set; }

    public GenericCertificate(X509Certificate2 certificate, StoreName storeName, StoreLocation storeLocation)
    {
        Value = certificate ?? throw new ArgumentNullException(nameof(certificate));

        StoreName = storeName;
        StoreLocation = storeLocation;
    }

    public PemDocument ExportAsPem()
    {
        return new PemDocument
        {
            CertificateBytes = Value.Export(X509ContentType.Cert)
        };
    }

    public PfxDocument ExportAsPfx(string password = null)
    {
        return new PfxDocument
        {
            CertificateBytes = Value.Export(X509ContentType.Pfx)
        };
    }

    public string GetName()
    {
        return Value?.GetNameInfo(X509NameType.SimpleName, false);
    }

    public RSA GetRsaPrivateKey()
    {
        return Value.GetRSAPrivateKey();
    }
}