using DustInTheWind.Crypto.Application.Sections;

namespace DustInTheWind.Crypto.Presentation.Commands.CertificateArea.RemoveCertificate;

internal class RemoveCertificateViewModel
{
    public FindCertificateResult FindCertificateResult { get; set; }

    public List<RemoveCertificateResult> RemoveCertificateResults { get; set; }
}