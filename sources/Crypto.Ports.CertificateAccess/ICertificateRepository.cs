using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;

namespace DustInTheWind.Crypto.Ports.CertificateAccess;

public interface ICertificateRepository
{
    IEnumerable<GenericCertificate> Get(CertificateIdentifier certificateIdentifier);

    IEnumerable<GenericCertificate> Get(string subjectName, StoreName storeName, StoreLocation storeLocation);

    void Add(GenericCertificate certificate, StoreName storeName, StoreLocation storeLocation);

    void Remove(GenericCertificate certificate);

    bool IsInstalled(GenericCertificate certificate);
}