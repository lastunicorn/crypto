using System.Security.Cryptography;

namespace DustInTheWind.Crypto.Domain.SymetricEncryption;

public class DesEncryptedText
{
    public string DecryptedValue { get; }

    public byte[] EncryptedValue { get; }

    public DesEncryptedText(string text, byte[] key, byte[] iv)
    {
        DecryptedValue = text;
        EncryptedValue = Encrypt(text, key, iv);
    }

    public DesEncryptedText(byte[] bytes, byte[] key, byte[] iv)
    {
        DecryptedValue = Decrypt(bytes, key, iv);
        EncryptedValue = bytes;
    }

    private static byte[] Encrypt(string text, byte[] key, byte[] iv)
    {
        using DES des = DES.Create();

        des.Key = key;
        des.IV = iv;

        ICryptoTransform cryptoTransform = des.CreateEncryptor(des.Key, des.IV);

        using MemoryStream memoryStream = new();
        using CryptoStream cryptoStream = new(memoryStream, cryptoTransform, CryptoStreamMode.Write);
        using StreamWriter streamWriter = new(cryptoStream);

        streamWriter.Write(text);
        streamWriter.Close();

        return memoryStream.ToArray();
    }

    private static string Decrypt(byte[] bytes, byte[] key, byte[] iv)
    {
        using DES des = DES.Create();

        des.Key = key;
        des.IV = iv;

        ICryptoTransform cryptoTransform = des.CreateDecryptor(des.Key, des.IV);

        using MemoryStream memoryStream = new(bytes);
        using CryptoStream cryptoStream = new(memoryStream, cryptoTransform, CryptoStreamMode.Read);
        using StreamReader streamReader = new(cryptoStream);

        return streamReader.ReadToEnd();
    }
}