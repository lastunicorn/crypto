using DustInTheWind.Crypto.Presentation.CommonControls;
using DustInTheWind.Crypto.Presentation.Infrastructure;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.FindCertificate;

internal class FindCertificateView : CustomView<FindCertificateViewModel>
{
    public override void Display(FindCertificateViewModel viewModel)
    {
        FindCertificateControl control = new(viewModel.FindCertificateResult);
        control.Display();
    }
}