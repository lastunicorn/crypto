using System.Security.Cryptography;
using System.Text;

namespace DustInTheWind.Crypto.Domain.HashEncryption;

public class Sha1EncryptedText
{
    private readonly byte[] encryptedMessage;

    public Sha1EncryptedText(string text)
    {
        byte[] originalBytes = Encoding.UTF8.GetBytes(text);

        using SHA1 sha1 = SHA1.Create();
        encryptedMessage = sha1.ComputeHash(originalBytes);
    }

    public static implicit operator byte[](Sha1EncryptedText encryptedText)
    {
        return encryptedText.encryptedMessage;
    }
}