using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class VerifyCertificateIsInstalledStep : StepBase
{
    public override string Title => "Verify Certificate is Installed";

    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public X509Certificate2 Certificate { get; set; }

    public VerifyCertificateIsInstalledStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        X509Store store = new(StoreName, StoreLocation);

        try
        {
            store.Open(OpenFlags.ReadWrite);

            bool exists = store.Certificates.Contains(Certificate);

            string location = $@"{StoreLocation}\{StoreName}";
            string certificateName = Certificate.GetNameInfo(X509NameType.SimpleName, false);

            string message = exists
                ? $"'{certificateName}' is installed in '{location}'."
                : $"'{certificateName}' is NOT installed in '{location}'.";

            Console.WriteLine(message);
        }
        finally
        {
            store.Close();
        }
    }
}