using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;

namespace DustInTheWind.Crypto.Adapters.CertificateAccess;

public class CertificateRepository : ICertificateRepository
{
    public IEnumerable<GenericCertificate> Get(CertificateIdentifier certificateIdentifier)
    {
        return ExecuteWithStore(certificateIdentifier.StoreName, certificateIdentifier.StoreLocation, store =>
        {
            store.Open(OpenFlags.ReadOnly);

            List<X509Certificate2> certificates = store.Certificates.Find(X509FindType.FindBySubjectName, certificateIdentifier.Name, false)
                .Where(c => c.GetNameInfo(X509NameType.SimpleName, false) == certificateIdentifier.Name)
                .ToList();

            return certificates
                .Select(x => new GenericCertificate(x, certificateIdentifier.StoreName, certificateIdentifier.StoreLocation));
        });
    }

    public IEnumerable<GenericCertificate> Get(string subjectName, StoreName storeName, StoreLocation storeLocation)
    {
        return ExecuteWithStore(storeName, storeLocation, store =>
        {
            store.Open(OpenFlags.ReadOnly);

            List<X509Certificate2> certificates = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, false)
                .Where(c => c.GetNameInfo(X509NameType.SimpleName, false) == subjectName)
                .ToList();

            return certificates
                .Select(x => new GenericCertificate(x, storeName, storeLocation));
        });
    }

    public void Add(GenericCertificate certificate)
    {
        ExecuteWithStore(certificate.StoreName, certificate.StoreLocation, store =>
        {
            store.Open(OpenFlags.MaxAllowed);
            store.Add(certificate.Value);
        });
    }

    public void Remove(GenericCertificate certificate)
    {
        StoreName storeName = certificate.StoreName;
        StoreLocation storeLocation = certificate.StoreLocation;

        ExecuteWithStore(storeName, storeLocation, store =>
        {
            store.Open(OpenFlags.MaxAllowed);
            store.Remove(certificate.Value);

            int indexInStore = store.Certificates.IndexOf(certificate.Value);

            if (indexInStore > -1)
            {
                string certificateName = certificate.GetName();
                throw new Exception($"Failed to remove certificate '{certificateName}' from store: '{storeLocation}\\{storeName}'.");
            }
        });
    }

    public bool IsInstalled(GenericCertificate certificate)
    {
        StoreName storeName = certificate.StoreName;
        StoreLocation storeLocation = certificate.StoreLocation;

        return ExecuteWithStore(storeName, storeLocation, store =>
        {
            store.Open(OpenFlags.ReadWrite);

            return store.Certificates.Contains(certificate.Value);
        });
    }

    private static T ExecuteWithStore<T>(StoreName storeName, StoreLocation storeLocation, Func<X509Store, T> action)
    {
        X509Store store = new(storeName, storeLocation);

        try
        {
            return action(store);
        }
        finally
        {
            store.Close();
        }
    }

    private static void ExecuteWithStore(StoreName storeName, StoreLocation storeLocation, Action<X509Store> action)
    {
        X509Store store = new(storeName, storeLocation);

        try
        {
            action(store);
        }
        finally
        {
            store.Close();
        }
    }
}