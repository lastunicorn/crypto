using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Application.Sections;

public class FindCertificateResult : ResultBase
{
    public override string Title => "Find Certificate";

    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string CertificateName { get; set; }

    public int CertificateCount { get; set; }
}