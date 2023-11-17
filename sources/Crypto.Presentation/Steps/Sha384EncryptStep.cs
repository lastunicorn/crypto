using System.Security.Cryptography;
using System.Text;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class Sha384EncryptStep : StepBase
{
    public override string Title => "SHA384 (Encrypt)";

    public string Message { get; set; }

    public byte[] EncryptedMessage { get; private set; }

    public Sha384EncryptStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        Log.WriteValue("Original Message", Message);

        byte[] originalBytes = Encoding.UTF8.GetBytes(Message);

        using SHA384 sha384 = SHA384.Create();
        EncryptedMessage = sha384.ComputeHash(originalBytes);

        Log.WriteValue("Hash", EncryptedMessage);
    }
}