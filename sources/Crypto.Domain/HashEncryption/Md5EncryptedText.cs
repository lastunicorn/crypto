using System.Security.Cryptography;
using System.Text;

namespace DustInTheWind.Crypto.Domain.HashEncryption;

public class Md5EncryptedText
{
    private readonly byte[] encryptedMessage;
    
    public Md5EncryptedText(string text)
    {
        byte[] originalBytes = Encoding.UTF8.GetBytes(text);

        using MD5 md5 = MD5.Create();
        encryptedMessage = md5.ComputeHash(originalBytes);
    }

    public static implicit operator byte[](Md5EncryptedText encryptedText)
    {
        return encryptedText.encryptedMessage;
    }
}