using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public class RootCertificateBuilder : CertificateBuilderBase
{
    public RootCertificateBuilder()
    {
        KeyUsage = new OidCollection
        {
            new(OidValues.ServerAuthentication)
        };

        KeyUsageFlags = X509KeyUsageFlags.DataEncipherment |
                        X509KeyUsageFlags.KeyEncipherment |
                        X509KeyUsageFlags.DigitalSignature |
                        X509KeyUsageFlags.KeyCertSign;
    }

    protected override X509Certificate2 BuildX509Certificate()
    {
        using RSA privateKey = RSA.Create();
        privateKey.KeySize = PrivateKeyLength;

        CertificateRequest certificateRequest = new(SubjectName, privateKey, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

        AddBasicConstraintsExtension(certificateRequest);
        AddSubjectKeyIdentifierExtension(certificateRequest);
        AddKeyUsageExtension(certificateRequest);
        AddEnhancedKeyUsageExtension(certificateRequest);

        DateTimeOffset notBefore = DateTimeOffset.UtcNow.AddDays(-1);
        DateTimeOffset notAfter = DateTimeOffset.UtcNow.AddDays(Validity.Days);

        X509Certificate2 certificate = certificateRequest.CreateSelfSigned(notBefore, notAfter);

        certificate = AddKeyStorageFlags(certificate);

        if (FriendlyName != null)
            certificate.FriendlyName = FriendlyName;

        return certificate;
    }

    private static void AddBasicConstraintsExtension(CertificateRequest certificateRequest)
    {
        X509BasicConstraintsExtension basicConstraintsExtension = new(true, false, 0, true);
        certificateRequest.CertificateExtensions.Add(basicConstraintsExtension);
    }

    private static void AddSubjectKeyIdentifierExtension(CertificateRequest certificateRequest)
    {
        X509SubjectKeyIdentifierExtension subjectKeyIdentifierExtension = new(certificateRequest.PublicKey, false);
        certificateRequest.CertificateExtensions.Add(subjectKeyIdentifierExtension);
    }

    private void AddEnhancedKeyUsageExtension(CertificateRequest certificateRequest)
    {
        X509EnhancedKeyUsageExtension enhancedKeyUsageExtension = new(KeyUsage, false);
        certificateRequest.CertificateExtensions.Add(enhancedKeyUsageExtension);
    }

    private void AddKeyUsageExtension(CertificateRequest certificateRequest)
    {
        X509KeyUsageExtension usageExtension = new(KeyUsageFlags, false);
        certificateRequest.CertificateExtensions.Add(usageExtension);
    }
}