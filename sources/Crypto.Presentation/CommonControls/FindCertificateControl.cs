using System.Security.Cryptography.X509Certificates;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Presentation.Infrastructure;

namespace DustInTheWind.Crypto.Presentation.Controls;

public class FindCertificateControl : SectionControlBase
{
    public StoreLocation StoreLocation { get; set; }

    public StoreName StoreName { get; set; }

    public string CertificateName { get; set; }

    public int CertificateCount { get; set; }

    public FindCertificateControl(FindCertificateResult result)
    {
        Title = result.Title;
        StoreLocation = result.StoreLocation;
        StoreName = result.StoreName;
        CertificateName = result.CertificateName;
        CertificateCount = result.CertificateCount;
    }

    protected override void DoDisplay()
    {
        string certificateLocation = $"{StoreLocation}\\{StoreName}";
        WriteValue("Store", certificateLocation);
        WriteValue("SubjectName", CertificateName);
        WriteLine();

        switch (CertificateCount)
        {
            case 0:
                WriteWarning("Certificate was NOT found.");
                break;

            case 1:
                WriteSuccess($"Found {CertificateCount} certificate.");
                break;

            default:
                WriteSuccess($"Found {CertificateCount} certificate(s).");
                break;
        }
    }
}