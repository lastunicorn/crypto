using DustInTheWind.Crypto.Application.Results;

namespace DustInTheWind.Crypto.Application.Sections;

public class CertificateRemovalResult
{
    public string CertificateSubjectName { get; set; }

    public bool CertificateContainsPrivateKey { get; set; }

    public PrivateKeyFileRemovalInfo PrivateKeyFileRemovalInfo { get; set; }

    public Exception Error { get; set; }
}