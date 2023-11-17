using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class GenerateNormalCertificateStep : StepBase
{
    public override string Title => "Generate Normal Certificate";

    public GenericCertificate ParentCertificate { get; set; }

    public string SubjectName { get; set; }

    public string FriendlyName { get; set; }

    public int KeyLength { get; set; }

    public bool AllowExport { get; set; }

    public bool IsEphemeralKey { get; set; }

    public OidCollection KeyUsage { get; set; } = new()
    {
        new Oid(OidValues.ServerAuthentication),
        new Oid(OidValues.ClientAuthentication)
    };

    public X509KeyUsageFlags KeyUsageFlags = X509KeyUsageFlags.KeyEncipherment |
                                             X509KeyUsageFlags.DigitalSignature |
                                             X509KeyUsageFlags.DataEncipherment;

    public GenericCertificate GeneratedCertificate { get; private set; }

    public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

    public StoreName StoreName { get; set; } = StoreName.My;

    public PersistenceType PrivateKeyPersistence { get; set; }

    public GenerateNormalCertificateStep(ILog log)
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

        NormalCertificateBuilder builder = new()
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
            KeyUsage = KeyUsage,
            KeyUsageFlags = KeyUsageFlags,
            StoreName = StoreName,
            StoreLocation = StoreLocation
        };

        GeneratedCertificate = builder.Build();

        Log.WriteSuccess("Certificate Generated Successfully");
        Log.WriteLine();
    }
}