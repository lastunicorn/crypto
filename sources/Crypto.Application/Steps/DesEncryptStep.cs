using System.Security.Cryptography;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.Application.Steps;

public class DesEncryptStep : StepBase
{
    public override string Title => "DES (Encrypt)";

    public string Message { get; set; }

    public byte[] Key { get; set; }

    public byte[] IV { get; set; }

    public byte[] EncryptedMessage { get; private set; }

    public DesEncryptStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        Log.WriteValue("Original Message", Message);
        Log.WriteValue("Key", Key);
        Log.WriteValue("Initial Vector", IV);
        Log.WriteLine();

        if (Message == null || Message.Length <= 0) throw new ArgumentException(nameof(Message));
        if (Key == null || Key.Length <= 0) throw new ArgumentException(nameof(Key));
        if (IV == null || IV.Length <= 0) throw new ArgumentException(nameof(IV));

        using DES des = DES.Create();

        des.Key = Key;
        des.IV = IV;

        ICryptoTransform cryptoTransform = des.CreateEncryptor(des.Key, des.IV);

        using MemoryStream memoryStream = new();
        using (CryptoStream cryptoStream = new(memoryStream, cryptoTransform, CryptoStreamMode.Write))
        {
            using StreamWriter streamWriter = new(cryptoStream);
            streamWriter.Write(Message);
        }

        EncryptedMessage = memoryStream.ToArray();

        Log.WriteValue("Encrypted Message", EncryptedMessage);
    }
}