// VeloCity
// Copyright (C) 2022 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Domain.PrivateKeyModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class ShowCertificateLocationStep : StepBase
{
    public override string Title => "Show Certificate Key Location";

    public GenericCertificate Certificate { get; set; }

    public ShowCertificateLocationStep(ILog log)
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