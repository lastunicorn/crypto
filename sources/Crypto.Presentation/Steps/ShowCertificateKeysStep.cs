using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Domain.PrivateKeyModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class ShowCertificateKeysStep : StepBase
{
    public override string Title => "Show Certificate Keys";

    public GenericCertificate Certificate { get; set; }

    public ShowCertificateKeysStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        if (Certificate == null) throw new ArgumentException(nameof(Certificate));

        X509Certificate2 certificate = Certificate.Value;

        Log.WriteValue("Simple Name", certificate.GetNameInfo(X509NameType.SimpleName, false));
        Log.WriteValue("Friendly Name", certificate.FriendlyName);

        Log.WriteHorizontalLine(1);

        DisplayKeyOverview(certificate);

        DisplayRsaKeyInfo(certificate);
        DisplayECDsaKeyInfo(certificate);
        DisplayECDiffieHellmanKeyInfo(certificate);
        DisplayDsaKeyInfo(certificate);
    }

    private void DisplayKeyOverview(X509Certificate2 certificate)
    {
        Log.WithIndentation("PublicKey property", () =>
        {
            Log.WriteValue("PublicKey.Oid", certificate.PublicKey.Oid.ToNiceString());
            Log.WriteValue("EncodedKeyValue OID", certificate.PublicKey.EncodedKeyValue.Oid.ToNiceString());
            Log.WriteValue("EncodedKeyValue Data", certificate.PublicKey.EncodedKeyValue.RawData);
            Log.WriteValue("EncodedParameters OID", certificate.PublicKey.EncodedParameters.Oid.ToNiceString());
            Log.WriteValue("EncodedParameters Data", certificate.PublicKey.EncodedParameters.RawData);
            Log.WriteType("PublicKey.Key (obsolete)", certificate.PublicKey.Key);
        });

        Log.WriteLine();

        Log.WithIndentation("PrivateKey property (obsolete)", () =>
        {
            Log.WriteType("PrivateKey", certificate.PrivateKey);
        });
    }

    #region Display RSA Keys

    private void DisplayRsaKeyInfo(X509Certificate2 certificate)
    {
        Log.WriteHorizontalLine(1);

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
            Log.WriteLine();
            Log.WriteType("Key type", rsa);
            
            Log.WriteLine();
            DisplayKeyDetails(rsa);

            Log.WriteLine();
            DisplayRsaParameters(rsa);
            
            Log.WriteLine();
            DisplayRsaFileLocation(rsa);
        });
    }

    private void DisplayKeyDetails(RSA rsa)
    {
        switch (rsa)
        {
            case RSACryptoServiceProvider rsaCryptoServiceProvider:
                DisplayKeyDetails(rsaCryptoServiceProvider);
                break;

            case RSACng rsaCng:
                DisplayKeyDetails(rsaCng);
                break;

            default:
                Log.WriteError("The key is not of a known type.");
                break;
        }
    }

    private void DisplayKeyDetails(RSACryptoServiceProvider rsaCryptoServiceProvider)
    {
        Log.WithIndentation("Key details (RSACryptoServiceProvider)", () =>
        {
            CspKeyContainerInfo cspKeyContainerInfo = rsaCryptoServiceProvider.CspKeyContainerInfo;

            Log.WriteValue("UniqueKeyContainerName", cspKeyContainerInfo.UniqueKeyContainerName);
            Log.WriteValue("KeyNumber", cspKeyContainerInfo.KeyNumber);
            Log.WriteValue("KeyContainerName", cspKeyContainerInfo.KeyContainerName);
        });
    }

    private void DisplayKeyDetails(RSACng rsaCng)
    {
        Log.WithIndentation("Key details (RSACng)", () =>
        {
            CngKey cngKey = rsaCng.Key;

            Log.WriteValue("KeyName", cngKey.KeyName);
            Log.WriteValue("UniqueName", cngKey.UniqueName);
            Log.WriteValue("IsMachineKey", cngKey.IsMachineKey);
            Log.WriteValue("IsEphemeral", cngKey.IsEphemeral);
            Log.WriteValue("KeySize", cngKey.KeySize);
            Log.WriteValue("KeyUsage", cngKey.KeyUsage);
            Log.WriteValue("ExportPolicy", cngKey.ExportPolicy);
            Log.WriteValue("UIPolicy", cngKey.UIPolicy);
            Log.WriteValue("Provider", cngKey.Provider);
            Log.WriteType("ProviderHandle", cngKey.ProviderHandle);
        });
    }

    private void DisplayRsaParameters(RSA rsa)
    {
        if (rsa == null)
        {
            Log.WriteValue("RSAParameters", null);
            return;
        }

        Log.WriteLine();
        Log.WithIndentation("RSAParameters (only public)", () =>
        {
            RSAParameters parameters = rsa.ExportParameters(false);
            Display(parameters);
        });

        Log.WriteLine();
        Log.WithIndentation("RSAParameters (XML Format) (only public)", () =>
        {
            string xml = rsa.ToXmlString(false);
            Log.WriteValue("Xml", xml);
        });

        Log.WriteLine();
        Log.WithIndentation("RSAParameters (public and private)", () =>
        {
            RSAParameters parameters = rsa.ExportParameters(true);
            Display(parameters);
        });

        Log.WriteLine();
        Log.WithIndentation("RSAParameters (XML Format) (public and private)", () =>
        {
            string xml = rsa.ToXmlString(true);
            Log.WriteValue("Xml", xml);
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

    private void Display(RSAParameters parameters)
    {
        Log.WriteValue("D", parameters.D);
        Log.WriteValue("DP", parameters.DP);
        Log.WriteValue("DQ", parameters.DQ);
        Log.WriteValue("Exponent", parameters.Exponent);
        Log.WriteValue("InverseQ", parameters.InverseQ);
        Log.WriteValue("Modulus", parameters.Modulus);
        Log.WriteValue("P", parameters.P);
        Log.WriteValue("Q", parameters.Q);
    }

    #endregion

    #region Display ECDsa Keys

    private void DisplayECDsaKeyInfo(X509Certificate2 certificate)
    {
        Log.WriteHorizontalLine(1);

        ECDsa ecdsaPublicKey = certificate.GetECDsaPublicKey();
        DisplayKey(ecdsaPublicKey, "ECDsa Public Key (the new approach)");

        Log.WriteLine();

        ECDsa ecdsaPrivateKey = certificate.GetECDsaPrivateKey();
        DisplayKey(ecdsaPrivateKey, "ECDsa Private Key (the new approach)");
    }

    private void DisplayKey(ECDsa ecDsa, string title)
    {
        Log.WithIndentation(title, () =>
        {
            Log.WriteType("Key type", ecDsa);
        });
    }

    #endregion

    #region Display ECDiffieHellman Keys

    private void DisplayECDiffieHellmanKeyInfo(X509Certificate2 certificate)
    {
        Log.WriteHorizontalLine(1);

        ECDiffieHellman ecDiffieHellmanPublicKey = certificate?.GetECDiffieHellmanPublicKey();
        DisplayKey(ecDiffieHellmanPublicKey, "ECDiffieHellman Public Key (the new approach)");

        Log.WriteLine();

        ECDiffieHellman ecDiffieHellmanPrivateKey = certificate.GetECDiffieHellmanPrivateKey();
        DisplayKey(ecDiffieHellmanPrivateKey, "ECDiffieHellman Private Key (the new approach)");
    }

    private void DisplayKey(ECDiffieHellman ecDiffieHellman, string title)
    {
        Log.WithIndentation(title, () =>
        {
            Log.WriteType("Key type", ecDiffieHellman);
        });
    }

    #endregion

    #region Display DSA Keys

    private void DisplayDsaKeyInfo(X509Certificate2 certificate)
    {
        Log.WriteHorizontalLine(1);

        DSA dsaPublicKey = certificate.GetDSAPublicKey();
        DisplayKey(dsaPublicKey, "DSA Public Key (the new approach)");

        Log.WriteLine();

        DSA dsaPrivateKey = certificate.GetDSAPrivateKey();
        DisplayKey(dsaPrivateKey, "DSA Private Key (the new approach)");
    }

    private void DisplayKey(DSA dsa, string title)
    {
        Log.WithIndentation(title, () =>
        {
            Log.WriteType("Key type", dsa);
        });
    }

    #endregion
}