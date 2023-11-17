using System.Security.Cryptography;
using System.Text;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class Md5EncryptStep : StepBase
{
    public override string Title => "MD5 (Encrypt)";

    public string Message { get; set; }

    public byte[] EncryptedMessage { get; private set; }

    public Md5EncryptStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        Log.WriteValue("Original Message", Message);

        byte[] originalBytes = Encoding.UTF8.GetBytes(Message);

        using MD5 md5 = MD5.Create();
        EncryptedMessage = md5.ComputeHash(originalBytes);

        Log.WriteValue("Hash", EncryptedMessage);
    }
}