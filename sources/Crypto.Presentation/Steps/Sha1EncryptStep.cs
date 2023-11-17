using System.Security.Cryptography;
using System.Text;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class Sha1EncryptStep : StepBase
{
    public override string Title => "SHA1 (Encrypt)";

    public string Message { get; set; }

    public byte[] EncryptedMessage { get; private set; }

    public Sha1EncryptStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        Log.WriteValue("Original Message", Message);

        byte[] originalBytes = Encoding.UTF8.GetBytes(Message);

        using SHA1 sha1 = SHA1.Create();
        EncryptedMessage = sha1.ComputeHash(originalBytes);

        Log.WriteValue("Hash", EncryptedMessage);
    }
}