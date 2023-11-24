using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

internal class FindCertificateStep : StepBase
{
    private readonly ICertificateRepository certificateRepository;

    public override string Title => "Find Certificate";

    public CertificateIdentifier CertificateIdentifier { get; set; }

    public List<GenericCertificate> FoundCertificates { get; set; }

    public FindCertificateStep(ILog log, ICertificateRepository certificateRepository)
        : base(log)
    {
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    protected override void DoExecute()
    {
        string certificateLocation = $"{CertificateIdentifier.StoreLocation}\\{CertificateIdentifier.StoreName}";
        Log.WriteValue("Store", certificateLocation);
        Log.WriteValue("SubjectName", CertificateIdentifier.Name);
        Log.WriteLine();

        FoundCertificates = certificateRepository.Get(CertificateIdentifier)
            .ToList();

        if (FoundCertificates == null || FoundCertificates.Count == 0)
        {
            Log.WriteError("Certificate was NOT found.");
        }
        else
        {
            int certificatesCount = FoundCertificates.Count;
            Log.WriteSuccess($"Found {certificatesCount} certificate(s).");
        }
    }
}