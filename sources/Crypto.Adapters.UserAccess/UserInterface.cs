using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.UserAccess;

namespace DustInTheWind.Crypto.Adapters.UserAccess;

public class UserInterface : IUserInterface
{
    public void DisplayOverview(GenericCertificate certificate)
    {
        CertificateOverviewControl certificateOverviewControl = new()
        {
            Certificate = certificate
        };

        certificateOverviewControl.Display();
    }
}