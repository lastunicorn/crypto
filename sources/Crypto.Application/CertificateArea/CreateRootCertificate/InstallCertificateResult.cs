using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Application.CertificateArea.CreateRootCertificate;

public class InstallCertificateResult
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }
    
    public string CertificateName { get; set; }

    public Exception Error { get; set; }
}