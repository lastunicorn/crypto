using System.Security.Cryptography.X509Certificates;
using DustInTheWind.Crypto.Domain.CertificateModel;

namespace DustInTheWind.Crypto.Ports.UserAccess;

public interface IUserInterface
{
    void DisplayOverview(GenericCertificate certificate);
}