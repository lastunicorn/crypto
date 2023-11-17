using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain.CertificateModel;

public class NormalCertificateBuilder : CertificateBuilderBase
{
    public GenericCertificate ParentCertificate { get; set; }

    public NormalCertificateBuilder()
    {
        KeyUsage = new OidCollection
        {
            new(OidValues.ServerAuthentication),
            new(OidValues.ClientAuthentication)
        };

        KeyUsageFlags = X509KeyUsageFlags.KeyEncipherment |
                        X509KeyUsageFlags.DigitalSignature |
                        X509KeyUsageFlags.DataEncipherment;
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
        X509BasicConstraintsExtension basicConstraintsExtension = new(false, false, 0, true);
        certificateRequest.CertificateExtensions.Add(basicConstraintsExtension);
    }

    private static void AddSubjectKeyIdentifierExtension(CertificateRequest certificateRequest)
    {
        X509SubjectKeyIdentifierExtension subjectKeyIdentifierExtension = new(certificateRequest.PublicKey, false);
        certificateRequest.CertificateExtensions.Add(subjectKeyIdentifierExtension);
    }

    private void AddEnhancedKeyUsageExtension(CertificateRequest certificateRequest)
    {
        //Indicate this certificate is to be used as a TLS Server/Client via the EKU extension

        X509EnhancedKeyUsageExtension enhancedKeyUsageExtension = new(KeyUsage, false);
        certificateRequest.CertificateExtensions.Add(enhancedKeyUsageExtension);
    }

    private void AddKeyUsageExtension(CertificateRequest certificateRequest)
    {
        // Allow the certificate to be used for specific actions.

        X509KeyUsageExtension keyUsageExtension = new(KeyUsageFlags, true);
        certificateRequest.CertificateExtensions.Add(keyUsageExtension);
    }

    private void AddAuthorityKeyIdentifierExtension(CertificateRequest certificateRequest)
    {
        X509AuthorityKeyIdentifierExtension authorityKeyIdentifierExtension = new(ParentCertificate.Value, false);

        if (!authorityKeyIdentifierExtension.IsEmpty)
            certificateRequest.CertificateExtensions.Add(authorityKeyIdentifierExtension);
    }
}