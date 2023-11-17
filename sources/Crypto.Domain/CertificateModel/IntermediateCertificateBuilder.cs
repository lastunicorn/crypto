using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public class IntermediateCertificateBuilder : CertificateBuilderBase
{
    public GenericCertificate ParentCertificate { get; set; }

    public IntermediateCertificateBuilder()
    {
        KeyUsage = new OidCollection
        {
            new(OidValues.ServerAuthentication),
            new(OidValues.ClientAuthentication)
        };

        KeyUsageFlags = X509KeyUsageFlags.KeyEncipherment |
                        X509KeyUsageFlags.DigitalSignature |
                        X509KeyUsageFlags.DataEncipherment |
                        X509KeyUsageFlags.KeyCertSign;
    }

    protected override X509Certificate2 BuildX509Certificate()
    {
        using RSA publicKey = RSA.Create();
        publicKey.KeySize = PublicKeyLength;

        CertificateRequest certificateRequest = new(SubjectName, publicKey, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

        AddBasicConstraintsExtension(certificateRequest);
        AddSubjectKeyIdentifierExtension(certificateRequest);
        AddKeyUsageExtension(certificateRequest);
        AddEnhancedKeyUsageExtension(certificateRequest);
        AddAuthorityKeyIdentifierExtension(certificateRequest);

        DateTimeOffset notBefore = DateTimeOffset.UtcNow.AddDays(-1);
        DateTime notAfter = ParentCertificate.EndDate;
        byte[] serialNumber = Guid.NewGuid().ToByteArray();

        X509Certificate2 certificate = certificateRequest.Create(ParentCertificate.Value, notBefore, notAfter, serialNumber);

        if (!certificate.HasPrivateKey)
            certificate = AddPrivateKey(certificate, publicKey);

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
        X509SubjectKeyIdentifierExtension x509SubjectKeyIdentifierExtension = new(certificateRequest.PublicKey, false);
        certificateRequest.CertificateExtensions.Add(x509SubjectKeyIdentifierExtension);
    }

    private void AddEnhancedKeyUsageExtension(CertificateRequest certificateRequest)
    {
        X509EnhancedKeyUsageExtension x509EnhancedKeyUsageExtension = new(KeyUsage, false);
        certificateRequest.CertificateExtensions.Add(x509EnhancedKeyUsageExtension);
    }

    private void AddKeyUsageExtension(CertificateRequest certificateRequest)
    {
        // Specify the key usage flags
        X509KeyUsageExtension x509KeyUsageExtension = new(KeyUsageFlags, true);
        certificateRequest.CertificateExtensions.Add(x509KeyUsageExtension);
    }

    private void AddAuthorityKeyIdentifierExtension(CertificateRequest certificateRequest)
    {
        X509AuthorityKeyIdentifierExtension authorityKeyIdentifierExtension = new(ParentCertificate.Value, false);

        if (!authorityKeyIdentifierExtension.IsEmpty)
            certificateRequest.CertificateExtensions.Add(authorityKeyIdentifierExtension);
    }
}