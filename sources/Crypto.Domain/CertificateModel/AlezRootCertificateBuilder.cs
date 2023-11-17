using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public class AlezRootCertificateBuilder : RootCertificateBuilder
{
    public AlezRootCertificateBuilder()
    {
        SubjectName = "CN=Alez Root CA,O=Dust in the Wind,OU=Informatics,L=Romania,C=US";
        FriendlyName = "Alez Root Certificate Authority";
        PublicKeyLength = 4096;
        PrivateKeyLength = 4096;
        Validity = TimeSpan.FromDays(36500);
        PrivateKeyPersistence = PersistenceType.MachineLevel;
        AllowPrivateKeyExport = false;
        IsPrivateKeyEphemeral = false;
        StoreName = StoreName.Root;
        StoreLocation = StoreLocation.CurrentUser;
    }
}