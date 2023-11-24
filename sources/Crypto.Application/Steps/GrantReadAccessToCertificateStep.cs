using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Domain.PrivateKeyModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

internal class GrantReadAccessToCertificateStep : StepBase
{
    public override string Title => "Grant Read Access to Certificate";

    public GenericCertificate Certificate { get; set; }

    public GrantReadAccessToCertificateStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        if (Certificate == null) throw new ArgumentException(nameof(Certificate));

        RSA privateKey = Certificate.GetRsaPrivateKey();

        string uniqueName = privateKey.GetUniqueName();
        Log.WriteValue("Private Key Unique Name", uniqueName);

        PrivateKeyFile privateKeyFile = new(privateKey);

        if (privateKeyFile.FullPath == null)
        {
            Console.WriteLine();
            Log.WriteWarning("Private key exists but is not accessible");
            return;
        }

        Log.WriteSuccess("Private key file found");
        Log.WithIndentation(() =>
        {
            Log.WriteValue("File", privateKeyFile.FullPath);
            Log.WriteValue("Directory", privateKeyFile.DirectoryPath);
            Log.WriteValue("Directory Type", privateKeyFile.LocationType.ToString());
        });

        AssignPermissions(privateKeyFile);
    }

    private static void AssignPermissions(PrivateKeyFile privateKeyFile)
    {
        FileInfo file = new(privateKeyFile.FullPath);
        FileSecurity fileSecurity = file.GetAccessControl();

        Console.WriteLine($"Add read permission on private key for {WellKnownSidType.RemoteLogonIdSid}");
        SecurityIdentifier remoteLogonSecurityIdentifier = new(WellKnownSidType.RemoteLogonIdSid, null);
        FileSystemAccessRule remoteLogonAccessRule = new(remoteLogonSecurityIdentifier, FileSystemRights.Read, AccessControlType.Allow);
        fileSecurity.AddAccessRule(remoteLogonAccessRule);
        //fileSecurity.RemoveAccessRule(remoteLogonAccessRule);

        Console.WriteLine($"Add read permission on private key for {WellKnownSidType.WorldSid}");
        SecurityIdentifier worldSecurityIdentifier = new(WellKnownSidType.WorldSid, null);
        FileSystemAccessRule worldAccessRule = new(worldSecurityIdentifier, FileSystemRights.Read, AccessControlType.Allow);
        fileSecurity.AddAccessRule(worldAccessRule);
        //fileSecurity.RemoveAccessRule(worldAccessRule);

        file.SetAccessControl(fileSecurity);
    }
}