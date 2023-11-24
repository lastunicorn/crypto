using System.Security.Cryptography;
using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Domain.PrivateKeyModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;

namespace DustInTheWind.Crypto.Application.CertificateArea.RemoveCertificate;

internal class CertificateRemoval
{
    private readonly ICertificateRepository certificateRepository;
    private readonly GenericCertificate certificate;
    private CertificateRemovalResult result;

    public CertificateRemoval(GenericCertificate certificate, ICertificateRepository certificateRepository)
    {
        this.certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public CertificateRemovalResult Execute()
    {
        result = new CertificateRemovalResult
        {
            CertificateSubjectName = certificate.SubjectName
        };

        try
        {
            PrivateKeyFile privateKeyFile = GetPrivateKey();

            if (privateKeyFile?.Exists == true)
            {
                result.PrivateKeyFileInfo = new PrivateKeyFileInfo
                {
                    FilePath = privateKeyFile.FullPath,
                    DirectoryPath = privateKeyFile.DirectoryPath,
                    LocationType = privateKeyFile.LocationType
                };
            }

            certificateRepository.Remove(certificate);

            if (privateKeyFile?.Exists == true)
                RemovePrivateKeyFile(privateKeyFile);
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }
        
        return result;
    }

    private PrivateKeyFile GetPrivateKey()
    {
        RSA rsaPrivateKey = certificate.GetRsaPrivateKey();

        if (rsaPrivateKey == null)
            return null;

        result.CertificateContainsPrivateKey = true;

        return new PrivateKeyFile(rsaPrivateKey);
    }

    private void RemovePrivateKeyFile(PrivateKeyFile privateKeyFile)
    {
        try
        {
            result.PrivateKeyFileInfo.IsSuccessfullyRemoved = privateKeyFile.Delete();
        }
        catch (Exception ex)
        {
            result.PrivateKeyFileInfo.RemoveError = ex;
        }
    }
}