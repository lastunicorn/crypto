using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Results;

namespace DustInTheWind.Crypto.Application.CertificateArea.RemoveCertificate;

public class RemoveCertificateResponse
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string CertificateName { get; set; }
    
    public int CertificateCount { get; set; }

    public List<CertificateRemovalResult> CertificateRemovalResults { get; } = new();
}