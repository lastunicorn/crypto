using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Application.Results;

public class FindCertificateResult
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string CertificateName { get; set; }

    public int CertificateCount { get; set; }
}