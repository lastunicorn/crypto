using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Domain.PrivateKeyModel;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.Ports.UserAccess;

namespace DustInTheWind.Crypto.Application.Steps;

internal class ShowCertificateLocationStep : StepBase
{
    private readonly IUserInterface userInterface;

    public override string Title => "Show Certificate Key Location";

    public GenericCertificate Certificate { get; set; }

    public ShowCertificateLocationStep(ILog log, IUserInterface userInterface)
        : base(log)
    {
        this.userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
    }

    protected override void DoExecute()
    {
        if (Certificate == null) throw new ArgumentException(nameof(Certificate));

        X509Certificate2 certificate = Certificate.Value;

        Log.WriteValue("Simple Name", certificate.GetNameInfo(X509NameType.SimpleName, false));
        Log.WriteValue("Friendly Name", certificate.FriendlyName);

        Log.WriteHorizontalLine(1);

        DisplayRsaKeyInfo(certificate);
    }

    #region Display RSA Keys

    private void DisplayRsaKeyInfo(X509Certificate2 certificate)
    {
        RSA rsaPublicKey = certificate.GetRSAPublicKey();
        DisplayKey(rsaPublicKey, "RSA Public Key (the new approach)");

        Log.WriteLine();

        RSA rsaPrivateKey = certificate.GetRSAPrivateKey();
        DisplayKey(rsaPrivateKey, "RSA Private Key (the new approach)");
    }

    private void DisplayKey(RSA rsa, string title)
    {
        Log.WithIndentation(title, () =>
        {
            Log.WriteType("Key type", rsa);

            DisplayRsaFileLocation(rsa);
        });
    }

    private void DisplayRsaFileLocation(RSA rsa)
    {
        PrivateKeyFile privateKeyFile = new(rsa);

        Log.WithIndentation("Key file", () =>
        {
            if (privateKeyFile.Exists)
            {
                Log.WriteValue("Full Path", privateKeyFile.FullPath);
                Log.WriteValue("Directory", privateKeyFile.DirectoryPath);
                Log.WriteValue("Directory Type", privateKeyFile.LocationType.ToString());
            }
            else
            {
                Log.WriteWarning("Key file was not found.");
            }
        });
    }

    #endregion
}