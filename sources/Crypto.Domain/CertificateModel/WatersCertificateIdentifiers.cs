using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public class WatersCertificateIdentifiers : IEnumerable<CertificateIdentifier>
{
    private readonly CertificateIdentifier[] certificateIdentifiers =
    {
        new()
        {
            Name = "Waters Message Queue Root CA",
            StoreLocation = StoreLocation.LocalMachine,
            StoreName = StoreName.Root
        },
        new()
        {
            Name = "waters_connect Root CA",
            StoreLocation = StoreLocation.LocalMachine,
            StoreName = StoreName.Root
        },
        new()
        {
            Name = "waters_connect Service CA",
            StoreLocation = StoreLocation.LocalMachine,
            StoreName = StoreName.CertificateAuthority
        },
        new()
        {
            Name = "unifi_mq_user",
            StoreLocation = StoreLocation.LocalMachine,
            StoreName = StoreName.My
        },
        new()
        {
            Name = "waters_connect Caller",
            StoreLocation = StoreLocation.LocalMachine,
            StoreName = StoreName.My
        },
        new()
        {
            Name = "waters_connect Client TLS",
            StoreLocation = StoreLocation.LocalMachine,
            StoreName = StoreName.My
        },
        new()
        {
            Name = "waters_connect Server",
            StoreLocation = StoreLocation.LocalMachine,
            StoreName = StoreName.My
        },
        new()
        {
            Name = "waters_connect Service",
            StoreLocation = StoreLocation.LocalMachine,
            StoreName = StoreName.My
        },
        new()
        {
            Name = "waters_connect TLS",
            StoreLocation = StoreLocation.LocalMachine,
            StoreName = StoreName.My
        }
    };

    public IEnumerator<CertificateIdentifier> GetEnumerator()
    {
        return ((IEnumerable<CertificateIdentifier>)certificateIdentifiers).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}