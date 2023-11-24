using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Sections;

namespace DustInTheWind.Crypto.Application.Results;

public class CertificateSearchAndRemovalResult
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string CertificateName { get; set; }
    
    public int CertificateCount { get; set; }

    public List<CertificateRemovalResult> CertificateRemovalResults { get; } = new();
}