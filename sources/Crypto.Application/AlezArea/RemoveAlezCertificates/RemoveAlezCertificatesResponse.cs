using DustInTheWind.Crypto.Application.Results;

namespace DustInTheWind.Crypto.Application.AlezArea.RemoveAlezCertificates;

public class RemoveAlezCertificatesResponse
{
    public List<CertificateSearchAndRemovalResult> Results { get; } = new();
}