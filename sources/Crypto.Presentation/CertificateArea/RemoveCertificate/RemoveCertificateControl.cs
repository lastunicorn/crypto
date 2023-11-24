using DustInTheWind.Crypto.Application.Sections;
using DustInTheWind.Crypto.Presentation.Infrastructure;

namespace DustInTheWind.Crypto.Presentation.CertificateArea.RemoveCertificate;

public class RemoveCertificateControl : SectionControlBase
{
    public string CertificateSubjectName { get; set; }

    public bool CertificateContainsPk { get; set; }

    public bool PkExists { get; set; }

    public string PkFilePath { get; set; }

    public string PkDirectoryPath { get; set; }

    public string PkLocationType { get; set; }

    public bool PkWasRemoved { get; set; }

    public Exception PkRemoveError { get; set; }

    public Exception Error { get; set; }

    public RemoveCertificateControl(CertificateRemovalResult removalResult)
    {
        Title = "Remove Certificate";
        CertificateSubjectName = removalResult.CertificateSubjectName;
        CertificateContainsPk = removalResult.CertificateContainsPrivateKey;
        PkExists = removalResult.PrivateKeyFileInfo != null;
        PkFilePath = removalResult.PrivateKeyFileInfo?.FilePath;
        PkDirectoryPath = removalResult.PrivateKeyFileInfo?.DirectoryPath;
        PkLocationType = removalResult.PrivateKeyFileInfo?.LocationType.ToString();
        PkWasRemoved = removalResult.PrivateKeyFileInfo?.IsSuccessfullyRemoved ?? false;
        PkRemoveError = removalResult.PrivateKeyFileInfo?.RemoveError;
        Error = removalResult.Error;
    }

    protected override void DoDisplay()
    {
        WriteValue("Subject Name", CertificateSubjectName);

        DisplayPrivateKeyInfo();
        DisplayPrivateKeyRemovalInfo();

        if (Error == null)
        {
            WriteLine();
            WriteSuccess("Certificate was successfully removed.");
        }
        else
        {
            WriteError(Error);
        }
    }

    private void DisplayPrivateKeyInfo()
    {
        if (!CertificateContainsPk)
        {
            WriteWarning("Certificate does not contain a private key.");
            return;
        }

        if (PkExists)
        {
            WithIndentation("Private key file was found:", () =>
            {
                WriteValue("Full Path", PkFilePath);
                WriteValue("Directory", PkDirectoryPath);
                WriteValue("Directory Type", PkLocationType.ToString());
            });
        }
        else
        {
            WriteWarning("Private key file was not found.");
        }
    }

    private void DisplayPrivateKeyRemovalInfo()
    {
        if (PkWasRemoved)
            WriteSuccess("Private key file was successfully deleted.");
        else
            WriteWarning("Private key file was not deleted.");

        if (PkRemoveError != null)
        {
            WriteError("Failed to delete certificate private key file.");
            WriteError(PkRemoveError);
        }
    }
}