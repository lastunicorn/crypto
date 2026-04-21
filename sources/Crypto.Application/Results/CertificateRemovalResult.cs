namespace DustInTheWind.Crypto.Application.Results;

public class CertificateRemovalResult
{
    public string CertificateSubjectName { get; set; }

    public bool CertificateContainsPrivateKey { get; set; }

    public PrivateKeyFileRemovalInfo PrivateKeyFileRemovalInfo { get; set; }

    public Exception Error { get; set; }
}