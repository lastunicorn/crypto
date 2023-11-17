using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class InstallCertificateStep : StepBase
{
    private readonly ICertificateRepository certificateRepository;

    public override string Title => "Install Certificate";

    public GenericCertificate Certificate { get; set; }

    public InstallCertificateStep(ILog log, ICertificateRepository certificateRepository)
        : base(log)
    {
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    protected override void DoExecute()
    {
        if (Certificate == null) throw new ArgumentException(nameof(Certificate));

        Log.WriteValue("Store Location", Certificate.StoreLocation);
        Log.WriteValue("Store Name", Certificate.StoreName);

        certificateRepository.Add(Certificate, Certificate.StoreName, Certificate.StoreLocation);

        string certificateName = Certificate.GetName();

        Log.WriteLine();
        Log.WriteSuccess($"Certificate '{certificateName}' added in store.");
    }
}