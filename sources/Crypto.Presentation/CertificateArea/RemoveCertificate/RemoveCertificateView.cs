using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Presentation.Infrastructure;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.RemoveCertificate;

internal class RemoveCertificateView : CustomView<RemoveCertificateViewModel>
{
    public override void Display(RemoveCertificateViewModel viewModel)
    {
        DisplaySearchInfo(viewModel);
        DisplayRemovalResults(viewModel.CertificateRemovalResults);
    }

    private void DisplaySearchInfo(RemoveCertificateViewModel viewModel)
    {
        WriteTitle("Search Certificates");

        string certificateLocation = $"{viewModel.StoreLocation}\\{viewModel.StoreName}";
        WriteValue("Store", certificateLocation);
        WriteValue("Certificate Name", viewModel.CertificateName);
        WriteLine();

        switch (viewModel.CertificateCount)
        {
            case 0:
                WriteWarning("Certificate was NOT found.");
                break;

            case 1:
                WriteSuccess($"Found {viewModel.CertificateCount} certificate.");
                break;

            default:
                WriteSuccess($"Found {viewModel.CertificateCount} certificate(s).");
                break;
        }
    }

    private void DisplayRemovalResults(IReadOnlyList<CertificateRemovalResult> removeCertificateResults)
    {
        if (removeCertificateResults.Count == 0)
        {
            WriteInfo("No certificates were removed.");
            return;
        }

        for (int i = 0; i < removeCertificateResults.Count; i++) 
            Display(removeCertificateResults[i], i);
    }

    private static void Display(CertificateRemovalResult certificateRemovalResult, int index)
    {
        RemoveCertificateControl removeCertificateControl = new(certificateRemovalResult)
        {
            Subtitle = index.ToString()
        };
        removeCertificateControl.Display();
    }
}