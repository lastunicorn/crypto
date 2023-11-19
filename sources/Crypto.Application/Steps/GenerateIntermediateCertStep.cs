using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class GenerateIntermediateCertStep : StepBase
{
    public override string Title => "Generate Intermediate Certificate";

    public GenericCertificate ParentCertificate { get; set; }

    public string SubjectName { get; set; }

    public string FriendlyName { get; set; }

    public int KeyLength { get; set; }

    public bool AllowExport { get; set; }

    public bool IsEphemeralKey { get; set; }

    public GenericCertificate GeneratedCertificate { get; private set; }

    public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

    public StoreName StoreName { get; set; } = StoreName.CertificateAuthority;

    public PersistenceType PrivateKeyPersistence { get; set; }

    public GenerateIntermediateCertStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        if (ParentCertificate == null) throw new ArgumentNullException(nameof(ParentCertificate));
        if (SubjectName == null) throw new ArgumentNullException(nameof(SubjectName));

        Log.WriteType("Parent Certificate", ParentCertificate);
        Log.WriteValue("Subject Name", SubjectName);
        Log.WriteValue("Certificate Friendly Name", FriendlyName);
        Log.WriteValue("Allow Export", AllowExport);
        Log.WriteLine();

        IntermediateCertificateBuilder builder = new()
        {
            ParentCertificate = ParentCertificate,
            SubjectName = SubjectName,
            FriendlyName = FriendlyName,
            PublicKeyLength = KeyLength,
            PrivateKeyLength = KeyLength,
            Validity = TimeSpan.FromDays(36500),
            PrivateKeyPersistence = PrivateKeyPersistence,
            AllowPrivateKeyExport = AllowExport,
            IsPrivateKeyEphemeral = IsEphemeralKey,
            StoreName = StoreName,
            StoreLocation = StoreLocation
        };

        GeneratedCertificate = builder.Build();

        Log.WriteSuccess("Certificate Generated Successfully");
        Log.WriteLine();
    }
}