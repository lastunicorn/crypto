using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class GenerateRootCertStep : StepBase
{
    public override string Title => "Generate Root Certificate";

    public string SubjectName { get; set; }

    public string FriendlyName { get; set; }

    public int ValidityYears { get; set; }

    public int KeyLength { get; set; }

    public PersistenceType PrivateKeyPersistence { get; set; }

    public bool AllowExport { get; set; }

    public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

    public StoreName StoreName { get; set; } = StoreName.Root;

    public GenericCertificate GeneratedCertificate { get; private set; }

    public GenerateRootCertStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        Log.WriteValue("Subject Name", SubjectName);
        Log.WriteValue("Friendly Name", FriendlyName);
        Log.WriteValue("Expire After Years", ValidityYears);
        Log.WriteValue("Key Length", KeyLength);
        Log.WriteValue("Persist Private Key", PrivateKeyPersistence);
        Log.WriteValue("Allow Export", AllowExport);
        Log.WriteValue("Store Location", StoreLocation);
        Log.WriteValue("Store Name", StoreName);
        Log.WriteLine();

        RootCertificateBuilder builder = new()
        {
            SubjectName = SubjectName,
            FriendlyName = FriendlyName,
            PublicKeyLength = KeyLength,
            PrivateKeyLength = KeyLength,
            Validity = TimeSpan.FromDays(36500),
            PrivateKeyPersistence = PrivateKeyPersistence,
            AllowPrivateKeyExport = AllowExport,
            IsPrivateKeyEphemeral = false,
            StoreName = StoreName,
            StoreLocation = StoreLocation
        };

        GeneratedCertificate = builder.Build();

        Log.WriteSuccess("Certificate Generated Successfully");
    }
}