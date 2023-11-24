using DustInTheWind.Crypto.Application.CertificateArea.CreateRootCertificate;
using DustInTheWind.Crypto.Application.Results;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.CreateRootCertificate;

internal class CreateRootCertificateViewModel
{
    public CertificateGenerationResult CertificateGenerationResult { get; set; }

    public InstallCertificateResult InstallCertificateResult { get; set; }
}