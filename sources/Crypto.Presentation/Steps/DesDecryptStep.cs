using System.Security.Cryptography;
using DustInTheWind.Crypto.Ports.LogAccess;

namespace DustInTheWind.Crypto.PresentationAndUseCases.Steps;

internal class DesDecryptStep : StepBase
{
    public override string Title => "DES (Decrypt)";

    public byte[] EncryptedMessage { get; set; }

    public byte[] Key { get; set; }

    public byte[] IV { get; set; }

    public string Message { get; private set; }

    public DesDecryptStep(ILog log)
        : base(log)
    {
    }

    protected override void DoExecute()
    {
        Log.WriteValue("Original Encrypted Message", EncryptedMessage);
        Log.WriteValue("Key", Key);
        Log.WriteValue("Initial Vector", IV);
        Log.WriteLine();

        if (EncryptedMessage == null || EncryptedMessage.Length <= 0) throw new ArgumentException(nameof(EncryptedMessage));
        if (Key == null || Key.Length <= 0) throw new ArgumentException(nameof(Key));
        if (IV == null || IV.Length <= 0) throw new ArgumentException(nameof(IV));

        using DES des = DES.Create();

        des.Key = Key;
        des.IV = IV;

        ICryptoTransform cryptoTransform = des.CreateDecryptor(des.Key, des.IV);

        using MemoryStream memoryStream = new(EncryptedMessage);
        using CryptoStream cryptoStream = new(memoryStream, cryptoTransform, CryptoStreamMode.Read);
        using StreamReader streamReader = new(cryptoStream);

        Message = streamReader.ReadToEnd();

        Log.WriteValue("Decrypted Message", Message);
    }
}