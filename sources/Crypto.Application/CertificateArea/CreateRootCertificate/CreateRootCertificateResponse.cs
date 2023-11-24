using DustInTheWind.Crypto.Application.Results;

namespace DustInTheWind.Crypto.Application.CertificateArea.CreateRootCertificate;

public class CreateRootCertificateResponse
{
    public CertificateGenerationResult CertificateGenerationResult { get; set; }
 
    public InstallCertificateResult InstallCertificateResult { get; set; }
}