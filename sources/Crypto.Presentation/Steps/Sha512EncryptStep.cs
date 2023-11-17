using System.Security.Cryptography;
using System.Text;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class Sha512EncryptStep : StepBase
{
    public override string Title => "SHA512 (Encrypt)";

    public string Message { get; set; }

    public byte[] EncryptedMessage { get; private set; }

    public Sha512EncryptStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        Log.WriteValue("Original Message", Message);

        byte[] originalBytes = Encoding.UTF8.GetBytes(Message);

        using SHA512 sha512 = SHA512.Create();
        EncryptedMessage = sha512.ComputeHash(originalBytes);

        Log.WriteValue("Hash", EncryptedMessage);
    }
}