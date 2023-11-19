using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.CertificateAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class FindCertificateBySubjectStep : StepBase
{
    private readonly ICertificateRepository certificateRepository;

    public override string Title => "Find Certificate by Subject Name";

    public string SubjectName { get; set; }

    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }
    
    public List<GenericCertificate> FoundCertificates { get; private set; }

    public FindCertificateBySubjectStep(ILog log, ICertificateRepository certificateRepository)
        : base(log)
    {
        this.certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    protected override void DoExecute()
    {
        string certificateLocation = $"{StoreLocation}\\{StoreName}";
        Log.WriteValue("Store", certificateLocation);
        Log.WriteValue("SubjectName", SubjectName);
        Log.WriteLine();

        FoundCertificates = certificateRepository.Get(SubjectName, StoreName, StoreLocation)
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