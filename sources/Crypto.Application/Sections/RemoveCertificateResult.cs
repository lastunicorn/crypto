namespace DustInTheWind.Crypto.Application.Sections;

public class RemoveCertificateResult : ResultBase
{
    public override string Title => "Remove Certificate";

    public string CertificateSubjectName { get; set; }

    public bool CertificateContainsPrivateKey { get; set; }

    public PrivateKeyFileInfo PrivateKeyFileInfo { get; set; }

    public Exception Error { get; set; }
}