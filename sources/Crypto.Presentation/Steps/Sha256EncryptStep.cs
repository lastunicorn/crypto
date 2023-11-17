using System.Security.Cryptography;
using System.Text;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class Sha256EncryptStep : StepBase
{
    public override string Title => "SHA256 (Encrypt)";

    public string Message { get; set; }

    public byte[] EncryptedMessage { get; private set; }

    public Sha256EncryptStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        Log.WriteValue("Original Message", Message);

        byte[] originalBytes = Encoding.UTF8.GetBytes(Message);

        using SHA256 sha256 = SHA256.Create();
        EncryptedMessage = sha256.ComputeHash(originalBytes);

        Log.WriteValue("Hash", EncryptedMessage);
    }
}