using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public class AlezIntermediateCertificateBuilder : IntermediateCertificateBuilder
{
    public AlezIntermediateCertificateBuilder()
    {
        ParentCertificate = ParentCertificate;
        SubjectName = "CN=Alez Intermediate CA,O=Dust in the Wind,OU=Informatics,L=Brașov,C=RO";
        FriendlyName = "Alez Intermediate Certificate Authority";
        PublicKeyLength = 4096;
        PrivateKeyLength = 4096;
        Validity = TimeSpan.FromDays(36500);
        PrivateKeyPersistence = PersistenceType.MachineLevel;
        AllowPrivateKeyExport = false;
        IsPrivateKeyEphemeral = false;
        StoreName = StoreName.CertificateAuthority;
        StoreLocation = StoreLocation.CurrentUser;
    }
}