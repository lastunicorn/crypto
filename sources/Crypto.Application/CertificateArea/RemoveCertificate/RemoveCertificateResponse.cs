using DustInTheWind.Crypto.Application.Sections;

namespace DustInTheWind.Crypto.Application.CertificateArea.RemoveCertificate;

public class RemoveCertificateResponse
{
    public FindCertificateResult FindCertificateResult { get; set; }

    public List<RemoveCertificateResult> RemoveCertificateResults { get; } = new();
}