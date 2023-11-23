using DustInTheWind.Crypto.Presentation.Controls;
using DustInTheWind.Crypto.Presentation.Infrastructure;

namespace DustInTheWind.Crypto.Presentation.Commands.CertificateArea.FindCertificate;

internal class FindCertificateView : CustomView<FindCertificateViewModel>
{
    public override void Display(FindCertificateViewModel viewModel)
    {
        FindCertificateControl control = new(viewModel.FindCertificateResult);
        control.Display();
    }
}