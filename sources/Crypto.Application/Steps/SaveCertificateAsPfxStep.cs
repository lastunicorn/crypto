using DustInTheWind.Crypto.Domain;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class SaveCertificateAsPfxStep : StepBase
{
    private readonly IFileSystem fileSystem;

    public override string Title => "Save Certificate in PFX format";

    public GenericCertificate Certificate { get; set; }

    public string Password { get; set; }

    public string FileName { get; set; }

    public SaveCertificateAsPfxStep(ILog log, IFileSystem fileSystem)
        : base(log)
    {
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    protected override void DoExecute()
    {
        try
        {
            Log.WriteValue("Certificate Thumbprint", Certificate.Thumbprint);
            Log.WriteValue("File Path", FileName);

            PfxDocument pfxDocument = Certificate.ExportAsPfx(Password);
            fileSystem.SaveFile(FileName, pfxDocument);
            
            Log.WriteValue("Certificate", pfxDocument.CertificateBytes);

            Log.WriteLine();
            Log.WriteSuccess("Certificate exported successfully.");
        }
        catch (Exception ex)
        {
            Log.WriteError(ex);
        }
    }
}