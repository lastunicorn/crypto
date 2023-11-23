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

    public X509SubjectKeyIdentifierExtension GetSki()
    {
        X509Extension x509Extension = Value?.Extensions
            .FirstOrDefault(x => x.Oid?.FriendlyName == "Subject Key Identifier");

        return x509Extension as X509SubjectKeyIdentifierExtension;
    }

    public X509Extension GetAki()
    {
        return Value?.Extensions
            .FirstOrDefault(x => x.Oid?.Value == OidValues.AuthorityKeyIdentifier);
    }

    public bool IsChildOf(GenericCertificate parentCert)
    {
        X509SubjectKeyIdentifierExtension subjectKeyIdentifier = parentCert.GetSki();
        X509Extension aki = GetAki();

        if (aki == null)
            return false;

        string parentSki = subjectKeyIdentifier?.Format(false) ?? string.Empty;
        string certAki = aki.Format(false) ?? string.Empty;

        return certAki.Contains(parentSki);
    }

    public IEnumerable<X509Extension> EnumerateExtensions()
    {
        return Value?.Extensions;
    }
}