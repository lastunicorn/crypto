using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.Crypto.Application.CertificateArea.CreateRootCertificate;
using DustInTheWind.Crypto.Application.Results;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.CreateRootCertificate;

internal class CreateRootCertificateView : ViewBase<CreateRootCertificateViewModel>
{
    public override void Display(CreateRootCertificateViewModel viewModel)
    {
        DisplayGenerationInfo(viewModel.CertificateGenerationResult);
        DisplayInstallInfo(viewModel.InstallCertificateResult);
    }

    private void DisplayGenerationInfo(CertificateGenerationResult certificateGenerationResult)
    {
        WriteTitle("Generate Root Certificate");

        WriteInfo("Certificate was generated using the following data:");
        WriteLine();

        WriteValue("Subject Name", certificateGenerationResult.SubjectName);
        WriteValue("Friendly Name", certificateGenerationResult.FriendlyName);
        WriteValue("Expire After Years", certificateGenerationResult.Validity);
        WriteValue("Public Key Length", certificateGenerationResult.PublicKeyLength);
        WriteValue("Private Key Length", certificateGenerationResult.PrivateKeyLength);
        WriteValue("Persist Private Key", certificateGenerationResult.PrivateKeyPersistence);
        WriteValue("Allow Export", certificateGenerationResult.AllowExport);
        WriteValue("Store Location", certificateGenerationResult.StoreLocation);
        WriteValue("Store Name", certificateGenerationResult.StoreName);
        WriteLine();

        if (certificateGenerationResult.Error == null)
        {
            WriteSuccess("Certificate generated successfully.");
        }
        else
        {
            WriteError("Certificate was not generated.");
            WriteError(certificateGenerationResult.Error);
        }
    }

    private void DisplayInstallInfo(InstallCertificateResult installCertificateResult)
    {
        WriteTitle("Install Certificate");

        WriteValue("Store Location", installCertificateResult.StoreLocation);
        WriteValue("Store Name", installCertificateResult.StoreName);
        WriteValue("Certificate Name", installCertificateResult.CertificateName);
        WriteLine();

        if (installCertificateResult.Error == null)
        {
            WriteSuccess("Certificate successfully added in store.");
        }
        else
        {
            WriteError("Certificate could not be added in store.");
            WriteError(installCertificateResult.Error);
        }
    }
}