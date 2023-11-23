using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class VerifyCertificateIsInstalledStep : StepBase
{
    private readonly ICertificateRepository certificateRepository;

    public override string Title => "Verify Certificate is Installed";

    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public GenericCertificate Certificate { get; set; }

    public VerifyCertificateIsInstalledStep(ILog log, ICertificateRepository certificateRepository)
        : base(log)
    {
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    protected override void DoExecute()
    {
        bool isInstalled = certificateRepository.IsInstalled(Certificate);
        
        string certificateName = Certificate.GetName();
        string location = $@"{StoreLocation}\{StoreName}";

        string message = isInstalled
            ? $"'{certificateName}' is installed in '{location}'."
            : $"'{certificateName}' is NOT installed in '{location}'.";

        Console.WriteLine(message);
    }
}