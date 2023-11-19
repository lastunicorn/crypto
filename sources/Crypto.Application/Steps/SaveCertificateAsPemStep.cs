using DustInTheWind.Crypto.Domain;
using DustInTheWind.Crypto.Domain.CertificateModel;
using DustInTheWind.Crypto.Ports.FileAccess;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class SaveCertificateAsPemStep : StepBase
{
    private readonly IFileSystem fileSystem;

    public override string Title => "Save Certificate in PEM format";

    public string FileName { get; set; }

    public GenericCertificate Certificate { get; set; }

    public SaveCertificateAsPemStep(ILog log, IFileSystem fileSystem)
        : base(log)
    {
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    protected override void DoExecute()
    {
        if (Certificate == null) throw new ArgumentNullException(nameof(Certificate));
        if (FileName == null) throw new ArgumentNullException(nameof(FileName));

        Log.WriteValue("Certificate Thumbprint", Certificate.Thumbprint);
        Log.WriteValue("File", FileName);

        PemDocument pemDocument = Certificate.ExportAsPem();
        fileSystem.SaveFile(FileName, pemDocument);

        Log.WriteLine();
        Log.WriteSuccess("Certificate exported successfully.");
    }
}