using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public abstract class CertificateBuilderBase
{
    public string SubjectName { get; set; }

    public string FriendlyName { get; set; }

    public TimeSpan Validity { get; set; } = TimeSpan.FromDays(365);

    public int PublicKeyLength { get; set; } = 4096;

    public int PrivateKeyLength { get; set; } = 4096;

    public bool AllowPrivateKeyExport { get; set; }

    public bool IsPrivateKeyEphemeral { get; set; }

    public PersistenceType PrivateKeyPersistence { get; set; } = PersistenceType.None;

    public OidCollection KeyUsage { get; set; }

    public X509KeyUsageFlags KeyUsageFlags { get; set; }

    public StoreName StoreName { get; set; } = StoreName.My;

    public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

    public GenericCertificate Build()
    {
        X509Certificate2 certificate = BuildX509Certificate();
        return new GenericCertificate(certificate, StoreName, StoreLocation);
    }

    protected abstract X509Certificate2 BuildX509Certificate();

    protected X509Certificate2 AddKeyStorageFlags(X509Certificate2 certificate)
    {
        X509KeyStorageFlags storageFlags = CreateKeyStorageFlags();

        if (storageFlags == 0)
            return certificate;

        string password = string.Empty;
        byte[] certificateBytes = certificate.Export(X509ContentType.Pkcs12, password);
        certificate.Dispose();

        return new X509Certificate2(certificateBytes, password, storageFlags);
    }

    private X509KeyStorageFlags CreateKeyStorageFlags()
    {
        X509KeyStorageFlags storageFlags = 0;

        switch (PrivateKeyPersistence)
        {
            case PersistenceType.UserLevel:
                storageFlags |= X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.UserKeySet;
                break;

            case PersistenceType.MachineLevel:
                storageFlags |= X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet;
                break;
        }

        if (AllowPrivateKeyExport)
            storageFlags |= X509KeyStorageFlags.Exportable;

        if (IsPrivateKeyEphemeral)
            storageFlags |= X509KeyStorageFlags.EphemeralKeySet;

        return storageFlags;
    }

    protected X509Certificate2 AddPrivateKey(X509Certificate2 certificate, RSA publicKey)
    {
        RSA privateKey = GeneratePrivateKey(publicKey);

        X509Certificate2 certificateWithPrivateKey = certificate.CopyWithPrivateKey(privateKey);
        certificate.Dispose();
        return certificateWithPrivateKey;
    }

    private RSA GeneratePrivateKey(RSA publicKey)
    {
        RSA privateKey = RSA.Create();
        privateKey.KeySize = PrivateKeyLength;

        RSAParameters publicKeyParameters = publicKey.ExportParameters(true);
        privateKey.ImportParameters(publicKeyParameters);

        return privateKey;
    }
}