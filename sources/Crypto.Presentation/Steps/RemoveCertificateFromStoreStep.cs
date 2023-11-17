using System.Security.Cryptography;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Domain.PrivateKeyModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class RemoveCertificateFromStoreStep : StepBase
{
    private readonly ICertificateRepository certificateRepository;

    public override string Title => "Remove Certificate";

    public List<GenericCertificate> Certificates { get; set; }
    
    public RemoveCertificateFromStoreStep(ILog log, ICertificateRepository certificateRepository)
        : base(log)
    {
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    protected override void DoExecute()
    {
        if (Certificates == null) throw new ArgumentException(nameof(Certificates));

        if (Certificates.Count == 0)
        {
            Log.WriteInfo("No certificates to remove.");
        }
        else
        {
            for (int i = 0; i < Certificates.Count; i++)
            {
                if (i > 0)
                    Log.WriteLine();

                Log.WriteInfo($"Removing certificate {i}.");

                GenericCertificate certificate = Certificates[i];
                RemoveCertificate(certificate);
            }
        }
    }

    private void RemoveCertificate(GenericCertificate certificate)
    {
        try
        {
            Log.WriteValue("Subject Name", certificate.SubjectName);

            PrivateKeyFile privateKeyFile = SearchForPrivateKeyFile(certificate);
            certificateRepository.Remove(certificate);

            if (privateKeyFile?.Exists == true)
                RemovePrivateKeyPathFromDisk(privateKeyFile);

            Log.WriteLine();
            Log.WriteSuccess("Certificate was successfully removed.");
        }
        catch (Exception ex)
        {
            Log.WriteError(ex);
        }
    }

    private PrivateKeyFile SearchForPrivateKeyFile(GenericCertificate certificate)
    {
        RSA rsaPrivateKey = certificate.GetRsaPrivateKey();

        if (rsaPrivateKey == null)
        {
            Log.WriteWarning("Certificate does not contain a private key.");
            return null;
        }

        PrivateKeyFile privateKeyFile = new(rsaPrivateKey);

        if (privateKeyFile.Exists)
        {
            Log.WithIndentation("Private key file was found:", () =>
            {
                Log.WriteValue("Full Path", privateKeyFile.FullPath);
                Log.WriteValue("Directory", privateKeyFile.DirectoryPath);
                Log.WriteValue("Directory Type", privateKeyFile.LocationType.ToString());
            });
        }
        else
        {
            Log.WriteWarning("Private key file was not found.");
        }

        return privateKeyFile;
    }

    private void RemovePrivateKeyPathFromDisk(PrivateKeyFile privateKeyFile)
    {
        try
        {
            bool success = privateKeyFile.Delete();

            if (success)
                Log.WriteSuccess("Private key file was successfully deleted.");
            else
                Log.WriteWarning("Private key file was not deleted.");
        }
        catch (Exception ex)
        {
            Log.WriteError("Failed to delete certificate private key file.");
            Log.WriteError(ex);
        }
    }
}