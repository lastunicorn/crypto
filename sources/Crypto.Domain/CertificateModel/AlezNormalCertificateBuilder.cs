using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public class AlezNormalCertificateBuilder : NormalCertificateBuilder
{
    public AlezNormalCertificateBuilder()
    {
        SubjectName = "CN=Alez Normal Certificate,O=Dust in the Wind,OU=Informatics,L=Brașov,C=RO";
        FriendlyName = "Alez Normal Certificate";
        PublicKeyLength = 4096;
        PrivateKeyLength = 4096;
        Validity = TimeSpan.FromDays(36500);
        PrivateKeyPersistence = PersistenceType.MachineLevel;
        AllowPrivateKeyExport = false;
        IsPrivateKeyEphemeral = false;
        StoreName = StoreName.My;
        StoreLocation = StoreLocation.CurrentUser;
    }
}