//using System.Security.Cryptography;
//using System.Security.Cryptography.X509Certificates;
//using Waters.Crypto.Domain.CertificateModel;
//using Waters.Crypto.Domain.PrivateKeyModel;
//using Waters.Crypto.Ports.LogAccess;

//namespace Waters.Crypto.PresentationAndUseCases.Steps;

//internal class ShowCertificatesStep : StepBase
//{
//    protected override string Title => "Show Certificates";

//    public List<CertificateBase> Certificates { get; set; }

//    public ShowCertificatesStep(ILog log)
//        : base(log)
//    {
//    }

//    protected override void DoExecute()
//    {
//        if (Certificates == null) throw new ArgumentException(nameof(Certificates));

//        if (Certificates.Count == 0)
//        {
//            Log.WriteInfo("No certificates.");
//        }
//        else
//        {
//            for (int i = 0; i < Certificates.Count; i++)
//            {
//                CertificateBase certificate = Certificates[i];
//                Log.WriteInfo($"Certificate index {i}");
//                Log.WriteLine();

//                ProcessCertificate(certificate.Value);
//            }
//        }
//    }

//    private void ProcessCertificate(X509Certificate2 certificate)
//    {
//        Log.WithIndentation("Names", () =>
//        {
//            IDictionary<string, string> names = Enum.GetValues<X509NameType>()
//                .ToDictionary(Enum.GetName, x => certificate.GetNameInfo(x, false));

//            foreach ((string key, string value) in names)
//                Log.WriteValue(key, value);
//        });

//        Log.WriteValue("Friendly Name", certificate.FriendlyName);

//        Log.WriteValue("Subject", certificate.Subject);
//        Log.WriteValue("SubjectName.Name", certificate.SubjectName.Name);
//        Log.WriteValue("SubjectName.Oid", certificate.SubjectName.Oid.ToNiceString());

//        Log.WriteValue("Has private key", certificate.HasPrivateKey);
//        Log.WriteValue("Issuer", certificate.Issuer);
//        Log.WriteValue("Version", certificate.Version);
//        Log.WriteValue("Valid Date", certificate.NotBefore);
//        Log.WriteValue("Expiry Date", certificate.NotAfter);
//        Log.WriteValue("Thumbprint", certificate.Thumbprint);
//        Log.WriteValue("Serial Number", certificate.SerialNumber);
//        Log.WriteValue("Archived", certificate.Archived);
//        Log.WriteValue("SignatureAlgorithm OID", certificate.SignatureAlgorithm.ToNiceString());
//        Log.WriteValue("Raw Data Length", certificate.RawData.Length);
//        Log.WriteValue("To String", certificate.ToString(true));

//        Log.WriteHorizontalLine(1);

//        DisplayKeyOverview(certificate);

//        DisplayRsaKeyInfo(certificate);
//        DisplayECDsaKeyInfo(certificate);
//        DisplayECDiffieHellmanKeyInfo(certificate);
//        DisplayDsaKeyInfo(certificate);

//        DisplayExtensions(certificate);
//    }

//    private void DisplayKeyOverview(X509Certificate2 certificate)
//    {
//        Log.WithIndentation("PublicKey property", () =>
//        {
//            Log.WriteValue("PublicKey.Oid", certificate.PublicKey.Oid.ToNiceString());
//            Log.WriteValue("EncodedKeyValue OID", certificate.PublicKey.EncodedKeyValue.Oid.ToNiceString());
//            Log.WriteValue("EncodedKeyValue Data", certificate.PublicKey.EncodedKeyValue.RawData);
//            Log.WriteValue("EncodedParameters OID", certificate.PublicKey.EncodedParameters.Oid.ToNiceString());
//            Log.WriteValue("EncodedParameters Data", certificate.PublicKey.EncodedParameters.RawData);
//            Log.WriteType("PublicKey.Key (obsolete)", certificate.PublicKey.Key);
//        });

//        Log.WriteLine();

//        Log.WithIndentation("PrivateKey property (obsolete)", () =>
//        {
//            Log.WriteType("PrivateKey", certificate.PrivateKey);
//        });
//    }

//    #region Display RSA Keys

//    private void DisplayRsaKeyInfo(X509Certificate2 certificate)
//    {
//        Log.WriteHorizontalLine(1);

//        RSA rsaPublicKey = certificate.GetRSAPublicKey();
//        DisplayKey(rsaPublicKey, "RSA Public Key (the new approach)");

//        Log.WriteLine();

//        RSA rsaPrivateKey = certificate.GetRSAPrivateKey();
//        DisplayKey(rsaPrivateKey, "RSA Private Key (the new approach)");
//    }

//    private void DisplayKey(RSA rsa, string title)
//    {
//        Log.WithIndentation(title, () =>
//        {
//            Log.WriteType("Key type", rsa);

//            switch (rsa)
//            {
//                case RSACryptoServiceProvider rsaCryptoServiceProvider:
//                    DisplayKeyDetails(rsaCryptoServiceProvider);
//                    break;

//                case RSACng rsaCng:
//                    DisplayKeyDetails(rsaCng);
//                    break;

//                default:
//                    Log.WriteError("The key is not of a known type.");
//                    break;
//            }

//            DisplayRsaParameters(rsa);
//            DisplayRsaFileLocation(rsa);
//        });
//    }

//    private void DisplayKeyDetails(RSACryptoServiceProvider rsaCryptoServiceProvider)
//    {
//        Log.WithIndentation("Key details (RSACryptoServiceProvider)", () =>
//        {
//            CspKeyContainerInfo cspKeyContainerInfo = rsaCryptoServiceProvider.CspKeyContainerInfo;

//            Log.WriteValue("UniqueKeyContainerName", cspKeyContainerInfo.UniqueKeyContainerName);
//            Log.WriteValue("KeyNumber", cspKeyContainerInfo.KeyNumber);
//            Log.WriteValue("KeyContainerName", cspKeyContainerInfo.KeyContainerName);
//        });
//    }

//    private void DisplayKeyDetails(RSACng rsaCng)
//    {
//        Log.WithIndentation("Key details (RSACng)", () =>
//        {
//            CngKey cngKey = rsaCng.Key;

//            Log.WriteValue("KeyName", cngKey.KeyName);
//            Log.WriteValue("UniqueName", cngKey.UniqueName);
//            Log.WriteValue("IsMachineKey", cngKey.IsMachineKey);
//            Log.WriteValue("IsEphemeral", cngKey.IsEphemeral);
//            Log.WriteValue("KeySize", cngKey.KeySize);
//            Log.WriteValue("KeyUsage", cngKey.KeyUsage);
//            Log.WriteValue("ExportPolicy", cngKey.ExportPolicy);
//            Log.WriteValue("UIPolicy", cngKey.UIPolicy);
//            Log.WriteValue("Provider", cngKey.Provider);
//            Log.WriteType("ProviderHandle", cngKey.ProviderHandle);
//        });
//    }

//    private void DisplayRsaParameters(RSA rsa)
//    {
//        if (rsa == null)
//        {
//            Log.WriteValue("RSAParameters", null);
//            return;
//        }

//        Log.WithIndentation("RSAParameters (only public)", () =>
//        {
//            RSAParameters parameters = rsa.ExportParameters(false);
//            Display(parameters);
//        });

//        Log.WithIndentation("RSAParameters (XML Format) (only public)", () =>
//        {
//            string xml = rsa.ToXmlString(false);
//            Log.WriteValue("Xml", xml);
//        });

//        Log.WithIndentation("RSAParameters (public and private)", () =>
//        {
//            RSAParameters parameters = rsa.ExportParameters(true);
//            Display(parameters);
//        });

//        Log.WithIndentation("RSAParameters (XML Format) (public and private)", () =>
//        {
//            string xml = rsa.ToXmlString(true);
//            Log.WriteValue("Xml", xml);
//        });
//    }

//    private void DisplayRsaFileLocation(RSA rsa)
//    {
//        PrivateKeyFile privateKeyFile = new(rsa);

//        Log.WithIndentation("Key file", () =>
//        {
//            if (privateKeyFile.FullPath == null)
//            {
//                Log.WriteWarning("Key file was not found.");
//            }
//            else
//            {
//                Log.WriteValue("Full Path", privateKeyFile.FullPath);
//                Log.WriteValue("Directory", privateKeyFile.DirectoryPath);
//                Log.WriteValue("Directory Type", privateKeyFile.LocationType.ToString());
//            }
//        });
//    }

//    private void Display(RSAParameters parameters)
//    {
//        Log.WriteValue("D", parameters.D);
//        Log.WriteValue("DP", parameters.DP);
//        Log.WriteValue("DQ", parameters.DQ);
//        Log.WriteValue("Exponent", parameters.Exponent);
//        Log.WriteValue("InverseQ", parameters.InverseQ);
//        Log.WriteValue("Modulus", parameters.Modulus);
//        Log.WriteValue("P", parameters.P);
//        Log.WriteValue("Q", parameters.Q);
//    }

//    #endregion

//    #region Display ECDsa Keys

//    private void DisplayECDsaKeyInfo(X509Certificate2 certificate)
//    {
//        Log.WriteHorizontalLine(1);

//        ECDsa ecdsaPublicKey = certificate.GetECDsaPublicKey();
//        DisplayKey(ecdsaPublicKey, "ECDsa Public Key (the new approach)");

//        Log.WriteLine();

//        ECDsa ecdsaPrivateKey = certificate.GetECDsaPrivateKey();
//        DisplayKey(ecdsaPrivateKey, "ECDsa Private Key (the new approach)");
//    }

//    private void DisplayKey(ECDsa ecDsa, string title)
//    {
//        Log.WithIndentation(title, () =>
//        {
//            Log.WriteType("Key type", ecDsa);
//        });
//    }

//    #endregion

//    #region Display ECDiffieHellman Keys

//    private void DisplayECDiffieHellmanKeyInfo(X509Certificate2 certificate)
//    {
//        Log.WriteHorizontalLine(1);

//        ECDiffieHellman ecDiffieHellmanPublicKey = certificate?.GetECDiffieHellmanPublicKey();
//        DisplayKey(ecDiffieHellmanPublicKey, "ECDiffieHellman Public Key (the new approach)");

//        Log.WriteLine();

//        ECDiffieHellman ecDiffieHellmanPrivateKey = certificate.GetECDiffieHellmanPrivateKey();
//        DisplayKey(ecDiffieHellmanPrivateKey, "ECDiffieHellman Private Key (the new approach)");
//    }

//    private void DisplayKey(ECDiffieHellman ecDiffieHellman, string title)
//    {
//        Log.WithIndentation(title, () =>
//        {
//            Log.WriteType("Key type", ecDiffieHellman);
//        });
//    }

//    #endregion

//    #region Display DSA Keys

//    private void DisplayDsaKeyInfo(X509Certificate2 certificate)
//    {
//        Log.WriteHorizontalLine(1);

//        DSA dsaPublicKey = certificate.GetDSAPublicKey();
//        DisplayKey(dsaPublicKey, "DSA Public Key (the new approach)");

//        Log.WriteLine();

//        DSA dsaPrivateKey = certificate.GetDSAPrivateKey();
//        DisplayKey(dsaPrivateKey, "DSA Private Key (the new approach)");
//    }

//    private void DisplayKey(DSA dsa, string title)
//    {
//        Log.WithIndentation(title, () =>
//        {
//            Log.WriteType("Key type", dsa);
//        });
//    }

//    #endregion

//    #region Certificate Extenstions

//    private void DisplayExtensions(X509Certificate2 certificate)
//    {
//        Log.WriteHorizontalLine(1);

//        for (int i = 0; i < certificate.Extensions.Count; i++)
//        {
//            if (i > 0)
//                Log.WriteLine();

//            X509Extension extension = certificate.Extensions[i];
//            DisplayExtension($"Extension {i}", extension);
//        }
//    }

//    private void DisplayExtension(string title, X509Extension extension)
//    {
//        Log.WithIndentation(title, () =>
//        {
//            Log.WriteValue("OID", extension.Oid.ToNiceString());
//            Log.WriteValue("Is Critical", extension.Critical);
//            Log.WriteValue("RawData", extension.RawData);
//            Log.WriteValue("Formatted data", extension.Format(true));

//            if (extension is X509BasicConstraintsExtension basicConstraintsExtension)
//            {
//                Log.WriteValue("CertificateAuthority", basicConstraintsExtension.CertificateAuthority);
//                Log.WriteValue("HasPathLengthConstraint", basicConstraintsExtension.HasPathLengthConstraint);
//                Log.WriteValue("PathLengthConstraint", basicConstraintsExtension.PathLengthConstraint);
//            }
//            else if (extension is X509SubjectKeyIdentifierExtension subjectKeyIdentifierExtension)
//            {
//                Log.WriteValue("SubjectKeyIdentifier", subjectKeyIdentifierExtension.SubjectKeyIdentifier);
//            }
//            else if (extension is X509EnhancedKeyUsageExtension enhancedKeyUsageExtension)
//            {
//                Log.WithIndentation("Usages", () =>
//                {
//                    foreach (Oid oid in enhancedKeyUsageExtension.EnhancedKeyUsages)
//                        Log.WriteValue("OID", oid.ToNiceString());
//                });
//            }
//            else if (extension is X509KeyUsageExtension keyUsageExtension)
//            {
//                Log.WriteValue("Usages", keyUsageExtension.KeyUsages);
//            }
//        });
//    }

//    #endregion
//}