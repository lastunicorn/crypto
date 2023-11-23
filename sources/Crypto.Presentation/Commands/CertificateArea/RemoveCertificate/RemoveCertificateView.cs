using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Ports.LogAccess;
using DustInTheWind.Crypto.Presentation.Controls;
using DustInTheWind.Crypto.Presentation.Infrastructure;

namespace DustInTheWind.Crypto.Presentation.Commands.CertificateArea.RemoveCertificate;

internal class RemoveCertificateView : CustomView<RemoveCertificateViewModel>
{
    public override void Display(RemoveCertificateViewModel viewModel)
    {
        Display(viewModel.FindCertificateResult);
        Display(viewModel.RemoveCertificateResults);
    }

    private static void Display(FindCertificateResult findCertificateResult)
    {
        FindCertificateControl findCertificateControl = new(findCertificateResult);
        findCertificateControl.Display();
    }

    private void Display(List<RemoveCertificateResult> removeCertificateResults)
    {
        if (removeCertificateResults.Count == 0)
        {
            WriteInfo("No certificates were removed.");
        }
        else
        {
            int index = 0;

            foreach (RemoveCertificateResult removeCertificateResult in removeCertificateResults)
            {
                if (index > 0)
                    WriteLine();
                
                WriteInfo($"Certificate {index}.");
                Display(removeCertificateResult);

                index++;
            }
        }
    }

    private static void Display(RemoveCertificateResult removeCertificateResult)
    {
        RemoveCertificateControl removeCertificateControl = new(removeCertificateResult);
        removeCertificateControl.Display();
    }
}