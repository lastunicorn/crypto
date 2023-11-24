using System.Security.Cryptography;

namespace DustInTheWind.Crypto.Application.EncryptionArea.SymmetricEncryption;

public class TripleDesEncryptedText
{
    public string DecryptedValue { get; }

    public byte[] EncryptedValue { get; }

    public TripleDesEncryptedText(string text, byte[] key, byte[] iv)
    {
        DecryptedValue = text;
        EncryptedValue = Encrypt(text, key, iv);
    }

    public TripleDesEncryptedText(byte[] bytes, byte[] key, byte[] iv)
    {
        DecryptedValue = Decrypt(bytes, key, iv);
        EncryptedValue = bytes;
    }

    private static byte[] Encrypt(string text, byte[] key, byte[] iv)
    {
        using TripleDES tripleDes = TripleDES.Create();

        tripleDes.Key = key;
        tripleDes.IV = iv;

        ICryptoTransform cryptoTransform = tripleDes.CreateEncryptor(tripleDes.Key, tripleDes.IV);

        using MemoryStream memoryStream = new();
        using CryptoStream cryptoStream = new(memoryStream, cryptoTransform, CryptoStreamMode.Write);
        using StreamWriter streamWriter = new(cryptoStream);

        streamWriter.Write(text);
        streamWriter.Close();

        return memoryStream.ToArray();
    }

    private static string Decrypt(byte[] bytes, byte[] key, byte[] iv)
    {
        using TripleDES tripleDes = TripleDES.Create();

        tripleDes.Key = key;
        tripleDes.IV = iv;

        ICryptoTransform cryptoTransform = tripleDes.CreateDecryptor(tripleDes.Key, tripleDes.IV);

        using MemoryStream memoryStream = new(bytes);
        using CryptoStream cryptoStream = new(memoryStream, cryptoTransform, CryptoStreamMode.Read);
        using StreamReader streamReader = new(cryptoStream);

        return streamReader.ReadToEnd();
    }
}